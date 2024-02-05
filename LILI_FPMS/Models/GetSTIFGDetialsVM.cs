using System;
using System.Collections.Generic;
namespace LILI_FPMS.Models
{
    public partial class GetSTIFGDetialsVM
    {
        public int Id { get; set; }
        public string Fgcode { get; set; }
        public string Fgname { get; set; }
        public string Unit { get; set; }
        public decimal Stiquantity { get; set; }

    }
}
