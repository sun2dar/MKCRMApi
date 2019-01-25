using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class BroadcastMessage
    {
        [Key]
        public Guid BroadcastMessageID { get; set; }

        public Guid? TeamID { get; set; }

        public Guid? UserID { get; set; }

        [StringLength(4000)]
        public string Content { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public int? Severity { get; set; }

        public Guid? ModifiedBy { get; set; }

        public Guid? BusinessUnit { get; set; }
    }
}