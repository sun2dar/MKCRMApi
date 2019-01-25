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
        public static ChangeStatusResult CorporateCard(Params request)
        {
            ChangeStatusResult result = new ChangeStatusResult();
            request.WSDL = "CorporateCardBlock";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                result.Status = data.Result[0]["BlockType"] == "B" ? ChangeStatusResultType.Success : ChangeStatusResultType.Failure;
                result.Result = "Card Number : " + data.Result[0]["CardNumber"] + " - Block Type : " + data.Result[0]["BlockType"] + " - Reason Code : " + data.Result[0]["ReasonCode"];
            }
            return result;
        }
    }
}