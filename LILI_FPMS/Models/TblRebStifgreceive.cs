using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebStifgreceive
    {

        public TblRebStifgreceive()
        {
            TblRebStifgreceiveDetail = new List<TblRebStifgreceiveDetail>();
        }

        public int Id { get; set; }
        public string StifgreceiveNo { get; set; }
        public DateTime StifgreceiveDate { get; set; }
        public string Stino { get; set; }
        public decimal Stistock { get; set; }
        public string ReceiveComment { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public TblRebStifgreceive IdNavigation { get; set; }
        public TblRebStifgreceive InverseIdNavigation { get; set; }
        public List<TblRebStifgreceiveDetail> TblRebStifgreceiveDetail { get; set; }

    }
}
