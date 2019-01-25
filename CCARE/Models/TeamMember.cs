using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class TeamMember
    {
        public Guid? TeamId { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string UserStatus { get; set; }
        public Guid? CreatedBy { get; set; }
        public string createdbyName { get; set; }
        public Guid? SystemUserId { get; set; }
        public string SystemUserName { get; set; }
        [Key]
        public Guid TeamMembershipID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string modifiedbyName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string EmailAddress { get; set; }
        public Guid? BusinessUnitID { get; set; }
        public string BusinessUnitName { get; set; }
        public int DeletionStateCode { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}