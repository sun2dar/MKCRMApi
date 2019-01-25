using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public class BroadcastTemp
        {
            public Guid? SystemUserId { get; set; }

            public string SystemUserName { get; set; }

            public string TeamName { get; set; }
        }

        public KeyValuePair<int, String> BroadcastMessage_Read(BroadcastMessageDetail model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BroadcastMessageDetailID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BroadcastMessageDetailID) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BroadcastMessage_Read] @BroadcastMessageDetailID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> BroadcastMessage_Insert(BroadcastMessage model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BroadcastMessageID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BroadcastMessageID) },
                new SqlParameter("@TeamID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.TeamID) },
                new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.UserID) },
                new SqlParameter("@Content", SqlDbType.NVarChar, 4000) { Value = CheckForDbNull(model.Content) },
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.StartDate) },
                new SqlParameter("@ExpireDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.ExpireDate) },
                new SqlParameter("@Severity", SqlDbType.Int) { Value = CheckForDbNull(model.Severity) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@BusinessUnit", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BusinessUnit) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BroadcastMessage_Insert] @BroadcastMessageID, @TeamID, @UserID, @Content, @StartDate, @ExpireDate, @Severity, @ModifiedBy, @BusinessUnit, @RetVal out, @Message out", param);

            int retVal = (int)param[9].Value;
            string valueRes = param[10].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public ObjectResult<BroadcastTemp> SpGetListBroadcastUserForSPV(Guid SystemUserID)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@SystemUserID", SystemUserID)
            };

            ObjectResult<BroadcastTemp> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<BroadcastTemp>("exec [CRM].[GetListBroadcastUserForSPV] @SystemUserID", param);
            return spResult;
        }
    }
}