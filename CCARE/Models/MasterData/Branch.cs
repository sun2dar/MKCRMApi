using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class Branch
    {
        [Key]
        public Guid? BranchID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Initials { get; set; }

        [StringLength(100)]
        public string OfficeCode { get; set; }

        //[StringLength(100)]
        //public string HeadOfMarketing { get; set; }

        [StringLength(100)]
        public string BranchClass { get; set; }

        public bool StatusForeignPerceptions { get; set; }

        [StringLength(100)]
        public string StatusForeignPerceptionsLabel { get; set; }

        public bool StatusPerceptions { get; set; }

        [StringLength(100)]
        public string StatusPerceptionsLabel { get; set; }

        [StringLength(400)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(40)]
        public string Zipcode { get; set; }

        [StringLength(100)]
        public string Region { get; set; }

        [StringLength(100)]
        public string RegionName { get; set; }  // Kanwil

        [StringLength(100)]
        public string Province { get; set; }

        [StringLength(100)]
        public string LongDistanceCode { get; set; }

        [StringLength(100)]
        public string Telephone1 { get; set; }

        [StringLength(100)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Telephone2 { get; set; }

        [StringLength(10)]
        public string VSAT { get; set; }

        public bool BCAPrioritas { get; set; }

        [StringLength(100)]
        public string BCAPrioritasLabel { get; set; }

        public bool BCABizz { get; set; }

        [StringLength(100)]
        public string BCABizzLabel { get; set; }

        [StringLength(200)]
        public string LeaderNamePrioritas { get; set; }

        [StringLength(40)]
        public string PostalCodePriority { get; set; }

        public bool CashOffice { get; set; }

        [StringLength(100)]
        public string CashOfficeLabel { get; set; }

        public bool Consumer { get; set; }

        public bool? SDB { get; set; }

        [StringLength(100)]
        public string ConsumerLabel { get; set; }

        [StringLength(400)]
        public string AddressPriority { get; set; }

        [StringLength(100)]
        public string FaxPrioritas { get; set; }

        [StringLength(100)]
        public string Telephone1Prioritas { get; set; }

        [StringLength(100)]
        public string Telephone2Prioritas { get; set; }

        [StringLength(200)]
        public string LeaderName { get; set; }

        [StringLength(200)]
        public string BankRepHead { get; set; }

        [StringLength(400)]
        public string RegionalAddressOffice { get; set; }

        [StringLength(150)]
        public string Notes { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(160)]
        public string StateLabel { get; set; }

        public int DeletionStateCode { get; set; }

        public int StateCode { get; set; }

        public int StatusCode { get; set; }
    }
}