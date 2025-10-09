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
			this.btnSortByName = new System.Windows.Forms.Button();
			this.dgvCustomers = new System.Windows.Forms.DataGridView();
			this.btnAddCustomer = new System.Windows.Forms.Button();
			this.btnEditCustomer = new System.Windows.Forms.Button();
			this.btnDeleteCustomer = new System.Windows.Forms.Button();
			this.tabCars = new System.Windows.Forms.TabPage();
			this.btnSortByPrise = new System.Windows.Forms.Button();
			this.dgvCars = new System.Windows.Forms.DataGridView();
			this.btnAddCar = new System.Windows.Forms.Button();
			this.btnEditCar = new System.Windows.Forms.Button();
			this.btnDeleteCar = new System.Windows.Forms.Button();
			this.tabOrders = new System.Windows.Forms.TabPage();
			this.BtnLoadCurrentMonth = new System.Windows.Forms.Button();
			this.cmbFilterCustomer = new System.Windows.Forms.ComboBox();
			this.cmbFilterCar = new System.Windows.Forms.ComboBox();
			this.dtFilterStartDate = new System.Windows.Forms.DateTimePicker();
			this.dtFilterEndDate = new System.Windows.Forms.DateTimePicker();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnResetFilter = new System.Windows.Forms.Button();
			this.btnSortByPrice = new System.Windows.Forms.Button();
			this.dgvOrders = new System.Windows.Forms.DataGridView();
			this.btnAddOrder = new System.Windows.Forms.Button();
			this.btnEditOrder = new System.Windows.Forms.Button();
			this.btnDeleteOrder = new System.Windows.Forms.Button();
			this.tabService = new System.Windows.Forms.TabPage();
			this.BtnAddService = new System.Windows.Forms.Button();
			this.BtnEditService = new System.Windows.Forms.Button();
			this.BtnDeleteService = new System.Windows.Forms.Button();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.dgvServices = new System.Windows.Forms.DataGridView();
			this.tabControlMain.SuspendLayout();
			this.tabCustomers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
			this.tabCars.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCars)).BeginInit();
			this.tabOrders.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
			this.tabService.SuspendLayout();
			this.statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabCustomers);
			this.tabControlMain.Controls.Add(this.tabCars);
			this.tabControlMain.Controls.Add(this.tabOrders);
			this.tabControlMain.Controls.Add(this.tabService);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1100, 622);
			this.tabControlMain.TabIndex = 0;
			// 
			// tabCustomers
			// 
			this.tabCustomers.Controls.Add(this.btnSortByName);
			this.tabCustomers.Controls.Add(this.dgvCustomers);
			this.tabCustomers.Controls.Add(this.btnAddCustomer);
			this.tabCustomers.Controls.Add(this.btnEditCustomer);
			this.tabCustomers.Controls.Add(this.btnDeleteCustomer);
			this.tabCustomers.Location = new System.Drawing.Point(4, 22);
			this.tabCustomers.Name = "tabCustomers";
			this.tabCustomers.Size = new System.Drawing.Size(1092, 596);
			this.tabCustomers.TabIndex = 0;
			this.tabCustomers.Text = "Клиенты";
			// 
			// btnSortByName
			// 
			this.btnSortByName.Location = new System.Drawing.Point(10, 4);
			this.btnSortByName.Name = "btnSortByName";
			this.btnSortByName.Size = new System.Drawing.Size(150, 35);
			this.btnSortByName.TabIndex = 9;
			this.btnSortByName.Text = "Сортировать по фио";
			this.btnSortByName.Click += new System.EventHandler(this.BtnSortByName_Click);
			// 
			// dgvCustomers
			// 
			this.dgvCustomers.AllowUserToAddRows = false;
			this.dgvCustomers.AllowUserToDeleteRows = false;
			this.dgvCustomers.ColumnHeadersHeight = 34;
			this.dgvCustomers.Location = new System.Drawing.Point(10, 45);
			this.dgvCustomers.Name = "dgvCustomers";
			this.dgvCustomers.ReadOnly = true;
			this.dgvCustomers.RowHeadersWidth = 62;
			this.dgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCustomers.Size = new System.Drawing.Size(1060, 460);
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
			this.tabCars.Controls.Add(this.btnSortByPrise);
			this.tabCars.Controls.Add(this.dgvCars);
			this.tabCars.Controls.Add(this.btnAddCar);
			this.tabCars.Controls.Add(this.btnEditCar);
			this.tabCars.Controls.Add(this.btnDeleteCar);
			this.tabCars.Location = new System.Drawing.Point(4, 22);
			this.tabCars.Name = "tabCars";
			this.tabCars.Size = new System.Drawing.Size(1092, 596);
			this.tabCars.TabIndex = 1;
			this.tabCars.Text = "Машины";
			// 
			// btnSortByPrise
			// 
			this.btnSortByPrise.Location = new System.Drawing.Point(10, 3);
			this.btnSortByPrise.Name = "btnSortByPrise";
			this.btnSortByPrise.Size = new System.Drawing.Size(150, 35);
			this.btnSortByPrise.TabIndex = 10;
			this.btnSortByPrise.Text = "Сортировать по цене";
			this.btnSortByPrise.Click += new System.EventHandler(this.BtnSortByPrise_Click);
			// 
			// dgvCars
			// 
			this.dgvCars.AllowUserToAddRows = false;
			this.dgvCars.AllowUserToDeleteRows = false;
			this.dgvCars.ColumnHeadersHeight = 34;
			this.dgvCars.Location = new System.Drawing.Point(10, 42);
			this.dgvCars.Name = "dgvCars";
			this.dgvCars.ReadOnly = true;
			this.dgvCars.RowHeadersWidth = 62;
			this.dgvCars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCars.Size = new System.Drawing.Size(1060, 518);
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
			this.tabOrders.Controls.Add(this.BtnLoadCurrentMonth);
			this.tabOrders.Controls.Add(this.cmbFilterCustomer);
			this.tabOrders.Controls.Add(this.cmbFilterCar);
			this.tabOrders.Controls.Add(this.dtFilterStartDate);
			this.tabOrders.Controls.Add(this.dtFilterEndDate);
			this.tabOrders.Controls.Add(this.btnSearch);
			this.tabOrders.Controls.Add(this.btnResetFilter);
			this.tabOrders.Controls.Add(this.btnSortByPrice);
			this.tabOrders.Controls.Add(this.dgvOrders);
			this.tabOrders.Controls.Add(this.btnAddOrder);
			this.tabOrders.Controls.Add(this.btnEditOrder);
			this.tabOrders.Controls.Add(this.btnDeleteOrder);
			this.tabOrders.Location = new System.Drawing.Point(4, 22);
			this.tabOrders.Name = "tabOrders";
			this.tabOrders.Size = new System.Drawing.Size(1092, 596);
			this.tabOrders.TabIndex = 2;
			this.tabOrders.Text = "Заказы";
			// 
			// BtnLoadCurrentMonth
			// 
			this.BtnLoadCurrentMonth.Location = new System.Drawing.Point(170, 39);
			this.BtnLoadCurrentMonth.Name = "BtnLoadCurrentMonth";
			this.BtnLoadCurrentMonth.Size = new System.Drawing.Size(150, 35);
			this.BtnLoadCurrentMonth.TabIndex = 23;
			this.BtnLoadCurrentMonth.Text = "Часы за месяц";
			this.BtnLoadCurrentMonth.Click += new System.EventHandler(this.BtnLoadCurrentMonth_Click);
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
			// btnSortByPrice
			// 
			this.btnSortByPrice.Location = new System.Drawing.Point(10, 39);
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
			this.dgvOrders.ColumnHeadersHeight = 34;
			this.dgvOrders.Location = new System.Drawing.Point(10, 80);
			this.dgvOrders.Name = "dgvOrders";
			this.dgvOrders.ReadOnly = true;
			this.dgvOrders.RowHeadersWidth = 62;
			this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvOrders.Size = new System.Drawing.Size(1060, 450);
			this.dgvOrders.TabIndex = 9;
			// 
			// btnAddOrder
			// 
			this.btnAddOrder.Location = new System.Drawing.Point(10, 552);
			this.btnAddOrder.Name = "btnAddOrder";
			this.btnAddOrder.Size = new System.Drawing.Size(75, 23);
			this.btnAddOrder.TabIndex = 10;
			this.btnAddOrder.Text = "Добавить";
			this.btnAddOrder.Click += new System.EventHandler(this.BtnAddOrder_Click);
			// 
			// btnEditOrder
			// 
			this.btnEditOrder.Location = new System.Drawing.Point(109, 552);
			this.btnEditOrder.Name = "btnEditOrder";
			this.btnEditOrder.Size = new System.Drawing.Size(75, 23);
			this.btnEditOrder.TabIndex = 11;
			this.btnEditOrder.Text = "Изменить";
			this.btnEditOrder.Click += new System.EventHandler(this.BtnEditOrder_Click);
			// 
			// btnDeleteOrder
			// 
			this.btnDeleteOrder.Location = new System.Drawing.Point(209, 552);
			this.btnDeleteOrder.Name = "btnDeleteOrder";
			this.btnDeleteOrder.Size = new System.Drawing.Size(75, 23);
			this.btnDeleteOrder.TabIndex = 12;
			this.btnDeleteOrder.Text = "Удалить";
			this.btnDeleteOrder.Click += new System.EventHandler(this.BtnDeleteOrder_Click);
			// 
			// tabService
			// 
			this.tabService.Controls.Add(this.dgvServices);
			this.tabService.Controls.Add(this.BtnAddService);
			this.tabService.Controls.Add(this.BtnEditService);
			this.tabService.Controls.Add(this.BtnDeleteService);
			this.tabService.Location = new System.Drawing.Point(4, 22);
			this.tabService.Name = "tabService";
			this.tabService.Padding = new System.Windows.Forms.Padding(3);
			this.tabService.Size = new System.Drawing.Size(1092, 596);
			this.tabService.TabIndex = 3;
			this.tabService.Text = "Услуги";
			// 
			// BtnAddService
			// 
			this.BtnAddService.Location = new System.Drawing.Point(6, 567);
			this.BtnAddService.Name = "BtnAddService";
			this.BtnAddService.Size = new System.Drawing.Size(75, 23);
			this.BtnAddService.TabIndex = 4;
			this.BtnAddService.Text = "Добавить";
			this.BtnAddService.Click += new System.EventHandler(this.BtnAddService_Click);
			// 
			// BtnEditService
			// 
			this.BtnEditService.Location = new System.Drawing.Point(106, 567);
			this.BtnEditService.Name = "BtnEditService";
			this.BtnEditService.Size = new System.Drawing.Size(75, 23);
			this.BtnEditService.TabIndex = 5;
			this.BtnEditService.Text = "Изменить";
			// 
			// BtnDeleteService
			// 
			this.BtnDeleteService.Location = new System.Drawing.Point(206, 567);
			this.BtnDeleteService.Name = "BtnDeleteService";
			this.BtnDeleteService.Size = new System.Drawing.Size(75, 23);
			this.BtnDeleteService.TabIndex = 6;
			this.BtnDeleteService.Text = "Удалить";
			// 
			// statusStrip
			// 
			this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 622);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1100, 22);
			this.statusStrip.TabIndex = 0;
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// dataGridView1
			// 
			this.dgvServices.AllowUserToAddRows = false;
			this.dgvServices.AllowUserToDeleteRows = false;
			this.dgvServices.ColumnHeadersHeight = 34;
			this.dgvServices.Location = new System.Drawing.Point(16, 68);
			this.dgvServices.Name = "dataGridView1";
			this.dgvServices.ReadOnly = true;
			this.dgvServices.RowHeadersWidth = 62;
			this.dgvServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvServices.Size = new System.Drawing.Size(1060, 460);
			this.dgvServices.TabIndex = 7;
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(1100, 644);
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
			this.tabService.ResumeLayout(false);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Поля элементов
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
		private System.Windows.Forms.Button btnSortByPrice;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.Button btnAddOrder;
		private System.Windows.Forms.Button btnEditOrder;
		private System.Windows.Forms.Button btnDeleteOrder;

		#endregion

		private System.Windows.Forms.Button btnSortByName;
		private System.Windows.Forms.Button btnSortByPrise;
		private System.Windows.Forms.Button BtnLoadCurrentMonth;
		private System.Windows.Forms.TabPage tabService;
		private System.Windows.Forms.Button BtnAddService;
		private System.Windows.Forms.Button BtnEditService;
		private System.Windows.Forms.Button BtnDeleteService;
		private System.Windows.Forms.DataGridView dgvServices;
	}
}
