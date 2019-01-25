//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class Loan
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerIdName { get; set; }

        public string AccountNo { get; set; }

        public string LoanNumber { get; set; }

        //public Guid? LoanTypeId { get; set; }

        public string LoanTypeIdCode { get; set; }

        public string LoanTypeIdName { get; set; }
    }
}