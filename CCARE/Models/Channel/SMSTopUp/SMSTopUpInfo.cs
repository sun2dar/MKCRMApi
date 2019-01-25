using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
	public class SMSTopUpInfo
	{
        public string AtmCardNo { get; set; }
		public string MobileNumber { get; set; }
		public string CardholderName { get; set; }
		public DateTime? RegistrationDate { get; set; }
		public DateTime? LastRegistrationDate { get; set; }
		public DateTime? LastTransactionDate { get; set; }
        public string smsTopUpKey { get; set; }
        public string smsTopUp { get; set; }
        public string CounterCodeAccess { get; set; }
		public string ProviderCode { get; set; }
		public string UpdateOfficer { get; set; }
		public DateTime? UpdateDate { get; set; }
    }
}