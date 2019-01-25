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
        public KeyValuePair<int, String> Team_Insert(Team model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TeamID", SqlDbType.UniqueIdentifier) { Value = model.TeamID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = model.BusinessUnitID },
                new SqlParameter("@OrganizationID", SqlDbType.UniqueIdentifier) { Value = model.OrganizationID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Team_Insert] @TeamID, @Name, @Description, @ModifiedBy, @BusinessUnitID, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Team_Update(Team model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TeamID", SqlDbType.UniqueIdentifier) { Value = model.TeamID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = model.BusinessUnitID },
                new SqlParameter("@OrganizationID", SqlDbType.UniqueIdentifier) { Value = model.OrganizationID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Team_Update] @TeamID, @Name, @Description, @ModifiedBy, @BusinessUnitID, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Team_Delete(Team model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TeamID", SqlDbType.UniqueIdentifier) { Value = model.TeamID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Team_Delete] @TeamID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}