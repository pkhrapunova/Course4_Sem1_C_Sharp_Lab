namespace CarRental.UI
{
	partial class FormAddEditService
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblName = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblPrice = new System.Windows.Forms.Label();
			this.numPrice = new System.Windows.Forms.NumericUpDown();
			this.btnSave = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(30, 30);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(95, 17);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Название услуги:";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(150, 27);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(220, 22);
			this.txtName.TabIndex = 1;
			// 
			// lblPrice
			// 
			this.lblPrice.AutoSize = true;
			this.lblPrice.Location = new System.Drawing.Point(30, 75);
			this.lblPrice.Name = "lblPrice";
			this.lblPrice.Size = new System.Drawing.Size(42, 17);
			this.lblPrice.TabIndex = 2;
			this.lblPrice.Text = "Цена:";
			// 
			// numPrice
			// 
			this.numPrice.DecimalPlaces = 2;
			this.numPrice.Location = new System.Drawing.Point(150, 73);
			this.numPrice.Maximum = new decimal(new int[] {
			100000,
			0,
			0,
			0});
			this.numPrice.Name = "numPrice";
			this.numPrice.Size = new System.Drawing.Size(120, 22);
			this.numPrice.TabIndex = 3;
			this.numPrice.ThousandsSeparator = true;
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.LightSteelBlue;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(150, 120);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(120, 35);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "Сохранить";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// FormAddEditService
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 190);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.numPrice);
			this.Controls.Add(this.lblPrice);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Name = "FormAddEditService";
			this.Text = "Добавление / Редактирование услуги";
			((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblPrice;
		private System.Windows.Forms.NumericUpDown numPrice;
		private System.Windows.Forms.Button btnSave;
		#endregion
	}
}