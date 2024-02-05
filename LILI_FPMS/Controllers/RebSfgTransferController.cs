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
using LILI_FPMS.Temp_Models;
using NPOI.Util;
using NPOI.OpenXmlFormats;
namespace LILI_FPMS.Controllers
{
    [Authorize]
    public class RebSfgTransferController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;
        public RebSfgTransferController(dbFormulationProductionSystemContext context)
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
            //          where ( s.ProductCode == p.ProductCode && s.TypeCode == "SFG")
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
            //              TransferNo = _context.TblRebSfgtransfer.Where(x=>x.RequisitionNo == s.RequisitionNo).Select(i=>i.TransferNo).FirstOrDefault(),
            //          };
            var req = _context.VMGetAllSFGTransfer.FromSql("EXEC sp_RebGetSFGTransferIndex");

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
            return View(await PaginatedList<VMGetAllSFGTransfer>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await employees.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Create(int Id)
        {
            TblRebSfgtransfer model = new TblRebSfgtransfer();
            if (!_context.TblRebRequisition.Any(x => x.Id == Id))
            {
                NotFound();
            }
            else 
            {
               
                TblRebRequisition reqModel = _context.TblRebRequisition.Where(x => x.Id == Id).First();
                List<TblRebRequisitionDetail> reqDetail = _context.TblRebRequisitionDetail.Where(x => x.RequisitionNo == reqModel.RequisitionNo).ToList();
                model.LinkedProcessNo = reqModel.LinkedProcessNo;
                model.RequisitionNo = reqModel.RequisitionNo;
                model.ProductCode = reqModel.ProductCode;
                model.ProductName = _context.View_Product.Where(x=>x.ProductCode==reqModel.ProductCode).Select(x=>x.ProductName).FirstOrDefault();
                model.TransferDate = DateTime.Now;
                model.ExtOfRequisitionNo = reqModel.ExtOfRequisitionNo;
                model.BatchNo = reqModel.BatchNo;
                model.BusinessCode = reqModel.BusinessCode;
                model.NumberOfBatch = reqModel.NumberOfBatch;
                model.Comments = reqModel.Comments;
                model.IssueStatus = reqModel.IssueStatus;
                model.TypeCode = reqModel.TypeCode;
                model.TransferNo = GenerateCode();
                foreach (var item in reqDetail)
                {
                    TblRebSfgtransferDetail itemdtl = new TblRebSfgtransferDetail();
                    itemdtl.RequisitionNo = item.RequisitionNo;
                    itemdtl.TransferNo = model.TransferNo;
                    itemdtl.MaterialCode = item.MaterialCode;
                    itemdtl.MaterialName = _context.View_Material.Where(x => x.MaterialCode == item.MaterialCode).Select(i=>i.MaterialName).First().ToString();
                    itemdtl.Unit = _context.View_Material.Where(x => x.MaterialCode == item.MaterialCode).Select(i => i.BaseUnit).First().ToString();
                    itemdtl.StandardRecipeQty = item.StandardRecipeQty;
                    itemdtl.FloorStock = item.FloorStock;
                    itemdtl.RequiredQty = item.RequiredQty;
                    itemdtl.AvailableStock = item.AvailableStock;
                    itemdtl.EstimatedQty = item.EstimatedQty;
                    model.TblRebSfgtransferDetail.Add(itemdtl);
                }
            }
            

           return View(model);
        }
        public async Task<IActionResult> EditView(string TransferNo)
        {
            TblRebSfgtransfer model = new TblRebSfgtransfer();
            if (!_context.TblRebSfgtransfer.Any(x => x.TransferNo == TransferNo))
            {
                NotFound();
            }
            else
            {
                model = _context.TblRebSfgtransfer.First(x => x.TransferNo == TransferNo);
                model.ProductName = _context.View_Product.Where(x => x.ProductCode == model.ProductCode).Select(x => x.ProductName).FirstOrDefault();
                model.TblRebSfgtransferDetail = _context.TblRebSfgtransferDetail.Where(x => x.TransferNo == TransferNo).ToList();
                foreach (var item in model.TblRebSfgtransferDetail)
                {
                    item.MaterialName = _context.View_Material.Where(x=>x.MaterialCode==item.MaterialCode).Select(i=>i.MaterialName).FirstOrDefault();
                    item.Unit = _context.View_Material.Where(x => x.MaterialCode == item.MaterialCode).Select(i => i.BaseUnit).FirstOrDefault();
                }
            }
        


            return View(model);
        }
        public async Task<ActionResult> CreateSfgTransfer(TblRebSfgtransfer model) {
            if (ModelState.IsValid)
            {
                if (_context.TblRebSfgtransfer.Any(x => x.TransferNo == model.TransferNo))
                {
                    model.TransferNo = GenerateCode();
                }
                model.Iuser = User.Identity.Name;
                model.Idate = DateTime.Now;
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        private string GenerateCode()
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var requisitionNo = "TRN-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblRebSfgtransfer.Where(c => c.TransferNo.Substring(0, 8) == requisitionNo).OrderByDescending(t => t.Id) select c.TransferNo.Substring(8, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            requisitionNo = "TRN-" + yy + mn + maxId;

            return requisitionNo;
        }
    }
}
