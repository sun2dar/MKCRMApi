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
        public static List<KeyBCAConnectionInfo> KBIKeyBCAConnection(Channel channel)
        {
            List<KeyBCAConnectionInfo> keybcaconnection = new List<KeyBCAConnectionInfo>();
            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            string token = null;

            if (!string.IsNullOrEmpty(channel.SNKeyBCA))
            {
                token = channel.SNKeyBCA;
            }

            else if (!string.IsNullOrEmpty(channel.UserId))
            {
                //param.RequestTransType = "SearchCustByIBDirect";
                param.Parameter = new Dictionary<string, string>();
                param.RequestTransType = "GetIBankInfoByUserId";
                param.Parameter.Add("userId", channel.UserId);
                param.WSDL = "ESBDBDelimiter";
                
                data = EAI.RetrieveESBData(param);

                if (data != null && data.Result.Count > 0)
                {
                    ESBData dataToken = new ESBData() { Result = new List<StringDictionary>() };
                    Params paramToken = new Params() { Parameter = new Dictionary<string, string>() };
                    paramToken.RequestTransType = "GetSNTokenByATMNoTblKoneksi";
                    paramToken.Parameter.Add("atmNo", data.Result[0]["cardnumber"]);
                    paramToken.WSDL = "ESBDBDelimiter";
                    
                    dataToken = EAI.RetrieveESBData(paramToken);

                    if (dataToken != null && dataToken.Result.Count > 0)
                    {
                        token = dataToken.Result[0]["sn_token"];
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
                    token = data.Result[0]["sn_token"];
                }
            }

            if (token != null)
            {
                StringDictionary tokenInfoDictionary = RetrieveTokenInfo(token);

                param.Parameter = new Dictionary<string, string>();
                param.RequestTransType = "GetTokenDetailTblKoneksiBySNToken";
                param.Parameter.Add("sn_token", token);
                param.WSDL = "ESBDBDelimiter";
                
                data = EAI.RetrieveESBData(param);

                if (data != null && data.Result.Count > 0)
                {
                    foreach (StringDictionary tokenDetailDictionary in data.Result)
                    {
                        string statuskey = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_Status];
                        //string status = Utility.GetStringMap(9, 4, tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_Status]);
                        string status = Utility.GetStringMap(9, 1, statuskey);
                        if (string.Compare(status, tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_Status]) == 0)
                        {
                            statuskey = "";
                            status = "TIDAK ADA TOKEN";
                        }

                        if (tokenInfoDictionary == null)
                        {
                            keybcaconnection.Add(new KeyBCAConnectionInfo()
                            {
                                KeyId = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_KeyId],
                                CardNo = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_CardNo],
                                BranchIssuingToken = null,
                                TokenTypeKey = null,
                                TokenType = null,
                                TokenStatus = null,
                                UpdatedBy = null,
                                ActivatedBy = tokenDetailDictionary[ESBKeyValueName.TokenInfo_ActivatedBy],
                                ConnectedOn = Utility.ParseExact(tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_ConnectedOn], "MM/dd/yyyy HH:mm:ss tt"),
                                UpdatedDate = Utility.ParseExact(tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_UpdatedDate], "MM/dd/yyyy HH:mm:ss tt"),
                                LastUpdateDate = null,
                                CustomerName = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_CustomerName],
                                ApplicationName = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_ApplicationName],
                                ConnectionType = Utility.GetStringMap(9, 14, tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_ConnectionType]),
                                UserId1 = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_UserId1],
                                UserId2 = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_UserId2],
                                SNToken = token,
                                StatusKey = statuskey,
                                Status = status
                            });
                        }
                        else
                        {
                            keybcaconnection.Add(new KeyBCAConnectionInfo()
                            {
                                KeyId = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_KeyId],
                                CardNo = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_CardNo],
                                BranchIssuingToken = tokenInfoDictionary[ESBKeyValueName.TokenInfo_Branch],
                                TokenTypeKey = tokenInfoDictionary["tokenType"],
                                //TokenType = Utility.GetStringMap(9, 18, tokenInfoDictionary["tokenType"]),
                                TokenType = Utility.GetStringMap(9, 15, tokenInfoDictionary["tokenType"]),
                                //TokenStatus = Utility.GetStringMap(9, 4, tokenInfoDictionary[ESBKeyValueName.TokenInfo_Status]),
                                TokenStatus = Utility.GetStringMap(9, 1, tokenInfoDictionary[ESBKeyValueName.TokenInfo_Status]),
                                UpdatedBy = tokenInfoDictionary[ESBKeyValueName.TokenInfo_UpdateBy],
                                ActivatedBy = tokenDetailDictionary[ESBKeyValueName.TokenInfo_ActivatedBy],
                                ConnectedOn = Utility.ParseExact(tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_ConnectedOn], "MM/dd/yyyy HH:mm:ss tt"),
                                UpdatedDate = Utility.ParseExact(tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_UpdatedDate], "MM/dd/yyyy HH:mm:ss tt"),
                                LastUpdateDate = Utility.ParseExact(tokenInfoDictionary[ESBKeyValueName.TokenInfo_LastUpdateDate], "MM/dd/yyyy HH:mm:ss tt"),
                                CustomerName = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_CustomerName],
                                ApplicationName = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_ApplicationName],
                                //ConnectionType = Utility.GetStringMap(9, 22, tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_ConnectionType]),
                                ConnectionType = Utility.GetStringMap(9, 14, tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_ConnectionType]),
                                UserId1 = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_UserId1],
                                UserId2 = tokenDetailDictionary[ESBKeyValueName.TokenDetailTblKoneksi_UserId2],
                                SNToken = token,
                                StatusKey = statuskey,
                                Status = status
                            });
                        }
                    }
                }
            }

            return keybcaconnection;
        }

        private static StringDictionary RetrieveTokenInfo(string snToken)
        {
            StringDictionary tokenInfo = null;
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            
            param.RequestTransType = "GetTokenType";
            param.Parameter.Add("sn_token", snToken);
            param.WSDL = "ESBDBDelimiter";

            ESBData data = EAI.RetrieveESBData(param);

            if (data != null && data.Result.Count > 0)
            {
                ESBData records = null;
                string tokenType = null;
                if (string.Compare("A", data.Result[0]["kdkeybca"], true) == 0)
                {
                    tokenType = "A";
                    Params paramRecordList = new Params() { Parameter = new Dictionary<string, string>() };
                    paramRecordList.WSDL = "ESBDBDelimiter";
                    paramRecordList.RequestTransType = "GetTokenInfoActiveCard";
                    paramRecordList.Parameter.Add("sn_token", snToken);
                    paramRecordList.Parameter.Add("SN_Token", snToken);
                    records = EAI.RetrieveESBData(paramRecordList);
                }
                else if (string.Compare("B", data.Result[0]["kdkeybca"], true) == 0)
                {
                    //snToken = "%" + snToken + "%";
                    tokenType = "V";
                    Params paramRecordList = new Params() { Parameter = new Dictionary<string, string>() };
                    paramRecordList.WSDL = "ESBDBDelimiter";
                    paramRecordList.RequestTransType = "GetTokenInfoVasco";
                    paramRecordList.Parameter.Add("sn_token", snToken);
                    paramRecordList.Parameter.Add("SN_Token", snToken);
                    records = EAI.RetrieveESBData(paramRecordList);
                }

                if (records != null && records.Result.Count > 0)
                {
                    records.Result[0].Add("tokenType", tokenType);
                    tokenInfo = records.Result[0];
                }
            }

            return tokenInfo;
        }
    }
}