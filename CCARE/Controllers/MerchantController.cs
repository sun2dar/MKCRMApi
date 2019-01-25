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
    public class MerchantController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inquiry()
        {
            string getVal = Request["_val"];
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetMerchantInfoByTerminalId";
            param.Parameter.Add("merchantId", getVal);
            Merchant model = MerchantInquiry.Search(param);
            string json = JsonConvert.SerializeObject(model);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
