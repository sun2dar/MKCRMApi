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
        public KeyValuePair<int, String> WSID_Insert(WSID model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@WSIDID", SqlDbType.UniqueIdentifier) { Value = model.WSIDID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Address", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Address) },
                new SqlParameter("@Camera", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Camera) },
                new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.City) },
                new SqlParameter("@Coordinator", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Coordinator) },
                new SqlParameter("@DescKU", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.DescKU) },
                new SqlParameter("@DescKW", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.DescKW) },
                new SqlParameter("@InstallDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.InstallDate) },
                new SqlParameter("@Island", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Island) },
                new SqlParameter("@Location", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Location) },
                new SqlParameter("@Lok", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Lok) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OrganizationID", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[WSID_Insert] @WSIDID, @Name, @Address, @Camera, @City, @Coordinator, @DescKU, @DescKW, @InstallDate, @Island, @Location, @Lok, @ModifiedBy, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[14].Value;
            string valueRes = param[15].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> WSID_Update(WSID model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@WSIDID", SqlDbType.UniqueIdentifier) { Value = model.WSIDID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Address", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Address) },
                new SqlParameter("@Camera", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Camera) },
                new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.City) },
                new SqlParameter("@Coordinator", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Coordinator) },
                new SqlParameter("@DescKU", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.DescKU) },
                new SqlParameter("@DescKW", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.DescKW) },
                new SqlParameter("@InstallDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.InstallDate) },
                new SqlParameter("@Island", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Island) },
                new SqlParameter("@Location", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Location) },
                new SqlParameter("@Lok", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Lok) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[WSID_Update] @WSIDID, @ModifiedBy, @Name, @Address, @Camera, @City, @Coordinator, @DescKU, @DescKW, @InstallDate, @Island, @Location, @Lok, @RetVal out, @Message out", param);

            int retVal = (int)param[13].Value;
            string valueRes = param[14].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> WSID_ChangeStatus(WSID model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@WSIDID", SqlDbType.UniqueIdentifier) { Value = model.WSIDID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[WSID_ChangeStatus] @WSIDID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> WSID_Delete(WSID model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@WSIDID", SqlDbType.UniqueIdentifier) { Value = model.WSIDID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[WSID_Delete] @WSIDID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}