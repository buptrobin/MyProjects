using System;
using System.IO;
namespace LongkeyMusic
{
	[Serializable]
	public struct SongDBMeta
	{
		public SongMeta.Status downloadStatus;
		public string song;
		public string album;
		public string artist;
		public short year;
		public short track;
		public string songId;
		public string albumKey;
		public string songKey;
		public string location;
		public bool isFromCollection;
		public long speed;
		public long fileSize;
		public int progress;
		public void DeleteFiles()
		{
			if (this.location != null)
			{
				string path = this.location + ".longkey";
				string path2 = this.location + ".mp3";
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
			}
		}
	}
}
