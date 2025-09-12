using System;
/*Вариант 17
1.Дан целочисленный вектор A(n). Найти значение максимального элемента среди элементов, кратных k и расположенных до первого отрицательного элемента.
2.Дана квадратная матрица A(nхn). Построить вектор b, где bi, i=1,…,n – минимальный элемент i-й строки матрицы, среди элементов, стоящих в нечетных столбцах.
3.Дана квадратная матрица A(nхn). Найти минимальную из сумм элементов, расположенных параллельно побочной диагонали.*/

namespace Lab1._2
{
	internal class Program
	{
		static void Main()
		{
			while (true)
			{
				Console.Clear();
				Console.WriteLine("1. Вектор 1");
				Console.WriteLine("2. Матрица 1");
				Console.WriteLine("3. Матрица 2");
				Console.WriteLine("0. Выход");
				Console.Write("Выберите пункт меню: ");

				string choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						HandleVector1();
						break;
					case "2":
						HandleMatrixMinOddColumns();
						break;
					case "3":
						HandleMatrixMinAntiDiagonal();
						break;
					case "0":
						return;
					default:
						Console.WriteLine("Неверный выбор! Нажмите любую клавишу для продолжения...");
						Console.ReadKey();
						break;
				}
			}
		}

		#region Вектор 1
		static void HandleVector1()
		{
			try
			{
				int n = InputInt("Введите размер вектора: ");
				int[] vector = new int[n];
				InputVector(ref vector);

				int k = InputInt("Введите значение k: ");
				if (FindMaxDivisibleBeforeNegative(vector, k, out int result))
				{
					Console.WriteLine($"Максимальный элемент, кратный {k} до первого отрицательного: {result}");
				}
				else
				{
					Console.WriteLine("Элемент, удовлетворяющий условию, не найден.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
			Console.WriteLine("Нажмите любую клавишу для продолжения...");
			Console.ReadKey();
		}

		static bool FindMaxDivisibleBeforeNegative(int[] a, int k, out int max)
		{
			max = int.MinValue;
			bool found = false;
			foreach (var val in a)
			{
				if (val < 0) break;
				if (val % k == 0)
				{
					if (!found || val > max)
						max = val;
					found = true;
				}
			}
			return found;
		}
		#endregion

		#region Матрица: вектор минимальных элементов по нечетным столбцам
		static void HandleMatrixMinOddColumns()
		{
			try
			{
				int n = InputInt("Введите размер матрицы n: ");
				int[,] matrix = new int[n, n];
				InputMatrix(matrix);
				OutputMatrix(matrix);
				int[] result = MinInOddColumns(matrix);
				Console.WriteLine("Минимальные элементы по нечетным столбцам каждой строки:");
				OutputVector(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
			Console.WriteLine("Нажмите любую клавишу для продолжения...");
			Console.ReadKey();
		}

		static int[] MinInOddColumns(int[,] matrix)
		{
			int n = matrix.GetLength(0);
			int[] result = new int[n];
			for (int i = 0; i < n; i++)
			{
				int min = int.MaxValue;
				for (int j = 1; j < n; j += 2) 
				{
					if (matrix[i, j] < min)
						min = matrix[i, j];
				}
				result[i] = min;
			}
			return result;
		}
		#endregion

		#region Матрица: минимальная сумма параллельная побочной диагонали
		static void HandleMatrixMinAntiDiagonal()
		{
			try
			{
				int n = InputInt("Введите размер матрицы n: ");
				int[,] matrix = new int[n, n];
				InputMatrix(matrix);
				OutputMatrix(matrix);
				int minSum = MinSumParallelAntiDiagonal(matrix);
				Console.WriteLine($"Минимальная сумма элементов, расположенных параллельно побочной диагонали: {minSum}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
			Console.WriteLine("Нажмите любую клавишу для продолжения...");
			Console.ReadKey();
		}


		static int MinSumParallelAntiDiagonal(int[,] matrix)
		{
			int n = matrix.GetLength(0);
			int minSum = int.MaxValue;


			for (int diag = 1 - n; diag <= n - 1; diag++)
			{
				if (diag == 0) continue;
				int sum = 0;
				for (int i = 0; i < n; i++)
				{
					int j = n - 1 - i - diag;
					if (j >= 0 && j < n)
						sum += matrix[i, j];
				}
				if (sum < minSum)
					minSum = sum;
			}
			return minSum;
		}
		#endregion

		#region Методы ввода/вывода
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


		static void OutputVector(int[] vector)
		{
			Console.WriteLine(string.Join(" ", vector));
		}

		static void InputMatrix(int[,] matrix)
		{
			int n = matrix.GetLength(0);
			for (int i = 0; i < n; i++)
			{
				while (true)
				{
					Console.WriteLine($"Введите {n} элементов {i + 1}-й строки через пробел:");
					string line = Console.ReadLine();
					string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

					if (parts.Length != n)
					{
						Console.WriteLine("Ошибка! Количество элементов не совпадает с размером строки. Попробуйте снова.");
						continue;
					}

					try
					{
						for (int j = 0; j < n; j++)
							matrix[i, j] = int.Parse(parts[j]);
						break;
					}
					catch
					{
						Console.WriteLine("Ошибка! Введите только целые числа.");
					}
				}
			}
		}


		static void OutputMatrix(int[,] matrix)
		{
			int n = matrix.GetLength(0);
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					Console.Write($"{matrix[i, j],5}");
				}
				Console.WriteLine();
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
		#endregion
	}
}
