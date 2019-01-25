using CCARE.Models.Role;
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
        public KeyValuePair<int, String> ReportFilters_Insert(ReportFilters model, Guid UserUpdate)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportFiltersID", SqlDbType.UniqueIdentifier) { Value = model.ReportFiltersID },
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@FilterType", SqlDbType.NVarChar, 50) { Value = model.FilterType },
                new SqlParameter("@FilterGUID", SqlDbType.UniqueIdentifier) { Value = CheckDefaultGuid(model.FilterGUID) },
                new SqlParameter("@FilterValue", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.FilterValue) },
                new SqlParameter("@IsEditable", SqlDbType.Int) { Value = CheckForDbNull(model.IsEditable) },
                new SqlParameter("@UserUpdate", SqlDbType.UniqueIdentifier) { Value = UserUpdate },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ReportFilters_Insert] @ReportFiltersID, @ReportID, @FilterType, @FilterGUID, @FilterValue, @IsEditable, @UserUpdate, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[7].Value;
            string valueRes = param[8].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> ReportFilters_Delete(ReportFilters model, Guid UserUpdate)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportFiltersID", SqlDbType.UniqueIdentifier) { Value = model.ReportFiltersID },
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@UserUpdate", SqlDbType.UniqueIdentifier) { Value = UserUpdate },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[ReportFilters_Delete] @ReportFiltersID, @ReportID, @UserUpdate, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public static object CheckDefaultGuid(object value)
        {
            if (value == null || value.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                return DBNull.Value;
            }
            return value;
        }
    }
}