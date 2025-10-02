using System;
using System.Windows.Forms;
using System.Xml.Linq;
using CarRental.Data;
using CarRental.Models;

namespace CarRental.UI
{
	public partial class FormAddEditCustomer : Form
	{
		private readonly CustomerRepository _repo;

		public FormAddEditCustomer(CustomerRepository repo)
		{
			InitializeComponent();
			_repo = repo;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			var c = new Customer
			{
				FullName = txtName.Text,
				Passport = txtPassport.Text,
				Address = txtAddress.Text,
				Phone = txtPhone.Text,
				DrivingLicense = txtLicense.Text
			};

			_repo.Insert(c);
			this.DialogResult = DialogResult.OK;
			Close();
		}
	}
}
