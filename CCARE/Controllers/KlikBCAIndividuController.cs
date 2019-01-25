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
    public class KlikBCAIndividuController : Controller
    {
        public CRMDb db = new CRMDb();
        public int entityType = 1;

        private Channel GetChannel()
        {
            Channel channel = new Channel();
            if(!string.IsNullOrEmpty(Request["customerId"])) channel.CustomerId = new Guid(Request["customerId"]);
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
        public PartialViewResult KBIKlikBCAIndividuPartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            List<KlikBCAIndividuInfo> model = new List<KlikBCAIndividuInfo>();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            
            if (!string.IsNullOrEmpty(channel.UserId))
            {
                param.Parameter.Add("userId", channel.UserId);
                model = ChannelInquiry.KlikBCAIndividual(param, channel);
            }
            else if (!string.IsNullOrEmpty(channel.CardNo))
            {
                model = ChannelInquiry.KlikBCAIndividual(param, channel);

            }
            else if (!string.IsNullOrEmpty(channel.EmailAddress))
            {
                Params paramEmail = new Params() { Parameter = new Dictionary<string, string>() };
                paramEmail.RequestTransType = "GetIBankInfoByEmail";
                paramEmail.Parameter.Add("EmailAddress", channel.EmailAddress);

                model = ChannelInquiry.KlikBCAIndividual(paramEmail, channel);
            }

            ViewBag.searchResults = model;
            ViewBag.Channel = channel;

            ViewBag.searchResultsCount = model.Count == 0 ? 0 : model.Count == 1 ? 1 : model.Count;

            if (!string.IsNullOrEmpty(channel.EmailAddress)) return PartialView("_KBIList", model);

            if (model.Count == 1)
            {
                return PartialView("_KBIInfo", model.First());
            }
            else if (model.Count == 0)
            {
                return PartialView("_KBIInfo", new KlikBCAIndividuInfo());
            }
            else
            {
                return PartialView("_KBIList", model);
            }
        }

        public PartialViewResult KBICCConnectionPartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            List<CreditCardConnectionInfo> model = ChannelInquiry.KBICreditCardConnection(channel);

            ViewBag.searchResultsCount = model.Count;
            ViewBag.searchResults = model;
            return PartialView("_KBICCConnection", model);
        }

        public PartialViewResult KBICCApplicationPartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            List<CreditCardApplicationInfo> model = ChannelInquiry.KBICreditCardApplication(channel);

            ViewBag.searchResultsCount = model.Count;
            ViewBag.searchResults = model;
            return PartialView("_KBICCApplication", model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult KBIKeyBCAConnectionPartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            List<KeyBCAConnectionInfo> model = ChannelInquiry.KBIKeyBCAConnection(channel);

            ViewBag.searchResultsCount = model.Count;
            ViewBag.searchResults = model;
            ViewBag.Channel = channel;
            return PartialView("_KBIKeyBCAConnection", model);
        }

        public PartialViewResult KBIKeyBCAApplicationPartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            List<ApplicationKeyBCAIBInfo> model = ChannelInquiry.KBIApplicationKeyBCAIB(channel);

            ViewBag.searchResultsCount = model.Count;
            ViewBag.searchResults = model;
            return PartialView("_KBIKeyBCAApplication", model);
        }

        public JsonResult ChangeUserIdStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string userid = Request["UserId"];
            string newstatus = Request["NewStatus"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeIBILoginByUserId";
            param.Parameter.Add("userId", userid);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("agentId", currentusername);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.LoginIBI(param);
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

        public JsonResult ChangeBlockStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();
            Guid userguid = new Guid(currentuser);
            SystemUser user = db.systemUser.Single(p => p.SystemUserId == userguid);
            string usernum = String.IsNullOrEmpty(user.TandemUserNum) ? String.Empty : user.TandemUserNum;
            string usergroup = String.IsNullOrEmpty(user.TandemUserGroup) ? String.Empty : user.TandemUserGroup;

            string userid = Request["UserId"];
            string newstatus = Request["NewStatus"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeIBIStatusByUserId";
            param.Parameter.Add("userId", userid);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("agentId", currentusername);
            param.Parameter.Add("sUserNum", usernum);
            param.Parameter.Add("sUserGroup", usergroup);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.UserIBI(param);
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


        public JsonResult ChangeMalwareStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string userid = Request["UserId"];
            string newstatus =  Request["NewStatus"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            ChangeStatusResult result = new ChangeStatusResult();

            param.Parameter.Add("cust_id", userid);
            param.Parameter.Add("update_officer", currentusername);

            if (newstatus.Equals("N"))
            {
                param.RequestTransType = "ReleaseMalwareStatusByUserId";
                result = StatusUpdate.BlockMalwareIB(param);
            }
            else 
            {
                param.RequestTransType = "BlockMalwareStatusByUserId";
                result = StatusUpdate.ReleaseMalwareIB(param);
            }

            string start = DateTime.Now.ToString();
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
