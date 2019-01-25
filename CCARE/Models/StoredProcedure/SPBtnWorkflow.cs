using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.Btn;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> Contact_Insert(BtnWorkflow model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Seq", SqlDbType.Int) { Value = CheckForDbNull(model.Seq) },
                new SqlParameter("@ProductType", SqlDbType.Int) { Value = CheckForDbNull(model.ProductType) },
                new SqlParameter("@Code", SqlDbType.Int) { Value = CheckForDbNull(model.Code) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BtnWorkflow_Insert] @ID, @Seq, @ProductType, @Code, @RetVal out, @Message out", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}