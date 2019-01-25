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
    public class Annotation
    {
        [Key]
        public Guid AnnotationID { get; set; }

        public Guid ObjectID { get; set; }

        public int? ObjectTypeCode { get; set; }

        [StringLength(200)]
        public string ObjectTypeLabel { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        public string NoteText { get; set; }

        public bool IsDocument { get; set; }

        [StringLength(200)]
        public string MimeType { get; set; }

        [StringLength(200)]
        public string LangID { get; set; }

        public string DocumentBody { get; set; }

        public int? FileSize { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        public bool IsPrivate { get; set; }

        public int DeletionStateCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedByID { get; set; }

        [StringLength(200)]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedByID { get; set; }

        [StringLength(200)]
        public string ModifiedBy { get; set; }

        public Guid? BusinessUnitID { get; set; }
    }
}