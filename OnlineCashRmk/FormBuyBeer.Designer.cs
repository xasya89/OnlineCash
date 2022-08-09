
namespace OnlineCashRmk
{
    partial class FormBuyBeer
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
            this.GoodNameLabel = new System.Windows.Forms.TextBox();
            this.BottleListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonMinus = new System.Windows.Forms.Button();
            this.CountTextBox = new System.Windows.Forms.TextBox();
            this.ButtonPlus = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Товар:";
            // 
            // GoodNameLabel
            // 
            this.GoodNameLabel.Location = new System.Drawing.Point(73, 15);
            this.GoodNameLabel.Name = "GoodNameLabel";
            this.GoodNameLabel.ReadOnly = true;
            this.GoodNameLabel.Size = new System.Drawing.Size(491, 29);
            this.GoodNameLabel.TabIndex = 10;
            // 
            // BottleListBox
            // 
            this.BottleListBox.FormattingEnabled = true;
            this.BottleListBox.ItemHeight = 21;
            this.BottleListBox.Location = new System.Drawing.Point(12, 50);
            this.BottleListBox.Name = "BottleListBox";
            this.BottleListBox.Size = new System.Drawing.Size(552, 193);
            this.BottleListBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 264);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Кол-во:";
            // 
            // ButtonMinus
            // 
            this.ButtonMinus.Location = new System.Drawing.Point(80, 254);
            this.ButtonMinus.Name = "ButtonMinus";
            this.ButtonMinus.Size = new System.Drawing.Size(40, 41);
            this.ButtonMinus.TabIndex = 10;
            this.ButtonMinus.Text = "-";
            this.ButtonMinus.UseVisualStyleBackColor = true;
            this.ButtonMinus.Click += new System.EventHandler(this.ButtonMinus_Click);
            // 
            // CountTextBox
            // 
            this.CountTextBox.Location = new System.Drawing.Point(126, 261);
            this.CountTextBox.Name = "CountTextBox";
            this.CountTextBox.Size = new System.Drawing.Size(100, 29);
            this.CountTextBox.TabIndex = 2;
            // 
            // ButtonPlus
            // 
            this.ButtonPlus.Location = new System.Drawing.Point(232, 254);
            this.ButtonPlus.Name = "ButtonPlus";
            this.ButtonPlus.Size = new System.Drawing.Size(40, 41);
            this.ButtonPlus.TabIndex = 10;
            this.ButtonPlus.Text = "-";
            this.ButtonPlus.UseVisualStyleBackColor = true;
            this.ButtonPlus.Click += new System.EventHandler(this.ButtonPlus_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.LightCoral;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(300, 254);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 41);
            this.button3.TabIndex = 3;
            this.button3.Text = "Отмена Esc";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightGreen;
            this.button4.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button4.Location = new System.Drawing.Point(405, 254);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(159, 41);
            this.button4.TabIndex = 4;
            this.button4.Text = "Добавить Enter";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // FormBuyBeer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 307);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.CountTextBox);
            this.Controls.Add(this.ButtonPlus);
            this.Controls.Add(this.ButtonMinus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BottleListBox);
            this.Controls.Add(this.GoodNameLabel);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormBuyBeer";
            this.Text = "Покупка пива";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormBuyBeer_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.FormBuyBeer_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox GoodNameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonMinus;
        private System.Windows.Forms.Button ButtonPlus;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.ListBox BottleListBox;
        public System.Windows.Forms.TextBox CountTextBox;
    }
}