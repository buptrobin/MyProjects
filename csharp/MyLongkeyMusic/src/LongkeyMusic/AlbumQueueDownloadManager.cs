using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace LongkeyMusic
{
	[Serializable]
	internal class AlbumQueueDownloadManager : IObserver
	{
		private readonly object waitingSyncRoot = new object();
		private Dictionary<string, AlbumMeta> WaitingQueue = new Dictionary<string, AlbumMeta>();
		private readonly object downloadingSyncRoot = new object();
		private Dictionary<string, AlbumMeta> DownloadingQueue = new Dictionary<string, AlbumMeta>();
		private Dictionary<string, DownloadManager> _dms = new Dictionary<string, DownloadManager>();
		private int MaxDownloadingQueueSize = 1;
		private BackgroundWorker taskMgr = new BackgroundWorker();
		private static AlbumQueueDownloadManager _aqdm;
		public static AlbumQueueDownloadManager Instance
		{
			get
			{
				if (AlbumQueueDownloadManager._aqdm == null)
				{
					AlbumQueueDownloadManager._aqdm = new AlbumQueueDownloadManager();
				}
				return AlbumQueueDownloadManager._aqdm;
			}
		}
		public void Notify(object obj)
		{
			AlbumMeta albumMeta = (AlbumMeta)obj;
			AlbumMeta.Status downloadStatus = albumMeta.DownloadStatus;
			switch (downloadStatus)
			{
			case AlbumMeta.Status.New:
			{
				AlbumMetaDownloader albumMetaDownloader = new AlbumMetaDownloader(albumMeta);
				albumMetaDownloader.FetchAlbumMeta();
				return;
			}
			case AlbumMeta.Status.GotMeta:
				break;
			case AlbumMeta.Status.ReadyToDownload:
				this.AddTask(albumMeta);
				return;
			default:
				if (downloadStatus != AlbumMeta.Status.Stopped)
				{
					return;
				}
				this.StopTask(albumMeta);
				break;
			}
		}
		private AlbumQueueDownloadManager()
		{
			this.taskMgr.DoWork += new DoWorkEventHandler(this.MonitorTask);
			this.taskMgr.RunWorkerAsync();
		}
		private void MonitorTask(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				Application.DoEvents();
				Thread.Sleep(2000);
				object obj;
				Monitor.Enter(obj = this.downloadingSyncRoot);
				try
				{
					while (this.DownloadingQueue.Count < this.MaxDownloadingQueueSize && this.WaitingQueue.Count > 0)
					{
						AlbumMeta albumMeta = null;
						object obj2;
						Monitor.Enter(obj2 = this.waitingSyncRoot);
						try
						{
							if (this.WaitingQueue.Count > 0)
							{
								albumMeta = this.WaitingQueue.Values.ToArray<AlbumMeta>()[0];
								this.WaitingQueue.Remove(albumMeta.AlbumKey);
							}
						}
						finally
						{
							Monitor.Exit(obj2);
						}
						if (albumMeta != null)
						{
							this.AddToDownloading(albumMeta);
						}
						Application.DoEvents();
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}
		}
		public void AddToDownloading(AlbumMeta albumMeta)
		{
			if (!this.DownloadingQueue.ContainsKey(albumMeta.AlbumKey))
			{
				this.DownloadingQueue.Add(albumMeta.AlbumKey, albumMeta);
				DownloadManager value = new DownloadManager(5, albumMeta, delegate(object sender, RunWorkerCompletedEventArgs e)
				{
					object obj;
					Monitor.Enter(obj = this.downloadingSyncRoot);
					try
					{
						UpdateManager.Instance.UploadAlbumStat(albumMeta);
						this.DownloadingQueue.Remove(albumMeta.AlbumKey);
						this._dms.Remove(albumMeta.AlbumKey);
					}
					finally
					{
						Monitor.Exit(obj);
					}
				});
				this._dms.Add(albumMeta.AlbumKey, value);
				this._dms[albumMeta.AlbumKey].Start();
			}
		}
		public void AddTask(AlbumMeta albumMeta)
		{
			object obj;
			Monitor.Enter(obj = this.downloadingSyncRoot);
			try
			{
				object obj2;
				Monitor.Enter(obj2 = this.waitingSyncRoot);
				try
				{
					if (this.DownloadingQueue.Count >= this.MaxDownloadingQueueSize)
					{
						this.AddToWaiting(albumMeta);
						return;
					}
				}
				finally
				{
					Monitor.Exit(obj2);
				}
				this.AddToDownloading(albumMeta);
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public void StopTask(AlbumMeta meta)
		{
			string albumKey = meta.AlbumKey;
			object obj;
			Monitor.Enter(obj = this.waitingSyncRoot);
			try
			{
				if (this.WaitingQueue.ContainsKey(albumKey))
				{
					this.WaitingQueue.Remove(albumKey);
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
			object obj2;
			Monitor.Enter(obj2 = this.downloadingSyncRoot);
			try
			{
				if (this.DownloadingQueue.ContainsKey(albumKey))
				{
					this._dms[albumKey].Stop();
				}
			}
			finally
			{
				Monitor.Exit(obj2);
			}
		}
		public void DeleteTask(AlbumMeta album, bool isDeleteFiles)
		{
			string albumKey = album.AlbumKey;
			object obj;
			Monitor.Enter(obj = this.waitingSyncRoot);
			try
			{
				if (this.WaitingQueue.ContainsKey(albumKey))
				{
					this.WaitingQueue.Remove(albumKey);
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
			object obj2;
			Monitor.Enter(obj2 = this.downloadingSyncRoot);
			try
			{
				if (this.DownloadingQueue.ContainsKey(albumKey))
				{
					throw new TaskOperationException("任务尚未停止, 无法删除");
				}
			}
			finally
			{
				Monitor.Exit(obj2);
			}
			if (isDeleteFiles)
			{
				foreach (SongDBMeta current in album.Songs.Values)
				{
					current.DeleteFiles();
				}
			}
			album.DownloadStatus = AlbumMeta.Status.Deleted;
			album.Refresh();
		}
		private void AddToWaiting(AlbumMeta albumMeta)
		{
			if (!this.WaitingQueue.ContainsKey(albumMeta.AlbumKey))
			{
				albumMeta.DownloadStatus = AlbumMeta.Status.Waiting;
				albumMeta.IsUIRefreshNeed = true;
				albumMeta.Refresh();
				this.WaitingQueue.Add(albumMeta.AlbumKey, albumMeta);
			}
		}
	}
}
