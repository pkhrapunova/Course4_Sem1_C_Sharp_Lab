using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFactory
{
	public class Shop
	{
		public string ShopName { get; }
		public Factory<CandyBase> SourceFactory { get; }

		public Shop(string name, Factory<CandyBase> factory)
		{
			ShopName = name;
			SourceFactory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		public void SellAll()
		{
			Console.WriteLine($"Shop '{ShopName}' selling items from factory '{SourceFactory.Name}' ({SourceFactory.GetAll().Count()} items)");
		}
	}
}