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
    [Authorize]
    public class STIFGReceiveController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public STIFGReceiveController(dbFormulationProductionSystemContext context)
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


            IQueryable<TblRebStifgreceive> fgtn_data = _context.TblRebStifgreceive;


            if (!String.IsNullOrEmpty(searchString))
            {
                fgtn_data = fgtn_data.Where(s => s.StifgreceiveNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    fgtn_data = fgtn_data.OrderByDescending(s => s.StifgreceiveNo);
                    break;

                default:
                    fgtn_data = fgtn_data.OrderByDescending(s => s.StifgreceiveNo);
                    break;
            }
            int pageSize = 7;


            return View(await PaginatedList<TblRebStifgreceive>.CreateAsync(fgtn_data.AsNoTracking(), pageNumber ?? 1, pageSize));

        }
        public IActionResult Create()
        {

            TblRebStifgreceive entities = new TblRebStifgreceive();
            var STIFGReceiveNo = GenerateSTIFGReceiveNo();
            entities.StifgreceiveNo = STIFGReceiveNo;
            entities.StifgreceiveDate = DateTime.Now;
            return View(entities);
        }

        public async Task<ActionResult> CreateReceive(TblRebStifgreceive model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var CodeExists = _context.TblRebStifgreceive.Any(x => x.StifgreceiveNo == model.StifgreceiveNo);
                    if (CodeExists)
                    {
                        model.StifgreceiveNo = GenerateSTIFGReceiveNo();
                    }
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;

                    var existingEntity = _context.TblRebStifgreceive.FirstOrDefault(x => x.StifgreceiveNo == model.StifgreceiveNo);

                    var details = model.TblRebStifgreceiveDetail.ToList();

                    await _context.AddAsync(model);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                     "Try again, and if the problem persists, " +
                     "see your system administrator.");
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int Id)
        {
            TblRebStifgreceive fgtnModel = new TblRebStifgreceive();
            var dt = _context.TblRebStifgreceive.Where(s => s.Id == Id).First();
            fgtnModel.Id = dt.Id;
            fgtnModel.StifgreceiveNo = dt.StifgreceiveNo;
            fgtnModel.StifgreceiveDate = dt.StifgreceiveDate;
            fgtnModel.Stino = dt.Stino;
            fgtnModel.Stistock = dt.Stistock;
            fgtnModel.ReceiveComment = dt.ReceiveComment;

            fgtnModel.TblRebStifgreceiveDetail = _context.TblRebStifgreceiveDetail.Where(x => x.StifgreceiveNo == fgtnModel.StifgreceiveNo).ToList();

            return View(fgtnModel);
        }

        //[HttpPost, ActionName("UpdateReceive")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReceive(int id, TblRebStifgreceive fgtnData)
        {
            if (id != fgtnData.Id)
            {
                return NotFound();
            }
            try
            {
                var fgtnDataToUpdate = await _context.TblRebStifgreceive.FirstOrDefaultAsync(s => s.Id == id);
                fgtnDataToUpdate.Edate = DateTime.Now;
                fgtnDataToUpdate.Euser = User.Identity.Name;

                if (await TryUpdateModelAsync<TblRebStifgreceive>(
                    fgtnDataToUpdate,
                    "",
                    s => s.StifgreceiveNo, s => s.StifgreceiveDate, s => s.Stino, s => s.Stistock, s => s.ReceiveComment))

                 
               
                
                if (fgtnData.TblRebStifgreceiveDetail != null)
                {
                    _context.TblRebStifgreceiveDetail.RemoveRange(_context.TblRebStifgreceiveDetail.Where(x => x.StifgreceiveNo == fgtnData.StifgreceiveNo));
                    var data = fgtnData.TblRebStifgreceiveDetail.ToList();
                    await _context.TblRebStifgreceiveDetail.AddRangeAsync(data);

                }
                

                    await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(fgtnData);
        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                TblRebStifgreceive data = _context.TblRebStifgreceive.Where(s => s.Id == id).First();
                _context.TblRebStifgreceive.RemoveRange(_context.TblRebStifgreceive.Where(d => d.StifgreceiveNo == data.StifgreceiveNo));
                _context.TblRebStifgreceive.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //[HttpPost]
        //public JsonResult GetSTIFGDetials(string StiNo)
        //{
        //    try
        //    {
        //        var sa = new JsonSerializerSettings();
        //        var model = (from c in _context.TblRebStifgreceiveDetail

        //                     select new TblRebStifgreceiveDetail
        //                     {
        //                         Id = c.Id,
        //                         StifgreceiveNo = c.StifgreceiveNo,
        //                         Fgcode = c.Fgcode,
        //                         Fgname = c.Fgname,
        //                         Unit = c.Unit,
        //                         Stiquantity = c.Stiquantity,
        //                         ReceiveQuantity = c.ReceiveQuantity,
        //                         ActualReceiveQty = c.ActualReceiveQty,
        //                        // Comments = c.Comments
        //                     }).ToList();

        //        return Json(model);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json("");
        //    }
        //}

        [HttpPost]
        public JsonResult GetSTIFGDetials(string StiNo)
        {
            try
            {
                var sa = new JsonSerializerSettings();
                var issueNoParam = new SqlParameter("@IssueNo", StiNo);

                //var model = _context.GetSTIFGDetialsVM
                //                    .FromSql("EXEC sp_FGIssueDetails @IssueNo", issueNoParam);
                var model = _context.GetSTIFGDetialsVM
                                .FromSql("EXEC sp_FGIssueDetails @IssueNo", issueNoParam)
                                .Select(c => new GetSTIFGDetialsVM
                                {
                                    Id = c.Id,
                                    Fgcode = c.Fgcode,
                                    Fgname = c.Fgname,
                                    Unit = c.Unit,
                                    Stiquantity = c.Stiquantity
                                    //,
                                    // ReceiveQuantity = c.ReceiveQuantity,
                                    //ActualReceiveQty = c.ActualReceiveQty,

                                }).ToList();
                return Json(model, sa);
            }
            catch (Exception ex)
            {

                return Json("");
            }
        }

        //[HttpGet]
        //public JsonResult GetSTIFGDetials(string StiNo)
        //{

        //    if (StiNo.Length >= 0)
        //    {
        //        var sa = new JsonSerializerSettings();

        //        var productNoParam = new SqlParameter("@IssueNo", StiNo);

        //        var productInfo = _context.TblRebStifgreceiveDetail
        //                            .FromSql("EXEC sp_FGIssueDetails @IssueNo", productNoParam)
        //                            .ToList();
        //        return Json(new { result = productInfo });
        //    }
        //    else
        //    {
        //        return Json("");
        //    }
        //}


        public string GenerateSTIFGReceiveNo()
        {
            //Generate FGTN No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var FGTNNo = "RFR-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblRebStifgreceive.Where(c => c.StifgreceiveNo.Substring(0, 8) == FGTNNo).OrderByDescending(t => t.Id) select c.StifgreceiveNo.Substring(8, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            FGTNNo = "RFR-" + yy + mn + maxId;

            return FGTNNo;


        }

        public IActionResult addSTINo() 
        {
            VMAllCurrentSTINo model = new VMAllCurrentSTINo();
            return PartialView("_STISearchPartial",model);
        }
        public JsonResult SearchSTINo( string STINo) 
        {
            var sa = new JsonSerializerSettings();
            var stinovalue = String.IsNullOrEmpty(STINo) ? "" : STINo;
            var stinoParam = new SqlParameter("@STINo", stinovalue);
            var model = _context.VMAllCurrentSTINo
                        .FromSql("Exec sp_GetAllCurrentSTINo @STINo",stinoParam)
                        .Select(c=> new VMAllCurrentSTINo { 
                            STINo = c.STINo,
                            STIDate = DateTime.Parse(c.STIDate.ToString("yyyy MM dd")),
                            });

            return Json(model, sa);
        }
        public JsonResult SetSTIInformation(string STINo)
        {
            var sa = new JsonSerializerSettings();

            var model = _context.VMAllCurrentSTINo.FromSql("Exec sp_GetAllCurrentSTINo");

            return Json(model, sa);
        }
}
}
