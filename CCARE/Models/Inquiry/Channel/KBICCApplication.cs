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
        public static List<CreditCardApplicationInfo> KBICreditCardApplication(Channel channel) {
            List<CreditCardApplicationInfo> creditcardapplication = new List<CreditCardApplicationInfo>();

            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            if (!string.IsNullOrEmpty(channel.UserId))
            {
                param.RequestTransType = "GetCreditCardAppliedStatusOnIBankByUserId";
                param.Parameter.Add("userId", channel.UserId);
                param.Parameter.Add("No_Rekening", "");
                param.Parameter.Add("Status1", "NEW");
                param.Parameter.Add("Status2", "OPR");
                param.Parameter.Add("Status3", "TOL");
                param.Parameter.Add("Status4", "HPS");
                param.Parameter.Add("Status5", "TOS");
                param.Parameter.Add("Status6", "HPX");
                param.WSDL = "ESBDBDelimiter";
                data = EAI.RetrieveESBData(param);
            }
            else if (!string.IsNullOrEmpty(channel.CustomerNo))
            {
                param.RequestTransType = "GetCreditCardAppliedStatusOnIBankByCustCC";
                param.Parameter.Add("KeyId", "");
                param.Parameter.Add("No_Rekening", channel.CustomerNo);
                param.Parameter.Add("Status1", "NEW");
                param.Parameter.Add("Status2", "OPR");
                param.Parameter.Add("Status3", "TOL");
                param.Parameter.Add("Status4", "HPS");
                param.Parameter.Add("Status5", "TOS");
                param.Parameter.Add("Status6", "HPX");
                param.WSDL = "ESBDBDelimiter";
                data = EAI.RetrieveESBData(param);
            }

            if (data != null && data.Result.Count != 0)
            {
                foreach (StringDictionary eachRecord in data.Result)
                {
                    creditcardapplication.Add(new CreditCardApplicationInfo()
                    {
                        RegisteredKeyId = eachRecord[ESBKeyValueName.CCAppl_RegisteredKeyId],
                        CustomerNo = eachRecord[ESBKeyValueName.CCAppl_CustomerNo],
                        CreditCardNo = eachRecord[ESBKeyValueName.CCAppl_CardNo],
                        CustomerName = eachRecord[ESBKeyValueName.CCAppl_CustomerName],
                        //ConnectionType = Utility.GetStringMap(8, 22, eachRecord[ESBKeyValueName.CCAppl_ConnectionType]),
                        ConnectionType = Utility.GetStringMap(8, 14, eachRecord[ESBKeyValueName.CCAppl_ConnectionType]),
                        //ApplicationName = Utility.GetStringMap(8, 15, eachRecord[ESBKeyValueName.CCAppl_ApplicationName]),
                        ApplicationName = Utility.GetStringMap(8, 13, eachRecord[ESBKeyValueName.CCAppl_ApplicationName]),
                        RequestDate = Utility.ParseExact(eachRecord[ESBKeyValueName.CCAppl_CreatedDate], "MM/dd/yyyy hh:mm:ss tt"),
                        Reason = eachRecord[ESBKeyValueName.CCAppl_Reason],
                        ConnectionDate = Utility.ParseExact(eachRecord[ESBKeyValueName.CCAppl_ConnectionDate], "MM/dd/yyyy hh:mm:ss tt"),
                        Id1 = eachRecord[ESBKeyValueName.CCAppl_Id1],
                        Id2 = eachRecord[ESBKeyValueName.CCAppl_Id2],
                        Address1 = eachRecord[ESBKeyValueName.CCAppl_Address1],
                        Address2 = eachRecord[ESBKeyValueName.CCAppl_Address2],
                        Address3 = eachRecord[ESBKeyValueName.CCAppl_Address3],
                        Address4 = eachRecord[ESBKeyValueName.CCAppl_Address4],
                        Address5 = eachRecord[ESBKeyValueName.CCAppl_Address5],
                        Address6 = eachRecord[ESBKeyValueName.CCAppl_Address6],
                        Address7 = eachRecord[ESBKeyValueName.CCAppl_Address7],
                        Address8 = eachRecord[ESBKeyValueName.CCAppl_Address8],
                        //Action = Utility.GetStringMap(8, 17, eachRecord[ESBKeyValueName.CCAppl_Status]),
                        Action = Utility.GetStringMap(8, 5, eachRecord[ESBKeyValueName.CCAppl_Status]),
                        //Status = Utility.GetStringMap(8, 4, eachRecord[ESBKeyValueName.CCAppl_Status])
                        Status = Utility.GetStringMap(8, 1, eachRecord[ESBKeyValueName.CCAppl_Status])
                    });
                }
            }

            return creditcardapplication;
        }
    }
}