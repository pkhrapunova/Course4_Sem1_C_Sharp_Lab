namespace CarRental.UI
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Очистка всех ресурсов.
		/// </summary>
		/// <param name="disposing">true, если управляемые ресурсы должны быть удалены; иначе false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Инициализация формы
		private void InitializeComponent()
		{
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabCustomers = new System.Windows.Forms.TabPage();
			this.dgvCustomers = new System.Windows.Forms.DataGridView();
			this.btnAddCustomer = new System.Windows.Forms.Button();
			this.btnEditCustomer = new System.Windows.Forms.Button();
			this.btnDeleteCustomer = new System.Windows.Forms.Button();
			this.tabCars = new System.Windows.Forms.TabPage();
			this.dgvCars = new System.Windows.Forms.DataGridView();
			this.btnAddCar = new System.Windows.Forms.Button();
			this.btnEditCar = new System.Windows.Forms.Button();
			this.btnDeleteCar = new System.Windows.Forms.Button();
			this.tabOrders = new System.Windows.Forms.TabPage();
			this.cmbFilterCustomer = new System.Windows.Forms.ComboBox();
			this.cmbFilterCar = new System.Windows.Forms.ComboBox();
			this.dtFilterStartDate = new System.Windows.Forms.DateTimePicker();
			this.dtFilterEndDate = new System.Windows.Forms.DateTimePicker();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnResetFilter = new System.Windows.Forms.Button();
			this.btnSortByDate = new System.Windows.Forms.Button();
			this.btnSortByPrice = new System.Windows.Forms.Button();
			this.dgvOrders = new System.Windows.Forms.DataGridView();
			this.btnAddOrder = new System.Windows.Forms.Button();
			this.btnEditOrder = new System.Windows.Forms.Button();
			this.btnDeleteOrder = new System.Windows.Forms.Button();
			this.btnReportAllOrders = new System.Windows.Forms.Button();
			this.btnReportByQuery = new System.Windows.Forms.Button();
			this.btnReportGrouped = new System.Windows.Forms.Button();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabControlMain.SuspendLayout();
			this.tabCustomers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
			this.tabCars.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCars)).BeginInit();
			this.tabOrders.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabCustomers);
			this.tabControlMain.Controls.Add(this.tabCars);
			this.tabControlMain.Controls.Add(this.tabOrders);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1100, 678);
			this.tabControlMain.TabIndex = 0;
			// 
			// tabCustomers
			// 
			this.tabCustomers.Controls.Add(this.dgvCustomers);
			this.tabCustomers.Controls.Add(this.btnAddCustomer);
			this.tabCustomers.Controls.Add(this.btnEditCustomer);
			this.tabCustomers.Controls.Add(this.btnDeleteCustomer);
			this.tabCustomers.Location = new System.Drawing.Point(4, 22);
			this.tabCustomers.Name = "tabCustomers";
			this.tabCustomers.Size = new System.Drawing.Size(1092, 652);
			this.tabCustomers.TabIndex = 0;
			this.tabCustomers.Text = "Клиенты";
			// 
			// dgvCustomers
			// 
			this.dgvCustomers.AllowUserToAddRows = false;
			this.dgvCustomers.AllowUserToDeleteRows = false;
			this.dgvCustomers.Location = new System.Drawing.Point(10, 10);
			this.dgvCustomers.Name = "dgvCustomers";
			this.dgvCustomers.ReadOnly = true;
			this.dgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCustomers.Size = new System.Drawing.Size(1060, 550);
			this.dgvCustomers.TabIndex = 0;
			// 
			// btnAddCustomer
			// 
			this.btnAddCustomer.Location = new System.Drawing.Point(10, 570);
			this.btnAddCustomer.Name = "btnAddCustomer";
			this.btnAddCustomer.Size = new System.Drawing.Size(75, 23);
			this.btnAddCustomer.TabIndex = 1;
			this.btnAddCustomer.Text = "Добавить";
			this.btnAddCustomer.Click += new System.EventHandler(this.BtnAddCustomer_Click);
			// 
			// btnEditCustomer
			// 
			this.btnEditCustomer.Location = new System.Drawing.Point(110, 570);
			this.btnEditCustomer.Name = "btnEditCustomer";
			this.btnEditCustomer.Size = new System.Drawing.Size(75, 23);
			this.btnEditCustomer.TabIndex = 2;
			this.btnEditCustomer.Text = "Изменить";
			this.btnEditCustomer.Click += new System.EventHandler(this.BtnEditCustomer_Click);
			// 
			// btnDeleteCustomer
			// 
			this.btnDeleteCustomer.Location = new System.Drawing.Point(210, 570);
			this.btnDeleteCustomer.Name = "btnDeleteCustomer";
			this.btnDeleteCustomer.Size = new System.Drawing.Size(75, 23);
			this.btnDeleteCustomer.TabIndex = 3;
			this.btnDeleteCustomer.Text = "Удалить";
			this.btnDeleteCustomer.Click += new System.EventHandler(this.BtnDeleteCustomer_Click);
			// 
			// tabCars
			// 
			this.tabCars.Controls.Add(this.dgvCars);
			this.tabCars.Controls.Add(this.btnAddCar);
			this.tabCars.Controls.Add(this.btnEditCar);
			this.tabCars.Controls.Add(this.btnDeleteCar);
			this.tabCars.Location = new System.Drawing.Point(4, 22);
			this.tabCars.Name = "tabCars";
			this.tabCars.Size = new System.Drawing.Size(1092, 652);
			this.tabCars.TabIndex = 1;
			this.tabCars.Text = "Машины";
			// 
			// dgvCars
			// 
			this.dgvCars.AllowUserToAddRows = false;
			this.dgvCars.AllowUserToDeleteRows = false;
			this.dgvCars.Location = new System.Drawing.Point(10, 10);
			this.dgvCars.Name = "dgvCars";
			this.dgvCars.ReadOnly = true;
			this.dgvCars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCars.Size = new System.Drawing.Size(1060, 550);
			this.dgvCars.TabIndex = 0;
			// 
			// btnAddCar
			// 
			this.btnAddCar.Location = new System.Drawing.Point(10, 570);
			this.btnAddCar.Name = "btnAddCar";
			this.btnAddCar.Size = new System.Drawing.Size(75, 23);
			this.btnAddCar.TabIndex = 1;
			this.btnAddCar.Text = "Добавить";
			this.btnAddCar.Click += new System.EventHandler(this.BtnAddCar_Click);
			// 
			// btnEditCar
			// 
			this.btnEditCar.Location = new System.Drawing.Point(110, 570);
			this.btnEditCar.Name = "btnEditCar";
			this.btnEditCar.Size = new System.Drawing.Size(75, 23);
			this.btnEditCar.TabIndex = 2;
			this.btnEditCar.Text = "Изменить";
			this.btnEditCar.Click += new System.EventHandler(this.BtnEditCar_Click);
			// 
			// btnDeleteCar
			// 
			this.btnDeleteCar.Location = new System.Drawing.Point(210, 570);
			this.btnDeleteCar.Name = "btnDeleteCar";
			this.btnDeleteCar.Size = new System.Drawing.Size(75, 23);
			this.btnDeleteCar.TabIndex = 3;
			this.btnDeleteCar.Text = "Удалить";
			this.btnDeleteCar.Click += new System.EventHandler(this.BtnDeleteCar_Click);
			// 
			// tabOrders
			// 
			this.tabOrders.Controls.Add(this.cmbFilterCustomer);
			this.tabOrders.Controls.Add(this.cmbFilterCar);
			this.tabOrders.Controls.Add(this.dtFilterStartDate);
			this.tabOrders.Controls.Add(this.dtFilterEndDate);
			this.tabOrders.Controls.Add(this.btnSearch);
			this.tabOrders.Controls.Add(this.btnResetFilter);
			this.tabOrders.Controls.Add(this.btnSortByDate);
			this.tabOrders.Controls.Add(this.btnSortByPrice);
			this.tabOrders.Controls.Add(this.dgvOrders);
			this.tabOrders.Controls.Add(this.btnAddOrder);
			this.tabOrders.Controls.Add(this.btnEditOrder);
			this.tabOrders.Controls.Add(this.btnDeleteOrder);
			this.tabOrders.Controls.Add(this.btnReportAllOrders);
			this.tabOrders.Controls.Add(this.btnReportByQuery);
			this.tabOrders.Controls.Add(this.btnReportGrouped);
			this.tabOrders.Location = new System.Drawing.Point(4, 22);
			this.tabOrders.Name = "tabOrders";
			this.tabOrders.Size = new System.Drawing.Size(1092, 652);
			this.tabOrders.TabIndex = 2;
			this.tabOrders.Text = "Заказы";
			// 
			// cmbFilterCustomer
			// 
			this.cmbFilterCustomer.Location = new System.Drawing.Point(10, 10);
			this.cmbFilterCustomer.Name = "cmbFilterCustomer";
			this.cmbFilterCustomer.Size = new System.Drawing.Size(150, 21);
			this.cmbFilterCustomer.TabIndex = 1;
			// 
			// cmbFilterCar
			// 
			this.cmbFilterCar.Location = new System.Drawing.Point(170, 9);
			this.cmbFilterCar.Name = "cmbFilterCar";
			this.cmbFilterCar.Size = new System.Drawing.Size(150, 21);
			this.cmbFilterCar.TabIndex = 2;
			// 
			// dtFilterStartDate
			// 
			this.dtFilterStartDate.Location = new System.Drawing.Point(336, 9);
			this.dtFilterStartDate.Name = "dtFilterStartDate";
			this.dtFilterStartDate.Size = new System.Drawing.Size(120, 20);
			this.dtFilterStartDate.TabIndex = 3;
			// 
			// dtFilterEndDate
			// 
			this.dtFilterEndDate.Location = new System.Drawing.Point(462, 9);
			this.dtFilterEndDate.Name = "dtFilterEndDate";
			this.dtFilterEndDate.Size = new System.Drawing.Size(120, 20);
			this.dtFilterEndDate.TabIndex = 4;
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(602, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 5;
			this.btnSearch.Text = "Поиск";
			this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// btnResetFilter
			// 
			this.btnResetFilter.Location = new System.Drawing.Point(683, 8);
			this.btnResetFilter.Name = "btnResetFilter";
			this.btnResetFilter.Size = new System.Drawing.Size(75, 23);
			this.btnResetFilter.TabIndex = 6;
			this.btnResetFilter.Text = "Сброс";
			this.btnResetFilter.Click += new System.EventHandler(this.BtnResetFilter_Click);
			// 
			// btnSortByDate
			// 
			this.btnSortByDate.Location = new System.Drawing.Point(10, 45);
			this.btnSortByDate.Name = "btnSortByDate";
			this.btnSortByDate.Size = new System.Drawing.Size(150, 35);
			this.btnSortByDate.TabIndex = 7;
			this.btnSortByDate.Text = "Сорт. по дате";
			this.btnSortByDate.Click += new System.EventHandler(this.BtnSortByDate_Click);
			// 
			// btnSortByPrice
			// 
			this.btnSortByPrice.Location = new System.Drawing.Point(170, 45);
			this.btnSortByPrice.Name = "btnSortByPrice";
			this.btnSortByPrice.Size = new System.Drawing.Size(150, 35);
			this.btnSortByPrice.TabIndex = 8;
			this.btnSortByPrice.Text = "Сорт. по цене";
			this.btnSortByPrice.Click += new System.EventHandler(this.BtnSortByPrice_Click);
			// 
			// dgvOrders
			// 
			this.dgvOrders.AllowUserToAddRows = false;
			this.dgvOrders.AllowUserToDeleteRows = false;
			this.dgvOrders.Location = new System.Drawing.Point(10, 80);
			this.dgvOrders.Name = "dgvOrders";
			this.dgvOrders.ReadOnly = true;
			this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvOrders.Size = new System.Drawing.Size(1060, 500);
			this.dgvOrders.TabIndex = 9;
			// 
			// btnAddOrder
			// 
			this.btnAddOrder.Location = new System.Drawing.Point(10, 600);
			this.btnAddOrder.Name = "btnAddOrder";
			this.btnAddOrder.Size = new System.Drawing.Size(75, 23);
			this.btnAddOrder.TabIndex = 10;
			this.btnAddOrder.Text = "Добавить";
			this.btnAddOrder.Click += new System.EventHandler(this.BtnAddOrder_Click);
			// 
			// btnEditOrder
			// 
			this.btnEditOrder.Location = new System.Drawing.Point(110, 600);
			this.btnEditOrder.Name = "btnEditOrder";
			this.btnEditOrder.Size = new System.Drawing.Size(75, 23);
			this.btnEditOrder.TabIndex = 11;
			this.btnEditOrder.Text = "Изменить";
			this.btnEditOrder.Click += new System.EventHandler(this.BtnEditOrder_Click);
			// 
			// btnDeleteOrder
			// 
			this.btnDeleteOrder.Location = new System.Drawing.Point(210, 600);
			this.btnDeleteOrder.Name = "btnDeleteOrder";
			this.btnDeleteOrder.Size = new System.Drawing.Size(75, 23);
			this.btnDeleteOrder.TabIndex = 12;
			this.btnDeleteOrder.Text = "Удалить";
			this.btnDeleteOrder.Click += new System.EventHandler(this.BtnDeleteOrder_Click);
			// 
			// btnReportAllOrders
			// 
			this.btnReportAllOrders.Location = new System.Drawing.Point(600, 600);
			this.btnReportAllOrders.Name = "btnReportAllOrders";
			this.btnReportAllOrders.Size = new System.Drawing.Size(150, 30);
			this.btnReportAllOrders.TabIndex = 20;
			this.btnReportAllOrders.Text = "Отчёт: все заказы";
			this.btnReportAllOrders.UseVisualStyleBackColor = true;
			this.btnReportAllOrders.Click += new System.EventHandler(this.BtnReportAllOrders_Click);
			// 
			// btnReportByQuery
			// 
			this.btnReportByQuery.Location = new System.Drawing.Point(760, 600);
			this.btnReportByQuery.Name = "btnReportByQuery";
			this.btnReportByQuery.Size = new System.Drawing.Size(150, 30);
			this.btnReportByQuery.TabIndex = 21;
			this.btnReportByQuery.Text = "Отчёт: запрос";
			this.btnReportByQuery.UseVisualStyleBackColor = true;
			this.btnReportByQuery.Click += new System.EventHandler(this.BtnReportByQuery_Click);
			// 
			// btnReportGrouped
			// 
			this.btnReportGrouped.Location = new System.Drawing.Point(920, 600);
			this.btnReportGrouped.Name = "btnReportGrouped";
			this.btnReportGrouped.Size = new System.Drawing.Size(150, 30);
			this.btnReportGrouped.TabIndex = 22;
			this.btnReportGrouped.Text = "Отчёт: группировка";
			this.btnReportGrouped.UseVisualStyleBackColor = true;
			this.btnReportGrouped.Click += new System.EventHandler(this.BtnReportGrouped_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 678);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1100, 22);
			this.statusStrip.TabIndex = 0;
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(1100, 700);
			this.Controls.Add(this.tabControlMain);
			this.Controls.Add(this.statusStrip);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Система проката автомобилей";
			this.tabControlMain.ResumeLayout(false);
			this.tabCustomers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
			this.tabCars.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvCars)).EndInit();
			this.tabOrders.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Поля элементов
		private System.Windows.Forms.Button btnReportAllOrders;
		private System.Windows.Forms.Button btnReportByQuery;
		private System.Windows.Forms.Button btnReportGrouped;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabCustomers;
		private System.Windows.Forms.TabPage tabCars;
		private System.Windows.Forms.TabPage tabOrders;
		private System.Windows.Forms.DataGridView dgvCustomers;
		private System.Windows.Forms.DataGridView dgvCars;
		private System.Windows.Forms.DataGridView dgvOrders;
		private System.Windows.Forms.Button btnAddCustomer;
		private System.Windows.Forms.Button btnEditCustomer;
		private System.Windows.Forms.Button btnDeleteCustomer;
		private System.Windows.Forms.Button btnAddCar;
		private System.Windows.Forms.Button btnEditCar;
		private System.Windows.Forms.Button btnDeleteCar;
		private System.Windows.Forms.ComboBox cmbFilterCustomer;
		private System.Windows.Forms.ComboBox cmbFilterCar;
		private System.Windows.Forms.DateTimePicker dtFilterStartDate;
		private System.Windows.Forms.DateTimePicker dtFilterEndDate;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnResetFilter;
		private System.Windows.Forms.Button btnSortByDate;
		private System.Windows.Forms.Button btnSortByPrice;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.Button btnAddOrder;
		private System.Windows.Forms.Button btnEditOrder;
		private System.Windows.Forms.Button btnDeleteOrder;

		#endregion

	}
}
