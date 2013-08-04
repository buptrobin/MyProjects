using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace LongkeyMusic
{
	[Serializable]
	public class Preference
	{
		private static Preference _instance;
		private static string _binaryPath = Util.GetLocalDir() + "/preference.data";
		public static Preference Instance
		{
			get
			{
				if (Preference._instance == null)
				{
					Preference._instance = new Preference();
					Preference._instance.LoadPreference();
				}
				return Preference._instance;
			}
		}
		public bool IsMonitorClipboard
		{
			get;
			set;
		}
		public bool IsNotify
		{
			get;
			set;
		}
		private Preference()
		{
			this.IsMonitorClipboard = false;
			this.IsNotify = false;
		}
		public void Serialize()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(Preference._binaryPath, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, Preference._instance);
			stream.Close();
		}
		public void LoadPreference()
		{
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				Stream stream = new FileStream(Preference._binaryPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				Preference._instance = (Preference)binaryFormatter.Deserialize(stream);
				stream.Close();
			}
			catch (Exception)
			{
				Preference._instance = new Preference();
			}
		}
	}
}
