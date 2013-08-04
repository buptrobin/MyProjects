using System;
namespace LongkeyMusic
{
	public interface IObserver
	{
		void Notify(object obj);
	}
}
