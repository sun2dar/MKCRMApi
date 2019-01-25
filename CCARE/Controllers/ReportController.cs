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
using System.Collections.Specialized;
using Newtonsoft.Json;


namespace CCARE.Controllers
{
    public class ReportController : Controller
    {
        CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            ViewBag.ReportBU = db.businessunit.Find(Session["BusinessUnitID"]).Name;
            
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            Guid RoleId = new Guid(Session["RoleID"].ToString());
            string getVal = Request["_val"];
            var model = db.reportRole.Where(x => x.SecurityRoleId == RoleId).Take(0);
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());


            if (string.IsNullOrEmpty(getVal))
            {
                model = db.reportRole.Where(x => x.SecurityRoleId == RoleId);
            }
            else
            {
                model = db.reportRole.Where(x => x.SecurityRoleId == RoleId).Where(x => x.masterReport.ReportName.Contains(getVal) || x.masterReport.Description.Contains(getVal));
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.masterReport.ReportName) : model.OrderBy(x => x.masterReport.ReportName);
                    break;
                case "Description":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.masterReport.Description) : model.OrderBy(x => x.masterReport.Description);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.masterReport.ReportName) : model.OrderBy(x => x.masterReport.ReportName);
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
                    Id = x.ReportRoleID,
                    Name = x.masterReport.ReportName,
                    Description = x.masterReport.Description,
                    RdlName = x.masterReport.RdlName,
                    ReportID = x.ReportID
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

        public JsonResult GetFilters(string _reportID)
        {
            Guid ReportID = new Guid(_reportID);
            var model = db.reportFilters
                .Where(x => x.ReportID == ReportID)
                .Where(x => x.IsEditable == 0)
                .Select(x => new SelectListItem{ Text = x.FilterType, Value = x.FilterType })
                .ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFiltersValue(string _reportID)
        {
            Guid ReportID = new Guid(_reportID);
            var data = db.reportFilters.Where(x => x.ReportID == ReportID).Where(x => x.IsEditable == 1).GroupBy(x => x.FilterType);
            List<FilterValues> model = new List<FilterValues>();
            
            foreach (var group in data)
            {
                FilterValues record = new FilterValues();
                record.key = group.Key.ToString();
                record.gid = string.Empty;
                record.val = string.Empty;
                foreach (var entry in group)
                {
                    record.gid = record.gid == string.Empty ? record.gid : record.gid + ",";
                    record.val = record.val == string.Empty ? record.val : record.val + ",";

                    record.gid += string.IsNullOrEmpty(entry.FilterGUID.ToString()) ? "-" : entry.FilterGUID.ToString();
                    record.val += string.IsNullOrEmpty(entry.FilterValue.ToString()) ? "-" : entry.FilterValue.ToString();
                }
                model.Add(record);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<BaseAttribute> queryList(string entityName, string attributeName)
        {
            var result = db.pickList.OrderBy(x => x.DisplayOrder)
                            .Where(x => x.EntityName == entityName)
                            .Where(x => x.AttributeName == attributeName)
                            .ToList()
                            .Select(x => new BaseAttribute
                            {
                                Text = x.label,
                                Value = x.AttributeValue.ToString()
                            });

            return result;
        }

        public JsonResult getSelectList(string entityName, string attributeName)
        {
            var data = new SelectList(queryList(entityName, attributeName), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
