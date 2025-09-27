// Файл: C#_многопоточность_async_mutex_примеры.cs
// Содержит 3 примера:
// 1) Dining Savages (задача дикарей) — многопоточность с семафорами
// 2) Async/Await demo — демонстрация async и await
// 3) Named Mutex demo — именованный мьютекс для синхронизации между процессами

// Чтобы запустить отдельно: создайте проект Console (dotnet new console), замените Program.cs на нужный класс/метод Main
// или используйте csc для компиляции отдельных файлов.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingExamples
{
	// --------------------
	// 1) Dining Savages (дикари и повар)
	// --------------------
	// Решение использует семафоры:
	// - "servings" (счетный семафор) — количество кусочков в горшке;
	// - "mutex" (бинарный семафор) — защита доступа к горшку при проверке/снятии порции;
	// - "emptyPot" (семафор/событие) — дикарь сигналит повару, что горшок пуст и нужно наполнить.

	public class DiningSavages
	{
		private readonly int m; // емкость горшка
		private SemaphoreSlim servings; // показывает, сколько кусков доступно
		private SemaphoreSlim mutex = new SemaphoreSlim(1, 1); // защищает доступ к горшку
		private AutoResetEvent emptyPot = new AutoResetEvent(false); // сигнал повару
		private volatile bool stopRequested = false;

		public DiningSavages(int m)
		{
			this.m = m;
			servings = new SemaphoreSlim(0, m);
		}

		// Дикарь: ест одну порцию, если пуст — будит повара и ждёт
		public void Savage(object idObj)
		{
			int id = (int)idObj;
			Random rnd = new Random(id * 1000 + Environment.TickCount);
			while (!stopRequested)
			{
				// попытка взять порцию
				mutex.Wait();
				if (servings.CurrentCount == 0)
				{
					Console.WriteLine($"Дикарь {id}: горшок пуст — будит повара.");
					// сигнал повару заполнить горшок
					emptyPot.Set();
					mutex.Release();
					// ждём пока повар наполнит — ожидание по семафору servings
					// Попробуем затем снова взять порцию (обычно cook заполняет m порций)
					servings.Wait(); // дождёмся первой порции
				}
				else
				{
					// есть порции — уменьшаем счётчик
					servings.Wait();
					mutex.Release();
				}

				Console.WriteLine($"Дикарь {id} ест кусок (осталось {servings.CurrentCount}).");
				Thread.Sleep(rnd.Next(200, 700)); // имитация еды
			}
			Console.WriteLine($"Дикарь {id} завершил работу.");
		}

		// Повар: просыпается при сигнале и наполняет горшок до m порций
		public void Cook()
		{
			while (!stopRequested)
			{
				// ждём сигнала о пустом горшке
				emptyPot.WaitOne();
				if (stopRequested) break;
				Console.WriteLine("Повар: проснулся и начинает готовить...");
				Thread.Sleep(500); // имитация готовки
								   // заполнить горшок: добавить m порций
				for (int i = 0; i < m; i++)
				{
					servings.Release();
				}
				Console.WriteLine($"Повар: горшок наполнен ({m} кусков).");
			}
			Console.WriteLine("Повар завершил работу.");
		}

		// Запуск примера
		public static void RunExample()
		{
			int n = 5; // количество дикарей
			int m = 3; // вместимость горшка
			var app = new DiningSavages(m);

			// запускаем повара
			Thread cook = new Thread(app.Cook) { IsBackground = true };
			cook.Start();

			// запускаем дикарей
			List<Thread> savages = new List<Thread>();
			for (int i = 0; i < n; i++)
			{
				Thread t = new Thread(app.Savage) { IsBackground = true };
				savages.Add(t);
				t.Start(i + 1);
			}

			Console.WriteLine("Нажмите любую клавишу для остановки...");
			Console.ReadKey();

			// останов
			app.stopRequested = true;
			// разблокировать повара и дикарей, чтобы они завершились
			app.emptyPot.Set();
			app.servings.Release(n * m); // на случай, если кто-то застрял в ожидании

			foreach (var t in savages) t.Join(1000);
			cook.Join(1000);

			Console.WriteLine("Пример 'Дикари и повар' завершён.");
		}
	}

	// --------------------
	// 2) Async / Await demo
	// --------------------
	public class AsyncAwaitDemo
	{
		// Простой async метод, симулирующий асинхронную работу
		public static async Task<int> DoWorkAsync(int id)
		{
			Console.WriteLine($"Task {id}: начало работы на потоке {Thread.CurrentThread.ManagedThreadId}.");
			// await имитирует асинхронную операцию (например IO)
			await Task.Delay(500 + id * 200);
			Console.WriteLine($"Task {id}: окончание работы.");
			return id * 10;
		}

		// Демонстрация запуска нескольких асинхронных задач и ожидания результатов
		public static async Task RunAsyncDemo()
		{
			Console.WriteLine("Запуск асинхронной демонстрации...");
			var tasks = new List<Task<int>>();
			for (int i = 1; i <= 4; i++)
			{
				tasks.Add(DoWorkAsync(i));
			}

			// Await на несколько задач одновременно
			int[] results = await Task.WhenAll(tasks);
			Console.WriteLine("Все задачи завершены. Результаты: " + string.Join(", ", results));

			// Демонстрация async void (только для событий), здесь показывается, почему его лучше избегать
			Console.WriteLine("Демонстрация async void (не рекомендуется для общей логики)...");
			FireAndForget();
			await Task.Delay(300); // даём времени async void показать вывод
		}

		private static async void FireAndForget()
		{
			await Task.Delay(200);
			Console.WriteLine("async void выполнился (FireAndForget)");
		}

		public static void RunExample()
		{
			// Запускаем синхронно async метод по правилу: Task.Run + Wait или .GetAwaiter().GetResult()
			RunAsyncDemo().GetAwaiter().GetResult();
			Console.WriteLine("Async/await пример завершён.");
		}
	}

	// --------------------
	// 3) Named Mutex demo (межпроцессная синхронизация) — вариант для 1,5,9,13,17,21,25,29
	// --------------------
	// Программа демонстрирует экстремально простую работу с именованным мьютексом:
	// -- один процесс может "захватить" mutex с именем и держать его, остальные процессы ждут
	// Чтобы протестировать: скомпилируйте эту программу в отдельный exe и запустите несколько копий.

	public class NamedMutexDemo
	{
		private const string MutexName = "Global\\MyUniqueNamedMutex_Demo_12345"; // Global: виден всем сессиям

		public static void RunExample()
		{
			Console.WriteLine($"Попытка открыть/создать именованный мьютекс: '{MutexName}'");
			bool createdNew = false;
			using (var mutex = new Mutex(false, MutexName, out createdNew))
			{
				Console.WriteLine(createdNew ? "Мьютекс создан (первый процесс)." : "Мьютекс уже существует (другой процесс держит его или уже создавал).");
				Console.WriteLine("Попытка захватить мьютекс (ждать до 10 секунд)...");
				try
				{
					if (mutex.WaitOne(TimeSpan.FromSeconds(10)))
					{
						Console.WriteLine("Мьютекс захвачен — выполняется критическая секция.");
						Console.WriteLine("Удерживаем мьютекс 8 секунд, чтобы другие процессы могли ждать...");
						Thread.Sleep(8000);
						Console.WriteLine("Критическая секция завершена, освобождаю мьютекс.");
						mutex.ReleaseMutex();
					}
					else
					{
						Console.WriteLine("Не удалось захватить мьютекс за отведённое время.");
					}
				}
				catch (AbandonedMutexException)
				{
					Console.WriteLine("Обнаружен abandoned mutex — предыдущий владелец аварийно завершил работу. Можно продолжить, но будьте внимательны.");
				}
			}
			Console.WriteLine("RunExample завершён.");
		}
	}

	// --------------------
	// Удобный Program с меню запуска нужного примера
	// --------------------
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine("Выберите пример для запуска:");
			Console.WriteLine("1 - Дикари и повар (семафоры)");
			Console.WriteLine("2 - Async/Await demo");
			Console.WriteLine("3 - Named Mutex demo (межпроцессный мьютекс)");
			Console.Write("Ваш выбор (1/2/3): ");
			var key = Console.ReadKey();
			Console.WriteLine();
			switch (key.KeyChar)
			{
				case '1': DiningSavages.RunExample(); break;
				case '2': AsyncAwaitDemo.RunExample(); break;
				case '3': NamedMutexDemo.RunExample(); break;
				default: Console.WriteLine("Неверный выбор."); break;
			}

			Console.WriteLine("Нажмите любую клавишу для выхода...");
			Console.ReadKey();
		}
	}
}
