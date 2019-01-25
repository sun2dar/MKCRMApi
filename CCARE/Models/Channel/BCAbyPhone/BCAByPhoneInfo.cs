using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
	public class BCAByPhoneInfo
	{
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string CustomerCategoryKey { get; set; }
        public string CustomerCategory { get; set; }
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
		public string AccountNo3 { get; set; }
		public string AccountNo4 { get; set; }
        public DateTime? RegistrationDate { get; set; }
		public DateTime? LastVerifyFlagUpdate { get; set; }
		public DateTime? LastUpdateDate { get; set; }
		public string UpdateByUserGroup { get; set; }
		public string UpdateByUserNumber { get; set; }
		public string PinChangeCounter { get; set; }
		public string WrongPinCounter { get; set; }
        public string LastStatusKey { get; set; }
        public string LastStatus { get; set; }
    }
}