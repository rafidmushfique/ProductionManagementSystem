using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebManufacturingManPower
    {
        public int Id { get; set; }
        public int ProductionId { get; set; }
        public string StaffId { get; set; }

        public TblRebProductionProcess Production { get; set; }
    }
}
