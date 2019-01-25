using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class CreditCardConnectionInfo
    {
        public string KeyId { get; set; }
        public string CustomerNoCreditCard { get; set; }
        public string AtmCardNo { get; set; }
        public string CustomerName { get; set; }
        public string ConnectionType { get; set; }
        public string ApplicationName { get; set; }
        public DateTime? ConnectionDate { get; set; }
        public string Id1 { get; set; }
        public string Id2 { get; set; }
    }
}