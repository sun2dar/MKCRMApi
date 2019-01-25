using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using CCARE.Models;
using CCARE.Models.General;
using Newtonsoft.Json;

namespace CCARE.Controllers
{
    public class ReportData
    {
        public List<ReportFilter> reportFilters { get; set; }
        public List<ReportColumn> reportColumns { get; set; }
        public string entity { get; set; }
        public bool? Hyperlink { get; set; }
    }

    public class ReportViewController : Controller
    {
        CRMDb db = new CRMDb();

        //
        // GET: /ReportView/

        public ActionResult Index()
        {
            ViewBag.ReportBU = db.businessunit.Find(Session["BusinessUnitID"]).Name;

            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            Guid RoleId = new Guid(Session["RoleID"].ToString());
            string getVal = Request["_val"];
            var model = db.reportRoleNew.Where(x => x.SecurityRoleId == RoleId).Take(0);
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());


            if (string.IsNullOrEmpty(getVal))
            {
                model = db.reportRoleNew.Where(x => x.SecurityRoleId == RoleId);
            }
            else
            {
                model = db.reportRoleNew.Where(x => x.SecurityRoleId == RoleId).Where(x => x.report.Name.Contains(getVal) || x.report.Description.Contains(getVal));
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.report.Name) : model.OrderBy(x => x.report.Name);
                    break;
                case "Description":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.report.Description) : model.OrderBy(x => x.report.Description);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.report.Name) : model.OrderBy(x => x.report.Name);
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
                    Id = x.ID,
                    Name = x.report.Name,
                    Description = x.report.Description,
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

        public IEnumerable<BaseAttribute> getDropdownItem(string entity, string name)
        {
            var result = db.pickList
                           .Where(x => x.EntityName == entity && x.AttributeName == name)
                           .OrderBy(x => x.DisplayOrder)
                           .ToList()
                           .Select(x => new BaseAttribute
                           {
                               Text = x.label,
                               Value = x.AttributeValue.ToString()
                           });

            return result;
        }

        public JsonResult setDropdownItem(string entity, string name)
        {
            var data = new SelectList(getDropdownItem(entity, name), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getReportData(string _reportId)
        {
            Guid reportId = new Guid(string.IsNullOrEmpty(_reportId) ? "" : _reportId);

            List<ReportFilter> reportFiltersTemp = db.reportFilter.Where(x => x.ReportID == reportId).OrderBy(x => x.Name).ToList();
            List<ReportFilter> reportFilters = new List<ReportFilter>();
            foreach (var x in reportFiltersTemp)
            {
                if (x.DataType == "guid")
                {
                    if (x.ValueID != null)
                        reportFilters.Add(x);
                }
                else
                {
                    reportFilters.Add(x);
                }
            }
            
            List<ReportColumn> reportColumns = db.reportColumn.Where(x => x.ReportID == reportId).OrderBy(x => x.SeqNo).ToList();
            ReportData reportData = new ReportData();
            reportData.reportFilters = reportFilters;
            reportData.reportColumns = reportColumns;
            Report report = db.report.Where(x => x.ID == reportId).FirstOrDefault();
            reportData.entity = db.entity.Where(x => x.ID == report.EntityID).FirstOrDefault().Name;
            reportData.Hyperlink = report.Hyperlink;

            return Json(reportData, JsonRequestBehavior.AllowGet);
        }
    }
}
