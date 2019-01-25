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
    public class CampaignController : Controller
    {
        private CRMDb db = new CRMDb();
        string entityName = "campaign";

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

            var model = db.campaign.Where(x => x.DeletionStateCode == 0);
            //model = isAll ? model : model.Where(x => x.IsDisabled == isDisabled);
            if (State != 2)
            {
                model = model.Where(x => x.StateCode == getFilter);
            }

            if (!string.IsNullOrEmpty(getVal))
            {
                model = model.Where(x => x.Name.Contains(getVal) || x.Description.Contains(getVal));
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "name":
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
                    break;
                case "code":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Code) : model.OrderBy(x => x.Code);
                    break;
                case "productName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ProductName) : model.OrderBy(x => x.ProductName);
                    break;
                case "campaignTypeName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CampaignTypeName) : model.OrderBy(x => x.CampaignTypeName);
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
                    code = x.Code,
                    productName = x.ProductName,
                    campaignTypeName = x.CampaignTypeName,
                    targetAudience = x.TargetAudience,
                    startDate = x.StartDate,
                    endDate = x.EndDate,
                    budget = x.Budget,
                    actualCost = x.ActualCost,
                    expectedRevenue = x.ExpectedRevenue,
                    actualRevenue = x.ActualRevenue,
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
            SelectList list = new SelectList(db.campaignType
                    .OrderBy(x => x.Name)
                    , "ID", "Name", 0);
            ViewBag.campaignType = list;
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Edit(Guid id)
        {
            Campaign model = db.campaign.Find(id);

            SelectList list = new SelectList(db.campaignType
                    .OrderBy(x => x.Name)
                    , "ID", "Name", model.CampaignType.Value.ToString());
            //var selected = list.Where(x => x.Value == model.CampaignTypeName).First();
            //selected.Selected = true;
            ViewBag.type = list;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Campaign model)
        {
            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult Edit(Campaign model)
        {
            return formSubmit(model, "Edit");
        }

        public ActionResult formSubmit(Campaign model, string actionType)
        {
            Guid logonUser = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            List<string> errorMessage = new List<string>();
            string successMessage = Resources.Campaign.CampaignSaveSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                switch (actionType)
                {
                    case "Create":
                        //model.ID = Guid.NewGuid();
                        model.CreatedBy = logonUser;
                        model.CreatedOn = DateTime.Now;
                        results = db.Campaign_Insert(model);
                        break;

                    case "Edit":
                        model.ModifiedBy = logonUser;
                        model.ModifiedOn = DateTime.Now;
                        results = db.Campaign_Update(model);
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Campaign", new { id = model.ID, success = successMessage });
                    string urlNew = u.Action("Create", "Campaign");

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

            results = db.Campaign_Delete(id, CreatedBy);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Campaign.CampaignDeleteSuccess };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CLead(Guid id)
        {
            Campaign model = db.campaign.Find(id);
            return View(model);
        }

        public JsonResult GetCLeads(string sidx, string sord, int rows, int page = 1)
        {
            string getCampaignID = Request["_ID"];

            var data = db.campaignLead.Where(x => x.CampaignID == new Guid(getCampaignID) && x.DeletionStateCode == 0).ToList()
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
            Guid getCampaignID = new Guid(Request["_ID"].ToString());

            //GET existing lead list
            var checkLead = db.campaignLead.Where(x => x.CampaignID == getCampaignID);

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

        //Add lead to Campaign
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doAddLead(Guid id, int type, Guid leadID, Guid campaignID)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            //0 untuk lead
            results = db.CampaignDetail_Insert(id, type, leadID, campaignID);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Campaign.CampaignLeadSuccess};
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        //Delete lead from campaign
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doDeleteLead(Guid id)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.CampaignDetail_Delete(id);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Campaign.CampaignLeadDelete };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        ///Customer tab//////////////////////////////////////////////
        public ActionResult CCustomer(Guid id)
        {
            Campaign model = db.campaign.Find(id);
            return View(model);
        }

        //Show list of customer in campaign
        public JsonResult GetCCustomer(string sidx, string sord, int rows, int page = 1)
        {
            string getCampaignID = Request["_ID"];

            var data = db.campaignCustomer.Where(x => x.CampaignID == new Guid(getCampaignID) && x.DeletionStateCode == 0).ToList()
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
            Guid getCampaignID = new Guid(Request["_ID"].ToString());

            //GET existing lead list
            var checkLead = db.campaignCustomer.Where(x => x.CampaignID == getCampaignID);

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

        //Add Customer to campaign
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doAddCustomer(Guid id, int type, Guid customerID, Guid campaignID)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.CampaignDetail_Insert(id, type, customerID, campaignID);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Campaign.CampaignCustomerSuccess};
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        //Delete Customer from campaign
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doDeleteCustomer(Guid id)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.CampaignDetail_Delete(id);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Campaign.CampaignCustomerDelete };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        ///Marketing List Tab///////////////////////////////////////////

        public ActionResult CMarketingList(Guid id)
        {
            Campaign model = db.campaign.Find(id);
            return View(model);
        }

        //Show list of marketing list in campaign
        public JsonResult GetCMarketingList(string sidx, string sord, int rows, int page = 1)
        {
            string getCampaignID = Request["_ID"];

            var data = db.campaignMarketingList.Where(x => x.CampaignID == new Guid(getCampaignID) && x.DeletionStateCode == 0).ToList()
                .Select(x => new
                {
                    Id = x.ID,
                    name = x.Name,
                    purpose = x.Purpose,
                    description = x.Description,
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

        //Show marketing list search
        public JsonResult GetMarketingList(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            Guid getCampaignID = new Guid(Request["_ID"].ToString());

            //GET existing lead list
            var checkMarketingList = db.campaignMarketingList.Where(x => x.CampaignID == getCampaignID);

            var model = db.marketingList.Where(x => x.DeletionStateCode == 0);
            model = model.Where(p => !checkMarketingList.Any(p2 => p2.EntityID == p.ID));

            if (!string.IsNullOrEmpty(getVal))
            {
                model = model.Where(x => x.Name.Contains(getVal) || x.Purpose.Contains(getVal) || x.Description.Contains(getVal));
            }

            var data = model
                .Where(x => x.DeletionStateCode == 0).ToList()
                .Select(x => new
                {
                    Id = x.ID,
                    name = x.Name,
                    purpose = x.Purpose,
                    description = x.Description
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

        //Add marketing list to campaign
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doAddMarketingList(Guid id, int type, Guid marketingListID, Guid campaignID)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.CampaignDetail_Insert(id, type, marketingListID, campaignID);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Campaign.CampaignMarketingListSuccess };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        //Delete marketing list from campaign
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult doDeleteMarketingList(Guid id)
        {
            Guid CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            results = db.CampaignDetail_Delete(id);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = Resources.Campaign.CampaignMarketingListDelete };
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
