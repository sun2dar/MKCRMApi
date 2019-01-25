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
    public class MarketingList
    {
        [Key]
        public Guid ID { get; set; }
        [StringLength(100)]
        public String Name { get; set; }
        [StringLength(100)]
        public String Purpose { get; set; }
        [StringLength(200)]
        public String Description { get; set; }
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