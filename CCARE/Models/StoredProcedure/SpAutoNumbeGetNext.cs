using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public string AutoNumber_GetNext(string objectName, int year)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@ObjectName", SqlDbType.NVarChar, 100) { Value = objectName },
                new SqlParameter("@Year", SqlDbType.Int) { Value = year },
                new SqlParameter("@RandomNo", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output }
            };

            List<string> listParams = new List<string>();
            listParams.Add("@ObjectName");
            listParams.Add("@Year");
            listParams.Add("@RandomNo out");

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("exec [CRM].[AutoNumber_GetNext]");
            sBuilder.Append(string.Join(",", listParams.ToArray()));
            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand(sBuilder.ToString(), param);
            string message = (string)param[listParams.Count - 1].Value;

            return message;
        }
    }
}