using LastTradeProject.Entities.Common;

namespace LastTradeProject.Models
{
    public class LastTrade : BaseEntity
    {
        public int InstrumentId { get; set; }
        public string ShortName { get; set; }
        public DateTime DateTimeEn { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}