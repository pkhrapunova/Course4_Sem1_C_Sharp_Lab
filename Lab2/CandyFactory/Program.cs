using System;
using System.Linq;

namespace CandyFactory
{
	class Program
	{
		static void Main()
		{
			// 2. Конструкторы с параметрами
			var chocolateFactory = new SpecialFactory<Chocolate>("Premium Chocolate");
			chocolateFactory.Add(new Chocolate("Alpen Gold", 120, 100, 45));
			chocolateFactory.Add(new Chocolate("Milka", 150, 90, 50));
			chocolateFactory.Add(new Chocolate("Lindt", 100, 150, 70));

			var lollipopFactory = new SpecialFactory<Lollipop>("Fruit Lollipops");
			lollipopFactory.Add(new Lollipop("Chupa Chups", 50, 20, "Plastic"));
			lollipopFactory.Add(new Lollipop("Fruttis", 45, 25, "Wood"));

			var toffeeFactory = new SpecialFactory<Toffee>("Toffee Factory");
			toffeeFactory.Add(new Toffee("Коровка", 40, 15, true));
			toffeeFactory.Add(new Toffee("Ирис", 60, 18, false));

			// 9. Наследование, 12. Обобщения, 14. Наследование обобщений
			var factories = new IFactory[] { chocolateFactory, lollipopFactory, toffeeFactory };

			// 19. Демонстрация всех элементов
			Console.WriteLine("=== Все конфеты на фабриках ===");
			foreach (var factory in factories)
			{
				Console.WriteLine($"\nФабрика: {factory.Name}");
				foreach (var candy in factory.GetAll())
				{
					Console.WriteLine($"  {candy}");
				}
			}

			// 13. Обобщенные методы
			Utils.PrintHeader("Поиск дорогих шоколадок (>100)");
			var expensiveChocolates = chocolateFactory.FindByPredicate(c => c.Price > 100);
			expensiveChocolates.ForEachDo(c => Console.WriteLine(c));

			// 15. Методы расширения
			Utils.PrintHeader("Короткие метки конфет");
			chocolateFactory.GetAll().ForEachDo(c => Console.WriteLine(c.ShortLabel()));

			// 5. Индексаторы
			Utils.PrintHeader("Демонстрация индексатора");
			var inventory = new CandyInventory();
			inventory.Add(chocolateFactory.GetAll().First());
			inventory.Add(lollipopFactory.GetAll().First());
			Console.WriteLine($"Первый элемент в инвентаре: {inventory[0]}");

			// Демонстрация SET в индексаторе
			Utils.PrintHeader("Демонстрация SET в индексаторе");
			try
			{
				inventory[0] = new Chocolate("Новый шоколад", 100, 80, 60);
				Console.WriteLine($"После замены: {inventory[0]}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при замене: {ex.Message}");
			}

			// 10. Переопределение методов
			Utils.PrintHeader("Подготовка конфет");
			foreach (var candy in inventory)
			{
				candy.Prepare();
			}

			// 11. Перегруженные операторы
			Utils.PrintHeader("Смешанные конфеты (оператор +)");
			var mixed = chocolateFactory.GetAll().First() + lollipopFactory.GetAll().First();
			mixed.Prepare();
			Console.WriteLine(mixed);

			// 16. Агрегация, 17. Композиция
			Utils.PrintHeader("Упаковка коробок");
			var box = new Box("Подарочный набор");
			foreach (var candy in chocolateFactory.GetAll().Take(2))
			{
				box.Pack(candy);
			}
			box.Pack(mixed);
			Console.WriteLine(box);

			// 3,4. Свойства с логикой
			Utils.PrintHeader("Демонстрация свойств с логикой");
			var testCandy = new Chocolate("Test", 50, 30, 60);
			Console.WriteLine($"Цена с налогом: {testCandy.Price}");

			// Демонстрация логики в set свойства Name
			Utils.PrintHeader("Демонстрация set с логикой в свойствах");
			try
			{
				var invalidCandy = new Chocolate("   ", 50, 30, 60); // Раскомментировать для теста
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка создания конфеты с пустым именем: {ex.Message}");
			}

			// 8. Статические элементы
			CandyBase.TaxRate = 0.1m;
			Console.WriteLine($"Цена с новым налогом: {testCandy.Price}");

			// 18. Интерфейсы - полиморфное использование
			Utils.PrintHeader("Проверка качества через интерфейс");
			foreach (var candy in toffeeFactory.GetAll())
			{
				IInspectable inspectable = candy; // Использование через интерфейс
				Console.WriteLine($"{candy.Name}: Проверка пройдена - {inspectable.Inspect()}");
			}

			// 14. Наследование обобщений с ограничениями
			Utils.PrintHeader("Демонстрация ограничений обобщений");
			Console.WriteLine("SpecialFactory<T> where T : CandyBase - работает корректно");


			// 16. Агрегация (магазин использует фабрику)
			var shop = new Shop("Сладкий мир", new Factory<CandyBase>("Основная фабрика"));
			shop.SellAll();

		}
	}
}