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

		private bool sortAscending = true;
		private bool sortNameAscending = true;
		private bool sortPriceAscending = true;
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
		#endregion

		#region Настройка DataGridView
		private void ConfigureGrid(DataGridView grid)
		{
			grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			grid.MultiSelect = false;
			grid.ReadOnly = true;
			grid.AllowUserToAddRows = false;
			grid.AllowUserToResizeRows = false;
			grid.RowHeadersVisible = false;

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
				ShowError($"Ошибка при загрузке данных: {ex.Message}");
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

			dgvCustomers.HideColumn("CustomerID");
			dgvCustomers.SetHeader("FullName", "ФИО");
			dgvCustomers.SetHeader("Passport", "Паспорт");
			dgvCustomers.SetHeader("Address", "Адрес");
			dgvCustomers.SetHeader("Phone", "Телефон");
			dgvCustomers.SetHeader("DrivingLicense", "Водительское удостоверение");

			InitCustomerFilter(customers);
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

			dgvCars.HideColumn("CarID");
			dgvCars.SetHeader("CarNumber", "Номер машины");
			dgvCars.SetHeader("Make", "Марка");
			dgvCars.SetHeader("Mileage", "Пробег");
			dgvCars.SetHeader("Status", "Статус");
			dgvCars.SetHeader("Seats", "Мест");
			dgvCars.SetHeader("PricePerHour", "Цена за час");

			InitCarFilter(cars);
			ConfigureGrid(dgvCars);
		}

		private void LoadOrders()
		{
			try
			{
				_allOrders = _orderRepo.GetAll().ToList();

				_displayOrders = _allOrders.Select(o => new
				{
					o.OrderID,
					o.OrderDate,
					o.EmployeeFullName,
					CustomerName = o.Customer?.FullName ?? "Неизвестно",
					CarNumber = o.Car?.CarNumber ?? "Неизвестно",
					o.Hours,
					TotalPrice = (o.Car?.PricePerHour ?? 0) * o.Hours
				}).ToList<dynamic>();

				SetOrdersDataSource(_displayOrders);
			}
			catch (Exception ex)
			{
				ShowError($"Ошибка при загрузке заказов: {ex.Message}");
			}
		}

		private void InitCustomerFilter(List<Customer> customers)
		{
			cmbFilterCustomer.Items.Clear();
			cmbFilterCustomer.Items.Add("Все");
			cmbFilterCustomer.Items.AddRange(customers.Select(c => c.FullName).ToArray());
			cmbFilterCustomer.SelectedIndex = 0;
		}

		private void InitCarFilter(List<Car> cars)
		{
			cmbFilterCar.Items.Clear();
			cmbFilterCar.Items.Add("Все");
			cmbFilterCar.Items.AddRange(cars.Select(c => c.CarNumber).ToArray());
			cmbFilterCar.SelectedIndex = 0;
		}
		#endregion

		#region Фильтрация и поиск
		private void BtnSearch_Click(object sender, EventArgs e) => ApplyFilters();

		private void BtnResetFilter_Click(object sender, EventArgs e)
		{
			cmbFilterCustomer.SelectedIndex = 0;
			cmbFilterCar.SelectedIndex = 0;
			dtFilterStartDate.Value = DateTime.Today.AddMonths(-1);
			dtFilterEndDate.Value = DateTime.Today;

			SetOrdersDataSource(_displayOrders);
		}

		private void ApplyFilters()
		{
			var filtered = FilterOrders(
				cmbFilterCustomer.SelectedIndex > 0 ? cmbFilterCustomer.SelectedItem.ToString() : null,
				cmbFilterCar.SelectedIndex > 0 ? cmbFilterCar.SelectedItem.ToString() : null,
				dtFilterStartDate.Value, dtFilterEndDate.Value
			);

			SetOrdersDataSource(filtered.ToList());
		}

		private IEnumerable<dynamic> FilterOrders(string customer, string car, DateTime? start, DateTime? end)
		{
			var filtered = _displayOrders.AsEnumerable();
			if (!string.IsNullOrEmpty(customer))
				filtered = filtered.Where(o => o.CustomerName == customer);
			if (!string.IsNullOrEmpty(car))
				filtered = filtered.Where(o => o.CarNumber == car);
			if (start.HasValue && end.HasValue)
				filtered = filtered.Where(o => o.OrderDate >= start.Value && o.OrderDate <= end.Value);
			return filtered;
		}
		#endregion

		#region CRUD Общие методы
		private void AddEntity<TForm>(Func<TForm> formFactory, Action reloadAction) where TForm : Form
		{
			var form = formFactory();
			if (form.ShowDialog() == DialogResult.OK)
				reloadAction();
		}

		private void EditEntity<TForm>(DataGridView grid, string idColumn, Func<int, TForm> formFactory, Action reloadAction) where TForm : Form
		{
			if (grid.SelectedRows.Count == 0) return;
			int id = (int)grid.SelectedRows[0].Cells[idColumn].Value;
			var form = formFactory(id);
			if (form.ShowDialog() == DialogResult.OK)
				reloadAction();
		}

		private void DeleteEntity(DataGridView grid, string idColumn, Action<int> deleteAction, Action reloadAction, string message)
		{
			if (grid.SelectedRows.Count == 0) return;

			int id = (int)grid.SelectedRows[0].Cells[idColumn].Value;
			if (MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				deleteAction(id);
				reloadAction();
			}
		}
		#endregion

		#region CRUD Клиентов
		private void BtnAddCustomer_Click(object sender, EventArgs e) =>
			AddEntity(() => new FormAddEditCustomer(_customerRepo), LoadCustomers);

		private void BtnEditCustomer_Click(object sender, EventArgs e) =>
			EditEntity(dgvCustomers, "CustomerID", id => new FormAddEditCustomer(_customerRepo, id), LoadCustomers);

		private void BtnDeleteCustomer_Click(object sender, EventArgs e) =>
			DeleteEntity(dgvCustomers, "CustomerID", _customerRepo.Delete, LoadCustomers,
				$"Удалить клиента {(dgvCustomers.SelectedRows[0].Cells["FullName"].Value)}?");
		#endregion

		#region CRUD Машин
		private void BtnAddCar_Click(object sender, EventArgs e) =>
			AddEntity(() => new FormAddEditCar(_carRepo), LoadCars);

		private void BtnEditCar_Click(object sender, EventArgs e) =>
			EditEntity(dgvCars, "CarID", id => new FormAddEditCar(_carRepo, id), LoadCars);

		private void BtnDeleteCar_Click(object sender, EventArgs e) =>
			DeleteEntity(dgvCars, "CarID", _carRepo.Delete, LoadCars,
				$"Удалить автомобиль {(dgvCars.SelectedRows[0].Cells["CarNumber"].Value)}?");
		#endregion

		#region CRUD Заказы
		private void BtnAddOrder_Click(object sender, EventArgs e) =>
			AddEntity(() => new FormAddEditOrder(_orderRepo, _customerRepo, _carRepo), LoadOrders);

		private void BtnEditOrder_Click(object sender, EventArgs e) =>
			EditEntity(dgvOrders, "OrderID", id => new FormAddEditOrder(_orderRepo, _customerRepo, _carRepo, id), LoadOrders);

		private void BtnDeleteOrder_Click(object sender, EventArgs e) =>
			DeleteEntity(dgvOrders, "OrderID", _orderRepo.Delete, LoadOrders,
				"Вы уверены, что хотите удалить этот заказ?");
		#endregion

		#region Сортировка
		private void BtnSortByPrice_Click(object sender, EventArgs e) =>
			SortGrid(dgvOrders, _displayOrders, o => o.TotalPrice, ref sortAscending);

		private void BtnSortByName_Click(object sender, EventArgs e) =>
			SortGrid(dgvCustomers, _displayCustomers, c => c.FullName, ref sortNameAscending);

		private void BtnSortByPrise_Click(object sender, EventArgs e) =>
			SortGrid(dgvCars, _displayCars, c => c.PricePerHour, ref sortPriceAscending);

		private void SortGrid<T>(DataGridView grid, IEnumerable<T> source, Func<T, object> keySelector, ref bool ascending)
		{
			grid.DataSource = ascending ? source.OrderBy(keySelector).ToList() : source.OrderByDescending(keySelector).ToList();
			ascending = !ascending;
		}
		#endregion

		#region Дополнительно
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

				if (dgvOrders.Columns.Contains("CarID"))
					dgvOrders.Columns["CarID"].Visible = false;

				dgvOrders.SetHeader("CarNumber", "Номер машины");
				dgvOrders.SetHeader("Make", "Марка");
				dgvOrders.SetHeader("TotalHoursThisMonth", "Часы аренды в текущем месяце");

				ConfigureGrid(dgvOrders);
			}
			catch (Exception ex)
			{
				ShowError($"Ошибка при загрузке данных: {ex.Message}");
			}
		}
		#endregion

		#region Вспомогательные методы
		private void SetOrdersDataSource(object source)
		{
			dgvOrders.DataSource = source;
			FormatOrdersGrid();
		}

		private void FormatOrdersGrid()
		{
			dgvOrders.HideColumn("OrderID");
			dgvOrders.SetHeader("OrderDate", "Дата");
			dgvOrders.SetHeader("EmployeeFullName", "Сотрудник");
			dgvOrders.SetHeader("CustomerName", "Клиент");
			dgvOrders.SetHeader("CarNumber", "Машина");
			dgvOrders.SetHeader("Hours", "Часы");
			dgvOrders.SetHeader("TotalPrice", "Стоимость");

			ConfigureGrid(dgvOrders);
		}

		private void ShowError(string message) =>
			MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		#endregion
	}

	#region Расширения DataGridView
	public static class DataGridViewExtensions
	{
		public static void HideColumn(this DataGridView grid, string columnName)
		{
			if (grid.Columns.Contains(columnName))
				grid.Columns[columnName].Visible = false;
		}

		public static void SetHeader(this DataGridView grid, string columnName, string headerText)
		{
			if (grid.Columns.Contains(columnName))
				grid.Columns[columnName].HeaderText = headerText;
		}
	}
	#endregion
}
