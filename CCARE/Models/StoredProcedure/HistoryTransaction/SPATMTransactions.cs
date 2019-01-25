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
        
        public ObjectResult<ATMTransaction> GetHistoryATMTransactions(string CardNo, string AccountNo, string RequestID, string TransactionDateFrom, string TransactionDateTo)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CardNo", SqlDbType.VarChar) { Value = CheckForDbNull(CardNo), IsNullable = true },
                new SqlParameter("@AccountNo", SqlDbType.VarChar) { Value = CheckForDbNull(AccountNo), IsNullable = true },
                new SqlParameter("@RequestID", SqlDbType.VarChar) { Value = CheckForDbNull(RequestID) },
                new SqlParameter("@TransactionDateFrom", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateFrom) },
                new SqlParameter("@TransactionDateTo", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateTo) }
            };

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ATMTransaction>("exec [GetHistoryATMTransactions] @CardNo, @AccountNo, @RequestID, @TransactionDateFrom, @TransactionDateTo", param);
            return spResult;
        }

        public void UpdateHistoryATMTransaction(string CardNo, DateTime TransactionDate, string SequenceNo, string RequestID)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CardNo", SqlDbType.VarChar) { Value = CheckForDbNull(CardNo) },
                new SqlParameter("@Date", SqlDbType.DateTime) { Value = CheckForDbNull(TransactionDate) },
                new SqlParameter("@SequenceNo", SqlDbType.VarChar) { Value = CheckForDbNull(SequenceNo) },
                new SqlParameter("@RequestID", SqlDbType.VarChar) { Value = CheckForDbNull(RequestID) }
            };

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ATMTransaction>("exec [UpdateHistoricalATMTransaction] @CardNo, @Date, @SequenceNo, @RequestID", param);
        }

        public ObjectResult<ATMTransaction> GetHistoryATMTransactionsByTerminalID(string Terminal, string SequenceNo)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@Terminal", SqlDbType.VarChar) { Value = CheckForDbNull(Terminal) },
                new SqlParameter("@SequenceNo", SqlDbType.VarChar) { Value = CheckForDbNull(SequenceNo) }
            };

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ATMTransaction>("exec [GetHistoryATMTransactionsByTerminal] @Terminal, @SequenceNo", param);
            return spResult;
        }
    }
}