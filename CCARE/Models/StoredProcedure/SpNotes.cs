using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public class NotesTemp
        {
            [Key]
            public Guid AnnotationID { get; set; }

            public int DeletionStateCode { get; set; }

            [DataType(DataType.MultilineText)]
            public string NoteText { get; set; }

            public Guid ObjectID { get; set; }

            public Guid CreatedBy { get; set; }

            [StringLength(160)]
            public string CreatedByName { get; set; }


            [StringLength(160)]
            public string ModifiedByName { get; set; }

            public DateTime? CreatedOn { get; set; }

            public Guid OwningBusinessUnit { get; set; }

            public Guid ModifiedBy { get; set; }

            public DateTime ModifiedOn { get; set; }

            [StringLength(500)]
            public string Subject { get; set; }

            public int? ObjectTypeCode { get; set; }   
        }



        public KeyValuePair<int, String> SpInsertNote(Annotation notes, Guid ObjectID)
        {
            string ObjectType = "112";

            var param = new SqlParameter[]{
                new SqlParameter("@ModifiedByID", notes.ModifiedByID),
                new SqlParameter("@ObjectID", ObjectID),
                new SqlParameter("@ObjectType", ObjectType),
                new SqlParameter("@AnnotationID", notes.AnnotationID),
                new SqlParameter("@BusinessUnitID", notes.BusinessUnitID),
                new SqlParameter("@NoteText", notes.NoteText),
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec CRM.[Note_Insert] @ModifiedByID, @ObjectID, @ObjectType, @AnnotationID, @BusinessUnitID, @NoteText, @Message out, @RetVal out", param);

            string valueRes = param[6].Value.ToString();
            int retVal = (int)param[7].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> SpUpdateNote(Annotation notes)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ModifiedByID", notes.ModifiedByID),
                new SqlParameter("@AnnotationID", notes.AnnotationID),
                new SqlParameter("@ObjectID", notes.ObjectID),
                new SqlParameter("@NoteText", notes.NoteText.Trim()),
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec CRM.[Note_Update] @ModifiedByID, @AnnotationID, @NoteText, @ObjectID, @Message out, @RetVal out", param);

            string valueRes = param[4].Value.ToString();
            int retVal = (int)param[5].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public ObjectResult<NotesTemp> SpGetNotes(Guid ObjectID)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ObjectID", ObjectID)
            };

            ObjectResult<NotesTemp> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<NotesTemp>("exec [CRM].[Annotation_GetContent] @ObjectID", param);
            return spResult;
        }
    }
}