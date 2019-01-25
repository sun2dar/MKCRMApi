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
        // SP for Menu Action
        public KeyValuePair<int, String> Cause_Insert(Cause model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CauseID", SqlDbType.UniqueIdentifier) { Value = model.CauseID },
                new SqlParameter("@Code", SqlDbType.NVarChar, 100) { Value = model.Code },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OrganizationID", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Cause_Insert] @CauseID, @Code, @Name, @ModifiedBy, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[5].Value;
            string valueRes = param[6].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Cause_Update(Cause model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CauseID", SqlDbType.UniqueIdentifier) { Value = model.CauseID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Code", SqlDbType.NVarChar, 100) { Value = model.Code },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Cause_Update] @CauseID, @ModifiedBy, @Code, @Name, @RetVal out, @Message out", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Cause_ChangeStatus(Cause model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CauseID", SqlDbType.UniqueIdentifier) { Value = model.CauseID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Cause_ChangeStatus] @CauseID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Cause_Delete(Cause model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CauseID", SqlDbType.UniqueIdentifier) { Value = model.CauseID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Cause_Delete] @CauseID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}