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
        public KeyValuePair<int, String> Customer_Insert(Customer customer)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customer.CustomerID) },
                new SqlParameter("@BirthDate", CheckForDbNull(customer.BirthDate)),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customer.ModifiedBy) },
                new SqlParameter("@CustomerTypeCode", CheckForDbNull(customer.CustomerTypeCode)),
                new SqlParameter("@EMailAddress1", CheckForDbNull(customer.EMailAddress1)),
                new SqlParameter("@Fax", CheckForDbNull(customer.Fax)),
                new SqlParameter("@GenderCode", CheckForDbNull(customer.GenderCode)),
                new SqlParameter("@FirstName", CheckForDbNull(customer.FirstName)),
                new SqlParameter("@LastName", CheckForDbNull(customer.LastName)),
                new SqlParameter("@FullName", CheckForDbNull(customer.FullName)),
                new SqlParameter("@MobilePhone", CheckForDbNull(customer.MobilePhone)),
                new SqlParameter("@Salutation", CheckForDbNull(customer.Salutation)),
                new SqlParameter("@Telephone1", CheckForDbNull(customer.Telephone1)),
                new SqlParameter("@Telephone2", CheckForDbNull(customer.Telephone2)),
                new SqlParameter("@AccountCode", CheckForDbNull(customer.AccountCode)),
                new SqlParameter("@AliasName", CheckForDbNull(customer.AliasName)),
                new SqlParameter("@BirthPlace", CheckForDbNull(customer.BirthPlace)),
                new SqlParameter("@CISNumber", CheckForDbNull(customer.CISNumber)),
                new SqlParameter("@CreditCardCustomerNo", CheckForDbNull(customer.CreditCardCustomerNo)),
                new SqlParameter("@IdentityNo", CheckForDbNull(customer.IdentityNo)),
                new SqlParameter("@MothersMaidenName", CheckForDbNull(customer.MothersMaidenName)),
                new SqlParameter("@BusinessUnit", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customer.BusinessUnit) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec CRM.[Customer_Insert] @CustomerID,@BirthDate,@ModifiedBy,@CustomerTypeCode,@EMailAddress1,@Fax,@GenderCode,@FirstName,@LastName,@FullName,@MobilePhone,@Salutation,@Telephone1,@Telephone2,@AccountCode,@AliasName,@BirthPlace,@CISNumber,@CreditCardCustomerNo,@IdentityNo,@MothersMaidenName,@BusinessUnit,@RetVal out,@Message out", param);
            string valueRes = param[23].Value.ToString();
            int retVal = (int)param[22].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Customer_Update(Customer customer)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customer.CustomerID) },
                new SqlParameter("@BirthDate", CheckForDbNull(customer.BirthDate)),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customer.ModifiedBy) },
                new SqlParameter("@CustomerTypeCode", CheckForDbNull(customer.CustomerTypeCode)),
                new SqlParameter("@EMailAddress1", CheckForDbNull(customer.EMailAddress1)),
                new SqlParameter("@Fax", CheckForDbNull(customer.Fax)),
                new SqlParameter("@GenderCode", CheckForDbNull(customer.GenderCode)),
                new SqlParameter("@FirstName", CheckForDbNull(customer.FirstName)),
                new SqlParameter("@LastName", CheckForDbNull(customer.LastName)),
                new SqlParameter("@FullName", CheckForDbNull(customer.FullName)),
                new SqlParameter("@MobilePhone", CheckForDbNull(customer.MobilePhone)),
                new SqlParameter("@Salutation", CheckForDbNull(customer.Salutation)),
                new SqlParameter("@Telephone1", CheckForDbNull(customer.Telephone1)),
                new SqlParameter("@Telephone2", CheckForDbNull(customer.Telephone2)),
                new SqlParameter("@AccountCode", CheckForDbNull(customer.AccountCode)),
                new SqlParameter("@AliasName", CheckForDbNull(customer.AliasName)),
                new SqlParameter("@BirthPlace", CheckForDbNull(customer.BirthPlace)),
                new SqlParameter("@CISNumber", CheckForDbNull(customer.CISNumber)),
                new SqlParameter("@CreditCardCustomerNo", CheckForDbNull(customer.CreditCardCustomerNo)),
                new SqlParameter("@IdentityNo", CheckForDbNull(customer.IdentityNo)),
                new SqlParameter("@MothersMaidenName", CheckForDbNull(customer.MothersMaidenName)),
                new SqlParameter("@BusinessUnit", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customer.BusinessUnit) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec CRM.[Customer_Update] @CustomerID,@BirthDate,@ModifiedBy,@CustomerTypeCode,@EMailAddress1,@Fax,@GenderCode,@FirstName,@LastName,@FullName,@MobilePhone,@Salutation,@Telephone1,@Telephone2,@AccountCode,@AliasName,@BirthPlace,@CISNumber,@CreditCardCustomerNo,@IdentityNo,@MothersMaidenName,@BusinessUnit,@RetVal out,@Message out", param);
            string valueRes = param[23].Value.ToString();
            int retVal = (int)param[22].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> CustomerAddress_Delete(Customer customer)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customer.CustomerID) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec CRM.[Customer_AddressDelete] @CustomerID,@RetVal out,@Message out", param);
            string valueRes = param[2].Value.ToString();
            int retVal = (int)param[1].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> CustomerAddress_Insert(CustomerAddress customeraddress)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CustomerAddressId", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customeraddress.CustomerAddressId) },
                new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customeraddress.ParentId) },
                new SqlParameter("@AddressNumber", CheckForDbNull(customeraddress.AddressNumber)),
                new SqlParameter("@AddressTypeCode", CheckForDbNull(customeraddress.AddressTypeCode)),
                new SqlParameter("@City", CheckForDbNull(customeraddress.City)),
                new SqlParameter("@Country", CheckForDbNull(customeraddress.Country)),
                new SqlParameter("@Line1", CheckForDbNull(customeraddress.Line1)),
                new SqlParameter("@Line2", CheckForDbNull(customeraddress.Line2)),
                new SqlParameter("@Line3", CheckForDbNull(customeraddress.Line3)),
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(customeraddress.ModifiedBy) },
                new SqlParameter("@PostalCode", CheckForDbNull(customeraddress.PostalCode)),
                new SqlParameter("@StateOrProvince", CheckForDbNull(customeraddress.StateOrProvince)),
                new SqlParameter("@Bca_CreditCardCustomerNo", CheckForDbNull(customeraddress.CreditCardCustomerNo)),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec CRM.[Customer_AddressInsert] @CustomerAddressId,@ParentId,@AddressNumber,@AddressTypeCode,@City,@Country,@Line1,@Line2,@Line3,@ModifiedBy,@PostalCode,@StateOrProvince,@Bca_CreditCardCustomerNo,@RetVal out,@Message out", param);
            string valueRes = param[14].Value.ToString();
            int retVal = (int)param[13].Value;
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public int Customer_GetCISNumber()
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CISNumber", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec CRM.[Customer_GetCISNumber] @CISNumber out,@RetVal out,@Message out", param);
            string valueRes = param[2].Value.ToString();
            int retVal = (int)param[1].Value;
            int CISNumber = (int)param[0].Value;
            return (CISNumber);
        }
    }
}