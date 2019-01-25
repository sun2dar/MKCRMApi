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
    public class SMSBCAController : Controller
    {
        public CRMDb db = new CRMDb();
        public int entityType = 6;

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
        public PartialViewResult SMSBCAPartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            List<SMSBCAInfo> model = new List<SMSBCAInfo>();

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            if (channel != null)
            {
                string mobileNo = null;
                if (!string.IsNullOrEmpty(channel.HpNo))
                {
                    mobileNo = channel.HpNo;
                    
                    param.RequestTransType = "GetSMSBankingInfoByMobileNo";
                    param.Parameter.Add("mobileNo", channel.HpNo);
                }
                else if (!string.IsNullOrEmpty(channel.CardNo))
                {
                    //wiwik 20151124 param.WSDL = "ESBDBDelimiter";
                    param.RequestTransType = "GetSMSBankingInfoByATMNo";
                    param.Parameter.Add("atmNo", channel.CardNo);
                }
                model = ChannelInquiry.SMSBCA(param);
            }

            ViewBag.searchResultsCount = model.Count;

            ViewBag.DDL_Reason = new SelectList(StatusLabel.GetChangeStatusReason(entityType), "Value", "Text");
            ViewBag.Channel = channel;

            if (model.Count > 0)
            {
                if (!string.IsNullOrEmpty(channel.HpNo))
                {
                    SMSBCAInfo smsbcafirst = model.First();
                    return PartialView("_SMSBCAInfo", smsbcafirst);
                }
                else
                {
                    ViewBag.searchResults = model;
                    return PartialView("_SMSBCAInfoList", model);
                }
            }
            return PartialView("_SMSBCAInfo");
        }

        public JsonResult ChangeBlockStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string mobileno = Request["MobileNo"];
            string newstatus = Request["NewStatus"];
            string reason = Request["Reason"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeSMSBankStatusByMobileNo";
            param.Parameter.Add("mobileNo", mobileno);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("agentId", currentusername);
            param.Parameter.Add("reason", reason);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.UserSMSBanking(param);
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
