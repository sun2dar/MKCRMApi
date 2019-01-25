using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCARE.Models.MasterData;

namespace CCARE.Models
{
    public class ServiceLevel
    {
        [Key]
        public Guid? ServiceLevelID { get; set; }
        
        [StringLength(200)]
        public string Name { get; set; }

        public Guid? CategoryID { get; set; }

        [StringLength(200)]
        public string CategoryIDName { get; set; }

        public Guid? ProductID { get; set; }

        [StringLength(200)]
        public string ProductIDName { get; set; }
        
        public int SLADays { get; set; }

        public Guid? WorkgroupID { get; set; }

        [StringLength(200)]
        public string WorkgroupIDName { get; set; }
        
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

        public Guid? KotaID { get; set; }
        [StringLength(100)]
        public string KotaName { get; set; }

        public Guid? ParentID { get; set; }
        [StringLength(255)]
        public string ParentName { get; set; }

        public Guid? SegmentationID { get; set; }
        [StringLength(200)]
        public string SegmentationName { get; set; }
        [StringLength(500)]
        public string SegmentationDesc { get; set; }
    }
}