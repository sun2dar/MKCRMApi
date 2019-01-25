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

namespace CCARE.Models.Transaction
{
    public partial class CurrentTransaction
    {
        public static List<ATMTransaction> ATM(Params request)
        {
            List<ATMTransaction> ResultList = new List<ATMTransaction>();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            param.Parameter.Add("NUM", request.Parameter.ContainsKey("atmNo") ? request.Parameter["atmNo"].Trim() : string.Empty);
            param.Parameter.Add("StartDate", request.Parameter.ContainsKey("startDate") ? request.Parameter["startDate"].Trim() : string.Empty);
            param.Parameter.Add("EndDate", request.Parameter.ContainsKey("endDate") ? request.Parameter["endDate"].Trim() : string.Empty);
            param.Parameter.Add("CRD_LN", request.Parameter.ContainsKey("lnet") ? request.Parameter["lnet"].Trim() : string.Empty);
            param.Parameter.Add("CRD_FIID", request.Parameter.ContainsKey("fiid") ? request.Parameter["fiid"].Trim() : string.Empty);
            if (request.Parameter.ContainsKey("terminalId"))
            {
                param.RequestTransType = "GetATMTransactionByATMNoWithTerminalID";
                param.Parameter.Add("TERM_ID", request.Parameter["terminalId"].Trim().ToUpper());
            }
            else
            {
                param.RequestTransType = "GetATMTransactionByATMNo";
            }
            param.WSDL = "ESBDBDelimiter";

            try
            {
                ESBData data = EAI.RetrieveESBData(param);
                if (data.Result != null && data.Result.Count > 0)
                {
                    int counter = 0;
                    ATMTransaction record = null;
                    foreach (var entry in data.Result)
                    {
                        record = new ATMTransaction();

                        //DateTime? transactionDate = Formatter.ParseExact(entry["tran_dat"], "yyyy-MM-dd HH:mm:ss");
                        //string a = Formatter.ToStringExact(DateTime.ParseExact(entry["tran_dat"], "dd/MM/yyyy hh:mm:ss", null), "dd/MM hh:mm");

                        //DateTime dt = DateTime.ParseExact(entry["tran_dat"], "dd MM yyyy hh:mm:ss", null);
                        //string s = dt.ToString("dd-MM hh:mm");

                        double amountValue = 0;
                        double.TryParse(entry["amt_1"], out amountValue);
                        amountValue = amountValue / 100;
                        
                        string rateEsb = entry["crncy_conv_rate"];
                        double rateValue = 0;
                        if (!string.IsNullOrEmpty(rateEsb))
                        {
                            double nValue = 0;
                            double.TryParse(rateEsb.Substring(0, 1), out nValue);
                            double.TryParse(rateEsb.Remove(0, 1), out rateValue);
                            rateValue = rateValue / Math.Pow(10, nValue);
                        }

                        double conversionAmount = 0;
                        double.TryParse(entry["crncy_conv_amt"], out conversionAmount);
                        conversionAmount = conversionAmount / 100;
                        double transactionAmount = 0;
                        double.TryParse(entry["amt_2"], out transactionAmount);
                        transactionAmount = transactionAmount / 100;

                        string currency = entry["from_crncy_cde"];

                        counter++;
                        record.Number = counter.ToString();
                        record.TransactionCode = entry["tran_cde"];
                        //record.TransactionDate = transactionDate.ToString();
                        record.TransactionDate = entry["tran_dat"];
                        record.Amount1 = amountValue.ToString();
                        record.ResponseCode = entry["respcode"];
                        record.ResponseDescription = entry["respdesc"];
                        record.TransactionDescription = entry["trandesc"];
                        //record.Currency = entry["from_crncy_cde"];
                        record.Currency = entry["orig_crncy_cde"];
                        record.Terminal = entry["term_id"];
                        record.Rate = rateValue.ToString();
                        record.ConversionAmount = GetConversionAmount(entry,conversionAmount);
                        record.Amount2 = GetConversionAmount(entry, transactionAmount);
                        record.FromAccount = entry["from_acct_acct_num"];
                        record.ToAccount = entry["to_acct_acct_num"];
                        record.PayeeCode = entry["payee_code"];
                        record.PayeeNumber = entry["bill_acct_num"];
                        record.SequenceNumber = entry["seq_num"];
                        
                        ResultList.Add(record);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return ResultList;
        }

        private static string GetConversionAmount(StringDictionary eachTransaction, double conversionAmount)
        {
            string originalCurrency = "IDR";
            string conversionValue = string.Format(CultureInfo.CurrentCulture, "{0} {1}", originalCurrency, Formatter.FormatNumeric(conversionAmount));
            return conversionValue;
        }
    }
}