using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFactory
{
	public static class CandyExtensions
	{
		// extension method
		public static string ShortLabel(this CandyBase candy)
		{
			return $"{candy.Name} ({candy.Weight}g)";
		}


		// generic extension method for IEnumerable
		public static void ForEachDo<T>(this System.Collections.Generic.IEnumerable<T> seq, Action<T> action)
		{
			foreach (var s in seq) action(s);
		}
	}
}
