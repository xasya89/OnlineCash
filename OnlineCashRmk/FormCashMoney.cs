using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineCashRmk.Models;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Services;

namespace OnlineCashRmk
{
    public partial class FormCashMoney : Form
    {
        DataContext _db;
        ISynchService _synchBackground;
        TypeCashMoneyOpertaion typeOperation;
        public FormCashMoney(IDbContextFactory<DataContext> dbFactory, ISynchService synchBackground)
        {
            _db = dbFactory.CreateDbContext();
            FormClosed += (s,e)=> { _db.Dispose(); };
            _synchBackground = synchBackground;
            InitializeComponent();
        }

        public void Open(TypeCashMoneyOpertaion typeOperation)
        {
            this.typeOperation = typeOperation;
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                decimal sum = Sum.Text.ToDecimal();
                var cashMoney = new CashMoney
                {
                    Create = Create.Value,
                    TypeOperation = typeOperation,
                    Sum = sum,
                    Note = Note.Text
                };
                _db.CashMoneys.Add(cashMoney);
                _db.SaveChanges();
                _synchBackground.AppendDoc(new DocSynch {DocId=cashMoney.Id, Create=DateTime.Now, TypeDoc=TypeDocs.CashMoney });
                Close();
            }
            catch (SystemException) { }
            catch (Exception ex) { };
        }

        private void Create_ValueChanged(object sender, EventArgs e)
        {
            if (DateTime.Compare(DateTime.Now.Date, Create.Value.Date) == -1)
                Create.Value = DateTime.Now;
        }
    }
}
