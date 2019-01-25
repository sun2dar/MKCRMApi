/* Created by Ardi */

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
    public class NonCustomer
    {
        [ForeignKey("ObjectID")]
        public virtual ICollection<Annotation> Notes { get; set; }

        [Key]
        public Guid NonCustomerID { get; set; }

        [StringLength(100)]
        public string Salutation { get; set; }

        [StringLength(200)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }

        [StringLength(100)]
        public string PhoneNo { get; set; }

        [StringLength(100)]
        public string HandphoneNo { get; set; }

        [StringLength(100)]
        public string FaxNo { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public int? AddressType { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(100)]
        public string Address3 { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Zipcode { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        [StringLength(160)]
        public string OwnerName { get; set; }

        public Guid? Owner { get; set; }

        public int StateCode { get; set; }

        [StringLength(1000)]
        public string StateLabel { get; set; }

        public int? StatusCode { get; set; }

        public int? DeletionStateCode { get; set; }

        public Guid? BusinessUnitID { get; set; }

    }
}