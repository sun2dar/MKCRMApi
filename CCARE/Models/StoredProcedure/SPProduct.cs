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

        
        /// <summary>
        /// BTN product insert
        /// </summary>
        public KeyValuePair<int, String> Product_Insert_Btn(Product model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = model.Description },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@ParentProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentProductID) },
                new SqlParameter("@ProductTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.ProductTypeCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Product_Insert_Btn] @ProductID, @ModifiedBy, @Description, @Name, @ParentProductId, @ProductTypeCode, @RetVal out, @Message out", param);

            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Product_Update_Btn(Product model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = model.Description },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@ParentProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentProductID) },
                new SqlParameter("@ProductTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.ProductTypeCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Product_Update_Btn] @ProductID, @ModifiedBy, @Description, @Name, @ParentProductId, @ProductTypeCode, @RetVal out, @Message out", param);

            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }


        /// <summary>
        /// Normal function 
        /// </summary>
        public KeyValuePair<int, String> Product_Insert(Product model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = model.Description },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@ParentProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentProductID) },
                new SqlParameter("@OrganizationId", sessionParam.OrganizationID ),          // session 
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Product_Insert] @ProductID, @ModifiedBy, @Description, @Name, @ParentProductId, @OrganizationId, @RetVal out, @Message out", param);

            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Product_Update(Product model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Description", SqlDbType.NVarChar, 100) { Value = model.Description },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = model.Name },
                new SqlParameter("@ParentProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ParentProductID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Product_Update] @ProductID, @ModifiedBy, @Description, @name, @ParentProductID, @RetVal out, @Message out", param);

            int retVal = (int)param[5].Value;
            string valueRes = param[6].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Product_ChangeStatus(Product model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Product_ChangeStatus] @ProductID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Product_Delete(Product model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Product_Delete] @ProductID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}