﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LILI_FPMS.Models;
using LILI_FPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace LILI_FPMS.Controllers
{
    [Authorize]
    public class RebRequisitionApprovalController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public RebRequisitionApprovalController(dbFormulationProductionSystemContext context)
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
                      where s.ProductCode==p.ProductCode
                      select new TblRebRequisition
                      {
                          Id=s.Id,
                          RequisitionNo = s.RequisitionNo,
                          LinkedProcessNo=s.LinkedProcessNo,
                          ProductCode = s.ProductCode,
                          ProductName = p.ProductName,
                          BatchNo = s.BatchNo,
                          NumberOfBatch = s.NumberOfBatch,
                          RequisitionDate = s.RequisitionDate,
                          ApprovalStatus = _context.TblRebRequisitionApprovalStatus.Where(x => x.RequisitionNo == s.RequisitionNo).OrderByDescending(x=>x.Id).FirstOrDefault().ApprovalStatus
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
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var requisitionNo = "REQ-" + yy + mn;
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
            requisitionNo = "REQ-" + yy + mn + maxId;

            //Generate Requisition No.---------End

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
                if (ModelState.IsValid)
                {
                    req.Iuser = User.Identity.Name;
                    req.Idate = DateTime.Now;
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

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                TblRebRequisition req = _context.TblRebRequisition.Where(s => s.Id == id).First();
                _context.TblRebRequisitionDetail.RemoveRange(_context.TblRebRequisitionDetail.Where(d => d.RequisitionNo == req.RequisitionNo));
                _context.TblRebRequisition.Remove(req);
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
            Models.TblRebRequisition reqModel = new Models.TblRebRequisition();
            var dt = _context.TblRebRequisition.Where(s => s.Id == id).First();
            reqModel.Id = dt.Id;
            reqModel.RequisitionNo = dt.RequisitionNo;
            reqModel.RequisitionDate = dt.RequisitionDate;
            reqModel.BatchNo = dt.BatchNo;
            reqModel.NumberOfBatch = dt.NumberOfBatch;
            reqModel.BatchSize = _context.View_BOM.Where(s => s.ProductCode == dt.ProductCode).FirstOrDefault().BatchSize;
            reqModel.ProductCode = dt.ProductCode;
            reqModel.ProductName = _context.View_Product.Where(s => s.ProductCode == dt.ProductCode).FirstOrDefault().ProductName;
            reqModel.Comments = dt.Comments;
            reqModel.TblRebRequisitionDetail = _context.TblRebRequisitionDetail.Where(d => d.RequisitionNo == dt.RequisitionNo).ToList();
            reqModel.LinkedProcessNo = dt.LinkedProcessNo;
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
                                     AvailableStock = reqDtl.AvailableStock
                                 };

            reqModel.TblRebRequisitionDetail = requisitionDetail.ToList();

            
            return View(reqModel);
        }

        [HttpPost, ActionName("UpdateRequisition")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRequisition(int id, TblRebRequisition req)
        {
            if (id != req.Id)
            {
                return NotFound();
            }
            try
            {
                //eFTestContext.Update(emp);
                //await eFTestContext.SaveChangesAsync();

                var requisitionToUpdate = await _context.TblRebRequisition.FirstOrDefaultAsync(s => s.Id == id);
                requisitionToUpdate.Euser = User.Identity.Name;
                requisitionToUpdate.Edate = DateTime.Now;
                if (await TryUpdateModelAsync<TblRebRequisition>(
                    requisitionToUpdate,
                    "",
                    s => s.RequisitionNo, s => s.RequisitionDate, s => s.BatchNo, s => s.NumberOfBatch, s => s.ProductCode, s => s.Comments, s => s.Euser, s => s.Edate))

                    _context.TblRebRequisitionDetail.RemoveRange(_context.TblRebRequisitionDetail.Where(d => d.RequisitionNo == req.RequisitionNo));

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
            return View(req);
        }

        public IActionResult SearchProduct()
        {
            //var model = new List<TblRequisition>(from c in npsContext.View_Product 
            //                                     select c);

            var model = new List<TblRequisition>(from b in _context.View_BOM
                        from p in _context.View_Product
                        where (b.ProductCode == p.ProductCode) && b.IsActive == "Y"
                        select new TblRequisition
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
                var productInfo = from b in _context.View_BOM
                            from p in _context.View_Product
                            where (b.ProductCode == p.ProductCode && b.ProductCode == productId && b.IsActive == "Y")
                            select new TblRequisition
                            {
                                ProductCode = b.ProductCode,
                                ProductName = p.ProductName,
                                StandardOutput = b.StandardOutput,
                                BatchSize = b.BatchSize
                            };
                return Json(productInfo, sa);
            }
            else
            {
                return Json("");
            }
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
            var productList = from b in _context.View_BOM
                              from p in _context.View_Product
                              from bd in _context.View_BOMDetail
                              from m in _context.View_Material                              
                              where (b.ProductCode == p.ProductCode && b.Id == bd.Bomid && bd.MaterialCode == m.MaterialCode)
                              && (b.ProductCode == productId) && b.IsActive=="Y"
                              orderby bd.MaterialCode
                              select new TblRebRequisitionDetail
                              {
                                  MaterialCode = bd.MaterialCode,
                                  MaterialName = m.MaterialName,
                                  Unit = m.BaseUnit,
                                  StandardRecipeQty = bd.QuantityPerBatch * noOfBatch,
                                  FloorStock = 0,
                                  RequiredQty = bd.QuantityPerBatch * noOfBatch,
                                  AvailableStock = 0
                              };
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

        [HttpPost]
        public IActionResult RequisitionApprovalStatus(string requisitionNo, string approvalStatus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TblRebRequisitionApprovalStatus ras = new TblRebRequisitionApprovalStatus();
                    ras.RequisitionNo = requisitionNo;
                    ras.RequisitionStatusDate = DateTime.Now;
                    ras.ApprovalStatus = approvalStatus;
                    ras.Approver = User.Identity.Name;
                    //_context.Add(ras);
                    //await _context.SaveChangesAsync();





                    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    var config = builder.Build();
                    string constring = config.GetConnectionString("DefaultConnection");

                    SqlConnection con = new SqlConnection(constring);

                    string queryDel = "DELETE TblRebRequisitionApprovalStatus WHERE RequisitionNo =" + "'" + ras.RequisitionNo + "'";
                    SqlCommand cmdDel = new SqlCommand(queryDel, con);
                    con.Open();
                    int d = cmdDel.ExecuteNonQuery();
                    con.Close();

                    string query = "INSERT INTO TblRebRequisitionApprovalStatus(RequisitionNo, RequisitionStatusDate, ApprovalStatus, Approver) " +
                                    "values ('" + ras.RequisitionNo + "','" + ras.RequisitionStatusDate + "','" + ras.ApprovalStatus + "','" + ras.Approver + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();

                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    //return View(ras);
                }
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


    }
}
