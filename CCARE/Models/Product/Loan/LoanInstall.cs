//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class LoanInstall
    {
        public DateTime? InstallmentDate { get; set; }
 
        public double? UnpaidInstallmentAmount { get; set; }
    }
}