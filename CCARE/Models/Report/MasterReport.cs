using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class MasterReport
    {
        [Key]
        public Guid? ReportID { get; set; }

        public string ReportName { get; set; }

        public string Description { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public string RdlName { get; set; }
    }
}