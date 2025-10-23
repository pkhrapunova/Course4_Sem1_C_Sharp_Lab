using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class AsyncAwaitDemo
{
	public static async Task Main(string[] args)
	{
		Console.WriteLine($"[Main Thread: {Thread.CurrentThread.ManagedThreadId}] Приложение запущено. Начало асинхронных операций...");
		Stopwatch stopwatch = Stopwatch.StartNew();

		// Запуск нескольких асинхронных операций
		Task<string> dataTask = LoadDataAsync();
		Task<bool> processImageTask = ProcessImageAsync();
		Task<string> sendNotificationTask = SendNotificationAsync("Операции начаты!");

		// Можно выполнять какую-то другую работу здесь, пока асинхронные задачи выполняются
		Console.WriteLine($"[Main Thread: {Thread.CurrentThread.ManagedThreadId}] Основной поток продолжает работу, пока ждут асинхронные задачи...");
		await Task.Delay(500); // Имитация какой-то короткой работы в основном потоке

		// Ожидание завершения всех задач и получение их результатов
		string loadedData = await dataTask;
		bool imageProcessed = await processImageTask;
		string notificationResult = await sendNotificationTask;

		Console.WriteLine($"[Main Thread: {Thread.CurrentThread.ManagedThreadId}] Все асинхронные операции завершены.");
		Console.WriteLine($"[Main Thread: {Thread.CurrentThread.ManagedThreadId}] Результаты:");
		Console.WriteLine($"- Загруженные данные: {loadedData}");
		Console.WriteLine($"- Изображение обработано: {imageProcessed}");
		Console.WriteLine($"- Результат уведомления: {notificationResult}");

		stopwatch.Stop();
		Console.WriteLine($"[Main Thread: {Thread.CurrentThread.ManagedThreadId}] Общее время выполнения: {stopwatch.ElapsedMilliseconds} мс.");
		Console.WriteLine($"[Main Thread: {Thread.CurrentThread.ManagedThreadId}] Приложение завершено.");
	}

	// Асинхронный метод для имитации загрузки данных
	static async Task<string> LoadDataAsync()
	{
		Console.WriteLine($"[Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}] Начинается загрузка данных...");
		await Task.Delay(2000); // Имитация длительной операции ввода/вывода (например, сетевой запрос)
		Console.WriteLine($"[Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}] Загрузка данных завершена.");
		return "Некоторые данные из базы данных.";
	}

	// Асинхронный метод для имитации обработки изображения
	static async Task<bool> ProcessImageAsync()
	{
		Console.WriteLine($"[Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}] Начинается обработка изображения...");
		await Task.Delay(3000); // Имитация длительной операции вычислений
		Console.WriteLine($"[Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}] Обработка изображения завершена.");
		return true;
	}

	// Асинхронный метод для имитации отправки уведомления
	static async Task<string> SendNotificationAsync(string message)
	{
		Console.WriteLine($"[Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}] Начинается отправка уведомления: '{message}'...");
		await Task.Delay(1500); // Имитация короткой сетевой операции
		Console.WriteLine($"[Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}] Уведомление отправлено.");
		return "Уведомление успешно отправлено.";
	}
}