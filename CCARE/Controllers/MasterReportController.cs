using CCARE.App_Function;
using CCARE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using CCARE.jqGrid;
using System.Configuration;

namespace CCARE.Controllers
{
    public class MasterReportController : Controller
    {
        private CRMDb db = new CRMDb();
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            var model = db.masterReport.Take(0);
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            if (string.IsNullOrEmpty(getVal))
            {
                model = db.masterReport;
            }
            else
            {
                model = db.masterReport.Where(x => x.ReportName.Contains(getVal) || x.RdlName.Contains(getVal) || x.Description.Contains(getVal));
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "ReportName":
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.ReportName) : model.OrderBy(x => x.ReportName);
                    break;
                case "RdlName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.RdlName) : model.OrderBy(x => x.RdlName);
                    break;
                case "Description":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Description) : model.OrderBy(x => x.Description);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.ReportName) : model.OrderBy(x => x.ReportName);
                    break;
            }

            if (model.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
            {
                page--;
                if (page < 1)
                {
                    page = 1;
                }
            }

            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            var data = model
                .Select(x => new
                {
                    Id = x.ReportID,
                    Name = x.ReportName,
                    Description = x.Description,
                    RdlName = x.RdlName
                });

            if (data.Count() == 0)
            {
                page--;
                if (page < 1)
                {
                    page = 1;
                }
                totalPages = page;
                totalRecords = page * 10 - data.Count();
            }

            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            MasterReport model = new MasterReport();
            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            MasterReport model = db.masterReport.Find(id);
            ViewBag.StatusLabel = model.Status == 1 ? "Active" : "Deactive";
            return View(model);
        }

        public ActionResult Filter(Guid ReportID)
        {
            ViewBag.ReportID = ReportID;
            var model = db.reportFilters.Where(x => x.ReportID == ReportID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MasterReport model)
        {
            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult Edit(MasterReport model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(MasterReport model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(MasterReport model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(MasterReport model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.DataSuccess;

            SessionForSP sessionParam = new SessionForSP();
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());

                if (actionType == "Create")
                {
                    model.ReportID = Guid.NewGuid();
                    results = db.MasterReport_Insert(model);
                }
                else if (actionType == "Edit")
                {
                    results = db.MasterReport_Update(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.MasterReport_ChangeStatus(model);
                }
                else if (actionType == "Delete")
                {
                    results = db.MasterReport_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "MasterReport", new { id = model.ReportID, success = successMessage });
                    string urlNew = u.Action("Create", "MasterReport");

                    var jsonData = new { flag = true, Message = url, urlNew = urlNew };
                    return Json(jsonData);
                }
                else
                {
                    var jsonData = new { flag = false, Message = results.Value.ToString() };
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
    }
}
