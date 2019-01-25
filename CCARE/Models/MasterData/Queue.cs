using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CCARE.Models
{
    public class Queue
    {

        [Key]
        public Guid QueueId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public Guid? BusinessUnitID { get; set; }

        [StringLength(160)]
        public string BusinessUnitName { get; set; }

        public Guid? BusinessUnitID_ORG { get; set; }

        [StringLength(160)]
        public string BusinessUnitName_ORG { get; set; }

        [StringLength(100)]
        public string EMailAddress { get; set; }

        public Guid? OwnerId { get; set; }

        [StringLength(160)]
        public string OwnerName { get; set; }

        public string Description { get; set; }

        public int? QueueTypeCode { get; set; }

        [StringLength(255)]
        public string QueueTypeLabel { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? DeletionStateCode { get; set; }
    }
}