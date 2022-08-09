using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OnlineCashRmk.Models;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Services;
using System.IO.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineCashRmk
{
    public partial class FormStocktaking : Form
    {
        DataContext _db;
        ISynchService _synchService;
        IServiceProvider _provider;
        Stocktaking stocktaking;
        ObservableCollection<StocktakingGroup> groups=new ObservableCollection<StocktakingGroup>();
        StocktakingGroup selectedGroup;

        ObservableCollection<Good> findGoods = new ObservableCollection<Good>();
        BindingSource bindingGroups;
        BindingSource bindingGoods;

        #region Сканер штрих кода
        SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) => {
            var activeform = Form.ActiveForm;
            if (activeform != null && nameof(FormStocktaking) == activeform.Name)
            {
                var port = (SerialPort)s;
                string code = port.ReadExisting();
                var form = activeform as FormStocktaking;
                var barcode = await form._db.BarCodes.Include(b => b.Good).Where(b => b.Code == code).FirstOrDefaultAsync();
                Action<Good> addGood = form.AddGood;
                if (barcode != null && barcode.Good.IsDeleted == false)
                    Task.Run(() => form.Invoke(addGood, barcode.Good));
            }
        });
        BarCodeScanner _barCodeScanner;
        #endregion

        public FormStocktaking(IDbContextFactory<DataContext> dbFactory, ISynchService synchService, BarCodeScanner barCodeScanner, IServiceProvider provider)
        {
            _db = dbFactory.CreateDbContext();
            _synchService = synchService;
            stocktaking = _db.Stocktakings.Include(s => s.StocktakingGroups).ThenInclude(gr => gr.StocktakingGoods).ThenInclude(g => g.Good).Where(s => s.isSynch == false).FirstOrDefault();
            if (stocktaking != null)
                if (MessageBox.Show($"Продолжить редактировать предыдущую инверторизацию от {stocktaking.Create.ToString("dd.MM.yy")}?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    foreach (var group in stocktaking.StocktakingGroups)
                    {
                        var stGroup = new StocktakingGroup { Id = group.Id, Name = group.Name };
                        groups.Add(stGroup);
                        foreach (var good in group.StocktakingGoods)
                        {
                            var stGood = new StocktakingGood
                            {
                                Id = good.Id,
                                StocktakingGroupId = good.StocktakingGroupId,
                                GoodId = good.GoodId,
                                Good = good.Good,
                                CountFact = good.CountFact
                            };
                            stGroup.StocktakingGoods.Add(stGood);
                            stGood.PropertyChanged += (s, e) =>
                            {
                                if (e.PropertyName == nameof(StocktakingGood.CountFact))
                                {
                                    bindingGroups.ResetBindings(false);
                                    label3.Text = groups.Sum(gr => gr.Sum).ToString();
                                }
                            };
                        }
                    }
                }
                else
                {
                    stocktaking.isSynch = true;
                    _db.SaveChanges();
                    stocktaking = null;
                }
            if (stocktaking == null)
            {
                FormText fr = new FormText();
                fr.label1.Text = "Денег в кассе";
                if (fr.ShowDialog() == DialogResult.OK)
                {
                    stocktaking = new Stocktaking { Create = DateTime.Now, CashMoney = fr.textBox1.Text.ToDecimal(), isSynch = false };
                    _db.Stocktakings.Add(stocktaking);

                    _db.SaveChanges();
                    _db.DocSynches.Add(new DocSynch { DocId = stocktaking.Id, TypeDoc = TypeDocs.StartStocktacking });
                    _db.SaveChanges();
                }
                else
                {
                    return;
                }
            }
            InitializeComponent();

            _barCodeScanner = barCodeScanner;
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived += serialDataReceivedEventHandler;

            bindingGroups = new BindingSource();
            bindingGroups.DataSource = groups;
            groups.CollectionChanged += (s, e) => { bindingGroups.ResetBindings(false); label3.Text = groups.Sum(gr => gr.Sum).ToString(); };
            listBoxGroups.DataSource = bindingGroups;
            listBoxGroups.DisplayMember = nameof(StocktakingGroup);

            bindingGoods = new BindingSource();
            bindingGoods.DataSource = selectedGroup?.StocktakingGoods;
            dataGridViewGoods.AutoGenerateColumns = false;
            dataGridViewGoods.DataSource = bindingGoods;
            ColumnGoodName.DataPropertyName = nameof(StocktakingGood.GoodName);
            ColumnUnit.DataPropertyName = nameof(StocktakingGood.UnitStr);
            ColumnCountFact.DataPropertyName = nameof(StocktakingGood.CountFactStr);
            ColumnPrice.DataPropertyName = nameof(StocktakingGood.Price);
            ColumnSum.DataPropertyName = nameof(StocktakingGood.Sum);
            ColumnCountMoveDoc.DataPropertyName = nameof(StocktakingGood.CountDocMove);

            listBoxGroups.SelectedIndexChanged += (s, e) =>
            {
                if (listBoxGroups.SelectedItem != null)
                {
                    selectedGroup = listBoxGroups.SelectedItem as StocktakingGroup;
                    bindingGoods.DataSource = selectedGroup.StocktakingGoods;
                    bindingGoods.ResetBindings(false);
                }
            };

            BindingSource bindingFind = new BindingSource();
            bindingFind.DataSource = findGoods;
            listBoxFind.DataSource = bindingFind;
            listBoxFind.DisplayMember = nameof(Good.Name);
            findGoods.CollectionChanged += (s, e) => { bindingFind.ResetBindings(false); };

            label3.Text = groups.Sum(gr => gr.Sum).ToString();
        }

        void LoadStocktaking()
        {

        }

        /// <summary>
        /// Добавить группу
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxGroupAdd.Text != "")
            {
                var newgroup = new StocktakingGroup { StocktakingId=stocktaking.Id, Name = textBoxGroupAdd.Text };
                _db.StocktakingGroups.Add(newgroup);
                _db.SaveChanges();
                groups.Add(new StocktakingGroup { StocktakingId=stocktaking.Id, Id=newgroup.Id, Name=newgroup.Name});
                listBoxGroups.SelectedIndex = listBoxGroups.Items.Count - 1;
            };
        }

        public void AddGood(Good good)
        {
            if (good == null || selectedGroup == null)
                return;
            FormEditCount fr = new FormEditCount();
            fr.labelGoodName.Text = good.Name;
            fr.priceLabel.Text = $"{good.Price} р. {good.UnitDescription}";
            if (fr.ShowDialog() == DialogResult.OK)
            {
                var stGood = selectedGroup.StocktakingGoods.Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                var count = fr.textBoxCount.Text.ToDecimal();
                var stocktackingGood = new StocktakingGood { StocktakingGroupId = selectedGroup.Id, Good = good, CountFact = count };
                _db.StocktakingGoods.Add(stocktackingGood);
                _db.SaveChanges();
                selectedGroup.StocktakingGoods.Add(stocktackingGood);

                bindingGoods.ResetBindings(false);
                dataGridViewGoods.FirstDisplayedScrollingRowIndex = selectedGroup.StocktakingGoods.Count - 1;

                bindingGroups.ResetBindings(false);
                label3.Text = groups.Sum(gr => gr.Sum).ToString();
                return;
                /*
                if (stGood == null)
                {
                    var stocktackingGood = new StocktakingGood { StocktakingGroupId=selectedGroup.Id, Good = good, CountFact = count };
                    _db.StocktakingGoods.Add(stocktackingGood);
                    _db.SaveChanges();
                    selectedGroup.StocktakingGoods.Add(stocktackingGood);

                    bindingGoods.ResetBindings(false);
                    dataGridViewGoods.FirstDisplayedScrollingRowIndex = selectedGroup.StocktakingGoods.Count - 1;

                    bindingGroups.ResetBindings(false);
                    return;
                }
                else
                {
                    var dbGood = _db.StocktakingGoods.Where(s => s.Id == stGood.Id).FirstOrDefault();
                    dbGood.CountFact = fr.textBoxCount.Text.ToDecimal();
                    _db.SaveChanges();
                    stGood.CountFact = fr.textBoxCount.Text.ToDecimal();
                }
                */

                //stGood.CountFactStr = fr.textBoxCount.Text;
                bindingGoods.ResetBindings(false);
                int rowSelected = -1;
                foreach (var stgood in selectedGroup.StocktakingGoods)
                    ++rowSelected;
                dataGridViewGoods.FirstDisplayedScrollingRowIndex = rowSelected;
            }
        }

        private void FormStocktaking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                textBoxFind.Select();
            if(e.KeyCode==Keys.F2 && dataGridViewGoods.SelectedCells.Count>0)
            {
                var stGood = selectedGroup.StocktakingGoods[dataGridViewGoods.SelectedCells[0].RowIndex];
                FormEditCount fr = new FormEditCount();
                fr.labelGoodName.Text = stGood.GoodName;
                if(fr.ShowDialog()==DialogResult.OK)
                {
                    _db.StocktakingGoods.Where(s => s.Id == stGood.Id).FirstOrDefault().CountFact = fr.textBoxCount.Text.ToDecimal();
                    _db.SaveChanges();
                    stGood.CountFactStr = fr.textBoxCount.Text;
                    bindingGoods.ResetBindings(false);
                    bindingGroups.ResetBindings(false);
                    label3.Text = groups.Sum(gr => gr.Sum).ToString();
                }
            }
            if (e.KeyCode == Keys.Delete && dataGridViewGoods.SelectedCells.Count > 0)
            {
                var stGood = selectedGroup.StocktakingGoods[dataGridViewGoods.SelectedCells[0].RowIndex];
                if(MessageBox.Show($"Удалить {stGood.GoodName}?","",MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    _db.StocktakingGoods.Remove(_db.StocktakingGoods.Where(s => s.Id == stGood.Id).FirstOrDefault());
                    _db.SaveChanges();
                    selectedGroup.StocktakingGoods.Remove(stGood);
                    bindingGoods.ResetBindings(false);
                    bindingGroups.ResetBindings(false);
                    label3.Text = groups.Sum(gr => gr.Sum).ToString();
                }
            }
        }

        #region Поиск
        private void textBoxFind_Enter(object sender, EventArgs e)
        {
            textBoxFind.BackColor = Color.LightGreen;
        }

        private void textBoxFind_Leave(object sender, EventArgs e)
        {
            textBoxFind.BackColor = SystemColors.Control;
        }

        private void textBoxFind_TextChanged(object sender, EventArgs e)
        {
            if (textBoxFind.Text != "")
                if (textBoxFind.Text.Length >= 2)
                {
                    findGoods.Clear();
                    var goods = _db.Goods.OrderBy(g => g.Name).ToList();
                    foreach (var good in goods.Where(g => g.IsDeleted == false && g.Name.ToLower().IndexOf(textBoxFind.Text.ToLower()) > -1).Take(20).ToList())
                        findGoods.Add(good);
                    foreach (var barcode in _db.BarCodes.Include(g => g.Good).Where(b => b.Code == textBoxFind.Text).ToList())
                        if (barcode.Good != null && barcode.Good.IsDeleted == false)
                            findGoods.Add(barcode.Good);
                }
        }

        private void textBoxFind_KeyDown(object sender, KeyEventArgs e)
        {

            if (textBoxFind.Text != "")
                switch (e.KeyCode)
                {
                    case Keys.Back:
                        textBoxFind.Text = "";
                        findGoods.Clear();
                        break;
                    case Keys.Enter:
                        var good = (Good)listBoxFind.SelectedItem;
                        if (good != null)
                            AddGood(good);
                        break;
                    case Keys.Down:
                        int cursor = listBoxFind.SelectedIndex;
                        int itemcount = listBoxFind.Items.Count;
                        if (cursor + 1 < itemcount)
                            listBoxFind.SelectedIndex = cursor + 1;
                        else
                            listBoxFind.SelectedIndex = 0;
                        break;
                    case Keys.Up:
                        int cursor1 = listBoxFind.SelectedIndex;
                        int itemcount1 = listBoxFind.Items.Count;
                        if (cursor1 == 0)
                            listBoxFind.SelectedIndex = itemcount1 - 1;
                        else
                            listBoxFind.SelectedIndex = cursor1 - 1;
                        break;
                }
        }

        private void listBoxFind_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxFind.SelectedItems.Count > 0)
            {
                var good = (Good)listBoxFind.SelectedItem;
                AddGood(good);
            }
        }
        #endregion

        private void dataGridViewGoods_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private async void button3_Click(object sender, EventArgs e)
        {
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            stocktaking.isSynch = true;
            _db.SaveChanges();
            _synchService.AppendDoc(new DocSynch { DocId = stocktaking.Id, TypeDoc = TypeDocs.StopStocktacking });
            Close();
        }

        private void FormStocktaking_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived -= serialDataReceivedEventHandler;
            _db.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
