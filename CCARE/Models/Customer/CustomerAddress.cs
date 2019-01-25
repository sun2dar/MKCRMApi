using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class CustomerAddress
    {
        [Key]
        public Guid CustomerAddressId { get; set; }

        public Guid ParentId { get; set; }
        
        public int? AddressNumber { get; set; }
       
        public int? ObjectTypeCode { get; set; }
        
        public int? AddressTypeCode { get; set; }
        
        public string AddressTypeLabel { get; set; }

        [StringLength(250)]
        public string Line1 { get; set; }

        [StringLength(250)]
        public string Line2 { get; set; }

        [StringLength(250)]
        public string Line3 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string StateOrProvince { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string Telephone1 { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ModifiedOn { get; set; }

        public int? DeletionStateCode { get; set; }

        [StringLength(16)]
        public string CreditCardCustomerNo { get; set; }
    }
}