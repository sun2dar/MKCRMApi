using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class CreditCardApplicationStatus
    {
        public string CustomerName { get; set; }
        public string BirthDate { get; set; }
        public string Birthplace { get; set; }
        public string CustomerNumber { get; set; }
        public string ApplicationId { get; set; }
        public string ReferenceNo { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public string CurrentHolder { get; set; }
        public string DateReceived { get; set; }
        public string OriginatingBranch { get; set; }
        public string Remark { get; set; }
        public string SourceCode { get; set; }
        public string DateCreated { get; set; }
        public string DateRecommended { get; set; }
        public string DateCanceled { get; set; }
        public string DateVerified { get; set; }
        public string DateApproved { get; set; }
        public string DateReject { get; set; }
        public string Comment { get; set; }
        public string Creator { get; set; }
    }
}