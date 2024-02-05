using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebManufacturingMachine
    {
        public int Id { get; set; }
        public int ProductionId { get; set; }
        public string MachineCode { get; set; }
        public decimal MachineHour { get; set; }
        public decimal ManufacMachineNoOfWorker { get; set; }
        public decimal ManufacMachineManHour { get; set; }
        [NotMapped]
        public string MachineName { get; set; }
        public TblRebProductionProcess Production { get; set; }
    }
}
