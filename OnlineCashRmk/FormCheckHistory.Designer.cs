
namespace OnlineCashRmk
{
    partial class FormCheckHistory
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewChecks = new System.Windows.Forms.DataGridView();
            this.ColumnDateCreate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSumAll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTypePayment = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnTypeReturn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheck = new System.Windows.Forms.DataGridView();
            this.ColumnGoodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonReturn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChecks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCheck)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 35);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewChecks);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewCheck);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(984, 504);
            this.splitContainer1.SplitterDistance = 317;
            this.splitContainer1.TabIndex = 2;
            // 
            // dataGridViewChecks
            // 
            this.dataGridViewChecks.AllowUserToAddRows = false;
            this.dataGridViewChecks.AllowUserToDeleteRows = false;
            this.dataGridViewChecks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChecks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDateCreate,
            this.ColumnSumAll,
            this.ColumnTypePayment,
            this.ColumnTypeReturn});
            this.dataGridViewChecks.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridViewChecks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewChecks.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewChecks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewChecks.Name = "dataGridViewChecks";
            this.dataGridViewChecks.RowHeadersVisible = false;
            this.dataGridViewChecks.RowTemplate.Height = 25;
            this.dataGridViewChecks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewChecks.Size = new System.Drawing.Size(317, 504);
            this.dataGridViewChecks.TabIndex = 0;
            this.dataGridViewChecks.SelectionChanged += new System.EventHandler(this.dataGridViewChecks_SelectionChanged);
            // 
            // ColumnDateCreate
            // 
            dataGridViewCellStyle1.Format = "t";
            dataGridViewCellStyle1.NullValue = null;
            this.ColumnDateCreate.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnDateCreate.HeaderText = "Время";
            this.ColumnDateCreate.Name = "ColumnDateCreate";
            this.ColumnDateCreate.Width = 80;
            // 
            // ColumnSumAll
            // 
            this.ColumnSumAll.HeaderText = "Сумма";
            this.ColumnSumAll.Name = "ColumnSumAll";
            // 
            // ColumnTypePayment
            // 
            this.ColumnTypePayment.HeaderText = "Нал";
            this.ColumnTypePayment.Name = "ColumnTypePayment";
            this.ColumnTypePayment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnTypePayment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnTypePayment.Width = 40;
            // 
            // ColumnTypeReturn
            // 
            this.ColumnTypeReturn.HeaderText = "Возврат";
            this.ColumnTypeReturn.Name = "ColumnTypeReturn";
            this.ColumnTypeReturn.ReadOnly = true;
            this.ColumnTypeReturn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnTypeReturn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnTypeReturn.Width = 60;
            // 
            // dataGridViewCheck
            // 
            this.dataGridViewCheck.AllowUserToAddRows = false;
            this.dataGridViewCheck.AllowUserToDeleteRows = false;
            this.dataGridViewCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCheck.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnGoodName,
            this.ColumnCount,
            this.ColumnPrice,
            this.ColumnSum});
            this.dataGridViewCheck.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridViewCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCheck.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewCheck.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCheck.Name = "dataGridViewCheck";
            this.dataGridViewCheck.RowHeadersVisible = false;
            this.dataGridViewCheck.RowTemplate.Height = 25;
            this.dataGridViewCheck.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCheck.Size = new System.Drawing.Size(663, 442);
            this.dataGridViewCheck.TabIndex = 1;
            // 
            // ColumnGoodName
            // 
            this.ColumnGoodName.HeaderText = "Наименование";
            this.ColumnGoodName.Name = "ColumnGoodName";
            this.ColumnGoodName.Width = 350;
            // 
            // ColumnCount
            // 
            this.ColumnCount.HeaderText = "Кол-во";
            this.ColumnCount.Name = "ColumnCount";
            // 
            // ColumnPrice
            // 
            this.ColumnPrice.HeaderText = "Цена";
            this.ColumnPrice.Name = "ColumnPrice";
            // 
            // ColumnSum
            // 
            this.ColumnSum.HeaderText = "Сумма";
            this.ColumnSum.Name = "ColumnSum";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonReturn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 442);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(663, 62);
            this.panel2.TabIndex = 0;
            // 
            // buttonReturn
            // 
            this.buttonReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReturn.Location = new System.Drawing.Point(476, 6);
            this.buttonReturn.Name = "buttonReturn";
            this.buttonReturn.Size = new System.Drawing.Size(157, 40);
            this.buttonReturn.TabIndex = 0;
            this.buttonReturn.Text = "Оформить возврат";
            this.buttonReturn.UseVisualStyleBackColor = true;
            this.buttonReturn.Click += new System.EventHandler(this.buttonReturn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 35);
            this.panel1.TabIndex = 3;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(54, 7);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 25);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дата";
            // 
            // FormCheckHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormCheckHistory";
            this.Text = "История продаж за смену";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChecks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCheck)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewChecks;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewCheck;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonReturn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateCreate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSumAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnTypePayment;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnTypeReturn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSum;
    }
}