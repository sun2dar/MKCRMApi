using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.Models.Role;
using CCARE.Models;
using System.Configuration;

namespace CCARE.Controllers
{
    public class ReportColumnController : Controller
    {
        CRMDb db = new CRMDb();
        //
        // GET: /ReportColumn/

        public ActionResult Index(Guid ReportID)
        {
            Report rp = db.report.Find(ReportID);

            List<EntityColumn> entlist = db.entityColumn.Where(x => x.EntityID == rp.EntityID)
                                                        .Where(x => x.IsColumn == true)
                                                        .OrderBy(x => x.Label).ToList();
            ViewBag.entity = entlist;

            return View(rp);
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            var data = db.reportColumn.Where(x => x.ReportID == new Guid(getVal)).OrderBy(x => x.SeqNo).ToList()

              .Select(x => new
              {
                  Id = x.ID,
                  Label = x.Label,
                  // Value = MappingSC(x.FilterValue, x.FilterType),
                  // Value = x.ValueText,
                  //Editable = x.IsEditable == 0 ? "No" : "Yes"
              });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.Skip((page - 1) * pageSize).Take(pageSize);

            var recordCount = data.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);

        }

        public ActionResult formSubmit(ReportColumn model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.DataSuccess;

            SessionForSP sessionParam = new SessionForSP();
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                // model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                var jsonData = new { flag = false, Message = "" };

                if (actionType == "Create")
                {
                    model.ID = Guid.NewGuid();
                    //  model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
                    jsonData = new { flag = true, Message = "Column Berhasil ditambahkan" };
                    results = db.ReportColumn_Insert(model);
                }
                else if (actionType == "Delete")
                {
                    jsonData = new { flag = true, Message = "Column Berhasil dihapus" };
                    results = db.ReportColumn_Delete(model);
                }
                else if (actionType == "SwapSeqNo")
                {
                    try
                    {
                        Guid ID1 = new Guid(TempData["ID1"].ToString());
                        int SeqNo1 = int.Parse(TempData["SeqNo1"].ToString());
                        Guid ID2 = new Guid(TempData["ID2"].ToString());
                        int SeqNo2 = int.Parse(TempData["SeqNo2"].ToString());

                        jsonData = new { flag = true, Message = "Swap Column Berhasil" };
                        results = db.ReportColumn_SwapSeqNo(ID1, SeqNo1, ID2, SeqNo2);
                    }
                    catch(Exception e)
                    {
                    }
                }

                //Update report
                ReportView reportModel = new ReportView();
                reportModel = db.reportView.Where(x => x.ID == model.ReportID).FirstOrDefault();
                reportModel.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                results = db.Report_Update(reportModel);

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    //string urlNew = u.Action("Edit?id="+model.ReportID+"&success="+results.Value, "MasterReport");
                    string urlNew = u.Action("Edit", "MasterReport", new { id = model.ReportID, success = results.Value });
                    return Json(jsonData);
                }
                else
                {
                    jsonData = new { flag = false, Message = "Swap Column Gagal" };
                    return Json(jsonData);
                }
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var error = ModelState[key].Errors.FirstOrDefault();
                    if (error != null)
                    {
                        errorMessage.Add(error.ErrorMessage);
                    }
                }
                var jsonData = new { flag = false, Message = errorMessage.First() };

                return Json(jsonData);
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            ReportColumn model = new ReportColumn();
            string ReportID = Request["reportid"];
            model.ReportID = new Guid(ReportID);
            model.ID = new Guid(Request["ID"]);

            int checkExistingColumn = db.reportColumn.Where(x => x.ReportID == model.ReportID).Count();
            if (checkExistingColumn == 1)
            {
                var jsonData = new { flag = false, Message = "Tidak dapat delete kolom terakhir. Kolom tidak boleh kosong" };
                return Json(jsonData);
            }

            return formSubmit(model, "Delete");
        }

        [HttpPost]
        public ActionResult Create()
        {
            ReportColumn model = new ReportColumn();
            string ReportID = Request["reportid"];
            string id = Request["ID"];
            model.ReportID = new Guid(ReportID);
            model.EntityColumnID = new Guid(id);

            var max = db.reportColumn.Where(x => x.ReportID == model.ReportID).Max(x => x.SeqNo);
            if (max == null)
            {
                max = 0;
            }
            model.SeqNo = max + 1;

            var check = db.reportColumn.Where(x => x.ReportID == model.ReportID)
                                       .Where(x => x.EntityColumnID == model.EntityColumnID);
            if (check.Count() > 0)
            {
                var jsonData = new { flag = false, Message = "Kolom yang dipilih sudah ada" };
                return Json(jsonData);
            }

            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult SwapUp(Guid ID, Guid ReportID)
        {
            ReportColumn model = new ReportColumn();
            var currentColumn = db.reportColumn.Find(ID);
            var tempColumnList = db.reportColumn.Where(x => x.ReportID == ReportID).OrderBy(x => x.SeqNo);
            List<ReportColumn> columList = new List<ReportColumn>();
            foreach (var x in tempColumnList)
            {
                if (x.ID != ID)
                    columList.Add(x);
                else
                    break;
            }

            if (columList.Count == 0)
            {
                return formSubmit(model, "SwapSeqNo");
            }

            var previousColumn = columList[columList.Count - 1];

            TempData["ID1"] = ID.ToString();
            TempData["SeqNo1"] = currentColumn.SeqNo;
            TempData["ID2"] = previousColumn.ID.ToString();
            TempData["SeqNo2"] = previousColumn.SeqNo;

            return formSubmit(model, "SwapSeqNo");
        }

        [HttpPost]
        public ActionResult SwapDown(Guid ID, Guid ReportID)
        {
            ReportColumn model = new ReportColumn();
            var currentColumn = db.reportColumn.Find(ID);
            var tempColumnList = db.reportColumn.Where(x => x.ReportID == ReportID).OrderByDescending(x => x.SeqNo);
            List<ReportColumn> columList = new List<ReportColumn>();
            foreach (var x in tempColumnList)
            {
                if (x.ID != ID)
                    columList.Add(x);
                else
                    break;
            }

            if (columList.Count == 0)
            {
                return formSubmit(model, "SwapSeqNo");
            }

            var previousColumn = columList[columList.Count - 1];

            TempData["ID1"] = ID.ToString();
            TempData["SeqNo1"] = currentColumn.SeqNo;
            TempData["ID2"] = previousColumn.ID.ToString();
            TempData["SeqNo2"] = previousColumn.SeqNo;

            return formSubmit(model, "SwapSeqNo");
        }
    }
}
