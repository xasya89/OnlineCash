using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models.BuyerRegister;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class BuyerRegistration
    {
        static ObservableCollection<VerificationCardAndPhone> registrtions = new ObservableCollection<VerificationCardAndPhone>();

        shopContext _db;
        ISmsService _sms;
        public BuyerRegistration(shopContext db, ISmsService sms)
        {
            _db = db;
            _sms = sms;
        }

        public bool Verification(VerificationCardAndPhone model)
        {
            var discount = VerificationCard(model.CardNum);
            if (discount==null || discount.isFree==false)
                return false;
            if (!VerificationPhone(model.Phone))
                return false;
            return true;
        }

        public DiscountCard VerificationCard(string cardNum)
            => _db.DiscountCards.Where(c => c.Num == cardNum).FirstOrDefault();

        public bool VerificationPhone(string phone)
        {
            phone = phone.Trim().Replace(" ","").Replace("+", "").Replace("(","").Replace(")", "");
            if (int.TryParse(phone, out int i))
                return false;
            if (phone.Length != "79192847021".Length)
                return false;
            if (phone.Substring(0, 1) != "7")
                return false;
            return true;
        }

        public VerificationCardAndPhone SandSMS(VerificationCardAndPhone model)
        {
            model.Uuid = Guid.NewGuid();
            model.Code = GenerationCode();
            //_sms.Send(model.Phone, model.Code);
            registrtions.Add(model);
            return model;
        }

        private string GenerationCode() =>
            $"{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}";

        public async Task<bool> AddBuyer(BuyerRegistrationModel model)
        {
            if (registrtions.Where(r => r.Uuid == model.Uuid & r.Code == model.Code) == null)
                return false;
            var discount= await _db.DiscountCards.Where(c => c.Num == model.CardNum).FirstOrDefaultAsync();
            discount.isFree = false;
            _db.Buyers.Add(new Buyer
            {
                Name = model.Name,
                Phone = model.Phone,
                Birthday = model.Birthday,
                DiscountCard = discount
            });
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
