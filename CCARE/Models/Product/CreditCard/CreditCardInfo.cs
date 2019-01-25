//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class CreditCardInfo
    {
        [Key]
        public Guid? bca_creditcardid { get; set; }
        public string bca_cctype { get; set; }

        public string CustomerName { get; set; }
        public string CycleDate { get; set; }
        public string AutoDebet { get; set; }
        public double? CreditLimit { get; set; } //1.5
        [DisplayFormat(DataFormatString = "{0:N}")]
        public double? CreditLine { get; set; }
        public double? AvailableCreditLimit { get; set; }
        public double? CashAdvancedLimit { get; set; }
        public double? PermanentCreditLimit { get; set; }
        public double? TemporaryCreditLimit { get; set; }
        public double? LimitCredit { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EffectiveDate { get; set; }
        public double? CashAdvanced { get; set; }
        public string BillingStatementSendStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? TemporaryCreditLimitExpiredDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? MaintenanceDate { get; set; }

        public string CardHolderName { get; set; }  //1.1
        public string CardType { get; set; }  //1.2
        public string AutoDebetAccountNo { get; set; }
        public string SecuredAccountNo { get; set; }
        public string AreaCard { get; set; }
        public string CreditCardNumber { get; set; } //1.7
        public string CustomerNo { get; set; }
        public string OwnershipFlag { get; set; }
        public string StatusKey { get; set; } //1.6
        public string Status { get; set; } //1.6
        public string BlockCode { get; set; }
        public string UserCode { get; set; }
        public double? CurrentBalance { get; set; } //1.3
        public double? PastDue { get; set; } //1.8
        public double? LastCreditLimit { get; set; }
        public string ExpiredDate { get; set; } //1.4
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? AnnualFeeDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreditLimitDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? LastCreditLimitDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PinMaintainDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? OpenDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CloseDate { get; set; }

        public string LastTandemStatus { get; set; }
        public string TandemWrongPinCounter { get; set; }
        public string UpdateByUserGroup { get; set; }
        public string UpdateByUserNumber { get; set; }
        public string TandemUpdateDate { get; set; }
    }
}