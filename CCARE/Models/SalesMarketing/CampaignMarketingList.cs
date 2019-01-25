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
    public class CampaignMarketingList
    {
        [Key]
        public Guid ID { get; set; }
        public int TargetType { get; set; }
        public Guid? EntityID { get; set; }
        public Guid? CampaignID { get; set; }
        [StringLength(100)]
        public String Name { get; set; }
        [StringLength(100)]
        public String Purpose { get; set; }
        [StringLength(200)]
        public String Description { get; set; }
        public int? DeletionStateCode { get; set; }
    }
}