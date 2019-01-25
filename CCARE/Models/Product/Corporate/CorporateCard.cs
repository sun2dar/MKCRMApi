using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class CorporateCard
    {
        public string CardNumber { get; set; }
        public string CompanyName { get; set; }
        public string CorporateId { get; set; }
        public string AccountNumber { get; set; }
        public string CardType { get; set; }
        public string EmbossName { get; set; }
        public Double? Limit { get; set; }
        public DateTime? LimitExpiredDate { get; set; }
        public string CardHolder { get; set; }
        public string Phone { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string UniqueKey { get; set; }
        public string Status { get; set; }
        public string StatusLabel { get; set; }
        public string RecurringType { get; set; }
        public string RecurringPeriod { get; set; }
    }

    public class Corporate {
        public int Id { get; set; }
        public string CorporateId { get; set; }
        public string CorporateName { get; set; }
        public string Status { get; set; }
    }
}