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
        public KeyValuePair<int, String> QueueItem_AssignTo(Guid OwnerID,Guid ModifiedBy, Guid ObjectId, string ObjectTypeCode, string BusinessUnitID, int a)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ObjectID", ObjectId),
                new SqlParameter("@ObjectTypeCode",ObjectTypeCode),
                new SqlParameter("@AssignToID",OwnerID),
                new SqlParameter("@NewQueueTypeCode",a),
                new SqlParameter("@ModifiedBy",ModifiedBy),
                new SqlParameter("@BusinessUnitID", BusinessUnitID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[QueueItem_AssignTo] @ObjectID, @ObjectTypeCode,@AssignToID,@NewQueueTypeCode, @ModifiedBy, @BusinessUnitID, @RetVal out, @Message out", param);
            int retVal = (int)param[6].Value;
            string valueRes = param[7].Value.ToString();
        
            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}