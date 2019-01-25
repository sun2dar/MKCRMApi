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
        public KeyValuePair<int, String> Annotation_Insert(Annotation model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ObjectID", SqlDbType.UniqueIdentifier) { Value = model.ObjectID },
                new SqlParameter("@AnnotationID", SqlDbType.UniqueIdentifier) { Value = model.AnnotationID },
                new SqlParameter("@ObjectTypeCode", SqlDbType.Int) { Value = model.ObjectTypeCode },
                new SqlParameter("@MimeType", SqlDbType.NVarChar, 256) { Value = model.MimeType },
                new SqlParameter("@IsDocument", SqlDbType.Bit) { Value = model.IsDocument },
                new SqlParameter("@Subject", SqlDbType.NVarChar, 100) { Value = model.Subject },
                new SqlParameter("@NoteText", SqlDbType.NText) { Value = model.NoteText },
                new SqlParameter("@DocumentBody", SqlDbType.NText) { Value = model.DocumentBody },
                new SqlParameter("@FileSize", SqlDbType.Int) { Value = model.FileSize },
                new SqlParameter("@FileName", SqlDbType.NVarChar, 255) { Value = model.FileName },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = model.BusinessUnitID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedByID },
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@ObjectID");
            listParams.Add("@AnnotationID");
            listParams.Add("@ObjectTypeCode");
            listParams.Add("@MimeType");
            listParams.Add("@IsDocument");
            listParams.Add("@Subject");
            listParams.Add("@NoteText");
            listParams.Add("@DocumentBody");
            listParams.Add("@FileSize");
            listParams.Add("@FileName");
            listParams.Add("@BusinessUnitID");
            listParams.Add("@ModifiedBy");
            listParams.Add("@Message out");
            listParams.Add("@RetVal out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[Annotation_Insert]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 1].Value;
            string message = (string)param[listParams.Count - 2].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> Annotation_Update(Annotation model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AnnotationID", SqlDbType.UniqueIdentifier) { Value = model.AnnotationID },
                new SqlParameter("@ObjectTypeCode", SqlDbType.Int) { Value = model.ObjectTypeCode },
                new SqlParameter("@MimeType", SqlDbType.NVarChar, 256) { Value = model.MimeType },
                new SqlParameter("@IsDocument", SqlDbType.Bit) { Value = model.IsDocument },
                new SqlParameter("@Subject", SqlDbType.NVarChar, 100) { Value = model.Subject },
                new SqlParameter("@NoteText", SqlDbType.NText) { Value = model.NoteText },
                new SqlParameter("@DocumentBody", SqlDbType.NText) { Value = model.DocumentBody },
                new SqlParameter("@FileSize", SqlDbType.Int) { Value = model.FileSize },
                new SqlParameter("@FileName", SqlDbType.NVarChar, 255) { Value = model.FileName },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedByID },
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@AnnotationID");
            listParams.Add("@ObjectTypeCode");
            listParams.Add("@MimeType");
            listParams.Add("@IsDocument");
            listParams.Add("@Subject");
            listParams.Add("@NoteText");
            listParams.Add("@DocumentBody");
            listParams.Add("@FileSize");
            listParams.Add("@FileName");
            listParams.Add("@ModifiedBy");
            listParams.Add("@Message out");
            listParams.Add("@RetVal out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[Annotation_Update]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 1].Value;
            string message = (string)param[listParams.Count - 2].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }

        public KeyValuePair<int, String> Annotation_Delete(Annotation model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AnnotationID", SqlDbType.UniqueIdentifier) { Value = model.AnnotationID },
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            List<string> listParams = new List<string>();
            listParams.Add("@AnnotationID");
            listParams.Add("@Message out");
            listParams.Add("@RetVal out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[Annotation_Delete]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);

            int retVal = (int)param[listParams.Count - 1].Value;
            string message = (string)param[listParams.Count - 2].Value;

            return new KeyValuePair<int, string>(retVal, message);
        }
    }
}