using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using CarRental.Data.Models;

namespace CarRental.UI
{
	public static class WordReportGenerator
	{
		public static void CreateAllCarsReport(string filePath, List<Car> cars)
		{
			using (var wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
			{
				MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
				mainPart.Document = new Document();
				var body = mainPart.Document.AppendChild(new Body());

				body.Append(CreateParagraph("Отчет: Все автомобили", true, 28));

				Table table = CreateTable();
				table.Append(CreateTableRow(true, "ID", "Номер машины", "Марка", "Пробег", "Статус", "Цена за час"));

				foreach (var car in cars)
				{
					table.Append(CreateTableRow(false,
						car.CarID.ToString(),
						car.CarNumber,
						car.Make,
						car.Mileage.ToString(),
						car.Status,
						car.PricePerHour.ToString("0.00")
					));
				}

				body.Append(table);

				mainPart.Document.Save();
			}
		}

		public static void CreatePopularCarsReport(string filePath, List<PopularCar> cars)
		{
			using (var wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
			{
				MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
				mainPart.Document = new Document();
				var body = mainPart.Document.AppendChild(new Body());

				body.Append(CreateParagraph("Отчет: Популярные автомобили", true, 28));

				Table table = CreateTable();
				table.Append(CreateTableRow(true, "ID", "Номер машины", "Марка", "Статус", "Цена за час", "Количество заказов", "Всего часов аренды", "Среднее количество часов"));

				foreach (var car in cars)
				{
					table.Append(CreateTableRow(false,
						car.CarID.ToString(),
						car.CarNumber,
						car.Make,
						car.Status,
						car.PricePerHour.ToString("0.00"),
						car.OrderCount.ToString(),
						car.TotalRentalHours.ToString(),
						car.AverageRentalHours.ToString("0.00")
					));
				}

				body.Append(table);
				mainPart.Document.Save();
			}
		}

		public static void CreateCustomerSummaryReport(string filePath, List<CustomerTotalReport> customers)
		{
			using (var wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
			{
				MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
				mainPart.Document = new Document();
				var body = mainPart.Document.AppendChild(new Body());

				body.Append(CreateParagraph("Отчет: Суммарная информация по клиентам", true, 28));

				Table table = CreateTable();
				table.Append(CreateTableRow(true, "ID", "ФИО", "Телефон", "Всего заказов", "Всего часов", "Общая сумма", "Дата первого заказа", "Дата последнего заказа"));

				foreach (var c in customers)
				{
					table.Append(CreateTableRow(false,
						c.CustomerID.ToString(),
						c.FullName,
						c.Phone,
						c.TotalOrders.ToString(),
						c.TotalHours.ToString(),
						c.TotalSpent.ToString("0.00"),
						c.FirstOrderDate.ToShortDateString(),
						c.LastOrderDate.ToShortDateString()
					));
				}

				body.Append(table);
				mainPart.Document.Save();
			}
		}

		private static Paragraph CreateParagraph(string text, bool bold = false, int fontSize = 24)
		{
			var run = new Run();

			var runProperties = new RunProperties();
			if (bold)
				runProperties.Append(new Bold());

			runProperties.Append(new FontSize() { Val = (fontSize * 2).ToString() }); 
			runProperties.Append(new RunFonts() { Ascii = "Arial", HighAnsi = "Arial" });

			run.RunProperties = runProperties;
			run.Append(new Text(text));

			var paragraph = new Paragraph();
			paragraph.Append(run);

			var paragraphProperties = new ParagraphProperties();
			paragraphProperties.Append(new Justification() { Val = JustificationValues.Center });
			paragraphProperties.Append(new SpacingBetweenLines() { After = "200" });

			paragraph.ParagraphProperties = paragraphProperties;
			return paragraph;
		}

		private static Table CreateTable()
		{
			Table table = new Table();

			TableProperties tableProperties = new TableProperties();

			TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };
			tableProperties.Append(tableStyle);

			TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
			tableProperties.Append(tableWidth);

			TableBorders tableBorders = new TableBorders();
			tableBorders.Append(new TopBorder() { Val = BorderValues.Single, Size = 4 });
			tableBorders.Append(new BottomBorder() { Val = BorderValues.Single, Size = 4 });
			tableBorders.Append(new LeftBorder() { Val = BorderValues.Single, Size = 4 });
			tableBorders.Append(new RightBorder() { Val = BorderValues.Single, Size = 4 });
			tableBorders.Append(new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 2 });
			tableBorders.Append(new InsideVerticalBorder() { Val = BorderValues.Single, Size = 2 });

			tableProperties.Append(tableBorders);
			table.Append(tableProperties);

			return table;
		}

		private static TableRow CreateTableRow(bool isHeader, params string[] values)
		{
			TableRow tr = new TableRow();

			foreach (var val in values)
			{
				TableCell tc = new TableCell();

				TableCellProperties tcProperties = new TableCellProperties();
				tcProperties.Append(new TableCellWidth() { Type = TableWidthUnitValues.Auto });
				tc.Append(tcProperties);

				var paragraph = new Paragraph();
				var run = new Run();

				var runProperties = new RunProperties();
				runProperties.Append(new RunFonts() { Ascii = "Arial", HighAnsi = "Arial" });
				runProperties.Append(new FontSize() { Val = "20" }); // 10pt

				if (isHeader)
				{
					runProperties.Append(new Bold());
				}

				run.RunProperties = runProperties;
				run.Append(new Text(val));

				paragraph.Append(run);

				paragraph.ParagraphProperties = new ParagraphProperties(
					new Justification() { Val = JustificationValues.Center }
				);

				tc.Append(paragraph);
				tr.Append(tc);
			}

			return tr;
		}
	}
}