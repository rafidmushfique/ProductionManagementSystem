using System;
using System.Collections.Generic;

namespace LILI_FPMS.Models
{
    public partial class GetGRNList
    {
        public decimal AvailableStock { get; set; }
        public decimal ConsumeStock { get; set; }
        public string Grnno { get; set; }
        public Int32 Id { get; set; }
        public decimal IssueStock { get; set; }
        public DateTime StockDate { get; set; }
    }
}
