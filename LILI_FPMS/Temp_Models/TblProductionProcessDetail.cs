﻿using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblProductionProcessDetail
    {
        public int Id { get; set; }
        public string ProcessNo { get; set; }
        public string MaterialCode { get; set; }
        public decimal ReqQuantity { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal PreviousUsedQty { get; set; }
        public decimal StdConsumptionQty { get; set; }
        public decimal CurrentUseQty { get; set; }
        public decimal ProcessLoss { get; set; }
        public decimal Wastage { get; set; }
        public decimal TotalConsumption { get; set; }
        public decimal? FloorStock { get; set; }
        public string RequisitionNo { get; set; }
        public decimal? RejectQty { get; set; }
        public decimal? Wip { get; set; }
        public string Grnno { get; set; }

        public TblProductionProcess ProcessNoNavigation { get; set; }
    }
}
