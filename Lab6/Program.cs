using System;
using System.Windows.Forms;

namespace CarRental.UI
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Глобальная обработка исключений
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += (s, e) =>
				ShowError("Необработанная ошибка приложения", e.Exception);
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
				ShowError("Критическая ошибка домена приложения", e.ExceptionObject as Exception);

			try
			{
				Application.Run(new MainForm());
			}
			catch (Exception ex)
			{
				ShowError("Критическая ошибка запуска приложения", ex);
			}
		}

		static void ShowError(string title, Exception ex)
		{
			string message = $"{title}:\n\n{ex?.Message}";

			if (ex?.InnerException != null)
			{
				message += $"\n\nВнутренняя ошибка: {ex.InnerException.Message}";
			}

			MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

			// Логирование ошибки (можно добавить запись в файл)
			System.Diagnostics.Debug.WriteLine($"[ERROR] {DateTime.Now}: {title} - {ex}");
		}
	}
}