using System;
/*Задание 2. Дан вектор. Описать необходимые методы (по вариантам, см. ниже), которые
 * в качестве параметров должны получать массив чисел и делегат, задающий условие для
 * значений массива через лямбда-выражение. В зависимости от ввода с клавиатуры вызвать
 * либо первый метод (если с клавиатуры введена цифра 1) либо  второй метод (если с клавиатуры введена 2.)
 1)Подсчет количества нечетных элементов вектора	2)Подсчет суммы положительных элементов вектора*/
class Program
{
	delegate bool Condition(int value);

	static int CountElements(int[] array, Condition cond)
	{
		int count = 0;
		foreach (int elem in array)
		{
			if (cond(elem)) 
				count++;
		}
		return count;
	}

	static int SumElements(int[] array, Condition cond)
	{
		int sum = 0;
		foreach (int elem in array)
		{
			if (cond(elem))
				sum += elem;
		}
		return sum;
	}
	static int ReadChoice(string message)
	{
		while (true)
		{
			Console.Write(message);
			if (int.TryParse(Console.ReadLine(), out int value) && (value == 1 || value == 2))
				return value;

			Console.WriteLine("Ошибка: нужно ввести 1 или 2!");
		}
	}
	static void Main()
	{
		int n = InputInt("Введите размер вектора: ");
		int[] vector = new int[n];
		InputVector(ref vector);
		Console.WriteLine(string.Join(", ", vector));
		Console.WriteLine("Выберите метод: ");
		int choice = ReadChoice("1 - Подсчет количества нечетных элементов \n2 - Подсчет суммы положительных элементов\n");

		if (choice == 1)
		{
			int result = CountElements(vector, x => x % 2 != 0);
			Console.WriteLine($"Количество нечётных элементов: {result}");
		}
		else
		{
			int result = SumElements(vector, x => x > 0);
			Console.WriteLine($"Сумма положительных элементов: {result}");
		}
	}

	static int InputInt(string prompt)
	{
		int value;
		while (true)
		{
			Console.Write(prompt);
			if (int.TryParse(Console.ReadLine(), out value))
				return value;
			Console.WriteLine("Ошибка! Введите целое число.");
		}
	}
	static void InputVector(ref int[] vector)
	{
		while (true)
		{
			Console.WriteLine($"Введите {vector.Length} элементов через пробел:");
			string line = Console.ReadLine();
			string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length != vector.Length)
			{
				Console.WriteLine("Количество введенных элементов не совпадает с размером вектора. Попробуйте снова.");
				continue;
			}

			try
			{
				for (int i = 0; i < vector.Length; i++)
					vector[i] = int.Parse(parts[i]);
				break;
			}
			catch
			{
				Console.WriteLine("Ошибка при вводе. Используйте целые числа.");
			}
		}
	}
}
