//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class LoanInfo
    {
        public string AccountNumber { get; set; }

        public string LoanNumber { get; set; }
        
        public string LoanTerm { get; set; }
        
        public string Currency { get; set; }
        
        public string RemainingTerm { get; set; }
        
        public string LoanType { get; set; }
        
        public double? InterestRate { get; set; }
        
        public double? OriginalPrincipal { get; set; }
        
        public double? OutstandingPrincipal { get; set; }
        
        public double? TotalUnpaidInstallment { get; set; }
        
        public string OpenDate { get; set; }
        
        public string DueDate { get; set; }
        
        public DateTime? NextInterestRateChangeDate { get; set; }

        public List<LoanInstall> Installments { get; set; }

        public LoanInfo()
        {
            Installments = new List<LoanInstall>();
        }
    }
}