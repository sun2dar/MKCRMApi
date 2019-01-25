using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using CCARE.Models.MasterData;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> Request_Create(Request request)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@RequestID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.RequestID) },
                new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CustomerID) },
                new SqlParameter("@NonCustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.NonCustomerID) },
                new SqlParameter("@ForCustomer", SqlDbType.Bit) { Value = CheckForDbNull(request.ForCustomer) },
                new SqlParameter("@AccountNo", CheckForDbNull(request.AccountNo)),
                new SqlParameter("@CustomerNo", CheckForDbNull(request.CustomerNo)),
                new SqlParameter("@CardNo", CheckForDbNull(request.CardNo)),
                new SqlParameter("@LoanNo", CheckForDbNull(request.LoanNo)),
                new SqlParameter("@ContactMethod", CheckForDbNull(request.ContactMethod)),
                new SqlParameter("@CustomerAttitude", CheckForDbNull(request.CustomerAttitude)),
                new SqlParameter("@CaseOriginCode", CheckForDbNull(request.CaseOriginCode)),
                new SqlParameter("@PriorityCode", CheckForDbNull(request.PriorityCode)),
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.ProductID) },
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CategoryID) },
                new SqlParameter("@Title", CheckForDbNull(request.Title)),
                new SqlParameter("@Location", CheckForDbNull(request.Location)),
                new SqlParameter("@Address1", CheckForDbNull(request.Address1)),
                new SqlParameter("@Address2", CheckForDbNull(request.Address2)),
                new SqlParameter("@City", CheckForDbNull(request.City)),
                new SqlParameter("@PostalCode", CheckForDbNull(request.PostalCode)),
                new SqlParameter("@CommunicationPhone", CheckForDbNull(request.CommunicationPhone)),
                new SqlParameter("@CompanyName", CheckForDbNull(request.CompanyName)),
                new SqlParameter("@Lokasi", CheckForDbNull(request.Lokasi)),
                new SqlParameter("@Reason", CheckForDbNull(request.Reason)),
                new SqlParameter("@TransactionAmount", SqlDbType.Decimal) { Value = CheckForDbNull(request.TransactionAmount) },
                //new SqlParameter("@TransactionAmount", CheckForDbNull(request.TransactionAmount)),
                new SqlParameter("@TransactionTime", CheckForDbNull(request.TransactionTime)),
                new SqlParameter("@BranchID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.BranchID) },    
                new SqlParameter("@CauseID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CauseID) },
                new SqlParameter("@Currency", CheckForDbNull(request.Currency)),
                new SqlParameter("@WsIdID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.WsIdID) },
                new SqlParameter("@RefTicketNumber", SqlDbType.NVarChar) { Value = CheckForDbNull(request.RefTicketNumber) },
                new SqlParameter("@IncomingPhoneNumber", SqlDbType.NVarChar) { Value = CheckForDbNull(request.IncomingPhoneNumber) },
                new SqlParameter("@ExtraCardType", SqlDbType.Int) { Value = CheckForDbNull(request.ExtraCardType) },
                new SqlParameter("@ExtraCardNumber", SqlDbType.NVarChar) { Value = CheckForDbNull(request.ExtraCardNumber) },
                new SqlParameter("@Profession", SqlDbType.NVarChar) { Value = CheckForDbNull(request.Profession) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CreatedBy) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.BusinessUnitID) },
                new SqlParameter("@KotaID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.KotaID) },
                new SqlParameter("@SegmentationID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.SegmentationID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Request_Insert] @RequestID, @CustomerId, @NonCustomerID, @ForCustomer, @AccountNo, @CustomerNo, @CardNo, @LoanNo, @ContactMethod, @CustomerAttitude, @CaseOriginCode, @PriorityCode, @ProductID, @CategoryID, @Title, @Location, @Address1, @Address2, @City, @PostalCode, @CommunicationPhone, @CompanyName, @Lokasi, @Reason, @TransactionAmount, @TransactionTime, @BranchID, @CauseID, @Currency, @WsIdID, @RefTicketNumber, @IncomingPhoneNumber, @ExtraCardType, @ExtraCardNumber, @Profession, @ModifiedBy, @BusinessUnitID, @KotaID, @SegmentationID, @RetVal out, @Message out", param);
            string valueRes = param[40].Value.ToString();
            int retVal = (int)param[39].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Request_Edit(Request request)
        {
            CRMDb db = new CRMDb();
            Request prevRequest = db.request.Find(request.RequestID);

            var param = new SqlParameter[]{
                new SqlParameter("@RequestID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.RequestID) },
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CustomerID) },
                new SqlParameter("@NonCustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.NonCustomerID) },
                new SqlParameter("@PrevNonCustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(prevRequest.NonCustomerID) },
                new SqlParameter("@ForCustomer", request.ForCustomer),
                new SqlParameter("@PrevForCustomer", prevRequest.ForCustomer),
                new SqlParameter("@AccountNo", CheckForDbNull(request.AccountNo)),
                new SqlParameter("@PrevAccountNo", CheckForDbNull(prevRequest.AccountNo)),
                new SqlParameter("@CustomerNo", CheckForDbNull(request.CustomerNo)),
                new SqlParameter("@PrevCustomerNo", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(prevRequest.CustomerNo) },
                new SqlParameter("@CardNo", CheckForDbNull(request.CardNo)),
                new SqlParameter("@PrevCardNo", CheckForDbNull(prevRequest.CardNo)),
                new SqlParameter("@LoanNo", CheckForDbNull(request.LoanNo)),
                new SqlParameter("@PrevLoanNo", CheckForDbNull(prevRequest.LoanNo)),
                new SqlParameter("@ContactMethod", CheckForDbNull(request.ContactMethod)),
                new SqlParameter("@PrevContactMethod", CheckForDbNull(prevRequest.ContactMethod)),
                new SqlParameter("@CustomerAttitude", CheckForDbNull(request.CustomerAttitude)),
                new SqlParameter("@PrevCustomerAttitude", CheckForDbNull(prevRequest.CustomerAttitude)),
                new SqlParameter("@CaseOriginCode", CheckForDbNull(request.CaseOriginCode)),
                new SqlParameter("@PrevCaseOriginCode", CheckForDbNull(prevRequest.CaseOriginCode)),
                new SqlParameter("@PriorityCode", CheckForDbNull(request.PriorityCode)),
                new SqlParameter("@PrevPriorityCode", CheckForDbNull(prevRequest.PriorityCode)),
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.ProductID) },
                new SqlParameter("@PrevProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(prevRequest.ProductID) },
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CategoryID) },
                new SqlParameter("@PrevCategoryID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(prevRequest.CategoryID) },
                new SqlParameter("@Title", CheckForDbNull(request.Title)),
                new SqlParameter("@PrevTitle", CheckForDbNull(prevRequest.Title)),
                new SqlParameter("@Location", CheckForDbNull(request.Location)),
                new SqlParameter("@PrevLocation", CheckForDbNull(prevRequest.Location)),
                new SqlParameter("@Address1", CheckForDbNull(request.Address1)),
                new SqlParameter("@PrevAddress1", CheckForDbNull(prevRequest.Address1)),
                new SqlParameter("@Address2", CheckForDbNull(request.Address2)),
                new SqlParameter("@PrevAddress2", CheckForDbNull(prevRequest.Address2)),
                new SqlParameter("@City", CheckForDbNull(request.City)),
                new SqlParameter("@PrevCity", CheckForDbNull(prevRequest.City)),
                new SqlParameter("@PostalCode", CheckForDbNull(request.PostalCode)),
                new SqlParameter("@PrevPostalCode", CheckForDbNull(prevRequest.PostalCode)),
                new SqlParameter("@CommunicationPhone", CheckForDbNull(request.CommunicationPhone)),
                new SqlParameter("@PrevCommunicationPhone", CheckForDbNull(prevRequest.CommunicationPhone)),
                new SqlParameter("@CompanyName", CheckForDbNull(request.CompanyName)),
                new SqlParameter("@PrevCompanyName", CheckForDbNull(prevRequest.CompanyName)),
                new SqlParameter("@Lokasi", CheckForDbNull(request.Lokasi)),
                new SqlParameter("@PrevLokasi", CheckForDbNull(prevRequest.Lokasi)),
                new SqlParameter("@Reason", CheckForDbNull(request.Reason)),
                new SqlParameter("@PrevReason", CheckForDbNull(prevRequest.Reason)),
                new SqlParameter("@TransactionAmount", CheckForDbNull(request.TransactionAmount)),
                new SqlParameter("@PrevTransactionAmount", CheckForDbNull(prevRequest.TransactionAmount)),
                new SqlParameter("@TransactionTime", CheckForDbNull(request.TransactionTime)),
                new SqlParameter("@PrevTransactionTime", CheckForDbNull(prevRequest.TransactionTime)),
                new SqlParameter("@BranchID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.BranchID) },    
                new SqlParameter("@PrevBranchID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(prevRequest.BranchID) },    
                new SqlParameter("@CauseID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CauseID) },
                new SqlParameter("@PrevCauseID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(prevRequest.CauseID) },
                new SqlParameter("@Currency", CheckForDbNull(request.Currency)),
                new SqlParameter("@PrevCurrency", CheckForDbNull(prevRequest.Currency)),
                new SqlParameter("@WsIdID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.WsIdID) },
                new SqlParameter("@PrevWsIdID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(prevRequest.WsIdID) },
                new SqlParameter("@InteractionCount", CheckForDbNull(request.InteractionCount)),
                new SqlParameter("@PrevInteractionCount", CheckForDbNull(prevRequest.InteractionCount)),
                new SqlParameter("@PrevWorkgroupID", CheckForDbNull(prevRequest.WorkgroupId)),
                new SqlParameter("@PrevDueDate", CheckForDbNull(prevRequest.DueDate)),
                new SqlParameter("@PrevSource", CheckForDbNull(prevRequest.Source)),
                new SqlParameter("@PrevServiceLevel", CheckForDbNull(prevRequest.ServiceLevel)),
                new SqlParameter("@PrevBusinessUnitID", CheckForDbNull(prevRequest.BusinessUnitID)),
                new SqlParameter("@RefTicketNumber", CheckForDbNull(request.RefTicketNumber)),
                new SqlParameter("@PrevRefTicketNumber", CheckForDbNull(prevRequest.RefTicketNumber)),
                new SqlParameter("@IncomingPhoneNumber", CheckForDbNull(request.IncomingPhoneNumber)),
                new SqlParameter("@PrevIncomingPhoneNumber", CheckForDbNull(prevRequest.IncomingPhoneNumber)),
                new SqlParameter("@ExtraCardType", CheckForDbNull(request.ExtraCardType)),
                new SqlParameter("@PrevExtraCardType", CheckForDbNull(prevRequest.ExtraCardType)),
                new SqlParameter("@ExtraCardNumber", CheckForDbNull(request.ExtraCardNumber)),
                new SqlParameter("@PrevExtraCardNumber", CheckForDbNull(prevRequest.ExtraCardNumber)),
                new SqlParameter("@PrevProfession", CheckForDbNull(prevRequest.Profession)),
                new SqlParameter("@Profession", CheckForDbNull(request.Profession)),
                new SqlParameter("@CreatedOn", SqlDbType.DateTime) { Value = CheckForDbNull(request.CreatedOn) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.ModifiedBy) },
                new SqlParameter("@PrevKotaID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(prevRequest.KotaID) },
                new SqlParameter("@KotaID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.KotaID) },
                new SqlParameter("@SegmentationID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.SegmentationID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Request_Update] @RequestID, @CustomerID, @NonCustomerID, @PrevNonCustomerID, @ForCustomer, @PrevForCustomer, @AccountNo, @PrevAccountNo, @CustomerNo, @PrevCustomerNo, @CardNo, @PrevCardNo, @LoanNo, @PrevLoanNo, @ContactMethod, @PrevContactMethod, @CustomerAttitude, @PrevCustomerAttitude, @CaseOriginCode, @PrevCaseOriginCode, @PriorityCode, @PrevPriorityCode, @ProductID, @PrevProductID, @CategoryID, @PrevCategoryID, @Title, @PrevTitle, @Location, @PrevLocation, @Address1, @PrevAddress1, @Address2, @PrevAddress2, @City, @PrevCity, @PostalCode, @PrevPostalCode, @CommunicationPhone, @PrevCommunicationPhone, @CompanyName, @PrevCompanyName, @Lokasi, @PrevLokasi, @Reason, @PrevReason, @TransactionAmount, @PrevTransactionAmount, @TransactionTime, @PrevTransactionTime, @BranchID, @PrevBranchID, @CauseID, @PrevCauseID, @Currency, @PrevCurrency, @WsIdID, @PrevWsIdID, @InteractionCount, @PrevInteractionCount, @PrevWorkgroupID, @PrevDueDate, @PrevSource, @PrevServiceLevel, @PrevBusinessUnitID, @RefTicketNumber, @PrevRefTicketNumber, @IncomingPhoneNumber, @PrevIncomingPhoneNumber, @ExtraCardType, @PrevExtraCardType, @ExtraCardNumber, @PrevExtraCardNumber, @PrevProfession, @Profession, @CreatedOn, @ModifiedBy, @PrevKotaID, @KotaID, @SegmentationID, @RetVal out, @Message out", param);
            string valueRes = param[81].Value.ToString();
            int retVal = (int)param[80].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        //dibuang bu ratih
        //public KeyValuePair<int, String> Request_Assign(Request request)
        //{
        //    //queue type code : 1 group, 2 user
        //    var param = new SqlParameter[]{
        //        new SqlParameter("@RequestID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.RequestID) },
        //        new SqlParameter("@AssignToID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.OwnerID) },
        //        new SqlParameter("@QueueTypeCode", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.OwnerID) },
        //        new SqlParameter("@OwnerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.OwnerID) },
        //        new SqlParameter("@StatusCode", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.StatusCode) },
        //        new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.CreatedBy) },
        //        new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.BusinessUnitID) },
        //        new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
        //        new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
        //    };

        //    int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Request_Assign] @RequestID, @AssignToID, @QueueTypeCode, @OwnerID, @StatusCode, @ModifiedBy, @BusinessUnitID, @RetVal out, @Message out", param);
        //    string valueRes = param[8].Value.ToString();
        //    int retVal = (int)param[7].Value;
        //    return new KeyValuePair<int, string>(retVal, valueRes);
        //}

        //5 close
        //6 cancel
        public KeyValuePair<int, String> Request_SetStatus(Request request, int status, string[] additionalParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@RequestID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.RequestID) },
                new SqlParameter("@Status", status),
                new SqlParameter("@PrevStatus", CheckForDbNull(request.StatusCode)),
                new SqlParameter("@Title", request.Title),
                new SqlParameter("@Resolution", additionalParam[0]),
                new SqlParameter("@ResolutionDescription", additionalParam[1]),
                new SqlParameter("@PrevClosedOn", SqlDbType.DateTime) { Value = CheckForDbNull(request.ClosedOn) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.ModifiedBy) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.BusinessUnitID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Request_SetStatus] @RequestID, @Status, @PrevStatus, @Title, @Resolution, @ResolutionDescription, @PrevClosedOn, @ModifiedBy, @BusinessUnitID, @RetVal out, @Message out", param);
            string valueRes = param[10].Value.ToString();
            int retVal = (int)param[9].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Request_Reopen(Request request, Guid AnnotationID, string NoteText)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@RequestID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.RequestID) },
                new SqlParameter("@OldOwnerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.OwnerID) },
                new SqlParameter("@OldStatus", CheckForDbNull(request.StatusCode)),
                new SqlParameter("@OldReOpenedOn", CheckForDbNull(request.ReopenedOn)),
                new SqlParameter("@AnnotationID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(AnnotationID) },
                new SqlParameter("@NoteText", NoteText),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.ModifiedBy) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(request.BusinessUnitID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Request_Reopen] @RequestID, @OldOwnerID, @OldStatus, @OldReOpenedOn, @AnnotationID, @NoteText, @ModifiedBy, @BusinessUnitID, @RetVal out, @Message out", param);
            string valueRes = param[9].Value.ToString();
            int retVal = (int)param[8].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public ObjectResult<Annotation> Request_GetNote(Guid requestID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Annotation>("EXEC [CRM].[Request_GetNote] @ObjectID ", new SqlParameter("@ObjectID", SqlDbType.UniqueIdentifier) { Value = requestID });
        }
    }
}