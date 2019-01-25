using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class RequestAuditLog
    {
        [Key]
        public Guid AuditLogID { get; set; }

        [StringLength(200)]
        public string EntityIdName { get; set; }

        public int? ActionType { get; set; }

        [StringLength(255)]
        public string ActionTypeLabel { get; set; }

        public string auditdata { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime CreatedOn { get; set; }

        public int? DeletionStateCode { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public Guid? RequestID { get; set; }
    }
}