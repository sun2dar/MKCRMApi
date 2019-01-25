using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using CCARE.jqGrid;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.Models.MasterData;
using CCARE.Models.Transaction;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;

namespace CCARE.Controllers
{
    public partial class LetterEntryController : Controller
    {
        private CRMDb db = new CRMDb();
        public String[] arrRomans = new String[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string id)
        {
            List<string> IdsList = string.IsNullOrEmpty(id) ? new List<string>() : new JavaScriptSerializer().Deserialize<List<string>>(id);
            Boolean isMultiple = IdsList.Count > 1 ? true : false;

            List<SelectListItem> durations = new List<SelectListItem>();
            for (int i = 0; i <= 5; i++)
            {
                // awalnya dalam menit, namun diubah menjadi dalam hari, approved by RCH (20151020)
                string value = i.ToString(); 
                string text = (i == 0) ? string.Empty : (i == 1) ? i + " day" : i + " days";
                durations.Add(new SelectListItem { Text = text, Value = value });
            }

            ViewData["ddl_durations"] = durations;

            LetterEntryView model = new LetterEntryView();
            model.LetterEntry = new LetterEntry();
            model.RequestIds = id;
            model.IsMultipleLetter = isMultiple;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(LetterEntryView model)
        {
            JSONResponse result = new JSONResponse();
            result.Value = false;
            result.Response = string.Empty;

            try
            {
                if (Session["BusinessUnitID"] == null | Session["CurrentUserID"] == null) throw new Exception(Resources.LetterEntry.Alert_SessionIsNull);

                var businessunitid = new Guid(Convert.ToString(Session["BusinessUnitID"]));
                var systemuserid = new Guid(Convert.ToString(Session["CurrentUserID"]));
                var userid = db.systemUser.Single(p => p.SystemUserId == systemuserid);

                if (ModelState.IsValid)
                {
                    StringBuilder errorMessages = new StringBuilder(), successMessages = new StringBuilder();
                    KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, string.Empty);
                    List<string> IdsList = string.IsNullOrEmpty(model.RequestIds) ? new List<string>() : new JavaScriptSerializer().Deserialize<List<string>>(model.RequestIds);

                    if (IdsList.Count.Equals(0)) throw new Exception(Resources.LetterEntry.Alert_NoRequest);
                    if (IdsList.Count > 5) throw new Exception(Resources.LetterEntry.Alert_RequestMoreThanFive);
                    if (!model.LetterEntry.TemplateID.HasValue) throw new Exception(Resources.LetterEntry.Alert_ChooseTemplate);

                    LetterTemplate lettertemplate = db.letterTemplate.Where(x => x.LetterTemplateId == model.LetterEntry.TemplateID).FirstOrDefault();
                    if (lettertemplate == null) throw new Exception(Resources.LetterEntry.Alert_TemplateUndefined);

                    for (int i = 0; i <= IdsList.Count - 1; i++)
                    {
                        Guid requestId = new Guid(IdsList.ElementAt(i));
                        Request request = db.request.Where(x => x.RequestID == requestId).FirstOrDefault();

                        /*
                         * Jika kondisi nya multiple-request, maka branch yang diambil adalah branch dari Request.
                         * Sebaliknya, jika field CC diisi oleh user maka branch yang diambil adalah branch dari CC tersebut.
                         * Sebaliknya, jika field UserBranch pada Request tidak null, maka branch yang diambil adalah branch dari Request.
                         * Sebaliknya, branch sama dengan null.
                         */
                        Guid branchID = (model.IsMultipleLetter) ? (request.BranchID.HasValue ? request.BranchID.Value : Guid.Empty) : (model.LetterEntry.CC_ID.HasValue ? model.LetterEntry.CC_ID.Value : (request.UserBranchID.HasValue ? request.UserBranchID.Value : Guid.Empty));
                        Branch branch = branchID.Equals(Guid.Empty) ? null : db.branch.Where(x => x.BranchID == branchID).FirstOrDefault();

                        string address1 = null, address2 = null, address3 = null, city = null, postalcode = null;

                        if (request.Location != null)
                        {
                            StringMap stringmap = db.pickList
                                .Where(x => x.EntityName.Equals("CustomerAddress") && x.AttributeName.Equals("addresstypecode") && x.AttributeValue.Equals(request.Location.Value))
                                .FirstOrDefault();

                            if (stringmap != null & stringmap.label.ToUpper().Equals("OTHER"))
                            {
                                address1 = request.Address1;
                                address2 = request.Address2;
                                city = request.City;
                                postalcode = request.PostalCode;
                            }
                            else if (request.CustomerID != null)
                            {
                                CCARE.Models.Customer customer = db.customer.Where(x => x.CustomerID == request.CustomerID.Value).FirstOrDefault();
                                if (customer != null)
                                {
                                    address1 = customer.Line1;
                                    address2 = customer.Line2;
                                    address3 = customer.Line3;
                                    city = customer.City;
                                    postalcode = customer.PostalCode;
                                }
                            }
                        }
                        else if (request.NonCustomerID != null)
                        {
                            NonCustomer noncustomer = db.noncustomer.Where(x => x.NonCustomerID == request.NonCustomerID.Value).FirstOrDefault();
                            if (noncustomer != null)
                            {
                                address1 = noncustomer.Address1;
                                address2 = noncustomer.Address2;
                                address3 = noncustomer.Address3;
                                city = noncustomer.City;
                                postalcode = noncustomer.Zipcode;
                            }
                        }

                        LetterEntry letterentry = new LetterEntry();
                        letterentry.AccountNo = request.AccountNo;
                        letterentry.Address1 = address1;
                        letterentry.Address2 = address2;
                        letterentry.Address3 = address3;
                        letterentry.AttchATM = model.LetterEntry.AttchATM;
                        letterentry.AttchATMSwitching = model.LetterEntry.AttchATMSwitching;
                        letterentry.AttchCirrus = model.LetterEntry.AttchCirrus;
                        letterentry.AttchDebit = model.LetterEntry.AttchDebit;
                        letterentry.AttchMaestro = model.LetterEntry.AttchMaestro;
                        letterentry.AttchDuration = model.LetterEntry.AttchDuration;
                        letterentry.BusinessUnitID = businessunitid;
                        letterentry.CardNo = request.CardNo;

                        if (branch != null)
                        {
                            if (branch.BranchID != null) letterentry.CC_ID = branch.BranchID;
                            if (!string.IsNullOrEmpty(branch.Name)) letterentry.CC_Name = branch.Name;
                        }

                        letterentry.City = city;
                        letterentry.Description1 = model.LetterEntry.Description1;
                        letterentry.Description2 = model.LetterEntry.Description2;
                        letterentry.Description3 = model.LetterEntry.Description3;
                        letterentry.Description4 = model.LetterEntry.Description4;
                        letterentry.FullName = request.CustomerName;
                        letterentry.LetterDate = model.LetterEntry.LetterDate == null ? DateTime.Now : model.LetterEntry.LetterDate.Value;
                        letterentry.LetterEntryID = Guid.NewGuid();

                        if (lettertemplate.Type.Equals((int)LetterType.SPC))
                        {
                            letterentry.LetterNo = Convert.ToString(Convert.ToDouble(model.LetterEntry.LetterNo) + i).PadLeft(4, '0');
                        }
                        else
                        {
                            string bcaNo = db.AutoNumber_GetNext("Letter Entry", DateTime.Now.Year);
                            if (bcaNo != "")
                            {
                                letterentry.LetterNo = string.Format("{0}/BHB/{1}/{2}", bcaNo.PadLeft(5, '0'), arrRomans.ElementAt(letterentry.LetterDate.Value.Month - 1), letterentry.LetterDate.Value.Year.ToString());
                            }
                            else
                            {
                                errorMessages.AppendLine("Unable to generate letter entry. Auto Number record does not exist. ");
                                continue;
                            }
                        }

                        letterentry.ModifiedBy = systemuserid;
                        letterentry.RequestID = request.RequestID.Value;
                        letterentry.TicketNumber = request.TicketNumber;
                        letterentry.TemplateID = lettertemplate.LetterTemplateId;
                        letterentry.ZIPPostalCode = postalcode;
                        results = db.LetterEntry_Insert(letterentry);

                        if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                        {
                            MergeDocument aMergeDocument = new MergeDocument();
                            JSONResponse mergeResult = aMergeDocument.Merge(letterentry.LetterEntryID);

                            if (Convert.ToBoolean(mergeResult.Value))
                            {
                                successMessages.Append(letterentry.LetterEntryID.ToString());
                            }
                            else
                                errorMessages.AppendLine(string.Format(Resources.LetterEntry.Alert_GenerateLetterError, request.TicketNumber, mergeResult.Response));

                            continue;
                        }
                        else
                        {
                            errorMessages.AppendLine(results.Value.ToString());
                            continue;
                        }
                    }

                    string strAnyErrors = errorMessages.ToString();
                    string strAnySuccess = successMessages.ToString();
                    successMessages.AppendLine(strAnyErrors);

                    if (string.IsNullOrEmpty(strAnyErrors))
                    {

                        UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                        string url = u.Action("Edit", "LetterEntry", new { id = successMessages.ToString().Trim() });

                        result.Value = true;
                        result.Response = url;
                    }
                    else
                        throw new Exception(successMessages.ToString());
                }
                else
                {
                    foreach (var key in ModelState.Keys)
                    {
                        var error = ModelState[key].Errors.FirstOrDefault();
                        if (error != null) throw new Exception(error.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                result.Value = false;
                result.Response = e.Message;
            }

            return Json(result);
        }

        public ActionResult Edit(Guid Id)
        {
            LetterEntry le = db.letterEntry.Find(Id);
            Annotation annotation = db.annotation.Where(x => x.ObjectID == le.LetterEntryID).FirstOrDefault();
            LetterEntryView model = new LetterEntryView();
            model.Annotation = annotation;
            model.LetterEntry = le;

            List<SelectListItem> durations = new List<SelectListItem>();
            for (int i = 0; i <= 5; i++)
            {
                string value = i.ToString();
                string text = (i == 0) ? string.Empty : (i == 1) ? i + " day" : i + " days";
                durations.Add(new SelectListItem { Text = text, Value = value });
            }

            ViewData["ddl_durations"] = durations;
            ViewBag.Title = string.Format("Letter Entry: {0}", le.LetterNo);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(LetterEntryView model)
        {
            return SubmitForm(model.LetterEntry, ActionType.Delete);
        }

        [HttpPost]
        public ActionResult ChangeStatus(LetterEntryView model)
        {
            return SubmitForm(model.LetterEntry, ActionType.ChangeStatus);
        }

        public ActionResult SubmitForm(LetterEntry model, ActionType actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.LetterEntry.Alert_Success;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, string.Empty);

                if (Session["CurrentUserID"] == null)
                {
                    var jsonData = new { flag = false, Message = Resources.LetterEntry.Alert_SessionIsNull };
                    return Json(jsonData);
                }

                model.ModifiedBy = new Guid(Convert.ToString(Session["CurrentUserID"]));
                switch (actionType)
                {
                    case ActionType.Create:
                        results = db.LetterEntry_Insert(model);
                        break;
                    case ActionType.Delete:
                        results = db.LetterEntry_Delete(model);
                        break;
                    case ActionType.ChangeStatus:
                        results = db.LetterEntry_ChangeStatus(model);
                        break;
                    default:
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "LetterEntry", new { id = model.LetterEntryID, success = successMessage });
                    string urlNew = u.Action("Create", "LetterEntry");

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
        
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];
            string getRequestId = Request["_requestId"];

            int stateCode = "1".Equals(getFilter) ? 0 : 1;
            int totalRecords = 0;

            var model = db.letterEntry.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0);
            model = string.IsNullOrEmpty(getRequestId) || "undefined".Equals(getRequestId) ? model : model.Where(x => x.RequestID == new Guid(getRequestId));

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchTemplate":
                        model = model.Where("(string.IsNullOrEmpty(it.TemplateName) ? string.Empty : it.TemplateName).StartsWith(@0)", getVal);
                        break;
                    case "searchFullName":
                        model = model.Where("(string.IsNullOrEmpty(it.FullName) ? string.Empty : it.FullName).StartsWith(@0)", getVal);
                        break;
                    case "searchAccountNo":
                        model = model.Where("(string.IsNullOrEmpty(it.AccountNo) ? string.Empty : it.AccountNo).StartsWith(@0)", getVal);
                        break;
                    case "searchRequestID":
                        model = model.Where("(string.IsNullOrEmpty(it.TicketNumber) ? string.Empty : it.TicketNumber).StartsWith(@0)", getVal);
                        break;
                    case "searchCardNo":
                        model = model.Where("(string.IsNullOrEmpty(it.CardNo) ? string.Empty : it.CardNo).StartsWith(@0)", getVal);
                        break;
                    default :
                        model = model.Where("(string.IsNullOrEmpty(it.TemplateName) ? string.Empty : it.TemplateName).StartsWith(@0)", getVal);
                        break;
                }
            }

            totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "TemplateName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TemplateName) : model.OrderBy(x => x.TemplateName);
                    break;
                case "LetterDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LetterDate) : model.OrderBy(x => x.LetterDate);
                    break;
                case "TicketNumber":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TicketNumber) : model.OrderBy(x => x.TicketNumber);
                    break;
                case "FullName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.FullName) : model.OrderBy(x => x.FullName);
                    break;
                case "AccountNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.AccountNo) : model.OrderBy(x => x.AccountNo);
                    break;
                case "CardNo":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CardNo) : model.OrderBy(x => x.CardNo);
                    break;
                case "CreatedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LetterEntryID) : model.OrderBy(x => x.LetterEntryID);
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
                       .OrderBy(x => x.LetterNo)
                       .Select(x => new
                       {
                           Id = x.LetterEntryID,
                           TemplateID = x.TemplateID,
                           TemplateName = x.TemplateName,
                           LetterDate = x.LetterDate,
                           RequestID = x.TicketNumber,
                           FullName = x.FullName,
                           AccountNo = x.AccountNo,
                           CardNo = x.CardNo,
                           CreatedOn = x.CreatedOn,
                           StateCode = x.StateCode,
                           StatusCode = x.StatusCode,
                           DeletionStateCode = x.DeletionStateCode
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

    }
}
