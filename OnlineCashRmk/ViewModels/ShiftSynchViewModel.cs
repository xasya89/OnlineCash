using Microsoft.Extensions.Configuration;
using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineCashRmk.ViewModels
{
    static class ShiftSynchViewModel
    {
        static string serverName = "";
        static int idShop = 1;
        static  ShiftSynchViewModel()
        {
            IConfiguration configuration;
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
            configuration = builder.Build();
            serverName = configuration.GetSection("serverName").Value;
            idShop = Convert.ToInt32(configuration.GetSection("idShop").Value);
        }

        public static async Task<Boolean> SynchAsync()
        {
            bool flagResult = true;
            if (await PostNewGood() == false)
                return false;

            var optBuilder = new DbContextOptionsBuilder();
            optBuilder.UseSqlite("Data Source=CustomerDB.db;");
            DataContext db = new DataContext(optBuilder.Options);
            List<Shift> shifts = await db.Shifts.Include(s => s.CheckSells).ThenInclude(c => c.CheckGoods).ThenInclude(cg => cg.Good).Where(s => s.Stop != null & !s.isSynch).ToListAsync();
            foreach(var shift in shifts)
                if(!shift.isSynch)
            {
                string str = JsonSerializer.Serialize(shift);
                var context = new StringContent(str, Encoding.UTF8, "application/json");
                var result = await new HttpClient().PostAsync($"{serverName}/api/Shifts/one", context);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    flagResult = false;
                else
                    shift.isSynch = true;
            }
            await db.SaveChangesAsync();
            /*
            foreach (var shift in shifts)
                shift.isSynch = true;
            await db.SaveChangesAsync();
            */
            return flagResult;
        }

        public static async Task<bool> PostNewGood()
        {
            try
            {
                var optBuilder = new DbContextOptionsBuilder();
                optBuilder.UseSqlite("Data Source=CustomerDB.db;");
                DataContext db = new DataContext(optBuilder.Options);
                var goods = await db.Goods.Include(b=>b.BarCodes).Where(g => g.Uuid == Guid.Parse("00000000-0000-0000-0000-000000000000") ).ToListAsync();
                
                if(goods.Count>0)
                {
                    try
                    {
                        foreach(var good in goods)
                        {       
                            string str = JsonSerializer.Serialize(good);
                            var context = new StringContent(str, Encoding.UTF8, "application/json");
                            var resp = await new HttpClient().PostAsync($"{serverName}/api/GoodsSynchNew/", context);
                            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                                throw new SystemException("Статус ответа от сервера error");
                            string respStr = await resp.Content.ReadAsStringAsync();
                            Guid uuid = JsonSerializer.Deserialize<Good>(respStr).Uuid;
                            good.Uuid = uuid;
                        };
                        await db.SaveChangesAsync();
                    }
                    catch(SystemException ex)
                    {
                        return false;
                    }
                }
            }
            catch(SystemException ex)
            {
                return false;
            }
            return true;
        }
    }
}
