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
using BCA.CRM.OSB.Model;

namespace CCARE.Models
{
    public partial class ChannelInquiry
    {
        public static List<SMSBCAInfo> SMSBCA(Params request)
        {
            List<SMSBCAInfo> smsbca = new List<SMSBCAInfo>();
            string mobileNo = null;

            request.WSDL = "ESBDBDelimiter";

            ESBData data = EAI.RetrieveESBData(request);

            if (data != null && data.Result.Count > 0)
            {
                foreach (StringDictionary entry in data.Result)
                {
                    SMSBCAInfo model = new SMSBCAInfo();
                    DateTime? registrationDate = Utility.ParseExact(entry[ESBKeyValueName.SMSBCA_RegistrationDate], "yyyy/MM/dd HH:mm:ss");
                    DateTime? lastRegistrationDate = Utility.ParseExact(entry[ESBKeyValueName.SMSBCA_LastRegistrationDate], "yyyy/MM/dd HH:mm:ss");
                    DateTime? lastTransactionDate = Utility.ParseExact(entry[ESBKeyValueName.SMSBCA_LastTransactionDate], "yyyy/MM/dd HH:mm:ss");
                    DateTime? updateDate = Utility.ParseExact(entry[ESBKeyValueName.SMSBCA_UpdateDate], "yyyy/MM/dd HH:mm:ss");

                    if (!request.Parameter.ContainsKey("mobileNo"))
                    {
                        mobileNo = entry["custid"];
                    }

                    model.AccessCodeCounter = entry[ESBKeyValueName.SMSBCA_CounterCodeAccess];
                    model.AtmCardNo = request.Parameter.ContainsKey("atmNo") ? request.Parameter["atmNo"] : entry[ESBKeyValueName.SMSBCA_AtmCardNo];
                    model.CardOwnerName = entry[ESBKeyValueName.SMSBCA_CustomerName];
                    model.MobileNumber = mobileNo;
                    model.LastRegistrationDate = lastRegistrationDate;
                    model.LastTransactionDate = lastTransactionDate;
                    model.StatusKey = entry[ESBKeyValueName.SMSBCA_Status];
                    //model.Status = Utility.GetStringMap(6, 4, entry[ESBKeyValueName.SMSBCA_Status]);
                    model.Status = Utility.GetStringMap(6, 1, model.StatusKey);
                    model.RegistrationDate = registrationDate;
                    model.UpdateDate = updateDate;
                    model.UpdateOfficer = entry[ESBKeyValueName.SMSBCA_UpdateOfficer];
                    smsbca.Add(model);
                }
            }
            return smsbca;
        }
    }
}