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

        public ObjectResult<ATMSwitchingTransaction> GetHistoryATMSwitchingTransactions(string AccountNo, string CardNo, string TransactionDateFrom, string TransactionDateTo)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountNo", SqlDbType.VarChar) { Value = CheckForDbNull(AccountNo) },
                new SqlParameter("@CardNo", SqlDbType.VarChar) { Value = CheckForDbNull(CardNo) },
                new SqlParameter("@TransactionDateFrom", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateFrom) },
                new SqlParameter("@TransactionDateTo", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateTo) }
            };

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ATMSwitchingTransaction>("exec [GetHistoricalAtmSwitchingTransactions] @AccountNo, @CardNo, @TransactionDateFrom, @TransactionDateTo", param);
            return spResult;
        }

    }
}