using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.Transaction
{
    public class CorrectionTransaction 
    {
        [Key]
        public Guid? CorrectionTransactionsID { get; set; }

        public string Name { get; set; }

        public string Date2 { get; set; }

        public string Branch { get; set; }

        public string Currency { get; set; }

        public string AccountNo { get; set; }

        public string TransactionDate { get; set; }

        public string InputDate { get; set; }

        public string DebitCredit { get; set; }

        public string ProsesDate { get; set; }

        public string Wsid { get; set; }

        public string Slid { get; set; }

        public string Kt { get; set; }

        public string Rate { get; set; }

        public string NilaiIDR { get; set; }

        public string NilaiVAL { get; set; }

        public string HasilIDR { get; set; }

        public string HasilVAL { get; set; }

        public string GagalIDR { get; set; }

        public string GagalVAL { get; set; } 

        public string Description { get; set; }

        public string RowNo { get; set; }

        public string RequestId { get; set; }

        public string DateOnly { get; set; }

        public string Time { get; set; }
        //============================================================================================

    }
}