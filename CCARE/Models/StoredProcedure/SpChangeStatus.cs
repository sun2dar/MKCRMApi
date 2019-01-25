using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;
using System.Collections.Specialized;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public ObjectResult<StatusLabel> GetStatusName(ChangeStatusInput input)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@EntityType", SqlDbType.Int) {Value = input.EntityType}
            };

            ObjectResult<StatusLabel> statusList = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<StatusLabel>("exec CRM.[StatusChange_GetStatusName] @EntityType", param);
            return (statusList);
        }

        public ObjectResult<StatusLabel> GetNewStatus(ChangeStatusInput input)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@EntityType", SqlDbType.Int) {Value = input.EntityType},
                new SqlParameter("@StatusType", SqlDbType.Int) {Value = input.StatusType},
                new SqlParameter("@Key", CheckForDbNull(input.Status)),
                new SqlParameter("@SecurityRoleID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(input.SecurityRoleId) }
            };

            ObjectResult<StatusLabel> statusList = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<StatusLabel>("exec CRM.[StatusChange_GetNewStatus] @EntityType,@StatusType,@Key,@SecurityRoleID", param);
            return (statusList);
        }

    }
}