using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class AllChannelTransaction
    {
        public bool RestrictedAccount { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public List<ACTRX> Transactions { get; set; }
        public AllChannelTransaction()
        {
            Transactions = new List<ACTRX>();
        }
    }

    public class ACTRX {
        public string Name { get; set; }
        public string Currency { get; set; }
        public string Number { get; set; }
        public string TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string displayTransactionDate { get; set; }
        public string TransactionDescription { get; set; }
        public string Branch { get; set; }
        public double? Amount { get; set; }
    }

}