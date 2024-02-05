using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebRequisition
    {
        public TblRebRequisition()
        {
            TblRebRequisitionApprovalStatus = new HashSet<TblRebRequisitionApprovalStatus>();
            TblRebRequisitionDetail = new HashSet<TblRebRequisitionDetail>();
            TblRebReturn = new HashSet<TblRebReturn>();
            TblRebSfgtransfer = new HashSet<TblRebSfgtransfer>();
        }

        public int Id { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string BusinessCode { get; set; }
        public string BatchNo { get; set; }
        public decimal NumberOfBatch { get; set; }
        public string ProductCode { get; set; }
        public string Comments { get; set; }
        public string ExtOfRequisitionNo { get; set; }
        public string LinkedProcessNo { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string IssueStatus { get; set; }
        public string TypeCode { get; set; }

        public TblRebRequisition IdNavigation { get; set; }
        public TblRebRequisition InverseIdNavigation { get; set; }
        public ICollection<TblRebRequisitionApprovalStatus> TblRebRequisitionApprovalStatus { get; set; }
        public ICollection<TblRebRequisitionDetail> TblRebRequisitionDetail { get; set; }
        public ICollection<TblRebReturn> TblRebReturn { get; set; }
        public ICollection<TblRebSfgtransfer> TblRebSfgtransfer { get; set; }
    }
}
