using System;
using System.Collections.Generic;
namespace LongkeyMusic
{
	public class MP3Lyris
	{
		public class LyrisUnit : IComparable<MP3Lyris.LyrisUnit>
		{
			public int timestamp;
			public string lyric;
			public int CompareTo(MP3Lyris.LyrisUnit u)
			{
				return this.timestamp - u.timestamp;
			}
		}
		private List<MP3Lyris.LyrisUnit> _lyris;
		public MP3Lyris()
		{
			this._lyris = new List<MP3Lyris.LyrisUnit>();
		}
		public void Add(int timestamp, string lyric)
		{
			this.Add(new MP3Lyris.LyrisUnit
			{
				timestamp = timestamp,
				lyric = lyric
			});
		}
		public void Add(MP3Lyris.LyrisUnit lu)
		{
			this._lyris.Add(lu);
		}
		public void Clear()
		{
			this._lyris.Clear();
		}
		public List<MP3Lyris.LyrisUnit> GetLyris()
		{
			return this._lyris;
		}
		public List<MP3Lyris.LyrisUnit> GetSortedLyris()
		{
			this._lyris.Sort();
			return this._lyris;
		}
	}
}
