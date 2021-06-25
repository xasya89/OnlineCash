using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace RMK.ViewModels
{
    public static class ShiftViewModel
    {
        public static async void PostOnServer(onlinecashContext db, string serverUrl, DateTime with, DateTime by)
        {
            with = with.Date;
            by = by.Date;
            var shifts = await db.Shifts
                .Include(s => s.CheckSells)
                //.ThenInclude(c => c.CheckGoods)
                //.ThenInclude(cg => cg.Good)
                .ToListAsync();
                //.Where(s => s.Start > with & s.Start < by.AddDays(1) & s.Stop!=null).ToListAsync();
            var shiftsModel = new List<Models.ShiftModel>();
            foreach(var shift in shifts)
            {
                var shiftModel = new Models.ShiftModel
                {
                    Start = shift.Start,
                    Stop = (DateTime)shift.Stop,
                    SumAll = shift.SumAll,
                    SumIncome = shift.SumIncome,
                    SummReturn = shift.SummReturn,
                    SumOutcome = shift.SumOutcome,
                    SumSell = shift.SumSell,
                    CheckSells = new List<Models.CheckSellModel>()
                };
                foreach(var check in shift.CheckSells)
                {
                    var checkModel = new Models.CheckSellModel
                    {
                        DateCreate = check.DateCreate,
                        IsElectron = check.IsElectron,
                        Sum = check.Sum,
                        SumDiscont = check.SumDiscont,
                        SumAll = check.SumAll,
                        Goods = new List<Models.CheckGoodModel>()
                    };
                    foreach (var checkGood in check.CheckGoods)
                        checkModel.Goods.Add(new Models.CheckGoodModel
                        {
                            Uuid = checkGood.Good.Uuid,
                            Cost = checkGood.Cost,
                            Count = checkGood.Count
                        });
                    shiftModel.CheckSells.Add(checkModel);
                }
                shiftsModel.Add(shiftModel);
            }
            try
            {
                string jsonRequest = JsonSerializer.Serialize(shiftsModel);
                string jsonResponse = await ClientServer.PostRequestAsync($"{serverUrl}/api/Shifts", jsonRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            /*
            var httpRequest = (HttpWebRequest)WebRequest.Create($"{serverUrl}/api/Shifts");
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "POST";
            using(var writer = new StreamWriter(httpRequest.GetRequestStream()))
            {
                var str = JsonSerializer.Serialize(shifts);
                writer.Write(str);
            }
            var response = httpRequest.GetResponse();
            */
        }
    }
}
