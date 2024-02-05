using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblProcessOrder
    {
        public TblProcessOrder()
        {
            TblProcessOrderDetail = new HashSet<TblProcessOrderDetail>();
            TblProductionProcess = new HashSet<TblProductionProcess>();
            TblRequisitionLine = new HashSet<TblRequisitionLine>();
        }

        public int Id { get; set; }
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

        public ICollection<TblProcessOrderDetail> TblProcessOrderDetail { get; set; }
        public ICollection<TblProductionProcess> TblProductionProcess { get; set; }
        public ICollection<TblRequisitionLine> TblRequisitionLine { get; set; }
    }
}
