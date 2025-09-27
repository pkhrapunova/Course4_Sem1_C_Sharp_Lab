using System;

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
			// Используем protected пое _name
			Console.WriteLine($"Готовим ириску '{_name}' - карамелизировано: {IsCaramelized}...");
		}

		public override bool Inspect()
		{
			return Weight <= 50;
		}
	}
}