//Created by Sumardi
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.MasterData
{
    public class Request_Workflow
    {
        [Key]
        public Guid ID { get; set; }
		public Guid? RequestID { get; set; }
		
		public Guid? WorkgroupID { get; set; }

        [StringLength(200)]
		public string WorkgroupName { get; set; }
		public Guid? ServiceLevelID { get; set; }
		public int? SLADays { get; set; }
		public int? SeqNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? AssignedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? OwnedDate { get; set; }

		public Guid? OwnedBy { get; set; }

        [StringLength(160)]
        public string OwnedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? DueDate { get; set; }

        public Boolean? IsOverdue { get; set; }
        public int? HandlingDays { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? HandledDate { get; set; }
		public int? ReopenNo { get; set; }
		public int? Status { get; set; }

        public int? RemainingSL { get; set; }
    }
}