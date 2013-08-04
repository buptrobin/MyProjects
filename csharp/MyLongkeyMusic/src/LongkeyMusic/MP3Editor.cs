using HundredMilesSoftware.UltraID3Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace LongkeyMusic
{
	public class MP3Editor
	{
		private UltraID3 _id3;
		private ID3v2Tag _v2Tags;
		public MP3Editor(string fileName)
		{
			this._id3 = new UltraID3();
			this._id3.Read(fileName);
			this._v2Tags = this._id3.ID3v2Tag;
			this._v2Tags.Frames.Clear();
		}
		public void SetTitle(string title)
		{
			ID3v23TitleFrame iD3v23TitleFrame = new ID3v23TitleFrame(TextEncodingTypes.Unicode);
			iD3v23TitleFrame.Title = title;
			this._v2Tags.Frames.Add(iD3v23TitleFrame);
		}
		public void SetAlbum(string album)
		{
			ID3v23AlbumFrame iD3v23AlbumFrame = new ID3v23AlbumFrame(TextEncodingTypes.Unicode);
			iD3v23AlbumFrame.Album = album;
			this._v2Tags.Frames.Add(iD3v23AlbumFrame);
		}
		public void SetSinger(string singer)
		{
			ID3v23ArtistFrame iD3v23ArtistFrame = new ID3v23ArtistFrame(TextEncodingTypes.Unicode);
			iD3v23ArtistFrame.Artist = singer;
			this._v2Tags.Frames.Add(iD3v23ArtistFrame);
		}
		public void SetYear(short year)
		{
			if (year > 0)
			{
				ID3v23YearFrame iD3v23YearFrame = new ID3v23YearFrame();
				iD3v23YearFrame.Year = new short?(year);
				this._v2Tags.Frames.Add(iD3v23YearFrame);
			}
		}
		public void SetCover(Bitmap picture)
		{
			ID3v23PictureFrame iD3v23PictureFrame = new ID3v23PictureFrame();
			iD3v23PictureFrame.MIMEType = "image/jpeg";
			iD3v23PictureFrame.Picture = picture;
			iD3v23PictureFrame.PictureType = PictureTypes.CoverFront;
			this._v2Tags.Frames.Add(iD3v23PictureFrame);
		}
		public void SetAlubmArtist(string albumArtist)
		{
			ID3v23BandFrame iD3v23BandFrame = new ID3v23BandFrame(TextEncodingTypes.Unicode);
			iD3v23BandFrame.Band = albumArtist;
			this._v2Tags.Frames.Add(iD3v23BandFrame);
		}
		public void SetTrack(short track)
		{
			ID3v23TrackNumFrame iD3v23TrackNumFrame = new ID3v23TrackNumFrame();
			iD3v23TrackNumFrame.TrackNum = new short?(track);
			this._v2Tags.Frames.Add(iD3v23TrackNumFrame);
		}
		public void SetLyris(MP3Lyris lyris)
		{
			if (lyris == null)
			{
				return;
			}
			ID3v23SynchronizedLyricsFrame iD3v23SynchronizedLyricsFrame = new ID3v23SynchronizedLyricsFrame(TextEncodingTypes.Unicode);
			List<MP3Lyris.LyrisUnit> lyris2 = lyris.GetLyris();
			for (int i = 0; i < lyris2.Count; i++)
			{
				iD3v23SynchronizedLyricsFrame.SynchronizedLyrics.Add(lyris2[i].lyric, lyris2[i].timestamp);
			}
			this._v2Tags.Frames.Add(iD3v23SynchronizedLyricsFrame);
		}
		public void SetUnSyncLyris(MP3Lyris lyris)
		{
			if (lyris == null)
			{
				return;
			}
			ID3v23UnsynchedLyricsFrame iD3v23UnsynchedLyricsFrame = new ID3v23UnsynchedLyricsFrame(TextEncodingTypes.Unicode);
			string text = "";
			List<MP3Lyris.LyrisUnit> sortedLyris = lyris.GetSortedLyris();
			for (int i = 0; i < sortedLyris.Count; i++)
			{
				text = text + sortedLyris[i].lyric + "\n";
			}
			iD3v23UnsynchedLyricsFrame.UnsynchedLyrics = text;
			this._v2Tags.Frames.Add(iD3v23UnsynchedLyricsFrame);
		}
		public void Commit()
		{
			try
			{
				this._id3.Write();
			}
			catch (Exception)
			{
			}
		}
	}
}
