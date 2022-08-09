using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.ViewModels;
using OnlineCashRmk.DataModels;

namespace OnlineCashRmk
{
    public partial class FormMenu : Form
    {
        IConfiguration _configuration;
        DataContext _db;
        string serverName = "";
        int idShop = 1;
        string cashierName = "";
        string cashierInn = "";
        public FormMenu(IConfiguration configuration, IDbContextFactory<DataContext> dbFactory)
        {
            serverName = configuration.GetSection("serverName").Value;
            idShop = Convert.ToInt32(configuration.GetSection("idShop").Value);
            cashierName = configuration.GetSection("cashierName").Value;
            cashierInn = configuration.GetSection("cashierInn").Value;
            _configuration = configuration;
            _db = dbFactory.CreateDbContext();
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Загрузить товары
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.LightBlue;

            Task.Run(async () =>
            {
                bool statusSuccess = true;
                string messageRequest = "";
                try
                {
                    var str = await new HttpClient().GetAsync($"{serverName}/api/Goodssynchnew/{idShop}").Result.Content.ReadAsStringAsync();
                    List<GoodSynchDataModel> goods = JsonSerializer.Deserialize<List<GoodSynchDataModel>>(str);
                    foreach (var good in goods)
                    {
                        var goodDb = _db.Goods.Include(g=>g.BarCodes).Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                        if (goodDb == null)
                        {
                            var newgood = new Good
                            {
                                Uuid = good.Uuid,
                                Name = good.Name,
                                Article = good.Name,
                                Unit = good.Unit,
                                Price = good.Price,
                                SpecialType=good.SpecialType,
                                VPackage=good.VPackage,
                                IsDeleted=good.IsDeleted
                            };
                            _db.Goods.Add(newgood);
                            //добавление штрих кодов
                            foreach (string barcode in good.Barcodes)
                                _db.BarCodes.Add(new BarCode
                                {
                                    Good = newgood,
                                    Code = barcode
                                });
                        }
                        else
                        {
                            goodDb.Name = good.Name;
                            goodDb.Unit = good.Unit;
                            goodDb.Price = good.Price;
                            goodDb.SpecialType = good.SpecialType;
                            goodDb.VPackage = good.VPackage;
                            goodDb.IsDeleted = good.IsDeleted;
                            //добавление новых или измененных штрих кодов
                            foreach (string barcode in good.Barcodes)
                                if (goodDb.BarCodes.Count(b => b.Code == barcode) == 0)
                                    _db.BarCodes.Add(new BarCode { Good = goodDb, Code = barcode });
                            //Удаление не зарегестрированных на сервере штрихкодов
                            foreach (var barcodeDb in goodDb.BarCodes)
                                if (good.Barcodes.Count(b => b == barcodeDb.Code) == 0)
                                    _db.BarCodes.Remove(barcodeDb);
                        }
                    };
                    await _db.SaveChangesAsync();
                    //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(20));
                    messageRequest = "Синхронизация успешно выполнена";
                }
                catch (SystemException ex)
                {
                    statusSuccess = false;
                    messageRequest = ex.Message;
                }
                var action = new Action(() =>
                  {
                      button1.BackColor = statusSuccess ? Color.LightGreen : Color.LightPink;
                      errorTextBox.Text = messageRequest;
                  });
                Invoke(action);
            });
        }
        //Приходы
        private void button3_Click(object sender, EventArgs e)
        {
            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            string progId;
            BrowserApplication browser;
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoice))
            {
                if (userChoiceKey == null)
                {
                    browser = BrowserApplication.Unknown;
                }
                object progIdValue = userChoiceKey.GetValue("Progid");
                if (progIdValue == null)
                {
                    browser = BrowserApplication.Unknown;
                }
                progId = progIdValue.ToString();
                switch (progId)
                {
                    case "IE.HTTP":
                        browser = BrowserApplication.InternetExplorer;
                        break;
                    case "FirefoxURL":
                        browser = BrowserApplication.Firefox;
                        break;
                    case "ChromeHTML":
                        browser = BrowserApplication.Chrome;
                        break;
                    case "OperaStable":
                        browser = BrowserApplication.Opera;
                        break;
                    case "SafariHTML":
                        browser = BrowserApplication.Safari;
                        break;
                    case "AppXq0fevzme2pys62n3e0fbqa7peapykr8v":
                        browser = BrowserApplication.Edge;
                        break;
                    default:
                        browser = BrowserApplication.Unknown;
                        break;
                }
                if (progId.ToLower().IndexOf("firefox") > -1)
                    browser = BrowserApplication.Firefox;
            }
            //System.Diagnostics.Process.Start("explorer.exe", $"{serverName}/Arrivals/Create?ShopId={idShop}");
            System.Diagnostics.Process.Start(new ProcessStartInfo($"{serverName}/Arrivals/Create?ShopId={idShop}") { UseShellExecute = true } );
            Close();
        }

        //Инверторизация
        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo($"{serverName}/Stocktaking/Create?idShop={idShop}") { UseShellExecute = true });
            Close();
        }

        //Кредиты
        private void button5_Click(object sender, EventArgs e)
        {
            FormCreditList fr = new FormCreditList();
            //fr.TopMost = true;
            fr.Show();
            Close();
        }

        private void FormMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_db.Dispose();
        }
    }
}
