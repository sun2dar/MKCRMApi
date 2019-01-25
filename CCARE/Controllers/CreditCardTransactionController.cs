using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.Models.Transaction;
using CCARE.App_Function;
using Newtonsoft.Json;

namespace CCARE.Controllers
{
    public class CreditCardTransactionController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inquiry()
        {
            string getVal = Request["_val"];
            string getTrxType = Request["_trxType"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            if (getVal.Length == 16)
            {
                param.Parameter.Add("cardNo", getVal);
            }
            else
            {
                param.Parameter.Add("custNo", getVal);
            }


            if ("outstanding".Equals(getTrxType))
            {
                CreditCardOutstandingTransaction model = CCTransaction.OutstandingTransactions(param);
                model.getTotalNominal();
                model.Sampling();
                var outstandingData = new { CustomerNo = model.CustomerNo, Total = model.TotalAmount, grid = model.TransactionData };
                return Json(outstandingData, JsonRequestBehavior.AllowGet);
            }
            
            if ("current".Equals(getTrxType))
            {
                CreditCardCurrentTransaction model = CCTransaction.CurrentTransactions(param);
                model.getTotalAmount();
                model.Sampling();
                var currentData = new { CustomerNo = model.CustomerNo, Total = model.TotalAmount, grid = model.TransactionData };
                return Json(currentData, JsonRequestBehavior.AllowGet);
            }
            
            if ("history".Equals(getTrxType))
            {
                CreditCardRetrieveMultipleStatementDate st = CreditCardService.RetrieveCreditCardStatementDates(param);
                string julianformat = string.Empty;
                param.Parameter.Add("statementDate", st.Statements.Count > 0 ? st.Statements.First().StatementDateInJulianFormat : "");
                CreditCardHistoricalTransaction model = CCTransaction.HistoryTransactions(param);
                model.CompletingInformation(st);
                var historicalData = new { Information = model.Information, StatementDates = st.Statements, grid = model.TransactionData };
                return Json(historicalData, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransactionsByStatement()
        {
            string getNo = Request["_no"];
            string getStatementDate = Request["_statementDate"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            if (getNo.Length == 16)
            {
                param.Parameter.Add("cardNo", getNo);
            }
            else 
            {
                param.Parameter.Add("custNo", getNo);            
            }

            param.Parameter.Add("statementDate", getStatementDate);

            CreditCardHistoricalTransaction model = CCTransaction.HistoryTransactions(param);

            double newBalance = 0;
            double newBill = 0;

            foreach (CreditCardInformation entry in model.CreditCards)
            {
                IEnumerable<CreditCardTransaction> creditCardTransactions =
                                        from transaction in model.Transactions
                                        where transaction.CreditCardNo == entry.CreditCardNo
                                        select transaction;

                double? negativeAmount = (from creditCardTransaction in creditCardTransactions
                                          where CCTransaction.HistoricalTransactionCodesForNegativeAmount.Contains(creditCardTransaction.TransactionCode)
                                          select creditCardTransaction.Amount).Sum();

                double? positiveAmount = (from creditCardTransaction in creditCardTransactions
                                          where !CCTransaction.HistoricalTransactionCodesForNegativeAmount.Contains(creditCardTransaction.TransactionCode)
                                          select creditCardTransaction.Amount).Sum();

                double? previousBalance = (from creditCardTransaction in creditCardTransactions
                                           select creditCardTransaction.PreviousBalance).Sum();

                double? subTotal = positiveAmount - negativeAmount + previousBalance;

                if (subTotal.HasValue)
                {
                    newBalance += subTotal.Value;
                    newBill = newBalance - ((double)model.Information.OldBalance) + ((double)model.Information.Credit);
                }

                entry.PrevBalance = previousBalance;
                entry.SubTotal = subTotal;
            }

            model.Information.NewBalance = newBalance;
            model.Information.NewBill = newBill;

            model.Sampling();

            var historicalData = new { Information = model.Information, grid = model.TransactionData };
            return Json(historicalData, JsonRequestBehavior.AllowGet);
        }
    }
}
