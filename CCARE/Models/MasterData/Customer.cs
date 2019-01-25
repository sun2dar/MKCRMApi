//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCARE.Models.MasterData
{
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; }

        [StringLength(50)]
        public string CISNumber { get; set; }

        [StringLength(50)]
        public string CreditCardCustomerNo { get; set; }

        public int? AccountCode { get; set; }

        public string AccountLabel { get; set; }

        [StringLength(100)]
        public string Salutation { get; set; }

        public int? GenderCode { get; set; }

        public string GenderLabel { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(160)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string AliasName { get; set; }

        [StringLength(100)]
        public string BirthPlace { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }

        [StringLength(100)]
        public string MothersMaidenName { get; set; }

        [StringLength(100)]
        public string IdentityNo { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [StringLength(50)]
        public string Telephone1 { get; set; }

        [StringLength(50)]
        public string Telephone2 { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string EMailAddress1 { get; set; }

        public int? AddressTypeCode { get; set; }

        public string AddressTypeLabel { get; set; }

        [StringLength(50)]
        public string City { get; set; }
        
        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(250)]
        public string Line1 { get; set; }

        [StringLength(250)]
        public string Line2 { get; set; }
        
        [StringLength(250)]
        public string Line3 { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string StateOrProvince { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? LastRefreshedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int StateCode { get; set; }

        public string StateLabel { get; set; }

        public int StatusCode { get; set; }

        public int DeletionStateCode { get; set; }

        //[ForeignKey("ParentId")]
        //public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
    }

}