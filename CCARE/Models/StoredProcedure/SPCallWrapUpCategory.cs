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
        public KeyValuePair<int, String> CallWrapUpCategory_Insert(CallWrapUpCategory model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CallWrapUpCategoryID", SqlDbType.UniqueIdentifier) { Value = model.CallWrapUpCategoryID },
                new SqlParameter("@Description", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OrganizationID", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallWrapUpCategory_Insert] @CallWrapUpCategoryID, @Description, @ModifiedBy, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> CallWrapUpCategory_Update(CallWrapUpCategory model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CallWrapUpCategoryID", SqlDbType.UniqueIdentifier) { Value = model.CallWrapUpCategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Description) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallWrapUpCategory_Update] @CallWrapUpCategoryID, @ModifiedBy, @Description, @RetVal out, @Message out", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> CallWrapUpCategory_ChangeStatus(CallWrapUpCategory model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CallWrapUpCategoryID", SqlDbType.UniqueIdentifier) { Value = model.CallWrapUpCategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallWrapUpCategory_ChangeStatus] @CallWrapUpCategoryID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> CallWrapUpCategory_Delete(CallWrapUpCategory model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CallWrapUpCategoryID", SqlDbType.UniqueIdentifier) { Value = model.CallWrapUpCategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallWrapUpCategory_Delete] @CallWrapUpCategoryID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}