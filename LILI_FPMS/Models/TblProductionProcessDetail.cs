﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblProductionProcessDetail
    {
        public int Id { get; set; }
        public string ProcessNo { get; set; }
        public string MaterialCode { get; set; }
        public decimal ReqQuantity { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal PreviousUsedQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N5}", ApplyFormatInEditMode = true)]
        public decimal StdConsumptionQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N5}", ApplyFormatInEditMode = true)]
        public decimal CurrentUseQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N5}", ApplyFormatInEditMode = true)]
        public decimal ProcessLoss { get; set; }
        public decimal Wastage { get; set; }
        [DisplayFormat(DataFormatString = "{0:N5}", ApplyFormatInEditMode = true)]
        public decimal TotalConsumption { get; set; }
        
        public decimal FloorStock { get; set; }
       public string RequisitionNo { get; set; }
        public decimal? RejectQty { get; set; }
        public decimal? Wip { get; set; }
        public string Grnno { get; set; }
        public TblProductionProcess ProcessNoNavigation { get; set; }

        [NotMapped]
        public string MaterialName { get; set; }
        [NotMapped]
        public string Unit { get; set; }
    }
}
