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

        public static List<MBCAInfo> MBLF(Params request)
        {
            List<MBCAInfo> mbca = new List<MBCAInfo>();

            request.WSDL = "ESBDBDelimiter";

            ESBData data = EAI.RetrieveESBData(request);

            if (data != null && data.Result.Count != 0)
            {
                foreach (StringDictionary entry in data.Result)
                {
                    MBCAInfo model = new MBCAInfo();
                    if (!string.IsNullOrEmpty(entry["mb_custid"]))
                    {
                        model.MobileNo = entry["mb_custid"];
                    }

                    if (model.AtmNo == null)
                    {
                        model.AtmNo = entry["mb_cardnumber"];
                    }

                    model.CustomerName = entry["mb_custname"];
                    model.ActivationDate = Utility.ParseExact("mb_createddate", "MM/dd/yyyy hh:mm:ss tt");
                    model.LastTransactionDate = Utility.ParseExact(entry["mb_lastlogindate"], "MM/dd/yyyy hh:mm:ss tt");
                    model.LastUpdateDate = Utility.ParseExact(entry["mb_lastupdate"], "MM/dd/yyyy hh:mm:ss tt");
                    model.UpdateBy = entry["mb_updateofficer"];
                    model.WrongPinCounter = entry["mb_trlcnt"];
                    model.PinActivation = entry["mb_flagfin"];
                    model.ActivationFinDate = Utility.ParseExact(entry["mb_flagfindate"], "MM/dd/yyyy hh:mm:ss tt");
                    model.ActivationFin = entry["mb_userfin"];
                    model.BlockStatusKey = entry["mb_status"];
                    model.BlockStatus = Utility.GetStringMap(2, 1, model.BlockStatusKey);
                    model.ChangePinCounter = entry["mb_pswdchgflag"];
                    model.Disclaimer = entry["mb_agree"];
                    model.Language = Utility.GetStringMap(2, 10, entry["mb_language"]);

                    model.TandemCustomerName = entry["mblf_name"];
                    string blockstatus = entry["mblf_useridstatus"];
                    model.TandemBlockStatus = string.IsNullOrEmpty(blockstatus) ? string.Empty : Utility.GetStringMap(2, 1, blockstatus);
                    model.TandemCardNo = entry["mblf_crdnum"];
                    model.TandemRegistrationDate = Utility.ParseExact(entry["mblf_regdate"], "yyMMdd");
                    model.TandemRegistrationTime = Utility.ParseExact(entry["mblf_regtime"], "HHmmss");
                    model.TandemHpNo = entry["mblf_hpnum"];

                    mbca.Add(model);
                }

            }
            return mbca;
        }


        public static List<MBCAInfo> MBCA(Params request)
        {
            List<MBCAInfo> mbca = new List<MBCAInfo>();

            request.WSDL = "ESBDBDelimiter";

            ESBData data = EAI.RetrieveESBData(request);

            if (data != null && data.Result.Count != 0)
            {
                foreach (StringDictionary entry in data.Result)
                {
                    MBCAInfo model = new MBCAInfo();
                    if (!string.IsNullOrEmpty(entry[ESBKeyValueName.MB_MobileNo]))
                    {
                        model.MobileNo = entry[ESBKeyValueName.MB_MobileNo];
                    }

                    if (model.AtmNo == null)
                    {
                        model.AtmNo = entry[ESBKeyValueName.MB_ATMCardNo];
                    }

                    model.CustomerName = entry[ESBKeyValueName.MB_CustomerName];
                    model.ActivationDate = Utility.ParseExact(entry[ESBKeyValueName.MB_ActivationDate], "MM/dd/yyyy hh:mm:ss tt");
                    model.LastTransactionDate = Utility.ParseExact(entry[ESBKeyValueName.MB_LastTransactionDate], "MM/dd/yyyy hh:mm:ss tt");
                    model.LastUpdateDate = Utility.ParseExact(entry[ESBKeyValueName.MB_LastUpdateDate], "MM/dd/yyyy hh:mm:ss tt");
                    model.UpdateBy = entry[ESBKeyValueName.MB_UpdateBy];
                    model.WrongPinCounter = entry[ESBKeyValueName.MB_WrongPinCounter];
                    model.PinActivation = entry[ESBKeyValueName.MB_PinActivation];
                    model.ActivationFinDate = Utility.ParseExact(entry[ESBKeyValueName.MB_ActivationFinDate], "MM/dd/yyyy hh:mm:ss tt");
                    model.ActivationFin = entry[ESBKeyValueName.MB_ActivationFin];
                    model.BlockStatusKey = entry[MobileBankingTandemInquiryStatusResultKeyName.BlockStatus];
                    //model.BlockStatus = Utility.GetStringMap(2, 1, entry[MobileBankingTandemInquiryStatusResultKeyName.BlockStatus]);
                    model.BlockStatus = Utility.GetStringMap(2, 1, model.BlockStatusKey);
                    model.ChangePinCounter = entry[MobileBankingInquiryStatusResultKeyName.ChangePinCounter];
                    model.Disclaimer = entry[ESBKeyValueName.MB_Disclaimer];
                    //model.Language = Utility.GetStringMap(2, 20, entry[ESBKeyValueName.MB_Language]);
                    model.Language = Utility.GetStringMap(2, 10, entry[ESBKeyValueName.MB_Language]);

                    Params paramTandemList = new Params() { Parameter = new Dictionary<string, string>() };
                    paramTandemList.RequestTransType = "GetTandemStatusMBank";
                    paramTandemList.Parameter.Add("atmNo", model.AtmNo);
                    paramTandemList.Parameter.Add("mobileNo", model.MobileNo);
                    paramTandemList.WSDL = "UserMobileBankingManagement";

                    ESBData tandemRecords = new ESBData() { Result = new List<StringDictionary>() };
                    tandemRecords = EAI.RetrieveESBData(paramTandemList);
                    StringDictionary tandemRecord = null;

                    if (tandemRecords != null)
                    {
                        if (tandemRecords.Result.Count > 0)
                        {
                            tandemRecord = tandemRecords.Result[0];
                            model.TandemCustomerName = tandemRecord[MobileBankingTandemInquiryStatusResultKeyName.CustomerName];
                            string blockstatus = tandemRecord[MobileBankingTandemInquiryStatusResultKeyName.UserIdStatus];
                            //model.TandemBlockStatus = string.IsNullOrEmpty(blockstatus) ? string.Empty : Utility.GetStringMap(2, 1, blockstatus);
                            model.TandemBlockStatus = string.IsNullOrEmpty(blockstatus) ? string.Empty : Utility.GetStringMap(2, 1, blockstatus);
                            model.TandemCardNo = tandemRecord[MobileBankingTandemInquiryStatusResultKeyName.AtmCardNo];
                            model.TandemRegistrationDate = Utility.ParseExact(tandemRecord[MobileBankingTandemInquiryStatusResultKeyName.RegistrationDate], "yyMMdd");
                            model.TandemRegistrationTime = Utility.ParseExact(tandemRecord[MobileBankingTandemInquiryStatusResultKeyName.RegistrationTime], "HHmmss");
                        }
                    }

                    mbca.Add(model);
                }

            }

            return mbca;
        }
    }
}