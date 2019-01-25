using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class Team
    {
        public Guid TeamID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EMailAddress { get; set; }
        //[DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public string createdbyName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string FullName { get; set; }
        public Guid BusinessUnitID { get; set; }
        public string BusinessUnitName { get; set; }
        public int DeletionStateCode { get; set; }
        public Guid? OrganizationID { get; set; }
        public Guid? Parent_BusinessUnitID { get; set; }
    }
}