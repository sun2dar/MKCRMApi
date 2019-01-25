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
    public class ReportAccessController : Controller
    {
        CRMDb db = new CRMDb();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(Guid ReportID)
        {
            Report report = db.report.Find(ReportID);
            ViewBag.businessUnitId = new Guid(Session["BusinessUnitID"].ToString());

            return View(report);
        }

        public ActionResult formSubmit(ReportRoleNew model, string actionType)
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
                    jsonData = new { flag = true, Message = "Role berhasil ditambahkan" };
                    results = db.ReportRoleNew_Insert(model);
                }
                else if (actionType == "Delete")
                {
                    jsonData = new { flag = true, Message = "Role berhasil dihapus" };
                    results = db.ReportRoleNew_Delete(model);
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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];

            Guid reportID = new Guid(getVal);
            Guid roleID = new Guid(Session["RoleID"].ToString());
            var data = db.reportRoleView.Where(x => x.ReportID == reportID)
                                        .OrderBy(x => x.SecurityRoleName).ToList()

              .Select(x => new
              {
                  Id = x.ID,
                  Name = x.SecurityRoleName
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

        [HttpPost]
        public ActionResult Create()
        {
            ReportRoleNew model = new ReportRoleNew();
            string ReportID = Request["reportid"];
            string id = Request["ID"];
            model.ReportID = new Guid(ReportID);
            model.SecurityRoleId = new Guid(id);

            var check = db.reportRoleView
                            .Where(x => x.ReportID == model.ReportID)
                            .Where(x => x.SecurityRoleId == model.SecurityRoleId);

            if (check.Count() > 0)
            {
                var jsonData = new { flag = false, Message = "Report sudah mendapatkan akses untuk Report ini." };
                return Json(jsonData);
            }

            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult Delete()
        {
            ReportRoleNew model = new ReportRoleNew();
            string ReportID = Request["reportid"];
            model.ReportID = new Guid(ReportID);
            model.ID = new Guid(Request["ID"]);

            return formSubmit(model, "Delete");
        }
    }
}
