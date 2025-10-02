using System;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Models;

namespace CarRental.UI
{
	public partial class MainForm : Form
	{
		// Репозитории
		private readonly CustomerRepository _customerRepo;
		private readonly CarRepository _carRepo;
		private readonly OrderRepository _orderRepo;

		// DataGridView для каждой вкладки
		private DataGridView dgvCustomers;
		private DataGridView dgvCars;
		private DataGridView dgvOrders;

		public MainForm()
		{
			InitializeComponent();

			_customerRepo = new CustomerRepository();
			_carRepo = new CarRepository();
			_orderRepo = new OrderRepository();

			InitializeTabControl();
		}

		private void InitializeTabControl()
		{
			var tabControl = new TabControl
			{
				Dock = DockStyle.Fill
			};

			// ================= Клиенты =================
			var tabCustomers = new TabPage("Клиенты");
			dgvCustomers = new DataGridView { Dock = DockStyle.Top, Height = 300, AutoGenerateColumns = true };
			tabCustomers.Controls.Add(dgvCustomers);

			var btnAddCustomer = new Button { Text = "Добавить", Top = 310, Left = 10 };
			btnAddCustomer.Click += BtnAddCustomer_Click;
			var btnEditCustomer = new Button { Text = "Изменить", Top = 310, Left = 100 };
			var btnDeleteCustomer = new Button { Text = "Удалить", Top = 310, Left = 200 };
			btnDeleteCustomer.Click += BtnDeleteCustomer_Click;

			tabCustomers.Controls.Add(btnAddCustomer);
			tabCustomers.Controls.Add(btnEditCustomer);
			tabCustomers.Controls.Add(btnDeleteCustomer);

			tabControl.TabPages.Add(tabCustomers);

			// ================= Машины =================
			var tabCars = new TabPage("Машины");
			dgvCars = new DataGridView { Dock = DockStyle.Top, Height = 300, AutoGenerateColumns = true };
			tabCars.Controls.Add(dgvCars);

			var btnAddCar = new Button { Text = "Добавить", Top = 310, Left = 10 };
			var btnEditCar = new Button { Text = "Изменить", Top = 310, Left = 100 };
			var btnDeleteCar = new Button { Text = "Удалить", Top = 310, Left = 200 };
			tabCars.Controls.Add(btnAddCar);
			tabCars.Controls.Add(btnEditCar);
			tabCars.Controls.Add(btnDeleteCar);

			tabControl.TabPages.Add(tabCars);

			// ================= Заказы =================
			var tabOrders = new TabPage("Заказы");
			dgvOrders = new DataGridView { Dock = DockStyle.Top, Height = 300, AutoGenerateColumns = true };
			tabOrders.Controls.Add(dgvOrders);

			var btnAddOrder = new Button { Text = "Добавить", Top = 310, Left = 10 };
			var btnEditOrder = new Button { Text = "Изменить", Top = 310, Left = 100 };
			var btnDeleteOrder = new Button { Text = "Удалить", Top = 310, Left = 200 };
			tabOrders.Controls.Add(btnAddOrder);
			tabOrders.Controls.Add(btnEditOrder);
			tabOrders.Controls.Add(btnDeleteOrder);

			tabControl.TabPages.Add(tabOrders);

			this.Controls.Add(tabControl);

			// Загрузка данных при старте
			LoadAllData();
		}

		private void LoadAllData()
		{
			dgvCustomers.DataSource = _customerRepo.GetAll();
			dgvCars.DataSource = _carRepo.GetAll();
			dgvOrders.DataSource = _orderRepo.GetAll();
		}

		// ================= Обработчики кнопок =================
		private void BtnAddCustomer_Click(object sender, EventArgs e)
		{
			var form = new FormAddEditCustomer(_customerRepo);
			form.ShowDialog();
			dgvCustomers.DataSource = _customerRepo.GetAll();
		}

		private void BtnDeleteCustomer_Click(object sender, EventArgs e)
		{
			if (dgvCustomers.CurrentRow != null)
			{
				int id = (int)dgvCustomers.CurrentRow.Cells["CustomerID"].Value;
				_customerRepo.Delete(id);
				dgvCustomers.DataSource = _customerRepo.GetAll();
			}
		}

		// Здесь аналогично можно добавить BtnAddCar, BtnDeleteCar, BtnAddOrder, BtnDeleteOrder и т.д.
	}
}
