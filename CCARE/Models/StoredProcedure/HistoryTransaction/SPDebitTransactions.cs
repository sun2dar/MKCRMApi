﻿using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.Transaction;
using System.Collections.Generic;
using System.Text;

namespace CCARE.Models.General
{
    public partial class HistDb
    {

        public ObjectResult<DebitTransaction> GetHistoryDebitTransactions(string AccountNo, string CardNo, string TransactionDateFrom, string TransactionDateTo)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountNo", SqlDbType.VarChar) { Value = CheckForDbNull(AccountNo) },
                new SqlParameter("@CardNo", SqlDbType.VarChar) { Value = CheckForDbNull(CardNo) },
                new SqlParameter("@TransactionDateFrom", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateFrom) },
                new SqlParameter("@TransactionDateTo", SqlDbType.VarChar) { Value = CheckForDbNull(TransactionDateTo) }
            };
            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<DebitTransaction>("exec [GetHistoryDebitTransactions] @AccountNo, @CardNo, @TransactionDateFrom, @TransactionDateTo", param);
            return spResult;
        }

        public void UpdateHistoryDebitTransaction(string CardNo, DateTime TransactionDate, string RefNo, string RequestID)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CardNo", SqlDbType.VarChar) { Value = CheckForDbNull(CardNo) },
                new SqlParameter("@Date", SqlDbType.DateTime) { Value = CheckForDbNull(TransactionDate) },
                new SqlParameter("@RefNo", SqlDbType.VarChar) { Value = CheckForDbNull(RefNo) },
                new SqlParameter("@RequestID", SqlDbType.VarChar) { Value = CheckForDbNull(RequestID) }
            };

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<DebitTransaction>("exec [UpdateHistoricalDebitTransaction] @CardNo, @Date, @RefNo, @RequestID", param);
        }
    }
}