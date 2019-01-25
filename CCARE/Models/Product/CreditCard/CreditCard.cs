//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class CreditCard
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerIdName { get; set; }

        public string CreditCardNumber { get; set; }

        public string CardholderName { get; set; }

        public string CardType { get; set; }

        public string CCType { get; set; }

        public string CreditCardCustomerNo { get; set; }

        public string Status { get; set; }
    }
}