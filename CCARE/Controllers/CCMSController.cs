using CCARE.App_Function;
using CCARE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using System.Windows;
using System.ComponentModel;
using System.Data;
using CCARE.Models.Transaction;
using System.Collections.Specialized;

using Newtonsoft.Json;
using System.Globalization;
using CCARE.jqGrid;

namespace CCARE.Controllers
{
    public class CCMSController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getName = Request["_name"];
            string getDob = Request["_dob"];
            string getCustNo = Request["_custNo"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            if (string.IsNullOrEmpty(getCustNo))
            {
                param.RequestTransType = "GetCCAplStatusByNameAndDOB";
                param.Parameter.Add("name", getName + "%");
                param.Parameter.Add("dob", getDob.Replace("/", ""));
            }
            else
            {
                param.RequestTransType = "GetCCAplStatusByCustCC";
                param.Parameter.Add("custNo", getCustNo);
            }

            List<CreditCardApplicationStatus> data = CCMS.Inquiry(param);
            int id = -1;
            var trx = data.ToList().Select(x => new
            {
                Id = id++,
                CustomerName = x.CustomerName,
                BirthDate = x.BirthDate,
                Birthplace = x.Birthplace,
                CustomerNumber = x.CustomerNumber,
                ApplicationId = x.ApplicationId,
                ReferenceNo = x.ReferenceNo,
                Purpose = x.Purpose,
                Status = x.Status,
                CurrentHolder = x.CurrentHolder,
                DateReceived = x.DateReceived,
                OriginatingBranch = x.OriginatingBranch,
                Remark = x.Remark,
                SourceCode = x.SourceCode,
                DateCreated = x.DateCreated,
                DateRecommended = x.DateRecommended,
                DateCanceled = x.DateCanceled,
                DateVerified = x.DateVerified,
                DateApproved = x.DateApproved,
                DateReject = x.DateReject,
                Comment = x.Comment,
                Creator = x.Creator
            });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = trx.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            trx = trx.AsQueryable().OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
            var recordCount = trx.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = trx.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }
    }
}
