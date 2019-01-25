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

namespace CCARE.Controllers
{
    public class ATMTransactionController : Controller
    {
        private HistDb db = new HistDb();
        private CRMDb crmDB = new CRMDb();

        public Dictionary<string, string> getListMapping(string categoryName, string attributeName)
        {
            var dict = crmDB.mapping.Where(x => x.CategoryName.Equals(categoryName) && x.AttributeName.Equals(attributeName))
                        .Select(x => new { x.Code, x.Label }).Distinct().ToDictionary(x => x.Code, x => x.Label);

            return dict;
        }

        public ActionResult Index()
        {
            return View();
        }



        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            if (Request["Tab"] == "History" && Request["SearchTerminalID"] != "Yes")
            {
                Dictionary<string, string> MappingCurrencyCode = getListMapping("CurrencyCode", "CurrencyCode");
                string getCardNo = (Request["CardNo"].Trim() != "") ? Request["CardNo"].Trim() : null;
                string getFrAcctNo = (Request["FromAccount"].Trim() != "") ? Request["FromAccount"].Trim() : null;
                DateTime getTransDate1 = DateTime.ParseExact(Request["TransDate1"], "dd/MM/yyyy", null);
                DateTime getTransDate2;

                if (Request["TransDate2"] != "")
                    getTransDate2 = DateTime.ParseExact(Request["TransDate2"], "dd/MM/yyyy", null).AddDays(1);
                else
                    getTransDate2 = getTransDate1.AddDays(1);

                var data = db.GetHistoryATMTransactions(getCardNo, getFrAcctNo, null, Formatter.FormatDateHist(getTransDate1), Formatter.FormatDateHist(getTransDate2)).ToList()
                                .Select(x => new
                                {
                                    TransactionDate = x.TransactionDate,
                                    Terminal = x.Terminal,
                                    CardNo = x.CardNo,
                                    TransactionCode = x.TransactionCode,
                                    FromAccount = x.FromAccount,
                                    Amount = x.Amount,
                                    Company = x.Company,
                                    ToAccount = x.ToAccount,
                                    Rate = x.Rate,
                                    Currency = (MappingCurrencyCode.ContainsKey(x.Currency)) ? MappingCurrencyCode[x.Currency] : "",
                                    Forex = x.Forex,
                                    ResponseCode = x.ResponseCode,
                                    Response = x.Response,
                                    Sequence = x.Sequence,
                                    RequestID = x.RequestID,
                                    DateOnly = x.DateOnly,
                                    Time = x.Time,
                                    Location = x.Location,
                                    TransactionDescription = x.TransactionDescription,
                                    Date2 = x.TransactionDate.ToString()
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
            else if (Request["Tab"] == "History" && Request["SearchTerminalID"] == "Yes")
            {
                Dictionary<string, string> MappingCurrencyCode = getListMapping("CurrencyCode", "CurrencyCode");
                string getTerminalID = Request["TerminalID"].Trim();
                string getSequenceNo = Request["SequenceNo"].Trim();

                var data = db.GetHistoryATMTransactionsByTerminalID(getTerminalID, getSequenceNo).ToList()
                                .Select(x => new
                                {
                                    TransactionDate = x.TransactionDate,
                                    Terminal = x.Terminal,
                                    CardNo = x.CardNo,
                                    TransactionCode = x.TransactionCode,
                                    FromAccount = x.FromAccount,
                                    Amount = x.Amount,
                                    Company = x.Company,
                                    ToAccount = x.ToAccount,
                                    Rate = x.Rate,
                                    Currency = (MappingCurrencyCode.ContainsKey(x.Currency)) ? MappingCurrencyCode[x.Currency] : "",
                                    Forex = x.Forex,
                                    ResponseCode = x.ResponseCode,
                                    Response = x.Response,
                                    Sequence = x.Sequence,
                                    RequestID = x.RequestID,
                                    DateOnly = x.DateOnly,
                                    Time = x.Time,
                                    Location = x.Location,
                                    TransactionDescription = x.TransactionDescription,
                                    Date2 = x.TransactionDate.ToString()
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
            else
            {
                Dictionary<string, string> MappingTransactionCode = getListMapping("TransactionType", "ATM Current");
                Dictionary<string, string> MappingCurrencyCode = getListMapping("CurrencyCode", "CurrencyCode");

                string atmNo = Request["ATMNo"];
                string lnet = Request["LNET"];
                string fiid = Request["FIID"];
                string terminalId = Request["TerminalID"];
                string trxDate = Formatter.ToStringExact(DateTime.ParseExact(Request["TransDate"], "dd/MM/yyyy", null), "MM/dd/yyyy");

                string startDate = trxDate + " 00:00:00";
                string endDate = trxDate + " 23:59:59";

                Params param = new Params() { Parameter = new Dictionary<string, string>() };

                param.Parameter.Add("atmNo", atmNo);
                param.Parameter.Add("startDate", startDate);
                param.Parameter.Add("endDate", endDate);
                param.Parameter.Add("lnet", lnet);
                param.Parameter.Add("fiid", fiid);
                if (!string.IsNullOrEmpty(terminalId))
                {
                    param.Parameter.Add("terminalId", terminalId);
                }

                var culture = CultureInfo.GetCultureInfo("de-DE");
                var trx = CurrentTransaction.ATM(param);
                var data = trx.Select(x => new
                {
                    Number = x.Number,
                    TransactionCode = x.TransactionCode,
                    TransactionDate = x.TransactionDate,
                    Amount1 = string.Format(culture, "{0:n}", Int32.Parse(x.Amount1)),
                    ResponseCode = x.ResponseCode,
                    ResponseDescription = x.ResponseDescription,
                    TransactionDescription = (MappingTransactionCode.ContainsKey(x.TransactionCode)) ? MappingTransactionCode[x.TransactionCode] : "",
                    Currency = (MappingCurrencyCode.ContainsKey(x.Currency)) ? MappingCurrencyCode[x.Currency] : "",
                    Terminal = x.Terminal,
                    Rate = string.Format(culture, "{0:n}", Int32.Parse(x.Rate)),
                    ConversionAmount = x.ConversionAmount,
                    Amount2 = x.Amount2,
                    FromAccount = x.FromAccount,
                    ToAccount = x.ToAccount,
                    PayeeCode = x.PayeeCode,
                    PayeeNumber = x.PayeeNumber,
                    SequenceNumber = x.SequenceNumber
                }
                );

                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;
                int totalRecords = trx.Count();
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
        }

        [HttpPost]
        public ActionResult Update(ATMTransaction model)
        {
            string CardNo = Request["CardNo"];
            string TransactionDate = Request["TransactionDate"].Trim();
            string SequenceNo = Request["SequenceNo"].Trim();
            string RequestID = Request["RequestID"].Trim();
            return formSubmit(model, "Update");
        }


        public ActionResult formSubmit(ATMTransaction atmTransaction, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string CardNo = Request["CardNo"].Trim();
            DateTime TransactionDate = DateTime.Parse(Request["TransactionDate"].Trim());
            string SequenceNo = Request["SequenceNo"].Trim();
            string RequestID = Request["RequestID"].Trim();

            db.UpdateHistoryATMTransaction(CardNo, TransactionDate, SequenceNo, RequestID);

            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            string url = u.Action("Index", "ATMController");

            var jsonData = new { flag = true, Message = url };
            return Json(jsonData);
        }
    }
}
