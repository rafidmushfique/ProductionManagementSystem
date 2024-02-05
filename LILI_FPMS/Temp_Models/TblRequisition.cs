using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRequisition
    {
        public TblRequisition()
        {
            TblProductionProcess = new HashSet<TblProductionProcess>();
            TblRequisitionDetail = new HashSet<TblRequisitionDetail>();
            TblRequisitionMachineSetup = new HashSet<TblRequisitionMachineSetup>();
            TblReturn = new HashSet<TblReturn>();
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
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string IssueStatus { get; set; }
        public long? Bomid { get; set; }
        public string LinkedProcessNo { get; set; }
        public decimal ManPower { get; set; }

        public TblRequisition IdNavigation { get; set; }
        public TblRequisition InverseIdNavigation { get; set; }
        public ICollection<TblProductionProcess> TblProductionProcess { get; set; }
        public ICollection<TblRequisitionDetail> TblRequisitionDetail { get; set; }
        public ICollection<TblRequisitionMachineSetup> TblRequisitionMachineSetup { get; set; }
        public ICollection<TblReturn> TblReturn { get; set; }
    }
}
