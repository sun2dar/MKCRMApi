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
using BCA.CRM.OSB.Model;

namespace CCARE.Models
{
    public partial class ProductInquiry
    {
        // corpId + uniqueKey || cardNo
        public static CorporateCard CorporateCard(Params request)
        {
            CorporateCard model = null;
            request.WSDL = "CorporateCardInquiry";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                model = new CorporateCard();
                model.CardNumber = string.IsNullOrEmpty(data.Result[0]["cardnumber"]) ? "-" : data.Result[0]["cardnumber"];
                model.CompanyName = string.IsNullOrEmpty(data.Result[0]["embossname"]) ? "-" : data.Result[0]["embossname"];
                model.CorporateId = string.IsNullOrEmpty(data.Result[0]["corporateid"]) ? "-" : data.Result[0]["corporateid"];
                model.AccountNumber = string.IsNullOrEmpty(data.Result[1]["accountnumber"]) ? "-" : Formatter.AccountNumber(data.Result[1]["accountnumber"]);
                model.EmbossName = string.IsNullOrEmpty(data.Result[0]["customfield"]) ? "-" : data.Result[0]["customfield"];
                model.CardHolder = string.IsNullOrEmpty(data.Result[0]["cardholder"]) ? "-" : data.Result[0]["cardholder"];
                model.Phone = string.IsNullOrEmpty(data.Result[0]["phone"]) ? "-" : data.Result[0]["phone"];
                model.IdNumber = string.IsNullOrEmpty(data.Result[0]["idnumber"]) ? "-" : data.Result[0]["idnumber"];
                model.UniqueKey = string.IsNullOrEmpty(data.Result[0]["uniquekey"]) ? "-" : data.Result[0]["uniquekey"];
                model.Email = string.IsNullOrEmpty(data.Result[0]["email"]) ? "-" : data.Result[0]["email"];
                model.Status = string.IsNullOrEmpty(data.Result[0]["status"]) ? "-" : data.Result[0]["status"];
                model.RecurringType = string.IsNullOrEmpty(data.Result[0]["reccurenttype"]) ? "-" : data.Result[0]["reccurenttype"];
                model.RecurringPeriod = string.IsNullOrEmpty(data.Result[0]["reccurentperiod"]) ? "-" : data.Result[0]["reccurentperiod"];

                model.CardType = string.IsNullOrEmpty(data.Result[0]["corporatetype"]) ? "-" : Utility.GetStringMap(18, 100, data.Result[0]["corporatetype"]);
                model.StatusLabel = string.IsNullOrEmpty(data.Result[0]["status"]) ? "-" : Utility.GetStringMap(18, 1, data.Result[0]["status"]);
                model.Limit = Utility.GetParsedDouble(data.Result[0]["limit"], false);
                model.LimitExpiredDate = Utility.ParseExact(data.Result[0]["limitexpireddate"], "yyyyMMdd");
            }
            return model;
        }

        // comp_nm_in
        public static List<Corporate> GetListCorporate(Params request)
        {
            List<Corporate> result = new List<Corporate>();
            request.Parameter.Add("max_row_in", "10");
            request.WSDL = "CallStoreProcedureDelimiter";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                int i = -1;
                foreach (StringDictionary entry in data.Result)
                {
                    if(!string.IsNullOrEmpty(entry["result"]))
                    {
                        continue;                    
                    }
                    else{
                        Corporate model = new Corporate();
                        model.Id = i++;
                        model.CorporateId = entry["o_comp_cd"];
                        model.CorporateName = entry["o_comp_nm"];
                        model.Status = entry["o_status"];
                        result.Add(model);
                    }
                }
            }
            return result;
        }
    }
}