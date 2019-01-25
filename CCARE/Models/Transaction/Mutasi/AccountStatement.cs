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
using CCARE.Models;
using BCA.CRM.OSB.Model;

namespace CCARE.Models
{
    public class AccountStatement
    {

        private static bool CheckRestrictedAccount(string AccountNo) {
            bool restricted = false;
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetAccountDetail";
            param.Parameter.Add("acctNo", AccountNo);
            param.WSDL = "AccountDetail";
            ESBData data = EAI.RetrieveESBData(param);
            if (data.Result != null && data.Result.Count > 0)
            {
                restricted = string.IsNullOrEmpty(data.Result[0]["miscStatus"]) ? false : data.Result[0]["miscStatus"] == "1" ? true : false;
            }
            return restricted;
        }

        public static AllChannelTransaction Mutasi(Params request, string isSuperAdmin = "N")
        {
            AllChannelTransaction transaction = new AllChannelTransaction();
            try
            {
                transaction.RestrictedAccount = CheckRestrictedAccount(request.Parameter["acctNo"]);

                if ((transaction.RestrictedAccount && "Y".Equals(isSuperAdmin)) || transaction.RestrictedAccount == false)
                {
                    request.WSDL = "AccountStatementByDate";
                    ESBData data = EAI.RetrieveESBData(request);
                    if (data.Result != null && data.Result.Count > 0)
                    {
                        transaction.Name = data.Result[0][AllChannelTransactionResultKeyName.Name];
                        transaction.Currency = Utility.GetCurrency(data.Result[0][AllChannelTransactionResultKeyName.Currency]);

                        for (int ctr = 1; ctr < data.Result.Count; ctr++)
                        {
                            double amountValue;
                            double.TryParse(data.Result[ctr][AllChannelTransactionResultKeyName.Amount], out amountValue);

                            string transDate = data.Result[ctr][AllChannelTransactionResultKeyName.TransactionDate];
                            if (!string.IsNullOrEmpty(transDate))
                            {
                                string[] datePart = transDate.Split('/');
                                if (datePart.Length == 2)
                                {
                                    string day = datePart[0].PadLeft(2, '0');
                                    string month = datePart[1].PadLeft(2, '0');

                                    transDate = day + "/" + month;
                                }
                            }

                            DateTime? transactionDate = Formatter.ParseExact(transDate, "dd/MM");
                            transaction.Transactions.Add(new ACTRX()
                            {
                                Number = ctr.ToString(CultureInfo.CurrentUICulture),
                                displayTransactionDate = transDate,
                                TransactionDate = transactionDate,
                                TransactionType = data.Result[ctr][AllChannelTransactionResultKeyName.TransactionType],
                                TransactionDescription = data.Result[ctr][AllChannelTransactionResultKeyName.Name] + " " + data.Result[ctr][AllChannelTransactionResultKeyName.Trailer],
                                Branch = data.Result[ctr][AllChannelTransactionResultKeyName.Branch],
                                Amount = Formatter.GetParsedDouble(data.Result[ctr][AllChannelTransactionResultKeyName.Amount], false)
                            });
                        }
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