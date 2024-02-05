using LILI_FPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_FPMS.Controllers
{
    [Authorize]
    public class StiList
    { 
        public string value {  get; set; }
        public string text { get; set; }
    }
    public class RMPMSFGReceiveController : Controller
    {
        private readonly dbFormulationProductionSystemContext _context;
        public RMPMSFGReceiveController(dbFormulationProductionSystemContext context)
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


            IQueryable<TblRebRmpmsfgreceive> fgtn_data = _context.TblRebRmpmsfgreceive;


            if (!String.IsNullOrEmpty(searchString))
            {
                fgtn_data = fgtn_data.Where(s => s.ReceiveNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    fgtn_data = fgtn_data.OrderByDescending(s => s.ReceiveNo);
                    break;

                default:
                    fgtn_data = fgtn_data.OrderByDescending(s => s.ReceiveNo);
                    break;
            }
            int pageSize = 7;


            return View(await PaginatedList<TblRebRmpmsfgreceive>.CreateAsync(fgtn_data.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public IActionResult Create()
        {

            TblRebRmpmsfgreceive entities = new TblRebRmpmsfgreceive();

            entities.ReceiveNo = GenerateReceiveNo();
            entities.ReceiveDate = DateTime.Now;

            var StiFgList = (from c in _context.TblRebStifgreceive
                            select new StiList
                            {
                                value = c.Stino,
                                text = c.Stino
                            }).GroupBy(item => item.value)
                              .Select(group => group.First()) 
                              .ToList(); 

            StiFgList.Insert(0, new StiList { value="",text="<--Select STI-->"});
            ViewBag.StiList = StiFgList.Distinct().ToList();

            return View(entities);
        }

        public async Task<ActionResult> CreateReceive(TblRebRmpmsfgreceive model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var CodeExists = _context.TblRebRmpmsfgreceive.Any(x => x.ReceiveNo == model.ReceiveNo);
                    if (CodeExists)
                    {
                        model.ReceiveNo = GenerateReceiveNo();
                    }
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;

                    var existingEntity = _context.TblRebRmpmsfgreceive.FirstOrDefault(x => x.ReceiveNo == model.ReceiveNo);

                    var details = model.TblRebRmpmsfgreceiveDetail.ToList();

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
            TblRebRmpmsfgreceive fgtnModel = new TblRebRmpmsfgreceive();
            var dt = _context.TblRebRmpmsfgreceive.Where(s => s.Id == Id).First();
            fgtnModel.Id = dt.Id;
            fgtnModel.BatchNo = dt.BatchNo;
            fgtnModel.ReceiveNo = dt.ReceiveNo;
            fgtnModel.ReceiveDate = dt.ReceiveDate;
            fgtnModel.Stino = dt.Stino;
            fgtnModel.Comments = dt.Comments;
            fgtnModel.RequisitionQty= dt.RequisitionQty;
            fgtnModel.TblRebRmpmsfgreceiveDetail = _context.TblRebRmpmsfgreceiveDetail.Where(x => x.ReceiveNo == fgtnModel.ReceiveNo).AsNoTracking().ToList();

            var StiFgList = (from c in _context.TblRebStifgreceive
                             select new StiList
                             {
                                 value = c.Stino,
                                 text = c.Stino
                             }).GroupBy(item => item.value)
                        .Select(group => group.First())
                        .ToList();

            StiFgList.Insert(0, new StiList { value = "", text = "<--Select STI-->" });
            ViewBag.StiList = StiFgList.Distinct().ToList();

            return View(fgtnModel);
        }
        public async Task<IActionResult> UpdateReceive( TblRebRmpmsfgreceive fgtnData)
        {
            var id = fgtnData.Id;
            if (!_context.TblRebRmpmsfgreceive.Any(x=>x.Id == id))
            {
                return NotFound();
            }
            try
            {
                var fgtnDataToUpdate = await _context.TblRebRmpmsfgreceive.FirstOrDefaultAsync(s => s.Id == id);
                fgtnDataToUpdate.Edate = DateTime.Now;
                fgtnDataToUpdate.Euser = User.Identity.Name;

                fgtnDataToUpdate.Comments = fgtnData.Comments;
                fgtnDataToUpdate.RequisitionQty = fgtnData.RequisitionQty;
                fgtnDataToUpdate.ReceiveDate= fgtnData.ReceiveDate;
                fgtnDataToUpdate.Stino = fgtnData.Stino;
                fgtnDataToUpdate.BatchNo = fgtnData.BatchNo;

                if (await TryUpdateModelAsync<TblRebRmpmsfgreceive>(
                    fgtnDataToUpdate,
                    "",
                     s => s.RequisitionQty, s => s.BatchNo, s => s.ReceiveDate, s => s.Stino, s => s.Comments)) ;

                _context.TblRebRmpmsfgreceiveDetail.RemoveRange(_context.TblRebRmpmsfgreceiveDetail.Where(d => d.ReceiveNo == fgtnData.ReceiveNo));

                if (fgtnData.TblRebRmpmsfgreceiveDetail != null)
                {
                    foreach (var item in fgtnData.TblRebRmpmsfgreceiveDetail.ToList())
                    {
                        TblRebRmpmsfgreceiveDetail itemDetail = new TblRebRmpmsfgreceiveDetail();
                        itemDetail.ReceiveNo = fgtnData.ReceiveNo;
                        itemDetail.Rmpmsfgcode = item.Rmpmsfgcode;
                        itemDetail.Rmpmsfgname = item.Rmpmsfgname;
                        itemDetail.Unit = item.Unit;
                        itemDetail.ReceiveQty = item.ReceiveQty;
                        itemDetail.Comments = item.Comments;
                        await _context.AddAsync(itemDetail); 
                        await _context.SaveChangesAsync();
                    }
      
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(fgtnData);
        }

        public bool Delete(int id)
        {
            try
            {
                TblRebRmpmsfgreceive data = _context.TblRebRmpmsfgreceive.Where(s => s.Id == id).First();
                _context.TblRebRmpmsfgreceive.RemoveRange(_context.TblRebRmpmsfgreceive.Where(d => d.ReceiveNo == data.ReceiveNo));
                _context.TblRebRmpmsfgreceive.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IActionResult AddMaterialSearch()
        {
            TblRebRmpmsfgreceiveDetail materialSearchModel = new TblRebRmpmsfgreceiveDetail();
            return PartialView("_MaterialSearchPartial", materialSearchModel);
        }

        [HttpPost]
        public JsonResult SearchMaterial(string MaterialSearchKey)
        {
            var model = new List<TblRebRmpmsfgreceiveDetail>(from b in _context.View_Material
                                                       where ((b.MaterialCode.ToUpper().Contains(MaterialSearchKey.ToUpper()) || b.MaterialName.ToUpper().Contains(MaterialSearchKey.ToUpper())))
                                                       select new TblRebRmpmsfgreceiveDetail
                                                       {
                                                           MaterialCode = b.MaterialCode,
                                                           MaterialName = b.MaterialName,
                                                           Unit = b.BaseUnit
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


        public string GenerateReceiveNo()
        {
            //Generate FGTN No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var FGTNNo = "RPS-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblRebRmpmsfgreceive.Where(c => c.ReceiveNo.Substring(0, 8) == FGTNNo).OrderByDescending(t => t.Id) select c.ReceiveNo.Substring(8, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            FGTNNo = "RPS-" + yy + mn + maxId;

            return FGTNNo;


        }
    }
}
