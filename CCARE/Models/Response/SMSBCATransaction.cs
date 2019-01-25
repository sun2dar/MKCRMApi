using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class SMSBCATransaction
    {
        public SMSBCATransaction() {
            Transactions = new List<SMSBCATRX>();
        }
        public string ATMCardHolderName { get; set; }
        public string ATMCardNumber { get; set; }
        public string Status { get; set; }
        public List<SMSBCATRX> Transactions { get; set; }
    }

    public class SMSBCATRX
    {
        public DateTime? TransactionDate { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionType { get; set; }
        public string ResponseCode { get; set; }
        public double? Amount { get; set; }

        /* Payment Menu 
         * 07 12 2016 - LPG
         */

        public string ReferenceNumber { get; set; }
        public string Biller { get; set; }
        public string Receiver { get; set; }
        public string Total { get; set; }
        public string Other { get; set; }
    }
}