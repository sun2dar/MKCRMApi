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
        public KeyValuePair<int, String> Category_Insert(Category model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = model.Description },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@ParentCategoryID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentCategoryID) },
                new SqlParameter("@OrganizationID", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Category_Insert] @CategoryID, @ModifiedBy, @Description, @Name, @ParentCategoryID, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Category_Update(Category model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = model.Description },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@ParentCategoryID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentCategoryID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Category_Update] @CategoryID, @ModifiedBy, @Description, @Name, @ParentCategoryID, @RetVal out, @Message out", param);

            int retVal = (int)param[5].Value;
            string valueRes = param[6].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Category_ChangeStatus(Category model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Category_ChangeStatus] @CategoryID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Category_Delete(Category model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Category_Delete] @CategoryID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}