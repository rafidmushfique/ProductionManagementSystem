﻿using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblBomdetail
    {
        public long Id { get; set; }
        public long? Bomid { get; set; }
        public int ItemNo { get; set; }
        public string MaterialCode { get; set; }
        public decimal CostPerProductUnit { get; set; }
        public decimal QuantityPerBatch { get; set; }
        public decimal LossQuantity { get; set; }
        public decimal PerBatchQuantityExcludingLoss { get; set; }
        public decimal Tolerance { get; set; }
    }
}
