using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.jqGrid;
using CCARE.Models.General;
using CCARE.Models.SalesMarketing;
using NLog;
using System.Configuration;
using CCARE.App_Function;
using DevTrends.MvcDonutCaching;

namespace CCARE.Controllers
{
    public class LeadController : Controller
    {
        private CRMDb db = new CRMDb();
        string entityName = "lead";

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            int getFilter = int.Parse(Request["_filter"].ToString());
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            bool isAll = "2".Equals(getFilter) ? true : false;
            int State = getFilter;

            var model = db.lead.Where(x => x.DeletionStateCode == 0);
            //model = isAll ? model : model.Where(x => x.IsDisabled == isDisabled);
            if (State != 4)
            {
                model = model.Where(x => x.StateCode == getFilter);
            }

            if (!string.IsNullOrEmpty(getVal))
            {
                model = model.Where(x => x.Fullname.Contains(getVal) || x.Fullname.Contains(getVal) || x.Topic.Contains(getVal));
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "fullName":
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.Fullname) : model.OrderBy(x => x.Fullname);
                    break;
                case "topic":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Topic) : model.OrderBy(x => x.Topic);
                    break;
                case "name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Fullname) : model.OrderBy(x => x.Fullname);
                    break;
                //case "company":
                //    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Company) : model.OrderBy(x => x.Company);
                //    break;
                case "email":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.EmailAddress) : model.OrderBy(x => x.EmailAddress);
                    break;
                case "handphone":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.MobilePhone) : model.OrderBy(x => x.MobilePhone);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.Fullname) : model.OrderBy(x => x.Fullname);
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
                .OrderBy(x => x.Fullname)
                .Select(x => new
                {
                    Id = x.LeadID,
                    company = x.AccountId,
                    topic = x.Topic,
                    name = x.Fullname,
                    fullname = x.Fullname,
                    email = x.EmailAddress,
                    handphone = x.MobilePhone
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
            Guid logonUser = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            Lead lead = new Lead();
            lead.CreatedBy = logonUser;

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Edit(Guid id)
        {
            Lead model = db.lead.Find(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Lead model)
        {
            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult Edit(Lead model)
        {
            return formSubmit(model, "Edit");
        }

        public ActionResult formSubmit(Lead model, string actionType)
        {
            Guid logonUser = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            List<string> errorMessage = new List<string>();
            string successMessage = Resources.Lead.LeadSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                switch (actionType)
                {
                    case "Create":
                        //model.ID = Guid.NewGuid();
                        model.CreatedBy = logonUser;
                        model.CreatedOn = DateTime.Now;
                        results = db.Lead_Insert(model);
                        break;

                    case "Edit":
                        model.ModifiedBy = logonUser;
                        model.ModifiedOn = DateTime.Now;
                        results = db.Lead_Update(model);
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Lead", new { id = model.LeadID, success = successMessage });
                    string urlNew = u.Action("Create", "Lead");

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

        //Qualify lead and create customer record
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult setQualify(Guid id, Guid CustomerID)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.Lead_Qualify(id, CustomerID, CreatedBy);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("Edit", "Customer", new { id = CustomerID});

                var jsonData = new { flag = true, Message = Resources.Lead.LeadQualifySuccess , customerURL = url };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        //Change State Lead
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult changeState(Guid id, int StateCode)
        {
            Guid ModifiedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            
            results = db.Lead_ChangeState(id, StateCode, ModifiedBy);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Lead.LeadChangeStateSuccess };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
