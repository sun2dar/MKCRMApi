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
    public class TaskArchive
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ArchivedOn { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
       
        [Key]
        public Guid? TaskID { get; set; }

        public Guid? PartyUserID { get; set; }

        [StringLength(160)]
        public string PartyUserName { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        [StringLength(100)]
        public string TaskNumber { get; set; }

        [StringLength(200)]
        public string Regarding { get; set; }

        public int? RequestStatusCode { get; set; }

        [StringLength(1000)]
        public string RequestStatusLabel { get; set; }

        public int TaskStatus { get; set; }

        [StringLength(100)]
        public string RequestTicketNumber { get; set; }

        [StringLength(1000)]
        public string TaskStatusLabel { get; set; }

        public int ActivityStatus { get; set; }

        [StringLength(1000)]
        public string ActivityStatusLabel { get; set; }

        public int Priority { get; set; }

        [StringLength(1000)]
        public string PriorityLabel { get; set; }

        public Guid? OwnerID { get; set; }

        [StringLength(160)]
        public string OwnerName { get; set; }

        public Guid? WorkGroupID { get; set; }

        [StringLength(200)]
        public string WorkGroupName { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public int DeletionStateCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid? BusinessUnitID { get; set; }

        public Guid? RequestID { get; set; }

        public Guid? Pointer_ActivityId { get; set; }
    }
}