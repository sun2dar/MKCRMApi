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
using BCA.CRM.OSB.Messaging;
using BCA.CRM.OSB.Trace;

namespace CCARE.Models
{
    public partial class ChannelInquiry
    {
        public static List<KlikBCABisnisInfo> KBBKeyBCAConnection(Channel channel, string requestToken, string userIdInput)//, string requestToken = null) 
        {
            string resultToken = null;
            List<KlikBCABisnisInfo> kbb = new List<KlikBCABisnisInfo>();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData data = new ESBData() { Result = new List<StringDictionary>() };

            if (!string.IsNullOrEmpty(requestToken))
            {
                resultToken = requestToken;
                param.RequestTransType = "GetTokenSMETblKoneksiInfoBySNToken";
                param.Parameter.Add("SN_Token", resultToken);
            }
            else if (!string.IsNullOrEmpty(channel.SNKeyBCA))
            {
                resultToken = requestToken;
                param.RequestTransType = "GetTokenSMETblKoneksiInfoBySNToken";
                param.Parameter.Add("SN_Token", channel.SNKeyBCA);
            }
            else if (!string.IsNullOrEmpty(channel.CorpId) && !string.IsNullOrEmpty(userIdInput))
            {
                param.RequestTransType = "GetTokenSMETblKoneksiInfoByKeyId";
                param.Parameter.Add("keyId", channel.CorpId + userIdInput);
            }
            else if (!string.IsNullOrEmpty(channel.CorpId))
            {
                param.RequestTransType = "GetTokenSMETblKoneksiInfoByCorpId";
                param.Parameter.Add("CorporateID", channel.CorpId);
            }
            param.WSDL = "ESBDBDelimiter";

            data = EAI.RetrieveESBData(param);

            if (data != null && data.Result.Count > 0)
            {
                string keyId = string.Empty;
                string corpId = string.Empty;
                string userId = string.Empty;
                string functionType = string.Empty;

                resultToken = data.Result[0]["sn_token"];

                if (resultToken != null)
                {
                    StringDictionary tokenInfoDictionary = RetrieveSMETokenInfo(resultToken);

                    foreach (StringDictionary record in data.Result)
                    {
                        keyId = string.Empty;
                        corpId = string.Empty;
                        userId = string.Empty;
                        functionType = string.Empty;

                        keyId = record["keyId"];
                        if (!string.IsNullOrEmpty(keyId) && keyId.Length > 10)
                        {
                            corpId = keyId.Substring(0, 10);
                            userId = keyId.Substring(10);
                            if (keyId.Substring(0, 3).ToUpperInvariant() == "KBC")
                            {
                                functionType = "100103";
                            }
                            else
                            {
                                functionType = "100102";
                            }
                        }

                        ESBData dataStep = new ESBData() { Result = new List<StringDictionary>() };
                        Params paramStep = new Params() { Parameter = new Dictionary<string, string>() };

                        try
                        {
                            paramStep.RequestTransType = "GetKBBId1ByUserIdStep1";
                            paramStep.Parameter.Add("userId", userId);
                            paramStep.Parameter.Add("funcType", functionType);
                            paramStep.Parameter.Add("corpId", corpId);
                            paramStep.Parameter.Add("ActionDescription", "RELEASE");
                            paramStep.WSDL = "ESBDBDelimiter";

                            dataStep = EAI.RetrieveESBData(paramStep);
                        }
                        catch (ServiceAgentException ex)
                        {
                            TraceHelper.TraceEvent(ex);
                        }

                        if (dataStep == null || dataStep.Result.Count <= 0)
                        {
                            try
                            {
                                paramStep.Parameter = new Dictionary<string, string>();
                                paramStep.RequestTransType = "GetKBBId1ByUserIdStep2";
                                paramStep.Parameter.Add("userId", userId);
                                paramStep.Parameter.Add("funcType", functionType);
                                paramStep.Parameter.Add("ActionDescription", "RELEASE");
                                paramStep.WSDL = "ESBDBDelimiter";

                                dataStep = EAI.RetrieveESBData(paramStep);
                            }
                            catch (ServiceAgentException ex)
                            {
                                TraceHelper.TraceEvent(ex);
                            }
                        }

                        string Id1 = string.Empty;
                        if (dataStep != null && dataStep.Result.Count > 0)
                        {
                            Id1 = dataStep.Result[0]["wktkonek"];
                        }

                        string Id2 = record["id2"];
                        if (!string.IsNullOrWhiteSpace(Id2))
                        {
                            DateTime timeId2 = DateTime.ParseExact(Id2.Substring(11, 8), "HH:mm:ss", CultureInfo.InvariantCulture);
                            Id2 = Id2.Substring(0, 11) + timeId2.ToString("hh:mm:ss") + Id2.Substring(22);
                        }

                        string corporateName = string.Empty;
                        int flagService1 = 0;
                        string updateOfficer = string.Empty;
                        string oldStatus = string.Empty;
                        string newStatus = string.Empty;
                        string tokenBranch = string.Empty;

                        if (flagService1 == 0)
                        {
                            try
                            {
                                Params paramCorp = new Params() { Parameter = new Dictionary<string, string>() };
                                paramCorp.WSDL = "ESBDBDelimiter";
                                paramCorp.RequestTransType = "GetCorporateNameByCorpId";
                                paramCorp.Parameter.Add("corpId", corpId);
                                ESBData dataCoorporate = new ESBData() { Result = new List<StringDictionary>() };
                                dataCoorporate = EAI.RetrieveESBData(paramCorp);

                                if (dataCoorporate != null && dataCoorporate.Result.Count > 0)
                                {
                                    corporateName = dataCoorporate.Result[0]["nm"];
                                }
                            }
                            catch (ServiceAgentException ex)
                            {
                                TraceHelper.TraceEvent(ex);
                            }

                            try
                            {
                                Params paramBranch = new Params() { Parameter = new Dictionary<string, string>() };
                                paramBranch.RequestTransType = "GetTokenSMEBranchInfo";
                                paramBranch.Parameter.Add("keyId", keyId);
                                paramBranch.Parameter.Add("OldStatus", "NEW");
                                paramBranch.Parameter.Add("NewStatus", "AKT");
                                paramBranch.WSDL = "ESBDBDelimiter";

                                ESBData dataBranch = new ESBData() { Result = new List<StringDictionary>() };
                                dataBranch = EAI.RetrieveESBData(paramBranch);

                                if (dataBranch != null && dataBranch.Result.Count > 0)
                                {
                                    updateOfficer = dataBranch.Result[0]["updateofficer"];
                                    /* The tokenBranch was initally mapped to  [GetTokenSMEBranchInfo.tokenBranch] , but was changed on 23May2011
                                       as BCA requested that it should be mapped to [GetTokenSMEBranchInfo.kcpCd] + ’-‘ + [GetTokenSMEBranchInfo.tokenCity]
                                    */
                                    tokenBranch = dataBranch.Result[0]["kdkcp"] + " - " + dataBranch.Result[0]["kotatoken"];
                                }
                            }
                            catch (ServiceAgentException ex)
                            {
                                TraceHelper.TraceEvent(ex);
                            }
                            flagService1 = 1;
                        }

                        DateTime? lastUpdateDate = Utility.ParseExact(record["lastupdate"], "yyyy-MM-dd HH:mm:ss");

                        KlikBCABisnisInfo kbbinfo = new KlikBCABisnisInfo()
                        {

                            SNKeyBca = record["sn_token"],
                            UserID = userId,
                            UserName = record["firstName"],
                            UpdatedOn = lastUpdateDate, //// lastUpdateDate.HasValue ? DateFormatter.ToStringExact(lastUpdateDate.Value, DateFormatter.KeyBCABusinessConnectionInquiryUpdateDateUIDisplayFormat) : string.Empty,
                            ID1 = Id1,
                            ID2 = Id2,
                            ActivatedBy = record["spvOfficer"],
                            UpdatedBy = record["updateofficer"],
                            CardNo = record["cardNo"],
                            CreatedDate = record["createdDate"],
                            KeyId = record["keyId"],
                            CorpName = corporateName,
                            BranchName = tokenBranch,
                            CorpID = corpId
                        };

                        // Earlier the ApplicationName value was based on "aplCd", but was changed on 24-May-2011 to read from "KeyId" (refer to the below logic)
                        if (!string.IsNullOrEmpty(kbbinfo.KeyId) && kbbinfo.KeyId.Length >= 3)
                        {

                            string appValue = string.Compare("KBC", kbbinfo.KeyId.Substring(0, 3), true, CultureInfo.InvariantCulture) != 0 ? "SME" : "KBC";
                            kbbinfo.ApplicationName = appValue;
                            //inquiryStatusRecord.ApplicationName = new KeyValuePair<string, string>(record["aplCd"], appValue);
                        }

                        if (tokenInfoDictionary != null)
                        {
                            kbbinfo.TokenTypeKey = tokenInfoDictionary["tokenType"];
                            kbbinfo.TokenType = kbbinfo.TokenTypeKey == "A" ? "Active Card" : "Vasco";
                        }

                        // If the Key is not found in the Status Mapper, then "TIDAK ADA TOKEN" is to be displayed.
                        //string keyValue = Utility.GetStringMap(11, 4, record["kd_status"]);

                        kbbinfo.KeyBCAStatusKey = record["kd_status"];
                        kbbinfo.KeyBCAStatus = Utility.GetStringMap(11, 1, kbbinfo.KeyBCAStatusKey);
                        if (kbbinfo.KeyBCAStatus == kbbinfo.KeyBCAStatusKey)
                        {
                            kbbinfo.KeyBCAStatusKey = "";
                            kbbinfo.KeyBCAStatus = "TIDAK ADA TOKEN";
                            kbbinfo.KeyBCAStatusLabel = kbbinfo.KeyBCAStatus;
                        }
                        else
                        {
                            kbbinfo.KeyBCAStatusLabel = kbbinfo.KeyBCAStatusKey + " - " + kbbinfo.KeyBCAStatus.ToUpper();
                        }
                        kbb.Add(kbbinfo);
                    }
                }
            }

            return kbb;
        }


        private static StringDictionary RetrieveSMETokenInfo(string snToken)
        {
            StringDictionary tokenInfo = null;
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            
            param.RequestTransType = "GetSMETokenType";
            param.Parameter.Add("snToken", snToken);
            //param.WSDL = "ESBDBDelimiter";

            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            data = EAI.RetrieveESBData(param);
            if (data != null && data.Result.Count > 0)
            {
                ESBData records = new ESBData() { Result = new List<StringDictionary>() };
                Params paramToken = new Params() { Parameter = new Dictionary<string, string>() };
                string tokenType = null;
                if (string.Compare("A", data.Result[0]["KdKeyBCA"], true, CultureInfo.CurrentUICulture) == 0)
                {
                    tokenType = "A";
                    paramToken.RequestTransType = "GetSMETokenInfoActiveCard";
                    paramToken.Parameter.Add("snToken", snToken);
                    //paramToken.WSDL = "ESBDBDelimiter";

                    records = EAI.RetrieveESBData(paramToken);
                }
                else if (string.Compare("B", data.Result[0]["KdKeyBCA"], true, CultureInfo.CurrentUICulture) == 0)
                {
                    tokenType = "V";
                    paramToken.RequestTransType = "GetSMETokenInfoVasco";
                    paramToken.Parameter.Add("snToken", snToken);
                    //paramToken.WSDL = "ESBDBDelimiter";

                    records = EAI.RetrieveESBData(paramToken);
                }

                if (records != null && records.Result.Count > 0)
                {
                    // Adding that to the dictionary since, it is not part of the result set from ESB.
                    records.Result[0].Add("tokenType", tokenType);
                    tokenInfo = records.Result[0];
                }
            }

            return tokenInfo;
        }
    }
}