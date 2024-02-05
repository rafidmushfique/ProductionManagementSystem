using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebProcessOrder
    {
        public TblRebProcessOrder()
        {
            TblRebProcessOrderDetail = new HashSet<TblRebProcessOrderDetail>();
            TblRebProcessOrderLine = new HashSet<TblRebProcessOrderLine>();
            TblRebProcessOrderSti = new HashSet<TblRebProcessOrderSti>();
            TblRebProductionProcess = new HashSet<TblRebProductionProcess>();
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

        public ICollection<TblRebProcessOrderDetail> TblRebProcessOrderDetail { get; set; }
        public ICollection<TblRebProcessOrderLine> TblRebProcessOrderLine { get; set; }
        public ICollection<TblRebProcessOrderSti> TblRebProcessOrderSti { get; set; }
        public ICollection<TblRebProductionProcess> TblRebProductionProcess { get; set; }
    }
}
