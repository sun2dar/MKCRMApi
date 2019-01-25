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
        public static List<CreditCardConnectionInfo> KBICreditCardConnection(Channel channel) {
            List<CreditCardConnectionInfo> creditcardconnection = new List<CreditCardConnectionInfo>();

            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            if (!string.IsNullOrEmpty(channel.UserId))
            {
                param.RequestTransType = "GetCreditCardConnectionOnIBankByUserId";
                param.Parameter.Add("userId", channel.UserId);
                param.Parameter.Add("No_Kartu", "");
                param.Parameter.Add("No_Rekening", "");
                param.Parameter.Add("NewStatus", "AKT");
                param.WSDL = "ESBDBDelimiter";
                data = EAI.RetrieveESBData(param);
            }
            else if (!string.IsNullOrEmpty(channel.CustomerNo))
            {
                param.RequestTransType = "GetCreditCardConnectionOnIBankByCustCC";
                param.Parameter.Add("KeyId", "");
                param.Parameter.Add("No_Kartu", "");
                param.Parameter.Add("custNo", channel.CustomerNo);
                param.Parameter.Add("NewStatus", "AKT");
                param.WSDL = "ESBDBDelimiter";
                data = EAI.RetrieveESBData(param);
            }

            if (data != null && data.Result.Count != 0)
            {
                foreach (StringDictionary eachRecord in data.Result)
                {
                    creditcardconnection.Add(new CreditCardConnectionInfo()
                    {
                        KeyId = eachRecord[ESBKeyValueName.CCConn_KeyId],
                        CustomerNoCreditCard = eachRecord[ESBKeyValueName.CCConn_CustomerNo],
                        AtmCardNo = eachRecord[ESBKeyValueName.CCConn_CardNo],
                        CustomerName = eachRecord[ESBKeyValueName.CCConn_CustomerName],
                        ConnectionType = eachRecord[ESBKeyValueName.CCConn_ConnectionType],
                        //ApplicationName = Utility.GetStringMap(7, 15, eachRecord[ESBKeyValueName.CCConn_ApplicationName]),
                        ApplicationName = Utility.GetStringMap(7, 12, eachRecord[ESBKeyValueName.CCConn_ApplicationName]),
                        ConnectionDate = Utility.ParseExact(eachRecord[ESBKeyValueName.CCConn_ConnectionDate], "MM/dd/yyyy hh:mm:ss tt"),
                        Id1 = eachRecord[ESBKeyValueName.CCConn_Id1],
                        Id2 = eachRecord[ESBKeyValueName.CCConn_Id2]
                    });
                }
            }
            return creditcardconnection;
        }
    }
}