using System;
using System.ComponentModel.DataAnnotations;

namespace LILI_FPMS.Models
{
    public class VMAllCurrentSTINo
    {
        public int ID { get; set; }
        public string STINo { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy MM dd}")]
        public DateTime STIDate { get; set; }
    }
}
