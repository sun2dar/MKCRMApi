using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class AllRelatedCreditCardInformation
    {
        public List<CreditCardInformation> RelatedCreditCards { get; private set; }

        public AllRelatedCreditCardInformation()
        {
            this.RelatedCreditCards = new List<CreditCardInformation>();
        }
    }

    public class CreditCardInformation
    {
        public string CustomerNo { get; set; }
        public string CreditCardNo { get; set; }
        public string CardType { get; set; }
        public string CardholderName { get; set; }
        public double? AvailableCredit { get; set; }
        public double? PrevBalance { get; set; }
        public double? SubTotal { get; set; }
    }
}