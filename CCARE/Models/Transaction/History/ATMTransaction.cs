using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.Transaction
{
    public class ATMTransaction 
    {
        //====================================HISTORY TRANSACTION================================================
        [Key]
        public Guid? GetHistoricalATMTransactionsID { get; set; }

        [StringLength(20)]
        public string CardNo { get; set; }

        public string TransactionDate { get; set; }

        public string Date2 { get; set; }

        [StringLength(20)]
        public string DateOnly { get; set; }

        [StringLength(15)]
        public string Time { get; set; }

        [StringLength(20)]
        public string Pan { get; set; }

        [StringLength(11)]
        public string FromAccount { get; set; }

        [StringLength(20)]
        public string ToAccount { get; set; }

        [StringLength(10)]
        public string Terminal { get; set; }

        [StringLength(7)]
        public string TransactionCode { get; set; } //kode Tran

        [StringLength(10)]
        public string ResponseCode { get; set; }

        [StringLength(5)]
        public string Currency { get; set; }

        [StringLength(50)]
        public string Rate { get; set; }

        [StringLength(100)]
        public string Amount { get; set; } // AmtRp

        [StringLength(60)]
        public string Forex { get; set; } // AmtVal

        [StringLength(5)]
        public string Company { get; set; }

        [StringLength(200)]
        public string ProductName { get; set; } /*ADD BY ARDI (20151029)*/

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? DateTime { get; set; } /*ADD BY ARDI (20151110)*/

        [StringLength(13)]
        public string Sequence { get; set; }

        [StringLength(30)]
        public string RequestID { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(100)]
        public string TransactionDescription { get; set; }

        [StringLength(50)]
        public string Response { get; set; }


        //====================================CURRENT TRANSACTION================================================
        public string Number { get; set; }

        public string Amount1 { get; set; }

        public string Amount2 { get; set; }

        public string ConversionAmount { get; set; }

        public string PayeeCode { get; set; }

        public string PayeeNumber { get; set; }

        public string SequenceNumber { get; set; }

        public string ResponseDescription { get; set; }
    }
}