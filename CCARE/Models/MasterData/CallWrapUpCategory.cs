using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class CallWrapUpCategory
    {
        [Key]
        public Guid? CallWrapUpCategoryID { get; set; }

        [StringLength(200)]
        public string Description { get; set; } 

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
