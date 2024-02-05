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
using LILI_FPMS.Models.ManageViewModels;
using System.Text;


namespace LILI_FPMS.Controllers
{

    public class RebReturnController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public RebReturnController(dbFormulationProductionSystemContext context)
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

            //var req = from s in _context.TblRebRequisition
            //          from p in _context.View_Product
            //          where s.ProductCode == p.ProductCode
            //          select new TblRebRequisition
            //          {
            //              Id = s.Id,
            //              RequisitionNo = s.RequisitionNo,
            //              ProductCode = s.ProductCode,
            //              ProductName = p.ProductName,
            //              BatchNo = s.BatchNo,
            //              NumberOfBatch = s.NumberOfBatch,
            //              RequisitionDate = s.RequisitionDate,
            //              IssueStatus = s.IssueStatus,
            //              LinkedProcessNo = s.LinkedProcessNo,
            //          };
            var req = _context.GetRebReturnIndex.FromSql("EXEC sp_GetAllRefurbishedReturnData");

            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(s => s.RequisitionNo.Contains(searchString)
                                    || s.FGCode.Contains(searchString)
                                    || s.FGName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "requisition_desc":
                    req = req.OrderByDescending(s => s.RequisitionNo);
                    break;
                case "productCode_desc":
                    req = req.OrderByDescending(s => s.FGCode);
                    break;
                case "productName_desc":
                    req = req.OrderByDescending(s => s.FGCode);
                    break;

                default:
                    req = req.OrderByDescending(s => s.RequisitionNo);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<GetRebReturnIndex>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));

            
        }

        public IActionResult Create ()
        {
            TblRebReturn model = new TblRebReturn();
            model.ReturnNo = GenerateNo();
            model.ReturnDate = DateTime.Now;
            return View(model);
        }

        public async Task<ActionResult> CreateReturn(TblRebReturn TblRebReturn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = FloorStockCheck(TblRebReturn);
                    var CheckFloorStock = false;
                    if (result != null)
                    {
                        if (result.Count == 0)
                        {
                            CheckFloorStock = true;
                        }

                        if (CheckFloorStock)
                        {

                            if (DoesReturnNoExists(TblRebReturn.ReturnNo))
                            {
                                TblRebReturn.ReturnNo = GenerateNo();
                            }


                            var return_entity = new TblRebReturn
                            {

                                ReturnNo = TblRebReturn.ReturnNo,
                                ReturnDate = TblRebReturn.ReturnDate,
                                RequisitionNo = TblRebReturn.RequisitionNo,
                                IssueNo = "000",
                                Comments = TblRebReturn.Comments,
                                Iuser = User.Identity.Name,
                                Idate = DateTime.Now
                            };
                            _context.TblRebReturn.Add(return_entity);
                            await _context.SaveChangesAsync();


                            if (TblRebReturn.TblRebReturnDetails != null)
                            {
                                foreach (var item in TblRebReturn.TblRebReturnDetails.ToList())
                                {
                                    TblRebReturnDetails itemDetail = new TblRebReturnDetails();
                                    if (item.ReturnQty > 0)
                                    {
                                        itemDetail.ReturnNo = TblRebReturn.ReturnNo;
                                        itemDetail.MaterialCode = item.MaterialCode;
                                        itemDetail.Grnno = item.Grnno;
                                        itemDetail.IssuedQty = item.IssuedQty;
                                        itemDetail.ReturnQty = item.ReturnQty;
                                        itemDetail.Type = item.Type;
                                        await _context.AddAsync(itemDetail);
                                    }
                                }
                            }
                            await _context.SaveChangesAsync();
                            TempData["Success"] = "Success message text.";
                        }
                    }
                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    return View("Create",TblRebReturn);
                }
            }
            catch (Exception ex)
            {

                    ModelState.AddModelError("", "Unable to save changes. " +
                   "Try again, and if the problem persists " +
                   "see your system administrator.");
            }
            return RedirectToAction("Index"); 
        }
        public IActionResult Update(int Id)
        {
            TblRebReturn model = new TblRebReturn();
            if (!_context.TblRebReturn.Any(x => x.Id == Id))
            {
                return NotFound();
            }
            var dt = _context.TblRebReturn.Where(x => x.Id == Id).FirstOrDefault();
            
            model.Id = dt.Id;
            model.ReturnNo = dt.ReturnNo;
            model.ReturnDate = dt.ReturnDate;
            model.Comments = dt.Comments;
            model.RequisitionNo = dt.RequisitionNo;
            var ProductCode = _context.TblRebRequisition.Where(x => x.RequisitionNo == dt.RequisitionNo).Select(x => x.ProductCode).First();
            model.ProductName = _context.View_Product.Where(x=> x.ProductCode== ProductCode).Select(x=>x.ProductName).FirstOrDefault();
            model.BatchSize = "";
            model.TblRebReturnDetails = _context.TblRebReturnDetails.Where(x=>x.ReturnNo == dt.ReturnNo).ToList();
            model.Unit = _context.View_Product.Where(x => x.ProductCode == ProductCode).Select(x => x.PackSize).FirstOrDefault();
            return View(model);
        }

        public async Task<IActionResult> UpdateReturn(int id, TblRebReturn return_data)
        {
            if (id != return_data.Id)
            {
                return NotFound();
            }
            try
            {

                var returndataToUpdate = await _context.TblRebReturn.FirstOrDefaultAsync(s => s.Id == id);
                returndataToUpdate.Edate = DateTime.Now;
                returndataToUpdate.Euser = User.Identity.Name;

                if (await TryUpdateModelAsync<TblRebReturn>(
                    returndataToUpdate,
                    "",
                    s => s.ReturnNo, s => s.ReturnDate, s => s.RequisitionNo, s => s.Comments))

                    _context.TblRebReturnDetails.RemoveRange(_context.TblRebReturnDetails.Where(d => d.ReturnNo == return_data.ReturnNo));

                if (return_data.TblRebReturnDetails != null)
                {
                    foreach (var item in return_data.TblRebReturnDetails.ToList())
                    {
                        TblRebReturnDetails itemDetail = new TblRebReturnDetails();
                        itemDetail.ReturnNo = return_data.ReturnNo;
                        itemDetail.MaterialCode = item.MaterialCode;
                        itemDetail.Grnno = item.Grnno;
                        itemDetail.IssuedQty = item.IssuedQty;
                        itemDetail.ReturnQty = item.ReturnQty;
                        await _context.AddAsync(itemDetail);
                    }
                    await _context.SaveChangesAsync();
                }


                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(return_data);
        }

        private string GenerateNo() 
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var returnNo = "RRT-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblRebReturn.Where(c => c.ReturnNo.Substring(0, 8) == returnNo).OrderByDescending(t => t.Id) select c.ReturnNo.Substring(8, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            returnNo = "RRT-" + yy + mn + maxId;

            return returnNo;
        }
        private bool DoesReturnNoExists(string vReturnNo)
        {
            return _context.TblRebReturn.Any(e => e.ReturnNo == vReturnNo);
        }
        private List<FloorStockCheckViewModel> FloorStockCheck(TblRebReturn prod)
        {
            //Getting updated FloorStock 
            var floorStockList = new List<FloorStockCheckViewModel>();
            var requsitionNo = prod.RequisitionNo;

            var requisitionNoParam = new SqlParameter("@requisitionNo", requsitionNo);
            var productList = _context.GetRequisitionIssueDetail
                                .FromSql("EXEC sp_GetRebRequisitionWiseIssueDetail @requisitionNo", requisitionNoParam)
                                .ToList();
            var userProductList = prod.TblRebReturnDetails.ToList();

            //CurrentUseQty and Floorstock check : CurrentUseQty can not be greater than Floorstock\
            if (productList != null && userProductList != null)
            {
                foreach (var item in userProductList)
                {
                    var floorStockItem = new FloorStockCheckViewModel();
                    var itemFloorStock = userProductList.Where(x => x.MaterialCode == item.MaterialCode).Select(x => x.AvailableFloorStock);
                    var compareItemStock = decimal.Compare(item.ReturnQty, item.AvailableFloorStock);
                    if (compareItemStock > 0)
                    {

                        floorStockItem.MaterialName = item.MaterialName;
                        floorStockItem.MaterialCode = item.MaterialCode;
                        floorStockItem.FloorStockQty = item.AvailableFloorStock;
                        floorStockList.Add(floorStockItem);

                    }

                }
            }

            return floorStockList;
        }

        public IActionResult addRequisition()
        {
            //List<SelectListItem> typeList = new List<SelectListItem>();
            //typeList.Add(new SelectListItem { Value = "Fresh", Text = "Fresh" });
            //typeList.Add(new SelectListItem { Value = "NC", Text = "NC" });

            //ViewBag.TypeList = typeList;

            StringBuilder sb = new StringBuilder();



            Models.GetRebRequisitionSearch requisitionModel = new Models.GetRebRequisitionSearch();
            return PartialView("_RequisitionAreaPartial", requisitionModel);
        }

        [HttpPost]
        public JsonResult SearchRequisition(string RequisitionNo)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            //var model = new List<TblRebRequisition>(from req in _context.TblRebRequisition
            //                                     from b in _context.View_BOM
            //                                     from p in _context.View_Product
            //                                     where (req.ProductCode == b.ProductCode && b.ProductCode == p.ProductCode && (req.RequisitionNo.ToUpper().Contains(RequisitionNo.ToUpper()))) &&
            //                                            req.RequisitionDate.Year == currentYear &&
            //                                            req.RequisitionDate.Month == currentMonth
            //                                     select new TblRebRequisition
            //                                     {
            //                                         RequisitionNo = req.RequisitionNo,
            //                                         ProductCode = req.ProductCode,
            //                                         ProductName = p.ProductName
            //                                     });
            string requisitionNoValue = string.IsNullOrEmpty(RequisitionNo) ? "" : RequisitionNo;
            var requisitionNoParam = new SqlParameter("@RequisitionNo", requisitionNoValue) ;
            var model = _context.GetRebRequisitionSearch.FromSql("Exec sp_GetAllRebRequisition @RequisitionNo", requisitionNoParam).ToList();
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        public JsonResult GetProcessNumber(string RequisitionNo)
        {
            var model = new List<TblRebProductionProcess>(from prop in _context.TblRebProductionProcess
                                                          from req in _context.TblRequisition
                                                       where (prop.RequisitionNo == req.RequisitionNo && (prop.RequisitionNo.ToUpper().Contains(RequisitionNo.ToUpper())))
                                                       select new TblRebProductionProcess
                                                       {
                                                           ProcessNo = prop.ProcessNo,
                                                           ProductionQty = prop.ProductionQty
                                                       });
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        public JsonResult GetQCQuantity(string ProcessNo)
        {
            var model = new List<TblRebProductionProcess>(from prop in _context.TblRebProductionProcess
                                                       select new TblRebProductionProcess
                                                       {
                                                           ProcessNo = prop.ProcessNo,
                                                           ProductionQty = prop.ProductionQty
                                                       });
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }


        [HttpPost]
        public JsonResult SetRequisitionInfomation(string RequisitionNo)
        {
            var keyVal = _context.TblRebRequisition.Where(c => c.RequisitionNo == RequisitionNo).ToList();
            if (keyVal != null)
            {
                RequisitionNo = keyVal.FirstOrDefault().RequisitionNo;
            }

            if (RequisitionNo != "")
            {
                var sa = new JsonSerializerSettings();
                var expertiesInfo = from c in _context.TblRebRequisition
                                    from p in _context.View_Product
                                    where (c.RequisitionNo == RequisitionNo && c.ProductCode == p.ProductCode)
                                    select new
                                    {
                                        c.RequisitionNo,
                                        c.BatchNo,
                                        p.ProductName,
                                        p.PackSize
                                    };
                return Json(expertiesInfo, sa);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetRequisitionDetailsData(string RequisitionNo)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();


            var requisitionNoParam = new SqlParameter("@requisitionNo", RequisitionNo);

            var productList = _context.GetRequisitionIssueDetail
                                .FromSql("EXEC sp_GetRebRequisitionWiseIssueDetail @requisitionNo", requisitionNoParam)
                                .ToList();

            return Json(productList, sa);
        }
    }

}
