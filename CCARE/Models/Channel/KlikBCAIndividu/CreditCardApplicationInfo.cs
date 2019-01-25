using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class CreditCardApplicationInfo
    {
        public string RegisteredKeyId { get; set; }
        public string CustomerNo { get; set; }
        public string CreditCardNo { get; set; }
        public string CustomerName { get; set; }
        public string ConnectionType { get; set; }
        public string ApplicationName { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Reason { get; set; }
        public DateTime? ConnectionDate { get; set; }
        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Address6 { get; set; }
        public string Address7 { get; set; }
        public string Address8 { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
    }
}