using System;

namespace Lab1._1
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                Console.Write("Введите xn: ");
                double xn = double.Parse(Console.ReadLine());

                Console.Write("Введите xk: ");
                double xk = double.Parse(Console.ReadLine());

                Console.Write("Введите шаг h: ");
                double h = double.Parse(Console.ReadLine());

                Console.Write("Введите yn: ");
                double yn = double.Parse(Console.ReadLine());

                Console.Write("Введите yk: ");
                double yk = double.Parse(Console.ReadLine());

                Console.Write("Введите шаг t: ");
                double t = double.Parse(Console.ReadLine());

                Console.WriteLine("\n{0,8}{1,8}{2,15}{3,15}", "x", "y", "f(x,y)", "g(x,y)");
                Console.WriteLine(new string('-', 50));

                for (double x = xn; x <= xk; x += h)
                {
                    for (double y = yn; y <= yk; y += t)
                    {
                        try
                        {
                            double fVal = F(x, y);
                            double gVal = G(x, y);

                            Console.WriteLine("{0,8:F2}{1,8:F2}{2,15:F5}{3,15:F5}", x, y, fVal, gVal);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("{0,8:F2}{1,8:F2}{2,15}{3,15}", x, y, "Ошибка", "Ошибка");
                            Console.WriteLine("   -> Причина: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка ввода: " + ex.Message);
            }
        }

        static double F(double x, double y)
        {
            double xy = x * y;

            if (xy < 0)
            {
                double denominator = 1 + y * y - x;
                if (denominator == 0)
                    throw new DivideByZeroException("Знаменатель равен нулю в функции f.");

                return (Math.Log(Math.Abs(xy)) + 1) / denominator;
            }
            else if (xy > 0)
            {
                double value = Math.Sin(x) + xy;
                if (value < 0)
                    throw new Exception("Подкоренное выражение < 0 в функции f.");

                return Math.Sqrt(value);
            }
            else // xy == 0
            {
                double denominator = 1 - x * x + Math.Pow(y, 3);
                if (denominator == 0)
                    throw new DivideByZeroException("Знаменатель равен нулю в функции f.");

                return (x - y) / denominator;
            }
        }

        static double G(double x, double y)
        {
            double xy = x * y;

            if (xy < 0)
            {
                double denominator = Math.Sqrt(2 * x * x + y);
                if (denominator == 0)
                    throw new DivideByZeroException("Знаменатель равен нулю в функции g.");

                return (x * x - y * y + 3 * x) / denominator;
            }
            else if (xy > 0)
            {
                return x * x * x + y * y;
            }
            else // xy == 0
            {
                if (xy == 0)
                    throw new Exception("Функция g не определена при ln(0).");

                return (Math.Sin(x * x) + Math.Pow(y - x, 2)) / (25 + Math.Log(xy));
            }
        }
    }

}
