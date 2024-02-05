using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRequisitionLine
    {
        public int Id { get; set; }
        public string ProcessOrderNo { get; set; }
        public string LineCode { get; set; }
        [NotMapped]
        public string RequisitionNo { get; set; }

        public TblProcessOrder ProcessOrderNoNavigation { get; set; }
    }
}
