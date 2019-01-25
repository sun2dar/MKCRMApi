using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using System.Globalization;
using System.Configuration;
using System.Collections.Specialized;
using CCARE.Models;
using CCARE.Models.General;
using BCA.CRM.OSB.Model;
using System.Web.Script.Serialization;
using System.Web.UI;
using DevTrends.MvcDonutCaching;

using System.Resources;
using System.Collections;

namespace CCARE.Controllers
{
    public partial class CustomerController : Controller
    {
        // ArchiveDb ArDb = new ArchiveDb();
       
        //public ActionResult AuditLog(Guid id)
        //{
        //    RequestArchive model = ArDb.requestarchive.Find(id);

        //    List<RequestArchive> auditlist = ArDb.requestarchive.Where(x => x.RequestID == id).ToList();
        //    ViewBag.auditl = auditlist;

        //    return View(model);
        //}

        //public ActionResult CaseResolution(Guid id)
        //{

        //    // ArchiveDb ArDb = new ArchiveDb();
        //    RequestResolutionArchive model = ArDb.requestresoluitonarchive.Find(id);

        //    //List<TaskArchive> notelist = ArDb.taskarchive.Where(x => x.RequestID == id).ToList();
        //    //ViewBag.notel = notelist;

        //    //List<AnnotationArchive> doclist = ArDb.annotationarchive.Where(x => x.ObjectID == model.TaskID.Value).ToList();
        //    //ViewBag.docl = doclist;


        //    return View(model);
        //}

        //public ActionResult ArchiveDetail(Guid id)
        //{
        //    RequestArchive model = ArDb.requestarchive.Find(id);
        //    List<AnnotationArchive> doclist = ArDb.annotationarchive.Where(x => x.ObjectID == model.RequestID.Value).ToList();
        //    ViewBag.docl = doclist;

        //    List<RequestArchive> auditlist = ArDb.requestarchive.Where(x => x.RequestID == id).ToList();
        //    ViewBag.auditl = auditlist;

        //    List<RequestArchive> notelist = ArDb.requestarchive.Where(x => x.RequestID == id).ToList();
        //    ViewBag.notel = notelist;

        //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"\d{2}-\d{2}-\d{4}");

        //    bool correctRegex = false;
        //    if (!string.IsNullOrEmpty(model.TransactionTime))
        //    {
        //        correctRegex = r.IsMatch(model.TransactionTime);
        //    }

        //    if (correctRegex == true && !string.IsNullOrEmpty(model.TransactionTime) && model.TransactionTime.Length > 10)
        //    {
        //        ViewBag.TransactionTimeHour = model.TransactionTime[11].ToString() + model.TransactionTime[12].ToString();
        //        ViewBag.TransactionTimeMin = model.TransactionTime[14].ToString() + model.TransactionTime[15].ToString();
        //        if (model.TransactionTime.Length == 19)
        //            ViewBag.TransactionTimeSec = model.TransactionTime[17].ToString() + model.TransactionTime[18].ToString();
        //        else
        //            ViewBag.TransactionTimeSec = "00";

        //        model.TransactionTime = model.TransactionTime.Substring(0, 10);
        //    }

        //    if (!string.IsNullOrEmpty(model.LoanNo))
        //    {
        //        ViewBag.CustomerProduct = "loan";
        //    }
        //    else if (!string.IsNullOrEmpty(model.CustomerNo))
        //    {
        //        ViewBag.CustomerProduct = "creditcard";
        //    }
        //    else if (!string.IsNullOrEmpty(model.AccountNo) || !string.IsNullOrEmpty(model.CardNo))
        //    {
        //        ViewBag.CustomerProduct = "deposit";
        //    }


        //    return View(model);
        //}
        ////public ActionResult Archive(string id)
        //public ActionResult Archive(Guid id)
        //{
        //    //RequestArchive model = ArDb.requestarchive.Where(x=>x.CISNumber == id).FirstOrDefault();

        //    //  RequestArchive model = ArDb.requestarchive.Where(x => x.CustomerID == id).FirstOrDefault();

        //    Customer model = db.customer.Find(id);

        //    //List<RequestArchive> notelist = ArDb.requestarchive.Where(x => x.CISNumber == id).ToList();
        //    //List<RequestArchive> notelist = ArDb.requestarchive.Where(x => x.CustomerID == id).ToList();
        //    if (id != null)
        //    {
        //        ViewBag.years = from n in Enumerable.Range(0, 10) select DateTime.Now.Year - n;
        //    }
        //    return View(model);
        //}

        //public ActionResult ArchiveLetterEntries(Guid id)
        //{

        //    // LetterEntryArchive model = ArDb.letterentriesarchive.Where(x=> x.RequestID == id).FirstOrDefault();
        //    RequestArchive model = ArDb.requestarchive.Find(id);


        //    return View(model);
        //}

        //public ActionResult ArchiveTasks(Guid id)
        //{
        //    //TaskArchive model = ArDb.taskarchive.Where(x => x.RequestID == id).FirstOrDefault();
        //    RequestArchive model = ArDb.requestarchive.Find(id);
        //    return View(model);
        //}

        //public JsonResult LoadLetterEntry(string sidx, string sord, int rows, int page = 1)
        //{
        //    string getParam = Request["_param"];
        //    string getVal = Request["_val"];
        //    string getFilter = Request["_filter"];
        //    string getRequestId = Request["_requestId"].ToUpper();

        //    int stateCode = "1".Equals(getFilter) ? 0 : 1;
        //    int totalRecords = 0;

        //    var model = ArDb.letterentriesarchive.Where(x => x.RequestID == new Guid(getRequestId));
        //    //model = string.IsNullOrEmpty(getRequestId) ? model : model.Where(x => x.RequestID == new Guid(getRequestId));

        //    if (!string.IsNullOrEmpty(getVal))
        //    {
        //        switch (getParam)
        //        {
        //            case "searchTemplate":
        //                model = model.Where("(string.IsNullOrEmpty(it.TemplateName) ? string.Empty : it.TemplateName).StartsWith(@0)", getVal);
        //                break;
        //            case "searchFullName":
        //                model = model.Where("(string.IsNullOrEmpty(it.FullName) ? string.Empty : it.FullName).StartsWith(@0)", getVal);
        //                break;
        //            case "searchAccountNo":
        //                model = model.Where("(string.IsNullOrEmpty(it.AccountNo) ? string.Empty : it.AccountNo).StartsWith(@0)", getVal);
        //                break;
        //            case "searchRequestID":
        //                model = model.Where("(string.IsNullOrEmpty(it.TicketNumber) ? string.Empty : it.TicketNumber).StartsWith(@0)", getVal);
        //                break;
        //            case "searchCardNo":
        //                model = model.Where("(string.IsNullOrEmpty(it.CardNo) ? string.Empty : it.CardNo).StartsWith(@0)", getVal);
        //                break;
        //            default:
        //                model = model.Where("(string.IsNullOrEmpty(it.TemplateName) ? string.Empty : it.TemplateName).StartsWith(@0)", getVal);
        //                break;
        //        }
        //    }

        //    totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

        //    int pageIndex = Convert.ToInt32(page) - 1;
        //    int pageSize = rows;
        //    int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

        //    switch (sidx)
        //    {
        //        case "TemplateName":
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TemplateName) : model.OrderBy(x => x.TemplateName);
        //            break;
        //        case "LetterDate":
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LetterDate) : model.OrderBy(x => x.LetterDate);
        //            break;
        //        case "TicketNumber":
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TicketNumber) : model.OrderBy(x => x.TicketNumber);
        //            break;
        //        case "FullName":
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.FullName) : model.OrderBy(x => x.FullName);
        //            break;
        //        case "AccountNo":
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.AccountNo) : model.OrderBy(x => x.AccountNo);
        //            break;
        //        case "CardNo":
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CardNo) : model.OrderBy(x => x.CardNo);
        //            break;
        //        case "CreatedOn":
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
        //            break;
        //        default:
        //            model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LetterEntryID) : model.OrderBy(x => x.LetterEntryID);
        //            break;
        //    }

        //    if (model.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
        //    {
        //        page--;
        //        if (page < 1)
        //        {
        //            page = 1;
        //        }
        //    }

        //    model = model.Skip((page - 1) * pageSize).Take(pageSize);

        //    var data = model
        //               .OrderBy(x => x.LetterNo)
        //               .Select(x => new
        //               {
        //                   Id = x.LetterEntryID,
        //                   TemplateName = x.TemplateName,
        //                   LetterDate = x.LetterDate,
        //                   RequestID = x.TicketNumber,
        //                   FullName = x.FullName,
        //                   AccountNo = x.AccountNo,
        //                   CardNo = x.CardNo,
        //                   CreatedOn = x.CreatedOn,
        //                   // StateCode = x.StateCode,
        //                   // StatusCode = x.StatusCode,
        //                   //  DeletionStateCode = x.DeletionStateCode
        //               });

        //    if (data.Count() == 0)
        //    {
        //        page--;
        //        if (page < 1)
        //        {
        //            page = 1;
        //        }
        //        totalPages = page;
        //        totalRecords = page * 10 - data.Count();
        //    }

        //    JSONTable jTable = new JSONTable();
        //    jTable.total = totalPages;
        //    jTable.page = page;
        //    jTable.records = totalRecords;
        //    jTable.rows = data.ToArray();

        //    return Json(jTable, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult LoadArchive(string sidx, string sord, int rows, int page = 1)
        //{
        //    string getParam = Request["_param"];
        //    string hid = Request["_hid"];
        //    string getVal = Request["_val"];
        //    string getYear = Request["_year"];
        //    var model = ArDb.requestarchive.Where(x => x.CustomerID == new Guid(hid));
        //    // var model = ArDb.requestarchive.Where(x => x.CISNumber == hid);
        //    //int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

        //    if (getVal != "")
        //    {
        //        getYear = "";
        //        switch (getParam)
        //        {
        //            case "searchRequestId":
        //                model = model.Where(x => x.TicketNumber == getVal);
        //                break;
        //            case "searchCIS":
        //                model = model.Where(x => x.CISNumber == getVal);
        //                break;
        //            //case "searchYear":
        //            //     DateTime year = Convert.ToDateTime(getYear + "-01-01");
        //            //    model = model.Where(x => x.CreatedOn.Value.Year == year.Year);

        //            //    break;
        //        }
        //    }
        //    if (getYear != "")
        //    {
        //        DateTime year = Convert.ToDateTime(getYear + "-01-01");
        //        model = model.Where(x => x.CreatedOn.Value.Year == year.Year);

        //    }
        //    var data = model
        //        .OrderBy(x => x.CreatedOn)
        //        .Select(x => new
        //        {
        //            Id = x.RequestID,
        //            requestId = x.TicketNumber,
        //            CustomerName = x.CustomerName,
        //            CISNumber = x.CISNumber,
        //            productIdName = x.ProductName,
        //            categoryIdName = x.CategoryName,
        //            Status = x.StatusLabel,
        //            CreatedOn = x.CreatedOn
        //        });

        //    int pageIndex = Convert.ToInt32(page) - 1;
        //    int pageSize = rows;
        //    int totalRecords = data.Count();
        //    int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

        //    data = data.Skip((page - 1) * pageSize).Take(pageSize);

        //    var recordCount = data.Count();
        //    JSONTable jTable = new JSONTable();
        //    jTable.total = totalPages;
        //    jTable.page = page;
        //    jTable.records = totalRecords;
        //    jTable.rows = data.ToArray();

        //    return Json(jTable, JsonRequestBehavior.AllowGet);
        //}

        //// historyarchive
        //public JsonResult LoadTask(string sidx, string sord, int rows, int page = 1)
        //{
        //    string getRequestID = Request["_requestID"];

        //    //var data = db.task.Where(x => x.RequestID == new Guid(getRequestID) && x.TaskStatusLabel != "Canceled" && x.TaskStatusLabel != "Completed").ToList()
        //    //var data = ArDb.taskarchive.Where(x => x.RequestID == new Guid(getRequestID)).ToList()
        //    var data = ArDb.requestresoluitonarchive.Where(x => x.RequestID == new Guid(getRequestID)).ToList()
        //        .Select(x => new
        //        {
        //            Id = x.ActivityId,
        //            ActivityTypeLabel = x.ActivityTypeLabel,
        //            ActivityStatusCode = x.StatusCode,
        //            ActivityStatusLabel = x.StatusCodeLabel,
        //            Subject = x.Subject,
        //            CreatedByName = x.CreatedByName,
        //            CreatedOn = x.CreatedOn
        //        });

        //    int pageIndex = Convert.ToInt32(page) - 1;
        //    int pageSize = rows;
        //    int totalRecords = data.Count();
        //    int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

        //    data = data.Skip((page - 1) * pageSize).Take(pageSize);

        //    var recordCount = data.Count();
        //    JSONTable jTable = new JSONTable();
        //    jTable.total = totalPages;
        //    jTable.page = page;
        //    jTable.records = totalRecords;
        //    jTable.rows = data.ToArray();

        //    return Json(jTable, JsonRequestBehavior.AllowGet);
        //}
    }
}
