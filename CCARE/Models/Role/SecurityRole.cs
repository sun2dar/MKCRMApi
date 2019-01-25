using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCARE.Models.Role
{
    public class SecurityRole
    {
        [Key]
        public Guid SecurityRoleId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string XMLRole { get; set; }

        //0 = Active
        //1 = Inactive
        public int? Status { get; set; }

        //0 = Active
        //2 = Delete
        public int? DeletionStateCode { get; set; }

        public Guid? BusinessUnitId { get; set; }
        [ForeignKey("BusinessUnitId")]
        public virtual BusinessUnit businessUnit { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? FileModifiedDate { get; set; }
    }

    public class RetrieveRole
    {
        public SecurityRole role { get; set; }
        public Root xmlRole { get; set; }
    }
}