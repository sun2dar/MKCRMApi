using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CCARE.Models;
using CCARE.Models.Transaction;
using Newtonsoft.Json;
using CCARE.App_Function;


namespace CCARE.Controllers
{
    public class AccountStatementController : Controller
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

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "AccountStatement";
            param.Parameter.Add("acctNo", getVal);
            param.Parameter.Add("startDate", getStartDate.Replace("/", "").Trim());
            param.Parameter.Add("endDate", getEndDate.Replace("/", "").Trim());
            AllChannelTransaction data = AccountStatement.Mutasi(param, Session["isSuperAdmin"].ToString());
            var trx = data.Transactions.ToList().Select(x => new
            {
                Number = x.Number,
                displayTransactionDate = x.displayTransactionDate,
                TransactionDate = x.TransactionDate,
                TransactionType = x.TransactionType,
                TransactionDescription = x.TransactionDescription,
                Branch = x.Branch,
                Amount = x.Amount
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

            string restricted = "Y".Equals(Session["isSuperAdmin"].ToString())? string.Empty : data.RestrictedAccount ? "Restricted Account" : string.Empty;
            jTable.additional = data.Name + "<@z>" + data.Currency + "<@z>" + restricted;

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }
    }
}
