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
        public KeyValuePair<int, String> spBusinessUnit_ChangeStatus(BusinessUnit model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BusinessUnitID",model.BusinessUnitId),
                new SqlParameter("@ModifiedBy",model.ModifiedByID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}         
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BusinessUnit_ChangeStatus] @BusinessUnitID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);

        }

        public KeyValuePair<int, String> spBusinessUnit_Delete(BusinessUnit model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@BusinessUnitID",model.BusinessUnitId),
                new SqlParameter("@ModifiedBy",model.ModifiedByID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}         
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BusinessUnit_Delete] @BusinessUnitID, @ModifiedBy, @RetVal out, @Message out", param);
            
            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();
            
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spBusinessUnit_Update(BusinessUnit model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@BusinessUnitId",model.BusinessUnitId),
                new SqlParameter ("@Name",model.Name),
                new SqlParameter ("@DivisionName",CheckForDbNull(model.Division)),
                new SqlParameter ("@WebSiteURL",CheckForDbNull(model.WebSite)),
                new SqlParameter ("@MainPhone",CheckForDbNull(model.MainPhone)),
                new SqlParameter ("@OtherPhone",CheckForDbNull(model.OtherPhone)),
                new SqlParameter ("@Fax",CheckForDbNull(model.Fax)),
                new SqlParameter ("@EMailAddress",CheckForDbNull(model.eMail)),
                new SqlParameter ("@ModifiedBy",SqlDbType.UniqueIdentifier) { Value = model.ModifiedByID},
                new SqlParameter ("@OrganizationId", sessionParam.OrganizationID),
                new SqlParameter ("@Bill_City",CheckForDbNull(model.Bill_City)),
                new SqlParameter ("@Bill_Country",CheckForDbNull(model.Bill_Country)),
                new SqlParameter ("@Bill_Line1",CheckForDbNull(model.Bill_Street1)),
                new SqlParameter ("@Bill_Line2",CheckForDbNull(model.Bill_Street2)),
                new SqlParameter ("@Bill_Line3",CheckForDbNull(model.Bill_Street3)),
                new SqlParameter ("@Bill_PostalCode",CheckForDbNull(model.Bill_PostalCode)),
                new SqlParameter ("@Bill_StateOrProvince",CheckForDbNull(model.Bill_StateOrProvince)),
                new SqlParameter ("@Ship_City",CheckForDbNull(model.Ship_City)),
                new SqlParameter ("@Ship_Country",CheckForDbNull(model.Ship_Country)),
                new SqlParameter ("@Ship_Line1",CheckForDbNull(model.Ship_Street1)),
                new SqlParameter ("@Ship_Line2",CheckForDbNull(model.Ship_Street2)),
                new SqlParameter ("@Ship_Line3",CheckForDbNull(model.Ship_Street3)),
                new SqlParameter ("@Ship_PostalCode",CheckForDbNull(model.Ship_PostalCode)),
                new SqlParameter ("@Ship_StateOrProvince",CheckForDbNull(model.Ship_StateOrProvince)),
                new SqlParameter ("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter ("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BusinessUnit_Update] @BusinessUnitId,@Name,@DivisionName,@WebSiteURL,@MainPhone,@OtherPhone,@Fax,@EMailAddress,@ModifiedBy,@OrganizationId,@Bill_City,@Bill_Country,@Bill_Line1,@Bill_Line2,@Bill_Line3,@Bill_PostalCode,@Bill_StateOrProvince,@Ship_City,@Ship_Country,@Ship_Line1,@Ship_Line2,@Ship_Line3,@Ship_PostalCode,@Ship_StateOrProvince,@RetVal out, @Message out", param);
            
            int retVal = (int)param[24].Value;
            string valueRes = param[25].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }


        public KeyValuePair<int, String> spBusinessUnit_Insert(BusinessUnit model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@BusinessUnitId",SqlDbType.UniqueIdentifier) {Value = model.BusinessUnitId},
                new SqlParameter ("@Name",model.Name),
                new SqlParameter ("@DivisionName",CheckForDbNull( model.Division)),
                new SqlParameter ("@ParentBusinessUnitId", model.ParentBusinessUnitId),
                new SqlParameter ("@WebSiteURL",CheckForDbNull(model.WebSite)),
                new SqlParameter ("@MainPhone",CheckForDbNull(model.MainPhone)),
                new SqlParameter ("@OtherPhone",CheckForDbNull(model.OtherPhone)),
                new SqlParameter ("@Fax",CheckForDbNull(model.Fax)),
                new SqlParameter ("@EMailAddress",CheckForDbNull(model.eMail)),
                new SqlParameter ("@ModifiedBy",SqlDbType.UniqueIdentifier) { Value = model.ModifiedByID},
                new SqlParameter ("@OrganizationId", sessionParam.OrganizationID),
                new SqlParameter ("@Bill_City",CheckForDbNull(model.Bill_City)),
                new SqlParameter ("@Bill_Country",CheckForDbNull(model.Bill_Country)),
                new SqlParameter ("@Bill_Line1",CheckForDbNull(model.Bill_Street1)),
                new SqlParameter ("@Bill_Line2",CheckForDbNull(model.Bill_Street2)),
                new SqlParameter ("@Bill_Line3",CheckForDbNull(model.Bill_Street3)),
                new SqlParameter ("@Bill_PostalCode",CheckForDbNull(model.Bill_PostalCode)),
                new SqlParameter ("@Bill_StateOrProvince",CheckForDbNull(model.Bill_StateOrProvince)),
                new SqlParameter ("@Ship_City",CheckForDbNull(model.Ship_City)),
                new SqlParameter ("@Ship_Country",CheckForDbNull(model.Ship_Country)),
                new SqlParameter ("@Ship_Line1",CheckForDbNull(model.Ship_Street1)),
                new SqlParameter ("@Ship_Line2",CheckForDbNull(model.Ship_Street2)),
                new SqlParameter ("@Ship_Line3",CheckForDbNull(model.Ship_Street3)),
                new SqlParameter ("@Ship_PostalCode",CheckForDbNull(model.Ship_PostalCode)),
                new SqlParameter ("@Ship_StateOrProvince",CheckForDbNull(model.Ship_StateOrProvince)),
                new SqlParameter ("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter ("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}

            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BusinessUnit_Insert] @BusinessUnitId,@Name,@DivisionName,@ParentBusinessUnitId,@WebSiteURL,@MainPhone,@OtherPhone,@Fax,@EMailAddress,@ModifiedBy,@OrganizationId,@Bill_City,@Bill_Country,@Bill_Line1,@Bill_Line2,@Bill_Line3,@Bill_PostalCode,@Bill_StateOrProvince,@Ship_City,@Ship_Country,@Ship_Line1,@Ship_Line2,@Ship_Line3,@Ship_PostalCode,@Ship_StateOrProvince,@RetVal out, @Message out", param);
            
            int retVal = (int)param[25].Value;
            string valueRes = param[26].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spBusinessUnit_ChParent(BusinessUnit model)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@BusinessUnitId",model.BusinessUnitId),
                new SqlParameter ("@ModifiedBy",SqlDbType.UniqueIdentifier) { Value = model.ModifiedByID},
                new SqlParameter ("@ParentBusinessUnitId",model.ParentBusinessUnitId),
                new SqlParameter ("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter ("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}

            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BusinessUnit_ChangeParent] @BusinessUnitId,@ModifiedBy,@ParentBusinessUnitId,@RetVal out,@Message out", param);
            
            int retVal = (int)param[3].Value;
            string valueRes = param[4].Value.ToString();
            
            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> spBusinessUnit_DetectLoop(BusinessUnit model)
        {
            var param = new SqlParameter[]{
                new SqlParameter ("@ParentId",model.ParentBusinessUnitId),
                new SqlParameter ("@ChildId",model.BusinessUnitId),
                new SqlParameter ("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter ("@Message", SqlDbType.NVarChar, 200) {Direction = ParameterDirection.Output}

            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[BusinessUnit_DetectLoop] @ParentId,@ChildId,@RetVal out,@Message out", param);
            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();
            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}