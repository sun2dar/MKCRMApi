using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
	public class SMSBCAInfo
	{
        public string AtmCardNo { get; set; }
        public string MobileNumber { get; set; }
		public string CardOwnerName { get; set; }
		public DateTime? RegistrationDate { get; set; }
		public DateTime? LastRegistrationDate { get; set; }
		public DateTime? LastTransactionDate { get; set; }
        public string StatusKey { get; set; }
        public string Status { get; set; }
        public string AccessCodeCounter { get; set; }
		public string UpdateOfficer { get; set; }
		public DateTime? UpdateDate { get; set; }
    }
}