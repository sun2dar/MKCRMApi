using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class Activities
    {
        [Key]
        public System.Guid ActivityID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> RegardingContactID { get; set; }
        public string FullName { get; set; }
        public Nullable<System.Guid> RegardingObjectID { get; set; }
        public Nullable<int> ObjectTypeCode { get; set; }
        public string ObjectTypeName { get; set; }
        public int ActivityTypeCode { get; set; }
        public string ActivityTypeName { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> ScheduledStart { get; set; }
        public Nullable<System.DateTime> ScheduledEnd { get; set; }
        public Nullable<System.DateTime> ActualEnd { get; set; }
        public string Remark { get; set; }
        public Nullable<System.Guid> OwnerID { get; set; }
        public Nullable<System.Guid> BusinessUnitID { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public int StateCode { get; set; }
        public string StateLabel { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public int DeletionStateCode { get; set; }
    }
}