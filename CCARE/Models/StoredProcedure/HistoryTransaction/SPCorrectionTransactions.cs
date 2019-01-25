using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.Transaction;

namespace CCARE.Models.General
{
    public partial class HistDb
    {

        public ObjectResult<CorrectionTransaction> GetHistoryCorrectionTransactions(string AccountNo, string TransactionDateFrom, string TransactionDateTo)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountNo", SqlDbType.VarChar) { Value = CheckForDbNull(AccountNo) },
                new SqlParameter("@TransactionDateFrom", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateFrom) },
                new SqlParameter("@TransactionDateTo", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateTo) }
            };

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CorrectionTransaction>("exec [GetHistoricalCorrectionTransactions] @AccountNo, @TransactionDateFrom, @TransactionDateTo", param);
            return spResult;
        }

        public void UpdateHistoryCorrectionTransaction(string AccountNo, DateTime TransactionDate, string RowNo, string RequestID)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountNo", SqlDbType.VarChar) { Value = CheckForDbNull(AccountNo) },
                new SqlParameter("@Date", SqlDbType.DateTime) { Value = CheckForDbNull(TransactionDate) },
                new SqlParameter("@RowNo", SqlDbType.VarChar) { Value = CheckForDbNull(RowNo) },
                new SqlParameter("@RequestID", SqlDbType.VarChar) { Value = CheckForDbNull(RequestID) }
            };

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CorrectionTransaction>("exec [UpdateHistoricalCorrectionTransaction] @AccountNo, @Date, @RowNo, @RequestID", param);
        }
    }
}