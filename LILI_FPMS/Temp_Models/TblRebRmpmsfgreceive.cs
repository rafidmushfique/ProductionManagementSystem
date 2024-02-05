using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebRmpmsfgreceive
    {
        public TblRebRmpmsfgreceive()
        {
            TblRebRmpmsfgreceiveDetail = new HashSet<TblRebRmpmsfgreceiveDetail>();
        }

        public int Id { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string BatchNo { get; set; }
        public decimal RequisitionQty { get; set; }
        public string Stino { get; set; }
        public string Comments { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public ICollection<TblRebRmpmsfgreceiveDetail> TblRebRmpmsfgreceiveDetail { get; set; }
    }
}
