using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebProcessOrderLine
    {
        public int Id { get; set; }
        public string ProcessOrderNo { get; set; }
        public string LineCode { get; set; }

        public TblRebProcessOrder ProcessOrderNoNavigation { get; set; }
    }
}
