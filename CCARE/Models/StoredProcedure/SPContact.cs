using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        // SP for Menu Action
        public KeyValuePair<int, String> Contact_Insert(Contact model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ContactID", SqlDbType.UniqueIdentifier) { Value = model.ContactID },
                new SqlParameter("@CustomerTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.CustomerTypeCode) },
                new SqlParameter("@Salutation", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Salutation) },
                new SqlParameter("@GenderCode", SqlDbType.Int) { Value = CheckForDbNull(model.GenderCode) },
                new SqlParameter("@FullName", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.FullName) },
                new SqlParameter("@Birthdate", SqlDbType.DateTime) { Value = CheckForDbNull(model.BirthDate) },
                new SqlParameter("@MobilePhone", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.MobilePhone) },
                new SqlParameter("@Telephone1", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Telephone1) },
                new SqlParameter("@Telephone2", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Telephone2) },
                new SqlParameter("@Fax", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@EMailAddress1", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@BusinessUnit", SqlDbType.Int) { Value = CheckForDbNull(model.BusinessUnit) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                //new SqlParameter("@OrganizationID", CheckForDbNull(sessionParam.OrganizationID)),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Contact_Insert] @ContactID , @CustomerTypeCode ,@Salutation ,@GenderCode ,@FullName ,@BirthDate ,@MobilePhone ,@Telephone1 ,@Telephone2 ,@Fax ,@EMailAddress1 ,@BusinessUnit ,@ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[13].Value;
            string valueRes = param[14].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Contact_Update(Contact model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ContactID", SqlDbType.UniqueIdentifier) { Value = model.ContactID },
                new SqlParameter("@CustomerTypeCode", SqlDbType.Int) { Value = CheckForDbNull(model.CustomerTypeCode) },
                new SqlParameter("@Salutation", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Salutation) },
                new SqlParameter("@GenderCode", SqlDbType.Int) { Value = CheckForDbNull(model.GenderCode) },
                new SqlParameter("@FullName", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.FullName) },
                new SqlParameter("@Birthdate", SqlDbType.DateTime) { Value = CheckForDbNull(model.BirthDate) },
                new SqlParameter("@MobilePhone", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.MobilePhone) },
                new SqlParameter("@Telephone1", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Telephone1) },
                new SqlParameter("@Telephone2", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Telephone2) },
                new SqlParameter("@Fax", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@EMailAddress1", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@BusinessUnit", SqlDbType.Int) { Value = CheckForDbNull(model.BusinessUnit) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                //new SqlParameter("@OrganizationID", CheckForDbNull(sessionParam.OrganizationID)),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Contact_Update] @ContactID , @CustomerTypeCode ,@Salutation ,@GenderCode ,@FullName ,@BirthDate ,@MobilePhone ,@Telephone1 ,@Telephone2 ,@Fax ,@EMailAddress1 ,@BusinessUnit ,@ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[13].Value;
            string valueRes = param[14].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}