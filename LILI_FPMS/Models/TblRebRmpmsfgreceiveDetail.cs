using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
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


        [NotMapped]
        public string MaterialCode { get; set; }
        [NotMapped]
        public string MaterialName { get; set; }
        public TblRebRmpmsfgreceive ReceiveNoNavigation { get; set; }
    }
}
