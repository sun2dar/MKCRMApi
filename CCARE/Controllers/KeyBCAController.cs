using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
namespace CCARE.Controllers
{
    public class KeyBCAController : Controller
    {
        public JsonResult ChangeKeyBCAStatus()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string entityType = string.IsNullOrEmpty(Request["EntityType"]) ? "" : Request["EntityType"];
            
            string theId = string.IsNullOrEmpty(Request["TheId"]) ? "" : Request["TheId"];
            string oldstatus = string.IsNullOrEmpty(Request["OldStatus"]) ? "" : Request["OldStatus"];
            string newstatus = string.IsNullOrEmpty(Request["NewStatus"]) ? "" : Request["NewStatus"];
            string reason = string.IsNullOrEmpty(Request["Reason"]) ? "" : Request["Reason"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            if (entityType == "9")
            {
                param.WSDL = "TokenUpdateStatus";
                param.RequestTransType = "ChangeTokenStatusByKeyId";
                param.Parameter.Add("keyId", theId);
            }
            else if (entityType == "11")
            {
                param.WSDL = "TokenSMEUpdateStatus";
                param.RequestTransType = "ChangeTokenSMEStatusBySnToken";
                param.Parameter.Add("snToken", theId);
            }
            param.Parameter.Add("oldStatus", oldstatus);
            param.Parameter.Add("newStatus", newstatus);
            param.Parameter.Add("desc", reason);
            param.Parameter.Add("operator", currentusername);

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.Token(param);
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

        public JsonResult KeyBCABukaBlokir()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string entityType = string.IsNullOrEmpty(Request["EntityType"]) ? "" : Request["EntityType"];
            
            string keyid = string.IsNullOrEmpty(Request["KeyId"]) ? "" : Request["KeyId"];
            string tokenType = string.IsNullOrEmpty(Request["TokenType"]) ? "" : Request["TokenType"];
            string random = string.IsNullOrEmpty(Request["Random"]) ? "" : Request["Random"];
            string updateOfficer = currentusername;
            string message = string.Empty;
            if (tokenType == "A")
            {
                message = "TokenUnlockActiveCard";
            }
            else
            {
                message = "TokenUnlockVasco";
            }
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = message;
            param.Parameter.Add("userId", keyid);
            param.Parameter.Add("random", random);
            if (entityType == "9")
            {
                param.Parameter.Add("type", "IBN");
            }
            else if (entityType == "11")
            {
                param.Parameter.Add("type", "SME");
                param.Parameter.Add("updateOfficer", updateOfficer);
            }

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = StatusUpdate.TokenUnlock(param);
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

        public JsonResult KeyBCAWriteToLog()
        {
            string currentuser = Session["CurrentUserID"].ToString();
            string currentusername = Session["DomainUserName"].ToString();

            string entityType = string.IsNullOrEmpty(Request["EntityType"]) ? "" : Request["EntityType"];

            string createdDate = string.IsNullOrEmpty(Request["CreatedDate"]) ? "" : Request["CreatedDate"];
            string keyId = string.IsNullOrEmpty(Request["KeyId"]) ? "" : Request["KeyId"];
            string updateOfficer = currentusername;
            DateTime? dtLastUpdatedDate = Formatter.ParseExact(Request["LastUpdatedDate"], "dd/MM/yyyy HH:mm:ss");
            string lastUpdatedDate = Formatter.ToStringExact(dtLastUpdatedDate, "MM/dd/yyyy HH:mm:ss");
            string snToken = string.IsNullOrEmpty(Request["SNToken"]) ? "" : Request["SNToken"];
            string atmNo = string.IsNullOrEmpty(Request["ATMNo"]) ? "" : Request["ATMNo"];
            string firstName = string.IsNullOrEmpty(Request["FirstName"]) ? "" : Request["FirstName"];
            string applicationCode = string.IsNullOrEmpty(Request["ApplicationCode"]) ? "" : Request["ApplicationCode"];
            string description = string.IsNullOrEmpty(Request["Description"]) ? "" : Request["Description"];
            string corporateName = string.IsNullOrEmpty(Request["CorporateName"]) ? "" : Request["CorporateName"];
            string action = string.IsNullOrEmpty(Request["Action"]) ? "" : Request["Action"];

            string start = DateTime.Now.ToString();

            ChangeStatusResult result = new ChangeStatusResult();
            bool logUpdateSuccess = false;
            try
            {
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                if (entityType == "9")
                {
                    param.RequestTransType = "UpdateTokenTblKoneksiByUpdateOfficerLastUpdateKeyIdAndSnToken";
                }
                else if (entityType == "11")
                {
                    param.RequestTransType = "UpdateTokenSMETblKoneksiByUpdateOfficerLastUpdateKeyIdAndSnToken";
                }
                param.Parameter.Add("updateOfficer", updateOfficer);
                param.Parameter.Add("lastUpdateDate", lastUpdatedDate);
                param.Parameter.Add("keyId", keyId);
                param.Parameter.Add("snToken", snToken);

                result = StatusUpdate.WriteToLog(param);

                logUpdateSuccess = (result.Status == ChangeStatusResultType.Success) ? true : false;
            }
            catch (Exception e)
            {
            }

            try
            {
                string currentDate = Formatter.ToStringExact(DateTime.Now, "MM/dd/yyyy HH:mm:ss");
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                if (entityType == "9")
                {
                    param.RequestTransType = "InsertTokenBackOfficeActifityLog1";
                    param.Parameter.Add("createdDate", currentDate);
                    param.Parameter.Add("kdLog", "BKBLKR");
                    param.Parameter.Add("kdBO", "HLO");
                    param.Parameter.Add("snToken", snToken);
                    param.Parameter.Add("keyId", keyId);
                    param.Parameter.Add("atmNo", atmNo);
                    param.Parameter.Add("firstName", firstName);
                    param.Parameter.Add("aplCode", applicationCode);
                    param.Parameter.Add("statusLama", "BLK");
                    param.Parameter.Add("statusBaru", "BKA");
                    param.Parameter.Add("updateOfficer", updateOfficer);
                    param.Parameter.Add("desc", description);
                    param.Parameter.Add("action", action);
                    param.Parameter.Add("lastUpdateDate", lastUpdatedDate);
                }
                else if (entityType == "11")
                {
                    param.RequestTransType = "InsertTokenSMEBackOfficeActifityLog";
                    param.Parameter.Add("createdDate", createdDate);
                    param.Parameter.Add("kdLog", "BKBLKR");
                    param.Parameter.Add("snToken", snToken);
                    param.Parameter.Add("keyId", keyId);
                    param.Parameter.Add("cardNo", atmNo);
                    param.Parameter.Add("firstName", firstName);
                    param.Parameter.Add("kdApl", "S1ACIB9502");
                    param.Parameter.Add("statusLama", "BLK");
                    param.Parameter.Add("statusBaru", "BKA");
                    param.Parameter.Add("updateOfficer", updateOfficer);
                    param.Parameter.Add("keterangan", description);
                    param.Parameter.Add("tindakan", action);
                    param.Parameter.Add("lastUpdate", lastUpdatedDate);
                    param.Parameter.Add("corpName", corporateName);
                }
                result = StatusUpdate.WriteToLog(param);

                logUpdateSuccess = (result.Status == ChangeStatusResultType.Success) ? true : false;
            }
            catch (Exception e)
            {
            }

            if (!(logUpdateSuccess))
            {
                result.Status = ChangeStatusResultType.FailedToWriteToLog;
            }
            ChangeStatusUpdatedBy updatedby = new ChangeStatusUpdatedBy
            {
                ID = currentuser,
                Name = currentusername
            };
            result.UpdatedBy = updatedby;

            string stop = DateTime.Now.ToString();
            ChangeStatusLog.Write(null, result, start, stop);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
