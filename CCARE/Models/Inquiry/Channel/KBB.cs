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
        public static List<KlikBCABisnisInfo> KBB(Params request, Channel channel, string userId = null)
        {
            List<KlikBCABisnisInfo> kbb = new List<KlikBCABisnisInfo>();
            request.WSDL = "ESBDBDelimiter";
            ESBData data = EAI.RetrieveESBData(request);

            userId = string.IsNullOrEmpty(userId)? channel.UserId : userId; 

            if (data != null && data.Result.Count != 0) 
            {
                foreach (StringDictionary entry in data.Result)
                {
                    KlikBCABisnisInfo model = new KlikBCABisnisInfo();
                    if (!string.IsNullOrEmpty(channel.CorpId) && !string.IsNullOrEmpty(userId))
                    {
                        model.CorpID = channel.CorpId;
                        model.UserID = userId;
                        DateTime? lastLoginDate = Utility.ParseExact(entry["last_login_dt"], "yyyy-MM-dd HH:mm:ss");
                        DateTime? registerDate = Utility.ParseExact(entry["registered_dt"], "yyyy-MM-dd HH:mm:ss");

                        model.UserName = entry["nm"];
                        model.UserID = userId;
                        model.CorpID = channel.CorpId;
                        model.LastLoginDate = lastLoginDate;
                        model.RegisterDate = registerDate;

                        model.LastStatusKey = entry["is_login"];
                        //model.LastStatus = Utility.GetStringMap(5, 4, entry["is_login"]);
                        model.LastStatus = Utility.GetStringMap(1, 1, model.LastStatusKey);
                        //kbb.LastStatus = new KeyValuePair<int, string>(Int32.Parse(entry["is_login"]), Utility.GetStringMap(5, 4, entry["is_login"]));
                        model.Email1 = entry["email1"];
                        model.Email2 = entry["email2"];
                    }
                    else if (!string.IsNullOrEmpty(channel.EmailAddress))
                    {
                        model.UserID = channel.UserId;
                        model.CorpID = entry["cd"];
                        model.CorpName = entry["nm"];
                        model.Email1 = entry["email1"];
                        model.Email2 = entry["email2"];
                    }

                    else if (!string.IsNullOrEmpty(channel.AccountNo))
                    {
                        model.CorpID = entry["corporate_code"];
                        model.CorpName = entry["corporate_name"];
                        model.Email1 = entry["corporate_email_address_1"];
                        model.Email2 = entry["corporate_email_address_2"];
                        model.AccountNo = entry["account_number"];
                    }
                    kbb.Add(model);
                }
            }
            return kbb;
        }
    }
}