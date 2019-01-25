using CCARE.Models.General;
//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class DepositInfo
    {
        public string AccountType { get; set; }
        public string ATMCardType { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string RelationType { get; set; }
        public string OwnerType { get; set; }
        public string LastStatusKey { get; set; }
        public string LastStatus { get; set; }
        public string OpenDate { get; set; }

        public string WrongPinCounter { get; set; }
        public string UpdateByUserNumber { get; set; }
        public string UpdateByUserGroup { get; set; }
        public string UpdateDate { get; set; }
        public string LastTransactionDate { get; set; }

        public string BranchHome { get; set; }
        public string CloseDate { get; set; }

        //public string CustomerId { get; set; }
    }
}