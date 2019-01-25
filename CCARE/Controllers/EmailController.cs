using CCARE.App_Function;
using CCARE.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCARE.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class EmailController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmailDelete()
        {
            string getType = Request["_type"];
            string getNoReferensi = Request["_noReferensi"];

            ViewBag.Type = "deleteEmail";
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData esbData = new ESBData() { Result = new List<StringDictionary>() };

            Object[] data = new Object[1];
            JSONResponse jsonResponse = new JSONResponse();

            bool checkSpv = false;

            switch (getType)
            {
                case "inbox":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailInbox", "Delete");
                    break;
                case "kirimEmail":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailSend", "Delete");
                    break;
            }
            
            switch (checkSpv)
            {
                case true:
                    paramList.RequestTransType = "DeleteEmailByRefNo";
                    paramList.Parameter.Add("PK_ID", getNoReferensi);
                    
                    paramList.WSDL = "ESBDBDelimiter";

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            if (kvp.ContainsKey("result"))
                            {
                                jsonResponse.Value = new
                                {
                                    Message = Resources.Email.EmailDeleteSuccess
                                };
                                jsonResponse.Response = kvp["result"];
                            }
                        }
                    }                    
                    break;
            }

            return Json(jsonResponse);
        }

        public JsonResult EmailDetail()
        {
            string getType = Request["_type"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData esbData = new ESBData() { Result = new List<StringDictionary>() };

            Object[] data = new Object[1];
            JSONResponse jsonResponse = new JSONResponse();
            switch (getType)
            {
                case "inboxFindByPKID":
                    paramList.RequestTransType = "InqSendedEmailDetailByRefNo";
                    paramList.Parameter.Add("RefNo", getVal);
                    paramList.WSDL = "ESBDBDelimiter";

                    List<EmailInboxByRefNo> listEmailInboxsByPKID = new List<EmailInboxByRefNo>();

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            EmailInboxByRefNo model = new EmailInboxByRefNo();
                            if (kvp.ContainsKey("refNo"))
                            {
                                model.Id = kvp[EmailInboxByRefNoMapping.refNo];
                                model.NoReferensi = kvp[EmailInboxByRefNoMapping.refNo];
                            }
                            else
                            {
                                model.Id = getVal;
                                model.NoReferensi = getVal;
                            }
                            if (kvp.ContainsKey("agentFlag"))
                            {
                                model.StatusTerakhir = kvp[EmailInboxByRefNoMapping.agentFlag];
                            }
                            if (kvp.ContainsKey("agentId"))
                            {
                                model.AgentId = kvp[EmailInboxByRefNoMapping.agentId];
                            }
                            model.Pengirim = kvp[EmailInboxByRefNoMapping.custId];
                            model.Subject = kvp[EmailInboxByRefNoMapping.subject];
                            model.Tanggal = kvp[EmailInboxByRefNoMapping.createdDate];

                            string dateString, format;
                            DateTime result;
                            CultureInfo provider = CultureInfo.InvariantCulture;

                            // Parse date-only value with invariant culture.
                            dateString = model.Tanggal;
                            format = "M/d/yyyy h:mm:ss tt"; // h for 12 hour format, HH dor 24 hour format
                            try
                            {
                                result = DateTime.ParseExact(dateString, format, provider);
                                Console.WriteLine("{0} converts to {1}.", dateString, result.ToString());
                                model.Tanggal = result.ToString("dd/MM/yyyy HH:mm:ss");
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("{0} is not in the correct format.", dateString);
                            } 
                            model.Content = kvp[EmailInboxByRefNoMapping.content];
                            listEmailInboxsByPKID.Add(model);
                        }
                    }
                    var temp = listEmailInboxsByPKID
                                .Select(x => new
                                {
                                    Id = x.Id,
                                    NoReferensi = x.NoReferensi,
                                    Pengirim = x.Pengirim,
                                    Subject = x.Subject,
                                    Tanggal = x.Tanggal,
                                    StatusTerakhir = x.StatusTerakhir,
                                    Content = x.Content,
                                    AgentId = x.AgentId,
                                    Tujuan = Session["DomainUserName"].ToString(),
                                    TujuanFullName = Session["Fullname"].ToString()
                                });
                    data = temp.ToArray();
                    jsonResponse.Data = data;
                    break;
            }
            return Json(jsonResponse);
        }

        public ActionResult EmailSend()
        {
            string getType = Request["_type"];
            string getVal = Request["_val"];
            string getAgentID = Request["_pengirim"];
            string getCustID = Request["_tujuan"];
            string getSubject = Request["_subject"];
            string getContent = Request["_isiBerita"];
            string getNoReferensi = Request["_noReferensi"];
            string getUserLoc = "INBX";
            string getUserFlag = "UNRD";
            string getAgentSpvLoc = "APPV";
            string getAgentFlag = "READ";
            string getSupervisorID = "";
            string getDateTimeAgent = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string getDateTimeUser = "";

            ViewBag.Type = "sendEmail";
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData esbData = new ESBData() { Result = new List<StringDictionary>() };
            
            Object[] data = new Object[1];
            JSONResponse jsonResponse = new JSONResponse();

            bool checkSpv = false;

            switch (getType)
            {
                case "kirimEmail":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailSend", "SPV");
                    break;
                case "inbox":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailInbox", "SPV");
                    break;
                case "outbox":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailOutbox", "SPV");
                    break;
            }            

            switch (checkSpv)
            {
                case true:
                    paramList.RequestTransType = "SendEmailBySupervisor";
                    paramList.Parameter.Add("AgentID", getAgentID);
                    paramList.Parameter.Add("CustID", getCustID);
                    paramList.Parameter.Add("Subject", getSubject);
                    paramList.Parameter.Add("Content", getContent);
                    paramList.Parameter.Add("UserLoc", getUserLoc);
                    paramList.Parameter.Add("UserFlag", getUserFlag);
                    paramList.Parameter.Add("AgentSpvLoc", getAgentSpvLoc);
                    paramList.Parameter.Add("AgentFlag", getAgentFlag);
                    paramList.Parameter.Add("SupervisorID", getSupervisorID);
                    paramList.Parameter.Add("DateTimeAgent", getDateTimeAgent);
                    paramList.Parameter.Add("DateTimeUser", getDateTimeUser);
                    paramList.WSDL = "ESBDBDelimiter";

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            if (kvp.ContainsKey("result"))
                            {
                                jsonResponse.Value = new
                                {
                                    Message = Resources.Email.EmailSendSuccess
                                };
                                jsonResponse.Response = kvp["result"];
                            }
                        }
                    }
                    break;
                case false:
                    paramList.RequestTransType = "SendEmailByOperator";
                    paramList.Parameter.Add("AgentID", getAgentID);
                    paramList.Parameter.Add("CustID", getCustID);
                    paramList.Parameter.Add("Subject", getSubject);
                    paramList.Parameter.Add("Content", getContent);
                    paramList.Parameter.Add("UserLoc", "");
                    paramList.Parameter.Add("UserFlag", "");
                    paramList.Parameter.Add("AgentSpvLoc", "WTAP");
                    paramList.Parameter.Add("AgentFlag", getAgentFlag);
                    paramList.Parameter.Add("SupervisorID", getSupervisorID);
                    paramList.Parameter.Add("DateTimeAgent", getDateTimeAgent);
                    paramList.Parameter.Add("DateTimeUser", getDateTimeUser);
                    paramList.WSDL = "ESBDBDelimiter";

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            if (kvp.ContainsKey("result"))
                            {
                                jsonResponse.Value = new
                                {
                                    Message = Resources.Email.EmailSendSuccess
                                };
                                jsonResponse.Response = kvp["result"];
                            }
                        }
                    }
                    break;
            }
            return Json(jsonResponse);
        }

        public ActionResult EmailSendApprove()
        {
            string getType = Request["_type"];
            string getVal = Request["_val"];
            string getUserLoc = "INBX";
            string getUserFlag = "UNRD";
            string getAgentSpvLoc = "APPV";
            string getSupervisorID = Session["DomainUserName"].ToString();
            string getContent = Request["_isiBerita"];
            string getNoReferensi = Request["_noReferensi"];
            string getDateTimeAgent = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            ViewBag.Type = "sendEmailApprove";
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData esbData = new ESBData() { Result = new List<StringDictionary>() };

            Object[] data = new Object[1];
            JSONResponse jsonResponse = new JSONResponse();

            bool checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailSend", "SPV");

            switch (checkSpv)
            {
                case true:
                    paramList.RequestTransType = "SendApprovedEmail";
                    paramList.Parameter.Add("UserLoc", getUserLoc);
                    paramList.Parameter.Add("UserFlag", getUserFlag);
                    paramList.Parameter.Add("AgentSpvLoc", getAgentSpvLoc);
                    paramList.Parameter.Add("SupervisorID", getSupervisorID);
                    paramList.Parameter.Add("Content", getContent);
                    paramList.Parameter.Add("DateTimeAgent", getDateTimeAgent);
                    paramList.Parameter.Add("PK_ID", getNoReferensi);
                    paramList.WSDL = "ESBDBDelimiter";

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            if (kvp.ContainsKey("result"))
                            {
                                jsonResponse.Value = new
                                {
                                    Message = Resources.Email.EmailSendSuccess
                                };
                                jsonResponse.Response = kvp["result"];
                            }
                        }
                    }
                    break;
                case false:
                    break;
            }
            return Json(jsonResponse);
        }

        public ActionResult EmailSendDraft()
        {
            string getType = Request["_type"];
            string getVal = Request["_val"];      
            string getAgentID = Request["_pengirim"];
            string getCustID = Request["_tujuan"];
            string getSubject = Request["_subject"];
            string getContent = Request["_isiBerita"];
            string getNoReferensi = Request["_noReferensi"];
            string getUserLoc = "";
            string getUserFlag = "";
            string getAgentSpvLoc = "WTAP";
            string getAgentFlag = "READ";
            string getSupervisorID = "";
            string getDateTimeAgent = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string getDateTimeUser = "";

            ViewBag.Type = "sendEmail";
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData esbData = new ESBData() { Result = new List<StringDictionary>() };

            Object[] data = new Object[1];
            JSONResponse jsonResponse = new JSONResponse();

            bool checkSpv = false;

            switch (getType)
            {
                case "kirimEmail":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailSend", "SPV");
                    break;
                case "inbox":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailInbox", "SPV");
                    break;
                case "outbox":
                    checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailOutbox", "SPV");
                    break;
            }

            switch (checkSpv)
            {
                case true:
                    paramList.RequestTransType = "SendDraftEmail";
                    paramList.Parameter.Add("UserLoc", getUserLoc);
                    paramList.Parameter.Add("UserFlag", getUserFlag);
                    paramList.Parameter.Add("AgentSpvLoc", getAgentSpvLoc);
                    paramList.Parameter.Add("SupervisorID", getSupervisorID);
                    paramList.Parameter.Add("Content", getContent);
                    paramList.Parameter.Add("DateTimeAgent", getDateTimeAgent);
                    paramList.Parameter.Add("PK_ID", getNoReferensi);
                    paramList.WSDL = "ESBDBDelimiter";

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            if (kvp.ContainsKey("result"))
                            {
                                jsonResponse.Value = new
                                {
                                    Message = Resources.Email.EmailSendSuccess
                                };
                                jsonResponse.Response = kvp["result"];
                            }
                        }
                    }
                    break;
                case false:
                    paramList.RequestTransType = "SendDraftEmail";
                    paramList.Parameter.Add("UserLoc", getUserLoc);
                    paramList.Parameter.Add("UserFlag", getUserFlag);
                    paramList.Parameter.Add("AgentSpvLoc", getAgentSpvLoc);
                    paramList.Parameter.Add("SupervisorID", getSupervisorID);
                    paramList.Parameter.Add("Content", getContent);
                    paramList.Parameter.Add("DateTimeAgent", getDateTimeAgent);
                    paramList.Parameter.Add("PK_ID", getNoReferensi);
                    paramList.WSDL = "ESBDBDelimiter";

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            if (kvp.ContainsKey("result"))
                            {
                                jsonResponse.Value = new
                                {
                                    Message = Resources.Email.EmailSendSuccess
                                };
                                jsonResponse.Response = kvp["result"];
                            }
                        }
                    }
                    break;
            }
            return Json(jsonResponse);
        }

    }
}
