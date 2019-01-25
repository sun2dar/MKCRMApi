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
    public class DepositController : Controller
    {
        private CRMDb db = new CRMDb();
        public int entityType = 14;

        private Deposit GetDeposit()
        {
            Deposit model = new Deposit();
            if (!string.IsNullOrEmpty(Request["Id"])) model.Id = new Guid(Request["Id"]);
            model.AccountNo = string.IsNullOrEmpty(Request["AccountNo"]) ? string.Empty : Request["AccountNo"];
            model.CardNo = string.IsNullOrEmpty(Request["CardNo"]) ? string.Empty : Request["CardNo"];
            model.OwnerTypeLabel = string.IsNullOrEmpty(Request["OwnerTypeLabel"]) ? string.Empty : Request["OwnerTypeLabel"];
            model.RelationType = string.IsNullOrEmpty(Request["RelationType"]) ? string.Empty : Request["RelationType"];
            if (!string.IsNullOrEmpty(Request["CustomerId"])) model.CustomerId = new Guid(Request["CustomerId"]);
            model.CustomerIdName = string.IsNullOrEmpty(Request["CustomerIdName"]) ? string.Empty : Request["CustomerIdName"]; 
            model.RelationshipWith = string.IsNullOrEmpty(Request["RelationshipWith"]) ? string.Empty : Request["RelationshipWith"]; 
            return model;
        }

        public ActionResult Index()
        {
            Deposit model = new Deposit();
            model = GetDeposit();
            return View(model);
        }

        public ActionResult DepositInfoPartial()
        {
            Deposit model = new Deposit();
            model = GetDeposit();
            
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetDepositDetailByAcctNo";
            param.Parameter.Add("acctNo", model.AccountNo);

            DepositInfo account = ProductInquiry.Account(param);

            ViewBag.searchResults = account;
            if (string.IsNullOrEmpty(account.AccountNumber))
            {
                ViewBag.searchResultsCount = 0;
            }
            else
            {
                ViewBag.searchResultsCount = 1;
            }

            return PartialView("_DepositAccountNo", account);
        }

        [OutputCache(NoStore=true,Duration=0,VaryByParam="*")]
        public ActionResult DepositCardNoPartial()
        {
            Deposit model = new Deposit();
            model = GetDeposit();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetDepositDetailByATMNo";
            param.Parameter.Add("atmNo", model.CardNo);

            DepositInfo ATMCard = ProductInquiry.ATMCard(param);

            ViewBag.searchResults = ATMCard;
            if (string.IsNullOrEmpty(ATMCard.CardNumber))
            {
                ViewBag.searchResultsCount = 0;
            }
            else
            {
                ViewBag.searchResultsCount = 1;
            }
            ViewBag.Deposit = model;
            return PartialView("_DepositCardNo", ATMCard);
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
            param.RequestTransType = "ChangeATMStatusByAtmNo";
            param.Parameter.Add("atmNo", cardno);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("sUserNum", usernum);
            param.Parameter.Add("sUserGroup", usergroup);

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
