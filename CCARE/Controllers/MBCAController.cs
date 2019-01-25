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
    public class MBCAController : Controller
    {
        public CRMDb db = new CRMDb();
        public int entityType = 2;

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
        public PartialViewResult MBCAPartial()
        {
            Channel channel = new Channel();
            channel = GetChannel();

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            List<MBCAInfo> model = new List<MBCAInfo>();

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            if (!string.IsNullOrEmpty(channel.HpNo))
            {
                /* Changes MBLF 02 11 2016 */
                // param.RequestTransType = "GetMBankInfoByMobileNo";
                param.RequestTransType = "GetMBLFInfoByMobileNo";
                param.Parameter.Add("mobileNo", channel.HpNo);
            }
            else if (!string.IsNullOrEmpty(channel.CardNo))
            {

                /* Changes MBLF 02 11 2016 */
                // param.RequestTransType = "GetMBankInfoByATMNo";
                param.RequestTransType = "GetMBLFInfoByATMNo";
                param.Parameter.Add("atmNo", channel.CardNo);
            }

            /* Changes MBLF 02 11 2016 */
            // model = ChannelInquiry.MBCA(param);
            model = ChannelInquiry.MBLF(param);

            ViewBag.searchResultsCount = model.Count;

            ViewBag.DDL_Reason = new SelectList(StatusLabel.GetChangeStatusReason(entityType), "Value", "Text");
            ViewBag.Channel = channel;

            if (model.Count > 0)
            {
                if (!string.IsNullOrEmpty(channel.HpNo))
                {
                    if (model.Count == 1)
                    {
                        MBCAInfo mbcafirst = model.First();
                        return PartialView("_MBCAInfo", mbcafirst);    
                    }
                    else {
                        ViewBag.searchResults = model;
                        return PartialView("_MBCAInfoList", model);
                    }
                }
                else if (!string.IsNullOrEmpty(channel.CardNo))
                {
                    ViewBag.searchResults = model;
                    return PartialView("_MBCAInfoList", model);
                }
            }
            return PartialView("_MBCAInfo");
        }

        public JsonResult ChangeBlockStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();
            Guid userguid = new Guid(currentuser);
            SystemUser user = db.systemUser.Single(p => p.SystemUserId == userguid);
            string usernum = String.IsNullOrEmpty(user.TandemUserNum) ? String.Empty : user.TandemUserNum;
            string usergroup = String.IsNullOrEmpty(user.TandemUserGroup) ? String.Empty : user.TandemUserGroup;

            string mobileno = Request["MobileNo"];
            string newstatus = Request["NewStatus"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeMBStatusByMobileNo";
            param.Parameter.Add("mobileNo", mobileno);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("agentId", currentusername);
            param.Parameter.Add("sUserNum", usernum);
            param.Parameter.Add("sUserGroup", usergroup);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.UserMB(param);
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