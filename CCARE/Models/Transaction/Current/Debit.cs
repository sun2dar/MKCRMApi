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
using CCARE.Models.Transaction;

namespace CCARE.Models.Transaction
{
    public partial class CurrentTransaction
    {
        public static List<DebitTransaction> Debit(Params request)
        {
            List<DebitTransaction> ResultList = new List<DebitTransaction>();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            param.Parameter.Add("NUM", request.Parameter.ContainsKey("atmNo") ? request.Parameter["atmNo"].Trim() : string.Empty);
            param.Parameter.Add("StartDate", request.Parameter.ContainsKey("startDate") ? request.Parameter["startDate"].Trim() : string.Empty);
            param.Parameter.Add("EndDate", request.Parameter.ContainsKey("endDate") ? request.Parameter["endDate"].Trim() : string.Empty);
            param.Parameter.Add("CRD_LN", request.Parameter.ContainsKey("lnet") ? request.Parameter["lnet"].Trim() : string.Empty);
            param.Parameter.Add("CRD_FIID", request.Parameter.ContainsKey("fiid") ? request.Parameter["fiid"].Trim() : string.Empty);
            if (request.Parameter.ContainsKey("terminalId"))
            {
                param.RequestTransType = "GetDebitTransactionByATMNoWithTerminalID";
                param.Parameter.Add("TERM_ID", request.Parameter["terminalId"].Trim().ToUpper());
            }
            else
            {
                param.RequestTransType = "GetDebitTransactionByATMNo";
            }
            param.WSDL = "ESBDBDelimiter";

            try
            {
                ESBData data = EAI.RetrieveESBData(param);
                if (data.Result != null && data.Result.Count > 0)
                {
                    int counter = 0;
                    DebitTransaction record = null;
                    foreach (var entry in data.Result)
                    {
                        record = new DebitTransaction();
                        DateTime? transactDate = Formatter.ParseExact(entry["tran_dat"], "yyyy-MM-dd HH:mm:ss");
                        DateTime? transactTime = Formatter.ParseExact(entry["tran_dat"], "yyyy-MM-dd HH:mm:ss");
                        double amountJumlah = 0;
                        double.TryParse(entry["amt_1"], out amountJumlah);
                        amountJumlah = amountJumlah / 100;
                        double cashJumlahTunai = 0;
                        double.TryParse(entry["amt_2"], out cashJumlahTunai);
                        cashJumlahTunai = cashJumlahTunai / 100;

                        counter++;
                        record.Number = counter.ToString();
                        record.TransactionCode = entry["transcode"];
                        //record.TransactionDate = transactDate.ToString();
                        //record.TransactionTime = transactTime.ToString();
                        record.TransactionDate = entry["tran_dat"];
                        record.TransactionTime = entry["tran_dat"].Substring(11, 8);
                        record.Amount = amountJumlah.ToString();
                        record.ResponseCode = entry["resp_cde"];
                        record.ApprovalCode = entry["apprv_cde"];
                        record.AtmCardNo = request.Parameter["atmNo"].Trim();
                        record.TerminalId = entry["term_term_id"];
                        record.CashAmount = cashJumlahTunai.ToString();
                        record.AccountNumber = entry["acct_num"];
                        record.Retailer = entry["retailer_id"];
                        record.TraceNo = entry["invoice_num"];
                        record.Batch = entry["batch_num"];
                        record.SequenceNo = entry["seq_num"];
                        record.CardType = entry["crd_typ"];

                        ResultList.Add(record);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return ResultList;
        }
    }
}