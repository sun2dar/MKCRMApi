using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CCARE.Models
{
    public class RequestQueue
    {
        public int QueueTypeCode { get; set; }
        
        [StringLength(1000)]
        public string QueueType { get; set; }
    
        [StringLength(200)]
        public string QueueName { get; set; }

        public Guid? QueueOwnerID { get; set; }

        public int ObjectTypeCode { get; set; }

        [StringLength(1000)]
        public string ObjectType { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public DateTime EnteredOn { get; set; }
        
        [Key]
        public Guid? QueueItemId { get; set; }

        public Guid? QueueId { get; set; }

        public Guid? ObjectId { get; set; }

        public int Priority { get; set; }

        public int State { get; set; }

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        //[StringLength(160)]
        //public int CreatedByName { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        //[StringLength(160)]
        //public int ModifiedByName { get; set; }

        public Guid? OrganizationId { get; set; }

        public int DeletionStateCode { get; set; }







    }
}