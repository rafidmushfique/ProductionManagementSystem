using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebQc
    {
        public TblRebQc()
        {
            TblRebQcdetails = new List<TblRebQcdetails>();
        }

        public int Id { get; set; }
        public string Qcno { get; set; }
        public DateTime Qcdate { get; set; }
        public string BusinessCode { get; set; }
        public string RequisitionNo { get; set; }
        public string ProcessNo { get; set; }
        public decimal Qcqty { get; set; }
        public decimal QcpassQty { get; set; }
        public decimal QcrejectQty { get; set; }
        public decimal Sfgqcqty { get; set; }
        public decimal SfgqcpassQty { get; set; }
        public decimal SfgqcrejectQty { get; set; }
        public decimal QcholdQty { get; set; }
        public string Comments { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public bool? IsSendToFloorStockFg { get; set; }
        public bool? IsSendToFloorStockSfg { get; set; }
        public decimal? FgqcqtyBeforeConversion { get; set; }
        public decimal? FgqcqtyConversionFactor { get; set; }
        public decimal? QcreferenceSampleQty { get; set; }
        public decimal? QcquarantineQty { get; set; }
        public string LinkedProcessNo { get; set; }

        [NotMapped]
        public string ProductCode { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

        [NotMapped]
        public decimal StandardOutput { get; set; }


        [NotMapped]
        public string BatchNo { get; set; }

        [NotMapped]
        public string PackSize { get; set; }

        [NotMapped]
        public decimal BatchSize { get; set; }

        [NotMapped]
        public decimal AvailableStock { get; set; }

        [NotMapped]
        public string TypeCode { get; set; }
        public List<TblRebQcdetails> TblRebQcdetails { get; set; }
    }
}
