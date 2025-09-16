using System;
using System.Linq;

namespace CandyFactory
{
	class Program
	{
		static void Main()
		{
			var chocolateFactory = new SpecialFactory<Chocolate>("Premium Chocolate");
			chocolateFactory.Add(new Chocolate("Alpen Gold", 120, 100, 45));
			chocolateFactory.Add(new Chocolate("Milka", 150, 90, 50));

			var lollipopFactory = new SpecialFactory<Lollipop>("Fruit Lollipops");
			lollipopFactory.Add(new Lollipop("Chupa Chups", 50, 20, "Plastic"));

			// ✅ общий список фабрик
			var factories = new IFactory[] { chocolateFactory, lollipopFactory };

			// вывод всех конфет
			foreach (var factory in factories)
			{
				Console.WriteLine($"Factory: {factory.Name}");
				foreach (var candy in factory.GetAll())
				{
					Console.WriteLine($"  {candy}");
				}
			}

			Console.WriteLine("\n🔍 Поиск дорогих шоколадок (>100):");

			var expensiveChocolates = chocolateFactory.FindByPredicate(c => c.Price > 100);

			foreach (var choc in expensiveChocolates)
			{
				Console.WriteLine(choc);
			}
		}
	}
}