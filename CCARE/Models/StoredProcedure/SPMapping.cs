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
        public KeyValuePair<int, String> Mapping_Insert(Mapping model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.MappingID) },
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.CategoryName) },
                new SqlParameter("@ObjectName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.ObjectName) },
                new SqlParameter("@AttributeName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.AttributeName) },
                new SqlParameter("@Code", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Code) },
                new SqlParameter("@Label", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Label) },
                new SqlParameter("@NewCodeList", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.NewCodeList) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Mapping_Insert] @MappingID, @CategoryName, @ObjectName, @AttributeName, @Code, @Label, @NewCodeList, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[8].Value;
            string valueRes = param[9].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Mapping_Update(Mapping model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.MappingID) },
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.CategoryName) },
                new SqlParameter("@ObjectName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.ObjectName) },
                new SqlParameter("@AttributeName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.AttributeName) },
                new SqlParameter("@Code", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Code) },
                new SqlParameter("@Label", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Label) },
                new SqlParameter("@NewCodeList", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.NewCodeList) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Mapping_Update] @MappingID, @CategoryName, @ObjectName, @AttributeName, @Code, @Label, @NewCodeList, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[8].Value;
            string valueRes = param[9].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Mapping_ChangeStatus(Mapping model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier) { Value = model.MappingID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Mapping_ChangeStatus] @MappingID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}