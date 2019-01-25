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
        public KeyValuePair<int, String> Branch_Insert(Branch model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BranchID", SqlDbType.UniqueIdentifier) { Value = model.BranchID },
                new SqlParameter("@Address", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Address) },
                new SqlParameter("@AddressPriority", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.AddressPriority) },
                new SqlParameter("@BankRepHead", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.BankRepHead) },
                new SqlParameter("@BcaBizz", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.BCABizz) },
                new SqlParameter("@BcaPrioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.BCAPrioritas) },
                new SqlParameter("@BranchClass", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.BranchClass) },
                new SqlParameter("@CashOffice", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.CashOffice) },
                new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.City) },
                new SqlParameter("@Consumer", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Consumer) },
                new SqlParameter("@Fax", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@FaxPrioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.FaxPrioritas) },
                //new SqlParameter("@HeadOfMarketing", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.HeadOfMarketing) },
                new SqlParameter("@Initials", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Initials) },
                new SqlParameter("@LeaderName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.LeaderName) },
                new SqlParameter("@LeaderNamePrioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.LeaderNamePrioritas) },
                new SqlParameter("@LongDistanceCode", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.LongDistanceCode) },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@OfficeCode", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.OfficeCode) },
                new SqlParameter("@PostalCodePriority", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.PostalCodePriority) },
                new SqlParameter("@Province", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Province) },
                new SqlParameter("@Region", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Region) },
                new SqlParameter("@RegionalAddressOffice", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.RegionalAddressOffice) },
                new SqlParameter("@RegionName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.RegionName) },
                new SqlParameter("@StatusForeignPerceptions", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.StatusForeignPerceptions) },
                new SqlParameter("@StatusPerceptions", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.StatusPerceptions) },
                new SqlParameter("@Telephone1", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone1) },
                new SqlParameter("@Telephone1Prioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone1Prioritas) },
                new SqlParameter("@Telephone2", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone2) },
                new SqlParameter("@Telephone2Prioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone2Prioritas) },
                new SqlParameter("@Zipcode", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Zipcode) },
                new SqlParameter("@VSAT", SqlDbType.NVarChar, 10) { Value = CheckForDbNull(model.VSAT) },
                new SqlParameter("@SDB", SqlDbType.Bit) { Value = CheckForDbNull(model.SDB) },
                new SqlParameter("@Notes", SqlDbType.NVarChar, 150) { Value = CheckForDbNull(model.Notes) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OrganizationID", SqlDbType.UniqueIdentifier) { Value = sessionParam.OrganizationID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Branch_Insert] @BranchID, @Address, @AddressPriority, @BankRepHead,  @BcaBizz, @BcaPrioritas, @BranchClass, @CashOffice, @City, @Consumer, @Fax, @FaxPrioritas, @Initials,  @LeaderName ,@LeaderNamePrioritas , @LongDistanceCode, @Name, @OfficeCode, @PostalCodePriority, @Province, @Region, @RegionalAddressOffice, @RegionName, @StatusForeignPerceptions, @StatusPerceptions, @Telephone1, @Telephone1Prioritas,  @Telephone2 , @Telephone2Prioritas ,@Zipcode, @VSAT, @SDB, @Notes, @ModifiedBy, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[35].Value;
            string valueRes = param[36].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Branch_Update(Branch model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BranchID", SqlDbType.UniqueIdentifier) { Value = model.BranchID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.ModifiedBy) },
                new SqlParameter("@Address", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Address) },
                new SqlParameter("@AddressPriority", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.AddressPriority) },
                new SqlParameter("@BankRepHead", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.BankRepHead) },
                new SqlParameter("@BcaBizz", SqlDbType.Bit) { Value = CheckForDbNull(model.BCABizz) },
                new SqlParameter("@BcaPrioritas", SqlDbType.Bit) { Value = CheckForDbNull(model.BCAPrioritas) },
                new SqlParameter("@BranchClass", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.BranchClass) },
                new SqlParameter("@CashOffice", SqlDbType.Bit) { Value = CheckForDbNull(model.CashOffice) },
                new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.City) },
                new SqlParameter("@Consumer", SqlDbType.Bit) { Value = CheckForDbNull(model.Consumer) },
                new SqlParameter("@Fax", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Fax) },
                new SqlParameter("@FaxPrioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.FaxPrioritas) },
                //new SqlParameter("@HeadOfMarketing", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.HeadOfMarketing) },
                new SqlParameter("@Initials", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Initials) },
                new SqlParameter("@LeaderName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.LeaderName) },
                new SqlParameter("@LeaderNamePrioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.LeaderNamePrioritas) },
                new SqlParameter("@LongDistanceCode", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.LongDistanceCode) },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Name) },
                new SqlParameter("@OfficeCode", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.OfficeCode) },
                new SqlParameter("@PostalCodePriority", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.PostalCodePriority) },
                new SqlParameter("@Province", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Province) },
                new SqlParameter("@Region", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Region) },
                new SqlParameter("@RegionalAddressOffice", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.RegionalAddressOffice) },
                new SqlParameter("@RegionName", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.RegionName) },
                new SqlParameter("@StatusForeignPerceptions", SqlDbType.Bit) { Value = CheckForDbNull(model.StatusForeignPerceptions) },
                new SqlParameter("@StatusPerceptions", SqlDbType.Bit) { Value = CheckForDbNull(model.StatusPerceptions) },
                new SqlParameter("@Telephone1", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone1) },
                new SqlParameter("@Telephone1Prioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone1Prioritas) },
                new SqlParameter("@Telephone2", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone2) },
                new SqlParameter("@Telephone2Prioritas", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Telephone2Prioritas) },
                new SqlParameter("@Zipcode", SqlDbType.NVarChar, 100) { Value = CheckForDbNull(model.Zipcode) },
                new SqlParameter("@VSAT", SqlDbType.NVarChar, 10) { Value = CheckForDbNull(model.VSAT) },
                new SqlParameter("@SDB", SqlDbType.Bit) { Value = CheckForDbNull(model.SDB) },
                new SqlParameter("@Notes", SqlDbType.NVarChar, 150) { Value = CheckForDbNull(model.Notes) },
                new SqlParameter("@OrganizationID", SqlDbType.UniqueIdentifier) { Value = sessionParam.OrganizationID },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Branch_Update] @BranchID, @Address, @AddressPriority, @BankRepHead,  @BcaBizz, @BcaPrioritas, @BranchClass, @CashOffice, @City, @Consumer, @Fax, @FaxPrioritas, @Initials,  @LeaderName ,@LeaderNamePrioritas , @LongDistanceCode, @Name, @OfficeCode, @PostalCodePriority, @Province, @Region, @RegionalAddressOffice, @RegionName, @StatusForeignPerceptions, @StatusPerceptions, @Telephone1, @Telephone1Prioritas,  @Telephone2 , @Telephone2Prioritas ,@Zipcode, @VSAT, @SDB, @Notes, @ModifiedBy, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[35].Value;
            string valueRes = param[36].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Branch_ChangeStatus(Branch model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BranchID", SqlDbType.UniqueIdentifier) { Value = model.BranchID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Branch_ChangeStatus] @BranchID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> Branch_Delete(Branch model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BranchID", SqlDbType.UniqueIdentifier) { Value = model.BranchID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[Branch_Delete] @BranchID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}