using System;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			int result = await GetNumberAsync();
			Console.WriteLine($"Результат из GetNumberAsync: {result}\n");

			await DoWorkAsync();

			string message = await GetMessageAsync("Асинхронный метод");
			Console.WriteLine(message);

			Console.WriteLine("\nЗапуск нескольких задач параллельно:");
			Task<int> t1 = GetNumberAfterDelayAsync(1);
			Task<int> t2 = GetNumberAfterDelayAsync(2);
			Task<int> t3 = GetNumberAfterDelayAsync(3);

			int[] results = await Task.WhenAll(t1, t2, t3);
			Console.WriteLine($"Сумма результатов: {results[0] + results[1] + results[2]}");
		}

		static async Task<int> GetNumberAsync()
		{
			Console.WriteLine("Начало GetNumberAsync...");
			await Task.Delay(1000); 
			Console.WriteLine("Окончание GetNumberAsync.");
			return 42;
		}

		static async Task DoWorkAsync()
		{
			Console.WriteLine("Начало DoWorkAsync...");
			await Task.Delay(1500);
			Console.WriteLine("Окончание DoWorkAsync.\n");
		}

		static async Task<string> GetMessageAsync(string text)
		{
			await Task.Delay(1000);
			return $"Сообщение получено: {text}";
		}

		static async Task<int> GetNumberAfterDelayAsync(int number)
		{
			await Task.Delay(1000 * number);
			Console.WriteLine($"GetNumberAfterDelayAsync({number}) завершён.");
			return number * 10;
		}
	}
}
