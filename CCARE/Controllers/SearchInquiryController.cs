using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using System.Globalization;
using System.Collections.Specialized;
using BCA.CRM.OSB.Model;
using CCARE.Models;
using CCARE.Models.General;

namespace CCARE.Controllers
{
    public class SearchInquiryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
