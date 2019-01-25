using System;
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
        //SP CATEGORY TOP
        //==========================================
        public ObjectResult<CategoryTree> spCategory_Top()
        {
            ObjectResult<CategoryTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CategoryTree>("exec [CRM].[Category_Tree_p]");
            return spResult;
        }

        //SP CATEGORY BY NAME
        //==========================================
        public ObjectResult<CategoryTree> spCategory_byName(string searchText)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@CategoryName", SqlDbType.VarChar) { Value = searchText }
          };

            ObjectResult<CategoryTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CategoryTree>("exec [CRM].[Category_ByCategoryName_p] @CategoryName", param);
            return spResult;
        }

        //SP CATEGORY BY PRODUCT
        //==========================================
        public ObjectResult<CategoryTree> spCategory_byProductId(Guid productid)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = productid }
          };

            ObjectResult<CategoryTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CategoryTree>("exec [CRM].[Category_byProductID_p] @ProductID", param);
            return spResult;
        }

        //SP CATEGORY BY PRODUCT BY NAME
        //==========================================
        public ObjectResult<CategoryTree> spCategory_byProductId_byName(Guid productid, string searchText)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ProductID", SqlDbType.UniqueIdentifier) { Value = productid },
                new SqlParameter("@CategoryName", SqlDbType.VarChar) { Value = searchText }
            };

            ObjectResult<CategoryTree> spResult = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CategoryTree>("exec [CRM].[Category_byProductID_byCategoryName_p] @ProductID, @CategoryName", param);
            return spResult;
        }
    }
}