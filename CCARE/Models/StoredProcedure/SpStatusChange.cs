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
        public KeyValuePair<int, String> StatusChange_Insert(StatusChange model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@StatusChangeID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.StatusChangeID) },
                new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.MappingID) },
                new SqlParameter("@NewStatus", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.NewKey) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[StatusChange_Insert] @StatusChangeID, @MappingID, @NewStatus, @RetVal out, @Message out", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> StatusChange_Delete(StatusChange model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@StatusChangeID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.StatusChangeID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[StatusChange_Delete] @StatusChangeID, @RetVal out, @Message out", param);

            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}