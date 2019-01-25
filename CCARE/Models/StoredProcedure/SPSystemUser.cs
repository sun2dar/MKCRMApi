using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {

        public KeyValuePair<int, String> SystemUser_Insert(SystemUser model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.SystemUserId) },
                new SqlParameter("@ActiveDirectoryGuID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ActiveDirectoryGuid) },
                new SqlParameter("@AuthInfo", SqlDbType.NVarChar, 128) {Value = CheckForDbNull(model.AuthInfo) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = model.BusinessUnitID },
                new SqlParameter("@DomainName", CheckForDbNull(model.DomainName)) ,
                new SqlParameter("@EmployeeID", CheckForDbNull(model.EmployeeID)),
                new SqlParameter("@FirstName", CheckForDbNull(model.FirstName)),
                new SqlParameter("@LastName", CheckForDbNull(model.LastName)),
                new SqlParameter("@HomePhone", CheckForDbNull(model.HomePhone)),
                new SqlParameter("@InternalEMailAddress", CheckForDbNull(model.InternalEMailAddress)),
                new SqlParameter("@MobilePhone", CheckForDbNull(model.HomePhone)),
                new SqlParameter("@PreferredPhoneCode", SqlDbType.Int) { Value = CheckForDbNull(model.PreferredPhoneCode) },
                new SqlParameter("@Title", CheckForDbNull(model.Title)),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@branchID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BranchID) },
                new SqlParameter("@BranchLoginName", CheckForDbNull(model.BranchLoginName)),
                new SqlParameter("@Department", CheckForDbNull(model.Department)),
                new SqlParameter("@Location", CheckForDbNull(model.Location)),
                new SqlParameter("@Site", CheckForDbNull(model.Site)),
                new SqlParameter("@TandemUserGroup", CheckForDbNull(model.TandemUserGroup)),
                new SqlParameter("@TandemUserNum", CheckForDbNull(model.TandemUserNum)),
                new SqlParameter("@Fax", CheckForDbNull(model.Fax)),
                new SqlParameter("@Phone", CheckForDbNull(model.Phone)),
                new SqlParameter("@Phone2", CheckForDbNull(model.Phone2)),
                new SqlParameter("@pager", CheckForDbNull(model.pager)),
                new SqlParameter("@SecurityRoleID", SqlDbType.UniqueIdentifier ) { Value = CheckForDbNull(model.SecurityRoleID) },
                new SqlParameter("@InternalAddressID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.InternalAddressId) },

                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
                
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[SystemUser_Insert] @UserID ,@ActiveDirectoryGuID ,@AuthInfo ,@BusinessUnitID ,@DomainName ,@EmployeeID ,@FirstName ,@LastName ,@HomePhone ,@InternalEMailAddress ,@MobilePhone ,@PreferredPhoneCode ,@Title ,@ModifiedBy ,@branchID ,@BranchLoginName ,@Department ,@Location ,@Site ,@TandemUserGroup ,@TandemUserNum ,@Fax ,@Phone ,@Phone2 ,@pager ,@SecurityRoleID ,@InternalAddressID ,@Message OUTPUT ,@RetVal OUTPUT", param);
            int retVal = (int)param[28].Value;
            string valueRes = param[27].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> SystemUser_Update(SystemUser model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.SystemUserId) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BusinessUnitID) },
                new SqlParameter("@DomainName", CheckForDbNull(model.DomainName)) ,
                new SqlParameter("@EmployeeID", CheckForDbNull(model.EmployeeID)),
                new SqlParameter("@FirstName", CheckForDbNull(model.FirstName)),
                new SqlParameter("@LastName", CheckForDbNull(model.LastName)),
                new SqlParameter("@HomePhone", CheckForDbNull(model.HomePhone)),
                new SqlParameter("@InternalEMailAddress", CheckForDbNull(model.InternalEMailAddress)),
                new SqlParameter("@MobilePhone", CheckForDbNull(model.HomePhone)),
                new SqlParameter("@PreferredPhoneCode", SqlDbType.Int) { Value = CheckForDbNull(model.PreferredPhoneCode) },
                new SqlParameter("@Title", CheckForDbNull(model.Title)),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@branchID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BranchID) },
                new SqlParameter("@BranchLoginName", CheckForDbNull(model.BranchLoginName)),
                new SqlParameter("@Department", CheckForDbNull(model.Department)),
                new SqlParameter("@Location", CheckForDbNull(model.Location)),
                new SqlParameter("@Site", CheckForDbNull(model.Site)),
                new SqlParameter("@TandemUserGroup", CheckForDbNull(model.TandemUserGroup)),
                new SqlParameter("@TandemUserNum", CheckForDbNull(model.TandemUserNum)),
                new SqlParameter("@Fax", CheckForDbNull(model.Fax)),
                new SqlParameter("@Phone", CheckForDbNull(model.Phone)),
                new SqlParameter("@Phone2", CheckForDbNull(model.Phone2)),
                new SqlParameter("@Pager", CheckForDbNull(model.pager)),
                new SqlParameter("@RoleID", SqlDbType.UniqueIdentifier ) { Value = CheckForDbNull(model.SecurityRoleID) },
                new SqlParameter("@SystemUserRoleID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(null) },
                new SqlParameter("@InternalAddressID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.InternalAddressId) },

                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
                
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[SystemUser_Update] @UserID ,@BusinessUnitID ,@DomainName ,@EmployeeID ,@FirstName ,@LastName ,@HomePhone ,@InternalEMailAddress ,@MobilePhone ,@PreferredPhoneCode ,@Title ,@ModifiedBy ,@branchID ,@BranchLoginName ,@Department ,@Location ,@Site ,@TandemUserGroup ,@TandemUserNum ,@Fax ,@Phone ,@Phone2 ,@Pager ,@RoleID ,@SystemUserRoleID ,@InternalAddressID ,@Message OUTPUT ,@RetVal OUTPUT", param);
            int retVal = (int)param[27].Value;
            string valueRes = param[26].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> SystemUser_SetEnableDisable(Guid id, Guid modifiedBy, bool isDisable)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = id },
                new SqlParameter("@IsDisabled", SqlDbType.Bit) { Value = isDisable },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = id },
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
                
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[SystemUser_SetStatus] @UserID, @IsDisabled, @ModifiedBy, @Message OUTPUT, @RetVal OUTPUT ", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}