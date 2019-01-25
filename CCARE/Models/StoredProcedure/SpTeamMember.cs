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
        public KeyValuePair<int, String> TeamMember_Insert(TeamMember model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TeamMemberID", SqlDbType.UniqueIdentifier) { Value = model.TeamMembershipID },
                new SqlParameter("@TeamID", SqlDbType.UniqueIdentifier) { Value = model.TeamId },
                new SqlParameter("@SystemUserID", SqlDbType.UniqueIdentifier) { Value = model.SystemUserId },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[TeamMember_Insert] @TeamMemberID, @TeamID, @SystemUserID, @RetVal out, @Message out", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> TeamMember_Delete(TeamMember model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TeamMemberID", SqlDbType.UniqueIdentifier) { Value = model.TeamMembershipID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = sessionParam.CurrentUserID},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[TeamMember_Delete] @TeamMemberID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}