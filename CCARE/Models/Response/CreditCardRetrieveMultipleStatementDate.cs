using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class CreditCardRetrieveMultipleStatementDate
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public double? MinimumPayment { get; set; }
        public List<CreditCardStatementInformation> Statements { get; set; }

        public CreditCardRetrieveMultipleStatementDate()
        {
            this.Statements = new List<CreditCardStatementInformation>();
        }
    }

    public class CreditCardStatementInformation
    {
        public DateTime? StatementDate { get; set; }
        public string StatementDateInJulianFormat { get; set; }
    }
}