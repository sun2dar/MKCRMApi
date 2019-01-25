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
    public class MarketingListPurpose
    {
        [Key]
        public Guid ID { get; set; }
        [StringLength(100)]
        public String Name { get; set; }
        [StringLength(200)]
        public String Description { get; set; }
        public int? DeletionStateCode { get; set; }
    }
}