using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.DataModels;

namespace OnlineCashRmk
{
    public partial class FormPaymentCombine : Form
    {
        DataContext _db;
        public FormPaymentCombine(IDbContextFactory<DataContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
            InitializeComponent();
        }

        private decimal SumSell { get; set; }
        public List<CheckPaymentDataModel> Show(decimal sumSeell)
        {
            SumSell = sumSeell;
            label2.Text = sumSeell.ToSellFormat();
            if (ShowDialog() == DialogResult.OK)
            {
                List<CheckPaymentDataModel> payments = new List<CheckPaymentDataModel>();
                if (textBoxCash.Text.ToDecimal() != 0)
                    payments.Add(new CheckPaymentDataModel { TypePayment = TypePayment.Cash, Sum = textBoxCash.Text.ToDecimal() });
                if (textBoxElectron.Text.ToDecimal() != 0)
                    payments.Add(new CheckPaymentDataModel { TypePayment = TypePayment.Electron, Sum = textBoxElectron.Text.ToDecimal() });
                return payments;
            }
            return null;
        }

        private void FormPaymentCombine_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    textBoxCash.Focus();
                    break;
                case Keys.F6:
                    textBoxElectron.Focus();
                    break;
                case Keys.Enter:
                    if (checkSumPay())
                        DialogResult = DialogResult.OK;
                    break;
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGreen;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = SystemColors.Window;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void textBoxCash_TextChanged(object sender, EventArgs e)
        {
            decimal cash = textBoxCash.Text.ToDecimal();
            decimal electron = textBoxElectron.Text.ToDecimal();
            textBoxSummary.Text = (cash + electron).ToSellFormat();
            textBoxSummary.BackColor = SumSell != cash + electron ? Color.LightPink : Color.LightGreen;
        }

        bool checkSumPay() => SumSell == textBoxCash.Text.ToDecimal() + textBoxElectron.Text.ToDecimal();

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (checkSumPay())
                DialogResult = DialogResult.OK;
        }

        private void FormPaymentCombine_FormClosed(object sender, FormClosedEventArgs e)
        {
            _db.Dispose();
        }
    }
}
