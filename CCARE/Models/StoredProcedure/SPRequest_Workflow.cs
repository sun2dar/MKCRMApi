using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;
using System.Data.SqlClient;
using System.Data;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> Request_Workflow_Change(Request rw, String OperationType)
        {           
            var param = new SqlParameter[]{
                new SqlParameter("@RequestID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(rw.RequestID) },
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(null) },
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(null) },
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(null) },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(rw.CreatedBy) },
                new SqlParameter("@IsReopen", CheckForDbNull(0)),
                new SqlParameter("@OperationType", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(OperationType) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Request_Workflow_Insert] @RequestID, @ProductID, @CategoryID, @ServiceLevelID, @CreatedBy, @IsReopen, @OperationType, @RetVal out, @Message out", param);
            string valueRes = param[8].Value.ToString();
            int retVal = (int)param[7].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}