//Created by Sumardi
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.MasterData
{
    public class Kota
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
       
        [StringLength(300)]
        public string Description { get; set; }

    }
}
