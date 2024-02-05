﻿using System;
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
    [Authorize]
    public class ProductionController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public ProductionController(dbFormulationProductionSystemContext context)
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

            var pross = from s in _context.TblProductionProcess
                        from po in _context.TblProcessOrder
                        from p in _context.View_Product
                        where  po.ProductCode == p.ProductCode && po.ProcessOrderNo== s.LinkedProcessNo 
                        select new TblProductionProcess
                        {
                            Id = s.Id,
                            ProcessNo = s.ProcessNo,
                            LinkedProcessNo= s.LinkedProcessNo,
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
            return View(await PaginatedList<TblProductionProcess>.CreateAsync(pross.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public ActionResult Create()
        {
            TblProductionProcess entities = new TblProductionProcess();
            entities.ProcessNo = GenerateProcessNo();
            entities.ProcessDate = DateTime.Now;
            entities.ProductionQtyConversionFactor = 1;
            //DateTime myDateTime = DateTime.UtcNow;

            //entities.ManufacBatchStartTime = myDateTime.TimeOfDay;
            //entities.ManufacBatchEndTime = myDateTime.TimeOfDay;
            //entities.PackBatchStartTime = myDateTime.TimeOfDay;
            //entities.PackBatchStartTime = myDateTime.TimeOfDay;
            entities.PackingQty = 0;
            entities.CodingQty = 0;
            entities.ManufacBatchStartTime = DateTime.Now;
            entities.ManufacBatchEndTime = DateTime.Now;
            entities.PackBatchStartTime = DateTime.Now;
            entities.PackBatchEndTime = DateTime.Now;

            
           var  sfgList = (from c in _context.View_Product
                         select new { 
                         SFGCode = c.ProductCode,
                         SFGName = c.ProductName,
                         Business=c.Business
                         }).Where(c=>c.Business == "A").ToList();
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
            //staffList.Insert(0, new TblProductionManPower { StaffId = "", Name = "Select Staff" });
            ViewBag.ListofStaff = staffList;


            int isCodingDetailVisible = 0;
            isCodingDetailVisible = _context.TblVisibility.Where(x => x.ItemName == "Production Coding Detail").FirstOrDefault().Isvisible;
            ViewBag.IsCodingDetailVisible = isCodingDetailVisible;

            return View(entities);
        }

        [HttpPost, ActionName("CreateProduction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduction(TblProductionProcess prod)
        {
            try
            {
                //var jsonresult = GetRequisitionDetails(prod.RequisitionNo, 0);
                //var result = jsonresult.ToString();
                //var tblDetails = JsonConvert.DeserializeObject<List<TblProductionProcessDetail>>(result);
                
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
                    return View("Create",prod);
                }

                // Update tblFloorStock Table
                UpdateFloorStockFromProductionConsumption(prod.ProcessNo);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            //return View(student);
            return RedirectToAction(nameof(Index));
        }

        public JsonResult UpdateFloorStockFromProductionConsumption(string processNo)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();
            var processNoParam = new SqlParameter("@ProcessNo", processNo);

            var productList = _context.UpdateFloorScockFromForductionQC
                                .FromSql("EXEC sp_UpdateFloorStockConsumption @ProcessNo", processNoParam)
                                .ToList();

            return Json(productList, sa);
        }


        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                TblProductionProcess prod = _context.TblProductionProcess.Where(s => s.Id == id).First();

                _context.TblManufacturingLine.RemoveRange(_context.TblManufacturingLine.Where(d => d.ProductionId == prod.Id));
                _context.TblManufacturingMachine.RemoveRange(_context.TblManufacturingMachine.Where(d => d.ProductionId == prod.Id));
                _context.TblManufacturingShift.RemoveRange(_context.TblManufacturingShift.Where(d => d.ProductionId == prod.Id));
                _context.TblManufacturingManPower.RemoveRange(_context.TblManufacturingManPower.Where(d => d.ProductionId == prod.Id));

                _context.TblPackingDetailLine.RemoveRange(_context.TblPackingDetailLine.Where(d => d.ProductionId == prod.Id));
                _context.TblPackingDetailMachine.RemoveRange(_context.TblPackingDetailMachine.Where(d => d.ProductionId == prod.Id));
                _context.TblPackingDetailShift.RemoveRange(_context.TblPackingDetailShift.Where(d => d.ProductionId == prod.Id));

                _context.TblProductionProcessDetail.RemoveRange(_context.TblProductionProcessDetail.Where(d => d.ProcessNo == prod.ProcessNo));
                _context.TblProductionProcess.Remove(prod);
                _context.SaveChanges();
                return true;
                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return false;
                //return RedirectToAction(nameof(Index));
            }
        }


        public ActionResult Update(int id)
        {
            TblProductionProcess prodModel = new TblProductionProcess();
            var dt = _context.TblProductionProcess.Where(s => s.Id == id).First();
            prodModel.Id = dt.Id;
            prodModel.ProcessNo = dt.ProcessNo;
            prodModel.ProcessDate = dt.ProcessDate;
            prodModel.RequisitionNo = dt.RequisitionNo;
            prodModel.ProductCode = _context.TblProcessOrder.Where(s => s.ProcessOrderNo == dt.LinkedProcessNo).FirstOrDefault().ProductCode;
            prodModel.ProductName = _context.View_Product.Where(s => s.ProductCode == prodModel.ProductCode).FirstOrDefault().ProductName;
            prodModel.BatchNo = dt.BatchNo; //_context.TblRequisition.Where(s => s.RequisitionNo == dt.RequisitionNo).FirstOrDefault().BatchNo;
            prodModel.StandardOutput = _context.View_BOM.Where(s => s.ProductCode == prodModel.ProductCode).FirstOrDefault().StandardOutput;
            prodModel.BatchSize = _context.View_BOM.Where(s => s.ProductCode == prodModel.ProductCode).FirstOrDefault().BatchSize;
            prodModel.IssueNo = dt.IssueNo;
            prodModel.ProductionQty = dt.ProductionQty;
            prodModel.SFGCode = dt.SFGCode;
            prodModel.SFGProductionQty = dt.SFGProductionQty;
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
            prodModel.QCReferenceSampleQty = dt.QCReferenceSampleQty;
            prodModel.LumpQty = dt.LumpQty;
            prodModel.IsProcessCompleted=dt.IsProcessCompleted;

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

            prodModel.TblProductionProcessDetail = _context.TblProductionProcessDetail.Where(d => d.ProcessNo == dt.ProcessNo).ToList();


            var manufacturingShiftDetail = from pp_master in _context.TblProductionProcess
                                           from manu_shift in _context.TblManufacturingShift
                                           where (pp_master.Id == manu_shift.ProductionId)
                                           from br_cause in _context.TblManufacturingBreakDownCause
                                           where (manu_shift.BreakeDownCauseId == br_cause.BreakeDownCauseId)
                                           where pp_master.Id == id
                                           select new TblManufacturingShift
                                           {
                                               Id = manu_shift.Id,
                                               ProductionId = pp_master.Id,
                                               ShiftCode = manu_shift.ShiftCode,
                                               BreakeDownCauseId = manu_shift.BreakeDownCauseId,
                                               BreakeDownCauseName = br_cause.BreakeDownCause,
                                               BreakeDownTime = manu_shift.BreakeDownTime
                                           };
            prodModel.TblManufacturingShift = manufacturingShiftDetail.ToList();


            var manufacturingLineDetail = from pp_master in _context.TblProductionProcess
                                          from manu_line in _context.TblManufacturingLine
                                          where (pp_master.Id == manu_line.ProductionId)
                                          from line in _context.TblLineSetup
                                          where (manu_line.LineCode == line.LineCode)
                                          where pp_master.Id == id
                                          select new TblManufacturingLine
                                          {
                                              Id = manu_line.Id,
                                              ProductionId = pp_master.Id,
                                              LineCode = manu_line.LineCode,
                                              LineName = line.LineName
                                          };
            prodModel.TblManufacturingLine = manufacturingLineDetail.ToList();

            var manufacturingMachineDetail = from pp_master in _context.TblProductionProcess
                                             from manu_machine in _context.TblManufacturingMachine
                                             where (pp_master.Id == manu_machine.ProductionId)
                                             from machine in _context.TblMachineName
                                             where (manu_machine.MachineCode == machine.MachineCode)
                                             where pp_master.Id == id
                                             select new TblManufacturingMachine
                                             {
                                                 Id = manu_machine.Id,
                                                 ProductionId = pp_master.Id,
                                                 MachineCode = manu_machine.MachineCode,
                                                 MachineName = machine.MachineName,
                                                 MachineHour = manu_machine.MachineHour,
                                                 ManufacMachineNoOfWorker = manu_machine.ManufacMachineNoOfWorker,
                                                 ManufacMachineManHour = manu_machine.ManufacMachineManHour
                                             };
            prodModel.TblManufacturingMachine = manufacturingMachineDetail.ToList();

            var manufacturingManPowerDetail= from pp_master in _context.TblProductionProcess
                                             from manu_manpower in _context.TblManufacturingManPower
                                             where (pp_master.Id == manu_manpower.ProductionId)
                                             from manpower in _context.TblProductionManPower
                                             where (manu_manpower.StaffId == manpower.StaffId)
                                             where pp_master.Id == id
                                             select new TblManufacturingManPower
                                             {
                                                 Id = manu_manpower.Id,
                                                 ProductionId = pp_master.Id,
                                                 StaffId = manu_manpower.StaffId,
                                                 Name = manpower.Name,
                                                
                                             };
            prodModel.TblManufacturingManPower = manufacturingManPowerDetail.ToList();

            var packingShiftDetail = from pp_master in _context.TblProductionProcess
                                     from packing_shift in _context.TblPackingDetailShift
                                     where (pp_master.Id == packing_shift.ProductionId)
                                     from br_cause in _context.TblManufacturingBreakDownCause
                                     where (packing_shift.BreakeDownCauseId == br_cause.BreakeDownCauseId)
                                     where pp_master.Id == id
                                     select new TblPackingDetailShift
                                     {
                                         Id = packing_shift.Id,
                                         ProductionId = pp_master.Id,
                                         ShiftCode = packing_shift.ShiftCode,
                                         BreakeDownCauseId = packing_shift.BreakeDownCauseId,
                                         BreakeDownCauseName = br_cause.BreakeDownCause,
                                         BreakeDownTime = packing_shift.BreakeDownTime
                                     };
            prodModel.TblPackingDetailShift = packingShiftDetail.ToList();


            var packingLineDetail = from pp_master in _context.TblProductionProcess
                                    from packing_line in _context.TblPackingDetailLine
                                    where (pp_master.Id == packing_line.ProductionId)
                                    from line in _context.TblLineSetup
                                    where (packing_line.LineCode == line.LineCode)
                                    where pp_master.Id == id
                                    select new TblPackingDetailLine
                                    {
                                        Id = packing_line.Id,
                                        ProductionId = pp_master.Id,
                                        LineCode = packing_line.LineCode,
                                        LineName = line.LineName
                                    };
            prodModel.TblPackingDetailLine = packingLineDetail.ToList();

            var packingMachineDetail = from pp_master in _context.TblProductionProcess
                                       from packing_machine in _context.TblPackingDetailMachine
                                       where (pp_master.Id == packing_machine.ProductionId)
                                       from machine in _context.TblMachineName
                                       where (packing_machine.MachineCode == machine.MachineCode)
                                       where pp_master.Id == id
                                       select new TblPackingDetailMachine
                                       {
                                           Id = packing_machine.Id,
                                           ProductionId = pp_master.Id,
                                           MachineCode = packing_machine.MachineCode,
                                           MachineName = machine.MachineName,
                                           MachineHour = packing_machine.MachineHour,
                                           PackMachineNoOfWorker = packing_machine.PackMachineNoOfWorker,
                                           PackMachineManHour = packing_machine.PackMachineManHour
                                       };
            prodModel.TblPackingDetailMachine = packingMachineDetail.ToList();

            var packingManPowerDetail = from pp_master in _context.TblProductionProcess
                                        from packing_manpower in _context.TblPackingManPower
                                        where (pp_master.Id == packing_manpower.ProductionId)
                                        from manpower in _context.TblProductionManPower
                                        where (packing_manpower.StaffId == manpower.StaffId)
                                        where pp_master.Id == id
                                        select new TblPackingManPower
                                        {
                                            Id = packing_manpower.Id,
                                            ProductionId = pp_master.Id,
                                            StaffId = packing_manpower.StaffId,
                                            Name = manpower.Name,

                                        };
            prodModel.TblPackingManPower = packingManPowerDetail.ToList();
            #region Code Details part

            var codingShiftDetail = from pp_master in _context.TblProductionProcess
                                    from coding_shift in _context.TblCodingDetailShift
                                    where (pp_master.Id == coding_shift.ProductionId)
                                    from br_cause in _context.TblManufacturingBreakDownCause
                                    where (coding_shift.BreakeDownCauseId == br_cause.BreakeDownCauseId)
                                    where pp_master.Id == id
                                    select new TblCodingDetailShift
                                    {
                                        Id = coding_shift.Id,
                                        ProductionId = pp_master.Id,
                                        ShiftCode = coding_shift.ShiftCode,
                                        BreakeDownCauseId = coding_shift.BreakeDownCauseId,
                                        BreakeDownCauseName = br_cause.BreakeDownCause,
                                        BreakeDownTime = coding_shift.BreakeDownTime
                                    };
            prodModel.TblCodingDetailShift = codingShiftDetail.ToList();


            var codinngLineDetail = from pp_master in _context.TblProductionProcess
                                    from coding_line in _context.TblCodingDetailLine
                                    where (pp_master.Id == coding_line.ProductionId)
                                    from line in _context.TblLineSetup
                                    where (coding_line.LineCode == line.LineCode)
                                    where pp_master.Id == id
                                    select new TblCodingDetailLine
                                    {
                                        Id = coding_line.Id,
                                        ProductionId = pp_master.Id,
                                        LineCode = coding_line.LineCode,
                                        LineName = line.LineName
                                    };
            prodModel.TblCodingDetailLine = codinngLineDetail.ToList();

            var codingMachineDetail = from pp_master in _context.TblProductionProcess
                                      from coding_machine in _context.TblCodingDetailMachine
                                      where (pp_master.Id == coding_machine.ProductionId)
                                      from machine in _context.TblMachineName
                                      where (coding_machine.MachineCode == machine.MachineCode)
                                      where pp_master.Id == id
                                      select new TblCodingDetailMachine
                                      {
                                          Id = coding_machine.Id,
                                          ProductionId = pp_master.Id,
                                          MachineCode = coding_machine.MachineCode,
                                          MachineName = machine.MachineName,
                                          MachineHour = coding_machine.MachineHour,
                                          CodeMachineNoOfWorker = coding_machine.CodeMachineNoOfWorker,
                                          CodeMachineManHour = coding_machine.CodeMachineManHour
                                      };
            prodModel.TblCodingDetailMachine = codingMachineDetail.ToList();
            #endregion

            var productionDetail = from prodDtl in _context.TblProductionProcessDetail
                                   from mat in _context.View_Material
                                   where (prodDtl.ProcessNo == dt.ProcessNo && prodDtl.MaterialCode == mat.MaterialCode)
                                   orderby prodDtl.MaterialCode
                                   select new TblProductionProcessDetail
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
                                       Wip=prodDtl.Wip,
                                   };


            prodModel.TblProductionProcessDetail = productionDetail.ToList();

            int isCodingDetailVisible = 0;
            isCodingDetailVisible = _context.TblVisibility.Where(x => x.ItemName == "Production Coding Detail").FirstOrDefault().Isvisible;
            ViewBag.IsCodingDetailVisible = isCodingDetailVisible;

            return View(prodModel);
        }

        [HttpPost, ActionName("UpdateProduction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduction(int id, TblProductionProcess prod)
        {
            if (id != prod.Id)
            {
                return NotFound();
            }
            try
            {
                //eFTestContext.Update(emp);
                //await eFTestContext.SaveChangesAsync();

                var productionToUpdate = await _context.TblProductionProcess.FirstOrDefaultAsync(s => s.Id == id);
                productionToUpdate.Euser = User.Identity.Name;
                productionToUpdate.Edate = DateTime.Now;
                if (await TryUpdateModelAsync<TblProductionProcess>(
                    productionToUpdate,
                    "",
                    s => s.ProcessNo, s => s.ProcessDate, s => s.RequisitionNo, s => s.ProductCode, s=>s.LinkedProcessNo,s => s.BusinessCode, s => s.IssueNo, s => s.ProductionQty,
                    s => s.SFGCode, s => s.SFGProductionQty, s => s.BatchNo,
                    s => s.ManufacBatchStartTime, s => s.ManufacBatchEndTime, s => s.ManufacShiftCode, s => s.ManufacShiftBreakDownChangeTime, s => s.ManufacLineCode,
                    s => s.ManufacMachineCode, s => s.ManufacMachineCapacity, s => s.ManufacMachineHour, s => s.ManufacManHour, s => s.ManufacNoOfWorker,
                    s=> s.ManufacCommonManHour, s=> s.ManufacCommonNoOfWorker,
                    s => s.PackBatchStartTime, s => s.PackBatchEndTime, s => s.PackShiftCode, s => s.PackShiftBreakDownChangeTime, s => s.PackLineCode,
                    s => s.PackMachineCode, s => s.PackMachineCapacity, s => s.PackMachineHour, s => s.PackManHour, s=>s.PackCommonManHour, s => s.PackCommonNoOfWorker,
                    s => s.PackNoOfWorker, s => s.Comments,
                    s => s.Euser, s => s.Edate, s =>s.NumberOfBatch, s => s.QCReferenceSampleQty))



                    _context.TblManufacturingShift.RemoveRange(_context.TblManufacturingShift.Where(d => d.ProductionId == id));

                if (prod.TblManufacturingShift != null)
                {
                    foreach (var item in prod.TblManufacturingShift.ToList())
                    {
                        TblManufacturingShift prodDetail = new TblManufacturingShift();
                        prodDetail.ProductionId = prod.Id;
                        prodDetail.ShiftCode = item.ShiftCode;
                        prodDetail.BreakeDownCauseId = item.BreakeDownCauseId;
                        prodDetail.BreakeDownTime = item.BreakeDownTime;
                        await _context.AddAsync(prodDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.TblManufacturingLine.RemoveRange(_context.TblManufacturingLine.Where(d => d.ProductionId == id));

                if (prod.TblManufacturingLine != null)
                {
                    foreach (var item in prod.TblManufacturingLine.ToList())
                    {
                        TblManufacturingLine prodDetail = new TblManufacturingLine();
                        prodDetail.ProductionId = prod.Id;
                        prodDetail.LineCode = item.LineCode;
                        await _context.AddAsync(prodDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.TblManufacturingMachine.RemoveRange(_context.TblManufacturingMachine.Where(d => d.ProductionId == id));

                if (prod.TblManufacturingMachine != null)
                {
                    foreach (var item in prod.TblManufacturingMachine.ToList())
                    {
                        TblManufacturingMachine prodDetail = new TblManufacturingMachine();
                        prodDetail.ProductionId = prod.Id;
                        prodDetail.MachineCode = item.MachineCode;
                        prodDetail.MachineHour = item.MachineHour;
                        await _context.AddAsync(prodDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.TblPackingDetailShift.RemoveRange(_context.TblPackingDetailShift.Where(d => d.ProductionId == id));

                if (prod.TblPackingDetailShift != null)
                {
                    foreach (var item in prod.TblPackingDetailShift.ToList())
                    {
                        TblPackingDetailShift prodDetail = new TblPackingDetailShift();
                        prodDetail.ProductionId = prod.Id;
                        prodDetail.ShiftCode = item.ShiftCode;
                        prodDetail.BreakeDownCauseId = item.BreakeDownCauseId;
                        prodDetail.BreakeDownTime = item.BreakeDownTime;
                        await _context.AddAsync(prodDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.TblPackingDetailLine.RemoveRange(_context.TblPackingDetailLine.Where(d => d.ProductionId == id));

                if (prod.TblPackingDetailLine != null)
                {
                    foreach (var item in prod.TblPackingDetailLine.ToList())
                    {
                        TblPackingDetailLine prodDetail = new TblPackingDetailLine();
                        prodDetail.ProductionId = prod.Id;
                        prodDetail.LineCode = item.LineCode;
                        await _context.AddAsync(prodDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.TblPackingDetailMachine.RemoveRange(_context.TblPackingDetailMachine.Where(d => d.ProductionId == id));

                if (prod.TblPackingDetailMachine != null)
                {
                    foreach (var item in prod.TblPackingDetailMachine.ToList())
                    {
                        TblPackingDetailMachine prodDetail = new TblPackingDetailMachine();
                        prodDetail.ProductionId = prod.Id;
                        prodDetail.MachineCode = item.MachineCode;
                        prodDetail.MachineHour = item.MachineHour;
                        await _context.AddAsync(prodDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.TblProductionProcessDetail.RemoveRange(_context.TblProductionProcessDetail.Where(d => d.ProcessNo == prod.ProcessNo));

                if (prod.TblProductionProcessDetail != null)
                {
                    foreach (var item in prod.TblProductionProcessDetail.ToList())
                    {
                        TblProductionProcessDetail prodDetail = new TblProductionProcessDetail();
                        prodDetail.ProcessNo = prod.ProcessNo;
                        prodDetail.MaterialCode = item.MaterialCode;
                        prodDetail.ReqQuantity = item.ReqQuantity;
                        prodDetail.IssuedQty = item.IssuedQty;
                        prodDetail.PreviousUsedQty = item.PreviousUsedQty;
                        prodDetail.StdConsumptionQty = item.StdConsumptionQty;
                        prodDetail.CurrentUseQty = item.CurrentUseQty;
                        prodDetail.ProcessLoss = item.ProcessLoss;
                        prodDetail.Wastage = item.Wastage;
                        prodDetail.TotalConsumption = item.TotalConsumption;

                        await _context.AddAsync(prodDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            //}
            return View(prod);
        }


        public IActionResult SearchRequisition(string searchKey)
        {

            var requisitionNoParam = new SqlParameter("@requisitionNo", searchKey == null?"": searchKey);
            var model = _context.GetSearchRequisitionList
                               .FromSql("EXEC sp_SearchRequisition @requisitionNo", requisitionNoParam)
                               .ToList();


            return PartialView("_RequisitionPartial", model);
        }

        public IActionResult SearchProcessOrder(string searchKey, bool jsonRequest = false)
        {
            var processOrderNoParam = new SqlParameter("@processOrderNo", searchKey == null ? "" : searchKey);
            var model = _context.GetSearchProcessOrderList
                               .FromSql("EXEC sp_SearchProcessOrder @processOrderNo", processOrderNoParam)
                               .ToList();

            if (jsonRequest)
            {
                var sa = new JsonSerializerSettings();
                return Json(model,sa);
            }
            else {
                return PartialView("_RequisitionPartial", model);
            }
      


           
        }

        [HttpPost]
        public JsonResult SetRequisition(string requisitionNo)
        {
            var keyVal = _context.TblRequisition.Where(c => c.RequisitionNo == requisitionNo).ToList();
            if (keyVal != null)
            {
                requisitionNo = keyVal.FirstOrDefault().RequisitionNo;
            }

            if (requisitionNo.Length >= 0)
            {
                var sa = new JsonSerializerSettings();
                var requisitionInfo = from r in _context.TblRequisition
                                      from p in _context.View_Product
                                      where (r.RequisitionNo == requisitionNo && r.ProductCode == p.ProductCode)
                                      select new TblProductionProcess
                                      {
                                          RequisitionNo = r.RequisitionNo,
                                          BatchNo = r.BatchNo,
                                          ProductCode = r.ProductCode,
                                          ProductName = p.ProductName,
                                          StandardOutput = _context.View_BOM.Where(s => s.ProductCode == p.ProductCode).FirstOrDefault().StandardOutput,
                                          BatchSize = _context.View_BOM.Where(s => s.ProductCode == p.ProductCode).FirstOrDefault().BatchSize,
                                          NumberOfBatch = r.NumberOfBatch,
                                          ProductionQty = 0,
                                          PreviousProcessedBatchNo = _context.TblProductionProcess.Where(x=>x.RequisitionNo == requisitionNo).Sum(x=>x.NumberOfBatch),
                                          NoOfBatchInRequisition = r.NumberOfBatch,
                                          PreviousProcessedProductionQty = _context.TblProductionProcess.Where(x => x.RequisitionNo == requisitionNo).Sum(x => x.ProductionQty),
                                          LinkedProcessNo=r.LinkedProcessNo,
                                          
                                      };

                
                return Json(requisitionInfo, sa);
            }
            else
            {
                return Json("");
            }
        }


        [HttpPost]
        public JsonResult SetProcessOrder(string processOrderNo)
        {
            var keyVal = _context.TblProcessOrder.Any(c => c.ProcessOrderNo == processOrderNo);

            if (!keyVal)
            {
                return Json("") ;
            }


            else
            {
                var sa = new JsonSerializerSettings();
                try
                {
                    var processOrderInfo = from r in _context.TblProcessOrder
                                          from p in _context.View_Product
                                          where (r.ProcessOrderNo == processOrderNo && r.ProductCode == p.ProductCode)
                                          select new TblProductionProcess
                                          {
                                              LinkedProcessNo = r.ProcessOrderNo,
                                              RequisitionNo="",
                                              BatchNo = r.BatchNo,
                                              ProductCode = r.ProductCode,
                                              ProductName = p.ProductName,
                                              StandardOutput = _context.View_BOM.Where(s => s.ProductCode == p.ProductCode).FirstOrDefault().StandardOutput,
                                              BatchSize = _context.View_BOM.Where(s => s.ProductCode == p.ProductCode).FirstOrDefault().BatchSize,
                                              NumberOfBatch = r.NumberOfBatch,
                                              ProductionQty = 0,
                                              PreviousProcessedBatchNo = _context.TblProductionProcess.Where(x => x.LinkedProcessNo == processOrderNo).Sum(x => x.NumberOfBatch),
                                              NoOfBatchInRequisition = r.NumberOfBatch,
                                              PreviousProcessedProductionQty = _context.TblProductionProcess.Where(x => x.LinkedProcessNo == processOrderNo).Sum(x => x.ProductionQty),

                                          };
                    return Json(processOrderInfo, sa);
                }
                catch (Exception ex)
                {

                    return Json("");
                }
               


               
            }
            //else
            //{
            //    return Json("");
            //}
        }


        [HttpPost]       

        public JsonResult GetRequisitionDetails(string requisitionNo, decimal productionQty)
        {
            try
            {
                var sa = new JsonSerializerSettings();
                var requisitionNoParam = new SqlParameter("@requisitionNo", requisitionNo);
                var productionQtyParam = new SqlParameter("@productionQty", productionQty);
                var productList = _context.GetTblProductionProcessDetail
                                    .FromSql("EXEC sp_GetRequisitionDetail @requisitionNo, @productionQty", requisitionNoParam, productionQtyParam)
                                    .ToList();
                return Json(productList, sa);
            }
            catch (Exception ex)
            {

                return Json("");
            }
        

           
        }

        public JsonResult GetProcessOrderDetails(string processOrderNo, decimal productionQty)
        {
            try
            {
                var sa = new JsonSerializerSettings();
                var processOrderNoParam = new SqlParameter("@processOrderNo", processOrderNo);
                var productionQtyParam = new SqlParameter("@productionQty", productionQty);
                var productList = _context.GetTblProductionProcessDetail
                                    .FromSql("EXEC sp_GetProcessOrderDetail @processOrderNo, @productionQty", processOrderNoParam, productionQtyParam)
                                    .ToList();
                return Json(productList, sa);
            }
            catch (Exception ex)
            {

                return Json("");
            }



        }


        [HttpPost]
        public JsonResult SearchRequisitionByKey(string searchKey)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();


            var processOrderNoParam = new SqlParameter("@requisitionNo", searchKey == null ? "" : searchKey);
            var model = _context.GetSearchProcessOrderList
                               .FromSql("EXEC sp_SearchProcessOrder @processOrderNo", processOrderNoParam)
                               .ToList();


            return Json(model, sa);
        }

        /* Done in SearchProcessOrder */

        //public JsonResult SearchProcessOrderByKey(string searchKey)
        //{
        //    var errorViewModel = new ErrorViewModel();
        //    var sa = new JsonSerializerSettings();

        //    var processOrderNoParam = new SqlParameter("@requisitionNo", searchKey == null ? "" : searchKey);
        //    var model = _context.GetSearchProcessOrderList
        //                       .FromSql("EXEC sp_SearchProcessOrder @processOrderNo", processOrderNoParam)
        //                       .ToList();

        //    return Json(model, sa);
        //}



        public JsonResult GetMachineCapacity(string machineCode, string processOrderNo)
        {
            if (machineCode.Length > 0)
            {
                var sa = new JsonSerializerSettings();
                var productCode = _context.TblProcessOrder.Where(x => x.ProcessOrderNo == processOrderNo).FirstOrDefault().ProductCode;
                var capacity = from c in _context.TblMachineSetup.Where(s => s.MachineCode == machineCode && s.ProductCode == productCode) select c.Capacity;
                return Json(capacity, sa);
            }
            else
            {
                return Json("");
            }
        }

        public JsonResult GetMachineCapacityPacking(string machineCode, string processOrderNo)
        {
            if (machineCode.Length > 0)
            {
                var sa = new JsonSerializerSettings();
                var productCode = _context.TblRebProcessOrder.Where(x => x.ProcessOrderNo == processOrderNo).FirstOrDefault().ProductCode;
                var capacity = from c in _context.TblMachineSetup.Where(s => s.MachineCode == machineCode && s.ProductCode == productCode) select c.CapacityPacking;
                return Json(capacity, sa);
            }
            else
            {
                return Json("");
            }
        }

        public JsonResult GetFGProductWiseMachineList(string processOrderNo)
        {
            var productCode = _context.TblProcessOrder.Where(x => x.ProcessOrderNo == processOrderNo).FirstOrDefault().ProductCode;
            var model = new List<TblMachineName>(from ms in _context.TblMachineSetup
                                                       from m in _context.TblMachineName
                                                       where (ms.MachineCode == m.MachineCode && (ms.ProductCode == productCode))
                                                       select new TblMachineName
                                                       {
                                                           MachineCode = ms.MachineCode,
                                                           MachineName = m.MachineName
                                                       });

            //var requisitionNoParam = new SqlParameter("@requisitionNo", RequisitionNo == null ? "" : RequisitionNo);
            //var model = _context.GetRequisitionWiseProcessList
            //                   .FromSql("EXEC sp_GetRequisitionWiseProcessNo @requisitionNo", requisitionNoParam)
            //                   .ToList();

            var sa = new JsonSerializerSettings();
            return Json(model, sa);
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

        public JsonResult GetRequsitionList(string MaterialCode)
        {


            var sa = new JsonSerializerSettings();

           var materialCodeParam = new SqlParameter("@MaterialCode", MaterialCode);

            var model = _context.GetSearchRequisitionList
                               .FromSql("EXEC sp_SearchRequisitionByMaterialCode @MaterialCode", materialCodeParam)
                               .ToList();
            //model.Insert(0, new GetSearchRequisitionList { RequisitionNo="", LinkedProcessNo="",ProductCode="", ProductName=""});
            return Json(model, sa);
        }

        [HttpGet]
        public JsonResult GetGRNList(string ProcessOrderNo, string MaterialCode)
        {


            var sa = new JsonSerializerSettings();

            var materialCodeParam = new SqlParameter("@MaterialCode", MaterialCode);


            var model = _context.GetGRNList
                       .FromSql("EXEC sp_GetGRNList @MaterialCode", materialCodeParam)
                       .ToList();


            model.Insert(0, new GetGRNList { Grnno = "SELECT" });
            //var model = (from c in _context.TblFloorStock
            //             from r in _context.TblRequisition

            //             where c.MaterialCode == MaterialCode && c.RequisitionNo == r.RequisitionNo && c.IsBooked == false && r.LinkedProcessNo==ProcessOrderNo
            //             select new TblFloorStock {
            //                 Grnno = c.Grnno,
            //                 AvailableStock = c.AvailableStock,
            //                 IssueStock = c.IssueStock,
            //                 StockDate = c.StockDate,
            //                 ConsumeStock = c.ConsumeStock,

            //             }
            //            ).ToList();

            return Json(model, sa);
        }
        public string GenerateProcessNo() {
            //Generate Process No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var processNo = "PRO-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblProductionProcess.Where(c => c.ProcessNo.Substring(0, 8) == processNo).OrderByDescending(t => t.Id) select c.ProcessNo.Substring(8, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            processNo = "PRO-" + yy + mn + maxId;

            //Generate Process No.---------End
            return processNo;
        }

        public Boolean DoesProcessNoExists(string vProcessNo)
        {

            return _context.TblProductionProcess.Any(e => e.ProcessNo == vProcessNo);

        }

        private List<FloorStockCheckViewModel> FloorStockCheck(TblProductionProcess prod) {
            //Getting updated FloorStock 
            var floorStockList= new List<FloorStockCheckViewModel>();
            var requsitionNo = prod.RequisitionNo;
            var productionQty = prod.ProductionQty;
            //var sa = new JsonSerializerSettings();
            var requisitionNoParam = new SqlParameter("@requisitionNo", requsitionNo);
            var productionQtyParam = new SqlParameter("@productionQty", productionQty);
            var productList = _context.GetTblProductionProcessDetail
                                .FromSql("EXEC sp_GetRequisitionDetail @requisitionNo, @productionQty", requisitionNoParam, productionQtyParam)
                                .ToList();
            var userProductList = prod.TblProductionProcessDetail.ToList();

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

                        floorStockItem.MaterialName=item.MaterialName;
                        floorStockItem.MaterialCode=item.MaterialCode;
                        floorStockItem.FloorStockQty = item.FloorStock;
                        floorStockList.Add(floorStockItem);

                    }

                }
            }

            return floorStockList;
        }


        public JsonResult GetAvailableFloorStock(string vMaterialCode, string vGrnno)
        {
            var sa = new JsonSerializerSettings();
            var model = (from c in _context.TblFloorStock
                        where  c.MaterialCode == vMaterialCode && c.Grnno == vGrnno
                        group c by c.Grnno into grouped
                        select new TblFloorStock
                        {
                            Grnno = grouped.Key,
                            AvailableStock = grouped.Sum(c=> c.AvailableStock),

                        }).ToList();
            return Json(model, sa);
        }
    }

    }

        //public JsonResult GetPackMachineCapacity(string machineCode)
        //{
        //    if (machineCode.Length > 0)
        //    {
        //        var sa = new JsonSerializerSettings();
        //        var capacity = from c in npsContext.TblMachineSetup.Where(s => s.MachineCode == machineCode) select c.Capacity;
        //        return Json(capacity, sa);
        //    }
        //    else
        //    {
        //        return Json("");
        //    }
        //}
    

