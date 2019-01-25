using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCARE.Models.SalesMarketing
{
    public class CampaignLead
    {
        [Key]
        public Guid ID { get; set; } 
        public int TargetType { get; set; }
        public Guid? EntityID { get; set; }
        public Guid? CampaignID { get; set; }
        [StringLength(100)]
        public String Name { get; set; }
        [StringLength(100)]
        public String Topic { get; set; }
        [StringLength(160)]
        public String Fullname { get; set; }
        [StringLength(100)]
        public String Email { get; set; }
        [StringLength(250)]
        public String Address { get; set; }
        [StringLength(50)]
        public String City { get; set; }
        [StringLength(50)]
        public String Country { get; set; }
        [StringLength(50)]
        public String Telephone { get; set; }
        [StringLength(500)]
        public String Handphone { get; set; }
        public int? DeletionStateCode { get; set; }
    }
}