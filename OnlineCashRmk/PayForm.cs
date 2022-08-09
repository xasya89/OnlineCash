using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.DataModels;
using System.Collections.ObjectModel;

namespace OnlineCashRmk
{
    public partial class PayForm : Form
    {
        DataContext _db;
        CheckSell _checkSell;
        BindingSource bindingCheckSell = new BindingSource();
        Buyer _buyer=new Buyer();
        BindingSource bindingBuyer = new BindingSource();
        ObservableCollection<CheckPaymentDataModel> CheckPayments = new ObservableCollection<CheckPaymentDataModel>();
        BindingSource bindingPayments = new BindingSource();

        SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) => {
            var activeform = Form.ActiveForm;
            if (activeform != null && nameof(Form1) == activeform.Name)
            {
                var port = (SerialPort)s;
                string code = port.ReadExisting();
                var form = activeform as PayForm;
            }
        });

        void ResetBinding()
        {
            bindingBuyer.DataSource = _buyer;
            bindingBuyer.ResetBindings(false);
        }

        public PayForm(DataContext db)
        {
            InitializeComponent();
            _db = db;
            bindingCheckSell.DataSource = _checkSell;
            BuySum.DataBindings.Add("Text", bindingCheckSell, nameof(CheckSell.Sum),false, DataSourceUpdateMode.OnPropertyChanged);
            DiscounSum.DataBindings.Add("Text", bindingCheckSell, nameof(CheckSell.SumDiscont), false, DataSourceUpdateMode.OnPropertyChanged);
            ItogSum.DataBindings.Add("Text", bindingCheckSell, nameof(CheckSell.SumAll), false, DataSourceUpdateMode.OnPropertyChanged);

            bindingBuyer.DataSource = _buyer;
            BuyerName.DataBindings.Add("Text", bindingBuyer, nameof(Buyer.Name), false, DataSourceUpdateMode.OnPropertyChanged);

            bindingPayments.DataSource = CheckPayments;
            dataGridViewPayments.AutoGenerateColumns = false;
            dataGridViewPayments.DataSource = bindingPayments;
            ColumnTypePayment.DataPropertyName = nameof(CheckPaymentDataModel.TypePaymentStr);
            ColumnIncome.DataPropertyName = nameof(CheckPaymentDataModel.Income);
            ColumnSum.DataPropertyName = nameof(CheckPaymentDataModel.Sum);
            ColumnReturn.DataPropertyName = nameof(CheckPaymentDataModel.Return);
            CheckPayments.CollectionChanged += (s, e) =>
            {
                bindingPayments.ResetBindings(false);
                var payment = s as CheckPaymentDataModel;
            };
        }

        public List<CheckPaymentDataModel> Pay(decimal sum)
        {
            _checkSell = new CheckSell();
            _checkSell.Sum = sum;
            _checkSell.SumAll = sum;
            bindingCheckSell.DataSource = _checkSell;
            bindingCheckSell.ResetBindings(false);
            if (ShowDialog() != DialogResult.OK)
                throw new Exception("Отмена операции");
            var payments = new List<CheckPaymentDataModel>();
            foreach (var pay in CheckPayments)
                payments.Add(pay);
            return payments;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(_checkSell.SumAll==CheckPayments.Sum(c=>c.Sum))
                    DialogResult = DialogResult.OK;
            }
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
            if (e.KeyCode == Keys.F6)
                AddPaymetnElectron();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            

        }

        private void AddPaymetnElectron()
        {
            decimal payRemaned = _checkSell.SumAll - CheckPayments.Sum(p => p.Sum);
            if (payRemaned > 0)
                CheckPayments.Add(new CheckPaymentDataModel
                {
                    TypePayment=TypePayment.Electron,
                    Sum=payRemaned
                });
        }

        private void AddPaymentCash()
        {
            decimal payRemaned = _checkSell.SumAll - CheckPayments.Sum(p => p.Sum);
            if (payRemaned > 0)
                CheckPayments.Add(new CheckPaymentDataModel
                {
                    TypePayment = TypePayment.Cash,
                    Sum = payRemaned
                });
        }

        private void dataGridViewPayments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridViewPayments.Focus();
            var row = dataGridViewPayments.Rows[e.RowIndex];
            var payment = CheckPayments[e.RowIndex];
            if (payment.TypePayment == TypePayment.Cash)
            {
                row.Cells[nameof(ColumnIncome)].ReadOnly = false;
                row.Cells[nameof(ColumnReturn)].ReadOnly = false;
                row.Cells[nameof(ColumnIncome)].Selected = true;
            }
            if (payment.TypePayment == TypePayment.Electron)
            {
                row.Cells[nameof(ColumnIncome)].ReadOnly = true;
                row.Cells[nameof(ColumnReturn)].ReadOnly = true;
                row.Cells[nameof(ColumnSum)].Selected = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddPaymetnElectron();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPaymentCash();
        }

        private void dataGridViewPayments_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_checkSell.SumAll == CheckPayments.Sum(c => c.Sum))
                DialogResult = DialogResult.OK;
        }
    }
}
