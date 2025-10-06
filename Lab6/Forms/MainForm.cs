using System;
using System.Collections.Generic;
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
			dgvCustomers.DataSource = customers;

			cmbFilterCustomer.Items.Clear();
			cmbFilterCustomer.Items.Add("Все");
			cmbFilterCustomer.Items.AddRange(customers.Select(c => c.FullName).ToArray());
			cmbFilterCustomer.SelectedIndex = 0;
		}

		private void LoadCars()
		{
			var cars = _carRepo.GetAll().ToList();
			dgvCars.DataSource = cars;

			cmbFilterCar.Items.Clear();
			cmbFilterCar.Items.Add("Все");
			cmbFilterCar.Items.AddRange(cars.Select(c => c.CarNumber).ToArray());
			cmbFilterCar.SelectedIndex = 0;
		}

		private void LoadOrders()
		{
			_allOrders = _orderRepo.GetAll().ToList();

			var customers = _customerRepo.GetAll().ToDictionary(c => c.CustomerID, c => c.FullName);
			var cars = _carRepo.GetAll().ToDictionary(c => c.CarID, c => c.CarNumber);

			_displayOrders = _allOrders
				.Select(o => (dynamic)new
				{
					o.OrderID,
					o.OrderDate,
					o.OrderTime,
					o.EmployeeFullName,
					CustomerName = customers.ContainsKey(o.CustomerID) ? customers[o.CustomerID] : "Неизвестно",
					CarNumber = cars.ContainsKey(o.CarID) ? cars[o.CarID] : "Неизвестно",
					o.ReturnDate,
					o.Hours,
					TotalPrice = o.Hours * 100 // пример расчета
				})
				.ToList();

			dgvOrders.DataSource = _displayOrders;
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
		}

		private void BtnSortByDate_Click(object sender, EventArgs e)
		{
			dgvOrders.DataSource = _displayOrders
				.OrderByDescending(o => o.OrderDate)
				.ThenByDescending(o => o.OrderTime)
				.ToList();
		}

		private void BtnSortByPrice_Click(object sender, EventArgs e)
		{
			dgvOrders.DataSource = _displayOrders
				.OrderByDescending(o => o.TotalPrice)
				.ToList();
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


	}
}
