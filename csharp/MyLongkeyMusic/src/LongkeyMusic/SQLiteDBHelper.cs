using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
namespace LongkeyMusic
{
	public class SQLiteDBHelper
	{
		private string connectionString = string.Empty;
		public SQLiteDBHelper(string dbPath)
		{
			this.connectionString = "Data Source=" + dbPath;
		}
		public static void CreateDB(string dbPath)
		{
			using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + dbPath))
			{
				sQLiteConnection.Open();
				using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection))
				{
					sQLiteCommand.CommandText = "CREATE TABLE Demo(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE)";
					sQLiteCommand.ExecuteNonQuery();
					sQLiteCommand.CommandText = "DROP TABLE Demo";
					sQLiteCommand.ExecuteNonQuery();
				}
			}
		}
		public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
		{
			int result = 0;
			using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connectionString))
			{
				sQLiteConnection.Open();
				using (DbTransaction dbTransaction = sQLiteConnection.BeginTransaction())
				{
					using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection))
					{
						sQLiteCommand.CommandText = sql;
						if (parameters != null)
						{
							sQLiteCommand.Parameters.AddRange(parameters);
						}
						result = sQLiteCommand.ExecuteNonQuery();
					}
					dbTransaction.Commit();
				}
			}
			return result;
		}
		public SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
		{
			SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connectionString);
			SQLiteCommand sQLiteCommand = new SQLiteCommand(sql, sQLiteConnection);
			if (parameters != null)
			{
				sQLiteCommand.Parameters.AddRange(parameters);
			}
			sQLiteConnection.Open();
			return sQLiteCommand.ExecuteReader(CommandBehavior.CloseConnection);
		}
		public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
		{
			DataTable result;
			using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connectionString))
			{
				using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sql, sQLiteConnection))
				{
					if (parameters != null)
					{
						sQLiteCommand.Parameters.AddRange(parameters);
					}
					SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand);
					DataTable dataTable = new DataTable();
					sQLiteDataAdapter.Fill(dataTable);
					result = dataTable;
				}
			}
			return result;
		}
		public object ExecuteScalar(string sql, SQLiteParameter[] parameters)
		{
			object result;
			using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connectionString))
			{
				using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sql, sQLiteConnection))
				{
					if (parameters != null)
					{
						sQLiteCommand.Parameters.AddRange(parameters);
					}
					SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand);
					DataTable dataTable = new DataTable();
					sQLiteDataAdapter.Fill(dataTable);
					result = dataTable;
				}
			}
			return result;
		}
		public DataTable GetSchema()
		{
			DataTable result;
			using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connectionString))
			{
				sQLiteConnection.Open();
				DataTable schema = sQLiteConnection.GetSchema("TABLES");
				sQLiteConnection.Close();
				result = schema;
			}
			return result;
		}
	}
}
