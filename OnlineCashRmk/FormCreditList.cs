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

namespace OnlineCashRmk
{
    public partial class FormCreditList : Form
    {
        DataContext db;
        public FormCreditList()
        {
            InitializeComponent();
            TopMost = false;
            var optBuilder = new DbContextOptionsBuilder();
            optBuilder.UseSqlite("Data Source=CustomerDB.db;");
            db = new DataContext(optBuilder.Options);
            Select();
        }

        void Select()
        {
            creditTable.Rows.Clear();
            var credits = db.Credits.Where(c => EF.Functions.Like(c.Creditor, $"%{findTextBox.Text}%") & c.SumCredit > 0).OrderByDescending(c=>c.DateCreate).ToList();
            foreach (var credit in credits)
                creditTable.Rows.Add(
                    credit.Id,
                    credit.DateCreate.ToString("dd.MM"),
                    credit.Creditor,
                    credit.SumCredit,
                    "оплатить"
                    );
        }

        private void creditTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void creditTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(creditTable.SelectedRows.Count>0)
            if (e.ColumnIndex == ColumnPay.Index)
            {
                    DataGridViewRow row = creditTable.SelectedRows[0];
                    int id = Convert.ToInt32(row.Cells[ColumnId.Name].Value);
                    var credit = db.Credits.Where(c => c.Id == id).FirstOrDefault();
                    FormCreditAdd fr = new FormCreditAdd();
                    fr.TopMost = false;
                    fr.Creditor.Text = credit.Creditor;
                    fr.Creditor.Enabled = false;
                    fr.SumCredit.Text = credit.SumCredit.ToString();
                    if(fr.ShowDialog()==DialogResult.OK)
                    {
                        decimal sumPay = 0;
                        decimal.TryParse(fr.SumCredit.Text, out sumPay);
                        if (sumPay > 0)
                        {
                            var payment = new CreditPayment
                            {
                                Credit = credit,
                                DatePayment = DateTime.Now,
                                Sum = sumPay
                            };
                            db.CreditPayments.Add(payment);
                            credit.isSynch = false;
                            credit.SumCredit -= sumPay;
                            db.SaveChanges();
                            row.Cells[ColumnSum.Name].Value = credit.SumCredit;
                            if (credit.SumCredit == 0)
                                creditTable.Rows.Remove(row);
                        }
                    }
            }
        }
    }
}
