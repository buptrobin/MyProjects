using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Threading;
namespace LongkeyMusic
{
	internal class MetaDataListView
	{
		private Dictionary<string, ObservableMeta> _listData;
		private ObjectListView _listView;
		private object synRoot = new object();
		public int ListSize
		{
			get
			{
				object obj;
				Monitor.Enter(obj = this.synRoot);
				int count;
				try
				{
					count = this._listData.Count;
				}
				finally
				{
					Monitor.Exit(obj);
				}
				return count;
			}
		}
		public MetaDataListView(ObjectListView listView)
		{
			this._listView = listView;
			this._listData = new Dictionary<string, ObservableMeta>();
			this._listView.SetObjects(this._listData.Values);
		}
		public ObservableMeta GetObject(string key)
		{
			object obj;
			Monitor.Enter(obj = this.synRoot);
			ObservableMeta result;
			try
			{
				if (this._listData.ContainsKey(key))
				{
					result = this._listData[key];
				}
				else
				{
					result = null;
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return result;
		}
		public void ResetObjects(Dictionary<string, ObservableMeta> listData)
		{
			this._listData = listData;
			this._listView.SetObjects(listData.Values);
			this._listView.BuildList();
		}
		public void SetObject(string key, ObservableMeta other)
		{
			object obj;
			Monitor.Enter(obj = this.synRoot);
			try
			{
				if (this._listData.ContainsKey(key))
				{
					ObservableMeta observableMeta = this._listData[key];
					observableMeta.MergeFrom(other);
					this._listData[key] = other;
					this._listView.RefreshObject(observableMeta);
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public void AddObject(string key, ObservableMeta meta)
		{
			object obj;
			Monitor.Enter(obj = this.synRoot);
			try
			{
				if (!this._listData.ContainsKey(key))
				{
					this._listData.Add(key, meta);
					this._listView.AddObject(meta);
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public void RemoveObject(string key)
		{
			object obj;
			Monitor.Enter(obj = this.synRoot);
			try
			{
				if (this._listData.ContainsKey(key))
				{
					this._listView.RemoveObject(this._listData[key]);
					this._listData.Remove(key);
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
	}
}
