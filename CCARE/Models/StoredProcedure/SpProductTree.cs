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
        //SP PRODUCT TOP
        //==========================================
        public ObjectResult<ProductTree> spProduct_Top()
        {
            ObjectResult<ProductTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProductTree>("exec [CRM].[Product_Tree_p]");
            return spResult;
        }

        //SP PRODUCT BY NAME
        //==========================================
        public ObjectResult<ProductTree> spProduct_byName(string searchText)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = searchText }
          };

            ObjectResult<ProductTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProductTree>("exec [CRM].[Product_ByProductName_p] @ProductName", param);
            return spResult;
        }

        //SP PRODUCT BY CATEGORY
        //==========================================
        public ObjectResult<ProductTree> spProduct_byCategoryId(Guid categoryid)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = categoryid }
          };

            ObjectResult<ProductTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProductTree>("exec [CRM].[Product_byCategoryID_p] @CategoryID", param);
            return spResult;
        }

        //SP PRODUCT BY CATEGORY BY NAME
        //==========================================
        public ObjectResult<ProductTree> spProduct_byCategoryId_byName(Guid categoryid, string searchText)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CategoryID", SqlDbType.UniqueIdentifier) { Value = categoryid },
                new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = searchText }
            };

            ObjectResult<ProductTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProductTree>("exec [CRM].[Product_byCategoryID_byProductName_p] @CategoryID, @ProductName", param);
            return spResult;
        }
    }
}