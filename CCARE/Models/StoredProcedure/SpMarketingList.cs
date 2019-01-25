using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.SalesMarketing;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> MarketingList_Insert(MarketingList model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
				new SqlParameter("@Purpose", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Purpose) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Description) },
				new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
				new SqlParameter("@CreatedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.CreatedOn) },
				new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
				new SqlParameter("@ModifiedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.ModifiedOn) },
				new SqlParameter("@DeletionStateCode", SqlDbType.Int) { Value = CheckForDbNull(model.DeletionStateCode) },
                new SqlParameter("@StateCode", SqlDbType.Int) { Value = CheckForDbNull(model.StateCode) },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MarketingList_Insert] @ID, @Name, @Purpose, @Description, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @DeletionStateCode, @StateCode, @StatusCode, @RetVal out, @Message out", param);

            int retVal = (int)param[11].Value;
            string valueRes = param[12].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> MarketingList_Update(MarketingList model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
				new SqlParameter("@Purpose", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Purpose) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Description) },
				new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
				new SqlParameter("@CreatedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.CreatedOn) },
				new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
				new SqlParameter("@ModifiedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.ModifiedOn) },
				new SqlParameter("@DeletionStateCode", SqlDbType.Int) { Value = CheckForDbNull(model.DeletionStateCode) },
                new SqlParameter("@StateCode", SqlDbType.Int) { Value = CheckForDbNull(model.StateCode) },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MarketingList_Update] @ID, @Name, @Purpose, @Description, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @DeletionStateCode, @StateCode, @StatusCode, @RetVal out, @Message out", param);

            int retVal = (int)param[11].Value;
            string valueRes = param[12].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> MarketingList_Delete(Guid ID, Guid ModifiedBy)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = ID },
				new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(ModifiedBy) },
				new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MarketingList_Delete] @ID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}