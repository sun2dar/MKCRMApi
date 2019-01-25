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

    public static class EmailOutboxMapping
    {
        public const string refNo = "pk_id";
        public const string custId = "fk_custid";
        public const string subject = "subject";
        public const string createdDate = "createddate";
        public const string supervisorId = "supervisorid";
        public const string agentSpvLoc = "agentspvloc";
    }
    public class EmailOutbox
    {
        public string Id { get; set; }
        public string NoReferensi { get; set; }
        public string Tujuan { get; set; }
        public string Subject { get; set; }
        public string Tanggal { get; set; }
        public string SpvAppv { get; set; }
        public string StatusTerakhir { get; set; }
    }

    [OutputCache(NoStore = true, Duration = 0)]
    public class EmailOutboxController : Controller
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

            switch (checkSpv)
            {
                case true:
                        paramList.RequestTransType = "InqReceivedEmailforSPV";
                        paramList.Parameter.Add("UserLoc1", "INBX");
                        paramList.Parameter.Add("UserLoc2", " ");
                        paramList.Parameter.Add("StartDate", getStartDate);
                        paramList.Parameter.Add("EndDate", getEndDate);
                        paramList.Parameter.Add("CustId", getTujuan);
                    break;
                case false:
                        string dateString, format;
                        DateTime result;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        // Parse date-only value with invariant culture.
                        format = "MM/dd/yyyy HH:mm:ss"; // h for 12 hour format, HH dor 24 hour format
                        try
                        {
                            result = DateTime.ParseExact(getStartDate, format, provider);
                            getStartDate = result.ToString("MM/dd/yyyy");

                            result = DateTime.ParseExact(getEndDate, format, provider);
                            getEndDate = result.AddDays(1).ToString("MM/dd/yyyy");
                        }
                        catch (FormatException)
                        {
                            //Console.WriteLine("{0} is not in the correct format.", dateString);
                        }
                        paramList.RequestTransType = "InqReceivedEmailforOPR";
                        paramList.Parameter.Add("AgentId", Session["DomainUserName"].ToString());
                        paramList.Parameter.Add("UserLoc1", "INBX");
                        paramList.Parameter.Add("UserLoc2", " ");
                        paramList.Parameter.Add("StartDate", getStartDate.Substring(0,10));
                        paramList.Parameter.Add("EndDate", getEndDate.Substring(0, 10));
                        paramList.Parameter.Add("CustId", getTujuan);
                    break;
            }

            paramList.WSDL = "ESBDBDelimiter";

            List<EmailOutbox> listEmailOutboxs = new List<EmailOutbox>();

            esbData = EAI.RetrieveESBData(paramList);

            if (esbData != null && esbData.Result.Count != 0)
            {
                foreach (StringDictionary kvp in esbData.Result)
                {
                    EmailOutbox model = new EmailOutbox();
                    model.Id = kvp[EmailOutboxMapping.refNo];
                    model.NoReferensi = kvp[EmailOutboxMapping.refNo];
                    model.Tujuan = kvp[EmailOutboxMapping.custId];
                    model.Subject = kvp[EmailOutboxMapping.subject];
                    model.SpvAppv = kvp[EmailOutboxMapping.supervisorId];
                    model.StatusTerakhir = kvp[EmailOutboxMapping.agentSpvLoc];
                    model.Tanggal = kvp[EmailOutboxMapping.createdDate];

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
                    
                    listEmailOutboxs.Add(model);
                }
            }
            else
            {
                //ViewBag.Message = esbData.Message;
            }
            var tempOutbox = listEmailOutboxs
                        .Select(x => new
                        {
                            Id = x.Id,
                            NoReferensi = x.NoReferensi,
                            Tujuan = x.Tujuan,
                            Subject = x.Subject,
                            Tanggal = x.Tanggal,
                            SpvAppv = x.SpvAppv,
                            StatusTerakhir = x.StatusTerakhir
                        }).AsQueryable();
            pageSize = rows;
            totalRecords = tempOutbox.Count();
            totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            tempOutbox = tempOutbox.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
            data = tempOutbox.ToArray();


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
