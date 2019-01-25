using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class MBCAInfo
    {
        public string CustomerName { get; set; }
        public string AtmNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? LastTransactionDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string WrongPinCounter { get; set; }
        public string PinActivation { get; set; }
        public DateTime? ActivationFinDate { get; set; }
        public string ActivationFin { get; set; }
        public string BlockStatusKey { get; set; }
        public string BlockStatus { get; set; }

        public string TandemCustomerName { get; set; }
        public string TandemBlockStatus { get; set; }
        public string TandemCardNo { get; set; }
        public string TandemHpNo { get; set; }
        public DateTime? TandemRegistrationDate { get; set; }
        public DateTime? TandemRegistrationTime { get; set; }

        
        public string ChangePinCounter { get; set; }
        public string Disclaimer { get; set; }
        public string Language { get; set; }
    }
}