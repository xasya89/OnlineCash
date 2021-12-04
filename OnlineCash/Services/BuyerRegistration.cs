using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models.BuyerRegister;
using Microsoft.EntityFrameworkCore;

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
            if (!VerificationCard(model.CardNum))
                return false;
            if (!VerificationPhone(model.Phone))
                return false;
            return true;
        }

        private bool VerificationCard(string cardNum)
            => _db.DiscountCards.Where(c =>c.Num==cardNum & c.isFree == true).FirstOrDefault() != null;

        private bool VerificationPhone(string phone) => true;

        public VerificationCardAndPhone SendSMSVerification(VerificationCardAndPhone model)
        {
            model.Uuid = Guid.NewGuid();
            model.Code = GenerationCode();
            _sms.Send(model.Phone, model.Code);
            registrtions.Add(model);
            return model;
        }

        private string GenerationCode() =>
            $"{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}";

        public async Task<bool> AddBuyer(BuyerRegistrationModel model)
        {
            if (registrtions.Where(r => r.Uuid == model.Uuid & r.Code == model.Code) == null)
                return false;
            var discount= await _db.DiscountCards.Where(c => c.Num == model.Code).FirstOrDefaultAsync();
            discount.isFree = false;
            
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
