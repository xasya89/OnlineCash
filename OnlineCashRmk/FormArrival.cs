using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineCashRmk.Services;
using OnlineCashRmk.Models;
using OnlineCashRmk.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO.Ports;

namespace OnlineCashRmk
{
    public partial class FormArrival : Form
    {
        ObservableCollection<Supplier> Suppliers { get; set; } = new ObservableCollection<Supplier>();
        ObservableCollection<ArrivalPositionDataModel> ArrivalPositions { get; set; } = new ObservableCollection<ArrivalPositionDataModel>();
        ObservableCollection<Good> findGoods = new ObservableCollection<Good>();
        ISynchService synch;
        DataContext db;
        IServiceProvider _provider;

        SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) => {
            var activeform = Form.ActiveForm;
            if (activeform != null && nameof(FormArrival) == activeform.Name)
            {
                var port = (SerialPort)s;
                string code = port.ReadExisting();
                var form = activeform as FormArrival;
                var barcode = await form.db.BarCodes.Include(b => b.Good).Where(b => b.Code == code).FirstOrDefaultAsync();
                Action<Good, decimal> addGood = form.AddGood;
                if (barcode != null && barcode.Good.IsDeleted == false)
                    Task.Run(() => form.Invoke(addGood, barcode.Good, 1M));
            }
        });
        BarCodeScanner _barCodeScanner;

        public FormArrival(ISynchService synchService, ILogger<FormArrival> logger,
            IDbContextFactory<DataContext> dbFactory, ISynchService synch, BarCodeScanner barCodeScanner, 
            IServiceProvider provider)
        {
            this.synch = synch;
            this.db = dbFactory.CreateDbContext();
            _provider = provider;
            InitializeComponent();
            CalcSumAll();

            _barCodeScanner = barCodeScanner;
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived += serialDataReceivedEventHandler;

            Task<List<Supplier>> taskSynchSuppliers = Task.Run(async () => await synchService.SynchSuppliersAsync());
            taskSynchSuppliers.ContinueWith(task =>
            {
                foreach (var supplier in task.Result)
                    Suppliers.Add(supplier);
                var action = new Action(() =>
                  {
                      SupplierComboBox.DataSource = Suppliers;
                      SupplierComboBox.DisplayMember = nameof(Supplier.Name);
                      SupplierComboBox.ValueMember = nameof(Supplier.Id);
                  });
                Invoke(action);
            });
            BindingSource positionBinding = new BindingSource();
            ArrivalPositions.CollectionChanged += (s, e) =>
            {
                positionBinding.ResetBindings(false);
                CalcSumAll();
            };
            positionBinding.DataSource = ArrivalPositions;
            dataGridViewPositions.AutoGenerateColumns = false;
            dataGridViewPositions.DataSource = positionBinding;
            Column_GoodName.DataPropertyName = nameof(ArrivalPositionDataModel.GoodName);
            Column_Unit.DataPropertyName = nameof(ArrivalPositionDataModel.UnitStr);
            Column_PriceArrival.DataPropertyName = nameof(ArrivalPositionDataModel.PriceArrival);
            Column_PriceSell.DataPropertyName = nameof(ArrivalPositionDataModel.PriceSell);
            Column_PricePercent.DataPropertyName = nameof(ArrivalPositionDataModel.PricePercent);
            Column_Count.DataPropertyName = nameof(ArrivalPositionDataModel.Count);
            Column_SumArrival.DataPropertyName = nameof(ArrivalPositionDataModel.SumArrival);
            Column_NdsPercent.DataPropertyName = nameof(ArrivalPositionDataModel.NdsPercent);
            Column_SumNds.DataPropertyName = nameof(ArrivalPositionDataModel.SumNdsStr);
            Column_SumSell.DataPropertyName = nameof(ArrivalPositionDataModel.SumSell);
            Column_ExpiresDate.DataPropertyName = nameof(ArrivalPositionDataModel.ExpiresDate);
            BindingSource binding = new BindingSource();
            findGoods.CollectionChanged += (sender, e) =>
            {
                binding.ResetBindings(false);
            };
            binding.DataSource = findGoods;
            findListBox.DataSource = binding;
            findListBox.DisplayMember = nameof(Good.Name);
        }

        private void findTextBox_TextChanged(object sender, EventArgs e)
        {
            if (findTextBox.Text != "")
                if (findTextBox.Text.Length >= 2)
                {
                    findGoods.Clear();
                    var goods = db.Goods.OrderBy(g => g.Name).ToList();
                    foreach (var good in goods.Where(g => g.Name != null && g.Name.ToLower().IndexOf(findTextBox.Text.ToLower()) > -1).Take(20).ToList())
                        if (good.IsDeleted == false)
                            findGoods.Add(good);
                    foreach (var barcode in db.BarCodes.Include(g => g.Good).Where(b => b.Code == findTextBox.Text).ToList())
                        if (barcode.Good?.IsDeleted == false)
                            findGoods.Add(barcode.Good);
                }
        }

        void AddGood(Good good, decimal count = 1)
        {
            if (good != null /*&& ArrivalPositions.FirstOrDefault(p => p.GoodId == good.Id) == null*/)
            {
                var newArrival = new ArrivalPositionDataModel { GoodId = good.Id, GoodName = good.Name, Unit = good.Unit, Count = count, PriceSell = good.Price };
                ArrivalPositions.Add(newArrival);
                newArrival.PropertyChanged += (s, e) => CalcSumAll();
                dataGridViewPositions.FirstDisplayedScrollingRowIndex = ArrivalPositions.Count - 1;
            }
                
        }

        private void findTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (findTextBox.Text != "")
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        var good = (Good)findListBox.SelectedItem;
                        if (good != null)
                            AddGood(good);
                        /*
                        findTextBox.Text = "";
                        findGoods.Clear();
                        */
                        break;
                    case Keys.Down:
                        int cursor = findListBox.SelectedIndex;
                        int itemcount = findListBox.Items.Count;
                        if (cursor + 1 < itemcount)
                            findListBox.SelectedIndex = cursor + 1;
                        else
                            findListBox.SelectedIndex = 0;
                        break;
                    case Keys.Up:
                        int cursor1 = findListBox.SelectedIndex;
                        int itemcount1 = findListBox.Items.Count;
                        if (cursor1 == 0)
                            findListBox.SelectedIndex = itemcount1 - 1;
                        else
                            findListBox.SelectedIndex = cursor1 - 1;
                        break;
                }
        }

        private void findListBox_DoubleClick(object sender, EventArgs e)
        {
            if (findListBox.SelectedItems.Count > 0)
            {
                var good = (Good)findListBox.SelectedItem;
                AddGood(good);
                /*
                findGoods.Clear();
                findTextBox.Text = "";
                */
            }
        }

        string barcodeScan = "";
        private void FormArrival_KeyDown(object sender, KeyEventArgs e)
        {
            if (!findTextBox.Focused)
            {
                switch (e.KeyCode)
                {
                    case Keys.NumPad0:
                    case Keys.D0:
                        barcodeScan = barcodeScan + "0";
                        break;
                    case Keys.NumPad1:
                    case Keys.D1:
                        barcodeScan = barcodeScan + "1";
                        break;
                    case Keys.NumPad2:
                    case Keys.D2:
                        barcodeScan = barcodeScan + "2";
                        break;
                    case Keys.NumPad3:
                    case Keys.D3:
                        barcodeScan = barcodeScan + "3";
                        break;
                    case Keys.NumPad4:
                    case Keys.D4:
                        barcodeScan = barcodeScan + "4";
                        break;
                    case Keys.NumPad5:
                    case Keys.D5:
                        barcodeScan = barcodeScan + "5";
                        break;
                    case Keys.NumPad6:
                    case Keys.D6:
                        barcodeScan = barcodeScan + "6";
                        break;
                    case Keys.NumPad7:
                    case Keys.D7:
                        barcodeScan = barcodeScan + "7";
                        break;
                    case Keys.NumPad8:
                    case Keys.D8:
                        barcodeScan = barcodeScan + "8";
                        break;
                    case Keys.NumPad9:
                    case Keys.D9:
                        barcodeScan = barcodeScan + "9";
                        break;
                }
                if (e.KeyCode == Keys.Enter)
                {
                    AddGood(db.BarCodes.Include(b => b.Good).Where(b => b.Code == barcodeScan & b.Good.IsDeleted == false).FirstOrDefault()?.Good);
                    barcodeScan = "";
                }
                if (e.KeyCode == Keys.Delete)
                    if((dataGridViewPositions.Focused & dataGridViewPositions.SelectedCells.Count>0) && dataGridViewPositions.Columns[dataGridViewPositions.SelectedCells[0].ColumnIndex].Name==Column_ExpiresDate.Name)
                    {
                        var position = ArrivalPositions[dataGridViewPositions.SelectedCells[0].RowIndex];
                        position.ExpiresDate = null;
                        dtp.Visible = false;
                    }
                else
                    button1_Click(null, null);
            };
            if (e.KeyCode == Keys.F4 & !e.Alt)
                if (!findTextBox.Focused)
                {
                    findTextBox.Focus();
                    findTextBox.BackColor = Color.LightBlue;
                }
                else
                {
                    dataGridViewPositions.Select();
                    findTextBox.BackColor = SystemColors.Window;
                }
        }

        private void SupplierComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewPositions.Select();
            findTextBox.BackColor = SystemColors.Window;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewPositions.SelectedCells.Count > 0)
            {
                int pos = dataGridViewPositions.SelectedCells[0].RowIndex;
                ArrivalPositions.RemoveAt(pos);
            }
            dataGridViewPositions.Select();
            findTextBox.BackColor = SystemColors.Window;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            var supplier = SupplierComboBox.SelectedItem as Supplier;
            if (arrivalNum.Text != "" && supplier!=null)
            {
                var arrival = new Arrival { Num = arrivalNum.Text, DateArrival = arrivalDate.Value, SupplierId = supplier.Id };
                db.Arrivals.Add(arrival);
                List<NewGoodFromCash> newGoodList = new List<NewGoodFromCash>();
                Revaluation revaluation = new Revaluation();
                List<RevaluationGood> revaluationGoods = new List<RevaluationGood>();
                foreach(var position in ArrivalPositions)
                {
                    var arrivalGood = new ArrivalGood { Arrival = arrival, GoodId = position.GoodId, Count = position.Count, Price = position.PriceArrival, Nds=position.NdsPercent, ExpiresDate=position.ExpiresDate };
                    var good = db.Goods.Where(g => g.Id == position.GoodId).FirstOrDefault();
                    if(good.Price!=position.PriceSell)
                    {
                        RevaluationGood revaluationGood = revaluationGoods.Where(r => r.Good.Id == good.Id).FirstOrDefault();
                        if (revaluationGood == null)
                            revaluationGoods.Add(new RevaluationGood { Revaluation = revaluation, Good = good, PriceOld = good.Price, PriceNew = position.PriceSell });
                        else
                            if (revaluationGood.PriceNew < position.PriceSell)
                            revaluationGood.PriceNew = position.PriceSell;
                        good.Price = position.PriceSell;
                        var newGood = new NewGoodFromCash { Good = good };
                        db.NewGoodsFromCash.Add(newGood);
                        newGoodList.Add(newGood);
                    }
                    db.ArrivalGoods.Add(arrivalGood);
                };
                if (revaluationGoods.Count > 0)
                {
                    db.Revaluations.Add(revaluation);
                    db.RevaluationGoods.AddRange(revaluationGoods);
                }
                await db.SaveChangesAsync();
                if (revaluationGoods.Count > 0)
                    synch.AppendDoc(new DocSynch { DocId=revaluation.Id, TypeDoc=TypeDocs.Revaluation});
                foreach (var newGood in newGoodList)
                    synch.AppendDoc(new DocSynch { DocId = newGood.Id, Create = DateTime.Now, TypeDoc = TypeDocs.NewGoodFromCash });
                synch.AppendDoc(new DocSynch { DocId = arrival.Id, Create = DateTime.Now, TypeDoc = TypeDocs.Arrival });
                Close();
            }
        }

        private void FormArrival_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived -= serialDataReceivedEventHandler;
            db.Dispose();
        }

        void CalcSumAll()
        {
            labelSumNds.Text = ArrivalPositions.Sum(a => a.SumNds).ToSellFormat();
            labelSumArrival.Text = ArrivalPositions.Sum(a => a.SumArrival).ToSellFormat();
            labelSumSell.Text = ArrivalPositions.Sum(a => a.SumSell).ToSellFormat();
        }

        private DateTimePicker dtp { get; set; }
        private void dataGridViewPositions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewPositions.Columns[e.ColumnIndex].Name == Column_ExpiresDate.Name)
            {
                // initialize DateTimePicker
                dtp = new DateTimePicker();
                dtp.Format = DateTimePickerFormat.Short;
                dtp.Visible = true;
                var position = ArrivalPositions[e.RowIndex];
                dtp.Value = position.ExpiresDate == null ? DateTime.Now : (DateTime)position.ExpiresDate;
                //dtp.Value = DateTime.Parse(dataGridViewPositions.CurrentCell?.Value?.ToString());

                // set size and location
                var rect = dataGridViewPositions.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dtp.Size = new Size(rect.Width, rect.Height);
                dtp.Location = new Point(rect.X, rect.Y);

                // attach events
                dtp.CloseUp += new EventHandler(dtp_CloseUp);
                dtp.TextChanged += new EventHandler(dtp_OnTextChange);

                dataGridViewPositions.Controls.Add(dtp);
            }
        }
        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            dataGridViewPositions.CurrentCell.Value = dtp.Text.ToString();
        }

        // on close of cell, hide dtp
        void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fr =(FormNewGood) _provider.GetService(typeof(FormNewGood));
            AddGood(fr.Show());
            fr.Dispose();
        }
    }
}
