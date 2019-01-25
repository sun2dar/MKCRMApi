using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//Created by SUN 25 Sep 2015
namespace CCARE.Models
{
    public class StatusMapperNewCode
    {
        [Key]
        public Guid MappingNewCodeID { get; set; } 
		public int EntityType { get; set; }
		public string EntityName { get; set; }
		public int StatusType { get; set; }
		public string StatusName { get; set; }
        public string Key { get; set; }
		public string Value { get; set; }
        public string NewKey { get; set; }
		public string NewValue { get; set; }
    }
}