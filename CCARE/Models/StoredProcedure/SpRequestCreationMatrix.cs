using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using CCARE.Models.MasterData;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> RequestCreationMatrix_Insert(RequestCreationMatrix model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@RequestCreationMatrixID", SqlDbType.UniqueIdentifier) { Value = model.RequestCreationMatrixID },
                new SqlParameter("@StatusChangeID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.StatusChangeID) },
                new SqlParameter("@AccountCode", SqlDbType.NChar) { Value = CheckForDbNull(model.AccountCode) },
                new SqlParameter("@RequestStatus", SqlDbType.Int) { Value = CheckForDbNull(model.RequestStatus) },
                new SqlParameter("@Summary", SqlDbType.NVarChar) { Value = model.Summary },
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@RequestCreationMatrixID");
            listParams.Add("@StatusChangeID");
            listParams.Add("@AccountCode");
            listParams.Add("@RequestStatus");
            listParams.Add("@Summary");
            listParams.Add("@ProductID");
            listParams.Add("@CategoryID");
            listParams.Add("@ModifiedBy");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("EXEC [CRM].[RequestCreationMatrix_Insert]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> RequestCreationMatrix_Update(RequestCreationMatrix model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@RequestCreationMatrixID", SqlDbType.UniqueIdentifier) { Value = model.RequestCreationMatrixID },
                new SqlParameter("@StatusChangeID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.StatusChangeID) },
                new SqlParameter("@AccountCode", SqlDbType.NChar) { Value = CheckForDbNull(model.AccountCode) },
                new SqlParameter("@RequestStatus", SqlDbType.Int) { Value = CheckForDbNull(model.RequestStatus) },
                new SqlParameter("@Summary", SqlDbType.NVarChar) { Value = model.Summary },
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = model.ProductID },
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = model.CategoryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@RequestCreationMatrixID");
            listParams.Add("@StatusChangeID");
            listParams.Add("@AccountCode");
            listParams.Add("@RequestStatus");
            listParams.Add("@Summary");
            listParams.Add("@ProductID");
            listParams.Add("@CategoryID");
            listParams.Add("@ModifiedBy");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("EXEC [CRM].[RequestCreationMatrix_Update]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> RequestCreationMatrix_ChangeStatus(RequestCreationMatrix model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@RequestCreationMatrixID", model.RequestCreationMatrixID),
                new SqlParameter("@ModifiedBy", model.ModifiedBy),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("EXEC [CRM].[RequestCreationMatrix_ChangeStatus] @RequestCreationMatrixID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> RequestCreationMatrix_Delete(RequestCreationMatrix model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@RequestCreationMatrixID", model.RequestCreationMatrixID),
                new SqlParameter("@ModifiedBy", model.ModifiedBy),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("EXEC [CRM].[RequestCreationMatrix_Delete] @RequestCreationMatrixID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public ObjectResult<RequestCreationMatrix> RequestCreationMatrix_GetData(RequestCreationMatrix model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@EntityType", SqlDbType.Int) { Value = model.EntityType },
                new SqlParameter("@StatusType", SqlDbType.Int) { Value = model.StatusType },
                new SqlParameter("@PrevStatus", SqlDbType.VarChar) { Value = model.PrevStatus },
                new SqlParameter("@NewStatus", SqlDbType.VarChar) { Value = model.NewStatus },
                new SqlParameter("@AccountCode", SqlDbType.VarChar) { Value = model.AccountCode }
            };

            ObjectResult<RequestCreationMatrix> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<RequestCreationMatrix>("exec [CRM].[RequestCreationMatrix_GetData] @EntityType, @StatusType, @PrevStatus, @NewStatus, @AccountCode", param);
            return spResult;
        }
    }
}