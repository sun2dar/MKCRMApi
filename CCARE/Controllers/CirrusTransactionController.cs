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
using System.Data.SqlClient;
using System.Globalization;

namespace CCARE.Controllers
{
    public class CirrusTransactionController : Controller
    {
        private HistDb db = new HistDb();
        private CRMDb crmDB = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        public Dictionary<string, string> getListMapping(string categoryName, string attributeName)
        {
            var dict = crmDB.mapping.Where(x => x.CategoryName.Equals(categoryName) && x.AttributeName.Equals(attributeName))
                        .Select(x => new { x.Code, x.Label }).Distinct().ToDictionary(x => x.Code, x => x.Label);

            return dict;
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getCardNo = (Request["CardNo"].Trim() != "") ? Request["CardNo"].Trim() : null;
            string getAccountNo = (Request["AccountNo"].Trim() != "") ? Request["AccountNo"].Trim() : null;
            DateTime getTransDate1 = DateTime.ParseExact(Request["TransDate1"], "dd/MM/yyyy", null);
            DateTime getTransDate2;
            if (Request["TransDate2"] != "")
                getTransDate2 = DateTime.ParseExact(Request["TransDate2"], "dd/MM/yyyy", null).AddDays(1);
            else
                getTransDate2 = getTransDate1.AddDays(1);

            var culture = CultureInfo.GetCultureInfo("de-DE");
            Dictionary<string, string> MappingCurrencyCode = getListMapping("CurrencyCode", "CurrencyCode");

            var data = db.GetHistoryCirrusTransactions(getAccountNo, getCardNo, Formatter.FormatDateHist(getTransDate1), Formatter.FormatDateHist(getTransDate2)).ToList()
                                .Select(x => new
                                {
                                    AccountNo = x.AccountNo,
                                    AmountIDR = x.AmountIDR,
                                    AmountUSD = x.AmountUSD,
                                    Branch = x.Branch,
                                    CardNo = x.CardNo,
                                    Cirrus = x.Cirrus,
                                    Complaint = x.Complaint,
                                    Currency = (MappingCurrencyCode.ContainsKey(x.Currency)) ? MappingCurrencyCode[x.Currency] : "",
                                    Date = x.Date,
                                    Isis = x.Isis,
                                    Location = x.Location,
                                    MUAsal = x.MUAsal,
                                    Partial = x.Partial,
                                    Rate = string.Format(culture, "{0:n}", Int32.Parse(x.Rate.Replace(",00", ""))),
                                    Reason = x.Reason,
                                    RequestID = x.RequestId,
                                    ResponseCode = x.ResponseCode,
                                    Reversal = x.Reversal,
                                    SequenceNo = x.SequenceNo,
                                    TerminalID = x.TerminalId,
                                    Trace = x.Trace,
                                    Transaction = x.Transaction,
                                    UserID = x.UserId,
                                    DateOnly = x.DateOnly,
                                    Time = x.Time,
                                    Date2 = x.Date.ToString()
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

        [HttpPost]
        public ActionResult Update(CirrusTransaction model)
        {
            return formSubmit(model, "Update");
        }


        public ActionResult formSubmit(CirrusTransaction model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string CardNo = Request["CardNo"].Trim();
            DateTime TransactionDate = DateTime.Parse(Request["TransactionDate"].Trim());
            string SequenceNo = Request["SequenceNo"].Trim();
            string RequestID = Request["RequestID"].Trim();

            db.UpdateHistoryCirrusTransaction(CardNo, TransactionDate, SequenceNo, RequestID);

            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            string url = u.Action("Index", "CirrusController");

            var jsonData = new { flag = true, Message = url };
            return Json(jsonData);
        }

    }
}
