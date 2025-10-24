using System;
using System.Threading;

class Program
{
	static void Main()
	{
		// Имя мьютекса (измените при желании).
		// Рекомендуется использовать префикс "Global\" если нужно между сессиями.
		const string mutexName = @"Global\MyNamedMutexExample";

		// Попытаемся создать/открыть именованный мьютекс
		bool createdNew;
		using (var mutex = new Mutex(false, mutexName, out createdNew))
		{
			Console.WriteLine($"[{ProcessId()}] Мьютекс: {(createdNew ? "создан" : "открыт (существовал)")}. Имя = {mutexName}");

			Console.WriteLine($"[{ProcessId()}] Попытка захватить мьютекс...");
			// Ждём разрешения входа (таймаут для примера — 30 секунд)
			const int waitMs = 30000;
			bool entered = false;
			try
			{
				entered = mutex.WaitOne(waitMs);
			}
			catch (AbandonedMutexException)
			{
				// Если предыдущий владелец завершился без ReleaseMutex, мы всё равно получаем владение.
				Console.WriteLine($"[{ProcessId()}] Обнаружен AbandonedMutex — получили владение.");
				entered = true;
			}

			if (!entered)
			{
				Console.WriteLine($"[{ProcessId()}] Не удалось получить мьютекс за {waitMs} ms. Выходим.");
				return;
			}

			// Критическая секция
			try
			{
				Console.WriteLine($"[{ProcessId()}] Захватили мьютекс — выполняем работу (5s)...");
				// Здесь имитируем некоторую работу, которую нельзя параллелить между процессами
				Thread.Sleep(5000);
				Console.WriteLine($"[{ProcessId()}] Работа завершена, передаём мьютекс дальше.");
			}
			finally
			{
				// Обязательно освобождаем мьютекс
				try
				{
					mutex.ReleaseMutex();
					Console.WriteLine($"[{ProcessId()}] Мьютекс освобождён.");
				}
				catch (ApplicationException ex)
				{
					// ReleaseMutex бросит, если текущий поток не владеет мьютексом
					Console.WriteLine($"[{ProcessId()}] Ошибка при ReleaseMutex: {ex.Message}");
				}
			}
		}
	}

	static int ProcessId()
	{
		return System.Diagnostics.Process.GetCurrentProcess().Id;
	}
}
