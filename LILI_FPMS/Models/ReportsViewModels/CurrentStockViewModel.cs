using System.ComponentModel;

namespace LILI_FPMS.Models.ReprotsViewModels
{
    public class CurrentStockViewModel
    {
        public int Id { get; set; }

        [DisplayName("Plant")]
        public long PlantId { get; set; }

        [DisplayName("Period")]
        public int Period { get; set; }

        [DisplayName("Material Code")]
        public string MaterialCode { get; set; }

        [DisplayName("Material Name")]
        public string MaterialName { get; set; }

        [DisplayName("Opening")]
        public decimal OpeningQuantity { get; set; }

        [DisplayName("Receive")]
        public decimal ReceiveQuantity { get; set; }

        [DisplayName("Consumption")]
        public decimal Consumption { get; set; }

        [DisplayName("Return")]
        public decimal ReturnQuantity { get; set; }

        [DisplayName("Floor Stock")]
        public decimal ClosingBalance { get; set; }

    }
}
