using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CCARE.Models;
using CCARE.Models.Transaction;
using Newtonsoft.Json;
using CCARE.App_Function;

namespace CCARE.Controllers
{
    public class KBITransactionController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getSearchby = Request["_searchby"];
            string getVal = Request["_val"];
            string getStartDate = Request["_startDate"];
            string getEndDate = Request["_endDate"];
            string getTrxType = Request["_trxType"];

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            param.RequestTransType = 
                "AccountInformation".Equals(getTrxType) ? "GetIBTransactionByUserIdAcctInfo" :
                "BCAAccountTransfer".Equals(getTrxType) ? "GetIBTransactionByUserIdTransferBCA" :
                "CreditCardInformation".Equals(getTrxType) ? "GetCreditCardLogIBankByUserId" :
                "CreditCustomerInformation".Equals(getTrxType) ? "GetIBTransactionByUserIdConsumerCreditInfo" :
                "EcommercePayment".Equals(getTrxType) ? "GetIBTransactionByUserIdPaymentECommerce" :
                "InvestmentProductInformation".Equals(getTrxType) ? "GetIBTransactionByUserIdInvesProductInfo" :
                "OtherBankAccountTransfer".Equals(getTrxType) ? "GetIBTransactionByUserIdTransferDomestic" :
                "Payment".Equals(getTrxType) ? "GetIBTransactionByUserIdPayment" :
                "Purchase".Equals(getTrxType) ? "GetIBTransactionByUserIdPurchase" :
                "TransferReject".Equals(getTrxType) ? "GetIBTransactionByUserIdRejected" :
                "TransferStatusBCAAccountInput".Equals(getTrxType) ? "GetIBTransactionByUserIdTransferBCAInputStatus" :
                "TransferStatusBCAAccountTransaction".Equals(getTrxType) ? "GetIBTransactionByUserIdTransferBCATxnStatus" :
                "TransferStatusOtherBankAccountInput".Equals(getTrxType) ? "GetIBTransactionByUserIdTransferDomesticInputStatus" :
                "TransferStatusOtherBankAccountTransaction".Equals(getTrxType) ? "GetIBTransactionByUserIdTransferDomesticTxnStatus" :
                "UserSession".Equals(getTrxType) ? "GetIBTransactionByUserIdUserSession" :
                "VirtualAccountTransfer".Equals(getTrxType) ? "GetIBTransactionByUserIdVirtualAcct" :
                "TopUpWallet".Equals(getTrxType) ? "GetIBTransactionByUserIdTopUpWallet" :
                "AKSesFundWithdrawal".Equals(getTrxType) ? "GetIBTransactionByUserIdAKSesFundWithdrawal" : 
                string.Empty;


            if ("userId".Equals(getSearchby)) { 
                param.Parameter.Add("userId", getVal);
            }
            if ("atmNo".Equals(getSearchby))
            {
                param.Parameter.Add("atmNo", getVal);
            }

            param.Parameter.Add("startDate", getStartDate);
            param.Parameter.Add("endDate", getEndDate);

            KBITransaction model = ChannelTransaction.KBI(param);
            var trx = model.Transactions.ToList().Select(x => new
                    {
                        ExpiredDate = x.ExpiredDate,
                        InputDate = x.InputDate,
                        MiddlewareDate = x.MiddlewareDate,
                        SignOffDate = x.SignOffDate,
                        SignOnDate = x.SignOnDate,
                        TandemDate = x.TandemDate,
                        TransactionDate = x.TransactionDate,
                        TransferDate = x.TransferDate,
                        Forex = x.Forex,
                        Amount = x.Amount,
                        AmountIDR = x.AmountIDR,
                        ConversiNominal = x.ConversiNominal,
                        Cost = x.Cost,
                        ExchangeRate = x.ExchangeRate,
                        Fee = x.Fee,
                        Nominal = x.Nominal,
                        NominalTransfers = x.NominalTransfers,
                        TransferAmount = x.TransferAmount,
                        TransferNominal = x.TransferNominal,
                        FromAccountId = x.FromAccountId,
                        AccountNumber = x.AccountNumber,
                        AccountPaymentNumber = x.AccountPaymentNumber,
                        AccountSendersNumber = x.AccountSendersNumber,
                        Bank = x.Bank,
                        BillerId = x.BillerId,
                        BillerRefInfo = x.BillerRefInfo,
                        Branch = x.Branch,
                        Cause = x.Cause,
                        Citizen = x.Citizen,
                        City = x.City,
                        Currency = x.Currency,
                        CustomerName = x.CustomerName,
                        CustomerNumber = x.CustomerNumber,
                        Description = x.Description,
                        Email = x.Email,
                        FlagToken = x.FlagToken,
                        FromAccountNumber = x.FromAccountNumber,
                        InformationType = x.InformationType,
                        LateChargeAmount = x.LateChargeAmount,
                        MiddlewareStatus = x.MiddlewareStatus,
                        MUFromAccountNumber = x.MUFromAccountNumber,
                        MUTransaction = x.MUTransaction,
                        News = x.News,
                        Number = x.Number,
                        PaymentAccountNumber = x.PaymentAccountNumber,
                        PaymentFor = x.PaymentFor,
                        Periodic = x.Periodic,
                        PPUNumber = x.PPUNumber,
                        Process = x.Process,
                        Reason = x.Reason,
                        Reference = x.Reference,
                        ReferenceNo = x.ReferenceNo,
                        ReferenceNumber = x.ReferenceNumber,
                        SenderAccountNo = x.SenderAccountNo,
                        SendToSubject = x.SendToSubject,
                        ServiceTransfer = x.ServiceTransfer,
                        Status = x.Status,
                        TandemStatus = x.TandemStatus,
                        ToAccountName = x.ToAccountName,
                        ToAccountNoHp = x.ToAccountNoHp,
                        ToAccountNumber = x.ToAccountNumber,
                        ToAccountType = x.ToAccountType,
                        Token = x.Token,
                        TokenStatus = x.TokenStatus,
                        TransactionType = x.TransactionType,
                        TransferService = x.TransferService,
                        TransferToAccount = x.TransferToAccount,
                        TransferToAccountName = x.TransferToAccountName,
                        TransferType = x.TransferType,
                        UserId = x.UserId,
                        WNI = x.WNI
                    });
            
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = trx.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            trx = trx.Skip((page - 1) * pageSize).Take(pageSize);
            var recordCount = trx.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = trx.ToArray();

            if ("userId".Equals(getSearchby))
            {
                jTable.additional = model.CustomerName + "<@z>" + model.CardNumber;
            }
            if ("atmNo".Equals(getSearchby))
            {
                jTable.additional = model.CustomerName + "<@z>" + model.UserId;
            }

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }
    }
}
