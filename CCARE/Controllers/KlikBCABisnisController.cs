//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using System.Globalization;
using System.Collections.Specialized;
using BCA.CRM.OSB.Model;
using CCARE.Models;
using CCARE.Models.General;

namespace CCARE.Controllers
{
    public class KlikBCABisnisController : Controller
    {
        public CRMDb db = new CRMDb();
        public int entityType = 5;

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
        public PartialViewResult KBBKlikBCABisnisPartial(string userIdInput = null)
        {
            Channel channel = new Channel();
            channel = GetChannel();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            List<KlikBCABisnisInfo> model = new List<KlikBCABisnisInfo>();

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            if (!string.IsNullOrEmpty(channel.CorpId) && !string.IsNullOrEmpty(userIdInput))
            {
                param.RequestTransType = "GetUserKBBInfo";
                param.Parameter.Add("corpId", channel.CorpId.ToUpperInvariant());
                param.Parameter.Add("userId", userIdInput.ToUpperInvariant());
                channel.UserId = userIdInput;
            }
            else if (!string.IsNullOrEmpty(channel.CorpId) && !string.IsNullOrEmpty(channel.UserId))
            {
                param.RequestTransType = "GetUserKBBInfo";
                param.Parameter.Add("corpId", channel.CorpId.ToUpperInvariant());
                param.Parameter.Add("userId", channel.UserId.ToUpperInvariant());
            }
            else if (!string.IsNullOrEmpty(channel.EmailAddress))
            {
                //// Converting the Email Address to Uppercase, as the ESB expects the Email in Uppercase.
                param.RequestTransType = "GetKBBEmailInfo";
                param.Parameter.Add("Email1", channel.EmailAddress.ToUpperInvariant());
                param.Parameter.Add("Email2", channel.EmailAddress.ToUpperInvariant());
            }
            else if (!string.IsNullOrEmpty(channel.AccountNo))
            {
                param.RequestTransType = "GetKBBInfoByAcctNo";
                param.Parameter.Add("AccountNo", channel.AccountNo);
            }

            model = ChannelInquiry.KBB(param, channel, userIdInput);

            ViewBag.searchResults = model;
            ViewBag.searchResultsCount = model.Count;
            ViewBag.Channel = channel;

            if (model.Count > 0)
            {
                KlikBCABisnisInfo kbbfirst = model.First();
                kbbfirst.CorpID = String.IsNullOrEmpty(kbbfirst.CorpID) ? channel.CorpId : kbbfirst.CorpID;
                kbbfirst.UserID = String.IsNullOrEmpty(kbbfirst.UserID) ? channel.UserId : kbbfirst.UserID;

                if (!string.IsNullOrEmpty(channel.CorpId) && !string.IsNullOrEmpty(channel.UserId))
                {
                    ViewBag.Mode = "KBB";
                    return PartialView("_KBBLogin", kbbfirst);
                }
                else if (!string.IsNullOrEmpty(channel.EmailAddress))
                {
                    return PartialView("_KBBList", model);
                }
                else if (!string.IsNullOrEmpty(channel.AccountNo))
                {
                    ViewBag.Mode = "AccountNo";
                    return PartialView("_KBBLogin", kbbfirst);
                }
                else
                {
                    ViewBag.Mode = "KBB";
                    return PartialView("_KBBLogin", kbbfirst);
                }
            }
            else
            {
                ViewBag.Mode = "KBB";
                return PartialView("_KBBLogin");
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult KBBKeyBCAConnectionPartial(string requestToken = null, string userIdInput = null)
        {
            Channel channel = new Channel();
            channel = GetChannel();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            List<KlikBCABisnisInfo> model = ChannelInquiry.KBBKeyBCAConnection(channel, requestToken, userIdInput);
            
            ViewBag.searchResults = model;
            ViewBag.searchResultsCount = model.Count;
            ViewBag.Channel = channel;
            return PartialView("_KBBKeyBCAConnection", model);
        }

        public PartialViewResult KBBKeyBCAApplicationPartial(string requestToken = null)
        {
            Channel channel = new Channel();
            channel = GetChannel();

            List<KlikBCABisnisInfo> model = ChannelInquiry.KBBKeyBCAApplication(channel, requestToken);

            ViewBag.searchResults = model;
            ViewBag.searchResultsCount = model.Count;
            ViewBag.Channel = channel;
            return PartialView("_KBBKeyBCAApplication", model);
        }

        public JsonResult ChangeUserIdStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string userid = Request["UserId"];
            string corpid = Request["CorpId"];
            string newstatus = Request["NewStatus"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeKBBLoginByUserId";
            param.Parameter.Add("userId", userid);
            param.Parameter.Add("corpId", corpid);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("operator", currentusername);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.LoginSME(param);
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
