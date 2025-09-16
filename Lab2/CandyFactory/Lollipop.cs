using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CandyFactory
{
	public class Lollipop : CandyBase
	{
		public string StickMaterial { get; private set; }


		public Lollipop(string name, double weight, decimal basePrice, string stickMaterial)
		: base(name, weight, basePrice)
		{
			StickMaterial = stickMaterial;
		}


		public override void Prepare()
		{
			Console.WriteLine($"Pouring lollipop syrup for '{Name}' onto stick ({StickMaterial})...");
		}
	}
}
