using System;
using System.Collections.Generic;

namespace LILI_FPMS.Models
{
    public partial class TblRebStifgreceiveDetail
    {
        public int Id { get; set; }
        public string StifgreceiveNo { get; set; }
        public string Fgcode { get; set; }
        public string Fgname { get; set; }
        public string Unit { get; set; }
        public decimal Stiquantity { get; set; }
        public decimal ReceiveQuantity { get; set; }
        public decimal ActualReceiveQty { get; set; }
        public string Comments { get; set; }

        public TblRebStifgreceive StifgreceiveNoNavigation { get; set; }
    }
}
