using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCARE.Models.Role;

namespace CCARE.Models
{
    public class ReportRoleNew
    {
        [Key]
        public Guid? ID { get; set; }

        public Guid? ReportID { get; set; }
        [ForeignKey("ReportID")]
        public virtual Report report { get; set; }

        public Guid? SecurityRoleId { get; set; }
        [ForeignKey("SecurityRoleId")]
        public virtual SecurityRole securityRole { get; set; }
    }
}