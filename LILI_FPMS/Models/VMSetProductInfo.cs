using System;

namespace LILI_FPMS.Models
{
    public class VMSetProductInfo
    {
       
            public int Id { get; set; }
            public string ProductCode { get; set; }
            //[NotMapped]
            public string ProductName { get; set; }
            //[NotMapped]
            public decimal StandardOutput { get; set; }
            public decimal BatchSize { get; set; }
            public decimal MonthlyPlannedQTY { get; set; }
            public decimal ProductionQty { get; set; }

            public int BOMId { get; set; }

        
    }
}
