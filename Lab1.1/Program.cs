using System;

namespace Lab1._1
{
    internal class Program
    {
        static void Main()
        {
            ReadRange("x","h", out double x_n, out double x_k, out double h);
            ReadRange("y", "t", out double y_n, out double y_k, out double t);


            Console.WriteLine("\n|{0,8}|{1,8}|{2,15}|{3,15}|", "x", "y", "f(x,y)", "g(x,y)");
            Console.WriteLine(new string('-', 52));


            for (double x = x_n; (h > 0) ? x <= x_k : x >= x_k; x += h)
            {
                for (double y = y_n; (t > 0) ? y <= y_k : y >= y_k; y += t)
                {
                    string fStr, gStr;

                    try
                    {
                        fStr = F(x, y).ToString("F5");
                    }
                    catch (Exception ex)
                    {
                        fStr = "Ошибка";
                        Console.WriteLine($"   -> F({x},{y}) ошибка: {ex.Message}");
                    }

                    try
                    {
                        gStr = G(x, y).ToString("F5");
                    }
                    catch (Exception ex)
                    {
                        gStr = "Ошибка";
                        Console.WriteLine($"   -> G({x},{y}) ошибка: {ex.Message}");
                    }

                    Console.WriteLine("|{0,8:F2}|{1,8:F2}|{2,15}|{3,15}|", x, y, fStr, gStr);
                }
            }

        }
        static void ReadRange(string name,string step_name, out double start, out double end, out double step)
        {
            start = ReadDouble($"Введите {name}_n: ");
            end = ReadDouble($"Введите {name}_k: ");

            while (true)
            {
                step = ReadDouble($"Введите шаг {step_name}: ");
                if ((end > start && step > 0) || (end < start && step < 0))
                    break;
                Console.WriteLine($"Ошибка: шаг {name} не соответствует направлению от {name}_n к {name}_k. Введите снова.");
            }
        }

        // Метод для безопасного ввода числа
        static double ReadDouble(string message)
        {
            double value;
            while (true)
            {
                Console.Write(message);
                if (double.TryParse(Console.ReadLine(), out value))
                    return value;
                else
                    Console.WriteLine("Ошибка: введите число ещё раз!");
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
                double value = Math.Abs(Math.Sin(x)) + xy;
                if (value < 0)
                    throw new Exception("Подкоренное выражение < 0 в функции f.");

                return Math.Sqrt(value);
            }
            else
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
                double denominator = Math.Sqrt(Math.Abs(2 * x * x + y));
                if (denominator == 0)
                    throw new DivideByZeroException("Знаменатель равен нулю в функции g.");

                return (x * x - y * y + 3 * x) / denominator;
            }
            else if (xy > 0)
            {
                return Math.Pow(x, 3) + y * y;
            }
            else
            {
                if (xy == 0)
                    throw new Exception("Функция g не определена при ln(0).");

                return (Math.Sin(x * x) + Math.Pow(y - x, 2)) / (25 + Math.Log(xy));
            }
        }
    }

}
