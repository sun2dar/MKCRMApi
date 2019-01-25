using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class Report
    {
        [Key]
        public Guid? ID { get; set; }

        public Guid? EntityID { get; set; }
        [ForeignKey("ID")]
        public virtual Entity Entity { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }

        public bool? Hyperlink  { get; set; }
    }
}