using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CCARE.Models.SalesMarketing
{
    public class Leadlama
    {
        [Key]
        public Guid ID { get; set; } 
        [StringLength(100)]
        public String Topic { get; set; }
        [StringLength(100)]
        public String Name { get; set; }
        [StringLength(100)]
        public String Company { get; set; }
        [StringLength(50)]
        public String JobTitle { get; set; }
        [StringLength(100)]
        public String Firstname { get; set; }
        [StringLength(60)]
        public String Lastname { get; set; }
        [StringLength(160)]
        public String Fullname { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }

        public int? GenderCode { get; set; }
        [StringLength(1000)]
        public String GenderLabel { get; set; }

        [StringLength(100)]
        public String Email { get; set; }
        [StringLength(200)]
        public String Address { get; set; }
        [StringLength(50)]
        public String City { get; set; }
        [StringLength(50)]
        public String Country { get; set; }
        [StringLength(50)]
        public String Telephone { get; set; }
        [StringLength(50)]
        public String Handphone { get; set; }
        public Guid? CreatedBy { get; set; }
        [StringLength(160)]
        public String CreatedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreatedOn { get; set; }
        
        public Guid? ModifiedBy { get; set; }
        [StringLength(160)]
        public String ModifiedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ModifiedOn { get; set; }

        public int? DeletionStateCode { get; set; }
        public int? StateCode { get; set; }
        [StringLength(100)]
        public String StateLabel { get; set; }
        public int? StatusCode { get; set; }
    }
}