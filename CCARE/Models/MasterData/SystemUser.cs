/* Model created by Ardi */
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
    public class SystemUser
    {
        [Key]
        public Guid? SystemUserId { get; set; }

        [StringLength(200)]
        public string FirstName { get; set; }

        [StringLength(200)]
        public string DomainName { get; set; }

        [StringLength(200)]
        public string EmployeeID { get; set; }

        [StringLength(200)]
        public string FullName { get; set; }

        [StringLength(200)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string HomePhone { get; set; }

        [StringLength(200)]
        public string InternalEMailAddress { get; set; }

        [StringLength(200)]
        public string MobilePhone { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        public int DeletionStateCode { get; set; }

        public Guid? ActiveDirectoryGuid { get; set; }

        public bool IsActiveDirectoryUser { get; set; }

        public bool? IsDisabled { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? ModifiedBy { get; set; }

        public bool? DisplayInServiceViews { get; set; }

        [StringLength(200)]
        public string BranchLoginName { get; set; }

        public int? CCQId { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(200)]
        public string Site { get; set; }

        [StringLength(200)]
        public string TandemUserGroup { get; set; }

        [StringLength(200)]
        public string TandemUserNum { get; set; }

        public Guid? InternalAddressId { get; set; }

        [StringLength(200)]
        public string Fax { get; set; }

        [StringLength(200)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Phone2 { get; set; }

        [StringLength(200)]
        public string pager { get; set; }

        public Guid? SecurityRoleID { get; set; }

        [StringLength(200)]
        public string RoleName { get; set; }

        public Guid? OrganizationID { get; set; }

        public Guid? BusinessUnitID { get; set; }

        [StringLength(200)]
        public string BusinessUnitName { get; set; }

        public Guid? BranchID { get; set; }

        [StringLength(200)]
        public string BranchName { get; set; }

        public int? PreferredPhoneCode { get; set; }

        [StringLength(200)]
        public string PreferredPhone { get; set; }

        //public Guid? CRMUserID { get; set; }

        //public Guid? Config_UserID { get; set; }

        [StringLength(200)]
        public string AuthInfo { get; set; }

        public Guid? SystemUserRoleID { get; set; }

        //public Guid? DefaultOrganizationID { get; set; }
    }
}