//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class Deposit
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerIdName { get; set; }

        public string AccountNo { get; set; }

        public string AccountTypeIdCode { get; set; }

        public string AccountTypeIdName { get; set; }

        public string CardNo { get; set; }

        public string CardTypeIdName { get; set; }

        //public int? OwnerType { get; set; }

        public string OwnerTypeLabel { get; set; }
            
        public string RelationType { get; set; }

        public string RelationshipWith { get; set; }
    }
}