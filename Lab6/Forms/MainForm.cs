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

				statusLabel.Text = "Данные успешно загружены";
			}
			catch (Exception ex)
			{
				statusLabel.Text = "Ошибка при загрузке данных";
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
			_allOrders = _orderRepo.GetAll().ToList();

			var customers = _customerRepo.GetAll().ToDictionary(c => c.CustomerID, c => c.FullName);
			var cars = _carRepo.GetAll().ToDictionary(c => c.CarID, c => c.CarNumber);

			_displayOrders = _allOrders
				.Select(o => new
				{
					o.OrderID,
					OrderDate = o.OrderDate,
					OrderTime = o.OrderTime,
					EmployeeName = o.EmployeeFullName,
					CustomerName = customers.ContainsKey(o.CustomerID) ? customers[o.CustomerID] : "Неизвестно",
					CarNumber = cars.ContainsKey(o.CarID) ? cars[o.CarID] : "Неизвестно",
					ReturnDate = o.ReturnDate,
					Hours = o.Hours,
					TotalPrice = o.Hours * 100
				})
				.ToList<dynamic>();

			dgvOrders.DataSource = _displayOrders;

			if (dgvOrders.Columns.Contains("OrderID"))
				dgvOrders.Columns["OrderID"].Visible = false;

			dgvOrders.Columns["OrderDate"].HeaderText = "Дата";
			dgvOrders.Columns["OrderTime"].HeaderText = "Время";
			dgvOrders.Columns["EmployeeName"].HeaderText = "Сотрудник";
			dgvOrders.Columns["CustomerName"].HeaderText = "Клиент";
			dgvOrders.Columns["CarNumber"].HeaderText = "Машина";
			dgvOrders.Columns["ReturnDate"].HeaderText = "Возврат";
			dgvOrders.Columns["Hours"].HeaderText = "Часы";
			dgvOrders.Columns["TotalPrice"].HeaderText = "Стоимость";

			ConfigureGrid(dgvOrders);
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
			var form = new FormAddEditCustomer(_customerRepo, customerId); // передаём репозиторий + ID
			if (form.ShowDialog() == DialogResult.OK)
				LoadCustomers();
		}


		private void BtnDeleteCustomer_Click(object sender, EventArgs e)
		{
			if (dgvCustomers.SelectedRows.Count == 0) return;

			var selected = (Customer)dgvCustomers.SelectedRows[0].DataBoundItem;
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

		#region репорты
		private void BtnReportAllOrders_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.Filter = "Word Document|*.docx";
				sfd.FileName = "AllCarsReport.docx";
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					var cars = _carRepo.GetAll().ToList();
					WordReportGenerator.CreateAllCarsReport(sfd.FileName, cars);
					MessageBox.Show("Отчет создан", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void BtnReportByQuery_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.Filter = "Word Document|*.docx";
				sfd.FileName = "PopularCarsReport.docx";
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					var popularCars = _carRepo.GetPopularCars().ToList();
					WordReportGenerator.CreatePopularCarsReport(sfd.FileName, popularCars);
					MessageBox.Show("Отчет создан", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void BtnReportGrouped_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.Filter = "Word Document|*.docx";
				sfd.FileName = "CustomerSummaryReport.docx";
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					var reportData = _customerRepo.GetCustomerTotals().ToList();
					WordReportGenerator.CreateCustomerSummaryReport(sfd.FileName, reportData);
					MessageBox.Show("Отчет создан", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
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
			var cars = _carRepo.GetCarsCurrentMonth();

			dgvOrders.DataSource = null;
			dgvOrders.DataSource = cars;
			if (cars.Count == 0)
			{
				MessageBox.Show("Нет заказов на текущий месяц.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
				dgvCars.DataSource = null; 
				return;
			}


			if (dgvOrders.Columns.Contains("CarID"))
				dgvOrders.Columns["CarID"].Visible = false;

			if (dgvOrders.Columns.Contains("CarNumber"))
				dgvOrders.Columns["CarNumber"].HeaderText = "Номер машины";

			if (dgvOrders.Columns.Contains("Make"))
				dgvOrders.Columns["Make"].HeaderText = "Марка";

			if (dgvOrders.Columns.Contains("TotalHoursThisMonth"))
				dgvOrders.Columns["TotalHoursThisMonth"].HeaderText = "Часы аренды в текущем месяце";

			dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}


	}
}
