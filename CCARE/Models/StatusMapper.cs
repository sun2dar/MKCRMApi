using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class StatusMapper
    {
        [Key]
        public Guid MappingID { get; set; }

        public int? EntityType { get; set; }

        public string EntityName { get; set; }

        public int? StatusType { get; set; }

        public string StatusName { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}