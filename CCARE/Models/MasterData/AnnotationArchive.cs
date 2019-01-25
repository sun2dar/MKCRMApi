/* Model created by Ardi */
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
    public class AnnotationArchive
    {
        [Key]
        public Guid AnnotationID { get; set; }

        public Guid ObjectID { get; set; }

        public int? ObjectTypeCode { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        [StringLength(200)]
        public string MimeType { get; set; }

        public string DocumentBody { get; set; }

        public int? FileSize { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

    }
}