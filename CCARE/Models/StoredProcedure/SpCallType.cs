using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> spCallType_Insert(CallType model)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@CallTypeID",SqlDbType.UniqueIdentifier){Value = model.CallTypeID},
                new SqlParameter ("@CallWrapUpID",model.CallWrapUpID),
                new SqlParameter ("@CallWrapUpCategoryID",CheckForDbNull(model.CallWrapUpCategoryID)),
                new SqlParameter ("@Summary",CheckForDbNull(model.Summary)),
                new SqlParameter ("@ModifiedBy",model.ModifiedBy),
                new SqlParameter ("@BusinessUnitID",model.BusinessUnitID),
                new SqlParameter ("@RetVal",SqlDbType.Int){Direction = ParameterDirection.Output},
                new SqlParameter ("@Message",SqlDbType.NVarChar, 200){Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallType_Insert] @CallTypeID,@CallWrapUpID,@CallWrapUpCategoryID,@Summary,@ModifiedBy,@BusinessUnitID,@RetVal out, @Message out", param);
            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spCallType_Update(CallType model)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@CallTypeID",SqlDbType.UniqueIdentifier){Value = model.CallTypeID},
                new SqlParameter ("@CallWrapUpID",model.CallWrapUpID),
                new SqlParameter ("@CallWrapUpCategoryID",CheckForDbNull(model.CallWrapUpCategoryID)),
                new SqlParameter ("@Summary",CheckForDbNull(model.Summary)),
                new SqlParameter ("@ModifiedBy",model.ModifiedBy),
                new SqlParameter ("@BusinessUnitID",model.BusinessUnitID),
                new SqlParameter ("@RetVal",SqlDbType.Int){Direction = ParameterDirection.Output},
                new SqlParameter ("@Message",SqlDbType.NVarChar, 200){Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallType_Update] @CallTypeID,@CallWrapUpID,@CallWrapUpCategoryID,@Summary,@ModifiedBy,@BusinessUnitID,@RetVal out, @Message out", param);
            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }



        public KeyValuePair<int, String> spCallType_ChangeStatus(CallType calltype)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CallTypeID", SqlDbType.UniqueIdentifier) { Value = calltype.CallTypeID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = calltype.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallType_ChangeStatus] @CallTypeID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spCallType_Delete(CallType calltype)
        {

            System.Windows.Forms.MessageBox.Show("delete " + calltype.CallTypeID);
            var param = new SqlParameter[]{
                new SqlParameter("@CallTypeID", SqlDbType.UniqueIdentifier) { Value = calltype.CallTypeID},
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = calltype.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallType_Delete] @CallTypeID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}