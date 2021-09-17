using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class MoveDoc
    {
        public int Id { get; set; }
        public bool IsSuccess { get; set; }
        public DocumentStatus Status { get; set; } = DocumentStatus.New;
        public DateTime DateMove { get; set; }
        public int ConsignerShopId { get; set; }
        public Shop ConsignerShop { get; set; }
        public int ConsigneeShopId { get; set; }
        public Shop ConsigneeShop { get; set; }
        public decimal SumAllConsigner { get; set; }
        public decimal SumAllConsignee { get; set; }
        public List<MoveGood> MoveGoods { get; set; } = new();
    }
}
