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
        public KeyValuePair<int, String> Report_Insert(ReportView model, Guid SecurityRoleID)
        {
            Guid ID2 = Guid.NewGuid();

            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@EntityID", SqlDbType.UniqueIdentifier) { Value = model.EntityID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 400) { Value = model.Name },
                new SqlParameter("@Description", SqlDbType.NVarChar, 200) { Value = model.Description },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = model.CreatedBy },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Hyperlink", SqlDbType.Bit) { Value = model.Hyperlink },
                new SqlParameter("@ID2", SqlDbType.UniqueIdentifier) { Value = ID2 },
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@SecurityRoleID", SqlDbType.UniqueIdentifier) { Value = SecurityRoleID },
                //new SqlParameter("@CreatedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.CreatedOn) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [rpt].[Report_Insert] @ID , @EntityID, @Name, @Description, @CreatedBy, @ModifiedBy, @Hyperlink, @ID2, @ReportID, @SecurityRoleID, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[10].Value;
            string valueRes = param[11].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Report_Update(ReportView model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = model.Name},
                new SqlParameter("@Description", SqlDbType.NVarChar, 400) { Value = model.Description },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Hyperlink", SqlDbType.Bit) { Value = model.Hyperlink},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [rpt].[Report_Update] @ID , @Name, @Description, @ModifiedBy, @Hyperlink, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[5].Value;
            string valueRes = param[6].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Report_Delete(ReportView model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [rpt].[Report_Delete] @ID, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}