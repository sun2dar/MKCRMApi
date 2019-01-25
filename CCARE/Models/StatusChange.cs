using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class StatusChange
    {

        public Guid MappingID { get; set; }

        public int? EntityType { get; set; }

        public string EntityName { get; set; }

        public int? StatusType { get; set; }

        public string StatusName { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        [Key]
        public Guid? StatusChangeID { get; set; }

        public string NewKey { get; set; }

        public string NewValue { get; set; }

        public int? StateCode { get; set; }

        public string StateLabel { get; set; }
    }
}