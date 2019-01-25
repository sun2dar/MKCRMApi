using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class Notes
    {
        [Key]
        public Guid AnnotationID { get; set; }

        public Guid ObjectID { get; set; }

        public int? ObjectTypeCode { get; set; }

        [StringLength(1000)]
        public string ObjectTypeLabel { get; set; }

        [StringLength(500)]
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        public string NoteText { get; set; }

        public int DeletionStateCode { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedByID { get; set; }

        [StringLength(160)]
        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid? ModifiedByID { get; set; }

        [StringLength(160)]
        public string ModifiedBy { get; set; }

        public Guid? BusinessUnitID { get; set; }
    }
}
      
        

        