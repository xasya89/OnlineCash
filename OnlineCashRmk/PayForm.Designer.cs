
namespace OnlineCashRmk
{
    partial class PayForm
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
            this.BuyerName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DiscountBonus = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BuySum = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DiscounSum = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ItogSum = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewPayments = new System.Windows.Forms.DataGridView();
            this.ColumnTypePayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIncome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReturn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // BuyerName
            // 
            this.BuyerName.AutoSize = true;
            this.BuyerName.Location = new System.Drawing.Point(11, 23);
            this.BuyerName.Name = "BuyerName";
            this.BuyerName.Size = new System.Drawing.Size(50, 20);
            this.BuyerName.TabIndex = 20;
            this.BuyerName.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Бонус";
            // 
            // DiscountBonus
            // 
            this.DiscountBonus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DiscountBonus.Location = new System.Drawing.Point(328, 21);
            this.DiscountBonus.Name = "DiscountBonus";
            this.DiscountBonus.ReadOnly = true;
            this.DiscountBonus.Size = new System.Drawing.Size(81, 27);
            this.DiscountBonus.TabIndex = 20;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.DiscountBonus);
            this.groupBox1.Controls.Add(this.BuyerName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 61);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Покупатель";
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(425, 18);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(106, 30);
            this.button5.TabIndex = 21;
            this.button5.Text = "Списать (Del)";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Покупка";
            // 
            // BuySum
            // 
            this.BuySum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BuySum.Location = new System.Drawing.Point(81, 6);
            this.BuySum.Name = "BuySum";
            this.BuySum.ReadOnly = true;
            this.BuySum.Size = new System.Drawing.Size(86, 27);
            this.BuySum.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(173, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Скидка";
            // 
            // DiscounSum
            // 
            this.DiscounSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DiscounSum.Location = new System.Drawing.Point(236, 6);
            this.DiscounSum.Name = "DiscounSum";
            this.DiscounSum.ReadOnly = true;
            this.DiscounSum.Size = new System.Drawing.Size(81, 27);
            this.DiscounSum.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(328, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "К оплате";
            // 
            // ItogSum
            // 
            this.ItogSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ItogSum.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ItogSum.Location = new System.Drawing.Point(397, 6);
            this.ItogSum.Name = "ItogSum";
            this.ItogSum.ReadOnly = true;
            this.ItogSum.Size = new System.Drawing.Size(89, 27);
            this.ItogSum.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Image = global::OnlineCashRmk.Properties.Resources.money;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(49, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 43);
            this.button1.TabIndex = 12;
            this.button1.Text = "Наличные (F5)";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = global::OnlineCashRmk.Properties.Resources.credit_cart;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(300, 105);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 43);
            this.button2.TabIndex = 13;
            this.button2.Text = "Карта (F6)";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.DiscounSum);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.BuySum);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.ItogSum);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 38);
            this.panel1.TabIndex = 21;
            // 
            // dataGridViewPayments
            // 
            this.dataGridViewPayments.AllowUserToAddRows = false;
            this.dataGridViewPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPayments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTypePayment,
            this.ColumnIncome,
            this.ColumnSum,
            this.ColumnReturn});
            this.dataGridViewPayments.Location = new System.Drawing.Point(8, 154);
            this.dataGridViewPayments.MultiSelect = false;
            this.dataGridViewPayments.Name = "dataGridViewPayments";
            this.dataGridViewPayments.RowHeadersVisible = false;
            this.dataGridViewPayments.RowTemplate.Height = 25;
            this.dataGridViewPayments.Size = new System.Drawing.Size(523, 111);
            this.dataGridViewPayments.TabIndex = 22;
            this.dataGridViewPayments.CurrentCellChanged += new System.EventHandler(this.dataGridViewPayments_CurrentCellChanged);
            this.dataGridViewPayments.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewPayments_RowsAdded);
            // 
            // ColumnTypePayment
            // 
            this.ColumnTypePayment.HeaderText = "Тип платежа";
            this.ColumnTypePayment.Name = "ColumnTypePayment";
            this.ColumnTypePayment.ReadOnly = true;
            this.ColumnTypePayment.Width = 150;
            // 
            // ColumnIncome
            // 
            this.ColumnIncome.HeaderText = "Дал покупатель";
            this.ColumnIncome.Name = "ColumnIncome";
            this.ColumnIncome.Width = 150;
            // 
            // ColumnSum
            // 
            this.ColumnSum.HeaderText = "Сумма";
            this.ColumnSum.Name = "ColumnSum";
            // 
            // ColumnReturn
            // 
            this.ColumnReturn.HeaderText = "Сдача";
            this.ColumnReturn.Name = "ColumnReturn";
            this.ColumnReturn.ReadOnly = true;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.LightGreen;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(300, 271);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(161, 43);
            this.button3.TabIndex = 12;
            this.button3.Text = "Оплатить (Enter)";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightSalmon;
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(49, 271);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(161, 43);
            this.button4.TabIndex = 12;
            this.button4.Text = "Отмена (Esc)";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // PayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 322);
            this.Controls.Add(this.dataGridViewPayments);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сложная оплата";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPayments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label BuyerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DiscountBonus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox BuySum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DiscounSum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ItogSum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridViewPayments;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTypePayment;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIncome;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReturn;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}