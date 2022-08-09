using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCashRmk
{
    public partial class FormPaymentNoElectron : Form
    {
        decimal sumBuy = 0;
        public FormPaymentNoElectron(decimal sumBuy)
        {
            this.sumBuy = sumBuy;
            InitializeComponent();
            SumBuyTextBox.Text = sumBuy.ToString();
            SumBuyerTextBox.Text = sumBuy.ToString();
        }

        private void SumBuyerTextBox_TextChanged(object sender, EventArgs e)
        {
            decimal sumBuyer = 0;
            decimal.TryParse(SumBuyerTextBox.Text, out sumBuyer);
            decimal ost = sumBuyer-sumBuy;
            SumCostTextBox.Text = ost.ToString();
            if (ost < 0)
                SumCostTextBox.ForeColor = Color.Red;
            else
                SumCostTextBox.ForeColor = SystemColors.WindowText;
        }

        private void FormPaymentNoElectron_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    DialogResult = DialogResult.OK;
                    break;
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
