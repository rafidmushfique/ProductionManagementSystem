using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using LILI_FPMS.Models;
using LILI_FPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Ocsp;
using static NPOI.HSSF.Util.HSSFColor;

namespace LILI_FPMS.Controllers
{
   
   [Authorize]
    public class RebProcessOrderController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;
        public RebProcessOrderController(dbFormulationProductionSystemContext context)
        {
            _context=context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
           // TblProcessOrder req= new TblProcessOrder();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "requisition_desc" : "";
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

             var req = from s in _context.TblRebProcessOrder
                      from p in _context.View_Product
                      where s.ProductCode == p.ProductCode
                      select new TblRebProcessOrder
                      {
                          Id = s.Id,
                          ProcessOrderNo = s.ProcessOrderNo,
                          ProductCode = s.ProductCode,
                          ProductName = p.ProductName,
                          BatchNo = s.BatchNo,
                          NumberOfBatch = s.NumberOfBatch,
                          ProcessOrderDate = s.ProcessOrderDate,
                          IssueStatus = s.IssueStatus,
                      };

            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(s => s.ProcessOrderNo.Contains(searchString)
                                    || s.ProductCode.Contains(searchString)
                                    || s.ProductName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "requisition_desc":
                    req = req.OrderByDescending(s => s.ProcessOrderNo);
                    break;
                case "productCode_desc":
                    req = req.OrderByDescending(s => s.ProductCode);
                    break;
                case "productName_desc":
                    req = req.OrderByDescending(s => s.ProductCode);
                    break;

                default:
                    req = req.OrderByDescending(s => s.ProcessOrderNo);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<TblRebProcessOrder>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await employees.AsNoTracking().ToListAsync());
        }

        public ActionResult Create()
        {

            var processOrderNo = GenerateProcessOrderNo();

            List<TblLineSetup> lineList = new List<TblLineSetup>();
            lineList = (from c in _context.TblLineSetup
                        select c).ToList();
            lineList.Insert(0, new TblLineSetup { LineCode = "", LineName = "Select Line" });
            ViewBag.ListofLine = lineList;

            List<TblRebStifgreceive> StiNoList = new List<TblRebStifgreceive>();
            StiNoList = (from c in _context.TblRebStifgreceive
                        select c).Distinct().ToList();
            //StiNoList.Insert(0, new TblRebStifgreceive { Stino = "", Stino = "Select Line" });
            ViewBag.ListofStino = StiNoList; ;

            List<TblMachineName> machineList = new List<TblMachineName>();
            machineList = (from c in _context.TblMachineName
                           select c).ToList();
            machineList.Insert(0, new TblMachineName { MachineCode = "", MachineName = "Select Machine" });
            ViewBag.ListofMachine = machineList;


            TblRebProcessOrder entities = new TblRebProcessOrder();
            entities.ProcessOrderNo = processOrderNo;
            entities.ProcessOrderDate = DateTime.Now;
            return View(entities);
        }

        public async Task<IActionResult> CreateProcessOrder(TblRebProcessOrder model)
        {
            try
            {
                if (ModelState.IsValid) { 
                    if (DoesPorcessOrderNoExists(model.ProcessOrderNo))
                    {
                        model.ProcessOrderNo = GenerateProcessOrderNo();
                    }

                    if (model.LineCode != null)
                    {
                        foreach (var item in model.LineCode)
                        {
                            TblRebProcessOrderLine rl = new TblRebProcessOrderLine();
                            rl.ProcessOrderNo = model.ProcessOrderNo;
                            rl.LineCode = item;
                            model.TblRebProcessOrderLine.Add(rl);
                        }
                    }
                    if (model.Stino != null)
                    {
                        foreach (var item in model.Stino)
                        {
                            TblRebProcessOrderSti sn = new TblRebProcessOrderSti();
                            sn.ProcessOrderNo = model.ProcessOrderNo;
                            sn.Stino = item;
                            model.TblRebProcessOrderSti.Add(sn);
                        }
                    }
                    model.Iuser = User.Identity.Name;
                        model.Idate = DateTime.Now;
                        model.IssueStatus = "Pending";
                        _context.Add(model);
                        await _context.SaveChangesAsync();
                    
                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    return View("Create",model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return RedirectToAction(nameof(Index));
            }
           
        }

        public ActionResult Update(int id)
        {

            List<TblLineSetup> lineList = new List<TblLineSetup>();
            lineList = (from c in _context.TblLineSetup
                        select c).ToList();
            lineList.Insert(0, new TblLineSetup { LineCode = "", LineName = "Select Line" });
            ViewBag.ListofLine = lineList;

            List<TblRebStifgreceive> StiNoList = new List<TblRebStifgreceive>();
            StiNoList = (from c in _context.TblRebStifgreceive
                         select c).Distinct().ToList();
            //StiNoList.Insert(0, new TblRebStifgreceive { Stino = "", Stino = "Select Line" });
            ViewBag.ListofStino = StiNoList; 

            List<TblMachineName> machineList = new List<TblMachineName>();
            machineList = (from c in _context.TblMachineName
                           select c).ToList();
            machineList.Insert(0, new TblMachineName { MachineCode = "", MachineName = "Select Machine" });
            ViewBag.ListofMachine = machineList;


            TblRebProcessOrder model= new TblRebProcessOrder();
            var dt =_context.TblRebProcessOrder.Where(s=>s.Id== id).First();
            model.Id = dt.Id;
            model.ProcessOrderNo = dt.ProcessOrderNo;
            model.ProcessOrderDate = dt.ProcessOrderDate;
            model.BatchNo = dt.BatchNo;
            model.NumberOfBatch = dt.NumberOfBatch;
            model.BatchSize = 0;
            model.StandardOutput = 0;
            model.ProductCode = dt.ProductCode;
            model.ProductName = _context.View_Product.Where(s => s.ProductCode == dt.ProductCode).FirstOrDefault().ProductName;
            model.Comments = dt.Comments;
            model.ExtOfProcessOrderNo = dt.ExtOfProcessOrderNo;
            model.TblRebProcessOrderDetail = _context.TblRebProcessOrderDetail.Where(d => d.ProcessOrderNo == dt.ProcessOrderNo).ToList();
            model.Bomid = dt.Bomid;
            model.ManPower=dt.ManPower;

            var rebProcessDetails= from dtl in _context.TblRebProcessOrderDetail
                                from mat in _context.View_Material
                                where (dtl.ProcessOrderNo == dt.ProcessOrderNo && dtl.MaterialCode == mat.MaterialCode)
                                orderby dtl.MaterialCode
                                select new TblRebProcessOrderDetail
                                {
                                    Id = dtl.Id,
                                    MaterialCode = dtl.MaterialCode,
                                    MaterialName = mat.MaterialName,
                                    Unit = mat.BaseUnit,
                                    StandardRecipeQty = dtl.StandardRecipeQty,
                                    FloorStock = dtl.FloorStock,
                                    RequiredQty = dtl.RequiredQty,
                                    AvailableStock = dtl.AvailableStock,
                                    EstimatedQty = dtl.EstimatedQty,
                                };

       

           // var selectedMachine = _context.TblRequisitionMachineSetup.Where(s => s.ProcessOrderNo == dt.ProcessOrderNo).Select(x => x.MachineCode).ToList();
            var selectedLine = _context.TblRebProcessOrderLine.Where(s => s.ProcessOrderNo == dt.ProcessOrderNo).Select(x => x.LineCode).ToList();
            var selectedStino = _context.TblRebProcessOrderSti.Where(s => s.ProcessOrderNo == dt.ProcessOrderNo).Select(x => x.Stino).ToList();

            //ViewBag.SelectedMachine = selectedMachine.ToList();
            ViewBag.SelectedLine = selectedLine.ToList();
            ViewBag.SelectedStino = selectedStino.ToList();


            model.TblRebProcessOrderDetail = rebProcessDetails.ToList();



            return View(model);
        }

        [HttpPost, ActionName("UpdateProcessOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProcessOrder(int id, TblRebProcessOrder model)
        {
            var checkIfExists = _context.TblRebProcessOrder.Any(s => s.Id == id);
            if (!checkIfExists)
            {
                return NotFound();
            }
            try
            {



                var processOrderToUpdate = await _context.TblRebProcessOrder.FirstOrDefaultAsync(s => s.Id == id);


                var rebProcessOrder = _context.TblRebProcessOrder.Where(s=>s.Id==id).Select(x=> x.ProcessOrderNo).FirstOrDefault();

                if (IsEditableDeleteable(rebProcessOrder) == true)
                {
                    processOrderToUpdate.Euser = User.Identity.Name;
                    processOrderToUpdate.Edate = DateTime.Now;
                    if (await TryUpdateModelAsync<TblRebProcessOrder>(
                        processOrderToUpdate,
                        "",
                        s => s.ProcessOrderNo, s => s.ProcessOrderDate, s => s.BatchNo, s => s.NumberOfBatch, s => s.ProductCode, s => s.Comments, s => s.Euser, s => s.Edate, s => s.ExtOfProcessOrderNo, s => s.ManPower))


                     _context.TblRequisitionLine.RemoveRange(_context.TblRequisitionLine.Where(d => d.ProcessOrderNo == model.ProcessOrderNo));
                    //_context.TblRequisitionMachineSetup.RemoveRange(_context.TblRequisitionMachineSetup.Where(d => d.ProcessOrderNo == model.ProcessOrderNo));
                    _context.TblRebProcessOrderDetail.RemoveRange(_context.TblRebProcessOrderDetail.Where(d => d.ProcessOrderNo == model.ProcessOrderNo));

                    if (model.TblRebProcessOrderDetail != null)
                    {
                        foreach (var item in model.TblRebProcessOrderDetail.ToList())
                        {
                            TblRebProcessOrderDetail proDetail = new TblRebProcessOrderDetail();
                            proDetail.ProcessOrderNo = model.ProcessOrderNo;
                            proDetail.MaterialCode = item.MaterialCode;
                            proDetail.StandardRecipeQty = item.StandardRecipeQty;
                            proDetail.FloorStock = item.FloorStock;
                            proDetail.RequiredQty = item.RequiredQty;
                            proDetail.AvailableStock = item.AvailableStock;

                            await _context.AddAsync(proDetail);
                        }
                        //await _context.SaveChangesAsync();
                    }

                    foreach (var item in model.LineCode)
                    {
                        TblRebProcessOrderLine rl = new TblRebProcessOrderLine();
                        rl.ProcessOrderNo = model.ProcessOrderNo;
                        rl.LineCode = item;
                        await _context.AddAsync(rl);
                    }

                    foreach (var item in model.Stino)
                    {
                        TblRebProcessOrderSti sn = new TblRebProcessOrderSti();
                        sn.ProcessOrderNo = model.ProcessOrderNo;
                        sn.Stino = item;
                        await _context.AddAsync(sn);
                    }

                    //foreach (var item in model.MachineCode)
                    //{
                    //    TblRequisitionMachineSetup machine = new TblRequisitionMachineSetup();
                    //    machine.ProcessOrderNo = model.ProcessOrderNo;
                    //    machine.MachineCode = item;
                    //    await _context.AddAsync(machine);
                    //}

                    await _context.SaveChangesAsync();
     
                }
                else
                {

                    TempData["AlertMessage"] = "Can't edit, May have data dependency !";

                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                return RedirectToAction(nameof(Index));
            }
            //}
           
        }
        [HttpPost]
        public bool Delete(int id)
        {
            try
            {

                TblRebProcessOrder rpo = _context.TblRebProcessOrder.Where(s => s.Id == id).First();
                var rebProcessOrderNo = _context.TblRebProcessOrder.Where(s=> s.ProcessOrderNo == rpo.ProcessOrderNo).Select(x=> x.ProcessOrderNo).FirstOrDefault();
                if (IsEditableDeleteable(rebProcessOrderNo) == true)
                {
                    _context.TblRebProcessOrderLine.RemoveRange(_context.TblRebProcessOrderLine.Where(d => d.ProcessOrderNo == rpo.ProcessOrderNo));
                    _context.TblRebProcessOrderSti.RemoveRange(_context.TblRebProcessOrderSti.Where(d => d.ProcessOrderNo == rpo.ProcessOrderNo));
                    _context.TblRebProcessOrderDetail.RemoveRange(_context.TblRebProcessOrderDetail.Where(d => d.ProcessOrderNo == rpo.ProcessOrderNo));
                    _context.TblRebProcessOrder.Remove(rpo);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return false;
                //return RedirectToAction(nameof(Index));
            }
        }

        #region modal Product Search
        public IActionResult SearchProduct()
        {
            //var model = new List<TblRequisition>(from c in npsContext.View_Product 
            //                                     select c);

            var model = new List<TblProcessOrder>(from b in _context.View_BOM
                                                 from p in _context.View_Product
                                                 where (b.ProductCode == p.ProductCode) && b.IsActive == "Y"
                                                 select new TblProcessOrder
                                                 {
                                                     ProductCode = b.ProductCode,
                                                     ProductName = p.ProductName,
                                                     StandardOutput = b.StandardOutput,
                                                     BatchSize = b.BatchSize
                                                 });

            return PartialView("_ProductPartial", model);
        }

        [HttpPost]
        public JsonResult SetProduct(string productId)
        {
            //var keyVal = _context.View_BOM.Where(c => c.ProductCode == productId).ToList();
            //if (keyVal != null)
            //{
            //    productId = keyVal.FirstOrDefault().ProductCode;
            //}

            if (productId.Length >= 0)
            {
                var sa = new JsonSerializerSettings();

                //var productInfo = from b in _context.View_BOM
                //                  from p in _context.View_Product
                //                  where (b.ProductCode == p.ProductCode && b.ProductCode == productId && b.IsActive == "Y")
                //                  select new TblRequisition
                //                  {
                //                      ProductCode = b.ProductCode,
                //                      ProductName = p.ProductName,
                //                      StandardOutput = b.StandardOutput,
                //                      BatchSize = b.BatchSize,
                //                      MonthlyPlannedQty = MonthlyPlannedQTY(productId)
                //                  };

                var productNoParam = new SqlParameter("@productId", productId);

                var productInfo = _context.VMSetProductInfo
                                    .FromSql("EXEC sp_SetRebProductInfoWithPlanning @productId", productNoParam)
                                    .ToList();

                return Json(productInfo, sa);
            }
            else
            {
                return Json("");
            }
        }

        public decimal MonthlyPlannedQTY(string productCode)
        {
            var monthlyPlanningQty = from mp in _context.TblMonthlyPlanning
                                     from mpd in _context.TblMonthlyPlanningDetail
                                     where (mp.PlanningNo == mpd.PlanningNo && mpd.Fgcode == productCode
                                               && mp.Year == DateTime.Now.Year && mp.Month == DateTime.Now.ToString("MMMM"))
                                     select new
                                     {
                                         revisedPlanQty = (mp == null || mpd == null) ? 0 : mpd.RevisedPlanQty
                                     };
            decimal revisedPlanQty = 0;
            if (monthlyPlanningQty.Count() > 0)
            {
                revisedPlanQty = monthlyPlanningQty.FirstOrDefault().revisedPlanQty;
            }
            return revisedPlanQty;
        }

        [HttpPost]
        public JsonResult SearchProductByKey(string searchKey)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();
            //var productList = 
            //                  from p in _context.View_Product
            //                  where (p.ProductCode.ToUpper().Contains(searchKey.ToUpper()) || (p.ProductName.ToUpper().Contains(searchKey.ToUpper())))
            //                  select new TblRequisition
            //                  {
            //                      ProductCode = p.ProductCode,
            //                      ProductName = p.ProductName,
            //                      StandardOutput = 0,
            //                      BatchSize = 0
            //                  };
            string searchKeyValue = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var searchKeyParam = new SqlParameter("@searchKey", searchKeyValue);
            var productList = _context.GetRebSearchProduct.FromSql("Exec sp_RebSearchProduct @searchKey", searchKeyParam).ToList();

            return Json(productList, sa);
        }

        [HttpPost]
        public JsonResult GetBOMDetail(string productId, decimal noOfBatch)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();
            //var productList = from b in _context.View_BOM
            //                  from p in _context.View_Product
            //                  from bd in _context.View_BOMDetail
            //                  from m in _context.View_Material                              
            //                  where (b.ProductCode == p.ProductCode && b.Id == bd.Bomid && bd.MaterialCode == m.MaterialCode)
            //                  && (b.ProductCode == productId) && b.IsActive=="Y"
            //                  orderby bd.MaterialCode
            //                  select new TblRequisitionDetail
            //                  {
            //                      MaterialCode = bd.MaterialCode,
            //                      MaterialName = m.MaterialName,
            //                      Unit = m.BaseUnit,
            //                      StandardRecipeQty = bd.QuantityPerBatch * noOfBatch,
            //                      FloorStock = 0,
            //                      RequiredQty = bd.QuantityPerBatch * noOfBatch,
            //                      AvailableStock = 0
            //                  };

            var requisitionNoParam = new SqlParameter("@productId", productId);
            var productionQtyParam = new SqlParameter("@noOfBatch", noOfBatch);

            var productList = _context.GetProductWiseBOMDetail
                                .FromSql("EXEC sp_GetProductWiseBOMDetail @productId, @noOfBatch", requisitionNoParam, productionQtyParam)
                                .ToList();

            return Json(productList, sa);
        }

        public IActionResult AddMaterialSearch()
        {
            Models.TblRequisitionDetail materialSearchModel = new Models.TblRequisitionDetail();
            return PartialView("_MaterialSearchPartial", materialSearchModel);
        }

        [HttpPost]
        public JsonResult SearchMaterial(string MaterialSearchKey)
        {
            var model = new List<TblRequisitionDetail>(from b in _context.View_Material
                                                       where ((b.MaterialCode.ToUpper().Contains(MaterialSearchKey.ToUpper()) || b.MaterialName.ToUpper().Contains(MaterialSearchKey.ToUpper())))
                                                       select new TblRequisitionDetail
                                                       {
                                                           MaterialCode = b.MaterialCode,
                                                           MaterialName = b.MaterialName,
                                                           Unit = b.BaseUnit,
                                                           RequiredQty = 0
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
        #endregion


        private bool IsEditableDeleteable(string reqNo)
        {
            if (string.IsNullOrEmpty(reqNo))
            {
                return true;
            }
            else {
                int countApp = 0;
                int countIssued = 0;
                countApp = _context.TblRequisitionApprovalStatus.Where(a => a.RequisitionNo == reqNo && a.ApprovalStatus == "Approved").Count();
                countIssued = _context.TblRequisition.Where(r => r.RequisitionNo == reqNo).Count();
                if ((countApp + countIssued) > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
         
        }

        private string GenerateProcessOrderNo() {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
       
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var processOrderNo = "RPON-" + yy + mn;


            String maxId = "";
            String maxNo = (from c in _context.TblRebProcessOrder.Where(c => c.ProcessOrderNo.Substring(0, 9) == processOrderNo).OrderByDescending(t => t.Id) select c.ProcessOrderNo.Substring(9, 3)).FirstOrDefault();
            
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            processOrderNo = "RPON-" + yy + mn + maxId;

            return processOrderNo;
        }
        private bool DoesPorcessOrderNoExists(string vProcessOrderNo)
        { 
         return _context.TblRebProcessOrder.Any(e => e.ProcessOrderNo == vProcessOrderNo);
        }
    }
}
