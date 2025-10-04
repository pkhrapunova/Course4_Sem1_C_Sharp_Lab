using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CarRental.Models;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace CarRental.UI
{
	public class WordReportService
	{
		public void GenerateAllOrdersReport(List<OrderViewModel> orders, string filePath)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);

			Word.Application wordApp = null;
			Word.Document document = null;

			try
			{
				wordApp = new Word.Application();
				wordApp.Visible = false;

				document = wordApp.Documents.Add();

				// Заголовок отчета
				var titleParagraph = document.Content.Paragraphs.Add();
				titleParagraph.Range.Text = "ОТЧЕТ ПО ВСЕМ ЗАКАЗАМ\n";
				titleParagraph.Range.Font.Bold = 1;
				titleParagraph.Range.Font.Size = 16;
				titleParagraph.Range.Font.Name = "Times New Roman";
				titleParagraph.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
				titleParagraph.Range.InsertParagraphAfter();

				// Информация о отчете
				var infoParagraph = document.Content.Paragraphs.Add();
				infoParagraph.Range.Text = $"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm}\n";
				infoParagraph.Range.Text += $"Всего заказов: {orders.Count}\n";
				infoParagraph.Range.Font.Size = 12;
				infoParagraph.Range.Font.Name = "Times New Roman";
				infoParagraph.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
				infoParagraph.Range.InsertParagraphAfter();

				// Пустая строка
				document.Content.InsertAfter("\n");

				// Создаем таблицу
				var tableRange = document.Content;
				tableRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
				var table = document.Tables.Add(tableRange, orders.Count + 1, 7);

				// Настраиваем таблицу
				table.Borders.Enable = 1;
				table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
				table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

				// Заголовки таблицы
				table.Cell(1, 1).Range.Text = "Клиент";
				table.Cell(1, 2).Range.Text = "Автомобиль";
				table.Cell(1, 3).Range.Text = "Сотрудник";
				table.Cell(1, 4).Range.Text = "Дата заказа";
				table.Cell(1, 5).Range.Text = "Часы";
				table.Cell(1, 6).Range.Text = "Стоимость";
				table.Cell(1, 7).Range.Text = "Дата возврата";

				// Стиль заголовков
				for (int i = 1; i <= 7; i++)
				{
					table.Cell(1, i).Range.Font.Bold = 1;
					table.Cell(1, i).Range.Font.Size = 10;
					table.Cell(1, i).Range.Font.Name = "Times New Roman";
					table.Cell(1, i).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
					table.Cell(1, i).Shading.BackgroundPatternColor = Word.WdColor.wdColorGray15;
				}

				// Заполняем таблицу данными
				for (int i = 0; i < orders.Count; i++)
				{
					var order = orders[i];
					table.Cell(i + 2, 1).Range.Text = order.CustomerName;
					table.Cell(i + 2, 2).Range.Text = order.CarNumber;
					table.Cell(i + 2, 3).Range.Text = order.EmployeeFullName;
					table.Cell(i + 2, 4).Range.Text = order.OrderDate.ToString("dd.MM.yyyy");
					table.Cell(i + 2, 5).Range.Text = order.Hours.ToString();
					table.Cell(i + 2, 6).Range.Text = order.TotalPrice.ToString("C2");
					table.Cell(i + 2, 7).Range.Text = order.ReturnDate.ToString("dd.MM.yyyy");

					// Стиль для строк данных
					for (int j = 1; j <= 7; j++)
					{
						table.Cell(i + 2, j).Range.Font.Size = 9;
						table.Cell(i + 2, j).Range.Font.Name = "Times New Roman";
					}
				}

				// Пустая строка после таблицы
				document.Content.InsertAfter("\n");

				// Итоговая информация
				var totalParagraph = document.Content.Paragraphs.Add();
				totalParagraph.Range.Text = $"ОБЩАЯ ВЫРУЧКА: {orders.Sum(o => o.TotalPrice):C2}\n";
				totalParagraph.Range.Text += $"Средняя стоимость заказа: {orders.Average(o => o.TotalPrice):C2}\n";
				totalParagraph.Range.Text += $"Максимальная стоимость: {orders.Max(o => o.TotalPrice):C2}\n";
				totalParagraph.Range.Text += $"Минимальная стоимость: {orders.Min(o => o.TotalPrice):C2}";
				totalParagraph.Range.Font.Bold = 1;
				totalParagraph.Range.Font.Size = 12;
				totalParagraph.Range.Font.Name = "Times New Roman";
				totalParagraph.Range.Font.Color = Word.WdColor.wdColorDarkBlue;

				document.SaveAs2(filePath);

				MessageBox.Show($"Отчет успешно сохранен: {filePath}", "Успех",
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				// Открываем документ
				System.Diagnostics.Process.Start(filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				// Корректно освобождаем ресурсы
				if (document != null)
				{
					document.Close();
					Marshal.ReleaseComObject(document);
				}
				if (wordApp != null)
				{
					wordApp.Quit();
					Marshal.ReleaseComObject(wordApp);
				}
			}
		}
		// Добавь эти методы в класс WordReportService
		public void GenerateCustomerOrderReport(List<CustomerOrderInfo> orders, string filePath)
		{
			try
			{
				using (var writer = new StreamWriter(filePath))
				{
					writer.WriteLine("ОТЧЕТ ПО ЗАКАЗАМ КЛИЕНТОВ");
					writer.WriteLine("==========================");
					writer.WriteLine($"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm}");
					writer.WriteLine($"Всего записей: {orders.Count}");
					writer.WriteLine();

					writer.WriteLine("┌──────────────────┬──────────────┬──────────────┬────────────┬──────────┬────────────┐");
					writer.WriteLine("│ Клиент           │ Телефон      │ Автомобиль   │ Дата       │ Часы     │ Стоимость  │");
					writer.WriteLine("├──────────────────┼──────────────┼──────────────┼────────────┼──────────┼────────────┤");

					foreach (var order in orders)
					{
						writer.WriteLine($"│ {Truncate(order.FullName, 16),-16} │ {Truncate(order.Phone, 12),-12} │ {Truncate(order.CarNumber, 12),-12} │ {order.OrderDate:dd.MM.yyyy} │ {order.Hours,8} │ {order.TotalPrice,10:C2} │");
					}

					writer.WriteLine("└──────────────────┴──────────────┴──────────────┴────────────┴──────────┴────────────┘");
				}

				System.Diagnostics.Process.Start(filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void GeneratePopularCarsReport(List<PopularCar> cars, string filePath)
		{
			try
			{
				using (var writer = new StreamWriter(filePath))
				{
					writer.WriteLine("ОТЧЕТ ПО ПОПУЛЯРНЫМ АВТОМОБИЛЯМ");
					writer.WriteLine("================================");
					writer.WriteLine($"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm}");
					writer.WriteLine();

					writer.WriteLine("┌──────────────┬──────────────┬────────┬──────────────┬────────────┬────────────────┐");
					writer.WriteLine("│ Номер        │ Марка        │ Статус │ Кол-во заказов│ Всего часов│ Средние часы   │");
					writer.WriteLine("├──────────────┼──────────────┼────────┼──────────────┼────────────┼────────────────┤");

					foreach (var car in cars)
					{
						writer.WriteLine($"│ {Truncate(car.CarNumber, 12),-12} │ {Truncate(car.Make, 12),-12} │ {Truncate(car.Status, 6),-6} │ {car.OrderCount,12} │ {car.TotalRentalHours,10} │ {car.AverageRentalHours,14:F1} │");
					}

					writer.WriteLine("└──────────────┴──────────────┴────────┴──────────────┴────────────┴────────────────┘");
				}

				System.Diagnostics.Process.Start(filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void GenerateGenericReport(System.Windows.Forms.DataGridView dataGridView, string filePath, string title = "Отчет")
		{
			try
			{
				using (var writer = new StreamWriter(filePath))
				{
					writer.WriteLine(title.ToUpper());
					writer.WriteLine(new string('=', title.Length));
					writer.WriteLine($"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm}");
					writer.WriteLine($"Количество записей: {dataGridView.Rows.Count}");
					writer.WriteLine();

					// Заголовки
					var headers = new List<string>();
					foreach (System.Windows.Forms.DataGridViewColumn column in dataGridView.Columns)
					{
						if (column.Visible)
							headers.Add(column.HeaderText);
					}

					// Данные
					foreach (System.Windows.Forms.DataGridViewRow row in dataGridView.Rows)
					{
						if (!row.IsNewRow)
						{
							var values = new List<string>();
							foreach (System.Windows.Forms.DataGridViewCell cell in row.Cells)
							{
								if (cell.OwningColumn.Visible)
									values.Add(cell.Value?.ToString() ?? "");
							}
							writer.WriteLine(string.Join(" | ", values));
						}
					}
				}

				System.Diagnostics.Process.Start(filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// Вспомогательный метод для обрезки строк
		private string Truncate(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
		}
		public void GenerateCustomerTotalReport(List<CustomerTotalReport> report, string filePath)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);

			Word.Application wordApp = null;
			Word.Document document = null;

			try
			{
				wordApp = new Word.Application();
				wordApp.Visible = false;

				document = wordApp.Documents.Add();

				// Заголовок отчета
				var titleParagraph = document.Content.Paragraphs.Add();
				titleParagraph.Range.Text = "ОТЧЕТ ПО КЛИЕНТАМ С ОБЩЕЙ СТОИМОСТЬЮ ПРОКАТА\n";
				titleParagraph.Range.Font.Bold = 1;
				titleParagraph.Range.Font.Size = 16;
				titleParagraph.Range.Font.Name = "Times New Roman";
				titleParagraph.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
				titleParagraph.Range.InsertParagraphAfter();

				// Информация о отчете
				var infoParagraph = document.Content.Paragraphs.Add();
				infoParagraph.Range.Text = $"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm}\n";
				infoParagraph.Range.Font.Size = 11;
				infoParagraph.Range.Font.Name = "Times New Roman";
				infoParagraph.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
				infoParagraph.Range.InsertParagraphAfter();

				document.Content.InsertAfter("\n");

				// Создаем таблицу
				var tableRange = document.Content;
				tableRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
				var table = document.Tables.Add(tableRange, report.Count + 1, 6);

				// Настраиваем таблицу
				table.Borders.Enable = 1;
				table.AllowAutoFit = true;

				// Заголовки таблицы
				table.Cell(1, 1).Range.Text = "Клиент";
				table.Cell(1, 2).Range.Text = "Телефон";
				table.Cell(1, 3).Range.Text = "Заказов";
				table.Cell(1, 4).Range.Text = "Часов";
				table.Cell(1, 5).Range.Text = "Общая стоимость";
				table.Cell(1, 6).Range.Text = "Первый заказ";

				// Стиль заголовков
				for (int i = 1; i <= 6; i++)
				{
					table.Cell(1, i).Range.Font.Bold = 1;
					table.Cell(1, i).Range.Font.Size = 9;
					table.Cell(1, i).Range.Font.Name = "Times New Roman";
					table.Cell(1, i).Shading.BackgroundPatternColor = Word.WdColor.wdColorLightGreen;
				}

				// Заполняем таблицу данными
				for (int i = 0; i < report.Count; i++)
				{
					var item = report[i];
					table.Cell(i + 2, 1).Range.Text = item.FullName;
					table.Cell(i + 2, 2).Range.Text = item.Phone ?? "—";
					table.Cell(i + 2, 3).Range.Text = item.TotalOrders.ToString();
					table.Cell(i + 2, 4).Range.Text = item.TotalHours.ToString();
					table.Cell(i + 2, 5).Range.Text = item.TotalSpent.ToString("C2");
					table.Cell(i + 2, 6).Range.Text = item.FirstOrderDate != DateTime.MinValue ?
						item.FirstOrderDate.ToString("dd.MM.yyyy") : "—";

					// Выделяем итоговую строку
					if (item.FullName.Contains("ВСЕГО") || item.FullName.Contains("ИТОГО"))
					{
						for (int j = 1; j <= 6; j++)
						{
							table.Cell(i + 2, j).Range.Font.Bold = 1;
							table.Cell(i + 2, j).Range.Font.Color = Word.WdColor.wdColorDarkRed;
							table.Cell(i + 2, j).Shading.BackgroundPatternColor = Word.WdColor.wdColorYellow;
						}
					}
				}

				document.Content.InsertAfter("\n");

				// Дополнительная информация
				var regularCustomers = report.Where(r => !r.FullName.Contains("ВСЕГО") && !r.FullName.Contains("ИТОГО")).ToList();
				if (regularCustomers.Any())
				{
					var infoParagraph2 = document.Content.Paragraphs.Add();
					infoParagraph2.Range.Text = "АНАЛИТИКА:\n";
					infoParagraph2.Range.Text += $"Всего клиентов: {regularCustomers.Count}\n";
					infoParagraph2.Range.Text += $"Средние затраты на клиента: {regularCustomers.Average(c => c.TotalSpent):C2}\n";
					infoParagraph2.Range.Text += $"Самый лояльный клиент: {regularCustomers.OrderByDescending(c => c.TotalOrders).First().FullName}";
					infoParagraph2.Range.Font.Size = 10;
					infoParagraph2.Range.Font.Italic = 1;
					infoParagraph2.Range.Font.Color = Word.WdColor.wdColorDarkBlue;
				}

				document.SaveAs2(filePath);

				MessageBox.Show($"Отчет по клиентам сохранен: {filePath}", "Успех",
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				System.Diagnostics.Process.Start(filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (document != null)
				{
					document.Close();
					Marshal.ReleaseComObject(document);
				}
				if (wordApp != null)
				{
					wordApp.Quit();
					Marshal.ReleaseComObject(wordApp);
				}
			}
		}

		// Вспомогательный метод для проверки установленного Word
		public bool IsWordInstalled()
		{
			try
			{
				var wordType = Type.GetTypeFromProgID("Word.Application");
				return wordType != null;
			}
			catch
			{
				return false;
			}
		}
	}
}