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
    public class SMSTopUpTransactionController : Controller
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
            param.RequestTransType = "GetTopUpTransactionByMobileNo";
            param.Parameter.Add("mobileNo", getVal);
            param.Parameter.Add("startDate", getStartDate);
            param.Parameter.Add("endDate", getEndDate);

            SMSTopUpTransaction model = ChannelTransaction.SMSTopUp(param);
            var trx = model.Transactions.ToList().Select(x => new
            {
                AtmCardNumber = x.AtmCardNumber,
                TransactionDate = x.TransactionDate,
                RequestId = x.RequestId,
                AccountNumber = x.AccountNumber,
                ValueOfTransactions = x.ValueOfTransactions,
                ResponseCode = x.ResponseCode,
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

            jTable.additional = model.ATMCardHolderName + "<@z>" + model.ATMCardNumber + "<@z>" + model.Status;

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

    }
}
