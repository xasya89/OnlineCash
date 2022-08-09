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
using OnlineCashRmk.Models;
using OnlineCashRmk.Services;

namespace OnlineCashRmk
{
    public partial class FormNewGood : Form
    {
        DataContext _db;
        ISynchService _synch;

        SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) => {
            var activeform = Form.ActiveForm;
            if (activeform != null && nameof(FormNewGood) == activeform.Name)
            {
                var port = (SerialPort)s;
                string code = port.ReadExisting();
                var form = activeform as FormNewGood;
                form.Invoke(new Action<string>((string code) => { form.goodBarCode.Text = code; }), code);
            }
        });
        BarCodeScanner _barCodeScanner;
        public FormNewGood(IDbContextFactory<DataContext> dbFactory, ISynchService synch, BarCodeScanner barCodeScanner)
        {
            _db = dbFactory.CreateDbContext();
            //FormClosed += (s, e) => { _db.Dispose(); };
            _synch = synch;
            _barCodeScanner = barCodeScanner;
            InitializeComponent();
        }

        public void Dispose() { }// => _db.Dispose();

        private void label2_Click(object sender, EventArgs e)
        {

        }

        bool flagNewGood = false;
        public Good Show(int goodId)
        {
            Good good = _db.Goods.Find(goodId);
            goodName.Text = good.Name;
            goodBarCode.Text = good.BarCodes.FirstOrDefault()?.Code;
            goodPrice.Text = good.Price.ToString();
            goodType.Text = good.SpecialType.DisplayName();
            goodUnit.Text = good.Unit.DisplayName();
            if (ShowDialog() != DialogResult.OK)
                return null;
            decimal price = 0;
            decimal.TryParse(goodPrice.Text, out price);
            good.Price = price;
            var newGood = new NewGoodFromCash { Good = good };
            _db.NewGoodsFromCash.Add(newGood);
            _db.SaveChanges();
            _synch.AppendDoc(new DocSynch { DocId = newGood.Id, Create = DateTime.Now, TypeDoc = TypeDocs.NewGoodFromCash });
            return good;
        }
        public Good Show(string barcode = "")
        {
            flagNewGood = true;
            goodBarCode.Text = barcode;
            if (ShowDialog() != DialogResult.OK)
                return null;
            Dictionary<string, Units> unitDict = new Dictionary<string, Units>()
            {
                {"шт",Units.PCE }, {"л",Units.Litr }, {"кг",Units.KG }, {"м",Units.MTR }, {"уп",Units.NMP }, {"см",Units.CMT }
            };
            Dictionary<string, SpecialTypes> specialTypes = new Dictionary<string, SpecialTypes>()
            {
                {"",SpecialTypes.None }, {"Пиво",SpecialTypes.Beer}, {"Пакет",SpecialTypes.Bag }
            };
            var good = new Good
            {
                Uuid=Guid.NewGuid(),
                Name = goodName.Text,
                Unit = unitDict[goodUnit.Text],
                BarCode = goodBarCode.Text,
                SpecialType = specialTypes[goodType.Text],
                Price=decimal.Parse(goodPrice.Text),
                IsDeleted=false
            };
            _db.Goods.Add(good);
            var barcodeGood = new BarCode { Good = good, Code = goodBarCode.Text };
            _db.BarCodes.Add(barcodeGood);
            var newGood = new NewGoodFromCash { Good = good };
            _db.NewGoodsFromCash.Add(newGood);
            _db.SaveChanges();
            _synch.AppendDoc(new DocSynch { DocId = newGood.Id, Create = DateTime.Now, TypeDoc = TypeDocs.NewGoodFromCash });
            return good;
        }

        //Создать
        private void button1_Click(object sender, EventArgs e)
        {
            if (goodName.Text == "" || goodUnit.Text=="" || decimal.TryParse(goodPrice.Text, out var price)==false)
                return;
            if(flagNewGood)
            {
                var barcode = _db.BarCodes.Include(b => b.Good).Where(b => b.Code == goodBarCode.Text).FirstOrDefault();
                if (barcode != null)
                {
                    MessageBox.Show($"Данный штрих код назначен {barcode.Good.Name}");
                    return;
                };
            }
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var barcodes = _db.BarCodes.Select(b=>b.Code).ToList();
            string barcode; 
            do
            {
                barcode = GenerateBarCode();
            }
            while (barcodes.IndexOf(barcode)>-1);
            goodBarCode.Text = barcode;
        }

        private string GenerateBarCode()
        {
            string code="";
            for (int i = 1; i <= 6; i++)
                code += new Random().Next(9).ToString();
            return code;
        }
    }
}
