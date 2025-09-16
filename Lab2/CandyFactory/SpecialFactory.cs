using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFactory
{
	public class SpecialFactory<T> : Factory<T> where T : CandyBase
	{
		public string Specialization { get; }

		public SpecialFactory(string specialization) : base(specialization)
		{
			Specialization = specialization;
		}
	}
}