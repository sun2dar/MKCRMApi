using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class Merchant
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string UserNumber { get; set; }
        public string TerminalId { get; set; }
        public string RetailerId { get; set; }
        public string RetailerGroup { get; set; }
        public string MerchantName { get; set; }
        public string TandemMerchantName { get; set; }
        public string MerchantType { get; set; }
        public string TerminalType { get; set; }
        public string TerminalStatus { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string TelephoneNo { get; set; }
    }
}