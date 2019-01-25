using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Globalization;
using CCARE.App_Function;

namespace CCARE.Models
{
    public partial class StatusUpdate
    {
        public static ChangeStatusResult LoginIBI(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "LoginIBIStatusUpdate";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0]["status"] == "Success" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.UpdateTime = data.Result[0].ContainsKey("updateTime") ? data.Result[0]["updateTime"] : string.Empty;
            }
            return result;
        }

        public static ChangeStatusResult UserIBI(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "UserIBIStatusUpdate";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0]["status"] == "Success" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.UpdateTime = data.Result[0].ContainsKey("updateTime") ? data.Result[0]["updateTime"] : string.Empty;
            }
            return result;
        }

        public static ChangeStatusResult BlockMalwareIB(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "CallStoreProcedure";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Result = data.Result[0]["status"];
                result.Status = data.Result[0]["status"] == "SUCCESS" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
            }
            return result;
        }

        public static ChangeStatusResult ReleaseMalwareIB(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "CallStoreProcedure";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Result = data.Result[0]["status"];
                result.Status = data.Result[0]["status"] == "SUCCESS" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
            }
            return result;
        }

        public List<KBIMalwareLogInfo> GetMalwareLogIB(Params request)
        {
            List<KBIMalwareLogInfo> result = new List<KBIMalwareLogInfo>();
            request.WSDL = "CallStoreProcedure";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                foreach (StringDictionary entry in data.Result) {
                    KBIMalwareLogInfo log = new KBIMalwareLogInfo();
                    log.createdDate = entry["createddate"];
                    log.userId = entry["fk_custid"];
                    log.activity = entry["activity"];
                    log.updateOfficer = entry["updateofficer"];
                    log.malwareType = entry["malwaretype"];
                    log.custIP = entry["custip"];
                    log.origin = entry["origin"];
                    result.Add(log);

                }
            }
            return result;
        }
    }
}