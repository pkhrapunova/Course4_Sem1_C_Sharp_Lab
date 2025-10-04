using System;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.UI
{
	public partial class FormQueries : Form
	{
		private readonly OrderRepository _orderRepo;
		private readonly CustomerRepository _customerRepo;

		private DataGridView dgvResults;
		private ComboBox cmbCustomers;
		private DateTimePicker dtQueryDate;
		private Button btnExecuteQuery1;
		private Button btnExecuteQuery2;
		private Button btnExecuteQuery3;
		private Button btnExecuteQuery4;
		private Button btnExecuteQuery5;
		private Button btnExportToWord;

		public FormQueries(OrderRepository orderRepo, CustomerRepository customerRepo)
		{
			_orderRepo = orderRepo;
			_customerRepo = customerRepo;

			InitializeComponents();
			LoadCustomers();
		}

		private void InitializeComponents()
		{
			this.Text = "Выполнение запросов";
			this.Width = 900;
			this.Height = 600;
			this.StartPosition = FormStartPosition.CenterParent;

			// Панель управления запросами
			var panelControls = new Panel { Dock = DockStyle.Top, Height = 120, BorderStyle = BorderStyle.FixedSingle };

			// Запрос 1: Клиенты по дате
			dtQueryDate = new DateTimePicker { Top = 10, Left = 10, Width = 120, Value = DateTime.Today };
			btnExecuteQuery1 = new Button { Text = "Клиенты на дату", Top = 10, Left = 140, Width = 120 };
			btnExecuteQuery1.Click += (s, e) => ExecuteQuery1();

			// Запрос 2: Популярные машины
			btnExecuteQuery2 = new Button { Text = "Популярные машины", Top = 10, Left = 270, Width = 120 };
			btnExecuteQuery2.Click += (s, e) => ExecuteQuery2();

			// Запрос 3: Заказы клиента
			cmbCustomers = new ComboBox { Top = 40, Left = 10, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
			btnExecuteQuery3 = new Button { Text = "Заказы клиента", Top = 40, Left = 220, Width = 120 };
			btnExecuteQuery3.Click += (s, e) => ExecuteQuery3();

			// Запрос 4: Часы проката за месяц
			btnExecuteQuery4 = new Button { Text = "Часы проката (месяц)", Top = 40, Left = 350, Width = 140 };
			btnExecuteQuery4.Click += (s, e) => ExecuteQuery4();

			// Запрос 5: Отчет по клиентам
			btnExecuteQuery5 = new Button { Text = "Отчет по клиентам", Top = 40, Left = 500, Width = 140 };
			btnExecuteQuery5.Click += (s, e) => ExecuteQuery5();

			// Кнопка экспорта
			btnExportToWord = new Button { Text = "Экспорт в Word", Top = 70, Left = 10, Width = 120, BackColor = System.Drawing.Color.LightGreen };
			btnExportToWord.Click += BtnExportToWord_Click;

			panelControls.Controls.AddRange(new Control[] {
				new Label { Text = "Дата:", Top = 10, Left = 10, Width = 40 },
				dtQueryDate,
				btnExecuteQuery1,
				btnExecuteQuery2,
				new Label { Text = "Клиент:", Top = 40, Left = 10, Width = 50 },
				cmbCustomers,
				btnExecuteQuery3,
				btnExecuteQuery4,
				btnExecuteQuery5,
				btnExportToWord
			});

			// DataGridView для результатов
			dgvResults = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AutoGenerateColumns = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect
			};

			this.Controls.AddRange(new Control[] { panelControls, dgvResults });
		}

		private void LoadCustomers()
		{
			try
			{
				var customers = _customerRepo.GetAll();
				cmbCustomers.DisplayMember = "FullName";
				cmbCustomers.ValueMember = "CustomerID";
				cmbCustomers.DataSource = customers.ToList();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ExecuteQuery1()
		{
			try
			{
				var results = _orderRepo.GetCustomersByOrderDate(dtQueryDate.Value);
				dgvResults.DataSource = results;
				MessageBox.Show($"Найдено {results.Count} клиентов на дату {dtQueryDate.Value:dd.MM.yyyy}",
					"Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ExecuteQuery2()
		{
			try
			{
				var results = _orderRepo.GetPopularCars(2);
				dgvResults.DataSource = results;
				MessageBox.Show($"Найдено {results.Count} популярных машин (более 2 заказов)",
					"Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ExecuteQuery3()
		{
			if (cmbCustomers.SelectedValue == null)
			{
				MessageBox.Show("Выберите клиента", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try
			{
				int customerId = (int)cmbCustomers.SelectedValue;
				var results = _orderRepo.GetOrdersByCustomer(customerId);
				dgvResults.DataSource = results;
				MessageBox.Show($"Найдено {results.Count} заказов для клиента {cmbCustomers.Text}",
					"Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ExecuteQuery4()
		{
			try
			{
				var results = _orderRepo.GetCarsRentalHoursCurrentMonth();
				dgvResults.DataSource = results;

				int totalHours = results.Sum(r => r.RentalHoursThisMonth);
				decimal totalRevenue = results.Sum(r => r.RevenueThisMonth);

				MessageBox.Show($"Часы проката за текущий месяц:\nВсего часов: {totalHours}\nОбщая выручка: {totalRevenue:C2}",
					"Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ExecuteQuery5()
		{
			try
			{
				var results = _orderRepo.GetCustomerTotalReport();
				var grandTotal = _orderRepo.GetGrandTotal();

				// Добавляем итоговую строку
				if (grandTotal != null)
				{
					var allResults = new List<CustomerTotalReport>(results) { grandTotal };
					dgvResults.DataSource = allResults;
				}
				else
				{
					dgvResults.DataSource = results;
				}

				MessageBox.Show($"Сформирован отчет по {results.Count} клиентам",
					"Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnExportToWord_Click(object sender, EventArgs e)
		{
			if (dgvResults.DataSource == null)
			{
				MessageBox.Show("Нет данных для экспорта", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try
			{
				var reportService = new WordReportService();
				string filePath = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.docx";

				// Определяем тип данных и вызываем соответствующий метод
				var dataSource = dgvResults.DataSource;

				if (dataSource is List<CustomerTotalReport> customerReport)
				{
					reportService.GenerateCustomerTotalReport(customerReport, filePath);
				}
				else if (dataSource is List<CustomerOrderInfo> orderInfo)
				{
					reportService.GenerateCustomerOrderReport(orderInfo, filePath);
				}
				else if (dataSource is List<PopularCar> popularCars)
				{
					reportService.GeneratePopularCarsReport(popularCars, filePath);
				}
				else
				{
					// Универсальный отчет для других типов данных
					reportService.GenerateGenericReport(dgvResults, filePath, "Отчет по запросу");
				}

				MessageBox.Show($"Отчет сохранен в файл: {filePath}", "Успех",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при экспорте в Word: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}