using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Globalization;
using CCARE.App_Function;

namespace CCARE.Models
{
    public class CCMS
    {
        public static List<CreditCardApplicationStatus> Inquiry(Params request)
        {
            List<CreditCardApplicationStatus> records = new List<CreditCardApplicationStatus>();
            try
            {
                request.WSDL = "ESBDBDelimiter";
                ESBData data = EAI.RetrieveESBData(request);
                if (data.Result != null && data.Result.Count > 0)
                {
                    CreditCardApplicationStatus record = null;
                    foreach (StringDictionary entry in data.Result)
                    {
                        record = new CreditCardApplicationStatus();
                        DateTime? birthDate = Formatter.ParseExact(entry["dob"], "ddMMyyyy HH:mm:ss");
                        DateTime? approvedDate = Formatter.ParseExact(entry["date_approved"], "ddMMyyyy HH:mm:ss");
                        DateTime? canceledDate = Formatter.ParseExact(entry["date_cancelled"], "ddMMyyyy HH:mm:ss");
                        DateTime? createdDate = Formatter.ParseExact(entry["date_created"], "ddMMyyyy HH:mm:ss");
                        DateTime? receivedDate = Formatter.ParseExact(entry["date_received"], "ddMMyyyy HH:mm:ss");
                        DateTime? recommendedDate = Formatter.ParseExact(entry["date_recommended"], "ddMMyyyy HH:mm:ss");
                        DateTime? rejectDate = Formatter.ParseExact(entry["date_reject"], "ddMMyyyy HH:mm:ss");
                        DateTime? verifiedDate = Formatter.ParseExact(entry["date_verified"], "ddMMyyyy HH:mm:ss");

                        record.CustomerName = entry["fullname"];
                        record.BirthDate = Formatter.ToStringExact(birthDate, "dd/MM/yyyy HH:mm:ss");
                        record.Birthplace = entry["birth_place"];
                        record.CustomerNumber = entry["cust_no"];
                        record.ApplicationId = entry["app_id"];
                        record.ReferenceNo = entry["ref_no"];
                        record.Purpose = entry["purpose"];
                        record.Status = entry["status"];
                        record.CurrentHolder = entry["current_holder"];
                        record.OriginatingBranch = entry["originator_branch"];
                        record.Remark = entry["remark"];
                        record.SourceCode = entry["source_code"];
                        record.DateReceived = Formatter.ToStringExact(receivedDate, "dd/MM/yyyy HH:mm:ss");
                        record.DateCreated = Formatter.ToStringExact(createdDate, "dd/MM/yyyy HH:mm:ss");
                        record.DateRecommended = Formatter.ToStringExact(recommendedDate, "dd/MM/yyyy HH:mm:ss");
                        record.DateCanceled = Formatter.ToStringExact(canceledDate, "dd/MM/yyyy HH:mm:ss");
                        record.DateVerified = Formatter.ToStringExact(verifiedDate, "dd/MM/yyyy HH:mm:ss");
                        record.DateApproved = Formatter.ToStringExact(approvedDate, "dd/MM/yyyy HH:mm:ss");
                        record.DateReject = Formatter.ToStringExact(rejectDate, "dd/MM/yyyy HH:mm:ss");
                        record.Comment = entry["comment"];
                        record.Creator = entry["creator"];
                        
                        records.Add(record);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return records;
        }
    }
}