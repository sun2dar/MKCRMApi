using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
	public class ApplicationKeyBCAIBInfo
	{
		private string status { get; set; }
		private string tokenType { get; set; }
		public string StatusKey { get; set; }
		public string TokenTypeKey { get; set; }
		public string BranchIssuingToken { get; set; }
		public string TokenType { get; set; }
		public string UpdatedBy { get; set; }
		public string ActivatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
		public string UserIdClickBCA { get; set; }
		public string AtmCardNo { get; set; }
		public string CustomerName { get; set; }
		public string ApplicationName { get; set; }
		public string ApplicationNameKey { get; set; }
        public DateTime? RequestDate { get; set; }
		public string Action { get; set; }
		public string Status { get; set; }
		public string RejectionReason { get; set; }
        public DateTime? ConnectionOrDeletedDate { get; set; }
		public string ConnectionType { get; set; }
		public string ConnectionTypeKey { get; set; }
		public string Id1 { get; set; }
		public string Id2 { get; set; }
	}
}