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

namespace CCARE.Models.Transaction
{
    public partial class ChannelTransaction
    {
        public static KBITransaction KBI(Params request)
        {
            KBITransaction transaction = new KBITransaction();
            ESBData data = null;
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            
            try
            {
                ESBData information = null;
                if (!(string.IsNullOrEmpty(request.RequestTransType)) && !("GetCreditCardLogIBankByUserId".Equals(request.RequestTransType)))
                {
                    if (request.Parameter.ContainsKey("userId"))
                    {
                        param.RequestTransType = "GetIBankInfoByUserId";
                        param.Parameter.Add("CustId", request.Parameter["userId"]);
                        param.WSDL = "ESBDBDelimiter";
                        information = EAI.RetrieveESBData(param);
                    }
                    if (request.Parameter.ContainsKey("atmNo"))
                    {
                        param.RequestTransType = "GetIBankInfoByATMNo";
                        param.Parameter.Add("CardNumber", request.Parameter["atmNo"]);
                        param.WSDL = "ESBDBDelimiter";
                        information = EAI.RetrieveESBData(param);
                    }

                    if (information.Result.Count > 0 && !string.IsNullOrEmpty(information.Result[0]["custid"]))
                    {
                        transaction.UserId = information.Result[0]["custid"];
                        transaction.CardNumber = information.Result[0]["cardnumber"];
                        transaction.CustomerName = information.Result[0]["firstname"];
                    }
                }
                else if ("GetCreditCardLogIBankByUserId".Equals(request.RequestTransType))
                {
                    if (request.Parameter.ContainsKey("userId"))
                    {
                        param.RequestTransType = "GetCardNoByUserIdIBank";
                        param.Parameter.Add("KeyId", request.Parameter["userId"]);
                        param.Parameter.Add("No_Kartu", string.Empty);
                        param.Parameter.Add("No_Rekening", string.Empty);
                        param.Parameter.Add("NewStatus", "AKT");
                        param.WSDL = "ESBDBDelimiter";
                        information = EAI.RetrieveESBData(param);
                        transaction.UserId = request.Parameter["userId"];
                        transaction.CardNumber = information.Result[0]["no_kartu"];
                        transaction.CustomerName = information.Result[0]["firstname"];
                    }
                    else if (request.Parameter.ContainsKey("atmNo"))
                    {
                        param.RequestTransType = "GetUserIdIBankByCardNo";
                        param.Parameter.Add("KeyId", string.Empty);
                        param.Parameter.Add("No_Kartu", request.Parameter["atmNo"]);
                        param.Parameter.Add("No_Rekening", string.Empty);
                        param.Parameter.Add("NewStatus", "AKT");
                        param.WSDL = "ESBDBDelimiter";
                        information = EAI.RetrieveESBData(param);
                        transaction.UserId = information.Result[0]["keyid"];
                        transaction.CardNumber = request.Parameter["atmNo"];
                        transaction.CustomerName = information.Result[0]["firstname"];
                    }
                }

                param.Parameter = new Dictionary<string, string>();
                param.RequestTransType = request.RequestTransType;

                param.Parameter.Add("FK_CustID", transaction.UserId);

                switch (param.RequestTransType)
                {
                    // ORAIBSELECTLOGTXNPAYMENTPURCHASEBYCUSTID
                    case "GetIBTransactionByUserIdPayment":
                        param.Parameter.Add("TxType", "PmtAddRq");
                        break;
                    case "GetIBTransactionByUserIdPaymentECommerce":
                        param.Parameter.Add("TxType", "EComAddRq");
                        break;
                    case "GetIBTransactionByUserIdVirtualAcct":
                        param.Parameter.Add("TxType", "XFerAddRq4");
                        break;
                    case "GetIBTransactionByUserIdPurchase":
                        param.Parameter.Add("TxType", "PchAddRq");
                        break;
                    case "GetIBTransactionByUserIdTopUpWallet":
                        param.Parameter.Add("TxType", "XFerAddRq5");
                        break;
                    // ORAIBSELECTTXNHISTORYBYCUSTID
                    case "GetIBTransactionByUserIdAcctInfo":
                        param.Parameter.Add("Process1", "BalInqRq");
                        param.Parameter.Add("Process2", "DeptAcctStmtInqRq");
                        param.Parameter.Add("Process3", "InvInqRq");
                        param.Parameter.Add("Process4", "InvAcctStmtInqRq");
                        param.Parameter.Add("Process5", "InvAcctStmtInqRq2");
                        break;
                    case "GetIBTransactionByUserIdConsumerCreditInfo":
                        param.Parameter.Add("Process1", "LOANINQRQ");
                        param.Parameter.Add("Process2", "LOANHISTINQRQ");
                        param.Parameter.Add("Process3", "");
                        param.Parameter.Add("Process4", "");
                        param.Parameter.Add("Process5", "");
                        break;
                    case "GetIBTransactionByUserIdInvesProductInfo":
                        param.Parameter.Add("Process1", "REKSAINQRQ");
                        param.Parameter.Add("Process2", "REKSAINQRQ");
                        param.Parameter.Add("Process3", "");
                        param.Parameter.Add("Process4", "");
                        param.Parameter.Add("Process5", "");
                        break;
                    case "GetCreditCardLogIBankByUserId":
                        param.Parameter.Add("Process1", "REGBCACC");
                        param.Parameter.Add("Process2", "DELBCACC");
                        param.Parameter.Add("Process3", "CCSTMTINQRQ");
                        param.Parameter.Add("Process4", "");
                        param.Parameter.Add("Process5", "");
                        break;
                    // ORAIBSELECTLOGTXNTRANSFERBYCUSTID
                    case "GetIBTransactionByUserIdTransferBCA":
                        param.Parameter.Add("TxType1", "XFerAddRq2");
                        param.Parameter.Add("TxType2", "XFerAddRq3");
                        param.Parameter.Add("EffDt1", "0");
                        param.Parameter.Add("EffDt2", "1");
                        param.Parameter.Add("EffDt3", "");
                        break;
                    case "GetIBTransactionByUserIdTransferDomestic":
                        param.Parameter.Add("TxType1", "XFerAddRq3");
                        param.Parameter.Add("TxType2", "");
                        param.Parameter.Add("EffDt1", "1");
                        param.Parameter.Add("EffDt2", "");
                        param.Parameter.Add("EffDt3", "");
                        break;
                    case "GetIBTransactionByUserIdAKSesFundWithdrawal":
                        param.Parameter.Add("TxType1", "FundWdrwRq");
                        param.Parameter.Add("TxType2", "");
                        param.Parameter.Add("EffDt1", "0");
                        param.Parameter.Add("EffDt2", "");
                        param.Parameter.Add("EffDt3", "");
                        break;
                    // ORAIBSELECTTRANSFERTUNDABYCUSTIDANDTXTDATE
                    case "GetIBTransactionByUserIdTransferBCAInputStatus":
                        param.Parameter.Add("TxType", "XFerAddRq3");
                        param.Parameter.Add("Is_Domestic", "0");
                        break;
                    case "GetIBTransactionByUserIdTransferDomesticInputStatus":
                        param.Parameter.Add("TxType", "XFerAddRq3");
                        param.Parameter.Add("Is_Domestic", "1");
                        break;
                    // ORAIBSELECTTRANSFERTUNDABYCUSTIDANDPROCESSDATE
                    case "GetIBTransactionByUserIdTransferBCATxnStatus":
                        param.Parameter.Add("TxType", "XFerAddRq3");
                        param.Parameter.Add("Is_Domestic", "0");
                        break;
                    case "GetIBTransactionByUserIdTransferDomesticTxnStatus":
                        param.Parameter.Add("TxType", "XFerAddRq3");
                        param.Parameter.Add("Is_Domestic", "1");
                        break;
                    // ORAIBSELECTTRANSFERTOLAKBYCUSTIDANDTXNDATE
                    case "GetIBTransactionByUserIdRejected":
                        break;
                }

                param.Parameter.Add("StartDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["startDate"]), "MM/dd/yyyy"));
                param.Parameter.Add("EndDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["endDate"]).AddDays(1), "MM/dd/yyyy"));

                // ORAIBSELECTUSERSESSIONLOGBYCUSTID
                if ("GetIBTransactionByUserIdUserSession".Equals(param.RequestTransType))
                {
                    param.Parameter.Add("ChgFlag", "");
                }

                param.WSDL = "ESBDBDelimiter";

                data = EAI.RetrieveESBData(param);

                if (data.Result != null && data.Result.Count > 0)
                {
                    KBITRX trx = null;
                    foreach (StringDictionary entry in data.Result)
                    {
                        trx = new KBITRX();
                        string tandemStatusCode = string.Empty;
                        string mdlwrStatusCode = string.Empty;
                        switch (param.RequestTransType)
                        {
                            // ORAIBSELECTLOGTXNPAYMENTPURCHASEBYCUSTID
                           case "GetIBTransactionByUserIdPayment":
                                tandemStatusCode = entry["srvrstatuscode"];
                                DateTime? tandemDate = (tandemStatusCode == "0" || tandemStatusCode == "00" || tandemStatusCode == "97") ? Formatter.ParseExact(entry["tandemtxdate"], "ddMMyyHHmm") : Formatter.ParseExact(entry["tandemtxdate"], "MMddyyHHmm");

                                trx.MiddlewareDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TandemDate = tandemDate;
                                trx.PaymentAccountNumber = entry["acctidfrom"];
                                trx.CustomerNumber = entry["billrefinfo"];
                                trx.Nominal = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                trx.TokenStatus = entry["flagtoken"];
                                trx.ReferenceNumber = entry["rqUID"];
                                trx.PaymentFor = entry["desceng"];
                                break;
                            case "GetIBTransactionByUserIdPaymentECommerce":
                                trx.MiddlewareDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TandemDate = Formatter.ParseExact(entry["tandemtxdate"], "ddMMyyHHmm");
                                trx.PaymentAccountNumber = entry["acctidfrom"];
                                trx.CustomerNumber = entry["billrefinfo"];
                                trx.Amount = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                trx.TokenStatus = entry["flagtoken"];
                                trx.ReferenceNumber = entry["rquid"];
                                trx.PaymentFor = entry["desceng"];
                                break;
                            case "GetIBTransactionByUserIdVirtualAcct":
                                double? forex = Formatter.GetParsedDouble(entry["currforex"], false);
                                double forexValue = 0;
                                if (forex.HasValue)
                                {
                                    forexValue = forex.Value / 100;
                                }
                                trx.TransferDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.InputDate = Formatter.ParseExact(entry["lateamt"], "yyyy-MM-dd H:mm:ss");
                                trx.TransactionType = entry["authidresponse"];
                                trx.SenderAccountNo = entry["acctidfrom"];
                                trx.TransferToAccount = entry["acctidto"];
                                trx.TransferToAccountName = entry["acctidtoname"];
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.LateChargeAmount = entry["latechargeamt"];
                                trx.TransferAmount = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.Forex = forexValue;
                                trx.AmountIDR = Formatter.GetParsedDouble(entry["amtidr"], false);
                                trx.ToAccountType = Utility.GetCurrency(entry["accttypeto"]);
                                trx.SendToSubject = entry["sendtosubject"];
                                trx.Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.Reason = entry["errormessage"];
                                trx.Reference = entry["rquid"];
                                break;
                            case "GetIBTransactionByUserIdPurchase":
                                tandemStatusCode = entry["srvrStatusCode"];
                                trx.MiddlewareDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TandemDate = (tandemStatusCode == "0" || tandemStatusCode == "00" || tandemStatusCode == "97") ? Formatter.ParseExact(entry["tandemtxdate"], "ddMMyyHHmm") : Formatter.ParseExact(entry["tandemtxdate"], "MMddyyHHmm");
                                trx.AccountPaymentNumber = entry["acctidfrom"];
                                trx.PaymentFor = entry["desceng"];
                                trx.Nominal = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                trx.Token = entry["flagtoken"];
                                trx.ReferenceNumber = entry["rquid"];
                                break;
                            case "GetIBTransactionByUserIdTopUpWallet":
                                trx.TransactionDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.FromAccountNumber = entry["acctidfrom"];
                                trx.ToAccountNoHp = entry["acctidto"];
                                trx.TransactionType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Information Type", entry["txtype"]);
                                trx.Amount = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                trx.FlagToken = entry["flagtoken"];
                                trx.ReferenceNumber = entry["rquid"];
                                trx.ToAccountName = entry["acctidtoname"];
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.Description = entry["errormessage"];
                                break;
                            // ORAIBSELECTTXNHISTORYBYCUSTID
                            case "GetIBTransactionByUserIdAcctInfo":
                                trx.MiddlewareDate = Formatter.ParseExact(entry["createddate"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.InformationType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Information Type", entry["process"]);
                                trx.AccountNumber = entry["acctidfrom"];
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                trx.ReferenceNumber = entry["rquid"];
                                break;
                            case "GetIBTransactionByUserIdConsumerCreditInfo":
                                trx.MiddlewareDate = Formatter.ParseExact(entry["createddate"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.InformationType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Information Type", entry["process"]);
                                trx.AccountNumber = entry["acctidfrom"];
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                trx.ReferenceNumber = entry["rquid"];
                                break;
                            case "GetIBTransactionByUserIdInvesProductInfo":
                                trx.MiddlewareDate = Formatter.ParseExact(entry["createddate"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.InformationType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Information Type", entry["process"]);
                                trx.AccountNumber = entry["acctidfrom"];
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                trx.ReferenceNumber = entry["rquid"];
                                break;
                            case "GetCreditCardLogIBankByUserId":
                                DateTime? middlewareDate = Formatter.ParseExact(entry["createddate"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.MiddlewareDate = middlewareDate;
                                trx.Process = Utility.GetDisplayText("TransactionAttributeMapping", "Kartu Kredit", "Transaction Type", entry["process"]);
                                trx.FromAccountId = Formatter.GetParsedNumericLong(entry["acctidfrom"], false);
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.ReferenceNo = entry["rquid"];
                                break;
                            // ORAIBSELECTLOGTXNTRANSFERBYCUSTID
                            case "GetIBTransactionByUserIdTransferBCA":
                                double? exchangeRate = Formatter.GetParsedDouble(entry["currforex"], false);
                                if (exchangeRate.HasValue)
                                {
                                    exchangeRate = exchangeRate.Value / 100;
                                }
                                trx.TransferDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.InputDate = Formatter.ParseExact(entry["lateamt"], "yyyy-MM-dd HH:mm:ss");
                                trx.TransferType = entry["authidresponse"];
                                trx.FromAccountNumber = entry["acctidfrom"];
                                trx.ToAccountNumber = entry["acctidto"];
                                trx.ToAccountName = entry["acctidtoname"];
                                trx.MUTransaction = Utility.GetCurrency(entry["curr"]);
                                trx.LateChargeAmount = entry["latechargeamt"];
                                trx.TransferNominal = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.ExchangeRate = exchangeRate;
                                trx.ConversiNominal = Formatter.GetParsedDouble(entry["amtidr"], false);
                                trx.MUFromAccountNumber = Utility.GetCurrency(entry["accttypeto"]);
                                trx.News = entry["sendtosubject"];
                                trx.Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.Cause = entry["errdesc"];
                                trx.ReferenceNumber = entry["rquid"];
                                break;
                            case "GetIBTransactionByUserIdTransferDomestic":
                                trx.InputDate = Formatter.ParseExact(entry["lateamt"], "yyyy-MM-dd HH:mm:ss");
                                trx.TransferDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferType = entry["authidresponse"];
                                trx.FromAccountNumber = entry["acctidfrom"];
                                trx.ToAccountNumber = entry["acctidto"];
                                trx.ToAccountName = entry["acctidtoname"];
                                trx.City = entry["dccustaddress"];
                                trx.Bank = entry["dcmerchantname"];
                                trx.Branch = entry["custname"];
                                trx.Citizen = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Citizen", entry["custstatus"]);
                                trx.WNI = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "WNI", entry["custbranch"]);
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.TransferNominal = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.Cost = Formatter.GetParsedDouble(entry["feeamt"], false);
                                trx.News = entry["sendtosubject"];
                                trx.TransferService = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Transfer Service", entry["feecur"]);
                                trx.Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.Reason = entry["errdesc"];
                                trx.ReferenceNumber = entry["rquid"];
                                break;
                            case "GetIBTransactionByUserIdAKSesFundWithdrawal":
                                trx.TransactionDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.FromAccountNumber = entry["acctidfrom"];
                                trx.UserId = transaction.UserId;
                                trx.CustomerName = entry["custname"];
                                trx.Email = entry["sendtoaddr"];
                                trx.TransactionType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Information Type", entry["txtype"]);
                                trx.BillerId = entry["billerid"];
                                trx.BillerRefInfo = entry["billrefinfo"];
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.Amount = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.Reason = entry["errdesc"];
                                trx.ReferenceNumber = entry["rquid"];
                                trx.MiddlewareStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Middleware Status", entry["statuscode"]);
                                trx.TandemStatus = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "KlikBCA Individual", "Tandem Status", entry["srvrstatuscode"]);
                                break;
                            // ORAIBSELECTTRANSFERTUNDABYCUSTIDANDTXTDATE
                            case "GetIBTransactionByUserIdTransferBCAInputStatus":
                                trx.InputDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferDate = Formatter.ParseExact(entry["xfer_process_dt"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Transfer Type", entry["xfer_type"]);
                                trx.Periodic = entry["xfer_desc"];
                                trx.ExpiredDate = Formatter.ParseExact(entry["xfer_expire_dt"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.AccountSendersNumber = entry["acctidfrom"];
                                trx.ToAccountNumber = entry["acctidto"];
                                trx.ToAccountName = entry["acctidtoname"];
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.TransferNominal = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.News = entry["sendToSubject"];
                                trx.Status = entry["xfer_status"];
                                break;
                            case "GetIBTransactionByUserIdTransferDomesticInputStatus":
                                trx.InputDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferDate = Formatter.ParseExact(entry["xfer_process_dt"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Transfer Type", entry["xfer_type"]);
                                trx.FromAccountNumber = entry["acctidfrom"];
                                trx.ToAccountNumber = entry["acctidto"];
                                trx.ToAccountName = entry["acctidtoname"];
                                trx.City = entry["bank_city"];
                                trx.Branch = entry["bank_br_nm"];
                                trx.Bank = entry["bank_nm"];
                                trx.Citizen = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Citizen", entry["status_pend"]);
                                trx.WNI = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "WNI", entry["status_wn"]);
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.NominalTransfers = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.Fee = Formatter.GetParsedDouble(entry["fee_amt"], false);
                                trx.News = entry["sendtosubject"];
                                trx.ServiceTransfer = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Transfer Service", entry["is_llg"]);
                                trx.Status = entry["xfer_status"];
                                break;
                            // ORAIBSELECTTRANSFERTUNDABYCUSTIDANDPROCESSDATE
                            case "GetIBTransactionByUserIdTransferBCATxnStatus":
                                trx.InputDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferDate = Formatter.ParseExact(entry["xfer_process_dt"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferType = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Transfer Type", entry["xfer_type"]);
                                trx.Periodic = entry["xfer_desc"];
                                //trx.ExpiredDate = Formatter.ParseExact(entry["xfer_expire_dt"], "yyyy-MM-dd hh:mm:ss tt");
                                string[] xdate = entry["xfer_expire_dt"].Split(' ');
                                string[] dtx = xdate[0].Split('-');
                                string expired_dt = dtx[1] + "/" + dtx[2] + "/" + dtx[0] + " " + "12:00:00 PM";
                                trx.ExpiredDate = Formatter.ParseExact(expired_dt, "MM/dd/yyyy hh:mm:ss tt");
                                trx.AccountSendersNumber = entry["acctidfrom"];
                                trx.ToAccountNumber = entry["acctidto"];
                                trx.ToAccountName = entry["acctidtoname"];
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.TransferNominal = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.News = entry["sendtosubject"];
                                trx.Status = entry["xfer_status"];
                                break;
                            case "GetIBTransactionByUserIdTransferDomesticTxnStatus":
                                trx.InputDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.TransferDate = Formatter.ParseExact(entry["xfer_expire_dt"], "yyyy-MM-dd H:mm:ss");
                                trx.TransferType = entry["xfer_type"];
                                trx.FromAccountNumber = entry["acctidfrom"];
                                trx.ToAccountNumber = entry["acctidto"];
                                trx.ToAccountName = entry["acctidtoname"];
                                trx.City = entry["bank_city"];
                                trx.Bank = entry["bank_nm"];
                                trx.Branch = entry["bank_br_nm"];
                                trx.Citizen = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Citizen", entry["status_pend"]);
                                trx.WNI = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "WNI", entry["status_wn"]);
                                trx.Currency = Utility.GetCurrency(entry["curr"]);
                                trx.NominalTransfers = Formatter.GetParsedDouble(entry["amt"], false);
                                trx.Fee = Formatter.GetParsedDouble(entry["fee_amt"], false);
                                trx.News = entry["sendtosubject"];
                                trx.ServiceTransfer = Utility.GetDisplayText("TransactionAttributeMapping", "KlikBCA Individual", "Transfer Service", entry["is_llg"]);
                                trx.Status = entry["xfer_status"];
                                break;
                            // ORAIBSELECTTRANSFERTOLAKBYCUSTIDANDTXNDATE
                            case "GetIBTransactionByUserIdRejected":
                                trx.InputDate = Formatter.ParseExact(entry["xfer_posting_dt"], "MM/dd/yyyy hh:mm:ss t");
                                trx.TransferDate = Formatter.ParseExact(entry["tanggal_transaksi"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.PPUNumber = entry["nomor_ppu"];
                                trx.FromAccountNumber = entry["nomor_rek_pengirim"];
                                trx.ToAccountNumber = entry["nomor_rek_penerima"];
                                trx.ToAccountName = entry["nama_penerima"];
                                trx.City = entry["bank_city"];
                                trx.Bank = entry["bank_name"];
                                trx.NominalTransfers = Formatter.GetParsedDouble(entry["nominal"], false);
                                trx.Reason = entry["alasan_penolakan"];
                                break;
                            // ORAIBSELECTUSERSESSIONLOGBYCUSTID   
                            case "GetIBTransactionByUserIdUserSession":
                                DateTime? createdDate = Formatter.ParseExact(entry["created_date"], "MM/dd/yyyy hh:mm:ss tt");
                                DateTime? signOnDate = Formatter.ParseExact(entry["signon_time"], "MM/dd/yyyy hh:mm:ss tt");
                                DateTime? signOffDate = Formatter.ParseExact(entry["signoff_time"], "MM/dd/yyyy hh:mm:ss tt");
                                trx.Number = entry["pk_id"];
                                trx.TransactionDate = createdDate;
                                trx.SignOnDate = signOnDate;
                                trx.SignOffDate = signOffDate;
                                break;
                        }
                        transaction.Transactions.Add(trx);
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