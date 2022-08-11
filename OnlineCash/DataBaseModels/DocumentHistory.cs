using System;

namespace OnlineCash.DataBaseModels
{
    public class DocumentHistory
    {
        public int Id { get; set; }
        public DateTime DateAppend { get; set; } = DateTime.Now;
        public int DocId { get; set; }
        public TypeDocs TypeDoc { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
