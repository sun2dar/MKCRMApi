using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using System.Configuration;

namespace CCARE.Controllers
{
    public class CallWrapUpController : Controller
    {
        private CRMDb db = new CRMDb();



        public ActionResult formSubmit(CallWrapUp model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());
          
            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                if (actionType == "Create")
                {
                    model.CallWrapUpID = Guid.NewGuid();
                    results = db.spCallWrapUp_Insert(model, sessionParam);                    
                }
                else if (actionType == "Edit")
                {
                    results = db.spCallWrapUp_Update(model, sessionParam);
                }
                else if (actionType == "ChangeStatus")
                {
                    //results = db.spCallWrapUp_ChangeStatus(callwrapup);
                }
                else if (actionType == "Delete")
                {
                    //results = db.spCallWrapUp_Delete(callwrapup);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "CallWrapUp", new { id = model.CallWrapUpID, success = successMessage });
                    string urlNew = u.Action("Create", "CallWrapUp");

                    var jsonData = new { flag = true, Message = url, urlNew = urlNew };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var jsonData = new { flag = false, Message = results.Value.ToString() };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult formSubmit2(CallType model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                if (actionType == "CreateCallType")
                {
                    model.CallTypeID = Guid.NewGuid();
                    model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                    model.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());
                    results = db.spCallType_Insert(model);
                }
                else if (actionType == "EditCallType")
                {
                    model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                    model.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());
                    results = db.spCallType_Update(model);
                }
                else if (actionType == "Delete")
                {
                    results = db.spCallType_Delete(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.spCallType_ChangeStatus(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url2 = u.Action("EditCallType", "CallWrapUp", new { id = model.CallTypeID, success = successMessage });
                    string urlNew2 = u.Action("CreateCallType", "CallWrapUp");

                    var jsonData = new { flag = true, Message = url2, urlNew = urlNew2 };
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

        //Automatic create call wrapup when click new call wrapup
        public ActionResult autoCreateCwu(Guid customerID, Guid ownerID, DateTime startCallTime, string flag)
        {
            CallWrapUp callwrapup = new CallWrapUp();
            if (flag == "customer")
            {
                callwrapup.CustomerID = customerID;
            }
            else if (flag == "noncustomer")
            {
                callwrapup.NonCustomerID = customerID;
            }


            if (startCallTime == null || flag == "")
            {
                return RedirectToAction("Create", "CallWrapUp");
            }

            callwrapup.CreatedBy = ownerID;
            callwrapup.CallStartTime = startCallTime;
            callwrapup.CallWrapUpID = Guid.NewGuid();

            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, ""); 
            results = db.spCallWrapUp_Insert(callwrapup, sessionParam); 

            //Success
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                CallWrapUp findCwu = db.callwrapup.Find(callwrapup.CallWrapUpID);

                TempData["CWUID"] = callwrapup.CallWrapUpID.ToString();
                return RedirectToAction("Edit", new { id = callwrapup.CallWrapUpID });
            }
            else
            {
                return RedirectToAction("Create", "CallWrapUp");
            }
        }



        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            int totalRecords = 0;
            int pageIndex = 0;
            int pageSize = 0;
            int totalPages = 0;

            JSONTable jTable = new JSONTable();

            if (getParam == "searchCustomer")
            {
                var modelCustomer = db.callwrapupcustomer.Where(x => x.DeletionStateCode == 0 && x.CustomerName != null).Where("it.CustomerName.StartsWith(@0)", getVal);
                totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

                pageIndex = Convert.ToInt32(page) - 1;
                pageSize = rows;
                totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                switch (sidx)
                {
                    case "CustomerName":
                        modelCustomer = (sord.ToLower() == "desc") ? modelCustomer.OrderByDescending(x => x.CustomerName) : modelCustomer.OrderBy(x => x.CustomerName);
                        break;
                    case "CustomerID":
                        modelCustomer = (sord.ToLower() == "desc") ? modelCustomer.OrderByDescending(x => x.CustomerID) : modelCustomer.OrderBy(x => x.CustomerID);
                        break;
                    case "NonCustomerID":
                        modelCustomer = (sord.ToLower() == "desc") ? modelCustomer.OrderByDescending(x => x.NonCustomerID) : modelCustomer.OrderBy(x => x.NonCustomerID);
                        break;
                    case "CreatedByName":
                        modelCustomer = (sord.ToLower() == "desc") ? modelCustomer.OrderByDescending(x => x.CreatedByName) : modelCustomer.OrderBy(x => x.CreatedByName);
                        break;
                    case "CallStartTime":
                        modelCustomer = (sord.ToLower() == "desc") ? modelCustomer.OrderByDescending(x => x.CallStartTime) : modelCustomer.OrderBy(x => x.CallStartTime);
                        break;
                    case "CallEndTime":
                        modelCustomer = (sord.ToLower() == "desc") ? modelCustomer.OrderByDescending(x => x.CallEndTime) : modelCustomer.OrderBy(x => x.CallEndTime);
                        break;
                    default:
                        modelCustomer = (sord.ToLower() == "desc") ? modelCustomer.OrderByDescending(x => x.CallWrapUpID) : modelCustomer.OrderBy(x => x.CallWrapUpID);
                        break;
                }

                if (modelCustomer.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                {
                    page--;
                    if (page < 1)
                    {
                        page = 1;
                    }
                }

                modelCustomer = modelCustomer.Skip((page - 1) * pageSize).Take(pageSize);

                var data = modelCustomer.Select(x => new
                {
                    Id = x.CallWrapUpID,
                    customername = x.CustomerName,
                    custoemrid = x.CustomerID,
                    noncustomername = "",
                    noncustomerid = x.NonCustomerID,
                    agent = x.CreatedByName,
                    callstarttime = x.CallStartTime,
                    callendtime = x.CallEndTime
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

                jTable.total = totalPages;
                jTable.page = page;
                jTable.records = totalRecords;
                jTable.rows = data.ToArray();
            }
            else if (getParam == "searchNonCustomer")
            {
                var modelNonCustomer = db.callwrapupnoncustomer.Where(x => x.DeletionStateCode == 0 && x.NonCustomerName != null).Where("it.NonCustomerName.StartsWith(@0)", getVal);
                totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

                pageIndex = Convert.ToInt32(page) - 1;
                pageSize = rows;
                totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                switch (sidx)
                {
                    case "CustomerID":
                        modelNonCustomer = (sord.ToLower() == "desc") ? modelNonCustomer.OrderByDescending(x => x.CustomerID) : modelNonCustomer.OrderBy(x => x.CustomerID);
                        break;
                    case "NonCustomerName":
                        modelNonCustomer = (sord.ToLower() == "desc") ? modelNonCustomer.OrderByDescending(x => x.NonCustomerName) : modelNonCustomer.OrderBy(x => x.NonCustomerName);
                        break;
                    case "NonCustomerID":
                        modelNonCustomer = (sord.ToLower() == "desc") ? modelNonCustomer.OrderByDescending(x => x.NonCustomerID) : modelNonCustomer.OrderBy(x => x.NonCustomerID);
                        break;
                    case "CreatedByName":
                        modelNonCustomer = (sord.ToLower() == "desc") ? modelNonCustomer.OrderByDescending(x => x.CreatedByName) : modelNonCustomer.OrderBy(x => x.CreatedByName);
                        break;
                    case "CallStartTime":
                        modelNonCustomer = (sord.ToLower() == "desc") ? modelNonCustomer.OrderByDescending(x => x.CallStartTime) : modelNonCustomer.OrderBy(x => x.CallStartTime);
                        break;
                    case "CallEndTime":
                        modelNonCustomer = (sord.ToLower() == "desc") ? modelNonCustomer.OrderByDescending(x => x.CallEndTime) : modelNonCustomer.OrderBy(x => x.CallEndTime);
                        break;
                    default:
                        modelNonCustomer = (sord.ToLower() == "desc") ? modelNonCustomer.OrderByDescending(x => x.CallWrapUpID) : modelNonCustomer.OrderBy(x => x.CallWrapUpID);
                        break;
                }

                if (modelNonCustomer.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                {
                    page--;
                    if (page < 1)
                    {
                        page = 1;
                    }
                }

                modelNonCustomer = modelNonCustomer.Skip((page - 1) * pageSize).Take(pageSize);

                var data = modelNonCustomer.Select(x => new
                {
                    Id = x.CallWrapUpID,
                    customername = "",
                    custoemrid = x.CustomerID,
                    noncustomername = x.NonCustomerName,
                    noncustomerid = x.NonCustomerID,
                    agent = x.CreatedByName,
                    callstarttime = x.CallStartTime,
                    callendtime = x.CallEndTime
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

                jTable.total = totalPages;
                jTable.page = page;
                jTable.records = totalRecords;
                jTable.rows = data.ToArray();
            }

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadGrid2(string sidx, string sord, int rows, int page)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            var model = db.calltype.Where(x => x.DeletionStateCode == 0).Where(x => x.CallWrapUpID == new Guid(getVal));

            var data = model.ToList()
                        .Select(x => new
                        {
                            Id = x.CallTypeID,
                            summary = x.Summary,
                            category = x.CategoryName,
                            createdon = x.CreatedOn,
                            modifiedon = x.ModifiedOn
                        });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = data.ToArray().Length;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.Skip((page - 1) * pageSize).Take(pageSize);

            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CallWrapUp model)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            CallWrapUp model = db.callwrapup.Find(id);
            if (TempData["CWUID"] != null)
            {
                ViewBag.CWUID = TempData["CWUID"].ToString();
            }
            else ViewBag.CWUID = "";

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CallWrapUp model, string flagc, string flagcw, CallType calltype)
        {
            if (flagcw == "flagct")
            {
                return formSubmit2(calltype, "EditCallType");

            }
            else
            {
                if (flagc == "cust")
                {
                    model.NonCustomerID = null;
                }
                else if (flagc == "ncust")
                {
                    model.CustomerID = null;
                }
                return formSubmit(model, "Edit");
            }
        }

        public ActionResult CreateCallType(Guid idcallwrapup)
        {
            CallType calltype = new CallType();
            calltype.CallWrapUpID = idcallwrapup;
            return View(calltype);
        }

        [HttpPost]
        public ActionResult CreateCallType(CallType calltype)
        {
            return formSubmit2(calltype, "CreateCallType");
        }

        public ActionResult EditCallType(Guid id)
        {
            CallType model = db.calltype.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditEndTime(Guid id, DateTime CallEndTime)
        {
            CallWrapUp cwu = db.callwrapup.Find(id);
            cwu.CallEndTime = CallEndTime;

            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());

            try
            {
                db.spCallWrapUp_Update(cwu, sessionParam);

                var jsonData = new { flag = true };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var jsonData = new { flag = false, message = ex.Message };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
