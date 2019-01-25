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

namespace CCARE.Controllers
{
    public class MarketingListController : Controller
    {
        private CRMDb db = new CRMDb();
        string entityName = "MarketingList";

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

            var model = db.marketingList.Where(x => x.DeletionStateCode == 0);
            //model = isAll ? model : model.Where(x => x.IsDisabled == isDisabled);
            if (State != 2)
            {
                model = model.Where(x => x.StateCode == getFilter);
            }

            if (!string.IsNullOrEmpty(getVal))
            {
                model = model.Where(x => x.Name.Contains(getVal) || x.Purpose.Contains(getVal) || x.Description.Contains(getVal));
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "name":
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
                    break;
                case "purpose":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Purpose) : model.OrderBy(x => x.Purpose);
                    break;
                case "description":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Description) : model.OrderBy(x => x.Description);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
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
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Id = x.ID,
                    name = x.Name,
                    purpose = x.Purpose,
                    description = x.Description
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

            MarketingList ml = new MarketingList();
            ml.CreatedBy = logonUser;

            return View();
        }


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Edit(Guid id)
        {
            MarketingList model = db.marketingList.Find(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MarketingList model)
        {
            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult Edit(MarketingList model)
        {
            return formSubmit(model, "Edit");
        }

        public ActionResult formSubmit(MarketingList model, string actionType)
        {
            Guid logonUser = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            List<string> errorMessage = new List<string>();
            string successMessage = Resources.MarketingList.MarketingListSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                switch (actionType)
                {
                    case "Create":
                        //model.ID = Guid.NewGuid();
                        model.CreatedBy = logonUser;
                        model.CreatedOn = DateTime.Now;
                        results = db.MarketingList_Insert(model);
                        break;

                    case "Edit":
                        model.ModifiedBy = logonUser;
                        model.ModifiedOn = DateTime.Now;
                        results = db.MarketingList_Update(model);
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "MarketingList", new { id = model.ID, success = successMessage });
                    string urlNew = u.Action("Create", "MarketingList");

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
        public ActionResult doDelete(Guid id)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.MarketingList_Delete(id, CreatedBy);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.MarketingList.MarketingListDelete};
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MLead(Guid id)
        {
            MarketingList model = db.marketingList.Find(id);
            return View(model);
        }

        public JsonResult GetMLeads(string sidx, string sord, int rows, int page = 1)
        {
            string getMarketingListID = Request["_ID"];

            var data = db.marketingListLead.Where(x => x.MarketingListID == new Guid(getMarketingListID) && x.DeletionStateCode == 0).ToList()
                .Select(x => new
                {
                    Id = x.ID,
                    Name = x.Name,
                    Topic = x.Topic,
                    Fullname = x.Fullname,
                    Email = x.Email,
                    EntityID = x.EntityID
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

        //Show lead
        public JsonResult GetLead(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            Guid getMarketingListID = new Guid(Request["_ID"].ToString());

            //GET existing lead list
            var checkLead = db.marketingListLead.Where(x => x.MarketingListID == getMarketingListID);

            var model = db.lead.Where(x => x.DeletionStateCode == 0);
            model = model.Where(p => !checkLead.Any(p2 => p2.EntityID == p.LeadID));

            if (!string.IsNullOrEmpty(getVal))
            {
                model = model.Where(x => x.Fullname.Contains(getVal) || x.Fullname.Contains(getVal) || x.Topic.Contains(getVal));
            }

            var data = model
                .Where(x => x.DeletionStateCode == 0).ToList()
                .Select(x => new
                {
                    Id = x.LeadID,
                    Name = x.Fullname,
                    Topic = x.Topic,
                    Fullname = x.Fullname,
                    Email = x.EmailAddress
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

        //Add lead to marketing list
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doAddLead(Guid id, int type, Guid leadID, Guid marketingListID)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.MarketingListDetail_Insert(id, 0, leadID, marketingListID);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.MarketingList.MarketingListLeadSuccess };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        //Delete lead from marketing list
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doDeleteLead(Guid id)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.MarketingListDetail_Delete(id);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.MarketingList.MarketingListLeadDelete };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MCustomer(Guid id)
        {
            MarketingList model = db.marketingList.Find(id);
            return View(model);
        }

        //Show list of customer in marketing list
        public JsonResult GetMCustomer(string sidx, string sord, int rows, int page = 1)
        {
            string getMarketingListID = Request["_ID"];

            var data = db.marketingListCustomer.Where(x => x.MarketingListID == new Guid(getMarketingListID) && x.DeletionStateCode == 0).ToList()
                .Select(x => new
                {
                    Id = x.ID,
                    cisnumber = x.CisNumber,
                    fullname = x.Fullname,
                    customertypelabel = x.CustomerTypeLabel,
                    email = x.Email,
                    EntityID = x.EntityID
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

        //Show customer search
        public JsonResult GetCustomer(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            Guid getMarketingListID = new Guid(Request["_ID"].ToString());

            //GET existing lead list
            var checkLead = db.marketingListCustomer.Where(x => x.MarketingListID == getMarketingListID);

            var model = db.customer.Where(x => x.DeletionStateCode == 0);
            model = model.Where(p => !checkLead.Any(p2 => p2.EntityID == p.CustomerID));

            if (!string.IsNullOrEmpty(getVal))
            {
                model = model.Where(x => x.CISNumber.Contains(getVal) || x.FullName.Contains(getVal) || x.CustomerTypeLabel.Contains(getVal));
            }

            var data = model
                .Where(x => x.DeletionStateCode == 0).ToList()
                .Select(x => new
                {
                    Id = x.CustomerID,
                    fullname = x.FullName,
                    cisnumber = x.CISNumber,
                    Email = x.EMailAddress1
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

        //Add Customer to marketing list
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doAddCustomer(Guid id, int type, Guid customerID, Guid marketingListID)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.MarketingListDetail_Insert(id, 1, customerID, marketingListID);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.MarketingList.MarketingListCustomerSuccess };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        //Delete Customer from marketing list
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doDeleteCustomer(Guid id)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.MarketingListDetail_Delete(id);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.MarketingList.MarketingListCustomerDelete };
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
