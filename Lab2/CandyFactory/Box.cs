using System;
using System.Collections.Generic;
//17 композиция 

namespace CandyFactory
{
	public class Box
	{
		private List<CandyBase> _contents = new List<CandyBase>();


		public string Label { get; set; }


		public Box(string label)
		{
			Label = label;
		}


		public void Pack(CandyBase candy)
		{
			if (candy == null) throw new ArgumentNullException(nameof(candy));
			_contents.Add(candy);
		}


		public decimal TotalPrice
		{
			get
			{
				decimal sum = 0;
				foreach (var c in _contents) sum += c.Price;
				return sum;
			}
		}


		public override string ToString() => $"Коробка '{Label}' - {_contents.Count} шт. - Итого: {TotalPrice:C}";
	}
}
