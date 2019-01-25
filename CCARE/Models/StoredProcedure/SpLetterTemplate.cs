using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> LetterTemplate_Insert(LetterTemplate model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LetterTemplateId", SqlDbType.UniqueIdentifier) { Value = model.LetterTemplateId },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OwningBusinessUnit", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.OwningBusinessUnit) },
                new SqlParameter("@Description1", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description1) },
                new SqlParameter("@Description2", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description2) },
                new SqlParameter("@Description3", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description3) },
                new SqlParameter("@Description4", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description4) },
                new SqlParameter("@Name", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Type", SqlDbType.Int) { Value = CheckForDbNull(model.Type) },
                new SqlParameter("@Language", SqlDbType.Int) { Value = CheckForDbNull(model.Language) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.VarChar, 200) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@LetterTemplateId");
            listParams.Add("@ModifiedBy");
            listParams.Add("@OwningBusinessUnit");
            listParams.Add("@Description1");
            listParams.Add("@Description2");
            listParams.Add("@Description3");
            listParams.Add("@Description4");
            listParams.Add("@Name");
            listParams.Add("@Type");
            listParams.Add("@Language");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[LetterTemplate_Insert]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> LetterTemplate_Update(LetterTemplate model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LetterTemplateId", SqlDbType.UniqueIdentifier) { Value = model.LetterTemplateId },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@Description1", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description1) },
                new SqlParameter("@Description2", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description2) },
                new SqlParameter("@Description3", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description3) },
                new SqlParameter("@Description4", SqlDbType.VarChar, 200) { Value = CheckForDbNull(model.Description4) },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
                new SqlParameter("@Name", SqlDbType.VarChar, 200) { Value = (model.Name) },
                new SqlParameter("@Type", SqlDbType.Int) { Value = (model.Type) },
                new SqlParameter("@Language", SqlDbType.Int) { Value = (model.Language) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.VarChar, 200) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@LetterTemplateId");
            listParams.Add("@ModifiedBy");
            listParams.Add("@Description1");
            listParams.Add("@Description2");
            listParams.Add("@Description3");
            listParams.Add("@Description4");
            listParams.Add("@StatusCode");
            listParams.Add("@Name");
            listParams.Add("@Type");
            listParams.Add("@Language");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");
            
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[LetterTemplate_Update]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> LetterTemplate_Delete(LetterTemplate model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LetterTemplateId", SqlDbType.UniqueIdentifier) { Value = model.LetterTemplateId },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.VarChar, 200) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@LetterTemplateId");
            listParams.Add("@ModifiedBy");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[LetterTemplate_Delete]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> LetterTemplate_ChangeStatus(LetterTemplate model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LetterTemplateId", SqlDbType.UniqueIdentifier) { Value = model.LetterTemplateId },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.VarChar, 200) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@LetterTemplateId");
            listParams.Add("@ModifiedBy");
            listParams.Add("@RetVal out");
            listParams.Add("@Message out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[LetterTemplate_ChangeStatus]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 2].Value;
            string message = (string)param[listParams.Count - 1].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }
    }
}