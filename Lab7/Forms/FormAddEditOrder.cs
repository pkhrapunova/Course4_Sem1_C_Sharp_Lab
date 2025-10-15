using System;
using System.Linq;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Data.Models;

namespace CarRental.UI
{
	public partial class FormAddEditOrder : Form
	{
		private readonly OrderRepository _orderRepo;
		private readonly CustomerRepository _customerRepo;
		private readonly CarRepository _carRepo;
		private readonly int? _orderId;

		private ComboBox cmbCustomer;
		private ComboBox cmbCar;
		private TextBox txtEmployee;
		private DateTimePicker dtOrderDate;
		private DateTimePicker dtReturnDate;
		private NumericUpDown numHours;
		private Label lblTotalPrice;
		private Button btnSave;
		private Button btnCancel;

		public FormAddEditOrder(OrderRepository orderRepo, CustomerRepository customerRepo, CarRepository carRepo, int? orderId = null)
		{
			_orderRepo = orderRepo;
			_customerRepo = customerRepo;
			_carRepo = carRepo;
			_orderId = orderId;

			InitializeControls();

			if (_orderId.HasValue)
				LoadOrder();
		}

		private void InitializeControls()
		{
			cmbCustomer = new ComboBox { Top = 10, Left = 150, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
			cmbCar = new ComboBox { Top = 40, Left = 150, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
			txtEmployee = new TextBox { Top = 70, Left = 150, Width = 200 };
			dtOrderDate = new DateTimePicker { Top = 100, Left = 150, Width = 200 };
			dtReturnDate = new DateTimePicker { Top = 160, Left = 150, Width = 200 };
			numHours = new NumericUpDown { Top = 190, Left = 150, Width = 200, Minimum = 1, Maximum = 1000 };
			lblTotalPrice = new Label { Top = 220, Left = 150, Width = 200, Text = "Общая стоимость: 0 руб." };

			btnSave = new Button { Text = "Сохранить", Top = 250, Left = 150, Width = 90 };
			btnSave.Click += BtnSave_Click;

			btnCancel = new Button { Text = "Отмена", Top = 250, Left = 250, Width = 90 };
			btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

			// Обработчики для автоматического расчета стоимости
			numHours.ValueChanged += CalculateTotalPrice;
			cmbCar.SelectedValueChanged += CalculateTotalPrice;

			Controls.Add(new Label { Text = "Клиент:*", Top = 10, Left = 10 });
			Controls.Add(new Label { Text = "Машина:*", Top = 40, Left = 10 });
			Controls.Add(new Label { Text = "Сотрудник:*", Top = 70, Left = 10 });
			Controls.Add(new Label { Text = "Дата заказа:*", Top = 100, Left = 10 });
			Controls.Add(new Label { Text = "Дата возврата:*", Top = 160, Left = 10 });
			Controls.Add(new Label { Text = "Часы:*", Top = 190, Left = 10 });
			Controls.Add(new Label { Text = "Стоимость:", Top = 220, Left = 10 });

			Controls.Add(cmbCustomer);
			Controls.Add(cmbCar);
			Controls.Add(txtEmployee);
			Controls.Add(dtOrderDate);
			Controls.Add(dtReturnDate);
			Controls.Add(numHours);
			Controls.Add(lblTotalPrice);
			Controls.Add(btnSave);
			Controls.Add(btnCancel);

			Width = 400;
			Height = 320;
			StartPosition = FormStartPosition.CenterParent;

			LoadCustomersAndCars();
		}

		private void LoadCustomersAndCars()
		{
			try
			{
				var customers = _customerRepo.GetAll();
				cmbCustomer.DataSource = customers.ToList();
				cmbCustomer.DisplayMember = "FullName";
				cmbCustomer.ValueMember = "CustomerID";

				// Для нового заказа показываем только доступные автомобили
				var cars = _orderId.HasValue ? _carRepo.GetAll() : _carRepo.GetAvailableCars();
				cmbCar.DataSource = cars.ToList();
				cmbCar.DisplayMember = "CarNumber";
				cmbCar.ValueMember = "CarID";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadOrder()
		{
			try
			{
				var order = _orderRepo.GetById(_orderId.Value);
				if (order != null)
				{
					// Безопасная установка значений
					if (cmbCustomer.Items.Count > 0)
						cmbCustomer.SelectedValue = order.CustomerID;

					if (cmbCar.Items.Count > 0)
						cmbCar.SelectedValue = order.CarID;

					txtEmployee.Text = order.EmployeeFullName;
					dtOrderDate.Value = order.OrderDate;
					dtReturnDate.Value = (DateTime)order.ReturnDate;
					numHours.Value = order.Hours;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке заказа: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CalculateTotalPrice(object sender, EventArgs e)
		{
			try
			{
				if (cmbCar.SelectedItem is Car selectedCar && numHours.Value > 0)
				{
					decimal total = selectedCar.PricePerHour * (int)numHours.Value;
					lblTotalPrice.Text = $"Общая стоимость: {total:C}";
				}
				else
				{
					lblTotalPrice.Text = "Общая стоимость: 0 руб.";
				}
			}
			catch
			{
				lblTotalPrice.Text = "Общая стоимость: 0 руб.";
			}
		}

		private bool ValidateInput()
		{
			if (cmbCustomer.SelectedValue == null)
			{
				MessageBox.Show("Выберите клиента", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cmbCustomer.Focus();
				return false;
			}

			if (cmbCar.SelectedValue == null)
			{
				MessageBox.Show("Выберите автомобиль", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cmbCar.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtEmployee.Text))
			{
				MessageBox.Show("Введите ФИО сотрудника", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmployee.Focus();
				return false;
			}

			if (dtReturnDate.Value.Date < dtOrderDate.Value.Date)
			{
				MessageBox.Show("Дата возврата не может быть раньше даты заказа", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				dtReturnDate.Focus();
				return false;
			}

			if (numHours.Value <= 0)
			{
				MessageBox.Show("Количество часов должно быть больше 0", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				numHours.Focus();
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
				var order = new Order
				{
					CustomerID = (int)cmbCustomer.SelectedValue,
					CarID = (int)cmbCar.SelectedValue,
					EmployeeFullName = txtEmployee.Text.Trim(),
					OrderDate = dtOrderDate.Value.Date,
					ReturnDate = dtReturnDate.Value.Date,
					Hours = (int)numHours.Value
				};

				if (_orderId.HasValue)
				{
					order.OrderID = _orderId.Value;
					_orderRepo.Update(order);
					MessageBox.Show("Заказ успешно обновлен", "Успех",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
						_orderRepo.Insert(order); // Сохраняем новый заказ

					MessageBox.Show("Заказ успешно создан", "Успех",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}", "Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

	}
}