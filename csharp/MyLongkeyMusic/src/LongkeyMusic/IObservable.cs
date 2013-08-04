using System;
namespace LongkeyMusic
{
	public interface IObservable
	{
		void Register(IObserver observer);
		void UnRegister(IObserver observer);
	}
}
