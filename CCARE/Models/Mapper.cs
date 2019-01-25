using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class Mapper
    {
        [Key]
        public Guid? MapperId { get; set; }

        public string ObjectName { get; set; }

        public string AttributeName { get; set; }

        public int? DisplayOrder { get; set; }

        public string EnumValue { get; set; }

        public string EnumLabel { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public string StateLabel { get; set; }

        public int? StateCode { get; set; }
    }
}