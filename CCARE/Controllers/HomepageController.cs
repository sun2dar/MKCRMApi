﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCARE.Controllers
{
    public class HomepageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
