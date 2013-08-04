using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
namespace LongkeyMusic
{
	public class UpdateManager
	{
		[Serializable]
		private class ApplicationMeta
		{
			public DateTime LastCheckTime;
			public string LastVersion = "0";
		}
		private string _serverBase = "http://www.longkeymusic.com/longkeymusic_services/";
		private string _secret = "I_HAVE_A_DREAM";
		private static string _appMetaFile = Util.GetLocalDir() + "/longkeymusic.app";
		private static UpdateManager.ApplicationMeta _appMeta;
		private static UpdateManager _instance;
		public static UpdateManager Instance
		{
			get
			{
				if (UpdateManager._instance == null)
				{
					UpdateManager._instance = new UpdateManager();
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						Stream stream = new FileStream(UpdateManager._appMetaFile, FileMode.Open, FileAccess.Read, FileShare.Read);
						UpdateManager._appMeta = (UpdateManager.ApplicationMeta)binaryFormatter.Deserialize(stream);
						stream.Close();
					}
					catch (Exception)
					{
						UpdateManager._appMeta = new UpdateManager.ApplicationMeta();
					}
				}
				return UpdateManager._instance;
			}
		}
		private UpdateManager()
		{
		}
		public void PingServer(SetTwoMessageHandler setMessage)
		{
			HttpPostHelper httpPostHelper = new HttpPostHelper(this._serverBase + "ping.php");
			httpPostHelper.Add("SECRET", this._secret);
			httpPostHelper.Add("version", Application.ProductVersion);
			if (UpdateManager._appMeta.LastVersion == "0")
			{
				httpPostHelper.Add("isNewUser", "true");
			}
			else
			{
				httpPostHelper.Add("isNewUser", "false");
			}
			if (Application.ProductVersion.CompareTo(UpdateManager._appMeta.LastVersion) > 0)
			{
				httpPostHelper.Add("isUpdateUser", "true");
			}
			else
			{
				httpPostHelper.Add("isUpdateUser", "false");
			}
			try
			{
				XmlDocument result = httpPostHelper.GetResult();
				if (result.GetElementsByTagName("ERROR").Count == 0)
				{
					UpdateManager._appMeta.LastVersion = Application.ProductVersion;
					UpdateManager._appMeta.LastCheckTime = DateTime.Now;
					IFormatter formatter = new BinaryFormatter();
					Stream stream = new FileStream(UpdateManager._appMetaFile, FileMode.Create, FileAccess.Write, FileShare.None);
					formatter.Serialize(stream, UpdateManager._appMeta);
					stream.Close();
					string text = string.Empty;
					string text2 = (result.GetElementsByTagName("VERSION").Count > 0) ? result.GetElementsByTagName("VERSION")[0].InnerText : "0";
					string msg = (result.GetElementsByTagName("DOWNLOADURL").Count > 0) ? result.GetElementsByTagName("DOWNLOADURL")[0].InnerText : string.Empty;
					string text3 = (result.GetElementsByTagName("POPMESSAGE").Count > 0) ? result.GetElementsByTagName("POPMESSAGE")[0].InnerText : string.Empty;
					if (text2.CompareTo(Application.ProductVersion) > 0)
					{
						text = text + "LongkeyMusic有新版本" + text2 + "下载啦！！！";
					}
					if (text3 != string.Empty)
					{
						text += text3;
					}
					setMessage(text, msg);
				}
			}
			catch (Exception)
			{
			}
		}
		public void UploadAlbumStat(AlbumMeta album)
		{
			HttpPostHelper httpPostHelper = new HttpPostHelper(this._serverBase + "upload_album.php");
			httpPostHelper.Add("SECRET", this._secret);
			httpPostHelper.Add("albumKey", album.AlbumKey);
			httpPostHelper.Add("album", album.Album);
			httpPostHelper.Add("artist", album.Artist);
			httpPostHelper.Add("songs", album.Songs.Count.ToString());
			try
			{
				httpPostHelper.GetResult();
			}
			catch (Exception)
			{
			}
		}
	}
}
