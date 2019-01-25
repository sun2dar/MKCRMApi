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
        public static List<KlikBCAIndividuInfo> KlikBCAIndividual(Params request, Channel channel) 
        {
            List<KlikBCAIndividuInfo> data = new List<KlikBCAIndividuInfo>();
            
            ESBData information = new ESBData();
            if (!string.IsNullOrEmpty(channel.UserId))
            {
                request.WSDL = "ESBDBDelimiter";
                request.RequestTransType = "GetIBankInfoByUserId";
                information = EAI.RetrieveESBData(request);

                request.WSDL = "UserInternetBankingManagement";
                request.RequestTransType = "GetTandemStatusIBankByUserId";
                ESBData tandem = EAI.RetrieveESBData(request);
                BuildInternetBankingIndividualInquiryStatus(ref data, information, tandem, InternetBankingIndividualInquiryStatusResultKeyName.UpdateBy);
            }
            else if (!string.IsNullOrEmpty(channel.CardNo))
            {
                InvokeGetIBankInfoByATMNo(ref data, channel.CardNo);
            }
            else if (!string.IsNullOrEmpty(channel.EmailAddress))
            {
                request.WSDL = "ESBDBDelimiter";
                information = EAI.RetrieveESBData(request);
                if (information != null && information.Result.Count != 0)
                {
                    for (int i = 0; i < information.Result.Count; i++)
                    {
                        InvokeGetIBankInfoByATMNo(ref data, information.Result[i]["cardnumber"]);
                    }
                    
                }
            }

            return data;
        }

        private static void InvokeGetIBankInfoByATMNo(ref List<KlikBCAIndividuInfo> data, string atmNo)
        {
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            ESBData information = new ESBData() { Result = new List<StringDictionary>() };
            param.WSDL = "ESBDBDelimiter";
            param.RequestTransType = "GetIBankInfoByATMNo";
            param.Parameter.Add("atmNo", atmNo);
            information = EAI.RetrieveESBData(param);

            if (information != null && information.Result.Count != 0)
            {
                Params paramTandem = new Params() { Parameter = new Dictionary<string, string>() };
                ESBData tandem = new ESBData() { Result = new List<StringDictionary>() };
                paramTandem.WSDL = "UserInternetBankingManagement";
                paramTandem.RequestTransType = "GetTandemStatusIBankByUserId";
                paramTandem.Parameter.Add("userId", information.Result[0]["custid"]);
                tandem = EAI.RetrieveESBData(paramTandem);

                BuildInternetBankingIndividualInquiryStatus(ref data, information, tandem, InternetBankingIndividualInquiryStatusResultKeyName.UpdatedByForEmail);
            }
        }

        public static void BuildInternetBankingIndividualInquiryStatus(ref List<KlikBCAIndividuInfo> data, ESBData information, ESBData tandem, string updateOfficerKeyName)
        {
            if (information != null && information.Result.Count != 0)
            {
                KlikBCAIndividuInfo klikbcaindividu = new KlikBCAIndividuInfo();
                klikbcaindividu.UserID = information.Result[0][ESBKeyValueName.KBI_UserId];
                klikbcaindividu.Name = information.Result[0][ESBKeyValueName.KBI_Name];
                klikbcaindividu.AtmCardNo = information.Result[0][ESBKeyValueName.KBI_ATMCardNo];
                klikbcaindividu.EmailAddress = information.Result[0][ESBKeyValueName.KBI_EmailAddress];
                klikbcaindividu.RegistrationDate = Utility.ParseExact(information.Result[0][ESBKeyValueName.KBI_RegistrationDate], "MM/dd/yyyy hh:mm:ss tt");
                klikbcaindividu.CreatedDate = Utility.ParseExact(information.Result[0][ESBKeyValueName.KBI_CreatedDate], "MM/dd/yyyy hh:mm:ss tt");
                klikbcaindividu.LastLoginDate = Utility.ParseExact(information.Result[0][ESBKeyValueName.KBI_LastLoginDate], "MM/dd/yyyy hh:mm:ss tt");
                klikbcaindividu.LastUpdateDate = Utility.ParseExact(information.Result[0][ESBKeyValueName.KBI_LastUpdateDate], "MM/dd/yyyy hh:mm:ss tt");
                klikbcaindividu.UpdateBy = information.Result[0][ESBKeyValueName.KBI_UpdateBy];
                klikbcaindividu.WrongPinCounter = information.Result[0][ESBKeyValueName.KBI_WrongPinCounter];
                klikbcaindividu.BlockStatusKey = information.Result[0][ESBKeyValueName.KBI_BlockStatus];
                klikbcaindividu.BlockStatus = Utility.GetStringMap(1, 1, klikbcaindividu.BlockStatusKey);
                klikbcaindividu.UserIdIBankStatusKey = information.Result[0][ESBKeyValueName.KBI_UserIdStatus];
                klikbcaindividu.UserIdIBankStatus = Utility.GetStringMap(1, 0, klikbcaindividu.UserIdIBankStatusKey);
                klikbcaindividu.ChangePasswordCounter = information.Result[0][ESBKeyValueName.KBI_ChangePasswordCounter];
                klikbcaindividu.Disclaimer = information.Result[0][ESBKeyValueName.KBI_Disclaimer];
                klikbcaindividu.Language = Utility.GetStringMap(1, 10, information.Result[0][ESBKeyValueName.KBI_Language]);
                klikbcaindividu.EmailStatus = information.Result[0][ESBKeyValueName.KBI_EmailStatus];
                klikbcaindividu.ReferenceNo = information.Result[0][ESBKeyValueName.KBI_ReferenceNo];

                Params reqmalware = new Params() { Parameter = new Dictionary<string, string>() };
                reqmalware.Parameter = new Dictionary<string, string>();
                reqmalware.RequestTransType = "GetMalwareStatusByUserId";
                reqmalware.Parameter.Add("cust_id", information.Result[0][ESBKeyValueName.KBI_UserId]);
                reqmalware.WSDL = "CallStoreProcedure";
                ESBData malware = EAI.RetrieveESBData(reqmalware);
                if (malware.Result != null && malware.Result.Count != 0)
                {
                    klikbcaindividu.MalwareStatusKey = malware.Result[0]["blockstatus"];
                    klikbcaindividu.MalwareStatus = Utility.GetStringMap(1, 9, klikbcaindividu.MalwareStatusKey);
                    klikbcaindividu.MalwareBlockedDate = malware.Result[0]["createddate"];
                    klikbcaindividu.MalwareLastUpdate = malware.Result[0]["lastupdate"];
                }

                if (tandem != null)
                {
                    if (tandem.Result.Count != 0)
                    {
                        klikbcaindividu.TandemStatusKey = tandem.Result[0][ESBKeyValueName.KBI_TandemStatus];
                        klikbcaindividu.TandemStatus = Utility.GetStringMap(1, 4, klikbcaindividu.TandemStatusKey);
                    }
                }

                data.Add(klikbcaindividu);
            }
        }
    }
}