using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LILI_FPMS.Models;
using LILI_FPMS.Models.ManageViewModels;
using LILI_FPMS.Models;
using LILI_FPMS.Models.ReprotsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace LILI_FPMS.Controllers
{
    public class RebProductionController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public RebProductionController(dbFormulationProductionSystemContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ProcessNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "processNo_desc" : "";
            ViewData["ProcessDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "processDate_desc" : "";
            ViewData["RequisitionNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "requisitionNo_desc" : "";
            ViewData["ProductCodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "productCode_desc" : "";
            ViewData["ProductNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "productName_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var pross = from s in _context.TblRebProductionProcess
                        from po in _context.TblRebProcessOrder
                        from p in _context.View_Product
                        where po.ProductCode == p.ProductCode && po.ProcessOrderNo == s.LinkedProcessNo
                        select new TblRebProductionProcess
                        {
                            Id = s.Id,
                            ProcessNo = s.ProcessNo,
                            LinkedProcessNo = s.LinkedProcessNo,
                            ProcessDate = s.ProcessDate,
                            BatchNo = s.BatchNo,
                            RequisitionNo = s.RequisitionNo,
                            ProductCode = po.ProductCode,
                            ProductName = p.ProductName,
                            ProductionQty = s.ProductionQty,
                            Comments = s.Comments
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                pross = pross.Where(s => s.ProcessNo.Contains(searchString)
                                    || s.RequisitionNo.Contains(searchString)
                                    || s.ProductCode.Contains(searchString)
                                    || s.ProductName.Contains(searchString)
                                    || s.LinkedProcessNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "processNo_desc":
                    pross = pross.OrderByDescending(s => s.ProcessNo);
                    break;
                case "processDate_desc":
                    pross = pross.OrderByDescending(s => s.ProcessDate);
                    break;
                case "requisitionNo_desc":
                    pross = pross.OrderByDescending(s => s.RequisitionNo);
                    break;
                case "productCode_desc":
                    pross = pross.OrderByDescending(s => s.ProductCode);
                    break;
                case "productName_desc":
                    pross = pross.OrderByDescending(s => s.ProductName);
                    break;
                default:
                    pross = pross.OrderByDescending(s => s.ProcessNo);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<TblRebProductionProcess>.CreateAsync(pross.AsNoTracking(), pageNumber ?? 1, pageSize));

        }
        public ActionResult Create()
        {
            TblRebProductionProcess entities = new TblRebProductionProcess();
            entities.ProcessNo = GenerateProcessNo();
            entities.ProcessDate = DateTime.Now;
            entities.ProductionQtyConversionFactor = 1;
            entities.PackingQty = 0;
            entities.CodingQty = 0;
            entities.ManufacBatchStartTime = DateTime.Now;
            entities.ManufacBatchEndTime = DateTime.Now;
            entities.PackBatchStartTime = DateTime.Now;
            entities.PackBatchEndTime = DateTime.Now;


            var sfgList = (from c in _context.View_Product
                           select new
                           {
                               SFGCode = c.ProductCode,
                               SFGName = c.ProductName,
                               Business = c.Business
                           }).Where(c => c.Business == "A").ToList();
            sfgList.Insert(0, new { SFGCode = "", SFGName = "Select By-product", Business = "A" });
            ViewBag.ListOfSFG = sfgList;

            List<TblShiftSetup> shiftList = new List<TblShiftSetup>();
            shiftList = (from c in _context.TblShiftSetup
                         select c).ToList();
            shiftList.Insert(0, new TblShiftSetup { ShiftCode = "", ShiftName = "Select Shift" });
            ViewBag.ListOfShift = shiftList;

            List<TblLineSetup> lineList = new List<TblLineSetup>();
            lineList = (from c in _context.TblLineSetup
                        select c).ToList();
            lineList.Insert(0, new TblLineSetup { LineCode = "", LineName = "Select Line" });
            ViewBag.ListofLine = lineList;

            List<TblMachineName> machineList = new List<TblMachineName>();
            machineList = (from c in _context.TblMachineName
                           select c).ToList();
            machineList.Insert(0, new TblMachineName { MachineCode = "", MachineName = "Select Machine" });
            ViewBag.ListofMachine = machineList;


            List<TblManufacturingBreakDownCause> ManufacturingBreakDownCauseList = new List<TblManufacturingBreakDownCause>();
            ManufacturingBreakDownCauseList = (from c in _context.TblManufacturingBreakDownCause select c).OrderBy(c => c.BreakeDownCause).ToList();
            ViewBag.ManufacturingBreakDownCauseList = ManufacturingBreakDownCauseList;

            List<TblProductionManPower> staffList = new List<TblProductionManPower>();
            staffList = (from c in _context.TblProductionManPower select c).ToList();
            ViewBag.ListofStaff = staffList;


            int isCodingDetailVisible = 0;
            isCodingDetailVisible = _context.TblVisibility.Where(x => x.ItemName == "Production Coding Detail").FirstOrDefault().Isvisible;
            ViewBag.IsCodingDetailVisible = isCodingDetailVisible;

            return View(entities);
        }

        public async Task<IActionResult> CreateProduction(TblRebProductionProcess prod)
        {
            try
            {
              

                if (ModelState.IsValid)
                {





                    if (DoesProcessNoExists(prod.ProcessNo))
                    {
                        prod.ProcessNo = GenerateProcessNo();
                    }
                    prod.Iuser = User.Identity.Name;
                    prod.Idate = DateTime.Now;
                    prod.IssueNo = prod.IssueNo == null ? "0" : prod.IssueNo;
                    _context.Add(prod);
                    await _context.SaveChangesAsync();





                }
                else
                {
                    TempData["Error"] = "Error message text.";
                    return View("Create", prod);
                }

                // Update tblFloorStock Table
                UpdateFloorStockFromProductionConsumption(prod.ProcessNo);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            //return View(student);
            return RedirectToAction(nameof(Index));
        }



        public ActionResult Update(int id)
        {
            TblRebProductionProcess prodModel = new TblRebProductionProcess();
            var dt = _context.TblRebProductionProcess.Where(s => s.Id == id).First();
            prodModel.Id = dt.Id;
            prodModel.ProcessNo = dt.ProcessNo;
            prodModel.ProcessDate = dt.ProcessDate;
            prodModel.RequisitionNo = dt.RequisitionNo;
            prodModel.ProductCode = _context.TblRebProcessOrder.Where(s => s.ProcessOrderNo == dt.LinkedProcessNo).FirstOrDefault().ProductCode;
            prodModel.ProductName = _context.View_Product.Where(s => s.ProductCode == prodModel.ProductCode).FirstOrDefault().ProductName;
            prodModel.BatchNo = dt.BatchNo; //_context.TblRequisition.Where(s => s.RequisitionNo == dt.RequisitionNo).FirstOrDefault().BatchNo;
            prodModel.StandardOutput = 0;
            prodModel.BatchSize =0;
            prodModel.IssueNo = dt.IssueNo;
            prodModel.ProductionQty = dt.ProductionQty;
           
            prodModel.NumberOfBatch = dt.NumberOfBatch;
            prodModel.ManufacBatchStartTime = dt.ManufacBatchStartTime;
            prodModel.ManufacBatchEndTime = dt.ManufacBatchEndTime;
            prodModel.ManufacShiftCode = dt.ManufacShiftCode;
            prodModel.LinkedProcessNo = dt.LinkedProcessNo;
            //prodModel.ManufacShiftBreakDownChangeTime = dt.ManufacShiftBreakDownChangeTime;
            //prodModel.ManufacLineCode = dt.ManufacLineCode;
            //prodModel.ManufacMachineCode = dt.ManufacMachineCode;
            //prodModel.ManufacMachineCapacity = _context.TblMachineSetup.Where(s => s.MachineCode == dt.ManufacMachineCode).FirstOrDefault().Capacity;
            //prodModel.ManufacMachineHour = dt.ManufacMachineHour;
            prodModel.ManufacManHour = dt.ManufacManHour;
            prodModel.ManufacNoOfWorker = dt.ManufacNoOfWorker;
            prodModel.ManufacCommonManHour = dt.ManufacCommonManHour;
            prodModel.ManufacCommonNoOfWorker = dt.ManufacCommonNoOfWorker;
            prodModel.PackBatchStartTime = dt.PackBatchStartTime;
            prodModel.PackBatchEndTime = dt.PackBatchEndTime;
            prodModel.PackShiftCode = dt.PackShiftCode;
            prodModel.PackingQty = dt.PackingQty;
            //prodModel.PackShiftBreakDownChangeTime = dt.PackShiftBreakDownChangeTime;
            //prodModel.PackLineCode = dt.PackLineCode;
            //prodModel.PackMachineCode = dt.PackMachineCode;
            //prodModel.PackMachineCapacity = _context.TblMachineSetup.Where(s => s.MachineCode == dt.PackMachineCode).FirstOrDefault().Capacity;
            prodModel.PackMachineHour = dt.PackMachineHour;
            prodModel.PackManHour = dt.PackManHour;
            prodModel.PackNoOfWorker = dt.PackNoOfWorker;
            prodModel.PackCommonManHour = dt.PackCommonManHour;
            prodModel.PackCommonNoOfWorker = dt.PackCommonNoOfWorker;

            prodModel.CodeBatchStartTime = dt.CodeBatchStartTime;
            prodModel.CodeBatchEndTime = dt.CodeBatchEndTime;
            prodModel.CodeMachineHour = dt.CodeMachineHour;
            prodModel.CodeManHour = dt.CodeManHour;
            prodModel.CodeNoOfWorker = dt.CodeNoOfWorker;
            prodModel.CodeCommonManHour = dt.CodeCommonManHour;
            prodModel.CodeCommonNoOfWorker = dt.CodeCommonNoOfWorker;
            prodModel.CodingQty = dt.CodingQty;

            prodModel.Comments = dt.Comments;
            prodModel.ProductionQtyBeforeConversion = dt.ProductionQtyBeforeConversion;
            prodModel.ProductionQtyConversionFactor = dt.ProductionQtyConversionFactor;
           
            prodModel.LumpQty = dt.LumpQty;
            prodModel.IsProcessCompleted = dt.IsProcessCompleted;

            var sfgList = (from c in _context.View_Product
                           select new
                           {
                               SFGCode = c.ProductCode,
                               SFGName = c.ProductName,
                               Business = c.Business
                           }).Where(c => c.Business == "A").ToList();
            sfgList.Insert(0, new { SFGCode = "", SFGName = "Select By-product", Business = "A" });
            ViewBag.ListOfSFG = sfgList;

            List<TblShiftSetup> shiftList = new List<TblShiftSetup>();
            shiftList = (from c in _context.TblShiftSetup
                         select c).ToList();
            shiftList.Insert(0, new TblShiftSetup { ShiftCode = "", ShiftName = "Select Shift" });
            ViewBag.ListOfShift = shiftList;

            List<TblLineSetup> lineList = new List<TblLineSetup>();
            lineList = (from c in _context.TblLineSetup
                        select c).ToList();
            lineList.Insert(0, new TblLineSetup { LineCode = "", LineName = "Select Line" });
            ViewBag.ListofLine = lineList;

            List<TblMachineName> machineList = new List<TblMachineName>();
            machineList = (from c in _context.TblMachineName
                           select c).ToList();
            machineList.Insert(0, new TblMachineName { MachineCode = "", MachineName = "Select Machine" });
            ViewBag.ListofMachine = machineList;

            List<TblManufacturingBreakDownCause> ManufacturingBreakDownCauseList = new List<TblManufacturingBreakDownCause>();
            ManufacturingBreakDownCauseList = (from c in _context.TblManufacturingBreakDownCause select c).OrderBy(c => c.BreakeDownCause).ToList();
            //ManufacturingBreakDownCauseList.Insert(0, new TblManufacturingBreakDownCause { BreakeDownCauseId = 0, BreakeDownCause = "Select Cause" });
            ViewBag.ManufacturingBreakDownCauseList = ManufacturingBreakDownCauseList;

            List<TblProductionManPower> staffList = new List<TblProductionManPower>();
            staffList = (from c in _context.TblProductionManPower select c).ToList();
            //  staffList.Insert(0, new TblProductionManPower { StaffId = "", Name = "Select Staff" });
            ViewBag.ListofStaff = staffList;

            prodModel.TblRebProductionProcessDetail = _context.TblRebProductionProcessDetail.Where(d => d.ProcessNo == dt.ProcessNo).ToList();


            var manufacturingShiftDetail = from pp_master in _context.TblRebProductionProcess
                                           from manu_shift in _context.TblManufacturingShift
                                           where (pp_master.Id == manu_shift.ProductionId)
                                           from br_cause in _context.TblManufacturingBreakDownCause
                                           where (manu_shift.BreakeDownCauseId == br_cause.BreakeDownCauseId)
                                           where pp_master.Id == id
                                           select new TblRebManufacturingShift
                                           {
                                               Id = manu_shift.Id,
                                               ProductionId = pp_master.Id,
                                               ShiftCode = manu_shift.ShiftCode,
                                               BreakeDownCauseId = manu_shift.BreakeDownCauseId,
                                               BreakeDownCauseName = br_cause.BreakeDownCause,
                                               BreakeDownTime = manu_shift.BreakeDownTime
                                           };
            prodModel.TblRebManufacturingShift = manufacturingShiftDetail.ToList();


            var manufacturingLineDetail = from pp_master in _context.TblRebProductionProcess
                                          from manu_line in _context.TblManufacturingLine
                                          where (pp_master.Id == manu_line.ProductionId)
                                          from line in _context.TblLineSetup
                                          where (manu_line.LineCode == line.LineCode)
                                          where pp_master.Id == id
                                          select new TblRebManufacturingLine
                                          {
                                              Id = manu_line.Id,
                                              ProductionId = pp_master.Id,
                                              LineCode = manu_line.LineCode,
                                              LineName = line.LineName
                                          };
            prodModel.TblRebManufacturingLine = manufacturingLineDetail.ToList();

            var manufacturingMachineDetail = from pp_master in _context.TblRebProductionProcess
                                             from manu_machine in _context.TblManufacturingMachine
                                             where (pp_master.Id == manu_machine.ProductionId)
                                             from machine in _context.TblMachineName
                                             where (manu_machine.MachineCode == machine.MachineCode)
                                             where pp_master.Id == id
                                             select new TblRebManufacturingMachine
                                             {
                                                 Id = manu_machine.Id,
                                                 ProductionId = pp_master.Id,
                                                 MachineCode = manu_machine.MachineCode,
                                                 MachineName = machine.MachineName,
                                                 MachineHour = manu_machine.MachineHour,
                                                 ManufacMachineNoOfWorker = manu_machine.ManufacMachineNoOfWorker,
                                                 ManufacMachineManHour = manu_machine.ManufacMachineManHour
                                             };
            prodModel.TblRebManufacturingMachine = manufacturingMachineDetail.ToList();

            var manufacturingManPowerDetail = from pp_master in _context.TblRebProductionProcess
                                              from manu_manpower in _context.TblManufacturingManPower
                                              where (pp_master.Id == manu_manpower.ProductionId)
                                              from manpower in _context.TblProductionManPower
                                              where (manu_manpower.StaffId == manpower.StaffId)
                                              where pp_master.Id == id
                                              select new TblRebManufacturingManPower
                                              {
                                                  Id = manu_manpower.Id,
                                                  ProductionId = pp_master.Id,
                                                  StaffId = manu_manpower.StaffId,
                                                  Name = manpower.Name,

                                              };
            prodModel.TblRebManufacturingManPower = manufacturingManPowerDetail.ToList();

            var packingShiftDetail = from pp_master in _context.TblRebProductionProcess
                                     from packing_shift in _context.TblPackingDetailShift
                                     where (pp_master.Id == packing_shift.ProductionId)
                                     from br_cause in _context.TblManufacturingBreakDownCause
                                     where (packing_shift.BreakeDownCauseId == br_cause.BreakeDownCauseId)
                                     where pp_master.Id == id
                                     select new TblRebPackingDetailShift
                                     {
                                         Id = packing_shift.Id,
                                         ProductionId = pp_master.Id,
                                         ShiftCode = packing_shift.ShiftCode,
                                         BreakeDownCauseId = packing_shift.BreakeDownCauseId,
                                         BreakeDownCauseName = br_cause.BreakeDownCause,
                                         BreakeDownTime = packing_shift.BreakeDownTime
                                     };
            prodModel.TblRebPackingDetailShift = packingShiftDetail.ToList();


            var packingLineDetail = from pp_master in _context.TblRebProductionProcess
                                    from packing_line in _context.TblPackingDetailLine
                                    where (pp_master.Id == packing_line.ProductionId)
                                    from line in _context.TblLineSetup
                                    where (packing_line.LineCode == line.LineCode)
                                    where pp_master.Id == id
                                    select new TblRebPackingDetailLine
                                    {
                                        Id = packing_line.Id,
                                        ProductionId = pp_master.Id,
                                        LineCode = packing_line.LineCode,
                                        LineName = line.LineName
                                    };
            prodModel.TblRebPackingDetailLine = packingLineDetail.ToList();

            var packingMachineDetail = from pp_master in _context.TblRebProductionProcess
                                       from packing_machine in _context.TblPackingDetailMachine
                                       where (pp_master.Id == packing_machine.ProductionId)
                                       from machine in _context.TblMachineName
                                       where (packing_machine.MachineCode == machine.MachineCode)
                                       where pp_master.Id == id
                                       select new TblRebPackingDetailMachine
                                       {
                                           Id = packing_machine.Id,
                                           ProductionId = pp_master.Id,
                                           MachineCode = packing_machine.MachineCode,
                                           MachineName = machine.MachineName,
                                           MachineHour = packing_machine.MachineHour,
                                           PackMachineNoOfWorker = packing_machine.PackMachineNoOfWorker,
                                           PackMachineManHour = packing_machine.PackMachineManHour
                                       };
            prodModel.TblRebPackingDetailMachine = packingMachineDetail.ToList();

            var packingManPowerDetail = from pp_master in _context.TblRebProductionProcess
                                        from packing_manpower in _context.TblPackingManPower
                                        where (pp_master.Id == packing_manpower.ProductionId)
                                        from manpower in _context.TblProductionManPower
                                        where (packing_manpower.StaffId == manpower.StaffId)
                                        where pp_master.Id == id
                                        select new TblRebPackingManPower
                                        {
                                            Id = packing_manpower.Id,
                                            ProductionId = pp_master.Id,
                                            StaffId = packing_manpower.StaffId,
                                            Name = manpower.Name,

                                        };
            prodModel.TblRebPackingManPower = packingManPowerDetail.ToList();
            #region Code Details part

          
            #endregion

            var productionDetail = from prodDtl in _context.TblRebProductionProcessDetail
                                   from mat in _context.View_Material
                                   where (prodDtl.ProcessNo == dt.ProcessNo && prodDtl.MaterialCode == mat.MaterialCode)
                                   orderby prodDtl.MaterialCode
                                   select new TblRebProductionProcessDetail
                                   {
                                       Id = prodDtl.Id,
                                       MaterialCode = prodDtl.MaterialCode,
                                       MaterialName = mat.MaterialName,
                                       Unit = mat.BaseUnit,
                                       ReqQuantity = prodDtl.ReqQuantity,
                                       IssuedQty = prodDtl.IssuedQty,
                                       PreviousUsedQty = prodDtl.PreviousUsedQty,
                                       StdConsumptionQty = prodDtl.StdConsumptionQty,
                                       CurrentUseQty = prodDtl.CurrentUseQty,
                                       ProcessLoss = prodDtl.ProcessLoss,
                                       Wastage = prodDtl.Wastage,
                                       TotalConsumption = prodDtl.TotalConsumption,
                                       RequisitionNo = prodDtl.RequisitionNo,
                                       Grnno = prodDtl.Grnno,
                                       RejectQty = prodDtl.RejectQty,
                                       Wip = prodDtl.Wip,
                                   };


            prodModel.TblRebProductionProcessDetail = productionDetail.ToList();

            int isCodingDetailVisible = 0;
            isCodingDetailVisible = _context.TblVisibility.Where(x => x.ItemName == "Production Coding Detail").FirstOrDefault().Isvisible;
            ViewBag.IsCodingDetailVisible = isCodingDetailVisible;

            return View(prodModel);
        }

        public IActionResult SearchProcessOrder(string searchKey, bool jsonRequest = false)
        {
            var processOrderNoParam = new SqlParameter("@processOrderNo", searchKey == null ? "" : searchKey);
            var model = _context.GetSearchProcessOrderList
                               .FromSql("EXEC sp_SearchRebProcessOrder @processOrderNo", processOrderNoParam)
                               .ToList();

            if (jsonRequest)
            {
                var sa = new JsonSerializerSettings();
                return Json(model, sa);
            }
            else
            {
                return PartialView("_RequisitionPartial", model);
            }




        }
        public JsonResult GetShiftDetail(string ShiftCode)
        {


            var sa = new JsonSerializerSettings();

            var model = new List<TblShiftSetup>(from s in _context.TblShiftSetup
                                                where (s.ShiftCode == ShiftCode)
                                                select new TblShiftSetup
                                                {
                                                    ShiftStartTime = s.ShiftStartTime,
                                                    ShiftEndTime = s.ShiftEndTime
                                                });

            return Json(model, sa);
        }
        public JsonResult SetProcessOrder(string processOrderNo)
        {
          


                var sa = new JsonSerializerSettings();
                try
                {
                    var processOrderInfo = from r in _context.TblRebProcessOrder
                                           from p in _context.View_Product
                                           where (r.ProcessOrderNo == processOrderNo && r.ProductCode == p.ProductCode)
                                           select new TblRebProductionProcess
                                           {
                                               LinkedProcessNo = r.ProcessOrderNo,
                                               RequisitionNo = "",
                                               BatchNo = r.BatchNo,
                                               ProductCode = r.ProductCode,
                                               ProductName = p.ProductName,
                                               StandardOutput =0,
                                               BatchSize = 0,
                                               NumberOfBatch = r.NumberOfBatch,
                                               ProductionQty = 0,
                                               PreviousProcessedBatchNo = 0,
                                               NoOfBatchInRequisition = 0,

                                           };
                    return Json(processOrderInfo, sa);
                }
                catch (Exception ex)
                {

                    return Json("");
                }




            
            //else
            //{
            //    return Json("");
            //}
        }



        public JsonResult GetFGProductWiseMachineList(string processOrderNo)
        {
            var productCode = _context.TblRebProcessOrder.Where(x => x.ProcessOrderNo == processOrderNo).FirstOrDefault().ProductCode;
            var model = new List<TblMachineName>(from ms in _context.TblMachineSetup
                                                 from m in _context.TblMachineName
                                                 where (ms.MachineCode == m.MachineCode && (ms.ProductCode == productCode))
                                                 select new TblMachineName
                                                 {
                                                     MachineCode = ms.MachineCode,
                                                     MachineName = m.MachineName
                                                 });
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        public string GenerateProcessNo()
        {
           
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var processNo = "RBP-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblRebProductionProcess.Where(c => c.ProcessNo.Substring(0, 8) == processNo).OrderByDescending(t => t.Id) select c.ProcessNo.Substring(8, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            processNo = "RBP-" + yy + mn + maxId;

          
            return processNo;
        }
        public Boolean DoesProcessNoExists(string vProcessNo)
        {

            return _context.TblRebProductionProcess.Any(e => e.ProcessNo == vProcessNo);

        }

        private List<FloorStockCheckViewModel> FloorStockCheck(TblRebProductionProcess prod)
        {
            //Getting updated FloorStock 
            var floorStockList = new List<FloorStockCheckViewModel>();
            var requsitionNo = prod.RequisitionNo;
            var productionQty = prod.ProductionQty;
            //var sa = new JsonSerializerSettings();
            var requisitionNoParam = new SqlParameter("@requisitionNo", requsitionNo);
            var productionQtyParam = new SqlParameter("@productionQty", productionQty);
            var productList = _context.GetTblProductionProcessDetail
                                .FromSql("EXEC sp_GetRequisitionDetail @requisitionNo, @productionQty", requisitionNoParam, productionQtyParam)
                                .ToList();
            var userProductList = prod.TblRebProductionProcessDetail.ToList();

            //CurrentUseQty and Floorstock check : CurrentUseQty can not be greater than Floorstock\
            if (productList != null && userProductList != null)
            {
                foreach (var item in userProductList)
                {
                    var floorStockItem = new FloorStockCheckViewModel();
                    var itemFloorStock = userProductList.Where(x => x.MaterialCode == item.MaterialCode).Select(x => x.FloorStock);
                    var compareItemStock = decimal.Compare(item.CurrentUseQty, item.FloorStock);
                    if (compareItemStock > 0)
                    {

                        floorStockItem.MaterialName = item.MaterialName;
                        floorStockItem.MaterialCode = item.MaterialCode;
                        floorStockItem.FloorStockQty = item.FloorStock;
                        floorStockList.Add(floorStockItem);

                    }

                }
            }

            return floorStockList;
        }
        public JsonResult UpdateFloorStockFromProductionConsumption(string processNo)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();
            var processNoParam = new SqlParameter("@ProcessNo", processNo);

            var productList = _context.UpdateFloorScockFromForductionQC
                                .FromSql("EXEC sp_RebUpdateFloorStockConsumption @ProcessNo", processNoParam)
                                .ToList();

            return Json(productList, sa);
        }

        public IActionResult AddMaterialSearch()
        {
            TblRebRmpmsfgreceiveDetail materialSearchModel = new TblRebRmpmsfgreceiveDetail();
            return PartialView("_MaterialSearchPartial", materialSearchModel);
        }

        [HttpPost]
        public JsonResult SearchMaterial(string MaterialSearchKey)
        {
            var model = new List<TblRebRmpmsfgreceiveDetail>(from b in _context.View_Material
                                                             where ((b.MaterialCode.ToUpper().Contains(MaterialSearchKey.ToUpper()) || b.MaterialName.ToUpper().Contains(MaterialSearchKey.ToUpper())))
                                                             select new TblRebRmpmsfgreceiveDetail
                                                             {
                                                                 MaterialCode = b.MaterialCode,
                                                                 MaterialName = b.MaterialName,
                                                                 Unit = b.BaseUnit
                                                             });
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        [HttpPost]
        public JsonResult SetMaterial(string MaterialCode)
        {
            var keyVal = _context.View_Material.Where(c => c.MaterialCode == MaterialCode).ToList();
            if (keyVal != null)
            {
                MaterialCode = keyVal.FirstOrDefault().MaterialCode;
            }

            if (MaterialCode != "")
            {
                var sa = new JsonSerializerSettings();
                var materialInfo = from c in _context.View_Material.Where(c => c.MaterialCode == MaterialCode).ToList()
                                   select new
                                   {
                                       c.MaterialCode,
                                       c.MaterialName,
                                       c.BaseUnit
                                   };
                return Json(materialInfo, sa);
            }
            else
            {
                return Json("");
            }
        }
    }
}
