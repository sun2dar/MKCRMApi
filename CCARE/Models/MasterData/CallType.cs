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
    public class CallType
    {
        [Key]
        public Guid? CallTypeID { get; set; }

        public Guid? CallWrapUpID { get; set; }

        public Guid? CallWrapUpCategoryID { get; set; }

        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(250)]
        public string Summary { get; set; }

        public int Source { get; set; }

        public Guid? CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        public int DeletionStateCode { get; set; }

        public Guid? OwnerID { get; set; }

        public Guid? ModifiedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? BusinessUnitID { get; set; }

        public int statecode { get; set; }

        public int statuscode { get; set; }



    }
}