using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> NonCustomer_Insert(NonCustomer model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@nonCustomerID", SqlDbType.UniqueIdentifier) { Value = model.NonCustomerID },
                new SqlParameter("@BirthDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.BirthDate) },
                new SqlParameter("@Salutation", CheckForDbNull(model.Salutation)),
                new SqlParameter("@Firstname", CheckForDbNull(model.FirstName)),
                new SqlParameter("@LastName", CheckForDbNull(model.LastName)),
                new SqlParameter("@fullname", CheckForDbNull(model.FullName)),
                new SqlParameter("@Phone", CheckForDbNull(model.PhoneNo)),
                new SqlParameter("@city", CheckForDbNull(model.City)),
                new SqlParameter("@Email", CheckForDbNull(model.Email)),
                new SqlParameter("@zipcode", CheckForDbNull(model.Zipcode)),
                new SqlParameter("@HandphoneNo", CheckForDbNull(model.HandphoneNo)),
                new SqlParameter("@address3", CheckForDbNull(model.Address3)),
                new SqlParameter("@address2", CheckForDbNull(model.Address2)),
                new SqlParameter("@address1", CheckForDbNull(model.Address1)),
                new SqlParameter("@country", CheckForDbNull(model.Country)),
                new SqlParameter("@AddressType", SqlDbType.Int) { Value = CheckForDbNull(model.AddressType) },
                new SqlParameter("@Fax", CheckForDbNull(model.FaxNo)),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@BusinessUnit", SqlDbType.UniqueIdentifier) { Value = model.BusinessUnitID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[NonCustomer_Insert] @nonCustomerID, @BirthDate, @Salutation, @Firstname, @LastName, @fullname, @Phone, @city, @Email, @zipcode, @HandphoneNo, @address3, @address2, @address1, @country, @AddressType, @Fax, @ModifiedBy, @BusinessUnit, @RetVal out, @Message out", param);

            int retVal = (int)param[19].Value;
            string valueRes = param[20].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> NonCustomer_Update(NonCustomer model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@nonCustomerID", SqlDbType.UniqueIdentifier) { Value = model.NonCustomerID },
                new SqlParameter("@BirthDate", SqlDbType.DateTime) { Value = CheckForDbNull(model.BirthDate) },
                new SqlParameter("@Salutation", CheckForDbNull(model.Salutation)),
                new SqlParameter("@Firstname", CheckForDbNull(model.FirstName)),
                new SqlParameter("@LastName", CheckForDbNull(model.LastName)),
                new SqlParameter("@fullname", CheckForDbNull(model.FullName)),
                new SqlParameter("@Phone", CheckForDbNull(model.PhoneNo)),
                new SqlParameter("@city", CheckForDbNull(model.City)),
                new SqlParameter("@Email", CheckForDbNull(model.Email)),
                new SqlParameter("@zipcode", CheckForDbNull(model.Zipcode)),
                new SqlParameter("@HandphoneNo", CheckForDbNull(model.HandphoneNo)),
                new SqlParameter("@address3", CheckForDbNull(model.Address3)),
                new SqlParameter("@address2", CheckForDbNull(model.Address2)),
                new SqlParameter("@address1", CheckForDbNull(model.Address1)),
                new SqlParameter("@country", CheckForDbNull(model.Country)),
                new SqlParameter("@AddressType", SqlDbType.Int) { Value = CheckForDbNull(model.AddressType) },
                new SqlParameter("@Fax", CheckForDbNull(model.FaxNo)),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@BusinessUnit", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.BusinessUnitID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[NonCustomer_Update] @nonCustomerID, @BirthDate, @Salutation, @Firstname, @LastName, @fullname, @Phone, @city, @Email, @zipcode, @HandphoneNo, @address3, @address2, @address1, @country, @AddressType, @Fax, @ModifiedBy, @BusinessUnit, @RetVal out, @Message out", param);

            int retVal = (int)param[19].Value;
            string valueRes = param[20].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> NonCustomer_ChangeStatus(NonCustomer model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@NonCustomerID", SqlDbType.UniqueIdentifier) { Value = model.NonCustomerID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[NonCustomer_ChangeStatus] @NonCustomerID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> NonCustomer_Delete(NonCustomer model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@NonCustomerID", SqlDbType.UniqueIdentifier) { Value = model.NonCustomerID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[NonCustomer_Delete] @NonCustomerID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}