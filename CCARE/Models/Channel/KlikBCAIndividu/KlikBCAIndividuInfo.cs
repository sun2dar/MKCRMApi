using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class KlikBCAIndividuInfo
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string AtmCardNo { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string WrongPinCounter { get; set; }
        public string BlockStatusKey { get; set; }
        public string BlockStatus { get; set; }
        public string UserIdIBankStatusKey { get; set; }
        public string UserIdIBankStatus { get; set; }
        public string TandemStatusKey { get; set; }
        public string TandemStatus { get; set; }
        public string ChangePasswordCounter { get; set; }
        public string Disclaimer { get; set; }
        public string Language { get; set; }
        public string EmailStatus { get; set; }
        public string ReferenceNo { get; set; }
        
        public string MalwareStatusKey { get; set; }
        public string MalwareStatus { get; set; }
        public string MalwareBlockedDate { get; set; }
        public string MalwareLastUpdate { get; set; }
    }
}