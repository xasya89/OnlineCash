
namespace OnlineCashRmk
{
    partial class FormCashMoney
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
            this.Create = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.Sum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Note = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Create
            // 
            this.Create.Location = new System.Drawing.Point(61, 12);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(200, 23);
            this.Create.TabIndex = 0;
            this.Create.ValueChanged += new System.EventHandler(this.Create_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Сумма";
            // 
            // Sum
            // 
            this.Sum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Sum.Location = new System.Drawing.Point(63, 41);
            this.Sum.Name = "Sum";
            this.Sum.Size = new System.Drawing.Size(198, 23);
            this.Sum.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Описание";
            // 
            // Note
            // 
            this.Note.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Note.Location = new System.Drawing.Point(12, 88);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(249, 96);
            this.Note.TabIndex = 4;
            this.Note.Text = "";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGreen;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(135, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormCashMoney
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 237);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Note);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Sum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Create);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCashMoney";
            this.Text = "FormCashMoney";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker Create;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Sum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox Note;
        private System.Windows.Forms.Button button1;
    }
}