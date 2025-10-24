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

	/// <summary>
	/// Метод, моделирующий поведение дикаря.
	/// </summary>
	/// <param name="savageId">Идентификатор дикаря.</param>
	/// <param name="cancellationToken">Токен отмены для контролируемого завершения.</param>
	private static void SavageEat(int savageId, CancellationToken cancellationToken)
	{
		Random random = new Random(savageId * Environment.TickCount); // Разный seed для каждого дикаря

		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				// Дикарь ждет, пока появится порция еды.
				// Если _foodAvailableSemaphore равен 0, дикарь будет ждать здесь.
				_foodAvailableSemaphore.Wait(cancellationToken);

				// Дикарь получил "разрешение" на еду, теперь нужно взять доступ к горшку
				_potAccessSemaphore.Wait(cancellationToken);

				// Теперь мы внутри критической секции (доступ к горшку).
				// В этом месте _currentStews не может быть 0,
				// потому что _foodAvailableSemaphore.Wait() уже прошел.
				_currentStews--;
				lock (_consoleLock)
				{
					Console.WriteLine($"{Thread.CurrentThread.Name}: Ем кусок. Осталось кусков: {_currentStews}");

					if (_currentStews == 0) 
					{
						Console.WriteLine($"{Thread.CurrentThread.Name}: Горшок пуст. Бужу повара!");
						_cookWakeUpSemaphore.Release(); // Разбудить повара
					}
				}

				_potAccessSemaphore.Release(); // Освобождаем горшок


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

	/// <summary>
	/// Метод, моделирующий поведение повара.
	/// </summary>
	/// <param name="cancellationToken">Токен отмены для контролируемого завершения.</param>
	private static void CookFood(CancellationToken cancellationToken)
	{
		Random random = new Random(Environment.TickCount + 100); // Разный seed

		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				// Повар ждет, пока его не разбудят дикари.
				// _cookWakeUpSemaphore.Wait() будет блокироваться, пока дикарь не вызовет Release().
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
					Console.WriteLine($"{Thread.CurrentThread.Name} : Наполнил горшок. Теперь ");
				}
				_potAccessSemaphore.Release(); 

				// Теперь, когда горшок наполнен, повар должен "разрешить" дикарям снова есть.
				// Он должен вызвать _foodAvailableSemaphore.Release() _maxStews_ раз,
				// чтобы все дикари, которые ждали на _foodAvailableSemaphore.Wait(), могли продолжить.
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