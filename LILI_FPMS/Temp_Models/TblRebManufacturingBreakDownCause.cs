using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebManufacturingBreakDownCause
    {
        public TblRebManufacturingBreakDownCause()
        {
            TblRebManufacturingShift = new HashSet<TblRebManufacturingShift>();
            TblRebPackingDetailShift = new HashSet<TblRebPackingDetailShift>();
        }

        public int BreakeDownCauseId { get; set; }
        public string BreakeDownCause { get; set; }

        public ICollection<TblRebManufacturingShift> TblRebManufacturingShift { get; set; }
        public ICollection<TblRebPackingDetailShift> TblRebPackingDetailShift { get; set; }
    }
}
