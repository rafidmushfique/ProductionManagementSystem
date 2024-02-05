using LILI_FPMS.Models;
using System;
using System.Collections.Generic;

namespace LILI_FPMS.Models
{
    public partial class TblRequisitionMachineSetup
    {
        public int Id { get; set; }
        public string MachineCode { get; set; }
        public string RequisitionNo { get; set; }
        public string ProcessOrderNo { get; set; }
        public TblRequisition RequisitionNoNavigation { get; set; }
    }
}
