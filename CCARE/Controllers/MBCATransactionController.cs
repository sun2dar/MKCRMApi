using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CCARE.Models;
using CCARE.Models.Transaction;
using Newtonsoft.Json;
using CCARE.App_Function;

namespace CCARE.Controllers
{
    public class MBCATransactionController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getSearchby = Request["_searchby"];
            string getVal = Request["_val"];
            string getStartDate = Request["_startDate"];
            string getEndDate = Request["_endDate"];
            string getTrxType = Request["_trxType"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            param.RequestTransType = "payment".Equals(getTrxType) ? "GetMBTransactionMPayment" 
                                        : "transfer".Equals(getTrxType) ? "GetMBTransactionMTransfer" 
                                        : "commerce".Equals(getTrxType) ? "GetMBTransactionMCommerce" 
                                        : "info".Equals(getTrxType) ? "GetMBTransactionMInfo"
                                        : "admin".Equals(getTrxType) ? "GetMBTransactionMAdmin"
                                        : "otp".Equals(getTrxType) ? "GetMBTransactionOTP" 
                                        : string.Empty;

            param.Parameter.Add("mobileNo", getVal);
            param.Parameter.Add("startDate", getStartDate);
            param.Parameter.Add("endDate", getEndDate);

            MBCATransaction model = ChannelTransaction.MBCA(param);
            var trx = model.Transactions.ToList().Select(x => new
                    {
                        MiddlewareDate = x.MiddlewareDate,
                        TandemDate = x.TandemDate,
                        AccountNumber = x.AccountNumber,
                        ATMCardNumber = x.ATMCardNumber,
                        CustomerName = x.CustomerName,
                        CustomerNumber = x.CustomerNumber,
                        PaymentAccountNumber = x.PaymentAccountNumber,
                        AccountSendersNumber = x.AccountSendersNumber,
                        CodeAndBankName = x.CodeAndBankName,
                        ToAccountNumber = x.ToAccountNumber,
                        ToAccountMU = x.ToAccountMU,
                        ToAccountName = x.ToAccountName,
                        AdminType = x.AdminType,
                        TransactionType = x.TransactionType,
                        PaymentType = x.PaymentType,
                        PaymentFor = x.PaymentFor,
                        Currency = x.Currency,
                        Nominal = x.Nominal,
                        Amount = x.Amount,
                        AmountForex = x.AmountForex,
                        ExchangesRate = x.ExchangesRate,
                        Status = x.Status,
                        DescCode = x.DescCode,
                        Information = x.Information,
                        ReferenceNumber = x.ReferenceNumber
                    });
            
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = trx.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            trx = trx.Skip((page - 1) * pageSize).Take(pageSize);
            var recordCount = trx.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = trx.ToArray();

            jTable.additional = model.CustomerName + "<@z>" + model.ATMCardNumber + "<@z>" + model.Status
                                + "<@z>" + model.HandPhoneNumberOnTandem + "<@z>" + model.CustomerNameTandem + "<@z>" + model.StatusTandem;

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }
    }
}
