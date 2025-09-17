using System;


namespace CandyFactory
{
	public static class CandyExtensions
	{
		public static string ShortLabel(this CandyBase candy)
		{
			return $"{candy.Name} ({candy.Weight}g)";
		}


		public static void ForEachDo<T>(this System.Collections.Generic.IEnumerable<T> seq, Action<T> action)
		{
			foreach (var s in seq) action(s);
		}
	}
}
