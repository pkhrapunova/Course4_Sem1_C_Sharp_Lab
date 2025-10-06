using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Data.Models;

namespace CarRental.UI
{
	public partial class FormAddEditCustomer : Form
	{
		private readonly CustomerRepository _repo;
		private readonly int? _customerId;

		private TextBox txtName;
		private TextBox txtPassport;
		private TextBox txtAddress;
		private TextBox txtPhone;
		private TextBox txtLicense;
		private Button btnSave;
		private Button btnCancel;

		public FormAddEditCustomer(CustomerRepository repo, int? customerId = null)
		{
			_repo = repo;
			_customerId = customerId;
			InitializeControls();

			Text = _customerId.HasValue ? "Редактирование клиента" : "Добавление клиента";
			StartPosition = FormStartPosition.CenterParent;

			if (_customerId.HasValue)
				LoadCustomer();
		}

		private void InitializeControls()
		{
			// Заголовок
			var lblTitle = new Label
			{
				Text = _customerId.HasValue ? "Редактирование клиента" : "Добавление нового клиента",
				Top = 10,
				Left = 10,
				Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold),
				AutoSize = true
			};

			txtName = new TextBox { Top = 50, Left = 150, Width = 200 };
			txtPassport = new TextBox { Top = 80, Left = 150, Width = 200 };
			txtAddress = new TextBox { Top = 110, Left = 150, Width = 200 };
			txtPhone = new TextBox { Top = 140, Left = 150, Width = 200 };
			txtLicense = new TextBox { Top = 170, Left = 150, Width = 200 };

			btnSave = new Button { Text = "Сохранить", Top = 210, Left = 150, Width = 90 };
			btnSave.Click += BtnSave_Click;

			btnCancel = new Button { Text = "Отмена", Top = 210, Left = 250, Width = 90 };
			btnCancel.Click += (s, e) => Close();

			Controls.Add(lblTitle);
			Controls.Add(new Label { Text = "ФИО:*", Top = 50, Left = 10 });
			Controls.Add(new Label { Text = "Паспорт:*", Top = 80, Left = 10 });
			Controls.Add(new Label { Text = "Адрес:*", Top = 110, Left = 10 });
			Controls.Add(new Label { Text = "Телефон:*", Top = 140, Left = 10 });
			Controls.Add(new Label { Text = "Вод. удостоверение:*", Top = 170, Left = 10 });

			Controls.Add(txtName);
			Controls.Add(txtPassport);
			Controls.Add(txtAddress);
			Controls.Add(txtPhone);
			Controls.Add(txtLicense);
			Controls.Add(btnSave);
			Controls.Add(btnCancel);

			Width = 380;
			Height = 280;
		}

		private void LoadCustomer()
		{
			try
			{
				var c = _repo.GetById(_customerId.Value);
				if (c != null)
				{
					txtName.Text = c.FullName;
					txtPassport.Text = c.Passport;
					txtAddress.Text = c.Address;
					txtPhone.Text = c.Phone;
					txtLicense.Text = c.DrivingLicense;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных клиента: {ex.Message}",
					"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
		}

		private bool ValidateInput()
		{
			if (string.IsNullOrWhiteSpace(txtName.Text))
			{
				MessageBox.Show("Введите ФИО клиента", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtName.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtPassport.Text))
			{
				MessageBox.Show("Введите паспортные данные", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtPassport.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtAddress.Text))
			{
				MessageBox.Show("Введите адрес клиента", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtAddress.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtPhone.Text))
			{
				MessageBox.Show("Введите номер телефона", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtPhone.Focus();
				return false;
			}

			// Простая валидация телефона
			if (!Regex.IsMatch(txtPhone.Text, @"^[\d\s\-\+\(\)]+$"))
			{
				MessageBox.Show("Некорректный формат телефона", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtPhone.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtLicense.Text))
			{
				MessageBox.Show("Введите номер водительского удостоверения", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtLicense.Focus();
				return false;
			}

			return true;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateInput())
				return;

			try
			{
				var customer = new Customer
				{
					FullName = txtName.Text.Trim(),
					Passport = txtPassport.Text.Trim(),
					Address = txtAddress.Text.Trim(),
					Phone = txtPhone.Text.Trim(),
					DrivingLicense = txtLicense.Text.Trim()
				};

				if (_customerId.HasValue)
				{
					customer.CustomerID = _customerId.Value;
					_repo.Update(customer);
					MessageBox.Show("Данные клиента успешно обновлены", "Успех",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					_repo.Insert(customer);
					MessageBox.Show("Клиент успешно добавлен", "Успех",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// FormAddEditCustomer
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Name = "FormAddEditCustomer";
			this.Load += new System.EventHandler(this.FormAddEditCustomer_Load);
			this.ResumeLayout(false);

		}

		private void FormAddEditCustomer_Load(object sender, EventArgs e)
		{

		}
	}
}