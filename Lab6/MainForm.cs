using System;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Models;
using System.Linq;
using System.Collections.Generic;

namespace CarRental.UI
{
	public partial class MainForm : Form
	{
		private readonly CustomerRepository _customerRepo;
		private readonly CarRepository _carRepo;
		private readonly OrderRepository _orderRepo;

		private DataGridView dgvCustomers;
		private DataGridView dgvCars;
		private DataGridView dgvOrders;
		private StatusStrip statusStrip;
		private ToolStripStatusLabel statusLabel;

		// Элементы для поиска и фильтрации заказов
		private TextBox txtSearchOrder;
		private ComboBox cmbFilterCustomer;
		private ComboBox cmbFilterCar;
		private DateTimePicker dtFilterStartDate;
		private DateTimePicker dtFilterEndDate;
		private Button btnSearch;
		private Button btnResetFilter;
		private Button btnSortByDate;
		private Button btnSortByPrice;

		// Храним оригинальные данные для фильтрации
		private List<OrderViewModel> _allOrders;

		public MainForm()
		{
			InitializeComponent();

			_customerRepo = new CustomerRepository();
			_carRepo = new CarRepository();
			_orderRepo = new OrderRepository();

			InitializeTabControl();
			InitializeStatusBar();

			LoadAllData();
		}

		private void InitializeStatusBar()
		{
			statusStrip = new StatusStrip();
			statusLabel = new ToolStripStatusLabel { Text = "Готово" };
			statusStrip.Items.Add(statusLabel);
			Controls.Add(statusStrip);
		}

		private void InitializeTabControl()
		{
			var tabControl = new TabControl { Dock = DockStyle.Fill };

			// Вкладка Клиенты
			var tabCustomers = new TabPage("Клиенты");
			InitializeCustomersTab(tabCustomers);
			tabControl.TabPages.Add(tabCustomers);

			// Вкладка Машины
			var tabCars = new TabPage("Машины");
			InitializeCarsTab(tabCars);
			tabControl.TabPages.Add(tabCars);

			// Вкладка Заказы
			var tabOrders = new TabPage("Заказы");
			InitializeOrdersTab(tabOrders);
			tabControl.TabPages.Add(tabOrders);

			Controls.Add(tabControl);
		}

		private void InitializeOrdersTab(TabPage tab)
		{
			// Панель поиска и фильтрации
			var panelFilter = new Panel { Dock = DockStyle.Top, Height = 120, BorderStyle = BorderStyle.FixedSingle };

			// Элементы поиска
			txtSearchOrder = new TextBox { Top = 10, Left = 10, Width = 150, Text = "Поиск по сотруднику..." };

			cmbFilterCustomer = new ComboBox { Top = 10, Left = 170, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
			cmbFilterCustomer.Items.Add("Все клиенты");

			cmbFilterCar = new ComboBox { Top = 10, Left = 330, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
			cmbFilterCar.Items.Add("Все автомобили");

			dtFilterStartDate = new DateTimePicker { Top = 40, Left = 10, Width = 150, Format = DateTimePickerFormat.Short };
			dtFilterEndDate = new DateTimePicker { Top = 40, Left = 170, Width = 150, Format = DateTimePickerFormat.Short };

			btnSearch = new Button { Text = "Поиск", Top = 70, Left = 10, Width = 80 };
			btnSearch.Click += BtnSearch_Click;

			btnResetFilter = new Button { Text = "Сброс", Top = 70, Left = 100, Width = 80 };
			btnResetFilter.Click += BtnResetFilter_Click;

			btnSortByDate = new Button { Text = "Сортировка по дате", Top = 70, Left = 190, Width = 120 };
			btnSortByDate.Click += BtnSortByDate_Click;

			btnSortByPrice = new Button { Text = "Сортировка по цене", Top = 70, Left = 320, Width = 120 };
			btnSortByPrice.Click += BtnSortByPrice_Click;

			panelFilter.Controls.AddRange(new Control[] {
				new Label { Text = "Поиск:", Top = 10, Left = 10, Width = 60 },
				txtSearchOrder,
				new Label { Text = "Клиент:", Top = 10, Left = 170, Width = 60 },
				cmbFilterCustomer,
				new Label { Text = "Автомобиль:", Top = 10, Left = 330, Width = 80 },
				cmbFilterCar,
				new Label { Text = "Дата с:", Top = 40, Left = 10, Width = 60 },
				dtFilterStartDate,
				new Label { Text = "по:", Top = 40, Left = 170, Width = 30 },
				dtFilterEndDate,
				btnSearch,
				btnResetFilter,
				btnSortByDate,
				btnSortByPrice
			});

			// DataGridView для заказов
			dgvOrders = new DataGridView
			{
				Dock = DockStyle.Fill,
				AutoGenerateColumns = true,
				ReadOnly = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect
			};

			// Обработчик события создания колонок
			dgvOrders.DataBindingComplete += (s, e) =>
			{
				if (dgvOrders.Columns["TotalPrice"] != null)
					dgvOrders.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";

				if (dgvOrders.Columns["OrderDate"] != null)
					dgvOrders.Columns["OrderDate"].DefaultCellStyle.Format = "dd.MM.yyyy";

				if (dgvOrders.Columns["ReturnDate"] != null)
					dgvOrders.Columns["ReturnDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
			};

			// Кнопки управления
			var panelButtons = new Panel { Dock = DockStyle.Bottom, Height = 40 };

			var btnAddOrder = new Button { Text = "Добавить", Top = 10, Left = 10, Width = 80 };
			btnAddOrder.Click += BtnAddOrder_Click;

			var btnEditOrder = new Button { Text = "Изменить", Top = 10, Left = 100, Width = 80 };
			btnEditOrder.Click += BtnEditOrder_Click;

			var btnDeleteOrder = new Button { Text = "Удалить", Top = 10, Left = 190, Width = 80 };
			btnDeleteOrder.Click += BtnDeleteOrder_Click;

			var btnRefreshOrders = new Button { Text = "Обновить", Top = 10, Left = 280, Width = 80 };
			btnRefreshOrders.Click += (s, e) => LoadOrders();

			// Кнопки для отчетов
			var btnReportAllOrders = new Button { Text = "Отчет все заказы", Top = 10, Left = 370, Width = 120 };
			btnReportAllOrders.Click += BtnReportAllOrders_Click;

			var btnReportCustomerStats = new Button { Text = "Отчет статистика", Top = 10, Left = 500, Width = 120 };
			btnReportCustomerStats.Click += BtnReportCustomerStats_Click;

			var btnOpenQueries = new Button { Text = "Запросы", Top = 10, Left = 630, Width = 80 };
			btnOpenQueries.Click += BtnOpenQueries_Click;

			panelButtons.Controls.AddRange(new Control[] {
				btnAddOrder, btnEditOrder, btnDeleteOrder, btnRefreshOrders,
				btnReportAllOrders, btnReportCustomerStats, btnOpenQueries
			});

			tab.Controls.AddRange(new Control[] { panelFilter, dgvOrders, panelButtons });
		}

		// Обработчики для кнопок отчетов
		private void BtnReportAllOrders_Click(object sender, EventArgs e)
		{
			try
			{
				var reportService = new WordReportService();
				string filePath = $"AllOrdersReport_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
				reportService.GenerateAllOrdersReport(_allOrders, filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnReportCustomerStats_Click(object sender, EventArgs e)
		{
			try
			{
				var customerReport = _orderRepo.GetCustomerTotalReport();
				var grandTotal = _orderRepo.GetGrandTotal();

				if (grandTotal != null)
				{
					customerReport.Add(grandTotal);
				}

				var reportService = new WordReportService();
				string filePath = $"CustomerStatsReport_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
				reportService.GenerateCustomerTotalReport(customerReport, filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnOpenQueries_Click(object sender, EventArgs e)
		{
			try
			{
				var form = new FormQueries(_orderRepo, _customerRepo);
				form.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при открытии формы запросов: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// Методы для поиска и фильтрации
		private void BtnSearch_Click(object sender, EventArgs e)
		{
			ApplyFilters();
		}

		private void BtnResetFilter_Click(object sender, EventArgs e)
		{
			ResetFilters();
		}

		private void BtnSortByDate_Click(object sender, EventArgs e)
		{
			if (_allOrders != null)
			{
				var sorted = _allOrders.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderTime).ToList();
				dgvOrders.DataSource = sorted;
				statusLabel.Text = "Отсортировано по дате (новые сначала)";
			}
		}

		private void BtnSortByPrice_Click(object sender, EventArgs e)
		{
			if (_allOrders != null)
			{
				var sorted = _allOrders.OrderByDescending(o => o.TotalPrice).ToList();
				dgvOrders.DataSource = sorted;
				statusLabel.Text = "Отсортировано по стоимости (дорогие сначала)";
			}
		}
		private void ApplyFilters()
		{
			if (_allOrders == null) return;

			var filtered = _allOrders.AsEnumerable();

			// Фильтр по текстовому поиску
			if (!string.IsNullOrWhiteSpace(txtSearchOrder.Text))
			{
				var searchText = txtSearchOrder.Text.ToLower();
				filtered = filtered.Where(o =>
					o.EmployeeFullName.ToLower().Contains(searchText) ||
					o.CustomerName.ToLower().Contains(searchText) ||
					o.CarNumber.ToLower().Contains(searchText));
			}

			// Фильтр по клиенту
			if (cmbFilterCustomer.SelectedIndex > 0)
			{
				var selectedCustomer = cmbFilterCustomer.SelectedItem.ToString();
				filtered = filtered.Where(o => o.CustomerName == selectedCustomer);
			}

			// Фильтр по автомобилю
			if (cmbFilterCar.SelectedIndex > 0)
			{
				var selectedCar = cmbFilterCar.SelectedItem.ToString();
				filtered = filtered.Where(o => o.CarNumber == selectedCar);
			}

			// Фильтр по дате
			if (dtFilterStartDate.Value != dtFilterStartDate.MinDate)
			{
				filtered = filtered.Where(o => o.OrderDate >= dtFilterStartDate.Value.Date);
			}

			if (dtFilterEndDate.Value != dtFilterEndDate.MinDate)
			{
				filtered = filtered.Where(o => o.OrderDate <= dtFilterEndDate.Value.Date);
			}

			dgvOrders.DataSource = filtered.ToList();
			statusLabel.Text = $"Найдено заказов: {filtered.Count()}";
		}

		private void ResetFilters()
		{
			txtSearchOrder.Text = "";
			cmbFilterCustomer.SelectedIndex = 0;
			cmbFilterCar.SelectedIndex = 0;
			dtFilterStartDate.Value = dtFilterStartDate.MinDate;
			dtFilterEndDate.Value = dtFilterEndDate.MinDate;

			if (_allOrders != null)
			{
				dgvOrders.DataSource = _allOrders;
				statusLabel.Text = $"Все заказы: {_allOrders.Count}";
			}
		}

		private void LoadFilterData()
		{
			// Загрузка данных для фильтров
			var customers = _customerRepo.GetAll().Select(c => c.FullName).Distinct().ToList();
			var cars = _carRepo.GetAll().Select(c => c.CarNumber).Distinct().ToList();

			cmbFilterCustomer.Items.AddRange(customers.ToArray());
			cmbFilterCar.Items.AddRange(cars.ToArray());

			// Устанавливаем начальные даты
			dtFilterStartDate.Value = DateTime.Today.AddMonths(-1);
			dtFilterEndDate.Value = DateTime.Today;
		}

		// Остальные методы InitializeTab для Customers и Cars остаются без изменений...
		private void InitializeCustomersTab(TabPage tab)
		{
			dgvCustomers = new DataGridView
			{
				Dock = DockStyle.Top,
				Height = 300,
				AutoGenerateColumns = true,
				ReadOnly = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect
			};

			dgvCustomers.DataBindingComplete += (s, e) =>
			{
				if (dgvCustomers.Columns["CustomerID"] != null)
					dgvCustomers.Columns["CustomerID"].Visible = false;
			};

			var btnAddCustomer = new Button { Text = "Добавить", Top = 310, Left = 10, Width = 80 };
			btnAddCustomer.Click += BtnAddCustomer_Click;

			var btnEditCustomer = new Button { Text = "Изменить", Top = 310, Left = 100, Width = 80 };
			btnEditCustomer.Click += BtnEditCustomer_Click;

			var btnDeleteCustomer = new Button { Text = "Удалить", Top = 310, Left = 190, Width = 80 };
			btnDeleteCustomer.Click += BtnDeleteCustomer_Click;

			var btnRefreshCustomers = new Button { Text = "Обновить", Top = 310, Left = 280, Width = 80 };
			btnRefreshCustomers.Click += (s, e) => LoadCustomers();

			tab.Controls.AddRange(new Control[] { dgvCustomers, btnAddCustomer, btnEditCustomer, btnDeleteCustomer, btnRefreshCustomers });
		}

		private void InitializeCarsTab(TabPage tab)
		{
			dgvCars = new DataGridView
			{
				Dock = DockStyle.Top,
				Height = 300,
				AutoGenerateColumns = true,
				ReadOnly = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect
			};

			dgvCars.DataBindingComplete += (s, e) =>
			{
				if (dgvCars.Columns["CarID"] != null)
					dgvCars.Columns["CarID"].Visible = false;

				if (dgvCars.Columns["PricePerHour"] != null)
					dgvCars.Columns["PricePerHour"].DefaultCellStyle.Format = "C2";
			};

			var btnAddCar = new Button { Text = "Добавить", Top = 310, Left = 10, Width = 80 };
			btnAddCar.Click += BtnAddCar_Click;

			var btnEditCar = new Button { Text = "Изменить", Top = 310, Left = 100, Width = 80 };
			btnEditCar.Click += BtnEditCar_Click;

			var btnDeleteCar = new Button { Text = "Удалить", Top = 310, Left = 190, Width = 80 };
			btnDeleteCar.Click += BtnDeleteCar_Click;

			var btnRefreshCars = new Button { Text = "Обновить", Top = 310, Left = 280, Width = 80 };
			btnRefreshCars.Click += (s, e) => LoadCars();

			tab.Controls.AddRange(new Control[] { dgvCars, btnAddCar, btnEditCar, btnDeleteCar, btnRefreshCars });
		}

		private void LoadAllData()
		{
			try
			{
				statusLabel.Text = "Загрузка данных...";
				Application.DoEvents();

				LoadCustomers();
				LoadCars();
				LoadOrders();
				LoadFilterData();

				statusLabel.Text = "Данные успешно загружены";
			}
			catch (Exception ex)
			{
				statusLabel.Text = "Ошибка при загрузке данных";
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadOrders()
		{
			try
			{
				_allOrders = _orderRepo.GetAllWithDetails();
				dgvOrders.DataSource = _allOrders;
				statusLabel.Text = $"Загружено заказов: {_allOrders.Count}";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadCustomers()
		{
			try
			{
				dgvCustomers.DataSource = _customerRepo.GetAll().ToList();
				statusLabel.Text = $"Загружено клиентов: {dgvCustomers.Rows.Count}";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadCars()
		{
			try
			{
				dgvCars.DataSource = _carRepo.GetAll().ToList();
				statusLabel.Text = $"Загружено автомобилей: {dgvCars.Rows.Count}";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке автомобилей: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}



		// Остальные методы остаются без изменений...
		private void BtnAddCustomer_Click(object sender, EventArgs e)
		{
			try
			{
				var form = new FormAddEditCustomer(_customerRepo);
				if (form.ShowDialog() == DialogResult.OK)
				{
					LoadCustomers();
					statusLabel.Text = "Клиент успешно добавлен";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnEditCustomer_Click(object sender, EventArgs e)
		{
			if (dgvCustomers.CurrentRow != null && !dgvCustomers.CurrentRow.IsNewRow)
			{
				try
				{
					int id = (int)dgvCustomers.CurrentRow.Cells["CustomerID"].Value;
					var form = new FormAddEditCustomer(_customerRepo, id);
					if (form.ShowDialog() == DialogResult.OK)
					{
						LoadCustomers();
						statusLabel.Text = "Данные клиента обновлены";
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при редактировании клиента: {ex.Message}", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Выберите клиента для редактирования", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void BtnAddCar_Click(object sender, EventArgs e)
		{
			try
			{
				var form = new FormAddEditCar(_carRepo);
				if (form.ShowDialog() == DialogResult.OK)
				{
					LoadCars();
					statusLabel.Text = "Автомобиль успешно добавлен";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при добавлении автомобиля: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnEditCar_Click(object sender, EventArgs e)
		{
			if (dgvCars.CurrentRow != null && !dgvCars.CurrentRow.IsNewRow)
			{
				try
				{
					int id = (int)dgvCars.CurrentRow.Cells["CarID"].Value;
					var form = new FormAddEditCar(_carRepo, id);
					if (form.ShowDialog() == DialogResult.OK)
					{
						LoadCars();
						statusLabel.Text = "Данные автомобиля обновлены";
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при редактировании автомобиля: {ex.Message}", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Выберите автомобиль для редактирования", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void BtnAddOrder_Click(object sender, EventArgs e)
		{
			try
			{
				var form = new FormAddEditOrder(_orderRepo, _customerRepo, _carRepo);
				if (form.ShowDialog() == DialogResult.OK)
				{
					LoadOrders();
					LoadCars(); // Обновляем статусы автомобилей
					statusLabel.Text = "Заказ успешно создан";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при создании заказа: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnEditOrder_Click(object sender, EventArgs e)
		{
			if (dgvOrders.CurrentRow != null && !dgvOrders.CurrentRow.IsNewRow)
			{
				try
				{
					int id = (int)dgvOrders.CurrentRow.Cells["OrderID"].Value;
					var form = new FormAddEditOrder(_orderRepo, _customerRepo, _carRepo, id);
					if (form.ShowDialog() == DialogResult.OK)
					{
						LoadOrders();
						statusLabel.Text = "Данные заказа обновлены";
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при редактировании заказа: {ex.Message}", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Выберите заказ для редактирования", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void BtnDeleteCustomer_Click(object sender, EventArgs e)
		{
			if (dgvCustomers.CurrentRow != null && !dgvCustomers.CurrentRow.IsNewRow)
			{
				try
				{
					int id = (int)dgvCustomers.CurrentRow.Cells["CustomerID"].Value;
					string name = dgvCustomers.CurrentRow.Cells["FullName"].Value?.ToString() ?? "неизвестный клиент";

					var result = MessageBox.Show($"Вы уверены, что хотите удалить клиента \"{name}\"?",
						"Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.Yes)
					{
						_customerRepo.Delete(id);
						LoadCustomers();
						statusLabel.Text = "Клиент успешно удален";
						MessageBox.Show("Клиент успешно удален", "Успех",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Выберите клиента для удаления", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void BtnDeleteCar_Click(object sender, EventArgs e)
		{
			if (dgvCars.CurrentRow != null && !dgvCars.CurrentRow.IsNewRow)
			{
				try
				{
					int id = (int)dgvCars.CurrentRow.Cells["CarID"].Value;
					string carNumber = dgvCars.CurrentRow.Cells["CarNumber"].Value?.ToString() ?? "неизвестный автомобиль";

					var result = MessageBox.Show($"Вы уверены, что хотите удалить автомобиль \"{carNumber}\"?",
						"Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.Yes)
					{
						_carRepo.Delete(id);
						LoadCars();
						statusLabel.Text = "Автомобиль успешно удален";
						MessageBox.Show("Автомобиль успешно удален", "Успех",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при удалении автомобиля: {ex.Message}", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Выберите автомобиль для удаления", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void BtnDeleteOrder_Click(object sender, EventArgs e)
		{
			if (dgvOrders.CurrentRow != null && !dgvOrders.CurrentRow.IsNewRow)
			{
				try
				{
					int id = (int)dgvOrders.CurrentRow.Cells["OrderID"].Value;

					var result = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?",
						"Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.Yes)
					{
						_orderRepo.Delete(id);
						LoadOrders();
						LoadCars(); // Обновляем статусы автомобилей
						statusLabel.Text = "Заказ успешно удален";
						MessageBox.Show("Заказ успешно удален", "Успех",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Выберите заказ для удаления", "Информация",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			this.Width = 1000;
			this.Height = 500;
			this.Text = "Система аренды автомобилей";
		}
	}
}