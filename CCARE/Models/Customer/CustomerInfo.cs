using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class CustomerInfo
    {
        public Customer Customer { set; get; }
        // -1: not update, 0: insert, 1: update
        public int StatusUpdate { set; get; } 
    }
}