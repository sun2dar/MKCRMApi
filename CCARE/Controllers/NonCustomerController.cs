using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.Models.MasterData;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Data.Objects;

namespace CCARE.Controllers
{
    public class NonCustomerController : Controller
    {
        private CRMDb db = new CRMDb();
        string entityName = "NonCustomer";

        public ActionResult formSubmit(NonCustomer model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                
                model.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());
                switch (actionType)
                {
                    case "Create":
                        model.NonCustomerID = Guid.NewGuid();
                        model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
                        model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                        results = db.NonCustomer_Insert(model);
                        break;

                    case "Edit":
                        model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                        results = db.NonCustomer_Update(model);
                        break;

                    case "ChangeStatus":
                        model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                        results = db.NonCustomer_ChangeStatus(model);
                        break;

                    case "Delete":
                        model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                        results = db.NonCustomer_Delete(model);
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "NonCustomer", new { id = model.NonCustomerID, success = successMessage });
                    string urlNew = u.Action("Create", "NonCustomer");

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
            var model = db.noncustomer.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0);
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchFullName":
                        model = model.Where(x => x.FullName != null).Where("it.FullName.StartsWith(@0)", getVal);
                        break;
                    case "searchTelephoneNo":
                        model = model.Where(x => x.PhoneNo != null).Where("it.PhoneNo.StartsWith(@0)", getVal);
                        break;
                }
            }

            totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "FullName":
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.FullName) : model.OrderBy(x => x.FullName);
                    break;
                case "PhoneNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.PhoneNo) : model.OrderBy(x => x.PhoneNo);
                    break;
                case "CreatedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
                case "StateCode":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StateCode) : model.OrderBy(x => x.StateCode);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.FullName) : model.OrderBy(x => x.FullName);
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

            var data = model.Select(x => new
            {
                Id = x.NonCustomerID,
                FullName = x.FullName,
                PhoneNo = x.PhoneNo,
                CreatedOn = x.CreatedOn,
                StateCode = x.StateCode
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
        
        public ActionResult Create(string customerID = null, string customerName = null)
        {
            StringMapController dropDown = new StringMapController();

            NonCustomer model = new NonCustomer();
            model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
            model.CreatedByName = Session["Fullname"].ToString();
            model.Owner = new Guid(Session["CurrentUserID"].ToString());
            model.OwnerName = Session["Fullname"].ToString();
            model.CreatedOn = DateTime.Now;

            //buat hide save and new di form
            ViewBag.hideSaveAndOpen = true;

            //buat dropdown di form
            ViewBag.SL_AddressType = dropDown.getDropDown(entityName, "addresstype", model.AddressType == null ? 0 : (int)model.AddressType);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NonCustomer model, string TransactionTimeFormat = null)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            NonCustomer model = db.noncustomer.Find(id);
            StringMapController dropDown = new StringMapController();

            //buat hide save and new di form
            ViewBag.hideSaveAndOpen = true;

            //buat notes
            ViewBag.BusinessUnitID = model.BusinessUnitID;

            //buat dropdown di form
            ViewBag.SL_AddressType = dropDown.getDropDown(entityName, "addresstype", model.AddressType == null ? 0 : (int)model.AddressType);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(NonCustomer model)
        {
            return formSubmit(model, "Edit");
        }

        public ActionResult RequestList(Guid id)
        {
            NonCustomer model = db.noncustomer.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeStatus(NonCustomer model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(NonCustomer model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult CallWrapUpNonCustomer(Guid id)
        {
            NonCustomer model = db.noncustomer.Find(id);
            return View(model);
        }
    }
}
