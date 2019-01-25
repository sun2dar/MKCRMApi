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
    public class LetterTemplate
    {
        [Key]
        public Guid LetterTemplateId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int? Type { get; set; }

        [StringLength(200)]
        public string TypeLabel { get; set; }

        public string AutoID { get; set; }

        public int? Language { get; set; }

        [StringLength(200)]
        public string LanguageLabel { get; set; }

        [StringLength(200)]
        public string OwnerName { get; set; }

        public Guid? OwnerID { get; set; }

        [StringLength(200)]
        public string Description1 { get; set; }

        [StringLength(200)]
        public string Description2 { get; set; }

        [StringLength(200)]
        public string Description3 { get; set; }

        [StringLength(200)]
        public string Description4 { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(200)]
        public string CreatedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(200)]
        public string ModifiedByName { get; set; }

        public Guid? OwningBusinessUnit { get; set; }

        public int StateCode { get; set; }

        [StringLength(200)]
        public string StateLabel { get; set; }

        public int? StatusCode { get; set; }

        public int? DeletionStateCode { get; set; }
    }
}