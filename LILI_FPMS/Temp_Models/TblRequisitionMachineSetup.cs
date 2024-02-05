using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
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
