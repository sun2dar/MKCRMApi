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
    public static class EmailInboxMapping
    {
        public const string refNo = "pk_id";
        public const string custId = "fk_custid";
        public const string subject = "subject";
        public const string createdDate = "createdDate";
        public const string agentFlag = "agentFlag";
    }
    public class EmailInbox
    {
        public string Id { get; set; }
        public string NoReferensi { get; set; }
        public string Pengirim { get; set; }
        public string Subject { get; set; }
        public string Tanggal { get; set; }
        public string StatusTerakhir { get; set; }
    }
    public static class EmailInboxByRefNoMapping
    {
        public const string refNo = "pk_id";
        public const string custId = "fk_custid";
        public const string subject = "subject";
        public const string createdDate = "createddate";
        public const string content = "content";
        public const string agentFlag = "agentflag";
        public const string agentId = "agentid";
    }
    public class EmailInboxByRefNo
    {
        public string Id { get; set; }
        public string NoReferensi { get; set; }
        public string Pengirim { get; set; }
        public string Subject { get; set; }
        public string Tanggal { get; set; }
        public string StatusTerakhir { get; set; }
        public string Content { get; set; }
        public string AgentId { get; set; }
    }
    public static class DateTimeExtensions
    {
        /// <summary>
        /// First tick of the day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime DayMin(this DateTime date)
        {
            return date.Date;   // minimum of this day
        }


        /// <summary>
        /// Last tick of the day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime DayMax(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);   // last tick of this day
        }
    }

    [OutputCache(NoStore = true, Duration = 0)]
    public class EmailInboxController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getType = Request["_type"];
            string getVal = Request["_val"];
            string getTujuan = "%" + Request["_tujuan"] + "%";
            string getStartDate = Request["_startDate"];
            string getEndDate = Request["_endDate"];

            DateTime sd;
            if (!DateTime.TryParse(getStartDate, out sd))
            {
                // handle parse failure
            }
            else
            {
                getStartDate = sd.ToString("MM/dd/yyyy HH:mm:ss");
            }
            if (!DateTime.TryParse(getEndDate, out sd))
            {
                // handle parse failure
            }
            else
            {
                getEndDate = sd.DayMax().ToString("MM/dd/yyyy HH:mm:ss");
            }

            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData esbData = new ESBData() { Result = new List<StringDictionary>() };

            Object[] data = new Object[1];
            int pageSize = 0;
            int totalRecords = 0;
            int totalPages = 0;

            bool checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailInbox", "SPV");

            switch (getType)
            {
                case "inbox":                   
                    
                    paramList.RequestTransType = "InqAllSendedEmail";
                    paramList.Parameter.Add("UserLoc", "SENT");
                    paramList.WSDL = "ESBDBDelimiter";

                    List<EmailInbox> listEmailInboxs = new List<EmailInbox>();

                    esbData = EAI.RetrieveESBData(paramList);

                    if (esbData != null && esbData.Result.Count != 0)
                    {
                        foreach (StringDictionary kvp in esbData.Result)
                        {
                            EmailInbox emailInbox = new EmailInbox();
                            emailInbox.Id = kvp[EmailInboxMapping.refNo];
                            emailInbox.NoReferensi = kvp[EmailInboxMapping.refNo];
                            emailInbox.Pengirim = kvp[EmailInboxMapping.custId];
                            emailInbox.Subject = kvp[EmailInboxMapping.subject];
                            emailInbox.Tanggal = kvp[EmailInboxMapping.createdDate];

                            string dateString, format;
                            DateTime result;
                            CultureInfo provider = CultureInfo.InvariantCulture;

                            // Parse date-only value with invariant culture.
                            dateString = emailInbox.Tanggal;
                            format = "M/d/yyyy h:mm:ss tt";
                            try
                            {
                                result = DateTime.ParseExact(dateString, format, provider);
                                Console.WriteLine("{0} converts to {1}.", dateString, result.ToString());
                                emailInbox.Tanggal = result.ToString("dd/MM/yyyy HH:mm:ss");
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("{0} is not in the correct format.", dateString);
                            } 
                            emailInbox.StatusTerakhir = kvp[EmailInboxMapping.agentFlag];
                            listEmailInboxs.Add(emailInbox);
                        }
                    }
                    else
                    {
                        //ViewBag.Message = esbData.Message;
                    }
                    var temp = listEmailInboxs
                                .Select(x => new
                                {
                                    Id = x.Id,
                                    NoReferensi = x.NoReferensi,
                                    Pengirim = x.Pengirim,
                                    Subject = x.Subject,
                                    Tanggal = x.Tanggal,
                                    StatusTerakhir = x.StatusTerakhir
                                }).AsQueryable();
                    pageSize = rows;
                    totalRecords = temp.Count();
                    totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                    temp = temp.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
                    data = temp.ToArray();
                    break;
            }


            var recordCount = totalRecords;
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = recordCount;
            jTable.rows = data;

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

    }
}
