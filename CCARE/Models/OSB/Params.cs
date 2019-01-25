using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class Params
    {
        public string RequestTransType { get; set; }
        public Dictionary<string, string> Parameter { get; set; }
        public string WSDL { get; set; }

        public bool IsValid()
        {
            if (String.IsNullOrEmpty(this.RequestTransType))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}