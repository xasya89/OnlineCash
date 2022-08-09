
namespace OnlineCashRmk
{
    partial class FormPaymentNoElectron
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
            this.label1 = new System.Windows.Forms.Label();
            this.SumBuyTextBox = new System.Windows.Forms.TextBox();
            this.SumBuyerTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SumCostTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "К оплате:";
            // 
            // SumBuyTextBox
            // 
            this.SumBuyTextBox.Location = new System.Drawing.Point(115, 12);
            this.SumBuyTextBox.Name = "SumBuyTextBox";
            this.SumBuyTextBox.ReadOnly = true;
            this.SumBuyTextBox.Size = new System.Drawing.Size(190, 29);
            this.SumBuyTextBox.TabIndex = 10;
            this.SumBuyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SumBuyerTextBox
            // 
            this.SumBuyerTextBox.Location = new System.Drawing.Point(115, 47);
            this.SumBuyerTextBox.Name = "SumBuyerTextBox";
            this.SumBuyerTextBox.Size = new System.Drawing.Size(190, 29);
            this.SumBuyerTextBox.TabIndex = 1;
            this.SumBuyerTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SumBuyerTextBox.TextChanged += new System.EventHandler(this.SumBuyerTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Покупатель:";
            // 
            // SumCostTextBox
            // 
            this.SumCostTextBox.Location = new System.Drawing.Point(115, 82);
            this.SumCostTextBox.Name = "SumCostTextBox";
            this.SumCostTextBox.ReadOnly = true;
            this.SumCostTextBox.Size = new System.Drawing.Size(190, 29);
            this.SumCostTextBox.TabIndex = 10;
            this.SumCostTextBox.Text = "0";
            this.SumCostTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "Сдача:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightSalmon;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(12, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 41);
            this.button1.TabIndex = 2;
            this.button1.Text = "Отмена Esc";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightGreen;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(169, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(136, 41);
            this.button2.TabIndex = 3;
            this.button2.Text = "Оплатить Enter";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormPaymentNoElectron
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 183);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SumCostTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SumBuyerTextBox);
            this.Controls.Add(this.SumBuyTextBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormPaymentNoElectron";
            this.Text = "FormPaymentNoElectron";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPaymentNoElectron_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SumBuyTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SumCostTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.TextBox SumBuyerTextBox;
    }
}