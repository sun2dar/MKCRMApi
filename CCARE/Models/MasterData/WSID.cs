using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class WSID
    {
        [Key]
        public Guid? WSIDID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string Camera { get; set; }

        [StringLength(200)]
        public string City { get; set; }

        [StringLength(200)]
        public string Coordinator { get; set; }

        [StringLength(200)]
        public string DescKU { get; set; }

        [StringLength(200)]
        public string DescKW { get; set; }

        public DateTime? InstallDate { get; set; }

        [StringLength(200)]
        public string Island { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(200)]
        public string Lok { get; set; }

        public int CreatedByDSC { get; set; }

        public int ModifiedByDSC { get; set; }

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
