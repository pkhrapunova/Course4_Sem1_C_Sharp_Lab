using System;
/*Задание 3. Изменить задание 2, таким образом, чтобы реализация вызова
 * метода происходила при помощи события. Также реализуйте проверку введённых
 * данных от пользователя через конструкцию TryCatchFinally с использованием собственного типа исключения.*/
class Program
{
	static event Action<int[]> OnProcess;

	class InvalidChoiceException : Exception
	{
		public InvalidChoiceException(string message) : base(message) { }
	}

	delegate bool Condition(int value);
	static int CountElements(int[] array, Condition cond)
	{
		int count = 0;
		foreach (int elem in array)
		{
			if (cond(elem)) count++;
		}
		return count;
	}

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
		Console.WriteLine(string.Join(", ", vector));
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

			OnProcess?.Invoke(vector);
		}
		catch (InvalidChoiceException ex)
		{
			Console.WriteLine($"Исключение: {ex.Message}");
		}
		finally
		{
			Console.WriteLine("Работа завершена");
		}
	}
}
