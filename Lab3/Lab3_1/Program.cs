using System;
//Задание 1. Определить и использовать делегат на методы. В качестве методов
//взять подсчет функций f и g с передачей параметров. Описать метод для подсчета
//значений функции z (по вариантам), в качестве func передать в  функцию либо функцию f
//(если с клавиатуры введена цифра 1) либо  функцию g (если с клавиатуры введена цифра 2).   
//17	z=func(5*x,y)-2*func(x,y-6);	f(x,y)=x+7y	g(x,y)=4x+6y
class Program
{
	delegate int MyDelegate(int x, int y);

	static int f(int x, int y)
	{
		return x + 7 * y;
	}

	static int g(int x, int y)
	{
		return 4 * x + 6 * y;
	}

	static int CalculateZ(MyDelegate func, int x, int y)
	{
		return func(5 * x, y) - 2 * func(x, y - 6);
	}

	static void Main()
	{
		Console.Write("Введите x: ");
		int x = int.Parse(Console.ReadLine());

		Console.Write("Введите y: ");
		int y = int.Parse(Console.ReadLine());

		Console.Write("Выберите функцию (1 - f, 2 - g): ");
		int choice = int.Parse(Console.ReadLine());

		MyDelegate func;

		if (choice == 1)
			func = f;
		else if (choice == 2)
			func = g;
		else
		{
			Console.WriteLine("Ошибка: нужно ввести 1 или 2!");
			return;
		}

		int z = CalculateZ(func, x, y);
		Console.WriteLine($"z = {z}");
	}
}
