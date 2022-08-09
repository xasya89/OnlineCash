
namespace OnlineCashRmk
{
    partial class FormArrival
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.SupplierComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.arrivalNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.arrivalDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelSumArrival = new System.Windows.Forms.Label();
            this.labelSumNds = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelSumSell = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridViewPositions = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.findListBox = new System.Windows.Forms.ListBox();
            this.findTextBox = new System.Windows.Forms.TextBox();
            this.Column_GoodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PriceArrival = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PriceSell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PricePercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_NdsPercent = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_SumNds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_SumArrival = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_SumSell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ExpiresDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPositions)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SupplierComboBox
            // 
            this.SupplierComboBox.FormattingEnabled = true;
            this.SupplierComboBox.Location = new System.Drawing.Point(500, 12);
            this.SupplierComboBox.Name = "SupplierComboBox";
            this.SupplierComboBox.Size = new System.Drawing.Size(225, 23);
            this.SupplierComboBox.TabIndex = 3;
            this.SupplierComboBox.SelectedIndexChanged += new System.EventHandler(this.SupplierComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Номер";
            // 
            // arrivalNum
            // 
            this.arrivalNum.Location = new System.Drawing.Point(64, 12);
            this.arrivalNum.Name = "arrivalNum";
            this.arrivalNum.Size = new System.Drawing.Size(100, 23);
            this.arrivalNum.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Дата";
            // 
            // arrivalDate
            // 
            this.arrivalDate.Location = new System.Drawing.Point(208, 12);
            this.arrivalDate.Name = "arrivalDate";
            this.arrivalDate.Size = new System.Drawing.Size(200, 23);
            this.arrivalDate.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(424, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Поставшик";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.arrivalNum);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.SupplierComboBox);
            this.panel1.Controls.Add(this.arrivalDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1041, 77);
            this.panel1.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(131, 51);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Новый товар";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.labelSumArrival);
            this.panel4.Controls.Add(this.labelSumNds);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.labelSumSell);
            this.panel4.Location = new System.Drawing.Point(298, 41);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(731, 29);
            this.panel4.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Всего по закупке";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(224, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "НДС";
            // 
            // labelSumArrival
            // 
            this.labelSumArrival.AutoSize = true;
            this.labelSumArrival.Location = new System.Drawing.Point(126, 8);
            this.labelSumArrival.Name = "labelSumArrival";
            this.labelSumArrival.Size = new System.Drawing.Size(38, 15);
            this.labelSumArrival.TabIndex = 7;
            this.labelSumArrival.Text = "label5";
            // 
            // labelSumNds
            // 
            this.labelSumNds.AutoSize = true;
            this.labelSumNds.Location = new System.Drawing.Point(275, 8);
            this.labelSumNds.Name = "labelSumNds";
            this.labelSumNds.Size = new System.Drawing.Size(38, 15);
            this.labelSumNds.TabIndex = 10;
            this.labelSumNds.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(378, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "по продаже";
            // 
            // labelSumSell
            // 
            this.labelSumSell.AutoSize = true;
            this.labelSumSell.Location = new System.Drawing.Point(469, 8);
            this.labelSumSell.Name = "labelSumSell";
            this.labelSumSell.Size = new System.Drawing.Size(38, 15);
            this.labelSumSell.TabIndex = 9;
            this.labelSumSell.Text = "label6";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Удалить (del)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridViewPositions
            // 
            this.dataGridViewPositions.AllowUserToAddRows = false;
            this.dataGridViewPositions.AllowUserToDeleteRows = false;
            this.dataGridViewPositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPositions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_GoodName,
            this.Column_Unit,
            this.Column_PriceArrival,
            this.Column_PriceSell,
            this.Column_PricePercent,
            this.Column_Count,
            this.Column_NdsPercent,
            this.Column_SumNds,
            this.Column_SumArrival,
            this.Column_SumSell,
            this.Column_ExpiresDate});
            this.dataGridViewPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPositions.Location = new System.Drawing.Point(0, 77);
            this.dataGridViewPositions.MultiSelect = false;
            this.dataGridViewPositions.Name = "dataGridViewPositions";
            this.dataGridViewPositions.RowHeadersVisible = false;
            this.dataGridViewPositions.RowTemplate.Height = 25;
            this.dataGridViewPositions.Size = new System.Drawing.Size(1041, 340);
            this.dataGridViewPositions.TabIndex = 0;
            this.dataGridViewPositions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPositions_CellClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 597);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1041, 44);
            this.panel2.TabIndex = 8;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.Salmon;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(613, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 31);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.LightGreen;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Location = new System.Drawing.Point(797, 6);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(112, 31);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 417);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1041, 180);
            this.panel3.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.findListBox);
            this.groupBox1.Controls.Add(this.findTextBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1041, 180);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Поиск товара (F4)";
            // 
            // findListBox
            // 
            this.findListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findListBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.findListBox.FormattingEnabled = true;
            this.findListBox.ItemHeight = 21;
            this.findListBox.Location = new System.Drawing.Point(3, 52);
            this.findListBox.Name = "findListBox";
            this.findListBox.Size = new System.Drawing.Size(1035, 125);
            this.findListBox.TabIndex = 1;
            this.findListBox.DoubleClick += new System.EventHandler(this.findListBox_DoubleClick);
            // 
            // findTextBox
            // 
            this.findTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.findTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.findTextBox.Location = new System.Drawing.Point(3, 19);
            this.findTextBox.Name = "findTextBox";
            this.findTextBox.Size = new System.Drawing.Size(1035, 33);
            this.findTextBox.TabIndex = 5;
            this.findTextBox.TextChanged += new System.EventHandler(this.findTextBox_TextChanged);
            this.findTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.findTextBox_KeyDown);
            // 
            // Column_GoodName
            // 
            this.Column_GoodName.HeaderText = "Наименование";
            this.Column_GoodName.Name = "Column_GoodName";
            this.Column_GoodName.ReadOnly = true;
            this.Column_GoodName.Width = 160;
            // 
            // Column_Unit
            // 
            this.Column_Unit.HeaderText = "Ед";
            this.Column_Unit.Name = "Column_Unit";
            this.Column_Unit.ReadOnly = true;
            this.Column_Unit.Width = 60;
            // 
            // Column_PriceArrival
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.Column_PriceArrival.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column_PriceArrival.HeaderText = "Цена";
            this.Column_PriceArrival.Name = "Column_PriceArrival";
            // 
            // Column_PriceSell
            // 
            this.Column_PriceSell.HeaderText = "Цена продажи";
            this.Column_PriceSell.Name = "Column_PriceSell";
            // 
            // Column_PricePercent
            // 
            this.Column_PricePercent.HeaderText = "Наценка %";
            this.Column_PricePercent.Name = "Column_PricePercent";
            this.Column_PricePercent.ReadOnly = true;
            // 
            // Column_Count
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.Column_Count.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_Count.HeaderText = "Кол-во";
            this.Column_Count.Name = "Column_Count";
            // 
            // Column_NdsPercent
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.Column_NdsPercent.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_NdsPercent.HeaderText = "% НДС";
            this.Column_NdsPercent.Items.AddRange(new object[] {
            "20%",
            "10%",
            "0%",
            "Без ндс"});
            this.Column_NdsPercent.Name = "Column_NdsPercent";
            // 
            // Column_SumNds
            // 
            this.Column_SumNds.HeaderText = "НДС";
            this.Column_SumNds.Name = "Column_SumNds";
            this.Column_SumNds.ReadOnly = true;
            this.Column_SumNds.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_SumNds.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_SumArrival
            // 
            this.Column_SumArrival.HeaderText = "Сумма покупки";
            this.Column_SumArrival.Name = "Column_SumArrival";
            this.Column_SumArrival.ReadOnly = true;
            // 
            // Column_SumSell
            // 
            this.Column_SumSell.HeaderText = "Сумма продажи";
            this.Column_SumSell.Name = "Column_SumSell";
            this.Column_SumSell.ReadOnly = true;
            // 
            // Column_ExpiresDate
            // 
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.Column_ExpiresDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column_ExpiresDate.HeaderText = "Срок годности";
            this.Column_ExpiresDate.Name = "Column_ExpiresDate";
            // 
            // FormArrival
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 641);
            this.Controls.Add(this.dataGridViewPositions);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "FormArrival";
            this.Text = "FormArrival";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormArrival_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormArrival_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPositions)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox SupplierComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox arrivalNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker arrivalDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridViewPositions;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox findListBox;
        private System.Windows.Forms.TextBox findTextBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelSumSell;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSumArrival;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSumNds;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_GoodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PriceArrival;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PriceSell;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PricePercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Count;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_NdsPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_SumNds;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_SumArrival;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_SumSell;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ExpiresDate;
    }
}