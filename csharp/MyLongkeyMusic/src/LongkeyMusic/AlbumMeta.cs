using System;
using System.Collections.Generic;
using System.Drawing;
namespace LongkeyMusic
{
	public class AlbumMeta : ObservableMeta
	{
		public enum Status
		{
			Default,
			New,
			GotMeta,
			ReadyToDownload,
			Waiting,
			Downloading,
			Stopping,
			Stopped,
			Finished,
			Error,
			Deleted
		}
		private Bitmap _cover;
		private Dictionary<string, SongMeta> _songs;
		private AlbumDBMeta _albumDBMeta;
		public AlbumDBMeta AlbumDBData
		{
			get
			{
				return this._albumDBMeta;
			}
		}
		public string BaseFolder
		{
			get
			{
				return this._albumDBMeta.baseFolder;
			}
			set
			{
				if (this._albumDBMeta.baseFolder == null)
				{
					this._albumDBMeta.baseFolder = value;
				}
			}
		}
		public DateTime StartTime
		{
			get
			{
				return this._albumDBMeta.startTime;
			}
			set
			{
				if (this._albumDBMeta.startTime.Year == 1)
				{
					this._albumDBMeta.startTime = value;
				}
			}
		}
		public DateTime FinishTime
		{
			get
			{
				return this._albumDBMeta.finishTime;
			}
			set
			{
				if (this._albumDBMeta.finishTime.Year == 1)
				{
					this._albumDBMeta.finishTime = value;
				}
			}
		}
		public string Album
		{
			get
			{
				return this._albumDBMeta.album;
			}
			set
			{
				if (this._albumDBMeta.album == null || this._albumDBMeta.album == this._albumDBMeta.albumKey)
				{
					this._albumDBMeta.album = value;
				}
			}
		}
		public Bitmap Cover
		{
			get
			{
				return this._cover;
			}
			set
			{
				if (this._cover == null && value != null)
				{
					this._cover = value;
					this._albumDBMeta.coverThumbnail = this._cover.GetThumbnailImage(100, 100, new Image.GetThumbnailImageAbort(this.ThumbnailCallback), IntPtr.Zero);
				}
			}
		}
		public Image CoverThumnail
		{
			get
			{
				return this._albumDBMeta.coverThumbnail;
			}
			set
			{
				if (value != null && this._albumDBMeta.coverThumbnail == null)
				{
					this._albumDBMeta.coverThumbnail = value;
				}
			}
		}
		public string Artist
		{
			get
			{
				return this._albumDBMeta.artist;
			}
			set
			{
				if (this._albumDBMeta.artist == null)
				{
					this._albumDBMeta.artist = value;
				}
			}
		}
		public short Year
		{
			get
			{
				return this._albumDBMeta.year;
			}
			set
			{
				if (this._albumDBMeta.year == 0)
				{
					this._albumDBMeta.year = value;
				}
			}
		}
		public int Tracks
		{
			get
			{
				return this._albumDBMeta.tracks;
			}
			set
			{
				if (this._albumDBMeta.tracks == 0)
				{
					this._albumDBMeta.tracks = value;
				}
			}
		}
		public Dictionary<string, SongDBMeta> Songs
		{
			get
			{
				return this._albumDBMeta.songs;
			}
			set
			{
				if (this._albumDBMeta.songs == null)
				{
					this._albumDBMeta.songs = value;
				}
			}
		}
		public string AlbumKey
		{
			get
			{
				return this._albumDBMeta.albumKey;
			}
			set
			{
				if (this._albumDBMeta.albumKey == null)
				{
					this._albumDBMeta.albumKey = value;
				}
			}
		}
		public bool IsCollection
		{
			get
			{
				return this._albumDBMeta.isCollection;
			}
			set
			{
				this._albumDBMeta.isCollection = value;
			}
		}
		public int FinishedTracks
		{
			get
			{
				return this._albumDBMeta.finishedTracks;
			}
			set
			{
				this._albumDBMeta.finishedTracks = value;
			}
		}
		public long Speed
		{
			get;
			set;
		}
		public string Progress
		{
			get
			{
				if (this.Tracks == 0)
				{
					return "- / -";
				}
				return string.Format("{0} / {1}", this.FinishedTracks, this.Tracks);
			}
		}
		public AlbumMeta.Status DownloadStatus
		{
			get
			{
				return this._albumDBMeta.downloadStatus;
			}
			set
			{
				if (value != AlbumMeta.Status.Default)
				{
					this._albumDBMeta.downloadStatus = value;
				}
			}
		}
		public bool IsUIRefreshNeed
		{
			get;
			set;
		}
		public AlbumMeta()
		{
			this._albumDBMeta = default(AlbumDBMeta);
		}
		public AlbumMeta(AlbumDBMeta albumDBMeta)
		{
			this._albumDBMeta = albumDBMeta;
		}
		public override int GetHashCode()
		{
			return this.AlbumKey.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			return this.GetHashCode() == obj.GetHashCode();
		}
		public override void MergeFrom(ObservableMeta obj)
		{
			AlbumMeta albumMeta = (AlbumMeta)obj;
			this.Album = albumMeta.Album;
			this.AlbumKey = albumMeta.AlbumKey;
			this.Artist = albumMeta.Artist;
			this.Cover = albumMeta.Cover;
			this.CoverThumnail = albumMeta.CoverThumnail;
			this.Tracks = albumMeta.Tracks;
			this.Year = albumMeta.Year;
			this.Speed = albumMeta.Speed;
			this.Songs = albumMeta.Songs;
			this.FinishedTracks = albumMeta.FinishedTracks;
			this.DownloadStatus = albumMeta.DownloadStatus;
			this.StartTime = albumMeta.StartTime;
			this.FinishTime = albumMeta.FinishTime;
			this.BaseFolder = albumMeta.BaseFolder;
		}
		public void Refresh()
		{
			base.NotifyObservers(this);
		}
		public bool ThumbnailCallback()
		{
			return true;
		}
	}
}
