using OnlineCashRmk.Models;
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

namespace OnlineCashRmk
{
    public partial class FormScreenForBuyer : Form
    {
        public FormScreenForBuyer(ObservableCollection<CheckGoodModel> checkGoods)
        {
            InitializeComponent();

            BindingSource bindingCheckGoods = new BindingSource();
            checkGoods.CollectionChanged += (s, e) =>
            {
                bindingCheckGoods.ResetBindings(false);
                labelSumAll.Text = Math.Ceiling(checkGoods.Sum(c => c.Sum)).ToString();
            };
            bindingCheckGoods.DataSource = checkGoods;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = bindingCheckGoods;
            ColumnId.DataPropertyName = nameof(CheckGoodModel.GoodId);
            ColumnName.DataPropertyName = nameof(CheckGoodModel.GoodName);
            ColumnUnit.DataPropertyName = nameof(CheckGoodModel.GoodUnit);
            ColumnCount.DataPropertyName = nameof(CheckGoodModel.Count);
            ColumnDiscount.DataPropertyName = nameof(CheckGoodModel.Discount);
            ColumnPrice.DataPropertyName = nameof(CheckGoodModel.Cost);
            ColumnSum.DataPropertyName = nameof(CheckGoodModel.Sum);
        }
    }
}
