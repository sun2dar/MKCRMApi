using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Web.Mvc;

//Picklist

namespace CCARE.Models
{
    public class StringMap
    {
        [Key]
        public Guid StringMapID { get; set; }

        [StringLength(64)]
        public string EntityName { get; set; }

        [StringLength(100)]
        public string AttributeName { get; set; }

        public int AttributeValue { get; set; }

        [StringLength(255)]
        public string label { get; set; }

        public int DisplayOrder { get; set; }
    }
}