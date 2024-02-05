using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebStifgreceive
    {
        public TblRebStifgreceive()
        {
            TblRebStifgreceiveDetail = new HashSet<TblRebStifgreceiveDetail>();
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
        public ICollection<TblRebStifgreceiveDetail> TblRebStifgreceiveDetail { get; set; }
    }
}
