using CCARE.Models.SalesMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> Lead_Insert(Lead model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LeadID", SqlDbType.UniqueIdentifier) { Value = model.LeadID },
                new SqlParameter("@Topic", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Topic) },
                new SqlParameter("@ContactID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ContactId) },
                new SqlParameter("@CustomerTypeCode", SqlDbType.Int) { Value = CheckForDbNull(0) },
                new SqlParameter("@Salutation", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Salutation) },
                new SqlParameter("@GenderCode", SqlDbType.Int) { Value = CheckForDbNull(model.GenderCode) },
                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Fullname) },
                new SqlParameter("@Birthdate", SqlDbType.DateTime) { Value = CheckForDbNull(model.BirthDate) },
                new SqlParameter("@MobilePhone", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.MobilePhone) },
                new SqlParameter("@Telephone", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Telephone) },
                new SqlParameter("@Fax", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@EMailAddress", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.EmailAddress) },
                new SqlParameter("@JobTitle", SqlDbType.NVarChar, 50) { Value = CheckForDbNull("jobtitle") },
                new SqlParameter("@Firstname", SqlDbType.NVarChar, 100) { Value = CheckForDbNull("firstname") },
                new SqlParameter("@Lastname", SqlDbType.NVarChar, 60) { Value = CheckForDbNull("lastname") },
				new SqlParameter("@Address", SqlDbType.NVarChar, 250) { Value = CheckForDbNull(model.Address) },
                new SqlParameter("@City", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.City) },
                new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Country) },
                new SqlParameter("@Company", SqlDbType.NVarChar, 100) { Value = CheckForDbNull("company") },
                new SqlParameter("@AccountID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.AccountId) },
                new SqlParameter("@CampaignID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CampaignId) },
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ProductId) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(null) },
                new SqlParameter("@OwnerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Lead_Insert] @LeadID, @Topic, @ContactID, @CustomerTypeCode, @Salutation, @GenderCode, @FullName, @BirthDate, @MobilePhone, @Telephone, @Fax, @EMailAddress, @JobTitle, @Firstname, @Lastname, @Address, @City, @Country, @Company, @AccountID, @CampaignID, @ProductID, @BusinessUnitID, @OwnerID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[25].Value;
            string valueRes = param[26].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Lead_Update(Lead model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LeadID", SqlDbType.UniqueIdentifier) { Value = model.LeadID },
                new SqlParameter("@Topic", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Topic) },
                new SqlParameter("@ContactID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ContactId) },
                new SqlParameter("@CustomerTypeCode", SqlDbType.Int) { Value = CheckForDbNull(0) },
                new SqlParameter("@Salutation", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Salutation) },
                new SqlParameter("@GenderCode", SqlDbType.Int) { Value = CheckForDbNull(model.GenderCode) },
                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Fullname) },
                new SqlParameter("@Birthdate", SqlDbType.DateTime) { Value = CheckForDbNull(model.BirthDate) },
                new SqlParameter("@MobilePhone", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.MobilePhone) },
                new SqlParameter("@Telephone", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Telephone) },
                new SqlParameter("@Fax", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@EMailAddress", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.EmailAddress) },
                new SqlParameter("@JobTitle", SqlDbType.NVarChar, 50) { Value = CheckForDbNull("jobtitle") },
                new SqlParameter("@Firstname", SqlDbType.NVarChar, 100) { Value = CheckForDbNull("firstname") },
                new SqlParameter("@Lastname", SqlDbType.NVarChar, 60) { Value = CheckForDbNull("lastname") },
                new SqlParameter("@Address", SqlDbType.NVarChar, 250) { Value = CheckForDbNull(model.Address) },
                new SqlParameter("@City", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.City) },
                new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Country) },
                new SqlParameter("@Company", SqlDbType.NVarChar, 100) { Value = CheckForDbNull("company") },
                new SqlParameter("@AccountID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.AccountId) },
                new SqlParameter("@CampaignID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CampaignId) },
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ProductId) },
                new SqlParameter("@BusinessUnitID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(null) },
                new SqlParameter("@OwnerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Lead_Update] @LeadID, @Topic, @ContactID, @CustomerTypeCode, @Salutation, @GenderCode, @FullName, @BirthDate, @MobilePhone, @Telephone, @Fax, @EMailAddress, @JobTitle, @Firstname, @Lastname, @Address, @City, @Country, @Company, @AccountID, @CampaignID, @ProductID, @BusinessUnitID, @OwnerID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[25].Value;
            string valueRes = param[26].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        //        var param = new SqlParameter[]{
        //            new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.LeadID },
        //            new SqlParameter("@Topic", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Topic) },
        //            new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Fullname) },
        //new SqlParameter("@Company", SqlDbType.NVarChar, 100) { Value = CheckForDbNull("company") },
        //            new SqlParameter("@JobTitle", SqlDbType.NVarChar, 50) { Value = CheckForDbNull("jobtitle") },
        //            new SqlParameter("@Firstname", SqlDbType.NVarChar, 100) { Value = CheckForDbNull("firstname") },
        //            new SqlParameter("@Lastname", SqlDbType.NVarChar, 60) { Value = CheckForDbNull("firstname") },
        //new SqlParameter("@Birthdate", SqlDbType.DateTime) { Value = CheckForDbNull(model.BirthDate) },
        //            new SqlParameter("@GenderCode", SqlDbType.Int) { Value = CheckForDbNull(model.GenderCode) },
        //new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.EmailAddress) },
        //new SqlParameter("@Address", SqlDbType.NVarChar, 250) { Value = CheckForDbNull(model.Address) },
        //            new SqlParameter("@City", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.City) },
        //            new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Country) },
        //            new SqlParameter("@Telephone", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.Telephone) },
        //            new SqlParameter("@Handphone", SqlDbType.NVarChar, 50) { Value = CheckForDbNull(model.MobilePhone) },
        //new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
        //new SqlParameter("@CreatedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.CreatedOn) },
        //new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
        //new SqlParameter("@ModifiedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.ModifiedOn) },
        //new SqlParameter("@DeletionStateCode", SqlDbType.Int) { Value = CheckForDbNull(model.DeletionStateCode) },
        //            new SqlParameter("@StateCode", SqlDbType.Int) { Value = CheckForDbNull(model.StateCode) },
        //            new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
        //            new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
        //            new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
        //        };

        //        int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Lead_Update] @ID, @Topic, @Name, @Company, @JobTitle, @Firstname, @Lastname, @Birthdate, @GenderCode, @Email, @Address, @City, @Country, @Telephone, @Handphone, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @DeletionStateCode, @StateCode, @StatusCode, @RetVal out, @Message out", param);

        //        int retVal = (int)param[22].Value;
        //        string valueRes = param[23].Value.ToString();

        //        return new KeyValuePair<int, string>(retVal, valueRes);


        //Change Status : value di table CRM.Workflow
        public KeyValuePair<int, String> Lead_ChangeStatus(Guid ID, int StatusCode, Guid ModifiedBy)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@LeadID", SqlDbType.UniqueIdentifier) { Value = ID },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(StatusCode) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(ModifiedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Lead_ChangeStatus] @LeadID, @StatusCode, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        //Change State : 0=Active, 1=Inactive, 3=Disqualify
        public KeyValuePair<int, String> Lead_ChangeState(Guid ID, int StateCode, Guid ModifiedBy)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = ID },
                new SqlParameter("@StateCode", SqlDbType.Int) { Value = CheckForDbNull(StateCode) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(ModifiedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Lead_ChangeState] @ID, @StateCode, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        //Qualify lead to become customer. Change state to 2 = Qualify
        public KeyValuePair<int, String> Lead_Qualify(Guid ID, Guid CustomerID, Guid CreatedBy)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = ID },
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(CustomerID) },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(CreatedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Lead_Qualify] @ID, @CustomerID, @CreatedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}