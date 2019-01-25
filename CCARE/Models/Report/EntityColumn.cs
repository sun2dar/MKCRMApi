using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class EntityColumn
    {
        [Key]
        public Guid ID { get; set; }

        public Guid EntityID { get; set; }
        [ForeignKey("ID")]
        public virtual Entity Entity { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string DataType { get; set; }

        public bool IsFilter { get; set; }

        public string FieldType { get; set; }

        public bool IsColumn { get; set; }
    }
}