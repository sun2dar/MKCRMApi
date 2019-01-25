using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class Entity
    {
        [Key]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}