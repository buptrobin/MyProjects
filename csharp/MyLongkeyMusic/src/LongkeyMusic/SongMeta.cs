using System;
namespace LongkeyMusic
{
	public class SongMeta : ObservableMeta
	{
		public enum Status
		{
			DEFAULT,
			NOT_START,
			DOWNLOADING,
			COMPLETED,
			FAILED,
			RETRYING,
			STOPPED
		}
		private SongDBMeta _songDBMeta;
		public SongMeta.Status DownloadStatus
		{
			get
			{
				return this._songDBMeta.downloadStatus;
			}
			set
			{
				if (value != SongMeta.Status.DEFAULT)
				{
					this._songDBMeta.downloadStatus = value;
				}
			}
		}
		public string Song
		{
			get
			{
				return this._songDBMeta.song;
			}
			set
			{
				if (this._songDBMeta.song == null)
				{
					this._songDBMeta.song = value;
				}
			}
		}
		public string Album
		{
			get
			{
				return this._songDBMeta.album;
			}
			set
			{
				if (this._songDBMeta.album == null)
				{
					this._songDBMeta.album = value;
				}
			}
		}
		public string Artist
		{
			get
			{
				return this._songDBMeta.artist;
			}
			set
			{
				if (this._songDBMeta.artist == null)
				{
					this._songDBMeta.artist = value;
				}
			}
		}
		public short Year
		{
			get
			{
				return this._songDBMeta.year;
			}
			set
			{
				if (this._songDBMeta.year == 0)
				{
					this._songDBMeta.year = value;
				}
			}
		}
		public short Track
		{
			get
			{
				return this._songDBMeta.track;
			}
			set
			{
				if (this._songDBMeta.track == 0)
				{
					this._songDBMeta.track = value;
				}
			}
		}
		public string SongId
		{
			get
			{
				return this._songDBMeta.songId;
			}
			set
			{
				if (this._songDBMeta.songId == null)
				{
					this._songDBMeta.songId = value;
				}
			}
		}
		public string AlbumKey
		{
			get
			{
				return this._songDBMeta.albumKey;
			}
			set
			{
				if (this._songDBMeta.albumKey == null)
				{
					this._songDBMeta.albumKey = value;
				}
			}
		}
		public string SongKey
		{
			get
			{
				return this._songDBMeta.songKey;
			}
			set
			{
				if (this._songDBMeta.songKey == null)
				{
					this._songDBMeta.songKey = value;
				}
			}
		}
		public string Location
		{
			get
			{
				return this._songDBMeta.location;
			}
			set
			{
				if (this._songDBMeta.location == null)
				{
					this._songDBMeta.location = value;
				}
			}
		}
		public bool IsFromCollection
		{
			get
			{
				return this._songDBMeta.isFromCollection;
			}
			set
			{
				this._songDBMeta.isFromCollection = value;
			}
		}
		public long Speed
		{
			get
			{
				return this._songDBMeta.speed;
			}
			set
			{
				this._songDBMeta.speed = value;
			}
		}
		public long FileSize
		{
			get
			{
				return this._songDBMeta.fileSize;
			}
			set
			{
				this._songDBMeta.fileSize = value;
			}
		}
		public int Progress
		{
			get
			{
				return this._songDBMeta.progress;
			}
			set
			{
				this._songDBMeta.progress = value;
			}
		}
		public SongDBMeta SongDBMeta
		{
			get
			{
				return this._songDBMeta;
			}
		}
		public SongMeta()
		{
			this._songDBMeta = default(SongDBMeta);
		}
		public SongMeta(SongDBMeta songDBMeta)
		{
			this._songDBMeta = songDBMeta;
		}
		public override void MergeFrom(ObservableMeta obj)
		{
			SongMeta songMeta = (SongMeta)obj;
			this.Album = songMeta.Album;
			this.AlbumKey = songMeta.AlbumKey;
			this.Artist = songMeta.Artist;
			this.FileSize = songMeta.FileSize;
			this.Location = songMeta.Location;
			this.Song = songMeta.Song;
			this.SongId = songMeta.SongId;
			this.SongKey = songMeta.SongKey;
			this.Speed = songMeta.Speed;
			this.Track = songMeta.Track;
			this.Year = songMeta.Year;
			this.DownloadStatus = songMeta.DownloadStatus;
		}
		public void Refresh()
		{
			base.NotifyObservers(this);
		}
	}
}
