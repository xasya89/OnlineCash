
namespace OnlineCashRmk
{
    partial class FormWriteOf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWriteOf));
            this.DateWriteOfInp = new System.Windows.Forms.DateTimePicker();
            this.DateWriteOf = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.writeofGoodTable = new System.Windows.Forms.DataGridView();
            this.ColumnGoodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGoodUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabelCountAll = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabelSumAll = new System.Windows.Forms.ToolStripStatusLabel();
            this.NoteInp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.findListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.findInpText = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.writeofGoodTable)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DateWriteOfInp
            // 
            this.DateWriteOfInp.Location = new System.Drawing.Point(136, 12);
            this.DateWriteOfInp.Name = "DateWriteOfInp";
            this.DateWriteOfInp.Size = new System.Drawing.Size(200, 27);
            this.DateWriteOfInp.TabIndex = 0;
            // 
            // DateWriteOf
            // 
            this.DateWriteOf.AutoSize = true;
            this.DateWriteOf.Location = new System.Drawing.Point(12, 17);
            this.DateWriteOf.Name = "DateWriteOf";
            this.DateWriteOf.Size = new System.Drawing.Size(118, 20);
            this.DateWriteOf.TabIndex = 1;
            this.DateWriteOf.Text = "Дата документа";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.writeofGoodTable);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Location = new System.Drawing.Point(-1, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(916, 293);
            this.panel1.TabIndex = 2;
            // 
            // writeofGoodTable
            // 
            this.writeofGoodTable.AllowUserToAddRows = false;
            this.writeofGoodTable.AllowUserToDeleteRows = false;
            this.writeofGoodTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.writeofGoodTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnGoodName,
            this.ColumnGoodUnit,
            this.ColumnCount,
            this.ColumnPrice,
            this.ColumnSum});
            this.writeofGoodTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.writeofGoodTable.Location = new System.Drawing.Point(0, 28);
            this.writeofGoodTable.Name = "writeofGoodTable";
            this.writeofGoodTable.Size = new System.Drawing.Size(916, 239);
            this.writeofGoodTable.TabIndex = 2;
            this.writeofGoodTable.SelectionChanged += new System.EventHandler(this.writeofGoodTable_SelectionChanged);
            // 
            // ColumnGoodName
            // 
            this.ColumnGoodName.FillWeight = 300F;
            this.ColumnGoodName.HeaderText = "Товар";
            this.ColumnGoodName.Name = "ColumnGoodName";
            this.ColumnGoodName.ReadOnly = true;
            this.ColumnGoodName.Width = 300;
            // 
            // ColumnGoodUnit
            // 
            this.ColumnGoodUnit.HeaderText = "Ед";
            this.ColumnGoodUnit.Name = "ColumnGoodUnit";
            this.ColumnGoodUnit.ReadOnly = true;
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
            this.ColumnPrice.ReadOnly = true;
            // 
            // ColumnSum
            // 
            this.ColumnSum.HeaderText = "Сумма";
            this.ColumnSum.Name = "ColumnSum";
            this.ColumnSum.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(916, 28);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(88, 25);
            this.toolStripButton2.Text = "Удалить";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.LabelCountAll,
            this.toolStripStatusLabel3,
            this.LabelSumAll});
            this.statusStrip1.Location = new System.Drawing.Point(0, 267);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(916, 26);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 21);
            this.toolStripStatusLabel1.Text = "Итог";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(59, 21);
            this.toolStripStatusLabel2.Text = "Кол-во";
            // 
            // LabelCountAll
            // 
            this.LabelCountAll.Name = "LabelCountAll";
            this.LabelCountAll.Size = new System.Drawing.Size(157, 21);
            this.LabelCountAll.Text = "toolStripStatusLabel3";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(58, 21);
            this.toolStripStatusLabel3.Text = "Сумма";
            // 
            // LabelSumAll
            // 
            this.LabelSumAll.Name = "LabelSumAll";
            this.LabelSumAll.Size = new System.Drawing.Size(157, 21);
            this.LabelSumAll.Text = "toolStripStatusLabel4";
            // 
            // NoteInp
            // 
            this.NoteInp.Location = new System.Drawing.Point(136, 486);
            this.NoteInp.Name = "NoteInp";
            this.NoteInp.Size = new System.Drawing.Size(766, 27);
            this.NoteInp.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 489);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Причина";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Location = new System.Drawing.Point(531, 519);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Отправить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button2.Location = new System.Drawing.Point(739, 519);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(163, 30);
            this.button2.TabIndex = 5;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.findListBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.findInpText);
            this.panel2.Location = new System.Drawing.Point(-1, 344);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(916, 136);
            this.panel2.TabIndex = 6;
            // 
            // findListBox
            // 
            this.findListBox.FormattingEnabled = true;
            this.findListBox.ItemHeight = 20;
            this.findListBox.Location = new System.Drawing.Point(13, 32);
            this.findListBox.Name = "findListBox";
            this.findListBox.Size = new System.Drawing.Size(890, 104);
            this.findListBox.TabIndex = 2;
            this.findListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.findListBox_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Поиск";
            // 
            // findInpText
            // 
            this.findInpText.Location = new System.Drawing.Point(71, 3);
            this.findInpText.Name = "findInpText";
            this.findInpText.Size = new System.Drawing.Size(832, 27);
            this.findInpText.TabIndex = 0;
            this.findInpText.TextChanged += new System.EventHandler(this.findInpText_TextChanged);
            this.findInpText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.findInpText_KeyDown);
            // 
            // FormWriteOf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NoteInp);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.DateWriteOf);
            this.Controls.Add(this.DateWriteOfInp);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormWriteOf";
            this.Text = "Списание";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWriteOf_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.writeofGoodTable)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DateWriteOfInp;
        private System.Windows.Forms.Label DateWriteOf;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView writeofGoodTable;
        private System.Windows.Forms.TextBox NoteInp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSum;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel LabelCountAll;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel LabelSumAll;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox findListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox findInpText;
    }
}