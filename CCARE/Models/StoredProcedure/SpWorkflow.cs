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
        public KeyValuePair<int, String> Workflow_Insert(Workflow workflow)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@WorkflowID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.ID) },
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.SLID) },
                new SqlParameter("@WorkgroupID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.WgID) },
                new SqlParameter("@SLADays", CheckForDbNull(workflow.WorkflowSLADays)),
                new SqlParameter("@SeqNo", CheckForDbNull(workflow.SeqNo)),
		        new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.CreatedBy) },
                new SqlParameter("@Keterangan", SqlDbType.NVarChar) { Value = CheckForDbNull(workflow.Keterangan) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Workflow_Insert] @WorkflowID, @ServiceLevelID, @WorkgroupID, @SLADays, @SeqNo, @CreatedBy, @Keterangan, @RetVal out, @Message out", param);
            string valueRes = param[8].Value.ToString();
            int retVal = (int)param[7].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }


        public KeyValuePair<int, String> Workflow_Update(Workflow workflow)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@WorkflowID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.ID) },
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.SLID) },
                new SqlParameter("@WorkgroupID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.WgID) },
                new SqlParameter("@SLADays", CheckForDbNull(workflow.WorkflowSLADays)),
                new SqlParameter("@SeqNo", CheckForDbNull(workflow.SeqNo)),
		        new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.ModifiedBy) },
                new SqlParameter("@Keterangan", SqlDbType.NVarChar) { Value = CheckForDbNull(workflow.Keterangan) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Workflow_Update] @WorkflowID, @ServiceLevelID, @WorkgroupID, @SLADays, @SeqNo, @ModifiedBy, @Keterangan, @RetVal out, @Message out", param);
            string valueRes = param[8].Value.ToString();
            int retVal = (int)param[7].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }


        public KeyValuePair<int, String> Workflow_Delete(Workflow workflow)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@WorkflowID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.ID) },
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.SLID) },
                new SqlParameter("@SeqNo", CheckForDbNull(workflow.SeqNo)),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Workflow_Delete] @WorkflowID, @ServiceLevelID, @SeqNo, @RetVal out, @Message out", param);
            string valueRes = param[4].Value.ToString();
            int retVal = (int)param[3].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Workflow_Swap(Workflow workflow, String Direction)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.ID) },
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(workflow.SLID) },
                new SqlParameter("@SeqNo", SqlDbType.Int) { Value = CheckForDbNull(workflow.SeqNo) },
                new SqlParameter("@Direction", SqlDbType.NVarChar, 100) {Value = CheckForDbNull(Direction)},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("EXECUTE [CRM].[Workflow_Swap] @ID, @ServiceLevelID, @SeqNo, @Direction, @RetVal out, @Message out", param);
            string valueRes = param[5].Value.ToString();
            int retVal = (int)param[4].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}