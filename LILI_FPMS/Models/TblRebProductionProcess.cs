using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_FPMS.Models
{
    public partial class TblRebProductionProcess
    {
        public TblRebProductionProcess()
        {
            TblRebManufacturingLine = new List<TblRebManufacturingLine>();
            TblRebManufacturingMachine = new List<TblRebManufacturingMachine>();
            TblRebManufacturingManPower = new List<TblRebManufacturingManPower>();
            TblRebManufacturingShift = new List<TblRebManufacturingShift>();
            TblRebPackingDetailLine = new List<TblRebPackingDetailLine>();
            TblRebPackingDetailMachine = new List<TblRebPackingDetailMachine>();
            TblRebPackingDetailShift = new List<TblRebPackingDetailShift>();
            TblRebPackingManPower = new List<TblRebPackingManPower>();
            TblRebProductionProcessDetail = new List<TblRebProductionProcessDetail>();
        }

        public int Id { get; set; }
        public string ProcessNo { get; set; }
        public DateTime ProcessDate { get; set; }
        public string RequisitionNo { get; set; }
        public string BusinessCode { get; set; }
        public string IssueNo { get; set; }
        public decimal ProductionQty { get; set; }
        public string Sfgcode { get; set; }
        public decimal SfgproductionQty { get; set; }
        public DateTime? ManufacBatchStartTime { get; set; }
        public DateTime? ManufacBatchEndTime { get; set; }
        public string ManufacShiftCode { get; set; }
        public decimal? ManufacShiftBreakDownChangeTime { get; set; }
        public string ManufacLineCode { get; set; }
        public string ManufacMachineCode { get; set; }
        public decimal? ManufacMachineHour { get; set; }
        public decimal? ManufacManHour { get; set; }
        public decimal? ManufacNoOfWorker { get; set; }
        public decimal? ManufacCommonManHour { get; set; }
        public decimal? ManufacCommonNoOfWorker { get; set; }
        public DateTime? PackBatchStartTime { get; set; }
        public DateTime? PackBatchEndTime { get; set; }
        public string PackShiftCode { get; set; }
        public decimal? PackShiftBreakDownChangeTime { get; set; }
        public string PackLineCode { get; set; }
        public string PackMachineCode { get; set; }
        public decimal? PackMachineHour { get; set; }
        public decimal? PackManHour { get; set; }
        public decimal? PackNoOfWorker { get; set; }
        public decimal? PackCommonManHour { get; set; }
        public decimal? PackCommonNoOfWorker { get; set; }
        public string Comments { get; set; }
        public decimal? NumberOfBatch { get; set; }
        public string BatchNo { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public decimal? ProductionQtyBeforeConversion { get; set; }
        public decimal? ProductionQtyConversionFactor { get; set; }
        public decimal? QcreferenceSampleQty { get; set; }
        public decimal? ManufacMachineNoOfWorker { get; set; }
        public decimal? ManufacMachineManHour { get; set; }
        public decimal? PackMachineNoOfWorker { get; set; }
        public decimal? PackMachineManHour { get; set; }
        public decimal? LumpQty { get; set; }
        public DateTime? CodeBatchStartTime { get; set; }
        public DateTime? CodeBatchEndTime { get; set; }
        public string CodeShiftCode { get; set; }
        public decimal? CodeShiftBreakDownChangeTime { get; set; }
        public string CodeLineCode { get; set; }
        public string CodeMachineCode { get; set; }
        public decimal? CodeMachineHour { get; set; }
        public decimal? CodeManHour { get; set; }
        public decimal? CodeNoOfWorker { get; set; }
        public decimal? CodeCommonManHour { get; set; }
        public decimal? CodeCommonNoOfWorker { get; set; }
        public decimal? CodeMachineNoOfWorker { get; set; }
        public decimal? CodeMachineManHour { get; set; }
        public decimal CodingQty { get; set; }
        public decimal PackingQty { get; set; }
        public bool? IsProcessCompleted { get; set; }
        public string LinkedProcessNo { get; set; }
        [NotMapped]
        public string ManufacMachineCapacity { get; set; }
        [NotMapped]
        public string PackMachineCapacity { get; set; }
        [NotMapped]
        public string ProductCode { get; set; }
        [NotMapped]
        public string ProductName { get; set; }

        [NotMapped]
        public decimal StandardOutput { get; set; }
        [NotMapped]
        public decimal BatchSize { get; set; }
        [NotMapped]
        public int BreakeDownCauseId { get; set; }
        [NotMapped]
        public int PackingBreakeDownCauseId { get; set; }

        [NotMapped]
        public int CodingBreakeDownCauseId { get; set; }
        [NotMapped]
        public decimal PreviousProcessedBatchNo { get; set; }
        [NotMapped]
        public decimal NoOfBatchInRequisition { get; set; }
        [NotMapped]
        public string ApprovalStatus { get; set; }
        [NotMapped]
        public string CodeMachineCapacity { get; set; }
        public TblRebProcessOrder LinkedProcessNoNavigation { get; set; }
        public List<TblRebManufacturingLine> TblRebManufacturingLine { get; set; }
        public List<TblRebManufacturingMachine> TblRebManufacturingMachine { get; set; }
        public List<TblRebManufacturingManPower> TblRebManufacturingManPower { get; set; }
        public List<TblRebManufacturingShift> TblRebManufacturingShift { get; set; }
        public List<TblRebPackingDetailLine> TblRebPackingDetailLine { get; set; }
        public List<TblRebPackingDetailMachine> TblRebPackingDetailMachine { get; set; }
        public List<TblRebPackingDetailShift> TblRebPackingDetailShift { get; set; }
        public List<TblRebPackingManPower> TblRebPackingManPower { get; set; }
        public List<TblRebProductionProcessDetail> TblRebProductionProcessDetail { get; set; }
    }
}
