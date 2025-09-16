using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CandyFactory
{
	public class Toffee : CandyBase
	{
		public bool IsCaramelized { get; private set; }


		public Toffee(string name, double weight, decimal basePrice, bool caramelized)
		: base(name, weight, basePrice)
		{
			IsCaramelized = caramelized;
		}


		public override void Prepare()
		{
			Console.WriteLine($"Cooking toffee '{Name}' - caramelized: {IsCaramelized}...");
		}


		public override bool Inspect()
		{
			// override interface implementation
			return Weight <= 50; // example: toffees >50g fail inspection
		}
	}
}
