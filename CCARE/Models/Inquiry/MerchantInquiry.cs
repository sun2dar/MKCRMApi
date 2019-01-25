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
    public class MerchantInquiry
    {
        public static Merchant Search(Params request)
        {
            Merchant merchant = new Merchant();
            try
            {
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                param.RequestTransType = "GetMerchantInfoByTerminalIdFromTandem";
                param.Parameter.Add("merchantId", request.Parameter.ContainsKey("merchantId") ? request.Parameter["merchantId"].Trim() : string.Empty);
                param.WSDL = "POSManagement";
                ESBData tandem = EAI.RetrieveESBData(param);
                if (tandem.Result != null && tandem.Result.Count > 0)
                {
                    string retailerId = tandem.Result[0]["retailerId"];
                    if (retailerId.Length == 12)
                    {
                        request.Parameter = new Dictionary<string, string>();
                        request.Parameter.Add("merchantOrgn", "005");
                        request.Parameter.Add("merchantAcct", retailerId.Substring(3));
                        request.WSDL = "Merchant";

                        ESBData data = EAI.RetrieveESBData(request);

                        merchant.Account = data.Result[0]["acct"];
                        merchant.Name = data.Result[0]["idName"];
                        merchant.City = data.Result[0]["idCity"];
                        merchant.Address1 = data.Result[0]["address1"];
                        merchant.Address2 = data.Result[0]["address2"];
                        merchant.Address3 = data.Result[0]["address3"];
                        merchant.Address4 = data.Result[0]["address4"];
                        merchant.ZipCode = data.Result[0]["zipCode"];
                        merchant.TelephoneNo = data.Result[0]["phoneNmbr"];
                        merchant.MerchantName = data.Result[0]["merchantName"];
                        merchant.UserNumber = data.Result[0]["edUserNbr"];

                        merchant.TandemMerchantName = tandem.Result[0]["merchantname"];
                        merchant.RetailerId = tandem.Result[0]["retailerid"];
                        merchant.RetailerGroup = tandem.Result[0]["retailergroup"];
                        merchant.MerchantType = Utility.GetDisplayText("StatusMapper", "Merchant", "Jenis Merchant", tandem.Result[0]["merchanttype"]);
                        merchant.TerminalType = tandem.Result[0]["terminaltype"];
                        merchant.TerminalStatus = Utility.GetDisplayText("StatusMapper", "Merchant", "Terminal Status", tandem.Result[0]["terminalstatus"]);

                        merchant.TerminalId = param.Parameter["merchantId"];
                    }
                }
            }
            catch (Exception e)
            {

            }
            return merchant;

        }
    }
}