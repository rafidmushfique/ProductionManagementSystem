using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
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
        [NotMapped]
        public string MaterialName { get; set; }

        [NotMapped]
        public string Unit { get; set; }
        [NotMapped]
        public decimal AvailableFloorStock { get; set; }

        public TblRebReturn ReturnNoNavigation { get; set; }
    }
}
