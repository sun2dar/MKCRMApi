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
        public KeyValuePair<int, String> ReportRole_Insert(ReportRole model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportRoleID", SqlDbType.UniqueIdentifier) { Value = model.ReportRoleID },
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@SecurityRoleId", SqlDbType.UniqueIdentifier) { Value = model.SecurityRoleId },
                new SqlParameter("@UserUpdate", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ReportRole_Insert] @ReportRoleID, @ReportID, @SecurityRoleId, @UserUpdate, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> ReportRole_Delete(ReportRole model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportRoleID", SqlDbType.UniqueIdentifier) { Value = model.ReportRoleID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ReportRole_Delete] @ReportRoleID, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}