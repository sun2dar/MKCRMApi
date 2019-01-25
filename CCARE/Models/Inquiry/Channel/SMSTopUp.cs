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
        public static List<SMSTopUpInfo> SMSTopUp(Params request)
        {
            List<SMSTopUpInfo> smstopup = new List<SMSTopUpInfo>();
            string mobileNo = null;

            request.WSDL = "ESBDBDelimiter";

            ESBData data = EAI.RetrieveESBData(request);

            if (data != null && data.Result.Count > 0)
            {
                foreach (StringDictionary entry in data.Result)
                {
                    mobileNo = entry["custid"];
                    SMSTopUpRetrieveByMobileNumber(ref smstopup,  mobileNo);
                }
            }
            return smstopup;
        }

        public static void SMSTopUpRetrieveByMobileNumber(ref List<SMSTopUpInfo> smstopup, string mobileNo)
        {
            if (mobileNo != null)
            {
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                param.WSDL = "ESBDBDelimiter";
                param.RequestTransType = "GetTopUpInfoByMobileNo";
                param.Parameter.Add("mobileNo", mobileNo);
                ESBData data = EAI.RetrieveESBData(param);

                if (data != null && data.Result.Count != 0)
                {
                    foreach (StringDictionary entry in data.Result)
                    {
                        SMSTopUpInfo model = new SMSTopUpInfo();

                        DateTime? registrationDate = Utility.ParseExact(entry[ESBKeyValueName.SMSTopUp_RegistrationDate], "MM/dd/yyyy hh:mm:ss tt");
                        DateTime? lastRegistrationDate = Utility.ParseExact(entry[ESBKeyValueName.SMSTopUp_LastRegistrationDate], "MM/dd/yyyy hh:mm:ss tt");
                        DateTime? lastTransactionDate = Utility.ParseExact(entry[ESBKeyValueName.SMSTopUp_LastTransactionDate], "MM/dd/yyyy hh:mm:ss tt");
                        DateTime? updateDate = Utility.ParseExact(entry[ESBKeyValueName.SMSTopUp_UpdateDate], "MM/dd/yyyy hh:mm:ss tt");

                        model.AtmCardNo = entry[ESBKeyValueName.SMSTopUp_ATMCardNo];
                        model.CardholderName = entry[ESBKeyValueName.SMSTopUp_CardholderName];
                        model.MobileNumber = mobileNo;
                        model.RegistrationDate = registrationDate;
                        model.LastRegistrationDate = lastRegistrationDate;
                        model.LastTransactionDate = lastTransactionDate;
                        model.smsTopUpKey = entry[ESBKeyValueName.SMSTopUp_Status];
                        //model.smsTopUp = Utility.GetStringMap(3, 4, entry[ESBKeyValueName.SMSTopUp_Status]);
                        model.smsTopUp = Utility.GetStringMap(3, 1, model.smsTopUpKey);
                        model.CounterCodeAccess = entry[ESBKeyValueName.SMSTopUp_CounterCodeAccess];
                        model.ProviderCode = entry[ESBKeyValueName.SMSTopUp_ProviderCode];
                        model.UpdateOfficer = entry[ESBKeyValueName.SMSTopUp_UpdateOfficer];
                        model.UpdateDate = updateDate;
                        smstopup.Add(model);
                    }
                }
            }
        }
    }
}