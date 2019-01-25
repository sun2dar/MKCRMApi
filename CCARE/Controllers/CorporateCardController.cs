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
    public class CorporateCardController : Controller
    {
        private CRMDb db = new CRMDb();
        int entityType = 18;

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CorporateCardPartial()
        {
            string getType = string.IsNullOrEmpty(Request["_type"]) ? string.Empty : Request["_type"];
            string cardNo = string.IsNullOrEmpty(Request["_cardNo"]) ? string.Empty : Request["_cardNo"];
            string corpId = string.IsNullOrEmpty(Request["_corpId"]) ? string.Empty : Request["_corpId"];
            string uniquekey = string.IsNullOrEmpty(Request["_uniqueKey"]) ? string.Empty : Request["_uniqueKey"];

            ViewBag.StatusChange = String.IsNullOrEmpty(Request["statusChange"]) ? String.Empty : Request["statusChange"];
            ViewBag.RequestId = String.IsNullOrEmpty(Request["requestId"]) ? String.Empty : Request["requestId"];
            ViewBag.TicketNumber = String.IsNullOrEmpty(Request["ticketNumber"]) ? String.Empty : Request["ticketNumber"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetCorporateCardDetail";
            switch (getType) 
            { 
                case "card" :
                    param.Parameter.Add("cardNo", cardNo);
                    break;
                case "unique" :
                    param.Parameter.Add("corpId", corpId);
                    param.Parameter.Add("uniqueKey", uniquekey);
                    break;
            }
            CorporateCard model = ProductInquiry.CorporateCard(param);
            ViewBag.searchResults = model;
            ViewBag.searchResultsCount = model == null ? 0 : 1;
            ViewBag.DDL_Reason = new SelectList(StatusLabel.GetChangeStatusReason(entityType), "Value", "Text");

            return PartialView("_CorporateCard", model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CompanyListPartial()
        {
            string corpName = string.IsNullOrEmpty(Request["_corpName"]) ? string.Empty : Request["_corpName"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetListCorporateByName";
            param.Parameter.Add("comp_nm_in", corpName);
            List<Corporate> model = ProductInquiry.GetListCorporate(param);

            ViewBag.searchResultsCount = model.Count;
            ViewBag.searchResults = model;
            return PartialView("_CompanyList", model);
        }

        public JsonResult BlockCorporateCard()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string cardNo = string.IsNullOrEmpty(Request["_cardNo"]) ? string.Empty : Request["_cardNo"];
            string blockType = string.IsNullOrEmpty(Request["_blockType"]) ? string.Empty : Request["_blockType"];
            string reasonCode = string.IsNullOrEmpty(Request["_reasonCode"]) ? string.Empty : Request["_reasonCode"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeCorporateCardStatusByCardNo";
            param.Parameter.Add("cardNo", cardNo);
            param.Parameter.Add("blockType", blockType);
            param.Parameter.Add("reasonCode", reasonCode);
            param.Parameter.Add("userId", currentusername);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.CorporateCard(param);
            ChangeStatusUpdatedBy updatedby = new ChangeStatusUpdatedBy
            {
                ID = currentuser,
                Name = currentusername
            };
            result.UpdatedBy = updatedby;

            if (result.Status == ChangeStatusResultType.Success) 
            {
                BlockCorporateCardOnCAF(cardNo);
            }

            string stop = DateTime.Now.ToString();
            ChangeStatusLog.Write(param, result, start, stop);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void BlockCorporateCardOnCAF(string cardNo)
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();
            Guid userguid = new Guid(currentuser);
            SystemUser user = db.systemUser.Single(p => p.SystemUserId == userguid);
            string usernum = String.IsNullOrEmpty(user.TandemUserNum) ? String.Empty : user.TandemUserNum;
            string usergroup = String.IsNullOrEmpty(user.TandemUserGroup) ? String.Empty : user.TandemUserGroup;

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "ChangeATMStatusByAtmNo";
            param.Parameter.Add("atmNo", cardNo);
            param.Parameter.Add("newStatus", "2");
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
        }

    }
}
