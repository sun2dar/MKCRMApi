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
    public class OrganizationClosureController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];
            int stateCode = "1".Equals(getFilter) ? 0 : 1;

            int totalRecords = 0;
            var model = db.organizationClosure.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0);

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchName":
                        model = model.Where("it.Name.Contains(@0)", getVal);
                        break;
                }
            }

            totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
                    break;
                case "StartDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StartDate) : model.OrderBy(x => x.StartDate);
                    break;
                case "EndDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.EndDate) : model.OrderBy(x => x.EndDate);
                    break;
                case "CreatedByName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedByName) : model.OrderBy(x => x.CreatedByName);
                    break;
                case "ModifiedByName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedByName) : model.OrderBy(x => x.ModifiedByName);
                    break;
                case "ModifiedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedOn) : model.OrderBy(x => x.ModifiedOn);
                    break;
                case "StateLabel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StateLabel) : model.OrderBy(x => x.StateLabel);
                    break;
                default:
                    model = model.OrderBy(x => x.Name);
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
                    Id = x.OrganizationClosureID,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByName,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedByName = x.ModifiedByName,
                    ModifiedOn = x.ModifiedOn,
                    StateLabel = x.StateLabel,
                    DeletionStateCode = x.DeletionStateCode,
                    StateCode = x.StateCode,
                    StatusCode = x.StatusCode
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
            OrganizationClosure organizationClosure = new OrganizationClosure();
            return View(organizationClosure);
        }

        [HttpPost]
        public ActionResult Create(OrganizationClosure model)
        {
            return formSubmit(model, "Create");
        }


        public ActionResult Edit(Guid id)
        {
            OrganizationClosure organizationClosure = db.organizationClosure.Find(id);
            return View(organizationClosure);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationClosure model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(OrganizationClosure model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(OrganizationClosure model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(OrganizationClosure organizationClosure, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                organizationClosure.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                if (actionType == "Create")
                {
                    organizationClosure.OrganizationClosureID = Guid.NewGuid();
                    results = db.OrganizationClosure_Insert(organizationClosure, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    results = db.OrganizationClosure_Update(organizationClosure);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.OrganizationClosure_ChangeStatus(organizationClosure);
                }
                else if (actionType == "Delete")
                {
                    results = db.OrganizationClosure_Delete(organizationClosure);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "OrganizationClosure", new { id = organizationClosure.OrganizationClosureID, success = successMessage });
                    string urlNew = u.Action("Create", "OrganizationClosure");

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
