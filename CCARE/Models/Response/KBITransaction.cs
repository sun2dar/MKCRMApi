using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class KBITransaction
    {
        public string CustomerName { get; set; }
        public string UserId { get; set; }
        public string CardNumber { get; set; }
        public List<KBITRX> Transactions { get; set; }

        public KBITransaction()
        {
            this.Transactions = new List<KBITRX>();
        }
    }

    public class KBITRX
    {
        public DateTime? ExpiredDate { get; set; }
        public DateTime? InputDate { get; set; }
        public DateTime? MiddlewareDate { get; set; }
        public DateTime? SignOffDate { get; set; }
        public DateTime? SignOnDate { get; set; }
        public DateTime? TandemDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public double? Forex { get; set; }
        public double? Amount { get; set; }
        public double? AmountIDR { get; set; }
        public double? ConversiNominal { get; set; }
        public double? Cost { get; set; }
        public double? ExchangeRate { get; set; }
        public double? Fee { get; set; }
        public double? Nominal { get; set; }
        public double? NominalTransfers { get; set; }
        public double? TransferAmount { get; set; }
        public double? TransferNominal { get; set; }
        public long? FromAccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountPaymentNumber { get; set; }
        public string AccountSendersNumber { get; set; }
        public string Bank { get; set; }
        public string BillerId { get; set; }
        public string BillerRefInfo { get; set; }
        public string Branch { get; set; }
        public string Cause { get; set; }
        public string Citizen { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string FlagToken { get; set; }
        public string FromAccountNumber { get; set; }
        public string InformationType { get; set; }
        public string LateChargeAmount { get; set; }
        public string MiddlewareStatus { get; set; }
        public string MUFromAccountNumber { get; set; }
        public string MUTransaction { get; set; }
        public string News { get; set; }
        public string Number { get; set; }
        public string PaymentAccountNumber { get; set; }
        public string PaymentFor { get; set; }
        public string Periodic { get; set; }
        public string PPUNumber { get; set; }
        public string Process { get; set; }
        public string Reason { get; set; }
        public string Reference { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceNumber { get; set; }
        public string SenderAccountNo { get; set; }
        public string SendToSubject { get; set; }
        public string ServiceTransfer { get; set; }
        public string Status { get; set; }
        public string TandemStatus { get; set; }
        public string ToAccountName { get; set; }
        public string ToAccountNoHp { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToAccountType { get; set; }
        public string Token { get; set; }
        public string TokenStatus { get; set; }
        public string TransactionType { get; set; }
        public string TransferService { get; set; }
        public string TransferToAccount { get; set; }
        public string TransferToAccountName { get; set; }
        public string TransferType { get; set; }
        public string UserId { get; set; }
        public string WNI { get; set; }

        public KBITRX()
        {
            ExpiredDate = DateTime.Now;
            InputDate = DateTime.Now;
            MiddlewareDate = DateTime.Now;
            SignOffDate = DateTime.Now;
            SignOnDate = DateTime.Now;
            TandemDate = DateTime.Now;
            TransactionDate = DateTime.Now;
            TransferDate = DateTime.Now;

            AccountNumber = string.Empty;
            AccountPaymentNumber = string.Empty;
            AccountSendersNumber = string.Empty;
            Bank = string.Empty;
            BillerId = string.Empty;
            BillerRefInfo = string.Empty;
            Branch = string.Empty;
            Cause = string.Empty;
            Citizen = string.Empty;
            City = string.Empty;
            Currency = string.Empty;
            CustomerName = string.Empty;
            CustomerNumber = string.Empty;
            Description = string.Empty;
            Email = string.Empty;
            FlagToken = string.Empty;
            FromAccountNumber = string.Empty;
            InformationType = string.Empty;
            LateChargeAmount = string.Empty;
            MiddlewareStatus = string.Empty;
            MUFromAccountNumber = string.Empty;
            MUTransaction = string.Empty;
            News = string.Empty;
            Number = string.Empty;
            PaymentAccountNumber = string.Empty;
            PaymentFor = string.Empty;
            Periodic = string.Empty;
            PPUNumber = string.Empty;
            Process = string.Empty;
            Reason = string.Empty;
            Reference = string.Empty;
            ReferenceNo = string.Empty;
            ReferenceNumber = string.Empty;
            SenderAccountNo = string.Empty;
            SendToSubject = string.Empty;
            ServiceTransfer = string.Empty;
            Status = string.Empty;
            TandemStatus = string.Empty;
            ToAccountName = string.Empty;
            ToAccountNoHp = string.Empty;
            ToAccountNumber = string.Empty;
            ToAccountType = string.Empty;
            Token = string.Empty;
            TokenStatus = string.Empty;
            TransactionType = string.Empty;
            TransferService = string.Empty;
            TransferToAccount = string.Empty;
            TransferToAccountName = string.Empty;
            TransferType = string.Empty;
            UserId = string.Empty;
            WNI = string.Empty;

            Forex = 0.0;
            Amount = 0.0;
            AmountIDR = 0.0;
            ConversiNominal = 0.0;
            Cost = 0.0;
            ExchangeRate = 0.0;
            Fee = 0.0;
            Fee = 0.0;
            Nominal = 0.0;
            NominalTransfers = 0.0;
            TransferAmount = 0.0;
            TransferNominal = 0.0;
            FromAccountId = 0; 
        }
    }


}