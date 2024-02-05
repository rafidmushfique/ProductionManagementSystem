using System;

namespace LILI_FPMS.Models
{
    public class GetRebReturnIndex
    {
        public int Id { get; set; }
        public string ReturnNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public string RequisitionNo { get; set; }
        public string BatchNo { get; set; }
        public string ProcessNo { get; set; }
        public string FGCode {  get; set; }
        public string FGName { get; set; }

    }
}
