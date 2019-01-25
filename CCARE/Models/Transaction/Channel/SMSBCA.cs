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
        public static SMSBCATransaction SMSBCA(Params request)
        {
            SMSBCATransaction transaction = new SMSBCATransaction();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            try
            {
                param.RequestTransType = request.RequestTransType;
                param.Parameter.Add("CustId", request.Parameter.ContainsKey("mobileNo") ? request.Parameter["mobileNo"].Trim() : string.Empty);
                param.Parameter.Add("StartDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["startDate"]), "MM/dd/yyyy"));
                param.Parameter.Add("EndDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["endDate"]).AddDays(1), "MM/dd/yyyy"));
                param.Parameter.Add("Proses", "GetSMSTransactionInfoSaldo".Equals(param.RequestTransType) ? "SMSBAccGetBalanceRq" :
                                        "GetSMSTransactionInfoMutasi".Equals(param.RequestTransType) ? "SMSBAcctStmtRq" :
                                        "GetSMSTransactionInfoCoupon".Equals(param.RequestTransType) ? "SMSBInqCouponRq" :
                                        "GetSMSTransactionInfoPayment".Equals(param.RequestTransType) ? "SMSBLPGRQ" : 
                                        string.Empty);
                param.WSDL = "ESBDBDelimiter";
                ESBData data = EAI.RetrieveESBData(param);
                if (data.Result != null && data.Result.Count > 0)
                {
                    foreach (StringDictionary entry in data.Result)
                    {
                        DateTime? transactionDate = Convert.ToDateTime(entry["createddate"]);
                        string status = entry["srvrstatuscode"];
                        SMSBCATRX trx = new SMSBCATRX();

                        trx.TransactionDate = transactionDate;
                        trx.AccountNumber = entry["acctidfrom"];
                        trx.TransactionType = Utility.GetDisplayText("TransactionAttributeMapping", "SMS BCA", "Transaction Type", entry["process"]);
                        trx.ResponseCode = Utility.GetDisplayText("TransactionAttributeMapping", "SMS BCA", "Response Code", status);
                        trx.Amount = Formatter.GetParsedDouble(entry["amt"], false);

                        /* Payment Menu 
                        * 07 12 2016 - LPG
                        * Tanggal Transaksi       createddate                 TransactionDate
                        * No Rekening             acctidfrom                  AccountNumber
                        * No Referensi            refcode                     ReferenceNumber
                        * Jenis Transaksi         process -company_name       TransactionType + string
                        * Nominal (IDR)           amt                         Amount
                        * Status                  srvrstatuscode - rquid      ResponseCode + string
                        * Pembayar                billerid                    Biller
                        * Penerima                authidresponse [0]          Receiver
                        * Jumlah                  authidresponse [1]          Total
                        * Informasi Lain          authidresponse [2]          Other
                        */

                        if("GetSMSTransactionInfoPayment".Equals(param.RequestTransType)){
                            trx.ReferenceNumber = entry["refcode"];
                            trx.TransactionType += " - " + entry["company_name"];
                            trx.ResponseCode += " - " + entry["rquid"];
                            trx.Biller = entry["billerid"];
                            string[] authidresp = entry["authidresponse"].Split('|');
                            if (authidresp.Length == 3) {
                                trx.Receiver = authidresp[0];
                                trx.Total = authidresp[1];
                                trx.Other = authidresp[2].Length == 8 ? Utility.FormatDateAuth(authidresp[2]) : authidresp[2];
                            }
                        }
                        
                        transaction.Transactions.Add(trx);
                    }

                    param.Parameter = new Dictionary<string, string>();
                    param.RequestTransType = "GetSMSBankingInfoByMobileNo";
                    param.Parameter.Add("CustId", request.Parameter.ContainsKey("mobileNo") ? request.Parameter["mobileNo"].Trim() : string.Empty);
                    param.WSDL = "ESBDBDelimiter";
                    ESBData information = EAI.RetrieveESBData(param);
                    if (information.Result != null && information.Result.Count > 0)
                    {
                        transaction.ATMCardHolderName = information.Result[0]["custname"];
                        transaction.ATMCardNumber =  information.Result[0]["cardnumber"];
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