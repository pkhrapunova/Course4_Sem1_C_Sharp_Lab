using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Data.Models;

namespace CarRental.UI
{
	public partial class MainForm : Form
	{
		#region Поля и репозитории
		private readonly CustomerRepository _customerRepo;
		private readonly CarRepository _carRepo;
		private readonly OrderRepository _orderRepo;
		private readonly ServiceRepository _serviceRepo;

		private List<Service> _displayServices;
		private List<Order> _allOrders;
		private List<dynamic> _displayOrders;
		private List<CarDisplayModel> _displayCars;
		private List<CustomerDisplayModel> _displayCustomers;

		#endregion

		#region Конструктор
		public MainForm()
		{
			InitializeComponent();

			_customerRepo = new CustomerRepository();
			_carRepo = new CarRepository();
			_orderRepo = new OrderRepository();
			_serviceRepo = new ServiceRepository();

			LoadAllData();
		}

		private void ConfigureGrid(DataGridView grid)
		{
			// Автоматическая подгонка ширины и высоты под содержимое
			grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

			// Заголовки тоже автоматически подстраиваются
			grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			// Выровнять текст по центру заголовков и по левому краю ячеек
			grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			// Немного улучшить внешний вид
			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			grid.MultiSelect = false;
			grid.ReadOnly = true;
			grid.AllowUserToAddRows = false;
			grid.AllowUserToResizeRows = false;
			grid.RowHeadersVisible = false;

			// Цвета и стиль (по желанию)
			grid.EnableHeadersVisualStyles = false;
			grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
			grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			grid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
		}

		#endregion

		#region Загрузка данных
		private void LoadAllData()
		{
			try
			{
				statusLabel.Text = "Загрузка данных...";
				Application.DoEvents();

				LoadCustomers();
				LoadCars();
				LoadOrders();
				LoadServices();

				statusLabel.Text = "Данные успешно загружены";
			}
			catch (Exception ex)
			{
				statusLabel.Text = "Ошибка при загрузке данных";
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void LoadServices()
		{
			var services = _serviceRepo.GetAll().ToList();

			_displayServices = services;

			dgvServices.DataSource = _displayServices;

			if (dgvServices.Columns.Contains("ServiceID"))
				dgvServices.Columns["ServiceID"].Visible = false;

			dgvServices.Columns["Name"].HeaderText = "Название услуги";
			dgvServices.Columns["Price"].HeaderText = "Цена";

			ConfigureGrid(dgvServices);
		}

		private void LoadCustomers()
		{
			var customers = _customerRepo.GetAll().ToList();

			_displayCustomers = customers.Select(c => new CustomerDisplayModel
			{
				CustomerID = c.CustomerID,
				FullName = c.FullName,
				Passport = c.Passport,
				Address = c.Address,
				Phone = c.Phone,
				DrivingLicense = c.DrivingLicense
			}).ToList();

			dgvCustomers.DataSource = _displayCustomers;

			if (dgvCustomers.Columns.Contains("CustomerID"))
				dgvCustomers.Columns["CustomerID"].Visible = false;
			dgvCustomers.Columns["FullName"].HeaderText = "ФИО";
			dgvCustomers.Columns["Passport"].HeaderText = "Паспорт";
			dgvCustomers.Columns["Address"].HeaderText = "Адрес";
			dgvCustomers.Columns["Phone"].HeaderText = "Телефон";
			dgvCustomers.Columns["DrivingLicense"].HeaderText = "Водительское удостоверение";
			cmbFilterCustomer.Items.Clear();
			cmbFilterCustomer.Items.Add("Все");
			cmbFilterCustomer.Items.AddRange(customers.Select(c => c.FullName).ToArray());
			cmbFilterCustomer.SelectedIndex = 0;

			ConfigureGrid(dgvCustomers);
		}

		private void LoadCars()
		{
			var cars = _carRepo.GetAll().ToList();

			_displayCars = cars.Select(c => new CarDisplayModel
			{
				CarID = c.CarID,
				CarNumber = c.CarNumber,
				Make = c.Make,
				Mileage = c.Mileage,
				Status = c.Status,
				Seats = c.Seats,
				PricePerHour = c.PricePerHour
			}).ToList();

			dgvCars.DataSource = _displayCars;

			if (dgvCars.Columns.Contains("CarID"))
				dgvCars.Columns["CarID"].Visible = false;
			dgvCars.Columns["CarNumber"].HeaderText = "Номер машины";
			dgvCars.Columns["Make"].HeaderText = "Марка";
			dgvCars.Columns["Mileage"].HeaderText = "Пробег";
			dgvCars.Columns["Status"].HeaderText = "Статус";
			dgvCars.Columns["Seats"].HeaderText = "Мест";
			dgvCars.Columns["PricePerHour"].HeaderText = "Цена за час";
			cmbFilterCar.Items.Clear();
			cmbFilterCar.Items.Add("Все");
			cmbFilterCar.Items.AddRange(cars.Select(c => c.CarNumber).ToArray());
			cmbFilterCar.SelectedIndex = 0;

			ConfigureGrid(dgvCars);
		}

		private void LoadOrders()
		{
			try
			{
				_allOrders = _orderRepo.GetAll().ToList();

				var customers = _customerRepo.GetAll().ToDictionary(c => c.CustomerID, c => c.FullName);
				var cars = _carRepo.GetAll().ToDictionary(c => c.CarID, c => c);

				_displayOrders = _allOrders
	.Select(o => new
	{
		o.OrderID,
		OrderDate = o.OrderDate,
		o.EmployeeFullName,
		CustomerName = o.Customer?.FullName ?? "Неизвестно",
		CarNumber = o.Car?.CarNumber ?? "Неизвестно",
		Services = string.Join(", ", o.Services.Select(s => s.Name)),
		Hours = o.Hours,
		TotalPrice = (o.Car?.PricePerHour ?? 0) * o.Hours + o.Services.Sum(s => s.Price)
	})
	.ToList<dynamic>();



				dgvOrders.DataSource = _displayOrders;

				// ПРОВЕРКА НА NULL перед обращением к колонкам
				if (dgvOrders.Columns["OrderID"] != null)
					dgvOrders.Columns["OrderID"].Visible = false;

				// Настройка заголовков с проверкой на null
				SafeSetHeaderText(dgvOrders, "OrderDate", "Дата");
				SafeSetHeaderText(dgvOrders, "OrderTime", "Время");
				SafeSetHeaderText(dgvOrders, "EmployeeName", "Сотрудник");
				SafeSetHeaderText(dgvOrders, "CustomerName", "Клиент");
				SafeSetHeaderText(dgvOrders, "CarNumber", "Машина");
				SafeSetHeaderText(dgvOrders, "ReturnDate", "Возврат");
				SafeSetHeaderText(dgvOrders, "Hours", "Часы");
				SafeSetHeaderText(dgvOrders, "TotalPrice", "Стоимость");
				SafeSetHeaderText(dgvOrders, "Services", "Услуги");

				ConfigureGrid(dgvOrders);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// Вспомогательный метод для безопасной установки заголовков
		private void SafeSetHeaderText(DataGridView grid, string columnName, string headerText)
		{
			if (grid.Columns[columnName] != null)
			{
				grid.Columns[columnName].HeaderText = headerText;
			}
		}

		private void FormatOrdersGrid()
		{
			if (dgvOrders.Columns.Contains("OrderID"))
				dgvOrders.Columns["OrderID"].Visible = false;

			if (dgvOrders.Columns.Contains("OrderDate"))
				dgvOrders.Columns["OrderDate"].HeaderText = "Дата";

			if (dgvOrders.Columns.Contains("OrderTime"))
				dgvOrders.Columns["OrderTime"].HeaderText = "Время";

			if (dgvOrders.Columns.Contains("EmployeeName"))
				dgvOrders.Columns["EmployeeName"].HeaderText = "Сотрудник";

			if (dgvOrders.Columns.Contains("CustomerName"))
				dgvOrders.Columns["CustomerName"].HeaderText = "Клиент";

			if (dgvOrders.Columns.Contains("CarNumber"))
				dgvOrders.Columns["CarNumber"].HeaderText = "Машина";

			if (dgvOrders.Columns.Contains("ReturnDate"))
				dgvOrders.Columns["ReturnDate"].HeaderText = "Возврат";

			if (dgvOrders.Columns.Contains("Hours"))
				dgvOrders.Columns["Hours"].HeaderText = "Часы";

			if (dgvOrders.Columns.Contains("TotalPrice"))
				dgvOrders.Columns["TotalPrice"].HeaderText = "Стоимость";

			// Автоподгон ширины
			dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}


		#endregion

		#region Фильтрация и поиск
		private void BtnSearch_Click(object sender, EventArgs e)
		{
			ApplyFilters();
		}

		private void BtnResetFilter_Click(object sender, EventArgs e)
		{
			cmbFilterCustomer.SelectedIndex = 0;
			cmbFilterCar.SelectedIndex = 0;
			dtFilterStartDate.Value = DateTime.Today.AddMonths(-1);
			dtFilterEndDate.Value = DateTime.Today;

			dgvOrders.DataSource = _displayOrders;
			FormatOrdersGrid();
		}

		private void ApplyFilters()
		{
			var filtered = _displayOrders.AsEnumerable();

			if (cmbFilterCustomer.SelectedIndex > 0)
				filtered = filtered.Where(o => o.CustomerName == cmbFilterCustomer.SelectedItem.ToString());

			if (cmbFilterCar.SelectedIndex > 0)
				filtered = filtered.Where(o => o.CarNumber == cmbFilterCar.SelectedItem.ToString());

			if (dtFilterStartDate.Value <= dtFilterEndDate.Value)
				filtered = filtered.Where(o => o.OrderDate >= dtFilterStartDate.Value &&
											   o.OrderDate <= dtFilterEndDate.Value);

			dgvOrders.DataSource = filtered.ToList();
			FormatOrdersGrid();
		}

		#endregion

		#region CRUD Клиентов
		private void BtnAddCustomer_Click(object sender, EventArgs e)
		{
			var form = new FormAddEditCustomer(_customerRepo);
			if (form.ShowDialog() == DialogResult.OK)
				LoadCustomers();
		}

		private void BtnEditCustomer_Click(object sender, EventArgs e)
		{
			if (dgvCustomers.SelectedRows.Count == 0) return;

			int customerId = (int)dgvCustomers.SelectedRows[0].Cells["CustomerID"].Value;
			var form = new FormAddEditCustomer(_customerRepo, customerId);
			if (form.ShowDialog() == DialogResult.OK)
				LoadCustomers();
		}


		private void BtnDeleteCustomer_Click(object sender, EventArgs e)
		{
			if (dgvCustomers.SelectedRows.Count == 0) return;

			var selected = (CustomerDisplayModel)dgvCustomers.SelectedRows[0].DataBoundItem;
			if (MessageBox.Show($"Удалить клиента {selected.FullName}?", "Подтверждение",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				_customerRepo.Delete(selected.CustomerID);
				LoadCustomers();
			}
		}
		#endregion

		#region CRUD машин
		// Добавить машину
		private void BtnAddCar_Click(object sender, EventArgs e)
		{
			var form = new FormAddEditCar(_carRepo);
			if (form.ShowDialog() == DialogResult.OK)
				LoadCars();
		}

		// Редактировать машину
		private void BtnEditCar_Click(object sender, EventArgs e)
		{
			if (dgvCars.SelectedRows.Count == 0) return;

			int carId = (int)dgvCars.SelectedRows[0].Cells["CarID"].Value;
			var form = new FormAddEditCar(_carRepo, carId);
			if (form.ShowDialog() == DialogResult.OK)
				LoadCars();
		}

		// Удалить машину
		private void BtnDeleteCar_Click(object sender, EventArgs e)
		{
			if (dgvCars.SelectedRows.Count == 0) return;

			int carId = (int)dgvCars.SelectedRows[0].Cells["CarID"].Value;
			if (MessageBox.Show("Вы уверены, что хотите удалить эту машину?", "Подтверждение",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				_carRepo.Delete(carId);
				LoadCars();
			}
		}
		#endregion

		#region CRUD заказы
		// Добавить заказ
		private void BtnAddOrder_Click(object sender, EventArgs e)
		{
			var form = new FormAddEditOrder(_orderRepo, _customerRepo, _carRepo);
			if (form.ShowDialog() == DialogResult.OK)
				LoadOrders();
		}

		// Редактировать заказ
		private void BtnEditOrder_Click(object sender, EventArgs e)
		{
			if (dgvOrders.SelectedRows.Count == 0) return;

			int orderId = (int)dgvOrders.SelectedRows[0].Cells["OrderID"].Value;
			var form = new FormAddEditOrder(_orderRepo, _customerRepo, _carRepo, orderId);
			if (form.ShowDialog() == DialogResult.OK)
				LoadOrders();
		}

		// Удалить заказ
		private void BtnDeleteOrder_Click(object sender, EventArgs e)
		{
			if (dgvOrders.SelectedRows.Count == 0) return;

			int orderId = (int)dgvOrders.SelectedRows[0].Cells["OrderID"].Value;
			if (MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				_orderRepo.Delete(orderId);
				LoadOrders();
			}
		}

		#endregion

		#region CRUD услуг
		private void BtnAddService_Click(object sender, EventArgs e)
		{
			var form = new FormAddEditService(_serviceRepo);
			if (form.ShowDialog() == DialogResult.OK)
				LoadServices();
		}

		private void BtnEditService_Click(object sender, EventArgs e)
		{
			if (dgvServices.SelectedRows.Count == 0) return;

			int serviceId = (int)dgvServices.SelectedRows[0].Cells["ServiceID"].Value;
			var form = new FormAddEditService(_serviceRepo, serviceId);
			if (form.ShowDialog() == DialogResult.OK)
				LoadServices();
		}

		private void BtnDeleteService_Click(object sender, EventArgs e)
		{
			if (dgvServices.SelectedRows.Count == 0) return;

			var selected = (Service)dgvServices.SelectedRows[0].DataBoundItem;
			if (MessageBox.Show($"Удалить услугу '{selected.Name}'?", "Подтверждение",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				_serviceRepo.Delete(selected.ServiceID);
				LoadServices();
			}
		}
		#endregion


		#region сортировки
		private bool sortAscending = true;

		private void BtnSortByPrice_Click(object sender, EventArgs e)
		{
			if (sortAscending)
				dgvOrders.DataSource = _displayOrders.OrderBy(o => o.TotalPrice).ToList();
			else
				dgvOrders.DataSource = _displayOrders.OrderByDescending(o => o.TotalPrice).ToList();

			sortAscending = !sortAscending;
			FormatOrdersGrid();
		}

		private bool sortNameAscending = true;

		private void BtnSortByName_Click(object sender, EventArgs e)
		{
			if (sortNameAscending)
				dgvCustomers.DataSource = _displayCustomers.OrderBy(c => c.FullName).ToList();
			else
				dgvCustomers.DataSource = _displayCustomers.OrderByDescending(c => c.FullName).ToList();

			sortNameAscending = !sortNameAscending;
			ConfigureGrid(dgvCustomers);
		}

		private bool sortPriceAscending = true;

		private void BtnSortByPrise_Click(object sender, EventArgs e)
		{
			if (sortPriceAscending)
				dgvCars.DataSource = _displayCars.OrderBy(c => c.PricePerHour).ToList();
			else
				dgvCars.DataSource = _displayCars.OrderByDescending(c => c.PricePerHour).ToList();

			sortPriceAscending = !sortPriceAscending;
			ConfigureGrid(dgvCars);

		}
		#endregion

		private void BtnLoadCurrentMonth_Click(object sender, EventArgs e)
		{
			try
			{
				var cars = _carRepo.GetCarsCurrentMonth();

				if (cars.Count == 0)
				{
					MessageBox.Show("Нет заказов на текущий месяц.", "Информация",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				dgvOrders.DataSource = cars;

				// Настройка колонок
				if (dgvOrders.Columns.Contains("CarID"))
					dgvOrders.Columns["CarID"].Visible = false;

				if (dgvOrders.Columns.Contains("CarNumber"))
					dgvOrders.Columns["CarNumber"].HeaderText = "Номер машины";

				if (dgvOrders.Columns.Contains("Make"))
					dgvOrders.Columns["Make"].HeaderText = "Марка";

				if (dgvOrders.Columns.Contains("TotalHoursThisMonth"))
					dgvOrders.Columns["TotalHoursThisMonth"].HeaderText = "Часы аренды в текущем месяце";

				ConfigureGrid(dgvOrders);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
