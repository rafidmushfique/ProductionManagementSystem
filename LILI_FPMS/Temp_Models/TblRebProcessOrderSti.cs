using System;
using System.Collections.Generic;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebProcessOrderSti
    {
        public int Id { get; set; }
        public string ProcessOrderNo { get; set; }
        public string Stino { get; set; }

        public TblRebProcessOrder ProcessOrderNoNavigation { get; set; }
    }
}
