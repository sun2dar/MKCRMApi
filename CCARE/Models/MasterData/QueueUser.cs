using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CCARE.Models
{
    public class QueueUser
    {
        [StringLength(160)]
        public string UserName { get; set; }

        [StringLength(160)]
        public string BusinessUnit { get; set; }

        [StringLength(200)]
        public string Queue { get; set; }

        
        public Guid? SystemUserID { get; set; }

        [Key]
        public Guid? QueueId { get; set; }

        public Guid? BusinessUnitId { get; set; }

    
    }
}