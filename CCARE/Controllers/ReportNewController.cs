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
    public class ReportNewController : Controller
    {
        //
        // GET: /ReportNew/
        CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            //var a = db.request.Where("CategoryID.Contains(@0)", new Guid[] { new Guid("687DC636-F276-E111-9CCF-00145E3DAE74"), new Guid("4EF267D3-824A-4B6A-BF39-8B43A3FF7BD4") });
            var a = new Guid?[] { new Guid("687DC636-F276-E111-9CCF-00145E3DAE74"), new Guid("4EF267D3-824A-4B6A-BF39-8B43A3FF7BD4") }.ToArray();
            
            //var awd = db.request.Where("CategoryID=@0", new Guid("687DC636-F276-E111-9CCF-00145E3DAE74")).ToString();
            //var awd = db.request.Where("CategoryID.Contains(@0)", a);
            //var awd = db.request.Where("CategoryID in @0", a);
            //var awd = db.request.Where("@0.Contains(outerIt.CategoryID)", a);
            //var awd = db.request.Where(x => a.Contains(x.CategoryID));
            //var awd = db.request.Where("CategoryID.Contains(@0)", a);

            //var awd = db.request.Where("CategoryID = cast(687dc636-f276-e111-9ccf-00145e3dae74 as uniqueidentifier)");
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            Guid RoleId = new Guid(Session["RoleID"].ToString());
            string getVal = Request["_val"];
            var model = db.reportRoleNew.Where(x => x.SecurityRoleId == RoleId).Take(0);
            Guid CurrentUser = new Guid(Session["CurrentUserID"].ToString());
            
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
                    Id = x.ReportID,
                    ReportID = x.ReportID,
                    Name = x.report.Name,
                    Description = x.report.Description
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

            ViewBag.ent = db.entity;

            ReportView model = new ReportView();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReportView model)
        {
            if (model.Hyperlink == null)
                model.Hyperlink = false;
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            ReportView model = db.reportView.Find(id);
            ViewBag.entity = db.entity.Where(x => x.ID == model.EntityID).FirstOrDefault();

            ViewBag.createdBy = db.systemUser.Where(x => x.SystemUserId == model.CreatedBy).FirstOrDefault().FullName;
            ViewBag.modifiedBy = db.systemUser.Where(x => x.SystemUserId == model.ModifiedBy).FirstOrDefault().FullName;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ReportView model)
        {
            if (model.Hyperlink == null)
                model.Hyperlink = false;

            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult Delete(Guid ID)
        {
            ReportView model = new ReportView();
            model.ID = ID;

            return formSubmit(model, "Delete");
        }

        [HttpPost]
        public ActionResult Duplicate(Guid ID)
        {
            ReportView model = db.reportView.Find(ID);

            return formSubmit(model, "Duplicate");
        }

        public ActionResult formSubmit(ReportView model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.DataSuccess;

            SessionForSP sessionParam = new SessionForSP();
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());

            Guid tempDuplicateID = Guid.Empty;
            
            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());

                if (actionType == "Create")
                {
                    model.ID = Guid.NewGuid();
                    model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
                    Guid SecurityRoleID = new Guid(Session["RoleID"].ToString());

                    results = db.Report_Insert(model, SecurityRoleID);

                    Guid entityID = new Guid(model.EntityID.ToString());
                    string entityType = db.entity.Where(x => x.ID == model.EntityID).FirstOrDefault().Name;

                    ReportFilter filter = new ReportFilter();
                    ReportColumn column = new ReportColumn();

                    List<string> defaultFilters = new List<string>();
                    List<string> defaultColumns = new List<string>();

                    defaultFilters.Add("CreatedOn");
                    switch (entityType)
                    {
                        case "Request":
                            defaultFilters.Add("CategoryID");
                            defaultColumns.Add("TicketNumber");
                            break;
                        case "Task":
                            defaultFilters.Add("WorkgroupID");
                            defaultColumns.Add("TaskNumber");
                            break;
                        case "SystemUser":
                            defaultColumns.Add("DomainName");
                            break;
                        case "CallWrapUp":
                            defaultColumns.Add("CallStartTime");
                            defaultColumns.Add("CallEndTime");
                            break;
                    }

                    //Insert default ReportFilter
                    for (int i = 0; i < defaultFilters.Count; i++)
                    {
                        filter = new ReportFilter();
                        filter.ReportID = new Guid(model.ID.ToString());
                        filter.ID = Guid.NewGuid();
                        string defaultValTemp = defaultFilters[i];
                        filter.EntityColumnID = db.entityColumn.Where(x => x.EntityID == entityID).Where(x => x.Name == defaultValTemp).FirstOrDefault().ID;
                        results = db.ReportFilter_Insert(filter);
                    }

                    //Insert default ReportColumn
                    for (int i = 0; i < defaultColumns.Count; i++)
                    {
                        column = new ReportColumn();
                        column.ReportID = new Guid(model.ID.ToString());
                        column.ID = Guid.NewGuid();
                        string defaultValTemp = defaultColumns[i];
                        column.EntityColumnID = db.entityColumn.Where(x => x.EntityID == entityID).Where(x => x.Name == defaultValTemp).FirstOrDefault().ID;
                        results = db.ReportColumn_Insert(column);
                    }
                }
                else if (actionType == "Edit")
                {
                    results = db.Report_Update(model);
                }
                else if (actionType == "Duplicate")
                {
                    Guid? tempID = model.ID;
                    model.ID = Guid.NewGuid();
                    string ReportID = model.ID.ToString();
                    tempDuplicateID = new Guid(ReportID);
                    model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
                    model.Name += " - Copy";
                    model.Hyperlink = model.Hyperlink == null ? false : model.Hyperlink;
                    Guid SecurityRoleID = new Guid(Session["RoleID"].ToString());

                    results = db.Report_Insert(model, SecurityRoleID);

                    var filter = db.reportFilter.Where(x => x.ReportID == tempID).OrderBy(x => x.Name).ToList();
                    var column = db.reportColumn.Where(x => x.ReportID == tempID).OrderBy(x => x.SeqNo).ToList();

                    foreach (var x in filter)
                    {
                        ReportFilter a = new ReportFilter();
                        a = x;
                        a.ID = Guid.NewGuid();
                        a.ReportID = new Guid(ReportID);
                        results = db.ReportFilter_Insert(a);
                    }

                    foreach (var x in column)
                    {
                        ReportColumn a = new ReportColumn();
                        a = x;
                        a.ID = Guid.NewGuid();
                        a.ReportID = new Guid(ReportID);
                        results = db.ReportColumn_Insert(a);
                    }
                }
                else if (actionType == "Delete")
                {
                    results = db.Report_Delete(model);
                }
                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "ReportNew", new { id = model.ID, success = successMessage });
                    string urlNew = u.Action("Create", "ReportNew");

                    var jsonData = new { flag = true, Message = url, urlNew = urlNew, ID = tempDuplicateID };
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
