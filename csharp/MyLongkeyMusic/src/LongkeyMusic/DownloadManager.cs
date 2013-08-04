using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
namespace LongkeyMusic
{
	public class DownloadManager
	{
		private int _maxThread;
		private int _currentThread;
		private AlbumMeta _meta;
		private List<SongDownloader> _dList;
		private bool _isStop;
		private Stopwatch _sw = new Stopwatch();
		private BackgroundWorker _bw = new BackgroundWorker();
		private BackgroundWorker _checkStatusWorker = new BackgroundWorker();
		public string DMKey
		{
			get
			{
				return this._meta.AlbumKey;
			}
		}
		public DownloadManager(int maxThread, AlbumMeta meta, RunWorkerCompletedEventHandler downloadCompeleteHandler)
		{
			this._maxThread = maxThread;
			this._currentThread = 0;
			this._isStop = false;
			this._meta = meta;
			this._bw.WorkerSupportsCancellation = true;
			this._checkStatusWorker.WorkerSupportsCancellation = true;
			this._dList = new List<SongDownloader>();
			List<SongMeta> albumSongs = DataAndUIMaster.Instance.GetAlbumSongs(meta.AlbumKey);
			foreach (SongMeta current in albumSongs)
			{
				this._dList.Add(new SongDownloader(current));
			}
			this._bw.DoWork += new DoWorkEventHandler(this.StartDownloads);
			this._bw.RunWorkerCompleted += downloadCompeleteHandler;
			this._checkStatusWorker.DoWork += new DoWorkEventHandler(this.CheckQueueStatus);
		}
		public void Stop()
		{
			this._isStop = true;
			this.SetTaskStatus(AlbumMeta.Status.Stopping);
		}
		public void Start()
		{
			this._sw.Start();
			this._bw.RunWorkerAsync();
			this._checkStatusWorker.RunWorkerAsync();
		}
		private void CheckQueueStatus(object sender, DoWorkEventArgs e)
		{
			bool flag = true;
			while (flag)
			{
				Thread.Sleep(2000);
				flag = false;
				int num = 0;
				long num2 = 0L;
				bool flag2 = false;
				for (int i = 0; i < this._dList.Count; i++)
				{
					if (this._dList[i].Status == SongMeta.Status.NOT_START || this._dList[i].Status == SongMeta.Status.DOWNLOADING || this._dList[i].Status == SongMeta.Status.RETRYING)
					{
						flag = true;
					}
					else
					{
						if (this._dList[i].Status == SongMeta.Status.COMPLETED)
						{
							num++;
						}
					}
					if (this._dList[i].Status == SongMeta.Status.DOWNLOADING)
					{
						num2 += this._dList[i].Speed;
					}
					if (this._dList[i].IsActive)
					{
						flag2 = true;
					}
				}
				this._meta.Speed = num2;
				this._meta.FinishedTracks = num;
				Application.DoEvents();
				if (!flag2 && this._isStop)
				{
					this.SetTaskStatus(AlbumMeta.Status.Stopped);
					this._isStop = false;
					flag = false;
				}
				else
				{
					this._meta.Refresh();
				}
			}
		}
		private void SetTaskStatus(AlbumMeta.Status status)
		{
			this._meta.DownloadStatus = status;
			if (status == AlbumMeta.Status.Finished)
			{
				this._meta.FinishTime = DateTime.Now;
			}
			this._meta.IsUIRefreshNeed = true;
			this._meta.Refresh();
		}
		private void StartDownloads(object sender, DoWorkEventArgs e)
		{
			this.SetTaskStatus(AlbumMeta.Status.Downloading);
			bool flag = true;
			while (flag)
			{
				flag = false;
				Thread.Sleep(2000);
				if (this._isStop)
				{
					for (int i = 0; i < this._dList.Count; i++)
					{
						this._dList[i].StopDownload();
					}
					return;
				}
				this._currentThread = 0;
				for (int j = 0; j < this._dList.Count; j++)
				{
					if (this._dList[j].Status == SongMeta.Status.DOWNLOADING || this._dList[j].Status == SongMeta.Status.RETRYING)
					{
						this._currentThread++;
					}
				}
				for (int k = 0; k < this._dList.Count; k++)
				{
					if ((this._dList[k].Status == SongMeta.Status.NOT_START || this._dList[k].Status == SongMeta.Status.STOPPED) && this._currentThread < this._maxThread)
					{
						this._currentThread++;
						this._dList[k].StartDownload();
					}
					if (this._dList[k].Status == SongMeta.Status.RETRYING)
					{
						this._dList[k].StartDownload();
					}
				}
				for (int l = 0; l < this._dList.Count; l++)
				{
					if (this._dList[l].Status == SongMeta.Status.DOWNLOADING || this._dList[l].Status == SongMeta.Status.RETRYING || this._dList[l].Status == SongMeta.Status.NOT_START)
					{
						flag = true;
					}
				}
			}
			this.SetTaskStatus(AlbumMeta.Status.Finished);
		}
	}
}
