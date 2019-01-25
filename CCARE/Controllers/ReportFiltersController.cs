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
    public class ReportFiltersController : Controller
    {
        CRMDb db = new CRMDb();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(Guid ReportID)
        {
            ViewBag.ReportID = ReportID;
            var model = db.reportFilters.Where(x => x.ReportID == ReportID);
            return View(model);
        }

        public JsonResult GetFilters(string _reportID)
        {
            Guid ReportID = new Guid(_reportID);
            string[] exceptfilter = System.Configuration.ConfigurationManager.AppSettings["FilterDateException"].Split(';');
            var model =
                db.reportFilters
                .Where(x => x.ReportID == ReportID)
                .Where(x => !exceptfilter.Contains(x.FilterType))
                .Where(x => x.IsEditable == 0)
                .Select(x => new SelectListItem
                {
                    Text = x.FilterType == "StateCode" ? "State" :
                            x.FilterType == "StatusCode" ? "Status" :
                            x.FilterType == "CreatedBy" ? "Created By" :
                            x.FilterType == "PerformedByNames" ? "Performed By Names" :
                            x.FilterType == "SecurityRole" ? "Security Role" :
                            x.FilterType == "BusinessUnit" ? "Business Unit" :
                            x.FilterType == "IsDisabledStatus" ? "IsDisabled Status" :
                            x.FilterType,
                    Value = x.FilterType
                })
                .ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];

            if (getVal == "" || getVal == null)
            {
                getVal = Guid.Empty.ToString();
            }

            Guid ReportID = new Guid(getVal);

            var model = db.reportFilters.Where(x => x.ReportID == ReportID).Where(x => x.IsEditable == 1);

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Type":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.FilterType) : model.OrderBy(x => x.FilterType);
                    break;
                case "Value":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.FilterValue) : model.OrderBy(x => x.FilterValue);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.IsEditable) : model.OrderBy(x => x.IsEditable);
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
                    Id = x.ReportFiltersID,
                    Type = x.FilterType,
                    // Value = MappingSC(x.FilterValue, x.FilterType),
                    Value = x.FilterValue,
                    Editable = x.IsEditable == 0 ? "No" : "Yes"
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

        public string MappingSC(string txt, string mode) {
            if("StateCode".Equals(mode))
            {
                txt = "0".Equals(txt) ? "Open"
                    : "1".Equals(txt) ? "Completed"
                    : "2".Equals(txt) ? "Canceled"
                    : txt;
            }
            else if("StateCode".Equals(mode))
            {
                 txt = "0".Equals(txt) ? "Open"
                    : "1".Equals(txt) ? "New"
                    : "2".Equals(txt) ? "Assigned"
                    : "3".Equals(txt) ? "Owned"
                    : "4".Equals(txt) ? "Incomplete"
                    : "5".Equals(txt) ? "Closed"
                    : "6".Equals(txt) ? "Canceled"
                    : txt;
            }
            else if("IsDisabledStatus".Equals(mode))
            {
                txt = "0".Equals(txt) ? "Active"
                    : "1".Equals(txt) ? "Inactive"
                    : txt;
            }
            return txt;
        }


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Execute(ReportFilters model, string action)
        {
            Guid UserUpdate = new Guid(Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = Resources.ReportFilters.FilterSuccess;

            if (action == "Create")
            {
                if (model.IsEditable == 0) { 
                    string[] acceptFilter = System.Configuration.ConfigurationManager.AppSettings["AcceptMasterFilter"].Split(';');
                    if (!acceptFilter.Contains(model.FilterType)) {
                        var jsonData = new { flag = false, Message = Resources.ReportFilters.FilterForbidden };
                        return Json(jsonData);
                    }
                }

                var check = db.reportFilters.Where(x => x.ReportID == model.ReportID).Where(x => x.FilterType == model.FilterType);
                int count = "StatusCode".Equals(model.FilterType) || "StateCode".Equals(model.FilterType)?
                    check.Where(x => x.FilterValue == model.FilterValue).Count() :
                    check.Where(x => x.FilterGUID == model.FilterGUID).Count();

                if (count > 0)
                {
                    var jsonData = new { flag = false, Message = Resources.ReportFilters.FilterSame };
                    return Json(jsonData);
                }
            }

            if (action == "Delete")
            {
                message = Resources.ReportFilters.FilterDelSuccess;
            }

            switch (action)
            {
                case "Delete":
                    results = db.ReportFilters_Delete(model, UserUpdate);
                    break;

                case "Create":
                    model.ReportFiltersID = Guid.NewGuid();
                    results = db.ReportFilters_Insert(model, UserUpdate);
                    break;
            }

            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = message };
                return Json(jsonData);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData);
            }
        }

        public JsonResult GetAcceptedFilters()
        {
            string[] acceptFilter = System.Configuration.ConfigurationManager.AppSettings["AcceptMasterFilter"].Split(';');
            return Json(acceptFilter, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Master(Guid ReportID)
        {
            ViewBag.ReportID = ReportID;
            var model = db.reportFilters.Where(x => x.ReportID == ReportID).Where(x => x.IsEditable == 0);
            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadGridMaster(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];

            if (getVal == "" || getVal == null)
            {
                getVal = Guid.Empty.ToString();
            }

            Guid ReportID = new Guid(getVal);

            var model = db.reportFilters.Where(x => x.ReportID == ReportID).Where(x => x.IsEditable == 0);

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Type":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.FilterType) : model.OrderBy(x => x.FilterType);
                    break;
                case "Value":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.FilterValue) : model.OrderBy(x => x.FilterValue);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.IsEditable) : model.OrderBy(x => x.IsEditable);
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
                    Id = x.ReportFiltersID,
                    Type = x.FilterType,
                    Value = x.FilterValue,
                    Editable = x.IsEditable == 0 ? "No" : "Yes"
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

    }
}
