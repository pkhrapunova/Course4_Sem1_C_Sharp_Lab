using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CarRental.Data;
using CarRental.Data.Models;

namespace CarRental.UI
{
	public partial class FormAddEditService : Form
	{
		private readonly ServiceRepository _serviceRepo;
		private readonly int? _serviceId;

		public FormAddEditService(ServiceRepository serviceRepo, int? serviceId = null)
		{
			InitializeComponent();
			_serviceRepo = serviceRepo;
			_serviceId = serviceId;

			if (_serviceId.HasValue)
				LoadServiceData(_serviceId.Value);
		}

		private void LoadServiceData(int id)
		{
			var service = _serviceRepo.GetAll().FirstOrDefault(s => s.ServiceID == id);
			if (service != null)
			{
				txtName.Text = service.Name;
				numPrice.Value = (decimal)service.Price;
			}
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtName.Text))
			{
				MessageBox.Show("Введите название услуги!");
				return;
			}

			var service = new Service
			{
				Name = txtName.Text.Trim(),
				Price = numPrice.Value
			};

			if (_serviceId.HasValue)
			{
				service.ServiceID = _serviceId.Value;
				_serviceRepo.Update(service);
			}
			else
				_serviceRepo.Insert(service);

			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
