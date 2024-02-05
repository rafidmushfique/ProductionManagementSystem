﻿using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebReturnDetails
    {
        public int Id { get; set; }
        public string ReturnNo { get; set; }
        public string MaterialCode { get; set; }
        public string Grnno { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal ReturnQty { get; set; }
        public string Type { get; set; }

        public TblRebReturn ReturnNoNavigation { get; set; }
    }
}
