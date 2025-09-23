using System;

class Program
{
	// Делегат для условия
	delegate bool Condition(int value);

	// Метод 1: подсчёт количества элементов по условию
	static int CountElements(int[] array, Condition cond)
	{
		int count = 0;
		foreach (int elem in array)
		{
			if (cond(elem)) count++;
		}
		return count;
	}

	// Метод 2: сумма элементов по условию
	static int SumElements(int[] array, Condition cond)
	{
		int sum = 0;
		foreach (int elem in array)
		{
			if (cond(elem)) sum += elem;
		}
		return sum;
	}

	static void Main()
	{
		// Пример вектора
		int[] vector = { -5, -2, 0, 3, 7, 8, 12, -1 };

		Console.WriteLine("Выберите метод: ");
		Console.WriteLine("1 - Подсчет количества нечетных элементов");
		Console.WriteLine("2 - Подсчет суммы положительных элементов");
		int choice = int.Parse(Console.ReadLine());

		if (choice == 1)
		{
			// Условие: число нечётное
			int result = CountElements(vector, x => x % 2 != 0);
			Console.WriteLine($"Количество нечётных элементов: {result}");
		}
		else if (choice == 2)
		{
			// Условие: число положительное
			int result = SumElements(vector, x => x > 0);
			Console.WriteLine($"Сумма положительных элементов: {result}");
		}
		else
		{
			Console.WriteLine("Ошибка: нужно ввести 1 или 2!");
		}
	}
}
