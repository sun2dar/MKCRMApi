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

namespace CCARE.Models.Transaction
{
    public partial class ChannelTransaction
    {
        public static SMSTopUpTransaction SMSTopUp(Params request)
        {
            SMSTopUpTransaction transaction = new SMSTopUpTransaction();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            try
            {
                param.RequestTransType = "GetTopUpTransactionByMobileNo";
                param.Parameter.Add("FK_CustID", request.Parameter.ContainsKey("mobileNo") ? request.Parameter["mobileNo"].Trim() : string.Empty);
                param.Parameter.Add("StartDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["startDate"]), "MM/dd/yyyy"));
                param.Parameter.Add("EndDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["endDate"]).AddDays(1), "MM/dd/yyyy"));
                param.WSDL = "ESBDBDelimiter";
                ESBData data = EAI.RetrieveESBData(param);

                if (data.Result != null && data.Result.Count > 0)
                {
                    foreach (StringDictionary entry in data.Result)
                    {
                        SMSTopUpTRX trx = new SMSTopUpTRX();
                        DateTime? TransactionDate = Formatter.ParseExact(entry["createddate"], "MM/dd/yyyy hh:mm:ss tt");
                        trx.TransactionDate = TransactionDate;
                        trx.RequestId = entry["billerid"];
                        trx.AtmCardNumber = entry["cardnumber"];
                        trx.AccountNumber = entry["acctidfrom"];
                        trx.ValueOfTransactions = Formatter.GetParsedNumeric(entry["amt"], false);
                        trx.ResponseCode = Utility.GetDisplayText("TransactionAttributeMapping", "SMS Top Up", "Response Code", entry["srvrstatuscode"]);
                        transaction.Transactions.Add(trx);
                    }

                    param.Parameter = new Dictionary<string, string>();
                    param.RequestTransType = "GetTopUpInfoByMobileNo";
                    param.Parameter.Add("CustId", request.Parameter.ContainsKey("mobileNo") ? request.Parameter["mobileNo"].Trim() : string.Empty);
                    param.WSDL = "ESBDBDelimiter";
                    ESBData information = EAI.RetrieveESBData(param);
                    if (information.Result != null && information.Result.Count > 0) 
                    {
                        transaction.ATMCardNumber = information.Result[0]["cardnumber"];
                        transaction.ATMCardHolderName = information.Result[0]["custname"];
                        transaction.Status = Utility.GetStatusInfo(information.Result[0]["status"]);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return transaction;
        }
    }
}
