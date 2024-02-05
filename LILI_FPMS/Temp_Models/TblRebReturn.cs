using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebReturn
    {
        public TblRebReturn()
        {
            TblRebReturnDetails = new HashSet<TblRebReturnDetails>();
        }

        public int Id { get; set; }
        public string ReturnNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public string BusinessCode { get; set; }
        public string RequisitionNo { get; set; }
        public string IssueNo { get; set; }
        public string Comments { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public TblRebRequisition RequisitionNoNavigation { get; set; }
        public ICollection<TblRebReturnDetails> TblRebReturnDetails { get; set; }
    }
}
