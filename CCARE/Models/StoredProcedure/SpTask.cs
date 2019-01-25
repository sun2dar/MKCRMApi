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
        public KeyValuePair<int, String> spTask_Update(Task task, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TaskID",task.TaskID),
                new SqlParameter("@RequestID",task.RequestID),
                new SqlParameter("@Subject",task.Subject),
                new SqlParameter("@PriorityCode",task.Priority),
                new SqlParameter("@OwnerID",task.OwnerID),
                new SqlParameter("@ModifiedBy",task.ModifiedBy),
                new SqlParameter("@BusinessUnitID", sessionParam.BusinessUnitID),
                new SqlParameter("@OrganizationId", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Task_Update] @TaskID, @RequestID, @Subject,@PriorityCode,@OwnerID,@ModifiedBy,@BusinessUnitID,@OrganizationID, @RetVal out, @Message out", param);
            int retVal = (int)param[8].Value;
            string valueRes = param[9].Value.ToString();
            return new KeyValuePair<int, string>(retVal, valueRes);
        }



        public KeyValuePair<int, String> spTask_Insert(Task task, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TaskID",task.TaskID),
                new SqlParameter("@RequestID",task.RequestID),
                new SqlParameter("@Subject",task.Subject),
                new SqlParameter("@PriorityCode",task.Priority),
                new SqlParameter("@OwnerID",task.OwnerID),
                new SqlParameter("@ModifiedBy",task.ModifiedBy),
                new SqlParameter("@BusinessUnitID", sessionParam.BusinessUnitID),
                new SqlParameter("@OrganizationId", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Task_Insert] @TaskID, @RequestID, @Subject,@PriorityCode,@OwnerID,@ModifiedBy,@BusinessUnitID,@OrganizationID, @RetVal out, @Message out", param);
            int retVal = (int)param[8].Value;
            string valueRes = param[9].Value.ToString();
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spTask_SetStatus(Task task)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TaskID",task.TaskID),        
                new SqlParameter("@NewStatus", task.ActivityStatus),      
                new SqlParameter("@ModifiedBy",task.ModifiedBy),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Task_SetStatus] @TaskID, @NewStatus, @ModifiedBy, @RetVal out, @Message out", param);
            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();
            return new KeyValuePair<int, string>(retVal, valueRes);

        }


        public KeyValuePair<int, String> spTask_Delete(Task task)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@TaskID",task.TaskID),        
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Task_Delete] @TaskID, @RetVal out, @Message out", param);
            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();
            
            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}