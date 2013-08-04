using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
namespace LongkeyMusic
{
	public class SQLManager
	{
		private SQLiteDBHelper _sb;
		private readonly string DB_NAME = "ALBUM";
		private readonly string DB_PATH = Util.GetLocalDir() + "/hs.db3";
		private static SQLManager _instance;
		public static SQLManager Instance
		{
			get
			{
				if (SQLManager._instance == null)
				{
					SQLManager._instance = new SQLManager();
				}
				return SQLManager._instance;
			}
		}
		private SQLManager()
		{
			this._sb = new SQLiteDBHelper(this.DB_PATH);
			if (!File.Exists(this.DB_PATH))
			{
				try
				{
					SQLiteDBHelper.CreateDB(this.DB_PATH);
					string sql = "CREATE TABLE " + this.DB_NAME + " (key vchar(256) NOT NULL PRIMARY KEY, addDate datetime, album data)";
					this._sb.ExecuteNonQuery(sql, null);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}
		private byte[] SerializeAlbum(AlbumDBMeta albumDBMeta)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			formatter.Serialize(stream, albumDBMeta);
			byte[] array = new byte[stream.Length];
			stream.Position = 0L;
			stream.Read(array, 0, (int)stream.Length);
			stream.Close();
			return array;
		}
		private AlbumDBMeta DeSerializeAlbum(byte[] bytes)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			Stream serializationStream = new MemoryStream(bytes);
			return (AlbumDBMeta)binaryFormatter.Deserialize(serializationStream);
		}
		public void AddAlbum(AlbumDBMeta albumDBMeta)
		{
			byte[] value = this.SerializeAlbum(albumDBMeta);
			string sql = "INSERT OR IGNORE INTO " + this.DB_NAME + " (key, addDate, album)values(@key, @addDate, @album)";
			SQLiteParameter[] parameters = new SQLiteParameter[]
			{
				new SQLiteParameter("@key", albumDBMeta.albumKey),
				new SQLiteParameter("@addDate", DateTime.Now),
				new SQLiteParameter("@album", value)
			};
			this._sb.ExecuteNonQuery(sql, parameters);
		}
		public void UpdateAlbum(AlbumDBMeta albumDBMeta)
		{
			string sql = "SELECT album from " + this.DB_NAME + " where key = @key";
			SQLiteParameter[] parameters = new SQLiteParameter[]
			{
				new SQLiteParameter("@key", albumDBMeta.albumKey)
			};
			SQLiteDataReader sQLiteDataReader = this._sb.ExecuteReader(sql, parameters);
			AlbumMeta albumMeta = new AlbumMeta(this.DeSerializeAlbum((byte[])sQLiteDataReader.GetValue(0)));
			sQLiteDataReader.Close();
			albumMeta.MergeFrom(new AlbumMeta(albumDBMeta));
			sql = "UPDATE " + this.DB_NAME + " SET album = @album where key = @key";
			parameters = new SQLiteParameter[]
			{
				new SQLiteParameter("@album", this.SerializeAlbum(albumMeta.AlbumDBData)),
				new SQLiteParameter("@key", albumMeta.AlbumKey)
			};
			this._sb.ExecuteNonQuery(sql, parameters);
		}
		public void RemoveAlbum(string key)
		{
			string sql = "DELETE from " + this.DB_NAME + " where key = @key";
			SQLiteParameter[] parameters = new SQLiteParameter[]
			{
				new SQLiteParameter("@key", key)
			};
			this._sb.ExecuteNonQuery(sql, parameters);
		}
		public Dictionary<string, AlbumDBMeta> LoadAlbums()
		{
			string sql = "SELECT album from " + this.DB_NAME + " order by addDate";
			SQLiteDataReader sQLiteDataReader = this._sb.ExecuteReader(sql, null);
			Dictionary<string, AlbumDBMeta> dictionary = new Dictionary<string, AlbumDBMeta>();
			if (!sQLiteDataReader.HasRows)
			{
				return dictionary;
			}
			while (sQLiteDataReader.Read())
			{
				AlbumDBMeta value = this.DeSerializeAlbum((byte[])sQLiteDataReader.GetValue(0));
				dictionary.Add(value.albumKey, value);
			}
			sQLiteDataReader.Close();
			return dictionary;
		}
	}
}
