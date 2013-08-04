using System;
using System.Collections.Generic;
namespace LongkeyMusic
{
	public class SongFactory
	{
		private List<IObserver> _observers = new List<IObserver>();
		private static SongFactory _instance;
		public static SongFactory Instance
		{
			get
			{
				if (SongFactory._instance == null)
				{
					SongFactory._instance = new SongFactory();
				}
				return SongFactory._instance;
			}
		}
		public void AddObserver(IObserver observer)
		{
			this._observers.Add(observer);
		}
		private SongFactory()
		{
		}
		public SongMeta CreateNewSong(string songId, string albumKey)
		{
			SongMeta songMeta = new SongMeta();
			foreach (IObserver current in this._observers)
			{
				songMeta.Register(current);
			}
			songMeta.AlbumKey = albumKey;
			songMeta.SongId = songId;
			songMeta.DownloadStatus = SongMeta.Status.NOT_START;
			return songMeta;
		}
		public SongMeta CreateNewSong(SongDBMeta songDBMeta)
		{
			SongMeta songMeta = new SongMeta(songDBMeta);
			foreach (IObserver current in this._observers)
			{
				songMeta.Register(current);
			}
			return songMeta;
		}
	}
}
