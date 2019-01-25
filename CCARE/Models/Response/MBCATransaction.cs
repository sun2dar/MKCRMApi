using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class MBCATransaction
    {
        public MBCATransaction()
        {
            this.Transactions = new List<MBTRX>();
        }

        public string CustomerName { get;  set; }
        public string ATMCardNumber { get;  set; }
        public string Status { get;  set; }
        public string HandPhoneNumberOnTandem { get;  set; }
        public string CustomerNameTandem { get;  set; }
        public string StatusTandem { get;  set; }
        public List<MBTRX> Transactions { get; set; }
    }

    public class MBTRX
    {
        /*
         * Cash Out OTP
         * Tanggal Create OTP   -> MiddlewareDate
         * Status OTP           -> Status
         * No Kartu             -> ATMCardNumber
         * Tanggal Transaksi    -> TandemDate
         * No Rekening          -> AccountNumber
         * Nominal (IDR)        -> Amount
         */

        public string MiddlewareDate { get; set; }
        public string TandemDate { get; set; }

        public string AccountNumber { get; set; }
        public string ATMCardNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }

        public string PaymentAccountNumber { get; set; }
        public string AccountSendersNumber { get; set; }
        public string CodeAndBankName { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToAccountMU { get; set; }
        public string ToAccountName { get; set; }

        public string AdminType { get; set; }
        public string TransactionType { get; set; }
        public string PaymentType { get; set; }
        public string PaymentFor { get; set; }

        public string Currency { get; set; }
        public double? Nominal { get; set; }
        public double? Amount { get; set; }
        public double? AmountForex { get; set; }
        public double? ExchangesRate { get; set; }

        public string Status { get; set; }
        public string DescCode { get; set; }
        public string Information { get; set; }
        public string ReferenceNumber { get; set; }

        public MBTRX()
        {
            MiddlewareDate = string.Empty;
            TandemDate = string.Empty;

            CustomerName = string.Empty;
            ATMCardNumber = string.Empty;
            AccountNumber = string.Empty;
            CustomerNumber = string.Empty;

            PaymentAccountNumber = string.Empty;
            AccountSendersNumber = string.Empty;
            CodeAndBankName = string.Empty;
            ToAccountNumber = string.Empty;
            ToAccountMU = string.Empty;
            ToAccountName = string.Empty;

            AdminType = string.Empty;
            TransactionType = string.Empty;
            PaymentType = string.Empty;
            PaymentFor = string.Empty;

            Currency = string.Empty;
            Nominal = 0.0;
            Amount = 0.0;
            AmountForex = 0.0;
            ExchangesRate = 0.0;

            Status = string.Empty;
            DescCode = string.Empty;
            Information = string.Empty;
            ReferenceNumber = string.Empty;
        }
    }


   
}