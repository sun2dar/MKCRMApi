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
    public class EmailKirim
    {
        public string Id { get; set; }
        public string NoReferensi { get; set; }
        public string Dari { get; set; }
        public string Tujuan { get; set; }
        public string Subject { get; set; }
        public string Tanggal { get; set; }
    }
    public static class EmailKirimMapping
    {
        public const string refNo = "pk_id";
        public const string agentId = "agentid";
        public const string custId = "fk_custid";
        public const string subject = "subject";
        public const string createdDate = "createddate";
    }

    [OutputCache(NoStore=true,Duration=0)]
    public class EmailSendController : Controller
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

            bool checkSpv = RoleNPrivilege.checkEntityButtonByRoleID(new Guid(Session["RoleID"].ToString()), "EmailSend", "SPV");

            switch (checkSpv)
            {
                case true:
                    paramList.RequestTransType = "InqSendedEmailByUserLoc";
                        paramList.Parameter.Add("UserLoc", "");
                        paramList.Parameter.Add("UserFlag", "");
                    break;
            }

            paramList.WSDL = "ESBDBDelimiter";
            List<EmailKirim> listEmailKirims = new List<EmailKirim>();

            esbData = EAI.RetrieveESBData(paramList);

            if (esbData != null && esbData.Result.Count != 0)
            {
                foreach (StringDictionary kvp in esbData.Result)
                {
                    EmailKirim model = new EmailKirim();
                    model.Id = kvp[EmailKirimMapping.refNo];
                    model.NoReferensi = kvp[EmailKirimMapping.refNo];
                    model.Dari = kvp[EmailKirimMapping.agentId];
                    model.Tujuan = kvp[EmailKirimMapping.custId];
                    model.Subject = kvp[EmailKirimMapping.subject];
                    model.Tanggal = kvp[EmailKirimMapping.createdDate];
                    
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
                    listEmailKirims.Add(model);
                }
            }
            else
            {
                //ViewBag.Message = esbData.Message;
            }
            var tempKirim = listEmailKirims
                        .Select(x => new
                        {
                            Id = x.Id,
                            NoReferensi = x.NoReferensi,
                            Dari = x.Dari,
                            Tujuan = x.Tujuan,
                            Subject = x.Subject,
                            Tanggal = x.Tanggal
                        }).AsQueryable();
            pageSize = rows;
            totalRecords = tempKirim.Count();
            totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            tempKirim = tempKirim.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
            data = tempKirim.ToArray();


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
