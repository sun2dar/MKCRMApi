using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
	public class KlikBCABisnisInfo
	{
		public string SNKeyBca { get; set; }
		public string CardNo { get; set; }
		public string KeyId { get; set; }
		public string BranchName { get; set; }
		public string CreatedDate { get; set; }
        public string TokenTypeKey { get; set; }
        public string TokenType { get; set; }

		public string UserID { get; set; }
		public string UserName { get; set; }
		public string Email1 { get; set; }
		public string Email2 { get; set; }

        public string ApplicationName { get; set; }
        public string Action { get; set; }
        public string LastStatusKey { get; set; }
        public string LastStatus { get; set; }
        public string KeyBCAStatusKey { get; set; }
        public string KeyBCAStatus { get; set; }
        public string KeyBCAStatusLabel { get; set; }

        public string ActivatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
		public string ID1 { get; set; }
		public string ID2 { get; set; }
		public DateTime? RegisterDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public string CorpID { get; set; }
		public string CorpName { get; set; }
		public string CorpEmail { get; set; }
        public string AccountNo { get; set; }
    }

    public class KBIMalwareLogInfo
    {
        public string createdDate { get; set; }
        public string userId { get; set; }
        public string activity { get; set; }
        public string updateOfficer { get; set; }
        public string malwareType { get; set; }
        public string custIP { get; set; }
        public string origin { get; set; }
    }
}