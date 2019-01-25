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
        public KeyValuePair<int, String> OrganizationClosure_Insert(OrganizationClosure model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@OrganizationClosureID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.OrganizationClosureID) },
                new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.StartDate) },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.EndDate) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OrganizationID", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[OrganizationClosure_Insert] @OrganizationClosureID, @Name, @StartDate, @EndDate, @ModifiedBy, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> OrganizationClosure_Update(OrganizationClosure model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@OrganizationClosureID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.OrganizationClosureID) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.StartDate) },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.EndDate) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[OrganizationClosure_Update] @OrganizationClosureID, @ModifiedBy, @Name, @StartDate, @EndDate, @RetVal out, @Message out", param);

            int retVal = (int)param[5].Value;
            string valueRes = param[6].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> OrganizationClosure_ChangeStatus(OrganizationClosure model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@OrganizationClosureID", SqlDbType.UniqueIdentifier) { Value = model.OrganizationClosureID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[OrganizationClosure_ChangeStatus] @OrganizationClosureID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> OrganizationClosure_Delete(OrganizationClosure model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@OrganizationClosureID", SqlDbType.UniqueIdentifier) { Value = model.OrganizationClosureID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[OrganizationClosure_Delete] @OrganizationClosureID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}