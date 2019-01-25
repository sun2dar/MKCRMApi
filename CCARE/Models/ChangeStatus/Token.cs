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
        public static ChangeStatusResult Token(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0]["status"] == "Success" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.UpdateTime = data.Result[0].ContainsKey("updateTime") ? data.Result[0]["updateTime"] : string.Empty;
            }
            return result;
        }

        public ChangeStatusResult TokenValidate(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "TokenAuthentication";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0].ContainsKey("result") ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.Result = data.Result[0].ContainsKey("result") ? data.Result[0]["result"] : data.Result[0]["reason"];
            }
            return result;
        }

        public static ChangeStatusResult TokenResynch(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "TokenResynch";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = (string.Compare(data.Result[0]["status"], "1", false, CultureInfo.InvariantCulture) == 0 || string.Compare(data.Result[0]["status"], "success", true, CultureInfo.InvariantCulture) == 0) ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.Result = data.Result[0]["result"];
            }
            return result;
        }

        public static ChangeStatusResult TokenUnlock(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "TokenUnlock";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = (string.Compare(data.Result[0]["status"], "1", false, CultureInfo.InvariantCulture) == 0 || string.Compare(data.Result[0]["status"], "success", true, CultureInfo.InvariantCulture) == 0) ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.Result = data.Result[0]["result"];
            }
            return result;
        }

        public static ChangeStatusResult WriteToLog(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "ESBDBDelimiter";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = (string.Compare(data.Result[0]["result"], "1", false, CultureInfo.InvariantCulture) == 0 || string.Compare(data.Result[0]["result"], "success", true, CultureInfo.InvariantCulture) == 0) ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
            }
            return result;
        }

    }
}