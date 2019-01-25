using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
	public class Channel
	{
        [Key]
        public Guid? Id { get; set; }
		public Guid CustomerId { get; set; }
        public string CustomerIdName { get; set; }
        public int? ChannelTypeId { get; set; }
        public string ChannelType { get; set; }
        public string CardNo { get; set; }
		public string UserId { get; set; }
		public string EmailAddress { get; set; }
		public string HpNo { get; set; }
		public string CorpId { get; set; }
		public string AccountNo { get; set; }
		public string CustomerNo { get; set; }
		public string SNKeyBCA { get; set; }
		public string Category { get; set; }
	}
}