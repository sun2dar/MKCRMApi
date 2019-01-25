using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using CCARE.Models.Transaction;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> LetterEntry_Insert(LetterEntry model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LetterEntryID", SqlDbType.UniqueIdentifier) { Value = model.LetterEntryID },
                new SqlParameter("@RequestID", SqlDbType.UniqueIdentifier) { Value = model.RequestID },
                new SqlParameter("@TicketNumber", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.TicketNumber) },
                new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.TemplateID) },
                new SqlParameter("@LetterDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.LetterDate) },
                new SqlParameter("@LetterNo", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.LetterNo) },
                new SqlParameter("@CC_ID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CC_ID) },
                new SqlParameter("@AttchATM", SqlDbType.Bit) { Value = CheckForDbNull(model.AttchATM) },
                new SqlParameter("@AttchDebit", SqlDbType.Bit) { Value = CheckForDbNull(model.AttchDebit) },
                new SqlParameter("@AttchCirrus", SqlDbType.Bit) { Value = CheckForDbNull(model.AttchCirrus) },
                new SqlParameter("@AttchATMSwitching", SqlDbType.Bit) { Value = CheckForDbNull(model.AttchATMSwitching) },
                new SqlParameter("@AttchMaestro", SqlDbType.Bit) { Value = CheckForDbNull(model.AttchMaestro) },
                new SqlParameter("@AttchDuration", SqlDbType.Int) { Value = CheckForDbNull(model.AttchDuration) },
                new SqlParameter("@Description1", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.Description1) },
                new SqlParameter("@Description2", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.Description2) },
                new SqlParameter("@Description3", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.Description3) },
                new SqlParameter("@Description4", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.Description4) },
                new SqlParameter("@FullName", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.FullName) },
                new SqlParameter("@AccountNo", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.AccountNo) },
                new SqlParameter("@CardNo", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.CardNo) },
                new SqlParameter("@Address1", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.Address1) },
                new SqlParameter("@Address2", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.Address2) },
                new SqlParameter("@Address3", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.Address3) },
                new SqlParameter("@City", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.City) },
                new SqlParameter("@ZipPostalCode", SqlDbType.VarChar, 100) { Value = CheckForDbNull(model.ZIPPostalCode) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BusinessUnitID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.VarChar, 200) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@LetterEntryID");
            listParams.Add("@RequestID");
            listParams.Add("@TicketNumber");
            listParams.Add("@TemplateID");
            listParams.Add("@LetterDate");
            listParams.Add("@LetterNo");
            listParams.Add("@CC_ID");
            listParams.Add("@AttchATM");
            listParams.Add("@AttchDebit");
            listParams.Add("@AttchCirrus");
            listParams.Add("@AttchATMSwitching");
            listParams.Add("@AttchMaestro");
            listParams.Add("@AttchDuration");
            listParams.Add("@Description1");
            listParams.Add("@Description2");
            listParams.Add("@Description3");
            listParams.Add("@Description4");
            listParams.Add("@FullName");
            listParams.Add("@AccountNo");
            listParams.Add("@CardNo");
            listParams.Add("@Address1");
            listParams.Add("@Address2");
            listParams.Add("@Address3");
            listParams.Add("@City");
            listParams.Add("@ZipPostalCode");
            listParams.Add("@ModifiedBy");
            listParams.Add("@BusinessUnitID");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[LetterEntry_Insert]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> LetterEntry_Delete(LetterEntry model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LetterEntryID", SqlDbType.UniqueIdentifier) { Value = model.LetterEntryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.VarChar, 200) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@LetterEntryID");
            listParams.Add("@ModifiedBy");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[LetterEntry_Delete]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> LetterEntry_ChangeStatus(LetterEntry model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LetterEntryID", SqlDbType.UniqueIdentifier) { Value = model.LetterEntryID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.VarChar, 200) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@LetterEntryID");
            listParams.Add("@ModifiedBy");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[LetterEntry_ChangeStatus]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }
    }

    public partial class HistDb
    {
        public ObjectResult<DebitTransaction> LetterEntry_GetDebitHistory(DateTime? trxDate, string cardNo, int attachDuration, int letterType, string docName)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@noKartu", SqlDbType.VarChar, 16) { Value = CheckForDbNull(cardNo) },
                new SqlParameter("@TrxDate", SqlDbType.DateTime) { Value = CheckForDbNull(trxDate) },
                new SqlParameter("@Dur", SqlDbType.Int) { Value = CheckForDbNull(attachDuration) },
                new SqlParameter("@LetterType", SqlDbType.Int) { Value = CheckForDbNull(letterType) },
                new SqlParameter("@LetterTemplate", SqlDbType.VarChar, 100) { Value = CheckForDbNull(docName) }
            };

            List<string> listParams = new List<string>();
            listParams.Add("@noKartu");
            listParams.Add("@TrxDate");
            listParams.Add("@Dur");
            listParams.Add("@LetterType");
            listParams.Add("@LetterTemplate");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("EXEC [crm].[LetterEntry_GetDebitHistory]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<DebitTransaction>(sBuilder.ToString(), param);
            return spResult;
        }

        public ObjectResult<CirrusTransaction> LetterEntry_GetCirrusHistory(DateTime? trxDate, string cardNo, int attachDuration)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@noKartu", SqlDbType.VarChar, 16) { Value = CheckForDbNull(cardNo) },
                new SqlParameter("@TrxDate", SqlDbType.DateTime) { Value = CheckForDbNull(trxDate) },
                new SqlParameter("@Dur", SqlDbType.Int) { Value = CheckForDbNull(attachDuration) }
            };

            List<string> listParams = new List<string>();
            listParams.Add("@noKartu");
            listParams.Add("@TrxDate");
            listParams.Add("@Dur");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("EXEC [crm].[LetterEntry_GetCirrusHistory]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CirrusTransaction>(sBuilder.ToString(), param);
            return spResult;
        }

        public ObjectResult<ATMTransaction> LetterEntry_GetATMHistory(DateTime? trxDate, string cardNo, int attachDuration, int letterType)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@nomorKartu", SqlDbType.VarChar, 16) { Value = CheckForDbNull(cardNo) },
                new SqlParameter("@TrxDate", SqlDbType.DateTime) { Value = CheckForDbNull(trxDate) },
                new SqlParameter("@Dur", SqlDbType.Int) { Value = CheckForDbNull(attachDuration) },
                new SqlParameter("@LetterType", SqlDbType.Int) { Value = CheckForDbNull(letterType) }
            };

            List<string> listParams = new List<string>();
            listParams.Add("@nomorKartu");
            listParams.Add("@TrxDate");
            listParams.Add("@Dur");
            listParams.Add("@LetterType");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("EXEC [crm].[LetterEntry_GetATMHistory]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ATMTransaction>(sBuilder.ToString(), param);
            return spResult;
        }

        public ObjectResult<ATMSwitchingTransaction> LetterEntry_GetATMSwitchingHistory(DateTime? trxDate, string cardNo, int attachDuration)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@noKartu", SqlDbType.VarChar, 16) { Value = CheckForDbNull(cardNo) },
                new SqlParameter("@TrxDate", SqlDbType.DateTime) { Value = CheckForDbNull(trxDate) },
                new SqlParameter("@Dur", SqlDbType.Int) { Value = CheckForDbNull(attachDuration) }
            };

            List<string> listParams = new List<string>();
            listParams.Add("@noKartu");
            listParams.Add("@TrxDate");
            listParams.Add("@Dur");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("EXEC [crm].[LetterEntry_GetATMSwitchingHistory]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));

            var spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ATMSwitchingTransaction>(sBuilder.ToString(), param);
            return spResult;
        }
    }
}