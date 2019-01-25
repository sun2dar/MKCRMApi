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
        public KeyValuePair<int, String> ServiceLevel_Insert(ServiceLevel model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = model.ServiceLevelID },
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@CategoryIDName", SqlDbType.NVarChar, 100) { Value = model.CategoryIDName },
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ProductIDName", SqlDbType.NVarChar, 100) { Value = model.ProductIDName },
                new SqlParameter("@SLADays", SqlDbType.Int) { Value = model.SLADays },
                new SqlParameter("@WorkgroupID", SqlDbType.UniqueIdentifier) { Value = model.WorkgroupID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OrganizationID", sessionParam.OrganizationID),
                new SqlParameter("@KotaID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.KotaID) },
                new SqlParameter("@ParentID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentID) },
                new SqlParameter("@SegmentationID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.SegmentationID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ServiceLevel_Insert] @ServiceLevelID, @CategoryID, @CategoryIDName, @ProductID, @ProductIDName, @SLADays, @WorkgroupID, @ModifiedBy, @OrganizationID, @KotaID, @ParentID, @SegmentationID, @RetVal out, @Message out", param);

            int retVal = (int)param[12].Value;
            string valueRes = param[13].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> ServiceLevel_Update(ServiceLevel model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = model.ServiceLevelID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@CategoryIDName", SqlDbType.NVarChar, 100) { Value = model.CategoryIDName },
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ProductIDName", SqlDbType.NVarChar, 100) { Value = model.ProductIDName },
                new SqlParameter("@SLADays", SqlDbType.Int) { Value = model.SLADays },
                new SqlParameter("@WorkgroupID", SqlDbType.UniqueIdentifier) { Value = model.WorkgroupID },
                new SqlParameter("@KotaID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.KotaID) },
                new SqlParameter("@ParentID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentID) },
                new SqlParameter("@SegmentationID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.SegmentationID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ServiceLevel_Update] @ServiceLevelID, @ModifiedBy, @CategoryID, @CategoryIDName, @ProductID, @ProductIDName, @SLADays, @WorkgroupID, @KotaID, @ParentID, @SegmentationID, @RetVal out, @Message out", param);

            int retVal = (int)param[11].Value;
            string valueRes = param[12].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> ServiceLevel_ChangeStatus(ServiceLevel model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = model.ServiceLevelID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ServiceLevel_ChangeStatus] @ServiceLevelID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> ServiceLevel_Delete(ServiceLevel model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ServiceLevelID", SqlDbType.UniqueIdentifier) { Value = model.ServiceLevelID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ServiceLevel_Delete] @ServiceLevelID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}