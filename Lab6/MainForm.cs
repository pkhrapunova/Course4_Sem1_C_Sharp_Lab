using System;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Models;

namespace CarRental.UI
{
	public partial class MainForm : Form
	{
		private readonly CustomerRepository _customerRepo;
		private DataGridView dgvCustomers;

		public MainForm()
		{
			InitializeComponent();
			_customerRepo = new CustomerRepository();
			InitializeDataGridView(); // инициализация dgv
		}

		private void InitializeDataGridView()
		{
			dgvCustomers = new DataGridView
			{
				Location = new System.Drawing.Point(10, 10),
				Size = new System.Drawing.Size(600, 300),
				AutoGenerateColumns = true
			};
			this.Controls.Add(dgvCustomers); // добавляем на форму
		}

		private void BtnLoad_Click(object sender, EventArgs e)
		{
			dgvCustomers.DataSource = _customerRepo.GetAll();
		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			var c = new Customer
			{
				FullName = "Новый Клиент",
				Passport = "XX000000",
				Address = "Адрес",
				Phone = "375291234567",
				DrivingLicense = "BY99999"
			};
			_customerRepo.Insert(c);
			dgvCustomers.DataSource = _customerRepo.GetAll();
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			if (dgvCustomers.CurrentRow != null)
			{
				int id = (int)dgvCustomers.CurrentRow.Cells["CustomerID"].Value;
				_customerRepo.Delete(id);
				dgvCustomers.DataSource = _customerRepo.GetAll();
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// здесь можно сразу подгрузить данные
			dgvCustomers.DataSource = _customerRepo.GetAll();
		}
	}
}
