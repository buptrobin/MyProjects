using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
namespace LongkeyMusic
{
	internal class AlbumMetaDownloader
	{
		private AlbumMeta collection;
		private string _defaultImgUrl = "http://img.xiami.com/res/img/default/cd185.gif";
		public AlbumMetaDownloader(AlbumMeta album)
		{
			this.collection = album;
		}
		public void FetchAlbumMeta()
		{
			if (Util.IsValidAlbumUrl(this.collection.AlbumKey) == Util.AlbumType.Unrecognize)
			{
				return;
			}
			WebClient webClient = new WebClient();
			webClient.Encoding = Encoding.UTF8;
			webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.loadPageComplete);
			webClient.DownloadStringAsync(new Uri(this.collection.AlbumKey));
		}
		private void GetCoverImageAsyn(string imgURL)
		{
			WebClient webClient = new WebClient();
			webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.coverDownloader_DownloadDataCompleted);
			webClient.DownloadDataAsync(new Uri(imgURL));
		}
		private void coverDownloader_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			this.collection.Cover = (Bitmap)Image.FromStream(new MemoryStream(e.Result));
			this.collection.DownloadStatus = AlbumMeta.Status.ReadyToDownload;
			this.collection.IsUIRefreshNeed = true;
			this.collection.Refresh();
		}
		private void SetSongLocation(ref HashSet<string> songsInAlbum, ref SongDBMeta song)
		{
			if (!songsInAlbum.Contains(song.song))
			{
				song.location = Path.Combine(Util.GetAlbumDir(this.collection), Util.CorrectFile(song.song));
			}
			else
			{
				song.location = Path.Combine(Util.GetAlbumDir(this.collection), Util.CorrectFile(song.song + "_" + song.songId));
			}
			songsInAlbum.Add(song.song);
		}
		private void GetSongCollectionFromAlbum(string htmlText)
		{
			this.collection.Songs = new Dictionary<string, SongDBMeta>();
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlText);
			HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//td[@class='song_name']");
			this.collection.Tracks = htmlNodeCollection.Count;
			this.collection.Album = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='title']").SelectSingleNode("./h1").FirstChild.InnerText;
			this.collection.Artist = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='album_info']").SelectSingleNode("//table/tr/td/a").InnerText;
			Regex regex = new Regex("([0-9]{4})年[0-9]{2}月[0-9]{2}日");
			Match match = regex.Match(htmlText);
			if (match.Success)
			{
				this.collection.Year = short.Parse(match.Groups[1].Value);
			}
			string imgURL;
			try
			{
				imgURL = htmlDocument.DocumentNode.SelectSingleNode("//a[@id='cover_lightbox']").Attributes["href"].Value;
			}
			catch (Exception)
			{
				imgURL = this._defaultImgUrl;
			}
			short num = 1;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)htmlNodeCollection)
			{
				SongDBMeta songDBMeta = default(SongDBMeta);
				songDBMeta.song = current.SelectSingleNode("a").InnerText;
				songDBMeta.songId = current.SelectSingleNode("a").Attributes["href"].Value.Substring(6);
				songDBMeta.artist = this.collection.Artist;
				songDBMeta.albumKey = this.collection.AlbumKey;
				songDBMeta.album = this.collection.Album;
				songDBMeta.track = num;
				songDBMeta.year = this.collection.Year;
				this.SetSongLocation(ref hashSet, ref songDBMeta);
				songDBMeta.downloadStatus = SongMeta.Status.NOT_START;
				num += 1;
				this.collection.Songs.Add(songDBMeta.songId, songDBMeta);
				DataAndUIMaster.Instance.AddSong(this.collection.AlbumKey, songDBMeta.songId, SongFactory.Instance.CreateNewSong(songDBMeta));
			}
			this.collection.DownloadStatus = AlbumMeta.Status.GotMeta;
			this.collection.IsUIRefreshNeed = true;
			this.collection.IsCollection = false;
			this.collection.Refresh();
			this.GetCoverImageAsyn(imgURL);
		}
		private void GetSongCollectionFromCollection(string htmlText)
		{
			this.collection.Songs = new Dictionary<string, SongDBMeta>();
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlText);
			this.collection.Album = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='info_collect_main']").SelectSingleNode("h2").InnerText;
			string imgURL;
			try
			{
				imgURL = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='info_collect']/p/span/a").Attributes["href"].Value;
			}
			catch (Exception)
			{
				imgURL = this._defaultImgUrl;
			}
			this.collection.Artist = "精选集";
			HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//span[@class='song_name']");
			this.collection.Tracks = htmlNodeCollection.Count;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)htmlNodeCollection)
			{
				SongDBMeta songDBMeta = default(SongDBMeta);
				songDBMeta.album = this.collection.Album;
				songDBMeta.albumKey = this.collection.AlbumKey;
				songDBMeta.isFromCollection = true;
				HtmlNodeCollection htmlNodeCollection2 = current.SelectNodes("a");
                if(htmlNodeCollection2.Count<2) continue;

				songDBMeta.song = htmlNodeCollection2[0].Attributes["title"].Value;
				string value = htmlNodeCollection2[0].Attributes["href"].Value;
				songDBMeta.songId = value.Substring(6);
				songDBMeta.artist = htmlNodeCollection2[1].InnerText;
				songDBMeta.downloadStatus = SongMeta.Status.NOT_START;
				this.SetSongLocation(ref hashSet, ref songDBMeta);
				this.collection.Songs.Add(songDBMeta.songId, songDBMeta);
				DataAndUIMaster.Instance.AddSong(this.collection.AlbumKey, songDBMeta.songId, SongFactory.Instance.CreateNewSong(songDBMeta));
			}
			this.collection.DownloadStatus = AlbumMeta.Status.GotMeta;
			this.collection.IsUIRefreshNeed = true;
			this.collection.IsCollection = true;
			this.collection.Refresh();
			this.GetCoverImageAsyn(imgURL);
		}
		private void GetSongCollectionFromTopSelection(string htmlText)
		{
			this.collection.Songs = new Dictionary<string, SongDBMeta>();
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlText);
			HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//td[@class='song_name']");
			this.collection.Tracks = htmlNodeCollection.Count;
			this.collection.Album = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='artist_trends']/h3").InnerText;
			this.collection.Artist = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='title']/h1").ChildNodes[0].InnerText;
			string imgURL;
			try
			{
				imgURL = htmlDocument.DocumentNode.SelectSingleNode("//a[@id='cover_lightbox']").Attributes["href"].Value;
			}
			catch (Exception)
			{
				imgURL = this._defaultImgUrl;
			}
			short num = 1;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)htmlNodeCollection)
			{
				SongDBMeta songDBMeta = default(SongDBMeta);
				songDBMeta.song = current.SelectSingleNode("a").InnerText;
				songDBMeta.songId = current.SelectSingleNode("a").Attributes["href"].Value.Substring(6);
				songDBMeta.artist = this.collection.Artist;
				songDBMeta.albumKey = this.collection.AlbumKey;
				songDBMeta.album = this.collection.Album;
				songDBMeta.track = num;
				this.SetSongLocation(ref hashSet, ref songDBMeta);
				songDBMeta.downloadStatus = SongMeta.Status.NOT_START;
				num += 1;
				this.collection.Songs.Add(songDBMeta.songId, songDBMeta);
				DataAndUIMaster.Instance.AddSong(this.collection.AlbumKey, songDBMeta.songId, SongFactory.Instance.CreateNewSong(songDBMeta));
			}
			this.collection.DownloadStatus = AlbumMeta.Status.GotMeta;
			this.collection.IsUIRefreshNeed = true;
			this.collection.IsCollection = true;
			this.collection.Refresh();
			this.GetCoverImageAsyn(imgURL);
		}
		private void loadPageComplete(object sender, DownloadStringCompletedEventArgs e)
		{
			if (e != null && e.Error == null && !e.Cancelled)
			{
				string result = e.Result;
				switch (Util.IsValidAlbumUrl(this.collection.AlbumKey))
				{
				case Util.AlbumType.Ablum:
					this.GetSongCollectionFromAlbum(result);
					return;
				case Util.AlbumType.Collection:
					this.GetSongCollectionFromCollection(result);
					return;
				case Util.AlbumType.Hot:
					this.GetSongCollectionFromTopSelection(result);
					break;
				default:
					return;
				}
			}
		}
	}
}
