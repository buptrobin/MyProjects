using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
namespace LongkeyMusic
{
	public class SongDownloader
	{
		private string _fileName;
		private string _tmpFile;
		private Stopwatch _sw = new Stopwatch();
		private WebClient _client;
		private XmlDocument _songXmlDoc;
		private MP3Lyris _lyris;
		private SongMeta _songMeta;
		private bool _isDelete;
		private int _retryTimes;
		private int _maxRetryTimes = 5;
		private Bitmap _songImage;
		private object fileLock = new object();
		private static Dictionary<string, string> _invalidChars = new Dictionary<string, string>
		{

			{
				"&",
				"&amp;"
			}
		};
		public SongMeta.Status Status
		{
			get
			{
				return this._songMeta.DownloadStatus;
			}
		}
		public long Speed
		{
			get
			{
				return this._songMeta.Speed;
			}
		}
		public bool IsActive
		{
			get
			{
				return this._client.IsBusy;
			}
		}
		public SongDownloader(SongMeta songMeta)
		{
			this._songMeta = songMeta;
			this._client = new WebClient();
			this._client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.DownloadChanged);
			this._client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.DownloadComplete);
			this._fileName = this._songMeta.Location + ".mp3";
			this._tmpFile = this._songMeta.Location + ".longkey";
		}
		private void AddMP3Tag(string fileName)
		{
			MP3Editor mP3Editor = new MP3Editor(fileName);
			mP3Editor.SetSinger(this._songMeta.Artist);
			mP3Editor.SetAlbum(this._songMeta.Album);
			mP3Editor.SetTitle(this._songMeta.Song);
			if (this._songMeta.Track > 0)
			{
				mP3Editor.SetTrack(this._songMeta.Track);
			}
			if (this._songMeta.IsFromCollection)
			{
				mP3Editor.SetAlubmArtist("群星");
			}
			if (this._songImage == null)
			{
				AlbumMeta album = DataAndUIMaster.Instance.GetAlbum(this._songMeta.AlbumKey);
				mP3Editor.SetCover(album.Cover);
			}
			else
			{
				mP3Editor.SetCover(this._songImage);
			}
			mP3Editor.SetYear(this._songMeta.Year);
			mP3Editor.SetLyris(this._lyris);
			mP3Editor.SetUnSyncLyris(this._lyris);
			mP3Editor.Commit();
		}
		private void DownloadComplete(object sender, AsyncCompletedEventArgs e)
		{
			this._songMeta.Speed = 0L;
			bool arg_18_0 = this._client.IsBusy;
			if (e.Cancelled)
			{
				this._songMeta.DownloadStatus = SongMeta.Status.STOPPED;
				this.UpdateStatus();
				this._client.Dispose();
				if (this._isDelete)
				{
					this._songMeta.SongDBMeta.DeleteFiles();
					return;
				}
			}
			else
			{
				if (e != null && e.Error != null)
				{
					this.Retry();
					return;
				}
				object obj;
				Monitor.Enter(obj = this.fileLock);
				try
				{
					this.AddMP3Tag(this._tmpFile);
					File.Move(this._tmpFile, this._fileName);
					this._songMeta.DownloadStatus = SongMeta.Status.COMPLETED;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Create MP3 EXCEPTION: " + ex.Message);
					this._songMeta.DownloadStatus = SongMeta.Status.FAILED;
				}
				finally
				{
					Monitor.Exit(obj);
				}
				this._sw.Stop();
				this.UpdateStatus();
				this._client.Dispose();
			}
		}
		private void UpdateStatus()
		{
			this._songMeta.Refresh();
		}
		private void DownloadChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this._songMeta.Progress = e.ProgressPercentage;
			this._songMeta.FileSize = e.TotalBytesToReceive;
			if (this._sw.Elapsed.TotalSeconds != 0.0)
			{
				long speed = (long)((double)e.BytesReceived / this._sw.Elapsed.TotalSeconds);
				this._songMeta.Speed = speed;
			}
			this.UpdateStatus();
		}
		public void StopDownload()
		{
			this._client.CancelAsync();
		}
		public void StartDownload()
		{
			this._sw.Start();
			this._songMeta.DownloadStatus = SongMeta.Status.DOWNLOADING;
			if (File.Exists(this._fileName))
			{
				this._songMeta.DownloadStatus = SongMeta.Status.COMPLETED;
				this._songMeta.Progress = 100;
				return;
			}
			if (!Directory.Exists(Path.GetDirectoryName(this._tmpFile)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this._tmpFile));
			}
			string url = "http://www.xiami.com/song/playlist/id/" + this._songMeta.SongId + "/object_name/default/object_id/0";
			this._songXmlDoc = new XmlDocument();
			try
			{
				string text = this.WGet(url);
				foreach (KeyValuePair<string, string> current in SongDownloader._invalidChars)
				{
					text = text.Replace(current.Key, current.Value);
				}
				this._songXmlDoc.LoadXml(text);
				this.ReadLyrics();
				this.ReadImage();
				string uriString = this.DecodeDownloadPath();
				this._client.DownloadFileAsync(new Uri(uriString), this._tmpFile);
			}
			catch (Exception)
			{
				this.Retry();
			}
		}
		private void Retry()
		{
			if (this._retryTimes < this._maxRetryTimes)
			{
				this._retryTimes++;
				this._songMeta.DownloadStatus = SongMeta.Status.RETRYING;
				this.UpdateStatus();
				return;
			}
			this._songMeta.DownloadStatus = SongMeta.Status.FAILED;
			this.UpdateStatus();
			this._client.Dispose();
		}
		private string WGet(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string result = streamReader.ReadToEnd();
			httpWebResponse.Close();
			streamReader.Close();
			return result;
		}
		private void ReadLyrics()
		{
			XmlNodeList elementsByTagName = this._songXmlDoc.GetElementsByTagName("lyric");
			if (elementsByTagName.Count > 0 && elementsByTagName[0].InnerText.Trim().Length > 0)
			{
				string url = elementsByTagName[0].InnerText.Trim();
				this._lyris = new MP3Lyris();
				string text = this.WGet(url).Replace("\r\n", "\n");
				string[] array = text.Split(new char[]
				{
					'\n'
				});
				Regex regex = new Regex("\\[(\\d+):(\\d+\\.\\d+)\\]");
				for (int i = 0; i < array.Length; i++)
				{
					Match match = regex.Match(array[i]);
					string lyric = "";
					List<int> list = new List<int>();
					int num = -1;
					while (match.Success)
					{
						string value = match.Groups[1].Value;
						string value2 = match.Groups[2].Value;
						int item = (int)((double.Parse(value) * 60.0 + double.Parse(value2)) * 1000.0);
						list.Add(item);
						num = match.Index + match.Groups[0].Value.Length;
						match = match.NextMatch();
					}
					if (num != -1 && num != array[i].Length)
					{
						lyric = array[i].Substring(num);
					}
					foreach (int current in list)
					{
						this._lyris.Add(current, lyric);
					}
				}
			}
		}
		private void ReadImage()
		{
			try
			{
				string innerText = this._songXmlDoc.GetElementsByTagName("pic")[0].InnerText;
				string text = innerText.Substring(innerText.LastIndexOf('.'));
				string text2 = innerText.Replace("_1" + text, "");
				string[] array = new string[]
				{
					text,
					"_2" + text,
					"_1" + text
				};
				if (text2.Length > 0 && this._songMeta.IsFromCollection)
				{
					for (int i = 0; i < array.Length; i++)
					{
						string requestUriString = text2 + array[i];
						bool flag = false;
						HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
						HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
						if (httpWebResponse.ContentType.ToLower().StartsWith("image/"))
						{
							Stream responseStream = httpWebResponse.GetResponseStream();
							this._songImage = new Bitmap(responseStream);
							responseStream.Close();
							flag = true;
						}
						httpWebResponse.Close();
						if (flag)
						{
							break;
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
		private string DecodeDownloadPath()
		{
			string innerText = this._songXmlDoc.GetElementsByTagName("location")[0].InnerText;
			int num = int.Parse(innerText.Substring(0, 1));
			Console.WriteLine("a2:" + num);
			string text = innerText.Substring(1);
			int num2 = (int)Math.Floor((double)text.Length / (double)num);
			int num3 = text.Length % num;
			List<string> list = new List<string>();
			while (list.Count < num3)
			{
				list.Add("");
			}
			int i;
			for (i = 0; i < num3; i++)
			{
				list[i] = text.Substring((num2 + 1) * i, num2 + 1);
			}
			i = num3;
			while (list.Count < num)
			{
				list.Add("");
			}
			while (i < num)
			{
				list[i] = text.Substring(num2 * (i - num3) + (num2 + 1) * num3, num2);
				i++;
			}
			string text2 = "";
			for (i = 0; i < list[0].Length; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (i < list[j].Length)
					{
						text2 += list[j].Substring(i, 1);
					}
				}
			}
			text2 = Uri.UnescapeDataString(text2);
			string text3 = "";
			for (i = 0; i < text2.Length; i++)
			{
				if (text2.Substring(i, 1) == "^")
				{
					text3 += "0";
				}
				else
				{
					text3 += text2.Substring(i, 1);
				}
			}
			return text3.Replace('+', ' ');
		}
	}
}
