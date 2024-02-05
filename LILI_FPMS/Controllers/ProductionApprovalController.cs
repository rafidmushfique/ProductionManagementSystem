using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LILI_FPMS.Models;
using LILI_FPMS;
using LILI_FPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace LILI_FPMS.Controllers
{
    public class ProductionApprovalController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public ProductionApprovalController(dbFormulationProductionSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ProcessNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "processNo_desc" : "";
            ViewData["ProcessDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "processDate_desc" : "";
            ViewData["ProcessNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "processNo_desc" : "";
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
                        from r in _context.TblProcessOrder
                        from p in _context.View_Product
                        where s.LinkedProcessNo == r.ProcessOrderNo && r.ProductCode == p.ProductCode
                        select new TblProductionProcess
                        {
                            Id = s.Id,
                            ProcessNo = s.ProcessNo,
                            ProcessDate = s.ProcessDate,
                            BatchNo = s.BatchNo,
                            RequisitionNo = s.RequisitionNo,
                            ProductCode = r.ProductCode,
                            ProductName = p.ProductName,
                            ProductionQty = s.ProductionQty,
                            Comments = s.Comments,
                            LinkedProcessNo = r.ProcessOrderNo,
                            ApprovalStatus = _context.TblProductionApprovalStatus.Where(x => x.ProcessNo == s.ProcessNo).OrderByDescending(x => x.Id).FirstOrDefault().ApprovalStatus
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                pross = pross.Where(s => s.ProcessNo.Contains(searchString)
                                    || s.ProcessNo.Contains(searchString)
                                    || s.ProductCode.Contains(searchString)
                                    || s.ProductName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "processNo_desc":
                    pross = pross.OrderByDescending(s => s.ProcessNo);
                    break;
                case "processDate_desc":
                    pross = pross.OrderByDescending(s => s.ProcessDate);
                    break;
                case "RequisitionNo_desc":
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

        public ActionResult Update(int id)
        {
            TblProductionProcess prodModel = new TblProductionProcess();
            var dt = _context.TblProductionProcess.Where(s => s.Id == id).First();
            prodModel.Id = dt.Id;
            prodModel.ProcessNo = dt.ProcessNo;
            prodModel.LinkedProcessNo = dt.LinkedProcessNo;
            prodModel.ProcessDate = dt.ProcessDate;
            prodModel.ProcessNo = dt.ProcessNo;
            prodModel.ProductCode = _context.TblProcessOrder.Where(s => s.ProcessOrderNo == dt.LinkedProcessNo).FirstOrDefault().ProductCode;
            prodModel.ProductName = _context.View_Product.Where(s => s.ProductCode == prodModel.ProductCode).FirstOrDefault().ProductName;
            prodModel.BatchNo = dt.BatchNo; //_context.TblRequisition.Where(s => s.ProcessNo == dt.ProcessNo).FirstOrDefault().BatchNo;
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

            var manufacturingManPowerDetail = from pp_master in _context.TblProductionProcess
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
                                       ProcessNo = prodDtl.ProcessNo,
                                       Grnno = prodDtl.Grnno,
                                       RejectQty = prodDtl.RejectQty,
                                       Wip = prodDtl.Wip,
                                   };


            prodModel.TblProductionProcessDetail = productionDetail.ToList();

            int isCodingDetailVisible = 0;
            isCodingDetailVisible = _context.TblVisibility.Where(x => x.ItemName == "Production Coding Detail").FirstOrDefault().Isvisible;
            ViewBag.IsCodingDetailVisible = isCodingDetailVisible;

            return View(prodModel);
        }
        public IActionResult ProductionApprovalStatus(string processNo, string approvalStatus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TblProductionApprovalStatus ras = new TblProductionApprovalStatus();
                    ras.ProcessNo = processNo;
                    ras.ProcessStatusDate = DateTime.Now;
                    ras.ApprovalStatus = approvalStatus;
                    ras.Approver = User.Identity.Name;


                    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    var config = builder.Build();
                    string constring = config.GetConnectionString("DefaultConnection");

                    SqlConnection con = new SqlConnection(constring);

                    string queryDel = "DELETE TblProductionApprovalStatus WHERE ProcessNo =" + "'" + ras.ProcessNo + "'";
                    SqlCommand cmdDel = new SqlCommand(queryDel, con);
                    con.Open();
                    int d = cmdDel.ExecuteNonQuery();
                    con.Close();

                    string query = "INSERT INTO TblProductionApprovalStatus(ProcessNo, ProcessStatusDate, ApprovalStatus, Approver) " +
                                    "values ('" + ras.ProcessNo + "','" + ras.ProcessStatusDate + "','" + ras.ApprovalStatus + "','" + ras.Approver + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();

                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                   
                }
       
            }
            catch (DbUpdateException ex)
            {
                
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
          
            return Ok();

        }

    }
}
