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
        public static string AtmNoBCAByPhone(Params request)
        {
            string atmNo = null;
            request.WSDL = "UserPhoneBankingInquiry";
            ESBData data = EAI.RetrieveESBData(request);
			if (data != null && data.Result.Count > 0)
			{
				atmNo = data.Result[0]["atmno"];
			}
            return atmNo;
        }

        public static BCAByPhoneInfo BCAByPhone(Params request)
        {
            BCAByPhoneInfo bcabyphone = new BCAByPhoneInfo();
            request.WSDL = "UserPhoneBankingManagement";
            
            ESBData data = EAI.RetrieveESBData(request);

            if (data != null && data.Result.Count > 0)
            {
                StringDictionary entry = data.Result[0];
                DateTime? registrationDate = Utility.ParseExact(entry[PhoneBankingInquiryStatusResultKeyName.RegistrationDate], "yyyy-MM-dd");
                DateTime? lastUpdateDate = Utility.ParseExact(entry[PhoneBankingInquiryStatusResultKeyName.LastUpdateDate], "yy/MM/dd HH:mm");
                DateTime? lastPinChangeDate = Utility.ParseExact(entry[PhoneBankingInquiryStatusResultKeyName.LastPinChangeDate], "yyyyMMddHHmmss");

                bcabyphone.CustomerId = entry[PhoneBankingInquiryStatusResultKeyName.CustomerId];
                bcabyphone.Name = entry[PhoneBankingInquiryStatusResultKeyName.Name];
                bcabyphone.CustomerCategoryKey = entry[PhoneBankingInquiryStatusResultKeyName.CustomerCategory];
                //bcabyphone.CustomerCategory = Utility.GetStringMap(4, 19, entry[PhoneBankingInquiryStatusResultKeyName.CustomerCategory]);
                bcabyphone.CustomerCategory = Utility.GetStringMap(4, 11, bcabyphone.CustomerCategoryKey);
                bcabyphone.AccountNo1 = entry[PhoneBankingInquiryStatusResultKeyName.AccountNo1];
                bcabyphone.AccountNo2 = entry[PhoneBankingInquiryStatusResultKeyName.AccountNo2];
                bcabyphone.AccountNo3 = entry[PhoneBankingInquiryStatusResultKeyName.AccountNo3];
                bcabyphone.AccountNo4 = entry[PhoneBankingInquiryStatusResultKeyName.AccountNo4];
                bcabyphone.RegistrationDate = registrationDate;
                bcabyphone.LastUpdateDate = lastUpdateDate;
                bcabyphone.LastVerifyFlagUpdate = lastPinChangeDate;
                bcabyphone.UpdateByUserGroup = entry[PhoneBankingInquiryStatusResultKeyName.UpdateByUserGroup];
                bcabyphone.UpdateByUserNumber = entry[PhoneBankingInquiryStatusResultKeyName.UpdateByUserNum];
                bcabyphone.WrongPinCounter = entry[PhoneBankingInquiryStatusResultKeyName.WrongPinCounter];
                bcabyphone.PinChangeCounter = entry[PhoneBankingInquiryStatusResultKeyName.PinChangeRequired];
                bcabyphone.LastStatusKey = entry[PhoneBankingInquiryStatusResultKeyName.LastStatus];
                //bcabyphone.LastStatus = Utility.GetStringMap(4, 9, entry[PhoneBankingInquiryStatusResultKeyName.LastStatus]);
                bcabyphone.LastStatus = Utility.GetStringMap(4, 1, bcabyphone.LastStatusKey);
            }
            return bcabyphone;
        }

    }
}