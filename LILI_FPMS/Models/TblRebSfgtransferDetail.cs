using LILI_FPMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Temp_Models
{
    public partial class TblRebSfgtransferDetail
    {
        public int Id { get; set; }
        public string TransferNo { get; set; }
        public string RequisitionNo { get; set; }
        public string MaterialCode { get; set; }
        public decimal StandardRecipeQty { get; set; }
        public decimal FloorStock { get; set; }
        public decimal RequiredQty { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal? EstimatedQty { get; set; }
        public decimal IssueQty { get; set; }
        [NotMapped]
        public string MaterialName { get; set; }

        [NotMapped]
        public string Unit { get; set; }
        public TblRebSfgtransfer TransferNoNavigation { get; set; }
    }
}
