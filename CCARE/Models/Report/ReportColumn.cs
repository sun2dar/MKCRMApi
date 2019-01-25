using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class ReportColumn
    {
        [Key]
        public Guid? ID { get; set; }

        public Guid ReportID { get; set; }
        [ForeignKey("ReportID")]
        public virtual Report report { get; set; }

        public int? SeqNo { get; set; }

        public Guid? EntityColumnID { get; set; }
        [ForeignKey("EntityColumnID")]
        public virtual EntityColumn entitycolumn { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        public string Name { get; set; }
    }
}