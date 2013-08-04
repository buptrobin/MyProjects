using System;
using System.IO;
using System.Text.RegularExpressions;
namespace LongkeyMusic
{
	internal class Util
	{
		public enum AlbumType
		{
			Ablum,
			Collection,
			Hot,
			Unrecognize
		}
		public static Util.AlbumType IsValidAlbumUrl(string url)
		{
			if (Regex.Match(url, "^http://www.xiami.com/album").Success)
			{
				return Util.AlbumType.Ablum;
			}
			if (Regex.Match(url, "^http://www.xiami.com/song/showcollect/id/").Success)
			{
				return Util.AlbumType.Collection;
			}
			if (Regex.Match(url, "^http://www.xiami.com/artist/[0-9]+$").Success)
			{
				return Util.AlbumType.Hot;
			}
			return Util.AlbumType.Unrecognize;
		}
		public static string CorrectPath(string dir)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			string text = dir;
			for (int i = 0; i < invalidFileNameChars.Length; i++)
			{
				text = text.Replace(invalidFileNameChars[i], ' ');
			}
			return text.Trim();
		}
		public static string CorrectFile(string fileName)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			string text = fileName;
			for (int i = 0; i < invalidFileNameChars.Length; i++)
			{
				text = text.Replace(invalidFileNameChars[i], ' ');
			}
			return text.Trim();
		}
		public static string GetAlbumDir(AlbumMeta meta)
		{
			return Path.Combine(meta.BaseFolder, Util.CorrectPath(meta.Artist) + "\\" + Util.CorrectPath(meta.Album));
		}
		public static void DeleteFolder(string dir)
		{
			string[] fileSystemEntries = Directory.GetFileSystemEntries(dir);
			for (int i = 0; i < fileSystemEntries.Length; i++)
			{
				string text = fileSystemEntries[i];
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				else
				{
					Util.DeleteFolder(text);
				}
			}
			Directory.Delete(dir);
		}
		public static string GetLocalDir()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "longkeymusic");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}
	}
}
