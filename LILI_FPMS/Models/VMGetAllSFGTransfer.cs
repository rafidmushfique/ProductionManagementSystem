using System;

namespace LILI_FPMS.Models
{
    public class VMGetAllSFGTransfer
    {
        public int Id { get; set; }
        public string RequisitionNo { get; set; }



        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public decimal NumberOfBatch { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string IssueStatus { get; set; }
        public string LinkedProcessNo { get; set; }
        public string TransferNo { get; set; }



    }
}
