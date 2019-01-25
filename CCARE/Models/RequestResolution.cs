using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCARE.Models
{
    public class RequestResolution
    {
        public Guid CreatedBy { get; set; }

        [Key]
        public Guid ActivityId { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int DeletionStateCode { get; set; }

        public string Description { get; set; }

        public Guid RequestID { get; set; }

        public string RequestTitle { get; set; }

        public int ObjectTypeCode { get; set; }

        public int ActivityTypeCode { get; set; }

        public string ActivityTypeLabel { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid BusinessUnitID { get; set; }
        
        public Guid OwnerID { get; set; }

        public int StatusCode { get; set; }

        public string StatusCodeLabel { get; set; }

        public string Subject { get; set; }
    }
}