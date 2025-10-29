public class Program
{
	private static int _maxStews; 
	private static int _currentStews; 

	private static SemaphoreSlim _potAccessSemaphore;

	private static SemaphoreSlim _cookWakeUpSemaphore;

	private static SemaphoreSlim _foodAvailableSemaphore;

	private static readonly object _consoleLock = new object();

	private static CancellationTokenSource _cts = new CancellationTokenSource();

	public static void Main(string[] args)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;

		Console.WriteLine("--- Начало обеда дикарей ---");

		int numSavages;
		while (true)
		{
			Console.Write("Введите количество дикарей (n): ");
			if (int.TryParse(Console.ReadLine(), out numSavages) && numSavages > 0)
			{
				break;
			}
			Console.WriteLine("Некорректный ввод. Введите положительное целое число.");
		}

		while (true)
		{
			Console.Write("Введите вместимость горшка (m): ");
			if (int.TryParse(Console.ReadLine(), out _maxStews) && _maxStews > 0)
			{
				break;
			}
			Console.WriteLine("Некорректный ввод. Введите положительное целое число.");
		}

		_currentStews = _maxStews; 


		_potAccessSemaphore = new SemaphoreSlim(1, 1); 
		_cookWakeUpSemaphore = new SemaphoreSlim(0, 1);
		_foodAvailableSemaphore = new SemaphoreSlim(_maxStews, _maxStews);

		List<Thread> savageThreads = new List<Thread>();
		for (int i = 0; i < numSavages; i++)
		{
			Thread savageThread = new Thread(() => SavageEat(i + 1, _cts.Token));
			savageThread.Name = $"Дикарь {i + 1}";
			savageThreads.Add(savageThread);
			savageThread.Start();
		}

		Thread cookThread = new Thread(() => CookFood(_cts.Token));
		cookThread.Name = "Повар";
		cookThread.Start();

		lock (_consoleLock)
		{
			Console.WriteLine("\n--- Начало обеда ---\n");
		}
		Console.ReadKey();

		_cts.Cancel();

		savageThreads.ForEach(t => t.Join());
		cookThread.Join();

		lock (_consoleLock)
		{
			Console.WriteLine("\nКонец работы");
		}
	}

	
	private static void SavageEat(int savageId, CancellationToken cancellationToken)
	{
		Random random = new Random(savageId * Environment.TickCount);

		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				_foodAvailableSemaphore.Wait(cancellationToken);
				_potAccessSemaphore.Wait(cancellationToken);

				_currentStews--;
				lock (_consoleLock)
				{
					Console.WriteLine($"{Thread.CurrentThread.Name}: Ем кусок. Осталось кусков: {_currentStews}");

					if (_currentStews == 0) 
					{
						Console.WriteLine($"{Thread.CurrentThread.Name}: Горшок пуст. Бужу повара!");
						_cookWakeUpSemaphore.Release(); 
					}
				}

				_potAccessSemaphore.Release(); 


				Thread.Sleep(random.Next(500, 1500));
			}
			catch (OperationCanceledException)
			{
				lock (_consoleLock)
				{
					Console.WriteLine($"{Thread.CurrentThread.Name}: Завершаю работу (отмена).");
				}
				break; 
			}
			catch (Exception ex)
			{
				lock (_consoleLock)
				{
					Console.WriteLine($" Ошибка в потоке {Thread.CurrentThread.Name}: {ex.Message}");
				}
			}
		}
	}

	private static void CookFood(CancellationToken cancellationToken)
	{
		Random random = new Random(Environment.TickCount + 100); 

		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				_cookWakeUpSemaphore.Wait(cancellationToken);

				lock (_consoleLock)
				{
					Console.WriteLine($"{Thread.CurrentThread.Name}: Меня разбудили. Готовлю еду...");
				}
				Thread.Sleep(random.Next(2000, 4000)); 

				_potAccessSemaphore.Wait(cancellationToken);
				_currentStews = _maxStews;
				lock (_consoleLock)
				{
					Console.WriteLine($"{Thread.CurrentThread.Name} : Наполнил горшок ");
				}
				_potAccessSemaphore.Release(); 

				_foodAvailableSemaphore.Release(_maxStews);

				lock (_consoleLock)
				{
					Console.WriteLine($"{Thread.CurrentThread.Name}: Засыпаю...");
				}
			}
			catch (OperationCanceledException)
			{
				lock (_consoleLock)
				{
					Console.WriteLine($"{Thread.CurrentThread.Name}: Завершаю работу (отмена).");
				}
				break; 
			}
			catch (Exception ex)
			{
				lock (_consoleLock)
				{
					Console.WriteLine($" Ошибка в потоке {Thread.CurrentThread.Name}: {ex.Message}");
				}
			}
		}
	}
}