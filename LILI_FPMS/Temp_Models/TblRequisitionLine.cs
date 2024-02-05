using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRequisitionLine
    {
        public int Id { get; set; }
        public string ProcessOrderNo { get; set; }
        public string LineCode { get; set; }

        public TblProcessOrder ProcessOrderNoNavigation { get; set; }
    }
}
