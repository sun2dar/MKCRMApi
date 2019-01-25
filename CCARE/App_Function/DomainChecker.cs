using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.App_Function
{
    public class DomainChecker
    {
        //isDomain name checker
        public static bool isDomainFormat(string domain, string input)
        {
            if (input.Contains("\\") && input.ToLower().Contains(domain.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Concat domain and username
        public static string toDomainName(String domain, String input)
        {
            ////If input contain domain name do nothing and return input
            //if (input.Contains("\\") && input.ToLower().Contains(domain.ToLower()))
            //{
            //    return input;
            //}
            //else
            //{
                return domain + "\\" + input;
            //}
        }
    }
}