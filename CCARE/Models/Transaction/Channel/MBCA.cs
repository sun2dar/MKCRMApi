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
        public static MBCATransaction MBCA(Params request)
        {
            MBCATransaction transaction = new MBCATransaction();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            string dateFormat = string.Empty; 
            try
            {
                param.RequestTransType = request.RequestTransType;

                dateFormat = "GetMBTransactionOTP".Equals(param.RequestTransType) ? "dd-MM-yyyy" : "MM/dd/yyyy";

                param.Parameter.Add("GetMBTransactionOTP".Equals(param.RequestTransType) ? "CustomerID" : "FK_CustID", 
                                        request.Parameter.ContainsKey("mobileNo") ? request.Parameter["mobileNo"].Trim() : string.Empty);                    
                
                if ("GetMBTransactionMCommerce".Equals(param.RequestTransType) || "GetMBTransactionMPayment".Equals(param.RequestTransType))
                {
                    param.Parameter.Add("TxType", "MBAccExecPaymentRq");
                }
                if ("GetMBTransactionMTransfer".Equals(param.RequestTransType))
                {
                    param.Parameter.Add("TxType1", "MBAccXFerExecRq");
                    param.Parameter.Add("TxType2", "MBAccGetCustomerInfo");
                }
                if ("GetMBTransactionMInfo".Equals(param.RequestTransType))
                {
                    // MBGetInfoCC - Info KK
                    param.Parameter.Add("Process1", "MBAccGetBalanceRq");               // Info Saldo
                    param.Parameter.Add("Process2", "MBAcctStmtRq");                    // Mutasi Rekening
                    param.Parameter.Add("Process3", "MBGetAcctStmtCCRq");               // Info Transaksi KK
                    param.Parameter.Add("Process4", "MBGetBalanceCCR");                 // Info Saldo KK
                    param.Parameter.Add("Process5", "MBGetBalanceDepR");                // Info Deposito
                    param.Parameter.Add("Process6", "MBGetBalReksaDanaRq ");            // Info Saldo Reksadana
                    param.Parameter.Add("Process7", "MBGetBankCod");                    // Info Kode Bank
                    param.Parameter.Add("Process8", "MBGetCompCod");                    // Info Kode Perusahaan
                    param.Parameter.Add("Process9", "MBGetExChgRateRq");                // Info Kurs
                    param.Parameter.Add("Process10", "MBGetKreditKonsumerRq");          // Info Kredit Konsumer
                    param.Parameter.Add("Process11", "MBGetNABReksaDanaRq ");           // Info NAB Reksadana
                    param.Parameter.Add("Process12", "MBInqCouponR");                   // Info Kupon
                    param.Parameter.Add("Process13", "MBIntRateDepIDRRq");              // Info Suku Bunga Deposito Rupiah
                    param.Parameter.Add("Process14", "MBIntRateDepValRq");              // Info Suku Bunga Deposito Valas
                    param.Parameter.Add("Process15", "MBIntRateSavingRq");              // Info Suku Bunga Tabungan
                    param.Parameter.Add("Process16", "MBRDNAcctStmtRq");                // Saldo RDN
                    param.Parameter.Add("Process17", "MBRDNGetBalanceRq");              // Info Mutasi RDN
                    param.Parameter.Add("Process18", "MBVerPIN");                       // Verifikasi PIN
                }
                if ("GetMBTransactionMAdmin".Equals(param.RequestTransType))
                {
                    param.Parameter.Add("Proses1", "MBBlockATMCardRq");                 // Blokir Kartu ATM
                    param.Parameter.Add("Proses2", "MBBlockCCRq");                      // Kartu Kredit
                    param.Parameter.Add("Proses3", "MBCCActivationConnectionRq");       // Koneksi KK setelah Aktivasi
                    param.Parameter.Add("Proses4", "MBCCActivationRq");                 // Aktivasi Kartu Kredit
                    param.Parameter.Add("Proses5", "MBChangePINRq");                    // Ganti PIN
                    param.Parameter.Add("Proses6", "MBDelBCACCRq");                     // Hapus KK
                    param.Parameter.Add("Proses7", "MBDelPaymentListRq");               // Hapus Daftar Pembayaran
                    param.Parameter.Add("Proses8", "MBDelSAKUListRq");                  // Daftar Transfer Sakuku
                    param.Parameter.Add("Proses9", "MBDelTranferListRq");               // Hapus Daftar Transfer Antar Rek
                    param.Parameter.Add("Proses10", "MBDelTranferSwitchingListRq");     // Daftar Transfer Antar Bank
                    param.Parameter.Add("Proses11", "MBDelVAListRq");                   // Hapus Daftar Transfer VA
                    param.Parameter.Add("Proses12", "MBLoginBlockATMCardRq");           // Login Blokir Kartu ATM
                    param.Parameter.Add("Proses13", "MBPINCCActivationRq");             // PIN Kartu Kredit
                    param.Parameter.Add("Proses14", "MBRegBCACCRq");                    // Registrasi KK
                    param.Parameter.Add("Proses15", "ActivationRq");                    // Aktivasi
                    param.Parameter.Add("Proses16", "");
                    param.Parameter.Add("Proses17", "");
                    param.Parameter.Add("Proses18", "");
                }

                param.Parameter.Add("StartDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["startDate"]), dateFormat));
                param.Parameter.Add("EndDate", Formatter.ToStringExact(Convert.ToDateTime(request.Parameter["endDate"]).AddDays(1), dateFormat));

                if ("GetMBTransactionMCommerce".Equals(param.RequestTransType))
                {
                    param.Parameter.Add("PaidType", "PURCHASE");
                }
                if ("GetMBTransactionMPayment".Equals(param.RequestTransType)) 
                {
                    param.Parameter.Add("PaidType", "PAYMENT");                    
                }

                param.WSDL = "GetMBTransactionOTP".Equals(param.RequestTransType) ? "OTPCashOut" : "ESBDBDelimiter";

                ESBData data = EAI.RetrieveESBData(param);

                if (data.Result != null && data.Result.Count > 0)
                {
                    foreach (StringDictionary entry in data.Result)
                    { 
                        DateTime? middlewareDate = Formatter.ParseExact(entry["txt_date"], "MM/dd/yyyy hh:mm:ss tt");
                        DateTime? tandemDate = Formatter.ParseExact(entry["tandemtxdate"], "ddMMHHmmss");

                        switch (param.RequestTransType) 
                        {
                            case "GetMBTransactionMCommerce":
                                transaction.Transactions.Add(new MBTRX()
                                {
                                    MiddlewareDate = Formatter.ToStringExact(middlewareDate, "dd/MM/yyyy HH:mm:ss"),
                                    TandemDate = Formatter.ToStringExact(tandemDate, "dd/MM HH:mm:ss"),
                                    ATMCardNumber = entry["cardacctid"],
                                    AccountNumber = entry["acctidfrom"],
                                    CustomerNumber = entry["billreffinfo"],
                                    PaymentType = entry["desceng"],
                                    Nominal = Formatter.GetParsedDouble(entry["amt"], false),
                                    Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Middleware Status", entry["statuscode"]),
                                    DescCode = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Tandem Status", entry["srvrstatuscode"]),
                                    Information = entry["dccustaddress"],
                                    ReferenceNumber = entry["refcode"]
                                });
                                break;

                            case "GetMBTransactionMPayment":
                                transaction.Transactions.Add(new MBTRX()
                                {
                                    MiddlewareDate = Formatter.ToStringExact(middlewareDate, "dd/MM/yyyy HH:mm:ss"),
                                    TandemDate = Formatter.ToStringExact(tandemDate, "dd/MM HH:mm:ss"),
                                    ATMCardNumber = entry["cardacctid"],
                                    PaymentAccountNumber = entry["acctidfrom"],
                                    CustomerNumber = entry["billreffinfo"],
                                    PaymentFor = entry["desceng"],
                                    Nominal = Formatter.GetParsedDouble(entry["amt"], false),
                                    Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Middleware Status", entry["statuscode"]),
                                    DescCode = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Tandem Status", entry["srvrstatuscode"]),
                                    Information =  entry["dccustaddress"],
                                    ReferenceNumber = entry["refcode"]
                                });
                                break;

                            case "GetMBTransactionMTransfer":
                                string reqId = string.IsNullOrEmpty(entry["rquid"]) ? string.Empty : entry["rquid"];
                                string billerAccountId = string.IsNullOrEmpty(entry["billeracctid"]) ? string.Empty : entry["billeracctid"];
                                string statusCode = entry["statuscode"];
                                double? kurs = 0;
                                double? amount = 0;
                                double? amountForx = 0;
                                string currency = Utility.GetCurrency(entry["curr"]);

                                if (statusCode == "00" && !string.IsNullOrEmpty(entry["forexrateamt"]))
                                {
                                    kurs = Formatter.GetParsedDouble(entry["forexrateamt"], false);
                                }

                                if (((statusCode != "00" && currency == "360") || statusCode == "00") && !string.IsNullOrEmpty(entry["amtidr"]))
                                {
                                    amount = Formatter.GetParsedDouble(entry["amtidr"], false);
                                }

                                if (((statusCode != "00" && currency == "360") || statusCode == "00") && !string.IsNullOrEmpty(entry["amtforex"]))
                                {
                                    amountForx = Formatter.GetParsedDouble(entry["amtforex"], false);
                                }

                                transaction.Transactions.Add(new MBTRX()
                                {
                                    MiddlewareDate = Formatter.ToStringExact(middlewareDate, "dd/MM/yyyy HH:mm:ss"),
                                    TandemDate = Formatter.ToStringExact(tandemDate, "dd/MM HH:mm:ss"),
                                    ATMCardNumber = entry["cardacctid"],
                                    AccountSendersNumber = entry["acctidfrom"],
                                    CodeAndBankName = billerAccountId + reqId,
                                    ToAccountNumber = entry["acctidto"],
                                    ToAccountMU = Utility.GetCurrency(entry["acctcurrto"]),
                                    ToAccountName = entry["acctidtoname"],
                                    Currency = currency,
                                    Amount = amount,
                                    AmountForex = amountForx,
                                    ExchangesRate = kurs,
                                    Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Middleware Status", entry["statuscode"]),
                                    DescCode = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Tandem Status", entry["srvrstatuscode"]),
                                    Information = entry["dccustaddress"],
                                    ReferenceNumber = entry["refcode"]
                                });
                                break;

                            case "GetMBTransactionMInfo":
                                middlewareDate = Formatter.ParseExact(entry["createddate"], "MM/dd/yyyy hh:mm:ss tt");
                                transaction.Transactions.Add(new MBTRX()
                                {
                                    MiddlewareDate = Formatter.ToStringExact(middlewareDate, "dd/MM/yyyy HH:mm:ss"),
                                    TransactionType = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Transaction Type", entry["process"]),
                                    ATMCardNumber = entry["cardnumber"],
                                    AccountNumber = entry["acctidfrom"],
                                    CustomerName = entry["custname"],
                                    Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Middleware Status", entry["statuscode"]),
                                    DescCode = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Tandem Status", entry["srvrstatuscode"]),
                                    Information = entry["rquid"]
                                });
                                break;

                            case "GetMBTransactionMAdmin":
                                middlewareDate = Formatter.ParseExact(entry["createddate"], "MM/dd/yyyy hh:mm:ss tt");
                                transaction.Transactions.Add(new MBTRX()
                                {
                                    MiddlewareDate = Formatter.ToStringExact(middlewareDate, "MM/dd/yyyy HH:mm:ss"),
                                    AdminType = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Admin Type", entry["process"]),
                                    ATMCardNumber = entry["cardnumber"],
                                    CustomerName = entry["custname"],
                                    Status = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Middleware Status", entry["statuscode"]),
                                    DescCode = Utility.GetMiddlewareTandemStatus("TransactionAttributeMapping", "M-BCA", "Tandem Status", entry["srvrstatuscode"]),
                                    Information = entry["rquid"]
                                });
                                break;

                            case "GetMBTransactionOTP":
                                transaction.Transactions.Add(new MBTRX() {
                                    MiddlewareDate = entry["CreatedDate"],
                                    Status = entry["Status"],
                                    ATMCardNumber = entry["CardNumber"],
                                    TandemDate = entry["TransactionDate"],
                                    AccountNumber = entry["AccountNumber"],
                                    Amount = Formatter.GetParsedDouble(entry["Amount"], false)
                                });
                                break;
                        }
                    }

                    param.Parameter = new Dictionary<string, string>();
                    param.RequestTransType = "GetMBankInfoByMobileNo";
                    param.Parameter.Add("CustId", request.Parameter.ContainsKey("mobileNo") ? request.Parameter["mobileNo"].Trim() : string.Empty);
                    param.WSDL = "ESBDBDelimiter";
                    ESBData information = EAI.RetrieveESBData(param);
                    if (information.Result != null && information.Result.Count > 0)
                    {
                        transaction.ATMCardNumber = information.Result[0]["cardnumber"];
                        transaction.CustomerName = information.Result[0]["custname"];
                        transaction.Status = Utility.GetStatusInfo(information.Result[0]["status"]);

                        param.Parameter = new Dictionary<string, string>();
                        param.RequestTransType = "GetTandemStatusMBank";
                        param.Parameter.Add("atmNo", information.Result[0]["cardnumber"]);
                        param.Parameter.Add("mobileNo", request.Parameter["mobileNo"]);
                        param.WSDL = "MBManagement";
                        ESBData tandem = EAI.RetrieveESBData(param);
                        transaction.HandPhoneNumberOnTandem = tandem.Result[0][MobileBankingTandemInquiryStatusResultKeyName.MobileNo];
                        transaction.CustomerNameTandem = tandem.Result[0][MobileBankingTandemInquiryStatusResultKeyName.CustomerName];
                        transaction.StatusTandem = Utility.GetStatusInfo(tandem.Result[0][MobileBankingTandemInquiryStatusResultKeyName.UserIdStatus]);
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