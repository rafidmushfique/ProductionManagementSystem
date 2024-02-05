using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblProductionApprovalStatus
    {
        public int Id { get; set; }
        public string ProcessNo { get; set; }
        public DateTime ProcessStatusDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Approver { get; set; }
    }
}
