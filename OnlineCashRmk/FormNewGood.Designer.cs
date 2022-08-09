
namespace OnlineCashRmk
{
    partial class FormNewGood
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
            this.goodName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.goodBarCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.goodUnit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.goodType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.goodPrice = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // goodName
            // 
            this.goodName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.goodName.Location = new System.Drawing.Point(108, 12);
            this.goodName.Name = "goodName";
            this.goodName.Size = new System.Drawing.Size(233, 23);
            this.goodName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Наименование";
            // 
            // goodBarCode
            // 
            this.goodBarCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.goodBarCode.Location = new System.Drawing.Point(108, 41);
            this.goodBarCode.Name = "goodBarCode";
            this.goodBarCode.Size = new System.Drawing.Size(150, 23);
            this.goodBarCode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Штрих код";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // goodUnit
            // 
            this.goodUnit.FormattingEnabled = true;
            this.goodUnit.Items.AddRange(new object[] {
            "шт",
            "л",
            "кг",
            "м",
            "уп",
            "см"});
            this.goodUnit.Location = new System.Drawing.Point(108, 70);
            this.goodUnit.Name = "goodUnit";
            this.goodUnit.Size = new System.Drawing.Size(103, 23);
            this.goodUnit.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Ед изм.";
            this.label3.Click += new System.EventHandler(this.label2_Click);
            // 
            // goodType
            // 
            this.goodType.FormattingEnabled = true;
            this.goodType.Items.AddRange(new object[] {
            "",
            "Пиво",
            "Пакет"});
            this.goodType.Location = new System.Drawing.Point(108, 99);
            this.goodType.Name = "goodType";
            this.goodType.Size = new System.Drawing.Size(103, 23);
            this.goodType.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Тип";
            this.label4.Click += new System.EventHandler(this.label2_Click);
            // 
            // goodPrice
            // 
            this.goodPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.goodPrice.Location = new System.Drawing.Point(108, 128);
            this.goodPrice.Name = "goodPrice";
            this.goodPrice.Size = new System.Drawing.Size(103, 23);
            this.goodPrice.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Цена продажи";
            this.label5.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGreen;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(228, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Salmon;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(122, 171);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 30);
            this.button2.TabIndex = 6;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(266, 41);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Создать";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormNewGood
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 218);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.goodType);
            this.Controls.Add(this.goodUnit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goodPrice);
            this.Controls.Add(this.goodBarCode);
            this.Controls.Add(this.goodName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormNewGood";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Создание товара";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox goodName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox goodBarCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox goodUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox goodType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox goodPrice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}