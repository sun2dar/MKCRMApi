using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCARE.Models
{
    public class RequestResolutionArchive
    {
      
        [Key]
        public Guid ActivityId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public Guid? RequestID { get; set; }

        [StringLength(1000)]
        public string ActivityTypeLabel { get; set; }

        public int StateCode { get; set; }

        public int? StatusCode { get; set; }

        [StringLength(1000)]
        public string StatusCodeLabel { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]      
        public DateTime? ModifiedOn { get; set; }
        
        
        [StringLength(160)]
        public string CreatedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]      
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ArchivedOn { get; set; }

    }
}