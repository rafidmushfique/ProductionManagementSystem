using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblProcessOrder
    {
        public TblProcessOrder()
        {
           


                TblProcessOrderDetail = new List<TblProcessOrderDetail>();

                TblRequisitionLine = new List<TblRequisitionLine>();
                //TblRequisitionMachineSetup = new List<TblRequisitionMachineSetup>();
            
        }
        public int Id { get; set; }
        [Required]
        public string ProcessOrderNo { get; set; }
        public DateTime ProcessOrderDate { get; set; }
        public string BusinessCode { get; set; }
        public string BatchNo { get; set; }
        public decimal NumberOfBatch { get; set; }
        public string ProductCode { get; set; }
        public string Comments { get; set; }
        public string ExtOfProcessOrderNo { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string IssueStatus { get; set; }
        public long? Bomid { get; set; }
        public decimal ManPower { get; set; }

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
        public TblProcessOrder IdNavigation { get; set; }
        public TblProcessOrder InverseIdNavigation { get; set; }


        public List<TblProcessOrderDetail> TblProcessOrderDetail { get; set; }
        public List<TblRequisitionLine> TblRequisitionLine { get; set; }

    }
}
