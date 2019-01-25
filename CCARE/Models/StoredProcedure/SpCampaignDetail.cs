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
        public KeyValuePair<int, String> CampaignDetail_Insert(Guid ID, int Type, Guid EntityID, Guid CampaignID)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = ID },
                new SqlParameter("@TargetType", SqlDbType.Int) { Value = CheckForDbNull(Type) },
				new SqlParameter("@EntityID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(EntityID) },
                new SqlParameter("@CampaignID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(CampaignID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CampaignDetail_Insert] @ID, @TargetType, @EntityID, @CampaignID, @RetVal out, @Message out", param);

            int retVal = (int)param[4].Value;
            string valueRes = param[5].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

      public KeyValuePair<int, String> CampaignDetail_Delete(Guid ID)
      {
          var param = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = ID },
				new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

          int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[CampaignDetail_Delete] @ID, @RetVal out, @Message out", param);

          int retVal = (int)param[1].Value;
          string valueRes = param[2].Value.ToString();

          return new KeyValuePair<int, string>(retVal, valueRes);
      }
    }
}