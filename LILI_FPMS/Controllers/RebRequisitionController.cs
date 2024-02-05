using LILI_FPMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LILI_FPMS.Controllers
{
    [Authorize]
    public class RebRequisitionController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public RebRequisitionController(dbFormulationProductionSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
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

            var req = from s in _context.TblRebRequisition
                      from p in _context.View_Product
                      where s.ProductCode == p.ProductCode
                      select new TblRebRequisition
                      {
                          Id = s.Id,
                          RequisitionNo = s.RequisitionNo,
                          ProductCode = s.ProductCode,
                          ProductName = p.ProductName,
                          BatchNo = s.BatchNo,
                          NumberOfBatch = s.NumberOfBatch,
                          RequisitionDate = s.RequisitionDate,
                          IssueStatus = s.IssueStatus,
                          LinkedProcessNo = s.LinkedProcessNo,
                      };

            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(s => s.RequisitionNo.Contains(searchString)
                                    || s.ProductCode.Contains(searchString)
                                    || s.ProductName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "requisition_desc":
                    req = req.OrderByDescending(s => s.RequisitionNo);
                    break;
                case "productCode_desc":
                    req = req.OrderByDescending(s => s.ProductCode);
                    break;
                case "productName_desc":
                    req = req.OrderByDescending(s => s.ProductCode);
                    break;

                default:
                    req = req.OrderByDescending(s => s.RequisitionNo);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<TblRebRequisition>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await employees.AsNoTracking().ToListAsync());
        }

        public ActionResult Create()
        {

            var requisitionNo = GenerateRequisitionNo();

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


            //List<TblRebProcessOrder> orderList = new List<TblRebProcessOrder>();
            var orderList = (from c in _context.TblRebProcessOrder
                             from p in _context.View_Product
                             where !_context.TblRebProductionProcess.Any(p => p.LinkedProcessNo == c.ProcessOrderNo)
                             && c.ProductCode == p.ProductCode
                             select new TblRebProcessOrder
                             {
                                 ProcessOrderNo = c.ProcessOrderNo,
                                 ProcessOrderDate = c.ProcessOrderDate,
                                 ProductName = p.ProductName,
                             }).ToList();

            orderList.Insert(0, new TblRebProcessOrder { ProcessOrderNo = "SELECT", ProcessOrderDate = DateTime.Now });

            DateTime currentDateTime = DateTime.Now;
            TimeSpan timeThreshold = TimeSpan.FromHours(72);
            //orderList = orderList.Where(x=> currentDateTime-x.ProcessOrderDate <= timeThreshold).OrderByDescending(x => x.ProcessOrderDate).ToList();
            orderList = orderList.OrderByDescending(x => x.ProcessOrderDate).ToList();
            ViewBag.ListofOrder = orderList;

            TblRebRequisition entities = new TblRebRequisition();
            entities.RequisitionNo = requisitionNo;
            entities.RequisitionDate = DateTime.Now;
            return View(entities);
        }

        [HttpPost, ActionName("CreateRequisition")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequisition(TblRebRequisition req)
        {


            try
            {
                var checkExists = _context.TblRebRequisition.Any(e => e.RequisitionNo == req.RequisitionNo);
                if (checkExists)
                {

                    req.RequisitionNo = GenerateRequisitionNo();
                }

                if (ModelState.IsValid)
                {

                    req.Iuser = User.Identity.Name;
                    req.Idate = DateTime.Now;
                    req.IssueStatus = "Pending";
                    _context.Add(req);
                    await _context.SaveChangesAsync();


                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    return View(req);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {

                TblRebRequisition req = _context.TblRebRequisition.Where(s => s.Id == id).First();
                if (IsEditableDeleteable(req.RequisitionNo) == true)
                {

                    _context.TblRebRequisitionDetail.RemoveRange(_context.TblRebRequisitionDetail.Where(d => d.RequisitionNo == req.RequisitionNo));
                    _context.TblRebRequisition.Remove(req);
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

        public ActionResult Update(int id)
        {

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

            Models.TblRebRequisition reqModel = new Models.TblRebRequisition();
            var dt = _context.TblRebRequisition.Where(s => s.Id == id).First();
            reqModel.Id = dt.Id;
            reqModel.RequisitionNo = dt.RequisitionNo;
            reqModel.RequisitionDate = dt.RequisitionDate;
            reqModel.BatchNo = dt.BatchNo;
            reqModel.BatchSize = _context.View_BOM.Where(s => s.ProductCode == dt.ProductCode).FirstOrDefault().BatchSize;
            reqModel.StandardOutput = _context.View_BOM.Where(s => s.ProductCode == dt.ProductCode).FirstOrDefault().StandardOutput;
            reqModel.ProductCode = dt.ProductCode;
            reqModel.ProductName = _context.View_Product.Where(s => s.ProductCode == dt.ProductCode).FirstOrDefault().ProductName;
            reqModel.Comments = dt.Comments;
            reqModel.ExtOfRequisitionNo = dt.ExtOfRequisitionNo;
            reqModel.TblRebRequisitionDetail = _context.TblRebRequisitionDetail.Where(d => d.RequisitionNo == dt.RequisitionNo).ToList();
            reqModel.LinkedProcessNo = dt.LinkedProcessNo;
            reqModel.TypeCode = dt.TypeCode;
            var poNoOfbatch = (_context.TblRebProcessOrder.Where(d => d.ProcessOrderNo == reqModel.LinkedProcessNo).Select(x => x.NumberOfBatch).FirstOrDefault());

            reqModel.ProcessOrderNumberOfBatch = poNoOfbatch;
            var compNoOfBatch = (_context.TblRebRequisition.Where(requisition => requisition.LinkedProcessNo == reqModel.LinkedProcessNo).Sum(requisition => requisition.NumberOfBatch));
            reqModel.CompletedNumberOfBatch = compNoOfBatch;
            reqModel.PendingNumberOfBatch = poNoOfbatch - compNoOfBatch;
            reqModel.NumberOfBatch = dt.NumberOfBatch;

            var requisitionDetail = from reqDtl in _context.TblRebRequisitionDetail
                                    from mat in _context.View_Material
                                    where (reqDtl.RequisitionNo == dt.RequisitionNo && reqDtl.MaterialCode == mat.MaterialCode)
                                    orderby reqDtl.MaterialCode
                                    select new TblRebRequisitionDetail
                                    {
                                        Id = reqDtl.Id,
                                        MaterialCode = reqDtl.MaterialCode,
                                        MaterialName = mat.MaterialName,
                                        Unit = mat.BaseUnit,
                                        StandardRecipeQty = reqDtl.StandardRecipeQty,
                                        FloorStock = reqDtl.FloorStock,
                                        RequiredQty = reqDtl.RequiredQty,
                                        AvailableStock = reqDtl.AvailableStock,
                                        EstimatedQty = reqDtl.EstimatedQty,
                                    };





            reqModel.TblRebRequisitionDetail = requisitionDetail.ToList();



            return View(reqModel);
        }


        public async Task<IActionResult> UpdateRequisition( TblRebRequisition req)
        {
           
            var id = req.Id;
            var result = _context.TblRebRequisition.Any(x => x.Id == id);
            if (!_context.TblRebRequisition.Any(x=> x.Id== id))
            {
                return NotFound();
            }
            try
            {
                //eFTestContext.Update(emp);
                //await eFTestContext.SaveChangesAsync();

                var requisitionToUpdate = await _context.TblRebRequisition.FirstOrDefaultAsync(s => s.Id == id);

                if (IsEditableDeleteable(req.RequisitionNo) == true)
                {
                    requisitionToUpdate.Euser = User.Identity.Name;
                    requisitionToUpdate.Edate = DateTime.Now;
                    requisitionToUpdate.Comments= req.Comments;
                    requisitionToUpdate.RequisitionDate = req.RequisitionDate;
                    requisitionToUpdate.TypeCode = req.TypeCode;

                    if (await TryUpdateModelAsync<TblRebRequisition>(
                        requisitionToUpdate,
                        "",
                        s => s.RequisitionDate, s => s.Comments, s => s.Euser, s => s.Edate, s => s.ExtOfRequisitionNo,s=>s.TypeCode)) ;

                        var ReqToDelete = _context.TblRebRequisitionDetail.Where(x=>x.RequisitionNo== req.RequisitionNo);
                        _context.TblRebRequisitionDetail.RemoveRange(ReqToDelete);



                    if (req.TblRebRequisitionDetail != null)
                    {
                        foreach (var item in req.TblRebRequisitionDetail.ToList())
                        {
                            TblRebRequisitionDetail reqDetail = new TblRebRequisitionDetail();
                            reqDetail.RequisitionNo = req.RequisitionNo;
                            reqDetail.MaterialCode = item.MaterialCode;
                            reqDetail.StandardRecipeQty = item.StandardRecipeQty;
                            reqDetail.FloorStock = item.FloorStock;
                            reqDetail.RequiredQty = item.RequiredQty;
                            reqDetail.AvailableStock = item.AvailableStock;

                            await _context.AddAsync(reqDetail);
                        }
                        //await _context.SaveChangesAsync();
                    }



                    await _context.SaveChangesAsync();
      
                }
                else
                {

                    TempData["AlertMessage"] = "Can't edit, May have data dependency !";

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            //}
            return View(req);
        }

        private bool IsEditableDeleteable(string reqNo)
        {
            int countApp = 0;
            int countIssued = 0;
            countApp = _context.TblRebRequisitionApprovalStatus.Where(a => a.RequisitionNo == reqNo && a.ApprovalStatus == "Approved").Count();
            countIssued = _context.TblRebRequisition.Where(r => r.RequisitionNo == reqNo && r.IssueStatus == "Issued").Count();
            if ((countApp + countIssued) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IActionResult SearchProduct()
        {


            var model = new List<TblRebRequisition>(from b in _context.View_BOM
                                                 from p in _context.View_Product
                                                 where (b.ProductCode == p.ProductCode) && b.IsActive == "Y"
                                                 select new TblRebRequisition
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
            var keyVal = _context.View_BOM.Where(c => c.ProductCode == productId).ToList();
            if (keyVal != null)
            {
                productId = keyVal.FirstOrDefault().ProductCode;
            }

            if (productId.Length >= 0)
            {
                var sa = new JsonSerializerSettings();


                var productNoParam = new SqlParameter("@productId", productId);

                var productInfo = _context.SetProductInfoWithPlanning
                                    .FromSql("EXEC sp_SetProductInfoWithPlanning @productId", productNoParam)
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
            var productList = from b in _context.View_BOM
                              from p in _context.View_Product
                              where (b.ProductCode == p.ProductCode) && b.IsActive == "Y"
                              && (b.ProductCode.ToUpper().Contains(searchKey.ToUpper()) || (p.ProductName.ToUpper().Contains(searchKey.ToUpper())))
                              select new TblRebRequisition
                              {
                                  ProductCode = b.ProductCode,
                                  ProductName = p.ProductName,
                                  StandardOutput = b.StandardOutput,
                                  BatchSize = b.BatchSize
                              };
            return Json(productList, sa);
        }

        [HttpPost]
        public JsonResult GetBOMDetail(string productId, decimal noOfBatch)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();


            var requisitionNoParam = new SqlParameter("@productId", productId);
            var productionQtyParam = new SqlParameter("@noOfBatch", noOfBatch);

            var productList = _context.GetProductWiseBOMDetail
                                .FromSql("EXEC sp_GetProductWiseBOMDetail @productId, @noOfBatch", requisitionNoParam, productionQtyParam)
                                .ToList();

            return Json(productList, sa);
        }

        public IActionResult AddMaterialSearch()
        {
            Models.TblRebRequisitionDetail materialSearchModel = new Models.TblRebRequisitionDetail();
            return PartialView("_MaterialSearchPartial", materialSearchModel);
        }

        [HttpPost]
        public JsonResult SearchMaterial(string MaterialSearchKey)
        {
            var model = new List<TblRebRequisitionDetail>(from b in _context.View_Material
                                                       where ((b.MaterialCode.ToUpper().Contains(MaterialSearchKey.ToUpper()) || b.MaterialName.ToUpper().Contains(MaterialSearchKey.ToUpper())))
                                                       select new TblRebRequisitionDetail
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

        public IActionResult SearchRequisition()
        {
            var model = new List<TblRebRequisition>(from r in _context.TblRebRequisition
                                                 from p in _context.View_Product
                                                 where (r.ProductCode == p.ProductCode && r.ExtOfRequisitionNo == null)
                                                 orderby r.RequisitionDate descending
                                                 select new TblRebRequisition
                                                 {
                                                     RequisitionNo = r.RequisitionNo,
                                                     RequisitionDate = r.RequisitionDate,
                                                     ProductCode = r.ProductCode,
                                                     ProductName = p.ProductName
                                                 });

            return PartialView("_RequisitionPartial", model);
        }

        public JsonResult SearchRequisitionByKey(string searchKey)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();

            var reqList = from r in _context.TblRebRequisition
                          from p in _context.View_Product
                          where (r.ProductCode == p.ProductCode && r.ExtOfRequisitionNo == null)
                          && (r.RequisitionNo.ToUpper().Contains(searchKey.ToUpper()) || r.ProductCode.ToUpper().Contains(searchKey.ToUpper()) || (p.ProductName.ToUpper().Contains(searchKey.ToUpper())))
                          orderby r.RequisitionDate descending
                          select new TblRebRequisition
                          {
                              RequisitionNo = r.RequisitionNo,
                              RequisitionDate = r.RequisitionDate,
                              ProductCode = r.ProductCode,
                              ProductName = p.ProductName
                          };

            return Json(reqList, sa);
        }

        [HttpPost]
        public JsonResult GetProcessOrderData(string vProcessOrderNo)
        {
            var sa = new JsonSerializerSettings();

            var productId = _context.TblRebProcessOrder.Where(x => x.ProcessOrderNo == vProcessOrderNo).Select(i => i.ProductCode).FirstOrDefault().ToString();

            //var keyVal = _context.View_BOM.Where(c => c.ProductCode == productId).ToList();
            //if (keyVal != null)
            //{
            //    productId = keyVal.FirstOrDefault().ProductCode;
            //}

            if (productId.Length >= 0)
            {




                var processOrderNo = new SqlParameter("@processOrderNo", vProcessOrderNo);
                var productNoParam = new SqlParameter("@productId", productId);

                var productInfo =    _context.SetProductInfo
                                    .FromSql("EXEC sp_GetRefurbishProductInfo @processOrderNo,@productId", processOrderNo, productNoParam) 
                                    . Select (c=> new SetProductInfo
                                    { 
                                        ProductCode = c.ProductCode,
                                        ProductName =c.ProductName,
                                        StandardOutput = 0,
                                        BatchSize = 0,
                                        MonthlyPlannedQTY = 0,
                                        ProductionQty = 0,
                                        BOMId = 0,
                                        NumberOfBatch = 0,
                                        CompletedNumberOfBatch = 0
                                    }).ToList();
                return Json(productInfo, sa);

            }
            else
            {
                return Json("");
            }
        }
        public JsonResult GetProcessOrderDetail(string processOrderNo)
        {
            var productId = _context.TblRebProcessOrder.Where(x => x.ProcessOrderNo == processOrderNo).Select(i => i.ProductCode).FirstOrDefault().ToString();

            var poNoOfBatch = _context.TblRebProcessOrder.Where(x => x.ProcessOrderNo == processOrderNo).Select(i => i.NumberOfBatch).FirstOrDefault();
            decimal defaulval = 0;
            var completedNoOfBatch = _context.TblRebRequisition
                                        .Where(requisition => requisition.LinkedProcessNo == processOrderNo)
                                        .Sum(requisition => requisition.NumberOfBatch);

            if (completedNoOfBatch == null)
            {
                completedNoOfBatch = defaulval;
            }
            var noOfBatch = poNoOfBatch;
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();


            var requisitionNoParam = new SqlParameter("@productId", productId);
            var productionQtyParam = new SqlParameter("@noOfBatch", noOfBatch);

            var productList = _context.GetProductWiseBOMDetail
                                .FromSql("EXEC sp_GetProductWiseBOMDetail @productId, @noOfBatch", requisitionNoParam, productionQtyParam).ToList();

            return Json(productList, sa);
        }




        private string GenerateRequisitionNo()
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var requisitionNo = "RBQ-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblRebRequisition.Where(c => c.RequisitionNo.Substring(0, 8) == requisitionNo).OrderByDescending(t => t.Id) select c.RequisitionNo.Substring(8, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            requisitionNo = "RBQ-" + yy + mn + maxId;

            return requisitionNo;
        }

 
    }
}

