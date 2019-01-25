using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCARE.Models.Role;

namespace CCARE.Models
{
    public class ReportFilters
    {
        [Key]
        public Guid? ReportFiltersID { get; set; }

        public Guid ReportID { get; set; }
        [ForeignKey("ReportID")]
        public virtual MasterReport masterReport { get; set; }

        public string FilterType { get; set; }

        public Guid? FilterGUID { get; set; }

        public string FilterValue { get; set; }

        public int IsEditable { get; set; }
    }

    public class FilterValues {
        public string key { get; set; }
        public string gid { get; set; }
        public string val { get; set; }
    }
}