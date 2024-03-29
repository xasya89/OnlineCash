﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class CashMoney
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        [Required]
        public Guid Uuid { get; set; } = Guid.NewGuid();
        [Required]
        public DateTime Create { get; set; }
        [Required]
        public CashMoneyTypeOperations TypeOperation { get; set; }
        [Required]
        public decimal Sum { get; set; }
        
        public string Note { get; set; }
    }

    public enum CashMoneyTypeOperations
    {
        [Description("Внесение")]
        Income = 0,
        [Description("Изъятие")]
        Outcome = 1,
        //TODO: Возможно стоит переменовать в ShiftReport, т.к. это больше для докумена закрытия смены
        [Description("Продажи")]
        Sale=2,
        [Description("Возврат")]
        Return=3
    }
}
