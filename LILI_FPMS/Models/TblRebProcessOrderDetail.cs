using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebProcessOrderDetail
    {
        public int Id { get; set; }
        public string ProcessOrderNo { get; set; }
        public string MaterialCode { get; set; }
        public decimal StandardRecipeQty { get; set; }
        public decimal FloorStock { get; set; }
        public decimal RequiredQty { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal? EstimatedQty { get; set; }

        public TblRebProcessOrder ProcessOrderNoNavigation { get; set; }

        [NotMapped]
        public string MaterialName { get; set; }
        [NotMapped]
        public string Unit { get; set; }
    }
}
