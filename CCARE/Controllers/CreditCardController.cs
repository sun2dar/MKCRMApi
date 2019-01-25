//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using System.Globalization;
using System.Collections.Specialized;
using BCA.CRM.OSB.Model;
using CCARE.Models.General;
using CCARE.Filters;

namespace CCARE.Controllers
{
    public class CreditCardController : Controller
    {
        private CRMDb db = new CRMDb();
        public int entityType = 13;
     
        private CreditCard GetCreditCard()
        {
            CreditCard creditcard = new CreditCard();
            if (!string.IsNullOrEmpty(Request["Id"])) creditcard.Id = new Guid(Request["Id"]);
            creditcard.CreditCardNumber = string.IsNullOrEmpty(Request["CreditCardNumber"]) ? string.Empty : Request["CreditCardNumber"];
            creditcard.CardholderName = string.IsNullOrEmpty(Request["CardholderName"]) ? string.Empty : Request["CardholderName"];
            creditcard.CardType = string.IsNullOrEmpty(Request["CardType"]) ? string.Empty : Request["CardType"];
            creditcard.CCType = string.IsNullOrEmpty(Request["CCType"]) ? string.Empty : Request["CCType"];
            creditcard.CreditCardCustomerNo = string.IsNullOrEmpty(Request["CreditCardCustomerNo"]) ? string.Empty : Request["CreditCardCustomerNo"];
            if (!string.IsNullOrEmpty(Request["CustomerId"])) creditcard.CustomerId = new Guid(Request["CustomerId"]);
            creditcard.CustomerIdName = string.IsNullOrEmpty(Request["CustomerIdName"]) ? string.Empty : Request["CustomerIdName"];
            creditcard.Status = string.IsNullOrEmpty(Request["Status"]) ? string.Empty : Request["Status"];
            return creditcard;
        }

        public ActionResult Index()
        {
            CreditCard model = new CreditCard();
            model = GetCreditCard();
            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CreditCardInfoPartial()
        {
            CreditCard creditcard = new CreditCard();
            creditcard = GetCreditCard();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.Parameter.Add("cardNo", creditcard.CreditCardNumber);

            CreditCardInfo creditCard = ProductInquiry.CreditCard(param);

            ViewBag.searchResults = creditCard;
            if (string.IsNullOrEmpty(creditCard.CreditCardNumber))
            {
                ViewBag.searchResultsCount = 0;
            }
            else
            {
                ViewBag.searchResultsCount = 1;
            }
            ViewBag.CreditCard = creditcard;
            return PartialView("_CreditCardInfo", creditCard);
        }

        public ActionResult CreditCardAutopayPartial()
        {
            CreditCard creditcard = new CreditCard();
            creditcard = GetCreditCard();

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetAutoPayInfoByCC";
            param.Parameter.Add("cardNo", creditcard.CreditCardNumber);

            List<CreditCardAutoPay> autopay = ProductInquiry.AutoPay(param);

            ViewBag.searchResultsCount = autopay.Count();
            ViewBag.searchResults = autopay;
            return PartialView("_CreditCardAutoPay", autopay);
        }

        public JsonResult ChangeBlockStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();
            Guid userguid = new Guid(currentuser);
            SystemUser user = db.systemUser.Single(p => p.SystemUserId == userguid);
            string usernum = String.IsNullOrEmpty(user.TandemUserNum) ? String.Empty : user.TandemUserNum;
            string usergroup = String.IsNullOrEmpty(user.TandemUserGroup) ? String.Empty : user.TandemUserGroup;

            string cardno = Request["CardNo"];
            string newstatus = Request["NewStatus"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeCCStatusByCCNo";
            param.Parameter.Add("cardNo", cardno);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("sUserNum", usernum);
            param.Parameter.Add("sUserGroup", usergroup);
            param.Parameter.Add("userId", currentusername);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.Card(param);
            ChangeStatusUpdatedBy updatedby = new ChangeStatusUpdatedBy
            {
                ID = currentuser,
                Name = currentusername
            };
            result.UpdatedBy = updatedby;

            string stop = DateTime.Now.ToString();
            ChangeStatusLog.Write(param, result, start, stop);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
