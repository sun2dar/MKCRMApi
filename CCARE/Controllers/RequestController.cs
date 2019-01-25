using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using System.Globalization;
using CCARE.jqGrid;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Configuration;


namespace CCARE.Controllers
{
    public partial class RequestController : Controller
    {
        CRMDb db = new CRMDb();
        string entityName = "Incident";

        public ActionResult formSubmit(Request model, string actionType, string reopenReason = "")
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            //validate if kotaid is default
            if (ModelState["KotaID"].Value.AttemptedValue == "-1")
            {
                model.KotaID = null;
                ModelState.Remove("KotaID");
            }

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

                model.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());
                int state = -1;

                if (!string.IsNullOrEmpty(model.RefTicketNumber))
                {
                    try
                    {
                        string refTicketNumber = db.request.Find(new Guid(model.RefTicketNumber)).TicketNumber;
                        model.RefTicketNumber = refTicketNumber;
                    }
                    catch
                    {
                    }
                }

                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                switch (actionType)
                {
                    case "Create":
                        model.RequestID = Guid.NewGuid();
                        model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
                        results = db.Request_Create(model);
                        state = 1;
                        break;

                    case "CreateFromRCM":
                        model.RequestID = Guid.NewGuid();
                        model.TransactionTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                        model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
                        results = db.Request_Create(model);
                        if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                        {
                            state = 1;
                        }
                        break;

                    case "Edit":
                        results = db.Request_Edit(model);
                        break;

                    case "CloseFromRCM":
                        model = db.request.Find(model.RequestID);
                        string getResolution1 = "Closing request as per Request Creation Matrix";
                        string getDescription1 = "Closing request as per Request Creation Matrix";
                        string[] additionalParam1 = new string[] { getResolution1, getDescription1 };
                        results = db.Request_SetStatus(model, 5, additionalParam1);
                        break;

                    case "CancelFromRCM":
                        model = db.request.Find(model.RequestID);
                        string[] additionalParamCancel1 = new string[] { "Cancel request as per Request Creation Matrix", "Cancel request as per Request Creation Matrix" };
                        results = db.Request_SetStatus(model, 6, additionalParamCancel1);
                        break;

                    case "Close":
                        string getResolution2 = Request["Resolution"];
                        string getDescription2 = Request["Description"];
                        string[] additionalParam2 = new string[] { getResolution2, getDescription2 };
                        results = db.Request_SetStatus(model, 5, additionalParam2);
                        break;

                    case "Cancel":
                        string[] additionalParamCancel2 = new string[] { "", "" };
                        results = db.Request_SetStatus(model, 6, additionalParamCancel2);
                        break;

                    case "Reopen":
                        string getAnnotationID = Request["annotationID"];
                        string getReason = reopenReason;
                        results = db.Request_Reopen(model, new Guid(getAnnotationID) , getReason);
                        break;
                    
                    case "AssignNext":
                        results = db.Request_Workflow_Change(model, "Next");
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Request", new { id = model.RequestID, success = successMessage });
                    string urlNew = u.Action("Create", "Request");
                    Guid? requestId = model.RequestID;
                    string ticketNumber = db.request.Find(requestId).TicketNumber;

                    var jsonData = new { flag = true, Message = url, urlNew = urlNew, requestId = requestId, ticketNumber = ticketNumber, state = state };
                    return Json(jsonData);
                }
                else if (results.Key == 3)
                {
                    var jsonData = new { flag = false, Message = results.Value.ToString(), state = state, resultsKey = 3 };
                    return Json(jsonData);
                }
                else
                {
                    var jsonData = new { flag = false, Message = results.Value.ToString(), state = state };
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
            string getCustomerID = Request["_customerID"];
            string getNonCustomerID = Request["_nonCustomerID"];
            string getAccountNo = Request["_accountNo"];
            string getCustomerNo = Request["_customerNo"];
            string getCardNo = Request["_cardNo"];
            string getLoanNo = Request["_loanNo"];
            string getType = Request["_type"];

            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString()) * 10;        // multiply with 10, different with usual
            Guid id;
            // Tricky for initiliaze model on var variable
            var model = db.request.Take(0); 

            // For Index Page
            if (string.IsNullOrEmpty(getType))
            {
                if (!string.IsNullOrEmpty(getVal))
                {
                    getVal = "searchNoCustomer".Equals(getParam) ? getVal.PadLeft(16, '0') : getVal;
                    model = "searchRequestId".Equals(getParam) ? db.request.Where(x => x.TicketNumber == getVal) :
                            "searchNoCustomer".Equals(getParam) ? db.request.Where(x => x.CustomerNo == getVal) :
                            "searchNoKartu".Equals(getParam) ? db.request.Where(x => x.CardNo == getVal) :
                            "searchNoRekening".Equals(getParam) ? db.request.Where(x => x.AccountNo == getVal) :
                            null;
                }
            }
            else {
                // Request in Customer Page
                if ("customer".Equals(getType))
                {
                    id = new Guid(getCustomerID);

                    if (string.IsNullOrEmpty(getVal))
                    {
                        model = db.request.Where(x => x.CustomerID == id);
                    }
                    else
                    {
                        model = "searchRequestId".Equals(getParam) ? db.request.Where(x => x.CustomerID == id).Where(x => x.TicketNumber == getVal) :
                                "searchNoCustomer".Equals(getParam) ? db.request.Where(x => x.CustomerID == id).Where(x => x.CustomerNo == getVal) :
                                "searchNoKartu".Equals(getParam) ? db.request.Where(x => x.CustomerID == id).Where(x => x.CardNo == getVal) :
                                "searchNoRekening".Equals(getParam) ? db.request.Where(x => x.CustomerID == id).Where(x => x.AccountNo == getVal) :
                                db.request.Where(x => x.CustomerID == id);
                    }
                }

                // Request in Non Customer Page
                else if ("noncustomer".Equals(getType))
                {
                    id = new Guid(getNonCustomerID);
                    model = db.request.Where(x => x.NonCustomerID == id);
                }

                else if ("deposit".Equals(getType))
                { 
                    model = db.request.Where(x => x.AccountNo == getAccountNo);
                }

                else if ("creditcard".Equals(getType))
                {
                    model = db.request.Where(x => x.CustomerNo == getCustomerNo && x.CardNo == getCardNo);
                }

                else if ("loan".Equals(getType))
                {
                    model = db.request.Where(x => x.LoanNo == getLoanNo);
                }
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "requestId":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TicketNumber) : model.OrderBy(x => x.TicketNumber);
                    break;
                case "productIdName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ProductName) : model.OrderBy(x => x.ProductName);
                    break;
                case "categoryIdName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CategoryName) : model.OrderBy(x => x.CategoryName);
                    break;
                case "status":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StatusLabel) : model.OrderBy(x => x.StatusLabel);
                    break;
                case "createdOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
                case "dueDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.DueDate) : model.OrderBy(x => x.DueDate);
                    break;
                case "CustomerNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CustomerNo) : model.OrderBy(x => x.CustomerNo);
                    break;
                case "CardNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CardNo) : model.OrderBy(x => x.CardNo);
                    break;
                case "AccountNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.AccountNo) : model.OrderBy(x => x.AccountNo);
                    break;
                case "LoanNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LoanNo) : model.OrderBy(x => x.LoanNo);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedOn) : model.OrderBy(x => x.ModifiedOn);
                    break;
            }
            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            var data = model
                .Select(x => new
                {
                    Id = x.RequestID,
                    requestId = x.TicketNumber,
                    productIdName = x.ProductName,
                    categoryIdName = x.CategoryName,
                    status = x.StatusLabel,
                    createdOn = x.CreatedOn,
                    dueDate = x.DueDate,
                    StateCode = x.StateCode,
                    CustomerNo = x.CustomerNo,
                    CardNo = x.CardNo,
                    AccountNo = x.AccountNo,
                    LoanNo = x.LoanNo,
                    CustomerID = x.CustomerID,
                    NonCustomerID = x.NonCustomerID
                });
            
            JSONTable jTable = new JSONTable();
            jTable.rows = data.ToArray();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
         
            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadTask(string sidx, string sord, int rows, int page = 1)
        {
            string getRequestID = Request["_requestID"];

            var data = db.task.Where(x => x.RequestID == new Guid(getRequestID) && x.TaskStatusLabel != "Canceled" && x.TaskStatusLabel != "Completed").ToList()
                .Select(x => new
                {
                    Id = x.TaskID,
                    Subject = x.Subject,
                    TaskStatusLabel = x.TaskStatusLabel,
                    CreatedByName = x.CreatedByName,
                    Regarding = x.Regarding,
                    CreatedOn = x.CreatedOn
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

        public ActionResult Create(Guid? customerID = null, string customerName = null, string segmentationName = null)
        {
            StringMapController dropDown = new StringMapController();
            string customerType = Request["customerType"];
            string accountNo = Request["accountNo"];
            string cardNo = Request["cardNo"];
            string customerNo = Request["customerNo"];
            string loanNo = Request["loanNo"];

            Request model = new Request();
            model.CreatedBy = new Guid(Session["CurrentUserID"].ToString());
            model.CreatedByName = Session["Fullname"].ToString();
            model.InteractionCount = "1";
            model.CreatedOn = DateTime.Now;
            model.ForCustomer = true;

            if (customerType == "noncustomer")
            {
                model.CustomerID = new Guid(ConfigurationManager.AppSettings["DummyCustomerID"].ToString());
                model.ForCustomer = false;
                model.NonCustomerID = customerID;
                model.NonCustomerName = customerName;
                ViewBag.customerType = "noncustomer";
            }
            else if (customerType == "customer_deposit")
            {
                model.ForCustomer = true;
                model.CustomerID = customerID;
                model.CustomerName = customerName;
                model.AccountNo = accountNo;
                model.CardNo = cardNo;
                ViewBag.customerType = "customer_deposit";
            }
            else if (customerType == "customer_creditcard")
            {
                model.ForCustomer = true;
                model.CustomerID = customerID;
                model.CustomerName = customerName;
                model.CustomerNo = customerNo;
                model.CardNo = cardNo;
                ViewBag.customerType = "customer_creditcard";
            }
            else if (customerType == "customer_loan")
            {
                model.ForCustomer = true;
                model.CustomerID = customerID;
                model.CustomerName = customerName;
                model.AccountNo = accountNo;
                model.LoanNo = loanNo;
                ViewBag.customerType = "customer_loan";
            }
            else if (customerType == "customer")
            {
                model.ForCustomer = true;
                model.CustomerID = customerID;
                model.CustomerName = customerName;
                ViewBag.customerType = "customer";
            }

            model.CustomerAttitude = 3;
            model.PriorityCode = 2;
            //buat hide save and new di form
            ViewBag.hideSaveAndOpen = true;

            //buat dropdown di form
            ViewBag.SL_ContactMethod = dropDown.getDropDown(entityName, "contactmethod", model.ContactMethod == null ? 0 : (int)model.ContactMethod);
            ViewBag.SL_CustomerAttitude = dropDown.getDropDown(entityName, "customerattitude", model.CustomerAttitude == null ? 0 : (int)model.CustomerAttitude);
            ViewBag.SL_Location = dropDown.getDropDown(entityName, "location", model.Location == null ? 0 : (int)model.Location);
            ViewBag.SL_PriorityCode = dropDown.getDropDown(entityName, "prioritycode", model.PriorityCode == null ? 0 : (int)model.PriorityCode);
            ViewBag.SL_StatusCode = dropDown.getDropDown(entityName, "statuscode", model.StatusCode == null ? 0 : (int)model.StatusCode);
            ViewBag.SL_Currency = dropDown.getDropDown(entityName, "currency", model.Currency == null ? 0 : (int)model.Currency);
            ViewBag.SL_CaseOriginCode = dropDown.getDropDown(entityName, "caseorigincode", model.CaseOriginCode == null ? 0 : (int)model.CaseOriginCode);

            //find segmentationID
            if (segmentationName != null)
            {
                var getSegmentation = db.Segmentation.Where(x => x.Name == segmentationName);
                if (getSegmentation != null)
                {
                    model.SegmentationID = getSegmentation.FirstOrDefault().ID;
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Request model, string TransactionTimeFormat = null)
        {
            string getTransactionTimeHour = Request["TransactionTimeHour"];
            string getTransactionTimeMin = Request["TransactionTimeMin"];
            string getTransactionTimeSec = Request["TransactionTimeSec"];

            if (!string.IsNullOrEmpty(getTransactionTimeHour) && !string.IsNullOrEmpty(getTransactionTimeMin) && !string.IsNullOrEmpty(getTransactionTimeSec))
                model.TransactionTime += " " + getTransactionTimeHour + ":" + getTransactionTimeMin + ":" + getTransactionTimeSec;

            if (model.TransactionTime == TransactionTimeFormat)
            {
                model.TransactionTime = null;
            }

            return formSubmit(model, "Create");
        }

        public ActionResult CreateRequestForChangeStatus(Request model, RequestCreationMatrix rcm, string userId, string serialNumber, string corpId, string status)
        {
            if (status == "cancel")
            {
                return formSubmit(model, "CancelFromRCM");
            }
            else if (status == "close")
            {
                return formSubmit(model, "CloseFromRCM");
            }
            else
            {
                if (rcm.AccountCode == "C")
                {
                    rcm.AccountCode = "B";
                }
                var RCM_data = db.RequestCreationMatrix_GetData(rcm).ToList();
                if (RCM_data.Count >= 1 && RCM_data[0] != null)
                {
                    model.ProductID = RCM_data[0].ProductID;
                    model.CategoryID = RCM_data[0].CategoryID;
                    model.StatusCode = RCM_data[0].RequestStatus;
                    model.Title = RCM_data[0].Summary + model.Title;
                    
                    string CIS = "";
                    if (model.CustomerID != null)
                        CIS = db.customer.Find(model.CustomerID).CISNumber;
                    model.Title = getSummary(model.Title, userId, corpId, model.CommunicationPhone, serialNumber, CIS, model.ModifiedByName);
                    model.CaseOriginCode = 200003;
                    model.Location = 1;
                    model.ContactMethod = 1;
                    model.CustomerAttitude = 3;
                    model.PriorityCode = 2;
                    model.ForCustomer = true;
                    return formSubmit(model, "CreateFromRCM");
                }
                else
                {
                    var jsonData = new { flag = false, state = 0, CHECK = "ga ada rcm" };
                    return Json(jsonData);
                }
            }
        }

        public ActionResult CreateRequestGetData()
        {
            string status = Request["status"];
            string requestID = Request["requestID"];
            string entityType = Request["entityType"]; 
            string statusType = Request["statusType"]; 
            string previousStatus = Request["previousStatus"]; 
            string newStatus = Request["newStatus"]; 
            string updatedById = Request["updatedById"];
            string updatedByName = Request["updatedByName"];
            string updateTime = Request["updateTime"];
            string customerID = Request["customerID"];
            string accountNo = Request["accountNo"];
            string cardNo = Request["cardNo"];
            string mobileNo = Request["mobileNo"];
            string customerNo = Request["customerNo"];
            string userId = Request["userId"];
            string emailAddress = Request["emailAddress"];
            string snKeyBCA = Request["snKeyBCA"];
            string corpId = Request["corpId"];
            string category = Request["category"];
            string additionalSummary = Request["additionalSummary"];
            string reason = Request["reason"];
            string currentUserName = Session["FullName"].ToString();

            Request model = new Request();
            RequestCreationMatrix rcm = new RequestCreationMatrix();

            model.RequestID = string.IsNullOrEmpty(requestID) ? Guid.Empty : new Guid(requestID);
            model.CustomerID = string.IsNullOrEmpty(customerID) ? Guid.Empty : new Guid(customerID);
            model.TransactionTime = updateTime;
            model.AccountNo = accountNo;
            model.CardNo = cardNo;
            model.CustomerNo = customerNo;
            model.CommunicationPhone = mobileNo;
            model.ModifiedByName = currentUserName;
            model.Title = " - " + additionalSummary;
            model.Reason = reason;

            rcm.EntityType = string.IsNullOrEmpty(entityType) ? 0 : int.Parse(entityType);
            rcm.StatusType = string.IsNullOrEmpty(statusType) ? 0 : int.Parse(statusType);
            rcm.PrevStatus = previousStatus;
            rcm.NewStatus = newStatus;
            if (rcm.EntityType == 4)
                rcm.AccountCode = category;
            else
                rcm.AccountCode = "0";

            return CreateRequestForChangeStatus(model, rcm, userId, snKeyBCA, corpId, status);
        }

        public ActionResult Edit(Guid id)
        {
            Request model = db.request.Find(id);

            Regex r = new Regex(@"\d{2}-\d{2}-\d{4}");

            bool correctRegex = false;
            if (!string.IsNullOrEmpty(model.TransactionTime))
            {
                correctRegex = r.IsMatch(model.TransactionTime);
            }

            if (correctRegex == true && !string.IsNullOrEmpty(model.TransactionTime) && model.TransactionTime.Length > 10)
            {
                ViewBag.TransactionTimeHour = model.TransactionTime[11].ToString() + model.TransactionTime[12].ToString();
                ViewBag.TransactionTimeMin = model.TransactionTime[14].ToString() + model.TransactionTime[15].ToString();
                if (model.TransactionTime.Length == 19)
                    ViewBag.TransactionTimeSec = model.TransactionTime[17].ToString() + model.TransactionTime[18].ToString();
                else
                    ViewBag.TransactionTimeSec = "00";

                model.TransactionTime = model.TransactionTime.Substring(0, 10);
            }

            StringMapController dropDown = new StringMapController();

            //Set Customer Product field
            if (!string.IsNullOrEmpty(model.LoanNo))
            {
                ViewBag.CustomerProduct = "loan";
            }
            else if (!string.IsNullOrEmpty(model.CustomerNo))
            {
                ViewBag.CustomerProduct = "creditcard";
            }
            else if (!string.IsNullOrEmpty(model.AccountNo) || !string.IsNullOrEmpty(model.CardNo))
            {
                ViewBag.CustomerProduct = "deposit";
            }

            if (model.CustomerID == null || model.CustomerID == new Guid(ConfigurationManager.AppSettings["DummyCustomerID"].ToString()))
            {
                ViewBag.customerType = "noncustomer";
            }
            else
            {
                ViewBag.customerType = "customer";
            }

            if ((model.StatusCode == 1 || model.StatusCode == 2 || model.StatusCode == 3) && model.UserBranchID == new Guid(Session["BusinessUnitID"].ToString()))
            {
                ViewBag.displayStatus = "enabled";
            }
            else
            {
                ViewBag.displayStatus = "disabled";
            }
            //buat hide save and new di form
            ViewBag.hideSaveAndOpen = true;

            //buat notes
            ViewBag.ObjectID = id;
            ViewBag.BusinessUnitID = model.BusinessUnitID;

            //buat documents
            ViewBag.TabPosition = Request["position"];

            //buat dropdown di form
            ViewBag.SL_ContactMethod = dropDown.getDropDown(entityName, "contactmethod", model.ContactMethod == null ? 0 : (int)model.ContactMethod);
            ViewBag.SL_CustomerAttitude = dropDown.getDropDown(entityName, "customerattitude", model.CustomerAttitude == null ? 0 : (int)model.CustomerAttitude);
            ViewBag.SL_Location = dropDown.getDropDown(entityName, "location", model.Location == null ? 0 : (int)model.Location);
            ViewBag.SL_PriorityCode = dropDown.getDropDown(entityName, "prioritycode", model.PriorityCode == null ? 0 : (int)model.PriorityCode);
            ViewBag.SL_StatusCode = dropDown.getDropDown(entityName, "statuscode", model.StatusCode == null ? 0 : (int)model.StatusCode);
            ViewBag.SL_Currency = dropDown.getDropDown(entityName, "currency", model.Currency == null ? 0 : (int)model.Currency);
            ViewBag.SL_CaseOriginCode = dropDown.getDropDown(entityName, "caseorigincode", model.CaseOriginCode == null ? 0 : (int)model.CaseOriginCode);


            //Get kota list
            if (model.SegmentationID != null)
            {
                ViewBag.Kota = new SelectList(db.serviceLevel.Where(x => x.ProductID == model.ProductID && x.CategoryID == model.CategoryID && x.SegmentationID == model.SegmentationID && x.DeletionStateCode == 0 && x.KotaID != null).Select(x => new { key = x.KotaID, value = x.KotaName }).Distinct(), "key", "value");
            }
            else
            {
                ViewBag.Kota = new SelectList(db.serviceLevel.Where(x => x.ProductID == model.ProductID && x.CategoryID == model.CategoryID && x.DeletionStateCode == 0 && x.KotaID != null).Select(x => new { key = x.KotaID, value = x.KotaName }).Distinct(), "key", "value");
            }

            Annotation note = db.SpGetNotes(id).Select(x => new Annotation
            {
                AnnotationID = x.AnnotationID,
                CreatedByID = x.CreatedBy,
                CreatedBy = x.CreatedByName,
                CreatedOn = x.CreatedOn,
                DeletionStateCode = x.DeletionStateCode,
                ModifiedByID = x.ModifiedBy,
                ModifiedBy = x.ModifiedByName,
                ModifiedOn = x.ModifiedOn,
                NoteText = x.NoteText,
                ObjectID = x.ObjectID,
                ObjectTypeCode = x.ObjectTypeCode,
                BusinessUnitID = x.OwningBusinessUnit,
                Subject = x.Subject
            }).FirstOrDefault();

            model.Notes.Add(note);

            var referral = string.IsNullOrEmpty(model.RefTicketNumber) ? null : db.request.Where(x => x.TicketNumber == model.RefTicketNumber).FirstOrDefault();
            ViewBag.RefferalProduct = referral == null ? string.Empty : referral.ProductName;
            ViewBag.RefferalCategory = referral == null ? string.Empty : referral.CategoryName;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Request model, string TransactionTimeFormat = null)
        {
            string getTransactionTimeHour = Request["TransactionTimeHour"];
            string getTransactionTimeMin = Request["TransactionTimeMin"];
            string getTransactionTimeSec = Request["TransactionTimeSec"];

            if (!string.IsNullOrEmpty(getTransactionTimeHour) && !string.IsNullOrEmpty(getTransactionTimeMin) && !string.IsNullOrEmpty(getTransactionTimeSec))
                model.TransactionTime += " " + getTransactionTimeHour + ":" + getTransactionTimeMin + ":" + getTransactionTimeSec;

            if (model.TransactionTime == TransactionTimeFormat)
            {
                model.TransactionTime = null;
            }
            
            return formSubmit(model, "Edit");
        }

        public ActionResult RequestAuditLog(Guid id)
        {
            Request model = db.request.Find(id);
            return View(model);
        }

        public ActionResult Task(Guid id)
        {
            Request model = db.request.Find(id);
            return View(model);
        }

        public ActionResult AssignToMe(Request model)
        {
            Request temp = new Request();
            temp.RequestID = model.RequestID;
            temp.ModifiedBy = model.ModifiedBy;
            temp.OwnerID = model.ModifiedBy;
            return formSubmit(temp, "AssignToUser");
        }

        public ActionResult AssignToWG(Request model)
        {
            Request temp = new Request();
            temp.RequestID = model.RequestID;
            temp.ModifiedBy = model.ModifiedBy;
            temp.OwnerID = model.OwnerID;
            string assignType = "";
            return formSubmit(temp, "Assign");
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult NextOrPrevWG(Request model, string operationType, string objectId, string prevReason, string annotationID)
        {
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = "Assign to "+operationType+" Workgroup sukses";
            model.CreatedBy = model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());

            results = db.Request_Workflow_Change(model, operationType);

            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                if (operationType == "Prev")
                {
                    // Append reason to note - Start
                    Annotation selectedNote = db.annotation.Find(new Guid(annotationID));
                    string ModifiedBy = Session["Fullname"].ToString();
                    string ModifiedOn = DateTime.Now.ToString();
                    string tempNoteText = prevReason;
                    string formatNote = "\nPrevious Workgroup By  " + ModifiedBy + "    on   " + ModifiedOn + "\n";

                    prevReason = "\n\n" + formatNote + tempNoteText + "\n\n-------------------------------------------------------------------------------\n" + selectedNote.NoteText;
                    Annotation note = new Annotation();
                    note.ObjectID = new Guid(objectId);
                    note.NoteText = prevReason;
                    note.AnnotationID = selectedNote.AnnotationID;
                    note.ModifiedByID = new Guid(Session["CurrentUserID"].ToString());

                    results = db.SpUpdateNote(note);
                    // Append reason to note - End
                }

                Request reqnew = new Request();
                reqnew = db.request.Find(model.RequestID);

                bool workflowHasNext = WorkflowFunction.hasNext(new Guid(model.RequestID.ToString()));
                bool workflowHasPrev = WorkflowFunction.hasPrev(new Guid(model.RequestID.ToString()));
                bool workflowIsOwner = model.OwnerID == new Guid(Session["CurrentUserID"].ToString());

                var jsonData = new { flag = true, Message = message, newWGID = reqnew.WorkgroupId, newWGName = reqnew.WorkgroupName, hasNext = workflowHasNext, hasPrev = workflowHasPrev, isOwner = workflowIsOwner };
                return Json(jsonData);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData);
            }
        }

        [HttpPost]
        public ActionResult Close(Request model, string Resolution, string Description)
        {
            return formSubmit(model, "Close");
        }

        [HttpPost]
        public ActionResult Reopen(Request model, string annotationID, string reopenReason)
        {
            Annotation selectedNote = db.annotation.Find(new Guid(annotationID));
            string ModifiedBy = Session["Fullname"].ToString();
            string ModifiedOn = DateTime.Now.ToString();
            string tempNoteText = reopenReason;
            string formatNote = "\nReopened/Modified By  " + ModifiedBy + "    on   " + ModifiedOn + "\n";

            reopenReason = "\n\n" + formatNote + tempNoteText + "\n\n-------------------------------------------------------------------------------\n" + selectedNote.NoteText;

            return formSubmit(model, "Reopen", reopenReason);
        }

        [HttpPost]
        public ActionResult CloseMultiple(string requestIds, string Resolution, string Description)
        {
            List<string> IdsList = string.IsNullOrEmpty(requestIds) ? new List<string>() : new JavaScriptSerializer().Deserialize<List<string>>(requestIds);

            List<Request> requestList = new List<Request>();

            foreach (var ids in IdsList)
            {
                requestList.Add(db.request.Find(new Guid(ids)));
            }

            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            string getResolution2 = Request["Resolution"];
            string getDescription2 = Request["Description"];
            string[] additionalParam2 = new string[] { getResolution2, getDescription2 };

            int succeedCounter = 0;
            int failedCounter = 0;

            foreach (var request in requestList)
            {
                if (ModelState.IsValid)
                {
                    KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

                    request.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());

                    results = db.Request_SetStatus(request, 5, additionalParam2);

                    if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                    {
                        succeedCounter++;
                    }
                    else
                    {
                        failedCounter++;
                    }
                }
            }

            var jsonData = new { closeSucceed = succeedCounter, closeFailed = failedCounter };
            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult AcceptMultiple(string requestIds, string assignee = "")
        {
            List<string> IdsList = string.IsNullOrEmpty(requestIds) ? new List<string>() : new JavaScriptSerializer().Deserialize<List<string>>(requestIds);

            List<Request> requestList = new List<Request>();

            foreach (var ids in IdsList)
            {
                requestList.Add(db.request.Find(new Guid(ids)));
            }

            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            int succeedCounter = 0;
            int failedCounter = 0;

            foreach (var request in requestList)
            {
                if (ModelState.IsValid)
                {
                    KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

                    request.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());

                    request.OwnerID = string.IsNullOrEmpty(assignee) ? new Guid(Session["CurrentUserID"].ToString()) : new Guid(assignee);
                    results = db.QueueItem_AssignTo(new Guid(request.OwnerID.ToString()), new Guid(Session["CurrentUserID"].ToString()), new Guid(request.RequestID.ToString()), "112", Session["BusinessUnitID"].ToString(), 2);

                    if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                    {
                        succeedCounter++;
                    }
                    else
                    {
                        failedCounter++;
                    }
                }
            }

            var jsonData = new { acceptSucceed = succeedCounter, acceptFailed = failedCounter };
            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult Cancel(Request model)
        {
            return formSubmit(model, "Cancel");
        }

        public ActionResult LetterEntry(Guid id)
        {
            var model = db.request.Find(id);
            return View(model);
        }

        public ActionResult History(Guid id)
        {
            var model = db.request.Find(id);
            return View(model);
        }

        public ActionResult Request_Workflow(Guid id)
        {
            var model = db.request.Find(id);
            return View(model);
        }

        public JsonResult LoadHistory(string sidx, string sord, int rows, int page = 1)
        {
            string getRequestID = Request["_requestID"];

            var data = db.requestresolution.Where(x => x.RequestID == new Guid(getRequestID) && (x.ActivityTypeLabel == "Case Resolution" || (x.ActivityTypeLabel == "Task" && x.StatusCode == 5 || x.StatusCode == 6))).ToList()
                .Select(x => new
                {
                    Id = x.ActivityId,
                    Subject = x.Subject,
                    ActivityType = x.ActivityTypeLabel,
                    ActivityStatus = x.StatusCodeLabel,
                    Regarding = x.RequestTitle,
                    ActualEnd = x.CreatedOn,
                    CreatedBy = x.CreatedByName
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

        public ActionResult Print(string Id)
        {
            Guid key;
            try
            {
                key = new Guid(Id);
            }
            catch (Exception e)
            {
                key = Guid.NewGuid();
            }

            CCARE.Models.Request request = db.request.Find(key);
            if (request == null)
            {
                request = new Request();
                request.RequestID = Guid.NewGuid();
            }
            List<Annotation> annotationList = db.annotation.Where(x => x.ObjectID == request.RequestID.Value && x.IsDocument == true).ToList();
            Annotation note = db.SpGetNotes(request.RequestID.Value).Select(x => new Annotation
            {
                AnnotationID = x.AnnotationID,
                CreatedByID = x.CreatedBy,
                CreatedBy = x.CreatedByName,
                CreatedOn = x.CreatedOn,
                DeletionStateCode = x.DeletionStateCode,
                ModifiedByID = x.ModifiedBy,
                ModifiedBy = x.ModifiedByName,
                ModifiedOn = x.ModifiedOn,
                NoteText = x.NoteText,
                ObjectID = x.ObjectID,
                ObjectTypeCode = x.ObjectTypeCode,
                BusinessUnitID = x.OwningBusinessUnit,
                Subject = x.Subject
            }).FirstOrDefault();
            if (note != null) annotationList.Add(note);
            request.Notes = annotationList;

            ViewBag.Title = string.Format("Request: {0}", request.Title);

            CustomerAddress address = db.customeradress.Where(x => x.ParentId == request.CustomerID).Where(x => x.AddressTypeLabel == request.LocationLabel).FirstOrDefault();
            if (address != null) {
                request.Address1 = address.Line1;
                request.Address2 = address.Line2 + " " + address.Line3;
                request.City = address.City;
                request.PostalCode = address.PostalCode;
            }
            
            return View(request);
        }

        public ActionResult Resolution(Guid id)
        {
            RequestResolution model = db.requestresolution.Find(id);
            return View(model);
        }

        public string getSummary(string Summary, string UserID, string CorpID, string HPno, string SerialNumber, string CustomerID, string ModifiedBy)
        {
            string result = "";

            string[] list = { "[User ID]", "[Corp ID]", "[HP No]", "[Serial Number]", "[Customer ID]", "[Modified By]" };

            result = Summary.Replace(list[0], UserID)
                            .Replace(list[1], CorpID)
                            .Replace(list[2], HPno)
                            .Replace(list[3], SerialNumber)
                            .Replace(list[4], CustomerID)
                            .Replace(list[5], ModifiedBy);

            return result;
        }

        public ActionResult ReferalRequestList(Guid Id)
        {
            ViewBag.RequestId = Id;
            return View();
        }

        public JsonResult LoadReferalRequestGrid(string sidx, string sord, int rows, int page = 1)
        {
            Guid requestId = string.IsNullOrEmpty(Request["_requestId"]) ? Guid.Empty : new Guid(Request["_requestId"].ToString());
            string ticketNumber = requestId != null ? db.request.Find(requestId).TicketNumber : "";
            int totalRecords = 0;

            var model = db.request.Take(0);

            totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "requestId":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TicketNumber) : model.OrderBy(x => x.TicketNumber);
                    break;
                case "productIdName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ProductName) : model.OrderBy(x => x.ProductName);
                    break;
                case "categoryIdName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CategoryName) : model.OrderBy(x => x.CategoryName);
                    break;
                case "status":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StatusLabel) : model.OrderBy(x => x.StatusLabel);
                    break;
                case "createdOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
                case "CustomerNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CustomerNo) : model.OrderBy(x => x.CustomerNo);
                    break;
                case "CardNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CardNo) : model.OrderBy(x => x.CardNo);
                    break;
                case "AccountNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CustomerNo) : model.OrderBy(x => x.CustomerNo);
                    break;
                case "LoanNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CardNo) : model.OrderBy(x => x.CardNo);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
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

            var data = model.Where(x => x.RefTicketNumber == ticketNumber)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new
                {
                    Id = x.RequestID,
                    requestId = x.TicketNumber,
                    productIdName = x.ProductName,
                    categoryIdName = x.CategoryName,
                    status = x.StatusLabel,
                    createdOn = x.CreatedOn,
                    StateCode = x.StateCode,
                    CustomerNo = x.CustomerNo,
                    CardNo = x.CardNo,
                    AccountNo = x.AccountNo,
                    LoanNo = x.LoanNo,
                    CustomerID = x.CustomerID,
                    NonCustomerID = x.NonCustomerID
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

        public JsonResult GetDetailReferralRequest(string _requestID)
        {
            var model = db.request.Where(x => x.TicketNumber == _requestID).FirstOrDefault();
            string output = "Product  : " + model.ProductName + "<br /> Category : " + model.CategoryName;
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getKotaList(Guid? productID, Guid? categoryID, Guid? segmentationID)
        {
            if (segmentationID != null)
            {
                var model = db.serviceLevel.Where(x => x.ProductID == productID && x.CategoryID == categoryID && x.SegmentationID == segmentationID && x.DeletionStateCode == 0 && x.KotaID != null).Select(x => new { key = x.KotaID, value = x.KotaName }).Distinct();
                //db.serviceLevel.Where(x => x.ProductID == model.ProductID && x.CategoryID == model.CategoryID && x.SegmentationID == x.SegmentationID).Select(x => new { x.KotaID, x.KotaName }).Distinct(), "KotaID", "KotaName");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = db.serviceLevel.Where(x => x.ProductID == productID && x.CategoryID == categoryID && x.DeletionStateCode == 0 && x.KotaID != null).Select(x => new { key = x.KotaID, value = x.KotaName }).Distinct();
                //db.serviceLevel.Where(x => x.ProductID == model.ProductID && x.CategoryID == model.CategoryID && x.SegmentationID == x.SegmentationID).Select(x => new { x.KotaID, x.KotaName }).Distinct(), "KotaID", "KotaName");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
