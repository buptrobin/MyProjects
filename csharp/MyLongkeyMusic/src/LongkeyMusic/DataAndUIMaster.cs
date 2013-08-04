using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace LongkeyMusic
{
	[Serializable]
	internal class DataAndUIMaster : IObserver
	{
		public enum ListType
		{
			DOWNLOADING,
			FINISHED,
			ALL
		}
		private SQLManager _historyDB;
		private Dictionary<DataAndUIMaster.ListType, MetaDataListView> _albumLists = new Dictionary<DataAndUIMaster.ListType, MetaDataListView>();
		private Dictionary<DataAndUIMaster.ListType, SetIntHandler> _setTabNumHandlers = new Dictionary<DataAndUIMaster.ListType, SetIntHandler>();
		private object songSync = new object();
		private MetaDataListView _selectedSongList;
		private Dictionary<string, Dictionary<string, SongMeta>> _allSongs = new Dictionary<string, Dictionary<string, SongMeta>>();
		private RefreshAlbumDetail _refreshAlbumDetail;
		private SetAlbumHandler _showDownloadFinishNotification;
		private static DataAndUIMaster _instance;
		public AlbumMeta CurrentSelectingAlbum
		{
			get;
			set;
		}
		public static DataAndUIMaster Instance
		{
			get
			{
				if (DataAndUIMaster._instance == null)
				{
					DataAndUIMaster._instance = new DataAndUIMaster();
				}
				return DataAndUIMaster._instance;
			}
		}
		private DataAndUIMaster()
		{
			this._historyDB = SQLManager.Instance;
		}
		public void SetAlbumList(Dictionary<DataAndUIMaster.ListType, ObjectListView> albumLists)
		{
			foreach (KeyValuePair<DataAndUIMaster.ListType, ObjectListView> current in albumLists)
			{
				this._albumLists.Add(current.Key, new MetaDataListView(current.Value));
			}
		}
		public void SetSetTabNumHandlers(Dictionary<DataAndUIMaster.ListType, SetIntHandler> handlers)
		{
			this._setTabNumHandlers = handlers;
		}
		public void SetSelectedSongList(ObjectListView songList)
		{
			this._selectedSongList = new MetaDataListView(songList);
		}
		public void SetRefreshUIDelegate(RefreshAlbumDetail refreshAlbumDetail)
		{
			this._refreshAlbumDetail = refreshAlbumDetail;
		}
		public void SetDownloadFinishNotification(SetAlbumHandler showDownloadFinishNotification)
		{
			this._showDownloadFinishNotification = showDownloadFinishNotification;
		}
		public void Notify(object obj)
		{
			if (obj is AlbumMeta)
			{
				AlbumMeta albumMeta = (AlbumMeta)obj;
				if (albumMeta.DownloadStatus == AlbumMeta.Status.New)
				{
					this.AddAlbum(albumMeta.AlbumKey, albumMeta);
				}
				else
				{
					if (albumMeta.DownloadStatus == AlbumMeta.Status.Deleted)
					{
						this.RemoveAlbum(albumMeta.AlbumKey);
					}
					else
					{
						this.SetAlbum(albumMeta.AlbumKey, albumMeta);
					}
				}
			}
			if (obj is SongMeta)
			{
				SongMeta songMeta = (SongMeta)obj;
				this.SetSong(songMeta.AlbumKey, songMeta.SongId, songMeta);
			}
		}
		public void LoadHistory()
		{
			Dictionary<string, AlbumDBMeta> dictionary = this._historyDB.LoadAlbums();
			foreach (AlbumDBMeta current in dictionary.Values)
			{
				AlbumMeta albumMeta = AlbumFactory.Instance.CreateNewAlbum(current);
				this.AddAlbum(albumMeta.AlbumKey, albumMeta);
				if (current.songs != null)
				{
					foreach (SongDBMeta current2 in current.songs.Values)
					{
						SongMeta songMeta = SongFactory.Instance.CreateNewSong(current2);
						if (albumMeta.DownloadStatus == AlbumMeta.Status.Finished)
						{
							songMeta.DownloadStatus = SongMeta.Status.COMPLETED;
						}
						this.AddSong(current2.albumKey, current2.songId, songMeta);
					}
				}
				albumMeta.Refresh();
			}
		}
		public void AddAlbum(string key, AlbumMeta album)
		{
			this._albumLists[DataAndUIMaster.ListType.ALL].AddObject(key, album);
			if (album.DownloadStatus == AlbumMeta.Status.Finished)
			{
				this._albumLists[DataAndUIMaster.ListType.FINISHED].AddObject(key, album);
			}
			else
			{
				this._albumLists[DataAndUIMaster.ListType.DOWNLOADING].AddObject(key, album);
			}
			this._historyDB.AddAlbum(album.AlbumDBData);
		}
		public void SetAlbum(string key, AlbumMeta album)
		{
			AlbumMeta albumMeta = (AlbumMeta)this._albumLists[DataAndUIMaster.ListType.DOWNLOADING].GetObject(key);
			AlbumMeta albumMeta2 = (AlbumMeta)this._albumLists[DataAndUIMaster.ListType.FINISHED].GetObject(key);
			this._albumLists[DataAndUIMaster.ListType.ALL].SetObject(key, album);
			if (albumMeta != null)
			{
				this._albumLists[DataAndUIMaster.ListType.DOWNLOADING].SetObject(key, album);
				if (album.DownloadStatus == AlbumMeta.Status.Finished)
				{
					this._albumLists[DataAndUIMaster.ListType.DOWNLOADING].RemoveObject(key);
					this._albumLists[DataAndUIMaster.ListType.FINISHED].AddObject(key, album);
					this._historyDB.UpdateAlbum(album.AlbumDBData);
					this._showDownloadFinishNotification(album);
				}
			}
			if (albumMeta2 != null)
			{
				this._albumLists[DataAndUIMaster.ListType.FINISHED].SetObject(key, album);
			}
			if (this.CurrentSelectingAlbum != null && this.CurrentSelectingAlbum.AlbumKey == album.AlbumKey && album.IsUIRefreshNeed)
			{
				this._refreshAlbumDetail(album);
				if (album.DownloadStatus == AlbumMeta.Status.GotMeta)
				{
					this.ResetSongs(album.AlbumKey);
				}
				album.IsUIRefreshNeed = false;
			}
		}
		public void RemoveAlbum(string key)
		{
			foreach (KeyValuePair<DataAndUIMaster.ListType, MetaDataListView> current in this._albumLists)
			{
				current.Value.RemoveObject(key);
			}
			object obj;
			Monitor.Enter(obj = this.songSync);
			try
			{
				this._allSongs.Remove(key);
			}
			finally
			{
				Monitor.Exit(obj);
			}
			this._historyDB.RemoveAlbum(key);
		}
		public void AddSong(string albumKey, string songKey, SongMeta song)
		{
			object obj;
			Monitor.Enter(obj = this.songSync);
			try
			{
				if (this._allSongs.ContainsKey(albumKey))
				{
					if (!this._allSongs[albumKey].ContainsKey(songKey))
					{
						this._allSongs[albumKey].Add(songKey, song);
					}
				}
				else
				{
					Dictionary<string, SongMeta> dictionary = new Dictionary<string, SongMeta>();
					dictionary.Add(songKey, song);
					this._allSongs.Add(albumKey, dictionary);
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public void SetSong(string albumKey, string songKey, SongMeta song)
		{
			object obj;
			Monitor.Enter(obj = this.songSync);
			try
			{
				if (this._allSongs.ContainsKey(albumKey) && this._allSongs[albumKey].ContainsKey(songKey))
				{
					this._allSongs[albumKey][songKey] = song;
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
			this._selectedSongList.SetObject(songKey, song);
		}
		public List<SongMeta> GetAlbumSongs(string albumKey)
		{
			object obj;
			Monitor.Enter(obj = this.songSync);
			List<SongMeta> result;
			try
			{
				result = this._allSongs[albumKey].Values.ToList<SongMeta>();
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return result;
		}
		public void ResetSongs(string albumKey)
		{
			object obj;
			Monitor.Enter(obj = this.songSync);
			try
			{
				if (this._allSongs.ContainsKey(albumKey))
				{
					Dictionary<string, ObservableMeta> dictionary = new Dictionary<string, ObservableMeta>();
					foreach (SongMeta current in this._allSongs[albumKey].Values)
					{
						dictionary.Add(current.SongId, current);
					}
					this._selectedSongList.ResetObjects(dictionary);
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public AlbumMeta GetAlbum(string key)
		{
			return (AlbumMeta)this._albumLists[DataAndUIMaster.ListType.ALL].GetObject(key);
		}
	}
}
