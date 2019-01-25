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
    public class ReportMappingNewController : Controller
    {
        CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Reports(Guid RoleID)
        {
            ViewBag.RoleID = RoleID;

            var model = db.reportRoleNew.Where(x => x.ID == RoleID);
            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            string getRoleId = string.IsNullOrEmpty(Request["_roleId"]) ? Session["RoleID"].ToString() : Request["_roleId"];
            Guid roleId = new Guid(getRoleId);

            var model = db.reportRoleNew.Where(x => x.SecurityRoleId == roleId);

            model = string.IsNullOrEmpty(getVal) ? model : model.Where("it.report.Name.Contains(@0)", getVal);

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
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
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.report.Name) : model.OrderBy(x => x.report.Name);
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
                    ReportID = x.report.ID,
                    Name = x.report.Name,
                    Description = x.report.Description,
                    RoleID = x.SecurityRoleId,
                    RolesName = x.securityRole.Name
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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Execute(ReportRoleNew model, string action)
        {
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = Resources.ReportRole.ReportRoleSuccess;

            if (action == "Create")
            {
                if (db.reportRoleNew.Where(x => x.SecurityRoleId == model.SecurityRoleId).Where(x => x.ReportID == model.ReportID).Count() > 0)
                {
                    var jsonData = new { flag = false, Message = Resources.ReportRole.ReportRoleSame };
                    return Json(jsonData);
                }
            }

            if (action == "Delete")
            {
                message = Resources.NotifResource.PrivilegeExceptionDelSuccess;
            }

            switch (action)
            {
                case "Delete":
                    results = db.ReportRoleNew_Delete(model);
                    break;

                case "Create":
                    model.ID = Guid.NewGuid();
                    results = db.ReportRoleNew_Insert(model);
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
    }
}
