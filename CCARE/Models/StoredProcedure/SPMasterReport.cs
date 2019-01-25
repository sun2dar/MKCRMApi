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
        public KeyValuePair<int, String> MasterReport_Insert(MasterReport model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@ReportName", SqlDbType.NVarChar, 200) { Value = model.ReportName },
                new SqlParameter("@Description", SqlDbType.NVarChar, 400) { Value = model.Description },
                new SqlParameter("@RdlName", SqlDbType.NVarChar, 200) { Value = model.RdlName },
                new SqlParameter("@UserUpdate", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MasterReport_Insert] @ReportID , @ReportName, @Description, @RdlName, @UserUpdate, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[5].Value;
            string valueRes = param[6].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> MasterReport_Update(MasterReport model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@ReportName", SqlDbType.NVarChar, 200) { Value = model.ReportName },
                new SqlParameter("@Description", SqlDbType.NVarChar, 400) { Value = model.Description },
                new SqlParameter("@RdlName", SqlDbType.NVarChar, 200) { Value = model.RdlName },
                new SqlParameter("@UserUpdate", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MasterReport_Update] @ReportID , @ReportName, @Description, @RdlName, @UserUpdate, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[5].Value;
            string valueRes = param[6].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> MasterReport_Delete(MasterReport model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MasterReport_Delete] @ReportID, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> MasterReport_ChangeStatus(MasterReport model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@UserUpdate", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[MasterReport_ChangeStatus] @ReportID, @UserUpdate, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}