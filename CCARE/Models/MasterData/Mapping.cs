using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CCARE.Models
{
    public class Mapping  // Model for menu Codes And Mappers including Currency Code, Jenis Transaksi, Loan Types, etc
    {
        [Key]
        public Guid? MappingID { get; set; }

        public int CategoryCode { get; set; }

        [StringLength(200)]
        public string CategoryName { get; set; }

        public int ObjectCode { get; set; }

        [StringLength(200)]
        public string ObjectName { get; set; }

        public int AttributeCode { get; set; }

        [StringLength(200)]
        public string AttributeName { get; set; }

        [StringLength(200)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Label { get; set; }

        [StringLength(200)]
        public string NewCodeList { get; set; }

        public int DisplayOrder { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int StateCode { get; set; }

        [StringLength(160)]
        public string StateLabel { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

    }

}
