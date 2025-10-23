using System;
using System.Threading;
using System.Collections.Generic;

public class SavageFeast
{
	// Количество дикарей
	private const int NumSavages = 5;
	// Вместимость горшка
	private const int PotCapacity = 10;
	// Количество еды, которую готовит повар
	private const int CookBatchSize = 10;

	// Семафор для доступа к горшку
	private static SemaphoreSlim _potMutex = new SemaphoreSlim(1, 1);
	// Семафор для дикарей, чтобы ждать еду
	private static SemaphoreSlim _savageWaiting = new SemaphoreSlim(0);
	// Семафор для повара, чтобы спать
	private static SemaphoreSlim _cookSleeping = new SemaphoreSlim(0);

	// Глобальная переменная для отслеживания еды в горшке
	private static int _foodInPot = 0;

	public static void Main(string[] args)
	{
		Console.WriteLine("Начало обеда дикарей...");

		// Запуск потока повара
		Thread cookThread = new Thread(Cook);
		cookThread.Start();

		// Запуск потоков дикарей
		List<Thread> savageThreads = new List<Thread>();
		for (int i = 0; i < NumSavages; i++)
		{
			int savageId = i + 1;
			Thread savageThread = new Thread(() => Savage(savageId));
			savageThreads.Add(savageThread);
			savageThread.Start();
		}

		// Ждем завершения (в данном случае бесконечный цикл)
		cookThread.Join();
		foreach (var thread in savageThreads)
		{
			thread.Join();
		}

		Console.WriteLine("Обед дикарей завершен.");
	}

	private static void Cook()
	{
		while (true)
		{
			// Повар спит, пока его не разбудят
			_cookSleeping.Wait();
			Console.WriteLine("\nПовар проснулся и начинает готовить...");
			// Имитация процесса приготовления
			Thread.Sleep(TimeSpan.FromSeconds(new Random().NextDouble() * 2 + 1)); // От 1 до 3 секунд

			_potMutex.Wait(); // Захватываем мьютекс горшка
			try
			{
				_foodInPot = CookBatchSize;
				Console.WriteLine($"Повар наполнил горшок. Теперь в горшке {_foodInPot} кусков.");
			}
			finally
			{
				_potMutex.Release(); // Освобождаем мьютекс горшка
			}

			// Разбудить всех дикарей, которые ждали
			// Внимание: Release() может вызвать исключение, если значение больше MaxCount
			// Однако, поскольку savageWaiting.Wait() уменьшает его, здесь это безопасно.
			// Можно использовать цикл для Release, чтобы избежать превышения MaxCount, если MaxCount ограничен.
			// Для SemaphoreSlim это не так критично, как для старого Semaphore.
			_savageWaiting.Release(NumSavages);
			Console.WriteLine("Повар приготовил еду и заснул.");
		}
	}

	private static void Savage(int savageId)
	{
		while (true)
		{
			Console.WriteLine($"Дикарь {savageId} хочет есть.");

			bool ate = false;
			while (!ate)
			{
				_potMutex.Wait(); // Захватываем мьютекс горшка
				try
				{
					if (_foodInPot == 0)
					{
						Console.WriteLine($"Дикарь {savageId} обнаружил, что горшок пуст. Будит повара и ждет.");
						// Разбудить повара
						_cookSleeping.Release();
						// Дикарь отпускает мьютекс и ждет еду
						_potMutex.Release();
						_savageWaiting.Wait(); // Ждет, пока повар не приготовит еду
					}
					else
					{
						_foodInPot--;
						Console.WriteLine($"Дикарь {savageId} съел кусок. Осталось {_foodInPot} кусков.");
						ate = true;
						// Имитация процесса еды
						Thread.Sleep(TimeSpan.FromSeconds(new Random().NextDouble() * 0.5 + 0.1)); // От 0.1 до 0.6 секунд
					}
				}
				finally
				{
					if (!ate) // Если не ел, то мьютекс должен быть освобожден в этом блоке
					{
						// Если дикарь ждал, он уже отпустил мьютекс перед _savageWaiting.Wait()
						// Если он все еще в цикле проверки (т.е. _foodInPot был не 0, но потом стал 0 до его еды),
						// то мьютекс должен быть освобожден.
						// Этот finally блок гарантирует, что мьютекс освобождается, если он был захвачен.
						// В случае, если дикарь ждет повара, он уже освобождает мьютекс явно.
						// Поэтому здесь нужно быть осторожным, чтобы не освободить его дважды.
						// Простая проверка состояния мьютекса невозможна для SemaphoreSlim.
						// Лучший подход - гарантировать, что Release() вызывается только один раз на Wait().
						// В текущей логике, если _foodInPot == 0, мы освобождаем мьютекс до _savageWaiting.Wait().
						// Если _foodInPot > 0, мы его освобождаем после еды.
						// Это выглядит корректно.
					}
				}
				if (ate)
				{
					_potMutex.Release(); // Освобождаем мьютекс горшка после еды
				}
			}
			Thread.Sleep(TimeSpan.FromSeconds(new Random().NextDouble() * 1 + 0.5)); // От 0.5 до 1.5 секунд до следующего голода
		}
	}
}