using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.Transaction
{
    public class ATMSwitchingTransaction 
    {
        //History Transaction
        [Key]
        public Guid? ATMSwitchingTransactionsID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? TransactionDate { get; set; } /*ADD BY ARDI (20151110)*/

        public string Branch { get; set; }

        public string Date2 { get; set; }

        public string Cirrus { get; set; }

        public string Isis { get; set; }

        public string TransactionCode { get; set; }

        public string RateIDR { get; set; }

        public string RateUSD { get; set; }

        public string Location { get; set; }

        public string CardNo { get; set; }

        public string AmountAsal { get; set; }

        public string AmountIDR { get; set; }

        public string AmountUSD { get; set; }

        public string AccountNo { get; set; }

        public string Partial { get; set; }

        public string Reason { get; set; }

        public string Response { get; set; }

        public string Reversal { get; set; } 

        public string Sequence { get; set; }

        public string Date { get; set; }

        public string Trace { get; set; }

        public string TerminalId { get; set; }

        public string DateOnly { get; set; }

        public string Time { get; set; }
        //============================================================================================

    }
}