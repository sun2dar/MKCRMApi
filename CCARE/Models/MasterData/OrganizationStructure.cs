using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class OrganizationStructure
    {
        [Key]
        public Guid OrganizationStructureID { get; set; }

        public Guid? QueueID { get; set; }

        [StringLength(200)]
        public string Queue { get; set; }

        public Guid BusinessUnitID { get; set; }

        [StringLength(160)]
        public string BusinessUnit { get; set; }

        public int DeletionStateCode { get; set; }

    }
}