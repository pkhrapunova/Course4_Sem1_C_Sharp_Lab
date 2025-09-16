using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CandyFactory
{
	public class Chocolate : CandyBase
	{
		public int CocoaPercent { get; private set; }


		public Chocolate(string name, double weight, decimal basePrice, int cocoaPercent)
		: base(name, weight, basePrice)
		{
			CocoaPercent = cocoaPercent;
		}


		public override void Prepare()
		{
			Console.WriteLine($"Tempering and molding chocolate '{Name}' ({CocoaPercent}% cocoa)...");
		}


		public override string GetLabel() => base.GetLabel() + $" (Cocoa: {CocoaPercent}% )";
	}
}
