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

namespace CCARE.Controllers
{
    public class ATMSwitchingTransactionController : Controller
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
            Dictionary<string, string> MappingCurrencyCode = getListMapping("CurrencyCode", "CurrencyCode");
            string getCardNo = (Request["CardNo"].Trim() != "") ? Request["CardNo"].Trim() : null;
            string getAccountNo = (Request["AccountNo"].Trim() != "") ? Request["AccountNo"].Trim() : null;
            DateTime getTransDate1 = DateTime.ParseExact(Request["TransDate1"], "dd/MM/yyyy", null);
            DateTime getTransDate2;
            if (Request["TransDate2"] != "")
                getTransDate2 = DateTime.ParseExact(Request["TransDate2"], "dd/MM/yyyy", null).AddDays(1);
            else
                getTransDate2 = getTransDate1.AddDays(1);

            var data = db.GetHistoryATMSwitchingTransactions(getAccountNo, getCardNo, Formatter.FormatDateHist(getTransDate1), Formatter.FormatDateHist(getTransDate2)).ToList()
                                .Select(x => new
                                {
                                    Branch = x.Branch,
                                    Cirrus = x.Cirrus,
                                    Isis = x.Isis,
                                    TransactionCode = x.TransactionCode,
                                    RateIDR = (MappingCurrencyCode.ContainsKey(x.RateIDR)) ? MappingCurrencyCode[x.RateIDR] : "",
                                    RateUSD = x.RateUSD,
                                    Location = x.Location,
                                    CardNo = x.CardNo,
                                    AmountAsal = x.AmountAsal,
                                    AmountIDR = x.AmountIDR,
                                    AmountUSD = x.AmountUSD,
                                    AccountNo = x.AccountNo,
                                    Partial = x.Partial,
                                    Reason = x.Reason,
                                    Response = x.Response,
                                    Reversal = x.Reversal,
                                    Sequence = x.Sequence,
                                    Date = x.Date,
                                    Trace = x.Trace,
                                    TerminalID = x.TerminalId,
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

    }
}
