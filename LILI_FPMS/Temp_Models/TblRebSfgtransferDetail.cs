using System;
using System.Collections.Generic;

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

        public TblRebSfgtransfer TransferNoNavigation { get; set; }
    }
}
