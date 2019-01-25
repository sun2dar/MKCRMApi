using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCARE.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult UnAuthorized()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult DBTimeoutException()
        {
            return View();
        }

        public ActionResult CustomerNotFound()
        {
            return View();
        }
    }
}
