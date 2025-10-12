using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CandyFactory
{
	public class MixedCandy : CandyBase
	{
		public List<CandyBase> Components { get; private set; }
		public MixedCandy(string name, double weight, decimal basePrice, params CandyBase[] components)
		: base(name, weight, basePrice)
		{
			Components = new List<CandyBase>(components ?? Array.Empty<CandyBase>());
		}
		public override void Prepare()
		{
			Console.WriteLine($"Preparing mixed candy '{Name}', containing: {string.Join(",", Components.ConvertAll(c => c.Name))}");
		}
	}
}
