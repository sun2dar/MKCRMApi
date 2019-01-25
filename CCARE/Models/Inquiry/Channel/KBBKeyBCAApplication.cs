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
        public static List<KlikBCABisnisInfo> KBBKeyBCAApplication(Channel channel, string requestToken = null) 
        {
            List<KlikBCABisnisInfo> kbbkeybcaapplication = new List<KlikBCABisnisInfo>();

            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            try
            {
                string token = null;
                InputParameterType parameter = InputParameterType.Token;
                string corporateId = string.Empty;
                string corpId = string.Empty;
                ESBData data = new ESBData() { Result = new List<StringDictionary>() };
                string firstKeyId = string.Empty;
                string corporateName = string.Empty;
                string tokenBranch = string.Empty;

                if (!string.IsNullOrEmpty(requestToken))
                {
                    token = requestToken;
                    param.RequestTransType = "GetTokenSMETblMohonTokenInfoBySNToken";
                    param.Parameter.Add("SN_Token", token);
                }
                else if (!string.IsNullOrEmpty(channel.SNKeyBCA))
                {
                    token = requestToken;
                    param.RequestTransType = "GetTokenSMETblMohonTokenInfoBySNToken";
                    param.Parameter.Add("SN_Token", channel.SNKeyBCA);
                }
                else if (!string.IsNullOrEmpty(channel.CorpId) && !string.IsNullOrEmpty(channel.UserId))
                {
                    parameter = InputParameterType.KeyId;
                    param.RequestTransType = "GetTokenSMETblMohonTokenInfoByKeyId";
                    param.Parameter.Add("KeyID", channel.CorpId + channel.UserId);
                }
                else if (!string.IsNullOrEmpty(channel.CorpId))
                {
                    parameter = InputParameterType.CorpId;
                    param.RequestTransType = "GetTokenSMETblMohonTokenInfoByCorpId";
                    param.Parameter.Add("CorporateID", channel.CorpId);
                }

                param.WSDL = "ESBDBDelimiter";

                data = EAI.RetrieveESBData(param);

                if (data != null && data.Result.Count > 0)
                {
                    if (!string.IsNullOrEmpty(data.Result[0]["keyid"]) && data.Result[0]["keyid"].Length >= 3)
                    {
                        corporateId = data.Result[0]["keyid"].Substring(0, 3);

                        if (data.Result[0]["keyid"].Length >= 10)
                        {
                            corpId = data.Result[0]["keyid"].Substring(0, 10);
                        }
                    }

                    int flagService = 0;
                    if (flagService == 0)
                    {
                        try
                        {
                            Params paramCorp = new Params() { Parameter = new Dictionary<string, string>() };
                            
                            paramCorp.RequestTransType = "GetCorporateNameByCorpId";
                            paramCorp.Parameter.Add("CorporateID", corpId);
                            paramCorp.WSDL = "ESBDBDelimiter";

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

                            Params paramCorp = new Params() { Parameter = new Dictionary<string, string>() };
                            paramCorp.RequestTransType = "GetTokenSMEBranchInfo";
                            paramCorp.Parameter.Add("KeyID", data.Result[0]["keyid"]);
                            paramCorp.Parameter.Add("OldStatus", "NEW");
                            paramCorp.Parameter.Add("NewStatus", "AKT");
                            paramCorp.WSDL = "ESBDBDelimiter";

                            ESBData databranch = new ESBData() { Result = new List<StringDictionary>() };
                            databranch = EAI.RetrieveESBData(paramCorp);

                            if (databranch != null && databranch.Result.Count > 0)
                            {
                                tokenBranch = databranch.Result[0]["kdkcp"] + '-' + databranch.Result[0]["kotatoken"];
                            }
                        }
                        catch (ServiceAgentException ex)
                        {
                            TraceHelper.TraceEvent(ex);
                        }
                        flagService = 1;
                    }

                    Step1StatuPermohananKeyBCASME(kbbkeybcaapplication, data.Result, corporateName, tokenBranch);
                }

                Step2StatuPermohananKeyBCASME(kbbkeybcaapplication, corporateName, tokenBranch, parameter, token, corpId, channel.UserId);
                Step3StatuPermohananKeyBCASME(kbbkeybcaapplication, corporateName, tokenBranch, parameter, token, corpId, channel.UserId);
            }
            catch (ServiceAgentException)
            {
            }

            return kbbkeybcaapplication;
        }

        private static void Step1StatuPermohananKeyBCASME(List<KlikBCABisnisInfo> respon, List<StringDictionary> records, string corporateName, string tokenBranch)
        {
            try
            {
                string keyId = string.Empty;
                string corpId = string.Empty;
                string userId = string.Empty;
                string functionType = string.Empty;

                foreach (StringDictionary record in records)
                {
                    keyId = record["keyId"];
                    corpId = string.Empty;
                    userId = string.Empty;
                    functionType = string.Empty;
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

                    ESBData data = new ESBData() { Result = new List<StringDictionary>() };
                    Params param = new Params() { Parameter = new Dictionary<string, string>() };

                    try
                    {
                        
                        param.RequestTransType = "GetKBBId1ByUserIdStep1";
                        param.Parameter.Add("UserID", userId);
                        param.Parameter.Add("FunctionType", functionType);
                        param.Parameter.Add("CorporateID", corpId);
                        param.Parameter.Add("ActionDescription", "RELEASE");
                        param.WSDL = "ESBDBDelimiter";

                        data = EAI.RetrieveESBData(param);
                    }
                    catch (ServiceAgentException ex)
                    {
                        TraceHelper.TraceEvent(ex);
                    }

                    if (data == null || data.Result.Count <= 0)
                    {
                        try
                        {
                            param.Parameter = new Dictionary<string, string>();
                            param.RequestTransType = "GetKBBId1ByUserIdStep2";
                            param.Parameter.Add("UserID", userId);
                            param.Parameter.Add("FunctionType", functionType);
                            param.Parameter.Add("ActionDescription", "RELEASE");
                            param.WSDL = "ESBDBDelimiter";
                            
                            data = EAI.RetrieveESBData(param);
                        }
                        catch (ServiceAgentException ex)
                        {
                            TraceHelper.TraceEvent(ex);
                        }
                    }

                    string Id1 = string.Empty;

                    if (data != null && data.Result.Count > 0)
                    {
                        Id1 = data.Result[0]["log_dt"];
                    }

                    DateTime? lastUpdatedDate = Utility.ParseExact(record["lastupdate"], "yyyy-MM-dd H:mm:ss");

                    KlikBCABisnisInfo kbbinfo = new KlikBCABisnisInfo()
                    {
                        SNKeyBca = record["sn_token"],
                        UserID = userId,
                        ID1 = Id1,
                        ID2 = record["id2"],
                        ActivatedBy = record["spvOfficer"],
                        UpdatedBy = record["updateofficer"],
                        UserName = record["firstName"],
                        KeyId = record["keyId"],
                        CorpName = corporateName,
                        BranchName = tokenBranch,
                        UpdatedOn = lastUpdatedDate
                    };


                    // Earlier the ApplicationName value was based on "aplCd", but was changed on 24-May-2011 to read from "KeyId" (refer to the below logic)
                    if (!string.IsNullOrEmpty(kbbinfo.KeyId) && kbbinfo.KeyId.Length >= 3)
                    {
                        string appValue = string.Compare("KBC", kbbinfo.KeyId.Substring(0, 3), true, CultureInfo.InvariantCulture) != 0 ? "SME" : "KBC";
                        kbbinfo.ApplicationName = appValue;
                        //inquiryStatusRecord.ApplicationName = new KeyValuePair<string, string>(record["KdApl"], appValue);
                    }

                    // If the Key is not found in the Status Mapper, then "TIDAK ADA TOKEN" is to be displayed.
                    //string keyValue = Utility.GetStringMap(11, 4, record["kd_status"]);
                    string keyValue = Utility.GetStringMap(12, 5, record["kd_status"]);
                    if (keyValue == record["kd_status"])
                    {
                        keyValue = "TIDAK ADA TOKEN";
                    }

                    kbbinfo.Action = keyValue;
                    //inquiryStatusRecord.Action = new KeyValuePair<string, string>(record["kd_status"], keyValue);

                    respon.Add(kbbinfo);
                }
            }
            catch (ServiceAgentException)
            {
            }
        }

        private static void Step2StatuPermohananKeyBCASME(List<KlikBCABisnisInfo> respon, string corporateName, string tokenBranch, InputParameterType parameter, string requestToken, string requestCorpId, string requestUserId)
        {
            try
            {
                ESBData data = new ESBData() { Result = new List<StringDictionary>() };
                Params param = new Params() { Parameter = new Dictionary<string, string>() };

                if (parameter == InputParameterType.Token)
                {
                    param.RequestTransType = "GetTokenSMETblHapusKoneksiTokenInfoBySNToken";
                    param.Parameter.Add("SN_Token", requestToken);
                }
                else if (parameter == InputParameterType.KeyId)
                {
                    parameter = InputParameterType.KeyId;
                    param.RequestTransType = "GetTokenSMETblHapusKoneksiTokenInfoByKeyId";
                    param.Parameter.Add("KeyID", requestCorpId + requestUserId);
                }
                else if (parameter == InputParameterType.CorpId)
                {
                    parameter = InputParameterType.CorpId;
                    param.RequestTransType = "GetTokenSMETblHapusKoneksiTokenInfoByCorpId";
                    param.Parameter.Add("CorporateID", requestCorpId);
                }

                param.WSDL = "ESBDBDelimiter";

                data = EAI.RetrieveESBData(param);

                string keyId = string.Empty;
                string corpId = string.Empty;
                string userId = string.Empty;

                foreach (StringDictionary record in data.Result)
                {
                    keyId = record["keyid"];
                    corpId = string.Empty;
                    userId = string.Empty;

                    if (!string.IsNullOrEmpty(keyId) && keyId.Length > 10)
                    {
                        corpId = keyId.Substring(0, 10);
                        userId = keyId.Substring(10);
                    }

                    DateTime? lastUpdatedDate = Utility.ParseExact(record["lastUpdate"], "yyyy-MM-dd H:mm:ss");

                    //if(fn:string-length($row/Record[fn:lower-case(*:key) = 'id2']/value/text()) > 0) then 
		            //  let $waktuKonek := fn:substring($row/Record[fn:lower-case(*:key) = 'id2']/value/text() , 1 , 22)
		            //  return concat(fn-bea:
                    //              dateTime-to-string-with-format('MM/dd/yyyy hh:mm:ss', fn-bea:dateTime-from-string-with-format('MM dd yyyy HH:mm:ss a', replace($waktuKonek, '/', ' '))) , ' ' , fn:substring($row/Record[fn:lower-case(*:key) = 'id2']/value/text() , 23))
		            //  else ''

                    KlikBCABisnisInfo kbbinfo = new KlikBCABisnisInfo()
                    {
                        SNKeyBca = record["sn_Token"],
                        UserID = userId,
                        ID2 = record["id2"],
                        ActivatedBy = record["spvofficer"],
                        UpdatedBy = record["updateofficer"],
                        UserName = record["firstname"],
                        KeyId = record["keyid"],
                        CorpName = corporateName,
                        BranchName = tokenBranch,
                        UpdatedOn = lastUpdatedDate
                    };

                    kbbinfo.ApplicationName = record["aplName"];
                    //inquiryStatusRecord.ApplicationName = new KeyValuePair<string, string>(record["aplName"], record["aplName"]);

                    //if (!string.IsNullOrEmpty(inquiryStatusRecord.KeyId) && inquiryStatusRecord.KeyId.Length >= 3)
                    //{
                    //    string appValue = string.Compare("KBC", inquiryStatusRecord.KeyId.Substring(0, 3), true, CultureInfo.InvariantCulture) != 0 ? "SME" : "KBC";
                    //    inquiryStatusRecord.ApplicationName = new KeyValuePair<string, string>(record["aplCd"], appValue);
                    //}

                    // If the Key is not found in the Status Mapper, then "TIDAK ADA TOKEN" is to be displayed.
                    //string keyValue = Utility.GetStringMap(11, 4, record["kd_status"]);
                    string keyValue = Utility.GetStringMap(12, 5, record["kd_status"]);
                    if (keyValue == record["kd_status"])
                    {
                        keyValue = "TIDAK ADA TOKEN";
                    }

                    kbbinfo.Action = keyValue;
                    //inquiryStatusRecord.Action = new KeyValuePair<string, string>(record["kd_status"], keyValue);

                    respon.Add(kbbinfo);
                }
            }
            catch (ServiceAgentException)
            {
                // If it fails still we have to show other results.
            }
        }

        private static void Step3StatuPermohananKeyBCASME(List<KlikBCABisnisInfo> respon, string corporateName, string tokenBranch, InputParameterType parameter, string requestToken, string requestCorpId, string requestUserId)
        {
            try
            {
                ESBData data = new ESBData() { Result = new List<StringDictionary>() };
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                if (parameter == InputParameterType.Token)
                {
                    param.RequestTransType = "GetTokenSMETblHapusKoneksiTokenInfoBySNToken";
                    param.Parameter.Add("SN_Token", requestToken);
                }
                else if (parameter == InputParameterType.KeyId)
                {
                    parameter = InputParameterType.KeyId;
                    param.RequestTransType = "GetTokenSMETblHapusKoneksiTokenInfoByKeyId";
                    param.Parameter.Add("KeyID", requestCorpId + requestUserId);
                }
                else if (parameter == InputParameterType.CorpId)
                {
                    parameter = InputParameterType.CorpId;
                    param.RequestTransType = "GetTokenSMETblHapusKoneksiTokenInfoByCorpId";
                    param.Parameter.Add("CorporateID", requestCorpId);
                }

                param.WSDL = "ESBDBDelimiter";

                data = EAI.RetrieveESBData(param);

                string keyId = string.Empty;
                string corpId = string.Empty;
                string userId = string.Empty;

                foreach (StringDictionary record in data.Result)
                {
                    keyId = record["keyid"];
                    corpId = string.Empty;
                    userId = string.Empty;

                    if (!string.IsNullOrEmpty(keyId) && keyId.Length > 10)
                    {
                        corpId = keyId.Substring(0, 10);
                        userId = keyId.Substring(10);
                    }

                    DateTime? lastUpdatedDate = Utility.ParseExact(record["lastupdate"], "yyyy-MM-dd H:mm:ss");

                    KlikBCABisnisInfo kbbinfo = new KlikBCABisnisInfo()
                    {
                        SNKeyBca = record["sn_token"],
                        UserID = userId,
                        ID2 = record["id2"],
                        ActivatedBy = record["spvofficer"],
                        UpdatedBy = record["updateofficer"],
                        UserName = record["firstname"],
                        KeyId = record["keyId"],
                        CorpName = corporateName,
                        BranchName = tokenBranch,
                        UpdatedOn = lastUpdatedDate
                    };

                    // Earlier the ApplicationName value was based on "aplCd", but was changed on 24-May-2011 to read from "KeyId" (refer to the below logic)
                    if (!string.IsNullOrEmpty(kbbinfo.KeyId) && kbbinfo.KeyId.Length >= 3)
                    {
                        string appKey = string.Compare("KBC", keyId.Substring(0, 3), true, CultureInfo.InvariantCulture) != 0 ? "SME" : "KBC";
                        string appValue = appKey;
                        kbbinfo.ApplicationName = appValue;
                        //inquiryStatusRecord.ApplicationName = new KeyValuePair<string, string>(appKey, appValue);
                    }

                    kbbinfo.Action = record["kd_status"];
                    //inquiryStatusRecord.Action = new KeyValuePair<string, string>("Hapus Token", record["kd_status"]);

                    respon.Add(kbbinfo);
                }
            }
            catch (ServiceAgentException)
            {
                // If it fails still we have to show other results.
            }
        }
    
    }
}