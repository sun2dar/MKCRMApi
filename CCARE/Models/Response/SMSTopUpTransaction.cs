using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class SMSTopUpTransaction
    {
        public SMSTopUpTransaction() {
            Transactions = new List<SMSTopUpTRX>();
        }
        public string ATMCardHolderName { get; set; }
        public string ATMCardNumber { get; set; }
        public string Status { get; set; }
        public List<SMSTopUpTRX> Transactions { get; set; }
    }

    public class SMSTopUpTRX
    {
        public string AtmCardNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string RequestId { get; set; }
        public string AccountNumber { get; set; }
        public double? ValueOfTransactions { get; set; }
        public string ResponseCode { get; set; }
    }
}