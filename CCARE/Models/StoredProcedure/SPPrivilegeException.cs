using CCARE.Models.Role;
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

        public SqlParameter[] populateModel(PrivilegeException model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@PrivilegeExceptionID", SqlDbType.UniqueIdentifier) { Value = model.PrivilegeExceptionID },
                new SqlParameter("@StatusChangeID", SqlDbType.UniqueIdentifier) { Value = model.StatusChangeID },
                new SqlParameter("@SecurityRoleID", SqlDbType.UniqueIdentifier) { Value = model.SecurityRoleID },
                new SqlParameter("@Inclusive", SqlDbType.Bit) { Value = model.Inclusive },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            return param;
        }

        public KeyValuePair<int, String> PrivilegeException_Insert(PrivilegeException model)
        {
            var param = populateModel(model);

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[PrivilegeException_Insert] @PrivilegeExceptionID, @StatusChangeID, @SecurityRoleID,@Inclusive , @RetVal OUTPUT ,@Message OUTPUT", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> PrivilegeException_Update(PrivilegeException model)
        {
            var param = populateModel(model);

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[PrivilegeException_Update] @PrivilegeExceptionID, @StatusChangeID, @SecurityRoleID,@Inclusive , @RetVal OUTPUT ,@Message OUTPUT", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> PrivilegeException_Delete(PrivilegeException model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@PrivilegeExceptionID", SqlDbType.UniqueIdentifier) { Value = model.PrivilegeExceptionID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[PrivilegeException_Delete] @PrivilegeExceptionID, @RetVal OUTPUT ,@Message OUTPUT", param);

            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}