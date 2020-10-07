using System;

namespace ZeebeWorker.ZeebeWorker.Extensions
{
	internal static class IntExtensions
	{
		public static void Repeat(this int numberOfTimes, Action action)
		{
			for (int i = 0; i < numberOfTimes; i++)
				action();
		}
	}
}
