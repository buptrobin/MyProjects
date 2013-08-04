using System;
using System.Collections.Generic;
namespace LongkeyMusic
{
	[Serializable]
	public abstract class ObservableMeta : IObservable
	{
		private HashSet<IObserver> _observers = new HashSet<IObserver>();
		public void Register(IObserver observer)
		{
			this._observers.Add(observer);
		}
		public void UnRegister(IObserver observer)
		{
			this._observers.Remove(observer);
		}
		public void NotifyObservers(object obj)
		{
			foreach (IObserver current in this._observers)
			{
				current.Notify(obj);
			}
		}
		public abstract void MergeFrom(ObservableMeta ther);
		public void UnRegisterAll()
		{
			this._observers = null;
		}
	}
}
