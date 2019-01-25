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
        public KeyValuePair<int, String> MarketingListPurpose_Insert(MarketingListPurpose model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@DeletionStateCode", SqlDbType.Int) { Value = CheckForDbNull(model.DeletionStateCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MarketingListPurpose_Insert] @ID, @Name, @Description, @DeletionStateCode, @RetVal out, @Message out", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> MarketingListPurpose_Update(MarketingListPurpose model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@DeletionStateCode", SqlDbType.Int) { Value = CheckForDbNull(model.DeletionStateCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MarketingListPurpose_Update] @ID, @Name, @Description, @DeletionStateCode, @RetVal out, @Message out", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> MarketingListPurpose_Delete(MarketingListPurpose model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
				new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MarketingListPurpose_Delete] @ID, @RetVal out, @Message out", param);

            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}