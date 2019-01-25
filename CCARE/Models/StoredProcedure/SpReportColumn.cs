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
        public KeyValuePair<int, String> ReportColumn_Insert(ReportColumn model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@ReportID", SqlDbType.UniqueIdentifier) { Value = model.ReportID },
                new SqlParameter("@SeqNo", SqlDbType.Int) { Value = CheckForDbNull(model.SeqNo) },      
                new SqlParameter("@EntityColumnID", SqlDbType.UniqueIdentifier) { Value = model.EntityColumnID },                
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [rpt].[ReportColumn_Insert] @ID, @ReportID, @SeqNo, @EntityColumnID, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> ReportColumn_Delete(ReportColumn model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [rpt].[ReportColumn_Delete] @ID, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[1].Value;
            string valueRes = param[2].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> ReportColumn_SwapSeqNo(Guid ID1, int SeqNo1, Guid ID2, int SeqNo2)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID1", SqlDbType.UniqueIdentifier) { Value = ID1 },
                new SqlParameter("@SeqNo1", SqlDbType.Int) { Value = SeqNo1 },
                new SqlParameter("@ID2", SqlDbType.UniqueIdentifier) { Value = ID2 },
                new SqlParameter("@SeqNo2", SqlDbType.Int) { Value = SeqNo2 },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [rpt].[ReportColumn_SwapSeqNo] @ID1, @SeqNo1, @ID2, @SeqNo2, @RetVal OUTPUT, @Message OUTPUT", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        //public static object CheckDefaultGuid(object value)
        //{
        //    if (value == null || value.ToString() == "00000000-0000-0000-0000-000000000000")
        //    {
        //        return DBNull.Value;
        //    }
        //    return value;
        //}
    }
}