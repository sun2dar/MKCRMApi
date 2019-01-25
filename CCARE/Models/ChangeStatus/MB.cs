﻿using System;
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
        public static ChangeStatusResult UserMB(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "UserMBStatusUpdate";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0]["status"] == "Success" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.UpdateTime = data.Result[0].ContainsKey("updateTime") ? data.Result[0]["updateTime"] : string.Empty;
            }
            return result;
        }

        public static ChangeStatusResult UserSMSBanking(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "UserSMSBankingStatusUpdate";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0]["status"] == "Success" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.UpdateTime = data.Result[0].ContainsKey("updateTime") ? data.Result[0]["updateTime"] : string.Empty;
            }
            return result;
        }

        // ChangeSMSTopUpStatusByMobileNo
        // mobileNo
        // newStatus
        // reason
        // agentId
        public static ChangeStatusResult UserSMSTopUp(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "UserSMSTopUpStatusUpdate";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0]["status"] == "Success" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.UpdateTime = data.Result[0].ContainsKey("updateTime") ? data.Result[0]["updateTime"] : string.Empty;
            }
            return result;
        }
    }
}