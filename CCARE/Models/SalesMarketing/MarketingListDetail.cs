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
    public class MarketingListDetail
    {
        [Key]
        public Guid ID { get; set; }
        [StringLength(100)]
        public int? Type { get; set; }
        public Guid? EntityID { get; set; }
        public Guid? MarketingListID { get; set; }
    }
}