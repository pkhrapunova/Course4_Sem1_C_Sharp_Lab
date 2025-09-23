using System;

class Program
{
	// Делегат для условия
	delegate bool Condition(int value);

	// Событие для вызова метода
	static event Action<int[]> OnProcess;

	// Собственное исключение
	class InvalidChoiceException : Exception
	{
		public InvalidChoiceException(string message) : base(message) { }
	}

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
		int[] vector = { -5, -2, 0, 3, 7, 8, 12, -1 };

		try
		{
			Console.WriteLine("Выберите метод: ");
			Console.WriteLine("1 - Подсчет количества нечетных элементов");
			Console.WriteLine("2 - Подсчет суммы положительных элементов");

			string input = Console.ReadLine();

			if (!int.TryParse(input, out int choice))
			{
				throw new InvalidChoiceException("Ошибка: введено не число!");
			}

			if (choice == 1)
			{
				// Подписка на событие
				OnProcess += (arr) =>
				{
					int result = CountElements(arr, x => x % 2 != 0);
					Console.WriteLine($"Количество нечётных элементов: {result}");
				};
			}
			else if (choice == 2)
			{
				OnProcess += (arr) =>
				{
					int result = SumElements(arr, x => x > 0);
					Console.WriteLine($"Сумма положительных элементов: {result}");
				};
			}
			else
			{
				throw new InvalidChoiceException("Ошибка: нужно ввести 1 или 2!");
			}

			// Вызов события
			OnProcess?.Invoke(vector);
		}
		catch (InvalidChoiceException ex)
		{
			Console.WriteLine($"Исключение: {ex.Message}");
		}
		finally
		{
			Console.WriteLine("Работа программы завершена.");
		}
	}
}
