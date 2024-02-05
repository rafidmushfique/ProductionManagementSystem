using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LILI_FPMS.Models;
using LILI_FPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using Org.BouncyCastle.Ocsp;

namespace LILI_FPMS.Controllers
{

    [Authorize]
    public class RebQCController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;

        public RebQCController(dbFormulationProductionSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["QCNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "qcno_desc" : "";
            ViewData["EmpIdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "empId_desc" : "";
            ViewData["RequisitionNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "req_desc" : "";    
            ViewData["ProcessNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "process_desc" : "";
            ViewData["QCDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "qcdate_desc" : "";
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

            //var plans = from s in eFTestContext.TblMonthlyPlanning 
            //            from d in eFTestContext.TblMonthlyPlanningDetail
            //            where (s.PlanningNo == d.PlanningNo)
            //            select new {s.PlanningNo,s.Year,s.Month,d.Fgcode};

            //var qc_data = from master in _context.TblRebQc
            //              from requisition in _context.TblRebProcessOrder
            //              from process in _context.TblRebProductionProcess
            //              from p in _context.View_Product
            //              where(master.LinkedProcessNo == requisition.ProcessOrderNo && master.ProcessNo == process.ProcessNo && requisition.ProductCode == p.ProductCode)
            //              select new TblRebQc {
            //                  Id = master.Id,
            //                  ProductCode = requisition.ProductCode,
            //                  ProductName = p.ProductName,
            //                  Qcno = master.Qcno,
            //                  Qcdate = master.Qcdate,
            //                  LinkedProcessNo =  master.LinkedProcessNo,
            //                  BatchNo =  process.BatchNo,
            //                  ProcessNo =  master.ProcessNo,
            //                  Qcqty =  master.Qcqty,
            //                  QcpassQty =  master.QcpassQty,
            //                  QcrejectQty =  master.QcrejectQty
            //              };

            var qc_data = _context.VMAllRebQCIndex.FromSql("Exec sp_GetAllRebQCForIndex");
            if (!String.IsNullOrEmpty(searchString))
            {
                qc_data = qc_data.Where(s => s.Qcno.Contains(searchString)
                                        //|| s.RequisitionNo.Contains(searchString)
                                        || s.ProcessNo.Contains(searchString)
                                        || s.BatchNo.Contains(searchString)
                                        || s.ProductCode.Contains(searchString)
                                        || s.ProductName.Contains(searchString)

                );
            }

            switch (sortOrder)
            {
                case "qcno_desc":
                    qc_data = qc_data.OrderByDescending(s => s.Qcno);
                    break;
                case "empId_desc":
                    qc_data = qc_data.OrderByDescending(s => s.Qcno);
                    break;
                case "qcdate_desc":
                    qc_data = qc_data.OrderByDescending(s => s.Qcdate);
                    break;
                case "req_desc":
                    qc_data = qc_data.OrderByDescending(s => s.LinkedProcessNo);
                    break;
                case "process_desc":
                    qc_data = qc_data.OrderByDescending(s => s.ProcessNo);
                    break;
                //case "date_desc":
                //    employees = employees.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    qc_data = qc_data.OrderByDescending(s => s.Qcno);
                    break;
            }
            int pageSize = 7;


            return View(await PaginatedList<VMAllRebQCIndex>.CreateAsync(qc_data.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public ActionResult Create()
        {
            TblRebQc entities = new TblRebQc();

            List<SelectListItem> Year = new List<SelectListItem>();
            entities.IsSendToFloorStockFg = false;
            entities.Idate = DateTime.Now;
            entities.Qcdate = DateTime.Now;
            entities.Qcno = GetAutoNumber();

            List<TblQcparameterType> typeList = new List<TblQcparameterType>();
            typeList = (from c in _context.TblQcparameterType
                        select c).ToList();
            typeList.Insert(0, new TblQcparameterType { TypeCode = "0", TypeName = "Select Type" });
            ViewBag.ListOfType = typeList;

            //entities.TblRebQcdetails = (from para in eFTestContext.TblRebQcparameter
            //                                 select new TblRebQcdetails
            //                                 {
            //                                     Id = para.Id,
            //                                     QcparameterCode = para.QcparameterCode,
            //                                     QcparameterName = para.QcparameterName,
            //                                     QcparameterStandardValue = para.QcparameterStandardValue,
            //                                     QcparameterActualValue = null,
            //                                     Comments = null
            //                                 }).ToList();

            return View(entities);
        }

        public string GetAutoNumber()
        {

            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var QCNo = "QC-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblRebQc.Where(c => c.Qcno.Substring(0, 7) == QCNo).OrderByDescending(t => t.Id) select c.Qcno.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            QCNo = "QC-" + yy + mn + maxId;

            return QCNo;

        }

        [HttpPost, ActionName("CreateQC")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQC(TblRebQc qc_data)
        //[Bind("EnrollmentDate,FirstMidName,LastName")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CodeCheck = _context.TblRebQc.Any(x => x.Qcno == qc_data.Qcno);
                        if(CodeCheck)
                         {
                            qc_data.Qcno = GetAutoNumber();
                         }
                    
                    qc_data.Iuser = User.Identity.Name;
                    qc_data.Idate = DateTime.Now;
                    qc_data.RequisitionNo = "";
                    _context.Add(qc_data);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    return View(qc_data);
                }


                // Update tblFloorStock Table
                var fgCodeQuery = from p in _context.TblRebProductionProcess
                             join r in _context.TblRebProcessOrder on p.LinkedProcessNo equals r.ProcessOrderNo
                             where p.ProcessNo == qc_data.ProcessNo
                             select new { ProductCode  = r.ProductCode };
                string fgCode = fgCodeQuery.FirstOrDefault().ProductCode.ToString();

                var sfgCode = _context.TblRebProductionProcess.Where(c=>c.ProcessNo==qc_data.ProcessNo).FirstOrDefault().Sfgcode;

                //if (qc_data.IsSendToFloorStockFG && qc_data.QcpassQty > 0)
                //{
                //    UpdateFloorStockFromProductionQC(fgCode.ToString(), qc_data.Qcno, qc_data.QcpassQty);
                //}

                //if (qc_data.IsSendToFloorStockSFG && qc_data.SFGQcpassQty>0)
                //{
                //    UpdateFloorStockFromProductionQC(sfgCode.ToString(), qc_data.Qcno, qc_data.SFGQcpassQty);
                //}


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

        public JsonResult UpdateFloorStockFromProductionQC(string productId, string qcNo, decimal QCQty)
        {
            var errorViewModel = new ErrorViewModel();
            var sa = new JsonSerializerSettings();
            var productIdParam = new SqlParameter("@productId", productId);
            var qcNoParam = new SqlParameter("@qcNo", qcNo);
            var QCQtyParam = new SqlParameter("@qcQuantity", QCQty);

            //var userType = dbContext.Set().FromSql("dbo.SomeSproc @Id = {0}, @Name = {1}", 45, "Ada");

            var productList = _context.UpdateFloorStockRebQC
                                .FromSql("EXEC sp_RebUpdateFloorStockQ @productId, @qcNo, @qcQuantity", productIdParam, qcNoParam, QCQtyParam)
                                .ToList();

            return Json(productList, sa);
        }

        [HttpPost]
        public bool Delete(int id)
        //public ActionResult Delete(int id)
        {
            try
            {
                TblRebQc data = _context.TblRebQc.Where(s => s.Id == id).First();
                _context.TblRebQcdetails.RemoveRange(_context.TblRebQcdetails.Where(d => d.Qcno == data.Qcno));
                _context.TblFloorStock.RemoveRange(_context.TblFloorStock.Where(d => d.RequisitionNo == data.Qcno));
                _context.TblRebQc.Remove(data);
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
            Models.TblRebQc QCModel = new Models.TblRebQc();
            var dt = _context.TblRebQc.Where(s => s.Id == id).First();
            QCModel.Id = dt.Id;
            QCModel.Qcno = dt.Qcno;
            QCModel.Qcdate = dt.Qcdate;
            QCModel.Comments = dt.Comments;
            QCModel.RequisitionNo = dt.RequisitionNo;
            QCModel.BatchNo = _context.TblRebProductionProcess.Where(pro => pro.ProcessNo == dt.ProcessNo).FirstOrDefault().BatchNo;            
            var productCode =  _context.TblRebProcessOrder.Where(req => req.ProcessOrderNo == dt.LinkedProcessNo).FirstOrDefault().ProductCode;
            QCModel.ProductCode = productCode;
            QCModel.ProductName = _context.View_Product.Where(req => req.ProductCode == productCode).FirstOrDefault().ProductName;
            QCModel.PackSize = _context.View_Product.Where(req => req.ProductCode == productCode).FirstOrDefault().PackSize;
            QCModel.BatchSize = 0;
            QCModel.ProcessNo = dt.ProcessNo;
            QCModel.Qcqty = dt.Qcqty;
            QCModel.QcpassQty = dt.QcpassQty;
            QCModel.QcholdQty = dt.QcholdQty;
            QCModel.QcrejectQty = dt.QcrejectQty;
            QCModel.Sfgqcqty = dt.Sfgqcqty;
            QCModel.SfgqcpassQty = dt.SfgqcpassQty;
            QCModel.SfgqcrejectQty = dt.SfgqcrejectQty;
            QCModel.IsSendToFloorStockFg = dt.IsSendToFloorStockFg;
            QCModel.IsSendToFloorStockSfg = dt.IsSendToFloorStockSfg;
            QCModel.FgqcqtyBeforeConversion = dt.FgqcqtyBeforeConversion;
            QCModel.FgqcqtyConversionFactor = dt.FgqcqtyConversionFactor;
            QCModel.QcreferenceSampleQty = dt.QcreferenceSampleQty;
            QCModel.QcquarantineQty = dt.QcquarantineQty;
            QCModel.LinkedProcessNo= dt.LinkedProcessNo;

            ViewBag.ddlProcessList = new SelectList(_context.TblRebProductionProcess.Where(c => c.RequisitionNo == dt.RequisitionNo), "ProcessNo", "ProcessNo");

            List<TblQcparameterType> typeList = new List<TblQcparameterType>();
            typeList = (from c in _context.TblQcparameterType
                        select c).ToList();
            typeList.Insert(0, new TblQcparameterType { TypeCode = "0", TypeName = "Select Type" });
            ViewBag.ListOfType = typeList;


            //from qc in _context.TblRebQc
            //from qcd in _context.TblRebQcdetails
            //from qcp in _context.TblRebQcparameter
            //from po in _context.TblProcessOrder
            //where qc.Qcno == qcd.Qcno && qcp.QcparameterCode == qcd.QcparameterCode && qc.LinkedProcessNo == po.ProcessOrderNo && qc.Qcno==dt.Qcno
            //orderby qcd.QcparameterCode

            var qc_detail = from qc in _context.TblRebQc
                            from qcd in _context.TblRebQcdetails
                            from qcp in _context.TblQcparameter
                            from po in _context.TblProcessOrder
                            where qc.Qcno == qcd.Qcno && qcp.QcparameterCode == qcd.QcparameterCode && qc.LinkedProcessNo == po.ProcessOrderNo && qc.Qcno == dt.Qcno
                            orderby qcd.QcparameterCode
                            select new TblRebQcdetails
                                 {
                                     Id = qcd.Id,
                                     QcparameterCode = qcd.QcparameterCode,
                                     QcparameterName = qcp.QcparameterName,
                                     QcparameterStandardValue = qcp.QcparameterStandardValue,
                                     QcparameterActualValue = qcd.QcparameterActualValue,
                                     Comments = qcd.Comments
                                 };

            QCModel.TblRebQcdetails = qc_detail.ToList();

            return View(QCModel);
        }        



        [HttpPost, ActionName("UpdateQC")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQC(int id, TblRebQc qc_data)
        {
            if (id != qc_data.Id)
            {
                return NotFound();
            }
            try
            {
                //eFTestContext.Update(emp);
                //await eFTestContext.SaveChangesAsync();

                var qcdataToUpdate = await _context.TblRebQc.FirstOrDefaultAsync(s => s.Id == id);
                qcdataToUpdate.Edate = DateTime.Now;
                qcdataToUpdate.Euser = User.Identity.Name;

                if (await TryUpdateModelAsync<TblRebQc>(
                    qcdataToUpdate,
                    "",
                    s => s.Qcno, s => s.Qcdate, s => s.RequisitionNo, s => s.ProcessNo, s => s.Qcqty, s => s.QcpassQty, s => s.QcholdQty, s => s.QcrejectQty, 
                    s => s.Sfgqcqty, s => s.SfgqcpassQty, s => s.SfgqcrejectQty, s => s.Comments,s => s.QcquarantineQty))

                _context.TblRebQcdetails.RemoveRange(_context.TblRebQcdetails.Where(d => d.Qcno == qc_data.Qcno));

                if (qc_data.TblRebQcdetails != null)
                {
                    foreach (var item in qc_data.TblRebQcdetails.ToList())
                    {
                        TblRebQcdetails itemDetail = new TblRebQcdetails();
                        itemDetail.Qcno = qc_data.Qcno;
                        itemDetail.QcparameterCode = item.QcparameterCode;
                        itemDetail.QcparameterName = item.QcparameterName;
                        itemDetail.QcparameterStandardValue = item.QcparameterStandardValue;
                        itemDetail.QcparameterActualValue = item.QcparameterActualValue;
                        itemDetail.Comments = item.Comments;
                        await _context.AddAsync(itemDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                // Update tblFloorStock Table
                var fgCodeQuery = from p in _context.TblRebProductionProcess
                                  join r in _context.TblRebProcessOrder on p.LinkedProcessNo equals r.ProcessOrderNo
                                  where p.ProcessNo == qc_data.ProcessNo
                                  select new { ProductCode = r.ProductCode };
                string fgCode = fgCodeQuery.FirstOrDefault().ProductCode.ToString();

                var sfgCode = _context.TblRebProductionProcess.Where(c => c.ProcessNo == qc_data.ProcessNo).FirstOrDefault().Sfgcode;

                //if (qc_data.IsSendToFloorStockFg && qc_data.QcpassQty > 0)
                //{
                //    UpdateFloorStockFromProductionQC(fgCode.ToString(), qc_data.Qcno, qc_data.QcpassQty);
                //}

                //if (qc_data.IsSendToFloorStockSFG && qc_data.SFGQcpassQty > 0)
                //{
                //    UpdateFloorStockFromProductionQC(sfgCode.ToString(), qc_data.Qcno, qc_data.SFGQcpassQty);
                //}


                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(qc_data);
        }

        public IActionResult addProcessOrder()
        {

            //var requisitionModel = new List<TblRequisition>(from c in _context.TblRequisition
            //                                                from p in _context.View_Product
            //                                                where (c.ProductCode == p.ProductCode && c.ExtOfRequisitionNo == null) 
            //                                                orderby c.RequisitionDate descending
            //                                                select new TblRequisition
            //                                                {
            //                                                    RequisitionNo = c.RequisitionNo,
            //                                                    ProductCode = c.ProductCode,
            //                                                    ProductName = p.ProductName
            //                                                });

            var processOrderNoParam = new SqlParameter("@processOrderNo", "");
            var model = _context.VMRebSearchProcessOrder
                               .FromSql("EXEC sp_RebSearchProcessOrderForQC @processOrderNo", processOrderNoParam)
                               .ToList();
            //return View("_RequisitionAreaPartial", model);

            return PartialView("_RequisitionAreaPartial", model);
        }


        [HttpPost]
        public JsonResult SearchRequisition(string RequisitionNo)
        {
            //var model = new List<TblRequisition>(from req in _context.TblRequisition
            //                                     from b in _context.View_BOM
            //                                     from p in _context.View_Product
            //                                     where (req.ProductCode == b.ProductCode && b.ProductCode == p.ProductCode && (req.RequisitionNo.ToUpper().Contains(RequisitionNo.ToUpper())))
            //                                     select new TblRequisition
            //                                     {
            //                                         RequisitionNo = req.RequisitionNo,
            //                                         ProductCode = req.ProductCode,
            //                                         ProductName = p.ProductName
            //                                     });

            var requisitionNoParam = new SqlParameter("@requisitionNo", RequisitionNo == null ? "" : RequisitionNo);
            var model = _context.GetSearchRequisitionList
                               .FromSql("EXEC sp_SearchRequisitionForQC @requisitionNo", requisitionNoParam)
                               .ToList();

            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        [HttpPost]
        public JsonResult SearchProcessOrder(string ProcessOrderNo)
        {
            
            var processOrderNoParam = new SqlParameter("@processOrderNo", ProcessOrderNo == null ? "" : ProcessOrderNo);
            var model = _context.GetSearchProcessOrderList
                               .FromSql("EXEC sp_RebSearchProcessOrderForQC @processOrderNo", processOrderNoParam)
                               .ToList();
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        public JsonResult GetProcessNumber(string RequisitionNo)
        {
            var model = new List<TblRebProductionProcess>(from prop in _context.TblRebProductionProcess
                                                          from req in _context.TblRebProcessOrder
                                                          where (prop.LinkedProcessNo == req.ProcessOrderNo && (prop.LinkedProcessNo.ToUpper().Contains(RequisitionNo.ToUpper())))
                                                          select new TblRebProductionProcess
                                                          {
                                                              ProcessNo = prop.ProcessNo,
                                                              ProductionQty = prop.ProductionQty
                                                          });

            //var requisitionNoParam = new SqlParameter("@requisitionNo", RequisitionNo == null ? "" : RequisitionNo);
            //var model = _context.GetRequisitionWiseProcessList
            //                   .FromSql("EXEC sp_GetRequisitionWiseProcessNo @requisitionNo", requisitionNoParam)
            //                   .ToList();

            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }

        public JsonResult GetQCQuantity(string ProcessNo)
        {
            var model = new List<TblRebProductionProcess>(from prop in _context.TblRebProductionProcess where prop.ProcessNo==ProcessNo
                                                       select new TblRebProductionProcess
                                                       {
                                                           ProcessNo = prop.ProcessNo,
                                                           ProductionQty = prop.ProductionQty,
                                                           SfgproductionQty = prop.SfgproductionQty,
                                                           BatchNo = prop.BatchNo,
                                                           QcreferenceSampleQty = prop.QcreferenceSampleQty,
                                                           LumpQty = prop.LumpQty,
                                                       });
            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }


        [HttpPost]
        public JsonResult SetRequisitionInfomation(string ProcessOrderNo)
        {
            var keyVal = _context.TblRebProcessOrder.Where(c => c.ProcessOrderNo == ProcessOrderNo).ToList();
            if (keyVal != null)
            {
                ProcessOrderNo = keyVal.FirstOrDefault().ProcessOrderNo;
            }

            if (ProcessOrderNo != "")
            {
                var availableFloorStock =     from c in _context.TblRebProcessOrder
                                              join fs in _context.TblFloorStock on c.ProductCode equals fs.MaterialCode into ps
                                              from f in ps.DefaultIfEmpty()
                                              where (c.ProcessOrderNo == ProcessOrderNo)
                                              select new
                                              {
                                                  AvailableStock = (ps != null ? ps.Sum(x => x.AvailableStock) : 0)
                                              };


                var sa = new JsonSerializerSettings();
                var expertiesInfo = from c in _context.TblRebProcessOrder
                                    from p in _context.View_Product
                                    where(c.ProcessOrderNo == ProcessOrderNo && c.ProductCode == p.ProductCode)
                                    select new
                                    {
                                        c.ProcessOrderNo,
                                        c.BatchNo,
                                        c.ProductCode,
                                        p.ProductName,
                                        p.PackSize,
                                        AvailableStock = availableFloorStock.FirstOrDefault().AvailableStock   // (ps != null ? ps.Sum(x=>x.AvailableStock) : 0) // ps.Sum(x=> (double)x.AvailableStock ?? 0.00).Sum() 
                                    };
                return Json(expertiesInfo, sa);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetTypeWiseQCParameter(string type)
        {
            //var model = new List<TblRebQcdetails>(from para in _context.TblRebQcparameter where para.TypeCode == type
            //                                   select new TblRebQcdetails
            //                                   {
            //                                       Id = para.Id,
            //                                       QcparameterCode = para.QcparameterCode,
            //                                       QcparameterName = para.QcparameterName,
            //                                       QcparameterStandardValue = para.QcparameterStandardValue,
            //                                       QcparameterActualValue = "",
            //                                       Comments = ""
            //                                   });

            var model = new List<TblRebQcdetails>(from para in _context.TblQcparameter
                                               where para.TypeCode == type
                                               select new TblRebQcdetails
                                               {
                                                   Id = para.Id,
                                                   QcparameterCode = para.QcparameterCode,
                                                   QcparameterName = para.QcparameterName,
                                                   QcparameterStandardValue = para.QcparameterStandardValue,
                                                   QcparameterActualValue = "0",
                                                   Comments = ""
                                               });

            var sa = new JsonSerializerSettings();
            return Json(model, sa);
        }
    }
}