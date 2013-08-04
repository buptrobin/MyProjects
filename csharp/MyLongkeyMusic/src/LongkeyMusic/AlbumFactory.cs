using System;
using System.Collections.Generic;
namespace LongkeyMusic
{
	public class AlbumFactory
	{
		private List<IObserver> _observers = new List<IObserver>();
		private static AlbumFactory _instance;
		public static AlbumFactory Instance
		{
			get
			{
				if (AlbumFactory._instance == null)
				{
					AlbumFactory._instance = new AlbumFactory();
				}
				return AlbumFactory._instance;
			}
		}
		public void AddObserver(IObserver observer)
		{
			this._observers.Add(observer);
		}
		private AlbumFactory()
		{
		}
		public AlbumMeta CreateNewAlbum(string key)
		{
			AlbumMeta albumMeta = new AlbumMeta();
			foreach (IObserver current in this._observers)
			{
				albumMeta.Register(current);
			}
			albumMeta.AlbumKey = key;
			albumMeta.Album = key;
			albumMeta.DownloadStatus = AlbumMeta.Status.New;
			return albumMeta;
		}
		public AlbumMeta CreateNewAlbum(AlbumDBMeta albumDBMeta)
		{
			AlbumMeta albumMeta = new AlbumMeta(albumDBMeta);
			foreach (IObserver current in this._observers)
			{
				albumMeta.Register(current);
			}
			return albumMeta;
		}
	}
}
