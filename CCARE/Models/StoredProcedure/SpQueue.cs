using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> spQueue_Insert(Queue model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@QueueID", SqlDbType.UniqueIdentifier) {Value = model.QueueId},
                new SqlParameter ("@Name", SqlDbType.NVarChar) {Value = model.Name},
                new SqlParameter ("@BusinessUnitID", SqlDbType.UniqueIdentifier) {Value = sessionParam.BusinessUnitID},
                new SqlParameter ("@BusinessUnitID_ORG", SqlDbType.UniqueIdentifier) {Value = model.BusinessUnitID_ORG},
                new SqlParameter ("@EmailAddress", SqlDbType.NVarChar) {Value = CheckForDbNull(model.EMailAddress)},
                new SqlParameter ("@OwnerID", SqlDbType.UniqueIdentifier) {Value = model.OwnerId},
                new SqlParameter ("@Description",  SqlDbType.NVarChar) {Value = CheckForDbNull(model.Description)},
                new SqlParameter ("@ModifiedBy", SqlDbType.UniqueIdentifier) {Value = sessionParam.CurrentUserID},
                new SqlParameter ("@OrganizationID", SqlDbType.UniqueIdentifier) {Value = sessionParam.OrganizationID},        
                new SqlParameter ("@RetVal", SqlDbType.Int){Direction = ParameterDirection.Output},
                new SqlParameter ("@Message", SqlDbType.NVarChar, 200){Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Queue_Insert] @QueueID,@Name,@BusinessUnitID,@BusinessUnitID_ORG,@EmailAddress,@OwnerID,@Description,@ModifiedBy,@OrganizationID,@RetVal out,@Message out", param);
            int retVal = (int)param[9].Value;
            string valueRes = param[10].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spQueue_Update(Queue model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@QueueID", SqlDbType.UniqueIdentifier) {Value = model.QueueId},
                new SqlParameter ("@Name", SqlDbType.NVarChar) {Value = model.Name},
                new SqlParameter ("@BusinessUnitID", SqlDbType.UniqueIdentifier) {Value = sessionParam.BusinessUnitID},
                new SqlParameter ("@BusinessUnitID_ORG", SqlDbType.UniqueIdentifier) {Value = model.BusinessUnitID_ORG},
                new SqlParameter ("@EmailAddress", SqlDbType.NVarChar) {Value = CheckForDbNull(model.EMailAddress)},
                new SqlParameter ("@OwnerID", SqlDbType.UniqueIdentifier) {Value = model.OwnerId},
                new SqlParameter ("@Description",  SqlDbType.NVarChar) {Value = CheckForDbNull(model.Description)},
                new SqlParameter ("@ModifiedBy", SqlDbType.UniqueIdentifier) {Value = sessionParam.CurrentUserID},
                new SqlParameter ("@OrganizationID", SqlDbType.UniqueIdentifier) {Value = sessionParam.OrganizationID},
                new SqlParameter ("@Message", SqlDbType.NVarChar, 200){Direction = ParameterDirection.Output},
                new SqlParameter ("@RetVal", SqlDbType.Int){Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Queue_Update] @QueueID,@Name,@BusinessUnitID,@BusinessUnitID_ORG,@EmailAddress,@OwnerID,@Description,@ModifiedBy,@OrganizationID,@Message out, @RetVal out", param);
            int retVal = (int)param[10].Value;
            string valueRes = param[9].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spQueue_Delete(Queue model, SessionForSP sessionParam)
        {

            var param = new SqlParameter[]{
                new SqlParameter ("@QueueID",model.QueueId),
                new SqlParameter ("@ModifiedBy", sessionParam.CurrentUserID),
                new SqlParameter ("@RetVal",SqlDbType.Int){Direction = ParameterDirection.Output},
                new SqlParameter ("@Message",SqlDbType.NVarChar, 200){Direction = ParameterDirection.Output}
             };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Queue_Delete] @QueueID,@ModifiedBy,@RetVal out, @Message out", param);
            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}