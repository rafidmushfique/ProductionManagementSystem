using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class SetProductInfo
    {
        public Int64 Id { get; set; }
        public string ProductCode { get; set; }
        //[NotMapped]
        public string ProductName { get; set; }
        //[NotMapped]
        public decimal StandardOutput { get; set; }
        public decimal BatchSize { get; set; }
        public decimal MonthlyPlannedQTY { get; set; }
        public decimal ProductionQty { get; set; }

        public long BOMId { get; set; }

        public decimal NumberOfBatch { get; set; }
        public decimal CompletedNumberOfBatch { get; set; }
    }
}
