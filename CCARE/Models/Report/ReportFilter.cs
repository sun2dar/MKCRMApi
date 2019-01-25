using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class ReportFilter
    {
        [Key]
        public Guid? ID { get; set; }

        public Guid ReportID { get; set; }
        [ForeignKey("ReportID")]
        public virtual Report report { get; set; }

        public Guid? EntityColumnID { get; set; }
        [ForeignKey("EntityColumnID")]
        public virtual EntityColumn entitycolumn { get; set; }

        public string ReportName { get; set; }

        public Guid? ValueID { get; set; }

        public int? ValueCode { get; set; }

        public string ValueText { get; set; }

        public DateTime? ValueStartDate { get; set; }

        public DateTime? ValueEndDate { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string DataType { get; set; }
        
        public string FieldType { get; set; }

        public bool IsFilter { get; set; }

        public bool IsEditable { get; set; }

        public int? SeqNo { get; set; }
    }
}