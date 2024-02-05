using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebRmpmsfgreceiveDetail
    {
        public int Id { get; set; }
        public string ReceiveNo { get; set; }
        public string Rmpmsfgcode { get; set; }
        public string Rmpmsfgname { get; set; }
        public string Unit { get; set; }
        public decimal ReceiveQty { get; set; }
        public string Comments { get; set; }

        public TblRebRmpmsfgreceive ReceiveNoNavigation { get; set; }
    }
}
