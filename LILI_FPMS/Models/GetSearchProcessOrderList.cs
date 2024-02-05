using System;
using System.Collections.Generic;

namespace LILI_FPMS.Models
{
    public partial class GetSearchProcessOrderList
    {
        public Int32 Id { get; set; }
        public string ProcessOrderNo { get; set; }
        public DateTime ProcessOrderDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}
