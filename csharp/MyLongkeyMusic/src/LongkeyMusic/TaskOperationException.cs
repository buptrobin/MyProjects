using System;
namespace LongkeyMusic
{
	internal class TaskOperationException : Exception
	{
		public TaskOperationException(string msg) : base(msg)
		{
		}
	}
}
