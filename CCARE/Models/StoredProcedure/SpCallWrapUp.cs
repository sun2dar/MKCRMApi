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
        public KeyValuePair<int, String> spCallWrapUp_Insert(CallWrapUp model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@CallWrapUpID",SqlDbType.UniqueIdentifier){Value = model.CallWrapUpID},
                new SqlParameter ("@CustomerID",CheckForDbNull(model.CustomerID)),
                new SqlParameter ("@NonCustomerID",CheckForDbNull(model.NonCustomerID)),
                new SqlParameter ("@CallEndTime",SqlDbType.DateTime) { Value = CheckForDbNull(model.CallEndTime)},
                new SqlParameter ("@CallStartTime",SqlDbType.DateTime) { Value = CheckForDbNull(model.CallStartTime)},
                new SqlParameter ("@ModifiedBy", SqlDbType.UniqueIdentifier) {Value = sessionParam.CurrentUserID},
                new SqlParameter ("@OrganizationId",sessionParam.OrganizationID),
                new SqlParameter ("@RetVal",SqlDbType.Int){Direction = ParameterDirection.Output},
                new SqlParameter ("@Message",SqlDbType.NVarChar, 200){Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallWrapUp_Insert] @CallWrapUpID,@CustomerID,@NonCustomerID,@CallEndTime,@CallStartTime,@ModifiedBy,@OrganizationId,@RetVal out, @Message out", param);
            int retVal = (int)param[7].Value;
            string valueRes = param[8].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }


        public KeyValuePair<int, String> spCallWrapUp_Update(CallWrapUp model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@CallWrapUpID",SqlDbType.UniqueIdentifier) {Value = model.CallWrapUpID},
                new SqlParameter ("@CustomerID",CheckForDbNull(model.CustomerID)),
                new SqlParameter ("@NonCustomerID",CheckForDbNull(model.NonCustomerID)),
                new SqlParameter ("@CallEndTime",SqlDbType.DateTime) { Value = CheckForDbNull(model.CallEndTime)},
                new SqlParameter ("@CallStartTime",SqlDbType.DateTime) { Value = CheckForDbNull(model.CallStartTime)},
                new SqlParameter ("@ModifiedBy", SqlDbType.UniqueIdentifier) {Value = sessionParam.CurrentUserID},
                new SqlParameter ("@OrganizationId",sessionParam.OrganizationID),
                new SqlParameter ("@RetVal",SqlDbType.Int){Direction = ParameterDirection.Output},
                new SqlParameter ("@Message",SqlDbType.NVarChar, 200){Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CallWrapUp_Update] @CallWrapUpID,@CustomerID,@NonCustomerID,@CallEndTime,@CallStartTime,@ModifiedBy,@OrganizationId,@RetVal out, @Message out", param);
            int retVal = (int)param[7].Value;
            string valueRes = param[8].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}