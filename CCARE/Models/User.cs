using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Web.Mvc;

namespace CCARE.Models
{
    public class User
    {
        [Key]
        public Guid SystemUserId { get; set; }

        [StringLength(160)]
        public string FullName { get; set; }

        public Guid? BusinessUnitID { get; set; }

        [StringLength(160)]
        public string BusinessUnitName { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(40)]
        public string Site { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string InternalEMailAddress { get; set; }
        
    }
}