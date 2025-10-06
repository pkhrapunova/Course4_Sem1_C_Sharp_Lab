using System;
using System.Windows.Forms;
using CarRental.Data;
using CarRental.Data.Models;


namespace CarRental.UI
{
	public partial class FormAddEditCar : Form
	{
		private readonly CarRepository _repo;
		private readonly int? _carId;

		private TextBox txtNumber;
		private TextBox txtMake;
		private NumericUpDown numMileage;
		private ComboBox cmbStatus;
		private NumericUpDown numSeats;
		private NumericUpDown numPrice;
		private Button btnSave;
		private Button btnCancel;

		public FormAddEditCar(CarRepository repo, int? carId = null)
		{
			_repo = repo;
			_carId = carId;

			InitializeControls();
			Text = _carId.HasValue ? "Редактирование автомобиля" : "Добавление автомобиля";
			StartPosition = FormStartPosition.CenterParent;

			if (_carId.HasValue)
				LoadCar();
		}

		private void InitializeControls()
		{
			// Заголовок
			var lblTitle = new Label
			{
				Text = _carId.HasValue ? "Редактирование автомобиля" : "Добавление нового автомобиля",
				Top = 10,
				Left = 10,
				Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold),
				AutoSize = true
			};

			txtNumber = new TextBox { Top = 50, Left = 120, Width = 200 };
			txtMake = new TextBox { Top = 80, Left = 120, Width = 200 };
			numMileage = new NumericUpDown { Top = 110, Left = 120, Maximum = 1000000, Width = 200 };
			cmbStatus = new ComboBox { Top = 140, Left = 120, Width = 200 };
			cmbStatus.Items.AddRange(new string[] { "Свободна", "Арендована", "Ремонт" });
			numSeats = new NumericUpDown { Top = 170, Left = 120, Minimum = 1, Maximum = 20, Width = 200 };
			numPrice = new NumericUpDown { Top = 200, Left = 120, Maximum = 10000, DecimalPlaces = 2, Width = 200 };

			btnSave = new Button { Text = "Сохранить", Top = 240, Left = 120, Width = 90 };
			btnSave.Click += BtnSave_Click;

			btnCancel = new Button { Text = "Отмена", Top = 240, Left = 220, Width = 90 };
			btnCancel.Click += (s, e) => Close();

			Controls.Add(lblTitle);
			Controls.Add(new Label { Text = "Номер:*", Top = 50, Left = 10 });
			Controls.Add(new Label { Text = "Марка:*", Top = 80, Left = 10 });
			Controls.Add(new Label { Text = "Пробег:", Top = 110, Left = 10 });
			Controls.Add(new Label { Text = "Статус:*", Top = 140, Left = 10 });
			Controls.Add(new Label { Text = "Сиденья:*", Top = 170, Left = 10 });
			Controls.Add(new Label { Text = "Цена/час:*", Top = 200, Left = 10 });

			Controls.Add(txtNumber);
			Controls.Add(txtMake);
			Controls.Add(numMileage);
			Controls.Add(cmbStatus);
			Controls.Add(numSeats);
			Controls.Add(numPrice);
			Controls.Add(btnSave);
			Controls.Add(btnCancel);

			Width = 350;
			Height = 320;
		}

		private void LoadCar()
		{
			try
			{
				var car = _repo.GetById(_carId.Value);
				if (car != null)
				{
					txtNumber.Text = car.CarNumber;
					txtMake.Text = car.Make;
					numMileage.Value = car.Mileage;
					cmbStatus.SelectedItem = car.Status;
					numSeats.Value = car.Seats;
					numPrice.Value = car.PricePerHour;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных автомобиля: {ex.Message}",
					"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
		}

		private bool ValidateInput()
		{
			if (string.IsNullOrWhiteSpace(txtNumber.Text))
			{
				MessageBox.Show("Введите номер автомобиля", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtNumber.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtMake.Text))
			{
				MessageBox.Show("Введите марку автомобиля", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMake.Focus();
				return false;
			}

			if (cmbStatus.SelectedItem == null)
			{
				MessageBox.Show("Выберите статус автомобиля", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cmbStatus.Focus();
				return false;
			}

			if (numSeats.Value < 1)
			{
				MessageBox.Show("Количество сидений должно быть не менее 1", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				numSeats.Focus();
				return false;
			}

			if (numPrice.Value <= 0)
			{
				MessageBox.Show("Цена за час должна быть больше 0", "Ошибка ввода",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				numPrice.Focus();
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
				var car = new Car
				{
					CarNumber = txtNumber.Text.Trim(),
					Make = txtMake.Text.Trim(),
					Mileage = (int)numMileage.Value,
					Status = cmbStatus.SelectedItem.ToString(),
					Seats = (int)numSeats.Value,
					PricePerHour = numPrice.Value
				};

				if (_carId.HasValue)
				{
					car.CarID = _carId.Value;
					_repo.Update(car);
					MessageBox.Show("Данные автомобиля успешно обновлены", "Успех",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					_repo.Insert(car);
					MessageBox.Show("Автомобиль успешно добавлен", "Успех",
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
	}
}