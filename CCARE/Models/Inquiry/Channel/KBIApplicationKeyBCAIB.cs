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
    public partial class ChannelInquiry
    {
        public static List<ApplicationKeyBCAIBInfo> KBIApplicationKeyBCAIB(Channel channel)
        {
            List<ApplicationKeyBCAIBInfo> applicationkeybcaIBI = new List<ApplicationKeyBCAIBInfo>();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData data = new ESBData() { Result = new List<StringDictionary>() };

            string tokenNumber = null;
            if (!string.IsNullOrEmpty(channel.SNKeyBCA))
            {
                tokenNumber = channel.SNKeyBCA;
            }
            else if (!string.IsNullOrEmpty(channel.UserId))
            {
                param.Parameter = new Dictionary<string, string>();
                param.RequestTransType = "SearchCustByIBDirect";
                param.Parameter.Add("userId", channel.UserId);
                param.WSDL = "ESBDBDelimiter";

                data = EAI.RetrieveESBData(param);

                if (data != null && data.Result.Count > 0)
                {
                    Params paramToken = new Params() { Parameter = new Dictionary<string, string>() };
                    ESBData dataToken = new ESBData() { Result = new List<StringDictionary>() };
                    paramToken.RequestTransType = "GetSNTokenByATMNoTblKoneksi";
                    paramToken.Parameter.Add("atmNo", data.Result[0]["cardnumber"]);
                    paramToken.WSDL = "ESBDBDelimiter";

                    dataToken = EAI.RetrieveESBData(paramToken);

                    if (dataToken != null && dataToken.Result.Count == 1)
                    {
                        tokenNumber = dataToken.Result[0]["sn_token"];
                    }
                }
            }
            else if (!string.IsNullOrEmpty(channel.CardNo))
            {
                param.Parameter = new Dictionary<string, string>();
                param.RequestTransType = "GetSNTokenByATMNoTblKoneksi";
                param.Parameter.Add("atmNo", channel.CardNo);
                param.WSDL = "ESBDBDelimiter";

                data = EAI.RetrieveESBData(param);

                if (data != null && data.Result.Count > 0)
                {
                    tokenNumber = data.Result[0]["sn_token"];
                }
            }

            if (tokenNumber != null)
            {
                StringDictionary tokenInfo = RetrieveTokenInfo(tokenNumber);

                Params paramTbh = new Params() { Parameter = new Dictionary<string, string>() };
                paramTbh.WSDL = "ESBDBDelimiter";
                paramTbh.RequestTransType = "GetTokenDetailTblTbhKoneksiBySNToken";
                paramTbh.Parameter.Add("snToken", tokenNumber);
                ESBData dataTbh = EAI.RetrieveESBData(paramTbh);
                StatusPermohanan(ref applicationkeybcaIBI, tokenInfo, dataTbh.Result, "Tambah koneksi");

                Params paramMhn = new Params() { Parameter = new Dictionary<string, string>() };
                paramMhn.WSDL = "ESBDBDelimiter";
                paramMhn.RequestTransType = "GetTokenDetailTblMhnTokenBySNToken";
                paramMhn.Parameter.Add("snToken", tokenNumber);
                ESBData dataMhn = EAI.RetrieveESBData(paramMhn);
                StatusPermohanan(ref applicationkeybcaIBI, tokenInfo, dataMhn.Result, "Koneksi baru");

                Params paramHps = new Params() { Parameter = new Dictionary<string, string>() };
                paramHps.WSDL = "ESBDBDelimiter";
                paramHps.RequestTransType = "GetTokenDetailTblHpsKoneksiBySNToken";
                paramHps.Parameter.Add("snToken", tokenNumber);
                ESBData dataHps = EAI.RetrieveESBData(paramHps);
                StatusPermohanan(ref applicationkeybcaIBI, tokenInfo, dataHps.Result, "Hapus Koneksi");

                if (applicationkeybcaIBI == null || applicationkeybcaIBI.Count <= 0)
                {
                    //throw new ServiceAgentException("No Result");
                }
            }

            return applicationkeybcaIBI;
        }

        private static void StatusPermohanan(ref List<ApplicationKeyBCAIBInfo> applicationkeybcaIBI, StringDictionary tokenInfo, List<StringDictionary> tokenDetail, string action)
        {
            if (tokenDetail != null && tokenDetail.Count > 0)
            {
                foreach (StringDictionary entry in tokenDetail)
                {
                    DateTime? connectionDate = Utility.ParseExact(entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_ConnectionDate], "MM/dd/yyyy h:mm:ss tt");
                    DateTime? lastUpdatedDate = null;
                    if (tokenInfo != null)
                        lastUpdatedDate = Utility.ParseExact(tokenInfo[ESBKeyValueName.TokenInfo_LastUpdateDate], "MM/dd/yyyy h:mm:ss tt");
                    DateTime? requestDate = Utility.ParseExact(entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_RequestDate], "MM/dd/yyyy h:mm:ss tt");

                    //string keyValue = Utility.GetStringMap(10, 4, entry[ESBKeyValueName.TokenInfo_Status]);
                    string keyValue = Utility.GetStringMap(10, 1, entry[ESBKeyValueName.TokenInfo_Status]);

                    if (string.Compare(keyValue, entry[ESBKeyValueName.TokenInfo_Status]) == 0)
                    {
                        keyValue = "TIDAK ADA TOKEN";
                    }

                    applicationkeybcaIBI.Add(new ApplicationKeyBCAIBInfo()
                    {
                        UserIdClickBCA = entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_KeyId],
                        AtmCardNo = entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_AtmCardNo],
                        BranchIssuingToken = tokenInfo != null ? tokenInfo[ESBKeyValueName.TokenInfo_Branch] : null,
                        //TokenType = Utility.GetStringMap(10, 18, tokenInfo != null ? tokenInfo["tokenType"] : null),
                        TokenType = Utility.GetStringMap(10, 15, tokenInfo != null ? tokenInfo["tokenType"] : null),
                        UpdatedBy = tokenInfo != null ? tokenInfo[ESBKeyValueName.TokenInfo_UpdateBy] : null,
                        ActivatedBy = tokenInfo != null ? tokenInfo[ESBKeyValueName.TokenInfo_ActivatedBy] : null,
                        ConnectionOrDeletedDate = connectionDate,
                        LastUpdatedDate = lastUpdatedDate,
                        RequestDate = requestDate,
                        Action = action,
                        //ApplicationName = Utility.GetStringMap(10, 15, entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_ApplicationName]),
                        ApplicationName = Utility.GetStringMap(10, 13, entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_ApplicationName]),
                        CustomerName = entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_CustomerName],
                        RejectionReason = entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_RejectionReason],
                        //ConnectionType = Utility.GetStringMap(10, 22, entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_ConnectionType]),
                        ConnectionType = Utility.GetStringMap(10, 14, entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_ConnectionType]),
                        Id1 = entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_Id1],
                        Id2 = entry[ESBKeyValueName.TokenDetailTblTbhKoneksi_Id2],
                        //edited
                        Status = tokenInfo != null ? entry[ESBKeyValueName.TokenInfo_Status] : null,
                        StatusKey = keyValue
                    });
                }
            }
        }
    }
}