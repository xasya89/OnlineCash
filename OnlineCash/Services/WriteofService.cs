using Microsoft.Extensions.Logging;
using OnlineCash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using Microsoft.Extensions.Configuration;

namespace OnlineCash.Services
{
    public class WriteofService : IWriteofService
    {
        private readonly shopContext _db;
        private readonly IConfiguration _configuration;
        private readonly RabbitService _rabbitService;
        public WriteofService(shopContext db, 
            IConfiguration configuration,
            RabbitService rabbitService)
        {
            _db = db;
            _configuration = configuration;
            _rabbitService = rabbitService;
        }

        public async Task SaveSynch(int shopId, WriteofSynchModel model, Guid? synchUuid=null)
        {
            bool autoSuccess = _configuration.GetSection("AutoSuccessFromCash").Value == "1";
            Writeof writeofDb = new Writeof { 
                IsSuccess=autoSuccess,
                Uuid = model.Uuid, 
                ShopId=shopId, 
                Status=autoSuccess? DocumentStatus.Confirm : DocumentStatus.New, 
                DateWriteof = model.DateCreate, 
                Note = model.Note,
                SumAll=model.Goods.Sum(w=>(decimal)w.Count * w.Price)
            };
            _db.Writeofs.Add(writeofDb);
            List<WriteofGood> writeofGoods = new List<WriteofGood>();
            foreach(var wgood in model.Goods)
            {
                Good good = await _db.Goods.Where(g => g.Uuid == wgood.Uuid).FirstOrDefaultAsync();
                if (good == null)
                    throw new Exception($"Товар uuid {wgood.Uuid} не найден");
                writeofGoods.Add(new WriteofGood
                {
                    Writeof = writeofDb,
                    GoodId = good.Id,
                    Count = wgood.Count,
                    Price = wgood.Price
                });
            };
            _db.WriteofGoods.AddRange(writeofGoods);
            await _db.SaveChangesAsync();
            var docSynch = await _db.DocSynches.Where(d => d.Uuid == synchUuid).FirstOrDefaultAsync();
            if (docSynch != null)
            {
                docSynch.DocId = writeofDb.Id;
                docSynch.TypeDoc = TypeDocs.WriteOf;
                docSynch.isSuccess = true;
            }
            _db.DocumentHistories.Add(new DocumentHistory { TypeDoc = TypeDocs.Arrival, DocId = writeofDb.Id });
            _db.SaveChanges();

            if (autoSuccess)
            {
                List<GoodBalanceSynchModel> balances = new();
                foreach (var writeofGood in writeofGoods)
                    balances.Add(new GoodBalanceSynchModel
                    {
                        DocumentId = writeofDb.Id,
                        DocumentDate = writeofDb.DateWriteof,
                        TypeDoc = TypeDocs.WriteOf,
                        GoodId = writeofGood.GoodId,
                        Count = -1*writeofGood.Count
                    });
                _rabbitService.Send<List<GoodBalanceSynchModel>>(RabbitService.QueueNames.GoodBalance, balances);
            };
            _rabbitService.Send<TelegramNotifyModel>(RabbitService.QueueNames.Notify,
                    new TelegramNotifyModel
                    {
                        Message = $"Списание на сумму {writeofDb.SumAll} Примечание {writeofDb.Note}",
                        Url = "Writeof/edit/" + writeofDb.Id
                    });
        }

        public async Task Create(Writeof model)
        {
            var shop = await _db.Shops.Where(s => s.Id == model.ShopId).FirstOrDefaultAsync();
            decimal sumAll = model.WriteofGoods.Sum(w => w.Count * w.Price);
            var writeof = new Writeof
            {
                Status = model.IsSuccess ? DocumentStatus.Confirm : DocumentStatus.New,
                Shop = shop,
                DateWriteof = model.DateWriteof,
                IsSuccess = model.IsSuccess,
                Note = model.Note,
                SumAll = sumAll
            };
            _db.Writeofs.Add(writeof);
            List<WriteofGood> writeofGoods = new List<WriteofGood>();
            foreach (var wgood in model.WriteofGoods)
                writeofGoods.Add(new WriteofGood
                {
                    Writeof = writeof,
                    GoodId = wgood.GoodId,
                    Count = wgood.Count,
                    Price = wgood.Price
                });
            _db.WriteofGoods.AddRange(writeofGoods);
            await _db.SaveChangesAsync();

            if (writeof.Status == DocumentStatus.Confirm)
            {
                List<GoodBalanceSynchModel> balances = new();
                foreach (var writeofGood in writeofGoods)
                    balances.Add(new GoodBalanceSynchModel
                    {
                        DocumentId = writeof.Id,
                        DocumentDate = writeof.DateWriteof,
                        TypeDoc = TypeDocs.WriteOf,
                        GoodId = writeofGood.GoodId,
                        Count = -1 * writeofGood.Count
                    });
                _rabbitService.Send<List<GoodBalanceSynchModel>>(RabbitService.QueueNames.GoodBalance, balances);
            };
            _rabbitService.Send<TelegramNotifyModel>(RabbitService.QueueNames.Notify,
                    new TelegramNotifyModel
                    {
                        Message = $"Списание на сумму {writeof.SumAll} Примечание {writeof.Note}",
                        Url = "Writeof/edit/" + writeof.Id
                    });
        }

        public async Task Edit(Writeof model)
        {
            var writeof = await _db.Writeofs.Where(w => w.Id == model.Id).Include(w => w.WriteofGoods).FirstOrDefaultAsync();
            var docStatusOld = writeof.Status;
            writeof.Status = model.IsSuccess ? DocumentStatus.Confirm : writeof.Status;
            writeof.ShopId = model.ShopId;
            writeof.DateWriteof = model.DateWriteof;
            writeof.IsSuccess = model.IsSuccess;
            writeof.Note = model.Note;
            writeof.SumAll = model.WriteofGoods.Sum(wg => (decimal)wg.Count * wg.Price);
            await _db.SaveChangesAsync();
            //Удалим удаленные позиции
            List<GoodBalanceSynchModel> balancesPlus = new();
            foreach (var writegood in writeof.WriteofGoods)
            {
                balancesPlus.Add(new GoodBalanceSynchModel
                {
                    DocumentId = writeof.Id,
                    DocumentDate = writeof.DateWriteof,
                    TypeDoc = TypeDocs.WriteOf,
                    GoodId = writegood.GoodId,
                    Count = writegood.Count
                });
                if (model.WriteofGoods.Where(wg => wg.Id == writegood.Id).FirstOrDefault() == null)
                    _db.WriteofGoods.Remove(writegood);
            }
            //Добавим новые позиции
            foreach (var wgood in model.WriteofGoods.Where(wg => wg.Id == -1).ToList())
                _db.WriteofGoods.Add(new WriteofGood
                {
                    Writeof = writeof,
                    GoodId = wgood.GoodId,
                    Count = wgood.Count,
                    Price = wgood.Price
                });
            //Изменим существующие позиции
            foreach (var wgood in model.WriteofGoods.Where(wg => wg.Id != -1).ToList())
            {
                var writegood = await _db.WriteofGoods.Where(wg => wg.Id == wgood.Id).FirstOrDefaultAsync();
                writegood.Count = wgood.Count;
                writegood.Price = wgood.Price;
            };
            await _db.SaveChangesAsync();

            if (docStatusOld != DocumentStatus.Confirm & writeof.Status == DocumentStatus.Confirm)
            {
                _rabbitService.Send<List<GoodBalanceSynchModel>>(RabbitService.QueueNames.GoodBalance, balancesPlus);
                List<GoodBalanceSynchModel> balancesMinus = new();
                foreach(var wgood in model.WriteofGoods)
                    balancesMinus.Add(new GoodBalanceSynchModel
                    {
                        DocumentId = writeof.Id,
                        DocumentDate = writeof.DateWriteof,
                        TypeDoc = TypeDocs.WriteOf,
                        GoodId = wgood.GoodId,
                        Count = -1 * wgood.Count
                    });
                _rabbitService.Send<TelegramNotifyModel>(RabbitService.QueueNames.Notify,
                        new TelegramNotifyModel
                        {
                            Message = $"Изменено списание на сумму {writeof.SumAll} Примечание {writeof.Note}",
                            Url = "Writeof/edit/" + writeof.Id
                        });
            }
        }
    }
}
