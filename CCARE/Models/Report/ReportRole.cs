using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCARE.Models.Role;

namespace CCARE.Models
{
    public class ReportRole
    {
        [Key]
        public Guid? ReportRoleID { get; set; }

        public Guid? ReportID { get; set; }
        [ForeignKey("ReportID")]
        public virtual MasterReport masterReport { get; set; }
        
        public Guid? SecurityRoleId { get; set; }
        [ForeignKey("SecurityRoleId")]
        public virtual SecurityRole securityRole{ get; set; }

        public DateTime? CreatedOn { get; set; }
        
        public Guid? CreatedBy { get; set; }
        
        public DateTime? ModifiedOn { get; set; }
        
        public Guid? ModifiedBy { get; set; }
    }
}