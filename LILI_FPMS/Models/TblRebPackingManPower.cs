using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebPackingManPower
    {
        public int Id { get; set; }
        public int ProductionId { get; set; }
        public string StaffId { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public TblRebProductionProcess Production { get; set; }
    }
}
