using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class BusinessUnit
    {
        [Key]
        public Guid? BusinessUnitId { get; set; }

        [StringLength(160)]
        public string Name { get; set; }
        
        [StringLength(100)]
        public string Division { get; set; }

        [StringLength(160)]
        public string ParentBusinessUnit { get; set; }

        public Guid? ParentBusinessUnitId { get; set; }

        [StringLength(200)]
        public string WebSite { get; set; }

        [StringLength(50)]
        public string MainPhone { get; set; }
        
        [StringLength(50)]
        public string OtherPhone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string eMail { get; set; }

        [StringLength(250)]
        public string Bill_Street1 { get; set; }

        [StringLength(250)]
        public string Bill_Street2 { get; set; }

        [StringLength(250)]
        public string Bill_Street3 { get; set; }

        [StringLength(50)]
        public string Bill_City { get; set; }

        [StringLength(50)]
        public string Bill_StateOrProvince { get; set; }

        [StringLength(20)]
        public string Bill_PostalCode { get; set; }

        [StringLength(50)]
        public string Bill_Country { get; set; }

        [StringLength(250)]
        public string Ship_Street1 { get; set; }

        [StringLength(250)]
        public string Ship_Street2 { get; set; }

        [StringLength(250)]
        public string Ship_Street3 { get; set; }

        [StringLength(50)]
        public string Ship_City { get; set; }

        [StringLength(50)]
        public string Ship_StateOrProvince { get; set; }

        [StringLength(20)]
        public string Ship_PostalCode { get; set; }

        [StringLength(50)]
        public string Ship_Country { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(160)]
        public string CreatedBy { get; set; }

        public Guid? CreatedByID { get; set; }

        public DateTime ModifiedOn { get; set; }

        [StringLength(160)]
        public string ModifiedBy { get; set; }

        public Guid? ModifiedByID { get; set; }

        public int DeletionStateCode { get; set; }

        public Boolean? IsDisabled { get; set; }
                
    }
}