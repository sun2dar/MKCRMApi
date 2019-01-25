using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCARE.Models
{
    public class CallWrapUpCustomer
    {
        [Key]
        public Guid? CallWrapUpID { get; set; }

        public Guid? CustomerID { get; set; }

        [StringLength(160)]
        public string CustomerName { get; set; }

        public Guid? NonCustomerID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CallEndTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CallStartTime { get; set; }

        public int Source { get; set; }

        [StringLength(11)]
        public string SourceDesc { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public Guid? CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public Guid? ModifiedBy { get; set; }

        public int DeletionStateCode { get; set; }

        public int StateCode { get; set; }

        public int StatusCode { get; set; }
    }
}