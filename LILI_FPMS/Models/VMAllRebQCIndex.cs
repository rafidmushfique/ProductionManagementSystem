using Org.BouncyCastle.Asn1.Misc;
using System;

namespace LILI_FPMS.Models
{
    public class VMAllRebQCIndex
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Qcno { get; set; }
        public DateTime Qcdate { get; set; }
        public string LinkedProcessNo { get; set; }
        public string BatchNo { get; set; }
        public string ProcessNo { get; set; }
        public decimal Qcqty { get; set; }
        public decimal QcpassQty { get; set; }
        public decimal QcrejectQty { get; set; }
    }
}
