using CCARE.App_Function;
using CCARE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using CCARE.jqGrid;
using CCARE.Models.Transaction;
using Newtonsoft.Json;
using BCA.CRM.OSB.Model;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace CCARE.Controllers
{
    public class BCAByPhoneTransactionController : Controller
    {
        private CRMDb db = new CRMDb();
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadJqGrid()
        {
            string noRek_CustID = Request["_inputParamBBP"];
            string getStartDate = Request["_startDate"];
            string getFIID = Request["_FIID"];
            string getLNET = Request["_LNET"];

            string getSearch = Request["_searchCondition"];
            string getNextPos = Request["_nextPos"];
            string getPrevPos = Request["_prevPos"];
            string getCurrPos = Request["_currPos"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetPBTransactionByATMNoOrAcctNo";
            param.Parameter.Add("idNum", noRek_CustID);
            param.Parameter.Add("txnDate", Formatter.ToStringExact(Convert.ToDateTime(getStartDate), "yyMMdd"));
            param.Parameter.Add("fiid", getFIID);
            param.Parameter.Add("lnet", getLNET);
            param.WSDL = "PhoneBankingTransaction";

            if (getSearch == "next")
            {
                param.RequestTransType = "GetPBTransactionByATMNoOrAcctNoNext";
                param.Parameter.Add("nextPos", getNextPos);
                param.Parameter.Add("prevPos", getPrevPos);
                param.Parameter.Add("curPos", getCurrPos);
            }
            else if (getSearch == "prev")
            {
                param.RequestTransType = "GetPBTransactionByATMNoOrAcctNoPrev";
                param.Parameter.Add("nextPos", getNextPos);
                param.Parameter.Add("prevPos", getPrevPos);
                param.Parameter.Add("curPos", getCurrPos);
            }

            BCAByPhoneTransaction data = ChannelTransaction.BBP(param);

            if (string.IsNullOrEmpty(data.Msg))
            {
                ViewBag.Message = data.Msg;
            }

            var trx = ( from x in data.Transactions
                        select new
                        {
                            CustomerIDOrAccountNo = x.CustomerID,
                            TransactionDate = x.TransactionDate,
                            TransactionDescription = x.TransactionDesc,
                            ResponseCode = x.ResponseCode,
                            TransactionTime = x.TransactionTime,
                        }).ToList();

            JSONResponse result = new JSONResponse();
            result.Value = trx;
            result.Response = data.NextPos + "<#>" + data.PrevPos + "<#>" + data.CurrPos;
            ViewBag.CountDataBBP = data.Transactions.Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
