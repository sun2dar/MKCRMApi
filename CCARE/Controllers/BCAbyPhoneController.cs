//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using CCARE.App_Function;
using System.Globalization;
using System.Collections.Specialized;
using BCA.CRM.OSB.Model;
using CCARE.Models;

namespace CCARE.Controllers
{
    public class BCAbyPhoneController : Controller
    {
        public CRMDb db = new CRMDb();
        public int entityType = 4;

        private Channel GetChannel()
        {
            Channel channel = new Channel();
            if (!string.IsNullOrEmpty(Request["customerId"])) channel.CustomerId = new Guid(Request["customerId"]);
            channel.ChannelTypeId = string.IsNullOrEmpty(Request["channelTypeId"]) ? 0 : int.Parse(Request["channelTypeId"]);
            channel.ChannelType = string.IsNullOrEmpty(Request["channelType"]) ? string.Empty : Request["channelType"];
            channel.CustomerIdName = string.IsNullOrEmpty(Request["customerIdName"]) ? string.Empty : Request["customerIdName"];
            channel.CardNo = string.IsNullOrEmpty(Request["cardNo"]) ? string.Empty : Request["cardNo"];
            channel.UserId = string.IsNullOrEmpty(Request["userId"]) ? string.Empty : Request["userId"];
            channel.EmailAddress = string.IsNullOrEmpty(Request["emailAddress"]) ? string.Empty : Request["emailAddress"];
            channel.HpNo = string.IsNullOrEmpty(Request["hpNo"]) ? string.Empty : Request["hpNo"];
            channel.CorpId = string.IsNullOrEmpty(Request["corpId"]) ? string.Empty : Request["corpId"];
            channel.AccountNo = string.IsNullOrEmpty(Request["accountNo"]) ? string.Empty : Request["accountNo"];
            channel.CustomerNo = string.IsNullOrEmpty(Request["customerNo"]) ? string.Empty : Request["customerNo"];
            channel.SNKeyBCA = string.IsNullOrEmpty(Request["snKeyBCA"]) ? string.Empty : Request["snKeyBCA"];
            channel.Category = string.IsNullOrEmpty(Request["category"]) ? string.Empty : Request["category"];
            return channel;
        }

        public ActionResult Index()
        {
            Channel channel = new Channel();
            channel = GetChannel();
            return View(channel);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult BCAbyPhonePartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            BCAByPhoneInfo bcabyphone = new BCAByPhoneInfo();

			Params param = new Params() { Parameter = new Dictionary<string, string>() };

			string atmNo = null;
			if (channel != null)
			{
                if (!string.IsNullOrEmpty(channel.CardNo))
				{
                    atmNo = channel.CardNo;
				}
                else if (!string.IsNullOrEmpty(channel.AccountNo))
				{
                    param.WSDL = "UserPhoneBankingInquiry";
                    param.RequestTransType = "GetPBInfoByAcctNo";
                    param.Parameter.Add("acctNo", channel.AccountNo);
                    atmNo = ChannelInquiry.AtmNoBCAByPhone(param);
				}

				if (!string.IsNullOrEmpty(atmNo))
				{
                    param.WSDL = "UserPhoneBankingManagement";
                    param.RequestTransType = "GetPBInfoByCardNo";
                    param.Parameter.Add("atmNo", atmNo);
                    bcabyphone = ChannelInquiry.BCAByPhone(param);
                    if (string.IsNullOrEmpty(channel.CardNo))
                    {
                        channel.CardNo = bcabyphone.CustomerId;
                    }
				}
			}
            
            if (string.IsNullOrEmpty(bcabyphone.CustomerId))
            {
                ViewBag.searchResultsCount = 0;
            }
            else
            {
                ViewBag.searchResultsCount = 1;
            }

            ViewBag.searchResults = bcabyphone;
            ViewBag.Channel = channel;
            return PartialView("_BCAbyPhone", bcabyphone);
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

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangePBStatusByAtmNo";
            param.Parameter.Add("atmNo", cardno);
            param.Parameter.Add("sUserNum", usernum);
            param.Parameter.Add("sUserGroup", usergroup);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.UserPhoneBanking(param);
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

        public JsonResult ChangePinCounter()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();
            Guid userguid = new Guid(currentuser);
            SystemUser user = db.systemUser.Single(p => p.SystemUserId == userguid);
            string usernum = String.IsNullOrEmpty(user.TandemUserNum) ? String.Empty : user.TandemUserNum;
            string usergroup = String.IsNullOrEmpty(user.TandemUserGroup) ? String.Empty : user.TandemUserGroup;

            string cardno = Request["CardNo"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangePBResetPinCounterByAtmNo";
            param.Parameter.Add("atmNo", cardno);
            param.Parameter.Add("sUserNum", usernum);
            param.Parameter.Add("sUserGroup", usergroup);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.UserPhoneBanking(param);
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
