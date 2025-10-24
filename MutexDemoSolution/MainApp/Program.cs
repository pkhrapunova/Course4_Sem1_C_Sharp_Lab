using System;
using System.Diagnostics;
using System.IO;

namespace MainApp
{
	class Program
	{
		static void Main()
		{
			string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Worker.exe");

			if (!File.Exists(exePath))
			{
				Console.WriteLine("Файл Worker.exe не найден! Убедитесь, что он находится рядом с MainApp.exe");
				return;
			}

			Console.WriteLine("Запускаем 3 процесса Worker...");
			for (int i = 0; i < 3; i++)
			{
				Process.Start(exePath);
				Console.WriteLine($"Процесс #{i + 1} запущен.");
			}

			Console.ReadLine();
		}
	}
}
