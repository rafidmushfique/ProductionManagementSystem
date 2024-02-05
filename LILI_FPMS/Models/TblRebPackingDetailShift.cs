using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebPackingDetailShift
    {
        public int Id { get; set; }
        public int ProductionId { get; set; }
        public string ShiftCode { get; set; }
        public string MachineCode { get; set; }
        public int? BreakeDownCauseId { get; set; }
        public decimal? BreakeDownTime { get; set; }
        [NotMapped]
        public string MachineName { get; set; }
        [NotMapped]
        public string BreakeDownCauseName { get; set; }
        public TblRebManufacturingBreakDownCause BreakeDownCause { get; set; }
        public TblRebProductionProcess Production { get; set; }
    }
}
