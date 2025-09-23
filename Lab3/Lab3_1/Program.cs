using System;
//Задание 1. Определить и использовать делегат на методы. В качестве методов
//взять подсчет функций f и g с передачей параметров. Описать метод для подсчета
//значений функции z (по вариантам), в качестве func передать в  функцию либо функцию f
//(если с клавиатуры введена цифра 1) либо  функцию g (если с клавиатуры введена цифра 2).   
//17	z=func(5*x,y)-2*func(x,y-6);	f(x,y)=x+7y	g(x,y)=4x+6y
class Program
{
	delegate double MyDelegate(double x, double y);

	static double f(double x, double y) => x + 7 * y;
	static double g(double x, double y) => 4 * x + 6 * y;

	static double CalculateZ(MyDelegate func, double x, double y) =>
		func(5 * x, y) - 2 * func(x, y - 6);

	static double ReadDouble(string message)
	{
		while (true)
		{
			Console.Write(message);
			if (double.TryParse(Console.ReadLine(), out double value))
				return value;

			Console.WriteLine("Ошибка: нужно ввести число!");
		}
	}

	static void Main()
	{
		double x = ReadDouble("Введите x: ");
		double y = ReadDouble("Введите y: ");
		double choice;
		while (true)
		{
			choice = ReadDouble("Выберите функцию:\n1) f\n2) g \n");
			if (choice == 1 || choice == 2)
				break;

			Console.WriteLine("Ошибка: нужно ввести 1 или 2!");
		}


		MyDelegate func;

		if (choice == 1)
			func = f;
		else
			func = g;
		

		double z = CalculateZ(func, x, y);
		Console.WriteLine($"z = {z}");
	}
}
