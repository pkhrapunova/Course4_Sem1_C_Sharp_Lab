using System;


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
			Console.WriteLine($"Заливаем сироп для леденца '{Name}' на палочку ({StickMaterial})...");
		}

	}
}
