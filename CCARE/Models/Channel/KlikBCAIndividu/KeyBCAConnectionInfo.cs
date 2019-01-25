using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class KeyBCAConnectionInfo
    {
        public string KeyId { get; set; }
        public string CardNo { get; set; }
        public string BranchIssuingToken { get; set; }
        public string TokenTypeKey { get; set; }
        public string TokenType { get; set; }
        public string TokenStatus { get; set; }
        public string UpdatedBy { get; set; }
        public string ActivatedBy { get; set; }
        public DateTime? ConnectedOn { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationName { get; set; }
        public string ConnectionType { get; set; }
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
        public string SNToken { get; set; }
        public string StatusKey { get; set; }
        public string Status { get; set; }
    }
}