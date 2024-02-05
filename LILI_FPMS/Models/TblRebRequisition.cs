using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebRequisition
    {
        public TblRebRequisition()
        {
            TblRebRequisitionApprovalStatus = new HashSet<TblRebRequisitionApprovalStatus>();
            TblRebRequisitionDetail = new List<TblRebRequisitionDetail>();
            TblRebSfgtransfer = new HashSet<TblRebSfgtransfer>();
            TblRebReturn = new HashSet<TblRebReturn>();
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
        [NotMapped]
        public string[] MachineCode { get; set; }

        [NotMapped]
        public string[] LineCode { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

        [NotMapped]
        public decimal StandardOutput { get; set; }

        [NotMapped]
        public decimal BatchSize { get; set; }

        [NotMapped]
        public string ApprovalStatus { get; set; }

        [NotMapped]
        public decimal MonthlyPlannedQty { get; set; }

        [NotMapped]
        public decimal MonthlyPlannedPendingQty { get; set; }
        [NotMapped]
        public decimal ProductionQty { get; set; }


        [NotMapped]
        public decimal ProcessOrderNumberOfBatch { get; set; }
        [NotMapped]
        public decimal CompletedNumberOfBatch { get; set; }
        [NotMapped]
        public decimal PendingNumberOfBatch { get; set; }



        public TblRebRequisition IdNavigation { get; set; }
        public TblRebRequisition InverseIdNavigation { get; set; }
        public ICollection<TblRebRequisitionApprovalStatus> TblRebRequisitionApprovalStatus { get; set; }
        public List<TblRebRequisitionDetail> TblRebRequisitionDetail { get; set; }
        public ICollection<TblRebSfgtransfer> TblRebSfgtransfer { get; set; }
        public ICollection<TblRebReturn> TblRebReturn { get; set; }
    }
}
