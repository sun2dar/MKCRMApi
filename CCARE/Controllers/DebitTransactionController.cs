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
using System.Globalization;
using System.Collections.Specialized;

namespace CCARE.Controllers
{
    public class DebitTransactionController : Controller
    {
        private HistDb db = new HistDb();
        
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            if (Request["Tab"] == "History")
            {
                string getCardNo = (Request["CardNo"].Trim() != "") ? Request["CardNo"].Trim() : null;
                string getAcctNo = (Request["AccountNo"].Trim() != "") ? Request["AccountNo"].Trim() : null;
                DateTime getTransDate1 = DateTime.ParseExact(Request["TransDate1"], "dd/MM/yyyy", null);
                DateTime getTransDate2;
                if (Request["TransDate2"] != "")
                    getTransDate2 = DateTime.ParseExact(Request["TransDate2"], "dd/MM/yyyy", null).AddDays(1);
                else
                    getTransDate2 = getTransDate1.AddDays(1);

                var data = db.GetHistoryDebitTransactions(getAcctNo, getCardNo, Formatter.FormatDateHist(getTransDate1), Formatter.FormatDateHist(getTransDate2)).ToList()
                                    .Select(x => new
                                    {
                                        MerchantID = x.MerchantId,
                                        TerminalID = x.TerminalId,
                                        Batch = x.Batch,
                                        Date = x.Date,
                                        DateOnly = x.DateOnly,
                                        Time = x.Time,
                                        TrTp = x.TrTp,
                                        CardNo = x.CardNo,
                                        AccountNo = x.AccountNo,
                                        Amount = x.Amount,
                                        Cash = x.Cash,
                                        ResponseCode = x.ResponseCode,
                                        ApprCd = x.ApprCd,
                                        TraceNo = x.TraceNo,
                                        RefNo = x.RefNo,
                                        Cashier = x.Cashier,
                                        RequestID = x.RequestId,
                                        Merchant = x.Merchant,
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
            else
            {
                var culture = CultureInfo.GetCultureInfo("de-DE");
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

                var trx = CurrentTransaction.Debit(param);
                var data = trx.Select(x => new
                {
                    Number = x.Number,
                    TransactionCode = x.TransactionCode,
                    TransactionDate = x.TransactionDate,
                    TransactionTime = x.TransactionTime,
                    Amount = string.Format(culture, "{0:n}", Int32.Parse(x.Amount)),
                    ResponseCode = x.ResponseCode,
                    ApprovalCode = x.ApprovalCode,
                    AtmCardNo = x.AtmCardNo,
                    TerminalId = x.TerminalId,
                    CashAmount = string.Format(culture, "{0:n}", Int32.Parse(x.CashAmount)),
                    AccountNumber = x.AccountNumber,
                    Retailer = x.Retailer,
                    TraceNo = x.TraceNo,
                    Batch = x.Batch,
                    SequenceNo = x.SequenceNo,
                    CardType = x.CardType
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
        public ActionResult Update(DebitTransaction model)
        {
            return formSubmit(model, "Update");
        }


        public ActionResult formSubmit(DebitTransaction model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string CardNo = Request["CardNo"].Trim();
            DateTime TransactionDate = DateTime.Parse(Request["TransactionDate"].Trim());
            string RefNo = Request["RefNo"].Trim();
            string RequestID = Request["RequestID"].Trim();    
                
            db.UpdateHistoryDebitTransaction(CardNo, TransactionDate, RefNo, RequestID);

            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            string url = u.Action("Index", "DebitController");
            
            var jsonData = new { flag = true, Message = url };
            return Json(jsonData);
        }
    }
}
