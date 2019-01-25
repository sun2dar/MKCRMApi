using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.SalesMarketing;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> Campaign_Insert(Campaign model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Code", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Code) },
				new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ProductID) },
                new SqlParameter("@CampaignType", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CampaignType) },
                new SqlParameter("@TargetAudience", SqlDbType.Int) { Value = CheckForDbNull(model.TargetAudience) },
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.StartDate) },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.EndDate) },
                new SqlParameter("@Budget", SqlDbType.Money) { Value = CheckForDbNull(model.Budget) },
                new SqlParameter("@ActualCost", SqlDbType.Money) { Value = CheckForDbNull(model.ActualCost) },
                new SqlParameter("@ExpectedRevenue", SqlDbType.Money) { Value = CheckForDbNull(model.ExpectedRevenue) },
                new SqlParameter("@ActualRevenue", SqlDbType.Money) { Value = CheckForDbNull(model.ActualRevenue) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Description) },
				new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
				new SqlParameter("@CreatedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.CreatedOn) },
				new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
				new SqlParameter("@ModifiedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.ModifiedOn) },
				new SqlParameter("@DeletionStateCode", SqlDbType.Int) { Value = CheckForDbNull(model.DeletionStateCode) },
                new SqlParameter("@StateCode", SqlDbType.Int) { Value = CheckForDbNull(model.StateCode) },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Campaign_Insert] @ID, @Name, @Code, @ProductID, @CampaignType, @TargetAudience, @StartDate, @EndDate, @Budget, @ActualCost, @ExpectedRevenue, @ActualRevenue, @Description, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @DeletionStateCode, @StateCode, @StatusCode, @RetVal out, @Message out", param);
            int retVal = (int)param[20].Value;
            string valueRes = param[21].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Campaign_Update(Campaign model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = model.ID },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@Code", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Code) },
				new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ProductID) },
                new SqlParameter("@CampaignType", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CampaignType) },
                new SqlParameter("@TargetAudience", SqlDbType.Int) { Value = CheckForDbNull(model.TargetAudience) },
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.StartDate) },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.EndDate) },
                new SqlParameter("@Budget", SqlDbType.Money) { Value = CheckForDbNull(model.Budget) },
                new SqlParameter("@ActualCost", SqlDbType.Money) { Value = CheckForDbNull(model.ActualCost) },
                new SqlParameter("@ExpectedRevenue", SqlDbType.Money) { Value = CheckForDbNull(model.ExpectedRevenue) },
                new SqlParameter("@ActualRevenue", SqlDbType.Money) { Value = CheckForDbNull(model.ActualRevenue) },
                new SqlParameter("@Description", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Description) },
				new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.CreatedBy) },
				new SqlParameter("@CreatedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.CreatedOn) },
				new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
				new SqlParameter("@ModifiedOn", SqlDbType.DateTime) { Value = CheckForDbNull(model.ModifiedOn) },
				new SqlParameter("@DeletionStateCode", SqlDbType.Int) { Value = CheckForDbNull(model.DeletionStateCode) },
                new SqlParameter("@StateCode", SqlDbType.Int) { Value = CheckForDbNull(model.StateCode) },
                new SqlParameter("@StatusCode", SqlDbType.Int) { Value = CheckForDbNull(model.StatusCode) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };


            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Campaign_Update] @ID, @Name, @Code, @ProductID, @CampaignType, @TargetAudience, @StartDate, @EndDate, @Budget, @ActualCost, @ExpectedRevenue, @ActualRevenue, @Description, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @DeletionStateCode, @StateCode, @StatusCode, @RetVal out, @Message out", param);

            int retVal = (int)param[20].Value;
            string valueRes = param[21].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Campaign_Delete(Guid ID, Guid ModifiedBy)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = ID },
				new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(ModifiedBy) },
				new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Campaign_Delete] @ID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}