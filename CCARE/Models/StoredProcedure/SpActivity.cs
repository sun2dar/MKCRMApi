using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        // SP for Menu Action
        public KeyValuePair<int, String> Activity_Insert(Activities model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ActivityID", SqlDbType.UniqueIdentifier) { Value = model.ActivityID },
                new SqlParameter("@Subject", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Subject) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@RegardingContactID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.RegardingContactID) },
                new SqlParameter("@RegardingObjectID", SqlDbType.UniqueIdentifier, 200) { Value = CheckForDbNull(model.RegardingObjectID) },
                new SqlParameter("@ObjectTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.ObjectTypeCode) },
                new SqlParameter("@ActivityTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.ActivityTypeCode) },
                new SqlParameter("@ScheduledStart", SqlDbType.DateTime) { Value = CheckForDbNull(model.ScheduledStart) },
                new SqlParameter("@ScheduledEnd", SqlDbType.DateTime) { Value = CheckForDbNull(model.ScheduledEnd) },
                new SqlParameter("@Remark", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Remark) },
                new SqlParameter("@OwnerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.OwnerID) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BusinessUnitID) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Activity_Insert]  @ActivityID, @Subject, @Description, @RegardingContactID, @RegardingObjectID, @ObjectTypeCode, @ActivityTypeCode, @ScheduledStart, @ScheduledEnd, @Remark, @OwnerID, @BusinessUnitID, @ModifiedBy, @StatusCode, @RetVal out, @Message out", param);

            int retVal = (int)param[14].Value;
            string valueRes = param[15].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Activity_Update(Activities model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ActivityID", SqlDbType.UniqueIdentifier) { Value = model.ActivityID },
                new SqlParameter("@Subject", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Subject) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@RegardingContactID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.RegardingContactID) },
                new SqlParameter("@RegardingObjectID", SqlDbType.UniqueIdentifier, 200) { Value = CheckForDbNull(model.RegardingObjectID) },
                new SqlParameter("@ObjectTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.ObjectTypeCode) },
                new SqlParameter("@ActivityTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.ActivityTypeCode) },
                new SqlParameter("@ScheduledStart", SqlDbType.DateTime) { Value = CheckForDbNull(model.ScheduledStart) },
                new SqlParameter("@ScheduledEnd", SqlDbType.DateTime) { Value = CheckForDbNull(model.ScheduledEnd) },
                new SqlParameter("@Remark", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Remark) },
                new SqlParameter("@OwnerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.OwnerID) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BusinessUnitID) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Activity_Update]  @ActivityID, @Subject, @Description, @RegardingContactID, @RegardingObjectID, @ObjectTypeCode, @ActivityTypeCode, @ScheduledStart, @ScheduledEnd, @Remark, @OwnerID, @BusinessUnitID, @ModifiedBy, @StatusCode, @RetVal out, @Message out", param);

            int retVal = (int)param[14].Value;
            string valueRes = param[15].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        //Change State : 0=Open, 1=Complete, 2=Canceled, 3=Scheduled
        //        SELECT* from CRM.StringMap
        //where EntityName = 'activitypointer' AND AttributeName = 'statecode'
        public KeyValuePair<int, String> Activity_ChangeState(Guid ID, int StateCode, Guid ModifiedBy)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = ID },
                new SqlParameter("@StateCode", SqlDbType.Int) { Value = CheckForDbNull(StateCode) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(ModifiedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Activity_ChangeState] @ID, @StateCode, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}