using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebQcdetails
    {
        public int Id { get; set; }
        public string Qcno { get; set; }
        public string QcparameterCode { get; set; }
        public string QcparameterActualValue { get; set; }
        public string Comments { get; set; }
        [NotMapped]
        public string QcparameterName { get; set; }
        [NotMapped]
        public string QcparameterStandardValue { get; set; }
        public TblRebQc QcnoNavigation { get; set; }
    }
}
