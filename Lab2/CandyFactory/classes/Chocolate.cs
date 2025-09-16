using System;

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
			Console.WriteLine($"Темперирование и формовка шоколада '{Name}' ({CocoaPercent}% какао)...");
		}

		public override string GetLabel() => base.GetLabel() + $" (Какао: {CocoaPercent}% )";

	}
}
