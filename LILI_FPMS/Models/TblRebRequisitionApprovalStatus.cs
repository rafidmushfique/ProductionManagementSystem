using System;
using System.Collections.Generic;

namespace LILI_FPMS.Models
{
    public partial class TblRebRequisitionApprovalStatus
    {
        public int Id { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionStatusDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Approver { get; set; }

        public TblRebRequisition RequisitionNoNavigation { get; set; }
    }
}
