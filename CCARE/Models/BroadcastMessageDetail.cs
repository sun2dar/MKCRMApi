using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class BroadcastMessageDetail
    {
        public Guid? BroadcastMessageID { get; set; }

        [Key]
        public Guid BroadcastMessageDetailID { get; set; }

        public Guid? ToUserID { get; set; }

        [StringLength(160)]
        public string ToUserName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ToTeamID { get; set; }

        [StringLength(100)]
        public string ToTeamName { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [StringLength(2000)]
        public string Content { get; set; }

        public int MessageType { get; set; }

        [StringLength(1000)]
        public string MessageTypeLabel { get; set; }

        public int? Severity { get; set; }

        [StringLength(1000)]
        public string SeverityLabel { get; set; }

        public Guid? BusinessUnitID { get; set; }
    }
}