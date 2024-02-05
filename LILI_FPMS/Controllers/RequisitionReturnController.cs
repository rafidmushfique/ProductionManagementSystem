using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LILI_FPMS.Models;
using LILI_FPMS.Models.ManageViewModels;
using LILI_FPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace LILI_FPMS.Controllers
{
    [Authorize]
    public class RequisitionReturnController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public RequisitionReturnController(dbFormulationProductionSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmpIdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "empId_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var return_data = from master in _context.TblReturn
                          from requisition in _context.TblRequisition
                          from bom in _context.View_BOM
                          from p in _context.View_Product
                          where(master.RequisitionNo == requisition.RequisitionNo && requisition.ProductCode == bom.ProductCode && bom.ProductCode== p.ProductCode)
                          select new TblReturn
                          {
                              Id = master.Id,
                              ReturnNo = master.ReturnNo,
                              ReturnDate = master.ReturnDate,
                              RequisitionNo = master.RequisitionNo,
                              BatchNo = requisition.BatchNo,
                              ProductCode = requisition.ProductCode,
                              ProductName = p.ProductName
                          };

            if (!String.IsNullOrEmpty(searchString))
            {
                return_data = return_data.Where(s => s.ReturnNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    return_data = return_data.OrderByDescending(s => s.ReturnNo);
                    break;
                case "empId_desc":
                    return_data = return_data.OrderByDescending(s => s.ReturnNo);
                    break;
                //case "Date":
                //    employees = employees.OrderBy(s => s.EnrollmentDate);
                //    break;
                //case "date_desc":
                //    employees = employees.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    return_data = return_data.OrderByDescending(s => s.ReturnNo);
                    break;
            }
            int pageSize = 7;


            return View(await PaginatedList<TblReturn>.CreateAsync(return_data.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public ActionResult Create()
        {
            TblReturn entities = new TblReturn();
            entities.Idate = DateTime.Now;
            entities.ReturnDate = DateTime.Now;
            entities.ReturnNo = GetAutoNumber();

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem { Value = "Fresh", Text = "Fresh" });
            typeList.Add(new SelectListItem { Value = "NC", Text = "NC" });

            ViewBag.TypeList=typeList;
            //entities.TblReturnDetails = (from ret in eFTestContext.TblReturnDetails
            //                                 select new TblReturnDetails
            //                                 {
            //                                     Id = ret.Id,
            //                                     MaterialCode = ret.MaterialCode,
            //                                     MaterialName = ret.MaterialName,
            //                                     Unit = ret.Unit,
            //                                     IssuedQty = ret.IssuedQty,
            //                                     ReturnQty = ret.ReturnQty
            //                                 }).ToList();

            return View(entities);
        }

      

        [HttpPost, ActionName("CreateReturn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReturn(TblReturn return_data)
        //[Bind("EnrollmentDate,FirstMidName,LastName")] Student student)
        {
            try
            {


                        if (ModelState.IsValid)
                        {
                            var result = FloorStockCheck(return_data);
                            var CheckFloorStock = false;
                            if (result != null)
                            {
                                if (result.Count == 0)
                                {
                                    CheckFloorStock = true;
                                }

                                if (CheckFloorStock) 
                                {
                            
                                    if (DoesReturnNoExists(return_data.ReturnNo))
                                    {
                                        return_data.ReturnNo = GetAutoNumber();
                                    }


                                    var return_entity = new TblReturn
                                    {

                                        ReturnNo = return_data.ReturnNo,
                                        ReturnDate = return_data.ReturnDate,
                                        RequisitionNo = return_data.RequisitionNo,
                                        IssueNo = "000",
                                        Comments = return_data.Comments,
                                        Iuser = User.Identity.Name,
                                        Idate = DateTime.Now
                                    };
                                    _context.TblReturn.Add(return_entity);
                                    await _context.SaveChangesAsync();


                                    if (return_data.TblReturnDetails != null)
                                    {
                                        foreach (var item in return_data.TblReturnDetails.ToList())
                                        {
                                            TblReturnDetails itemDetail = new TblReturnDetails();
                                            if (item.ReturnQty > 0)
                                            {
                                                itemDetail.ReturnNo = return_data.ReturnNo;
                                                itemDetail.MaterialCode = item.MaterialCode;
                                                itemDetail.GRNNo = item.GRNNo;
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
                    return View(return_data);
                }

                // Update tblFloorStock Table
                UpdateFloorStockFromReturn(return_data.ReturnNo);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
        }

        public JsonResult UpdateFloorStockFromReturn(string returnNo)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();
            var returnNoParam = new SqlParameter("@ReturnNo", returnNo);

            var productList = _context.UpdateFloorScockFromForductionQC
                                .FromSql("EXEC sp_UpdateFloorStockFromReturn @ReturnNo", returnNoParam)
                                .ToList();

            return Json(productList, sa);
        }


        [HttpPost]
        public bool Delete(int id)
        //public ActionResult Delete(int id)
        {
            try
            {
                TblReturn data = _context.TblReturn.Where(s => s.Id == id).First();
                _context.TblReturnDetails.RemoveRange(_context.TblReturnDetails.Where(d => d.ReturnNo == data.ReturnNo));
                _context.TblReturn.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ActionResult Update(int id)
        {
            Models.TblReturn ReturnModel = new Models.TblReturn();
            var dt = _context.TblReturn.Where(s => s.Id == id).First();
            ReturnModel.Id = dt.Id;
            ReturnModel.ReturnNo = dt.ReturnNo;
            ReturnModel.ReturnDate = dt.ReturnDate;
            ReturnModel.Comments = dt.Comments;
            ReturnModel.RequisitionNo = dt.RequisitionNo;
            ReturnModel.BatchNo = _context.TblRequisition.Where(req => req.RequisitionNo == dt.RequisitionNo).FirstOrDefault().BatchNo;            
            var productCode =  _context.TblRequisition.Where(req => req.RequisitionNo == dt.RequisitionNo).FirstOrDefault().ProductCode;
            ReturnModel.ProductName = _context.View_Product.Where(req => req.ProductCode == productCode).FirstOrDefault().ProductName;
            ReturnModel.Unit = _context.View_Product.Where(req => req.ProductCode == productCode).FirstOrDefault().PackSize;
            ReturnModel.BatchSize = _context.View_BOM.Where(req => req.ProductCode == productCode).FirstOrDefault().BatchSize.ToString();

            var return_detail =  from master in _context.TblReturn
                                 from detail in _context.TblReturnDetails
                                 from material in _context.View_Material
                                 where (master.ReturnNo == detail.ReturnNo)
                                 where (detail.MaterialCode == material.MaterialCode)                                 
                                 where (master.ReturnNo == dt.ReturnNo)
                                 orderby detail.MaterialCode
                                 select new TblReturnDetails
                                 {
                                     Id = detail.Id,
                                     ReturnNo = detail.ReturnNo,
                                     MaterialCode = detail.MaterialCode,
                                     MaterialName = material.MaterialName,
                                     GRNNo = detail.GRNNo,
                                     IssuedQty = detail.IssuedQty,
                                     ReturnQty = detail.ReturnQty,
                                     Type = detail.Type
                                 };

            ReturnModel.TblReturnDetails = return_detail.ToList();

            return View(ReturnModel);
        }        



        [HttpPost, ActionName("UpdateReturn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReturn(int id, TblReturn return_data)
        {
            if (id != return_data.Id)
            {
                return NotFound();
            }
            try
            {
                
                var returndataToUpdate = await _context.TblReturn.FirstOrDefaultAsync(s => s.Id == id);
                returndataToUpdate.Edate = DateTime.Now;
                returndataToUpdate.Euser = User.Identity.Name;

                if (await TryUpdateModelAsync<TblReturn>(
                    returndataToUpdate,
                    "",
                    s => s.ReturnNo, s => s.ReturnDate, s => s.RequisitionNo, s => s.Comments))

                _context.TblReturnDetails.RemoveRange(_context.TblReturnDetails.Where(d => d.ReturnNo == return_data.ReturnNo));

                if (return_data.TblReturnDetails != null)
                {
                    foreach (var item in return_data.TblReturnDetails.ToList())
                    {
                        TblReturnDetails itemDetail = new TblReturnDetails();
                        itemDetail.ReturnNo = return_data.ReturnNo;
                        itemDetail.MaterialCode = item.MaterialCode;
                        itemDetail.GRNNo = item.GRNNo;
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

        public IActionResult addRequisition()
        {
            //List<SelectListItem> typeList = new List<SelectListItem>();
            //typeList.Add(new SelectListItem { Value = "Fresh", Text = "Fresh" });
            //typeList.Add(new SelectListItem { Value = "NC", Text = "NC" });

            //ViewBag.TypeList = typeList;

            StringBuilder sb = new StringBuilder();
     


            Models.TblRequisition requisitionModel = new Models.TblRequisition();
            return PartialView("_RequisitionAreaPartial", requisitionModel);
        }

        [HttpPost]
        public JsonResult SearchRequisition(string RequisitionNo)
        {
            //var currentMonth = DateTime.Now.Month;
            //var currentYear = DateTime.Now.Year;
            //var model = new List<TblRequisition>(from req in _context.TblRequisition
            //                                     from b in _context.View_BOM
            //                                     from p in _context.View_Product
            //                                     where (req.ProductCode == b.ProductCode && b.ProductCode == p.ProductCode && (req.RequisitionNo.ToUpper().Contains(RequisitionNo.ToUpper()))) &&
            //                                            req.RequisitionDate.Year == currentYear &&
            //                                            req.RequisitionDate.Month == currentMonth
            //                                     select new TblRequisition
            //                                     {
            //                                         RequisitionNo = req.RequisitionNo,
            //                                         ProductCode = req.ProductCode,
            //                                         ProductName = p.ProductName
            //                                     });
            var requisitionNoParam = new SqlParameter("@RequisitionNo", RequisitionNo);
            var model = _context.VMRequisitionSerachReturn.FromSql("Exec sp_SearchRequisitionForReturn @RequisitionNo", requisitionNoParam).ToList();
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        public JsonResult GetProcessNumber(string RequisitionNo)
        {
            var model = new List<TblProductionProcess>(from prop in _context.TblProductionProcess
                                                 from req in _context.TblRequisition                                                
                                                 where (prop.RequisitionNo == req.RequisitionNo && (prop.RequisitionNo.ToUpper().Contains(RequisitionNo.ToUpper())))
                                                 select new TblProductionProcess
                                                 {
                                                    ProcessNo = prop.ProcessNo,
                                                    ProductionQty = prop.ProductionQty
                                                 });
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        public JsonResult GetQCQuantity(string ProcessNo)
        {
            var model = new List<TblProductionProcess>(from prop in _context.TblProductionProcess
                                                       select new TblProductionProcess
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
            var keyVal = _context.TblRequisition.Where(c => c.RequisitionNo == RequisitionNo).ToList();
            if (keyVal != null)
            {
                RequisitionNo = keyVal.FirstOrDefault().RequisitionNo;
            }

            if (RequisitionNo != "")
            {
                var sa = new JsonSerializerSettings();
                var expertiesInfo = from c in _context.TblRequisition
                                    from b in _context.View_BOM
                                    from p in _context.View_Product
                                    where(c.RequisitionNo == RequisitionNo && c.ProductCode == b.ProductCode && b.ProductCode == p.ProductCode)
                                    select new
                                    {
                                        c.RequisitionNo,
                                        c.BatchNo,
                                        p.ProductName,
                                        p.PackSize,
                                        b.BatchSize
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
                                .FromSql("EXEC sp_GetRequisitionWiseIssueDetail @requisitionNo", requisitionNoParam)
                                .ToList();

            return Json(productList, sa);
        }
        public string GetAutoNumber()
        {

            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var ReturnNo = "RT-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblReturn.Where(c => c.ReturnNo.Substring(0, 7) == ReturnNo).OrderByDescending(t => t.Id) select c.ReturnNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            ReturnNo = "RT-" + yy + mn + maxId;

            return ReturnNo;

        }
        private bool DoesReturnNoExists(string vReturnNo)
        {
            return _context.TblReturn.Any(e => e.ReturnNo == vReturnNo);
        }
        private List<FloorStockCheckViewModel> FloorStockCheck(TblReturn prod)
        {
            //Getting updated FloorStock 
            var floorStockList = new List<FloorStockCheckViewModel>();
            var requsitionNo = prod.RequisitionNo;

            var requisitionNoParam = new SqlParameter("@requisitionNo", requsitionNo);
            var productList = _context.GetRequisitionIssueDetail
                                .FromSql("EXEC sp_GetRequisitionWiseIssueDetail @requisitionNo", requisitionNoParam)
                                .ToList();
            var userProductList = prod.TblReturnDetails.ToList();

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
    }
}