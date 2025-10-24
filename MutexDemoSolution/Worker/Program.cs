using System;
using System.Threading;
using System.Diagnostics;

namespace Worker
{
	class Program
	{
		static void Main()
		{
			const string mutexName = "Global\\MyNamedMutexExample";
			bool createdNew;

			using (var mutex = new Mutex(false, mutexName, out createdNew))
			{
				int pid = Process.GetCurrentProcess().Id;
				Console.WriteLine($"[{pid}] ожидает доступ...");

				try
				{
					mutex.WaitOne();
					Console.WriteLine($"[{pid}] получил доступ, выполняю работу 5 сек...");
					Thread.Sleep(5000);
					Console.WriteLine($"[{pid}] завершил работу.");
				}
				finally
				{
					mutex.ReleaseMutex();
					Console.WriteLine($"[{pid}] освободил мьютекс.");
				}
			}
		}
	}
}
