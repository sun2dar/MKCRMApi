using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.Transaction
{
    public class CirrusTransaction 
    {
        //History Transaction
        [Key]
        public Guid? CirrusTransactionsID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? TransactionDate { get; set; } /*ADD BY ARDI (20151110)*/

        [StringLength(20)]
        public string AccountNo { get; set; }

        public string Date2 { get; set; }

        public string AmountIDR { get; set; }

        public string AmountUSD { get; set; }

        public string Branch { get; set; }

        public string CardNo { get; set; }

        public string Cirrus { get; set; }

        public string Complaint { get; set; }

        public string Currency { get; set; }

        public string Date { get; set; }

        public string Isis { get; set; }

        public string Location { get; set; }

        public string MUAsal { get; set; }

        public string Partial { get; set; }

        public string Rate { get; set; }

        public string Reason { get; set; }

        public string RequestId { get; set; }

        public string ResponseCode { get; set; }

        public string Reversal { get; set; }

        [StringLength(100)]
        public string Remark { get; set; } /*Add By Ardi (20151029)*/

        public string SequenceNo { get; set; }

        public string TerminalId { get; set; }

        public string Trace { get; set; }

        public string Transaction { get; set; }

        public string UserId { get; set; }

        public string DateOnly { get; set; }

        public string Time { get; set; }
        //============================================================================================

        /*Added by Ardi*/
        public string Response { get; set; }
    }
}