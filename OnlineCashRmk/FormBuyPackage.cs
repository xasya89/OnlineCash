using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    public partial class FormBuyPackage : Form
    {
        public FormBuyPackage(Good goodPackage)
        {
            /*
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
            IConfiguration configuration = builder.Build();
            Guid uuidPackage = Guid.Parse(configuration.GetSection("BuyGoodPackage").Value);
            DataContext db = new DataContext();
            var goodPackage = db.Goods.Where(g => g.Uuid == uuidPackage).FirstOrDefault();
            GoodPackaeNameLabel.Text = goodPackage.Name;
            */
            InitializeComponent();
            GoodPackaeNameLabel.Text = $"Добавить: {goodPackage?.Name}";
        }

        private void FormBuyPackage_KeyDown(object sender, KeyEventArgs e)
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
    }
}
