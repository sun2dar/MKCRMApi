using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class ActivityType
    {
        public string EntityName { get; set; }
        public string AttributeName { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<int> AttributeValue { get; set; }
        public string Label { get; set; }
        [Key]
        public System.Guid StringMapID { get; set; }
    }
}