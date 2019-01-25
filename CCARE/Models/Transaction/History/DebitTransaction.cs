using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.Transaction
{
    public class DebitTransaction 
    {
        //History Transaction
        [Key]
        public Guid? DebitTransactionsID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? DateTime { get; set; } /*ADD BY ARDI (20151110)*/

        [StringLength(20)]
        public string AccountNo { get; set; }

        public string Date2 { get; set; }

        public string Amount { get; set; }

        public string ApprCd { get; set; }

        [StringLength(20)]
        public string Batch { get; set; }

        [StringLength(15)]
        public string CardNo { get; set; }

        [StringLength(20)]
        public string Cash { get; set; }

        [StringLength(11)]
        public string Date { get; set; }

        [StringLength(11)]
        public string DateOnly { get; set; }

        [StringLength(11)]
        public string Time { get; set; }

        [StringLength(20)]
        public string MerchantId { get; set; }

        [StringLength(150)]
        public string Merchant { get; set; }

        [StringLength(10)]
        public string MerchantName { get; set; }

        [StringLength(7)]
        public string RefNo { get; set; } //kode Tran

        [StringLength(10)]
        public string RequestId { get; set; }

        [StringLength(5)]
        public string ResponseCode { get; set; }

        [StringLength(50)]
        public string TerminalId { get; set; }

        [StringLength(100)]
        public string TraceNo { get; set; }

        [StringLength(60)]
        public string TrTp { get; set; } 

        [StringLength(5)]
        public string Cashier { get; set; }

        [StringLength(12)]
        public string RowNo { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }

        [StringLength(20)]
        public string Response { get; set; }

        //============================================================================================
        //Current
        public string Number { get; set; }

        public string TransactionCode { get; set; }

        public string TransactionDate { get; set; }

        public string TransactionTime { get; set; }

        public string ApprovalCode { get; set; }

        public string AtmCardNo { get; set; }

        public string CashAmount { get; set; }

        public string AccountNumber { get; set; }

        public string Retailer { get; set; }

        public string SequenceNo { get; set; }

        public string CardType { get; set; }
    }
}