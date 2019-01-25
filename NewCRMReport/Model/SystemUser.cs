//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewCRMReport.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SystemUser
    {
        public System.Guid SystemUserID { get; set; }
        public string FirstName { get; set; }
        public string DomainName { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string HomePhone { get; set; }
        public string InternalEMailAddress { get; set; }
        public string MobilePhone { get; set; }
        public string Title { get; set; }
        public int DeletionStateCode { get; set; }
        public Nullable<System.Guid> ActiveDirectoryGuid { get; set; }
        public bool IsActiveDirectoryUser { get; set; }
        public Nullable<bool> IsDisabled { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<bool> DisplayInServiceViews { get; set; }
        public string BranchLoginName { get; set; }
        public Nullable<int> CCQId { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Site { get; set; }
        public string TandemUserGroup { get; set; }
        public string TandemUserNum { get; set; }
        public Nullable<System.Guid> InternalAddressId { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string pager { get; set; }
        public Nullable<System.Guid> OrganizationID { get; set; }
        public System.Guid BusinessUnitID { get; set; }
        public string BusinessUnitName { get; set; }
        public Nullable<System.Guid> BranchID { get; set; }
        public string BranchName { get; set; }
        public Nullable<int> PreferredPhoneCode { get; set; }
        public string PreferredPhone { get; set; }
        public Nullable<System.Guid> CRMUserID { get; set; }
        public Nullable<System.Guid> Config_UserID { get; set; }
        public string AuthInfo { get; set; }
        public Nullable<System.Guid> DefaultOrganizationID { get; set; }
        public Nullable<System.Guid> SecurityRoleID { get; set; }
        public string RoleName { get; set; }
        public Nullable<System.Guid> SystemUserRoleID { get; set; }
    }
}
