using System;
using System.Collections.Generic;
using System.Drawing;
namespace LongkeyMusic
{
	[Serializable]
	public struct AlbumDBMeta
	{
		public string album;
		public Image coverThumbnail;
		public string artist;
		public short year;
		public int tracks;
		public Dictionary<string, SongDBMeta> songs;
		public string albumKey;
		public DateTime startTime;
		public DateTime finishTime;
		public string baseFolder;
		public int finishedTracks;
		public bool isCollection;
		public AlbumMeta.Status downloadStatus;
	}
}
