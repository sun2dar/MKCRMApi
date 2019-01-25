using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class BCAByPhoneTransaction
    {
        public string NextPos { get; set; }
        public string PrevPos { get; set; }
        public string CurrPos { get; set; }
        public string Msg { get; set; }
        public List<BBPTRX> Transactions { get; set; }

        public BCAByPhoneTransaction()
        {
            Transactions = new List<BBPTRX>();
            NextPos = string.Empty;
            PrevPos = string.Empty;
            CurrPos = string.Empty;
            Msg = string.Empty;
        }
    }

    public class BBPTRX
    {
        public string CustomerID { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string TransactionDesc { get; set; }
        public string ResponseCode { get; set; }
    }
}