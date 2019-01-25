using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.jqGrid;
using System.Configuration;

namespace CCARE.Models
{
    public class QueueController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            var model = db.queue.Where(x => x.QueueTypeCode == 1 && x.DeletionStateCode == 0);
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchName":
                        model = model.Where(x => x.Name != null).Where("it.Name.Contains(@0)", getVal);
                        break;
                    case "searchBusinessUnit":
                        model = model.Where(x => x.BusinessUnitName != null).Where("it.BusinessUnitName.Contains(@0)", getVal);
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
                case "eMail":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.EMailAddress) : model.OrderBy(x => x.EMailAddress);
                    break;
                case "BusinessUnitName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.BusinessUnitName_ORG) : model.OrderBy(x => x.BusinessUnitName_ORG);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
                    break;
            }

            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            var data = model
                .Select(x => new
                {
                    Id = x.QueueId,
                    Name = x.Name,
                    eMail = x.EMailAddress,
                    BusinessUnitName = x.BusinessUnitName_ORG,
                    DeletionStateCode = x.DeletionStateCode
                });

            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            Queue model = new Queue();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Queue model)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            Queue model = db.queue.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Queue model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult Delete(Queue model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(Queue model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());
            sessionParam.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                if (actionType == "Create")
                {
                    // Create new ID for Incident
                    model.QueueId = Guid.NewGuid();
                    results = db.spQueue_Insert(model, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    results = db.spQueue_Update(model, sessionParam);
                }
                else if (actionType == "Delete")
                {
                    results = db.spQueue_Delete(model, sessionParam);
                }
                else if (actionType == "ChangeStatus")
                {
                    // results = sp.spBusinessUnit_ChangeStatus(businessunit);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Queue", new { id = model.QueueId, success = successMessage });
                    string urlNew = u.Action("Create", "Queue");

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
