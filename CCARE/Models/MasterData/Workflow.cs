//Created by Sumardi
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.MasterData
{
    public class Workflow
    {
        [Key]
        public Guid ID { get; set; }

		public Guid? SLID { get; set; }
		public Guid? WgID { get; set; }

        [StringLength(200)]
		public string WgName { get; set; }

        public int? WorkflowSLADays { get; set; }
		public int? SeqNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? CreatedOn { get; set; }

		public Guid? CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? ModifiedOn { get; set; }

		public Guid? ModifiedBy { get; set; }

        [StringLength(500)]
        public string Keterangan { get; set; }
    }
}