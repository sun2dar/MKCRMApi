//Created by Sumardi
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using System.Web.Security;

namespace CCARE.Filters
{
    public class LoginFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string loginController = "Login";

            //ByPass Error Controller
            string errorController = "Error";
            if ((string)(filterContext.RouteData.Values["Controller"]) != errorController)
            {
                //Check Session
                if (filterContext.HttpContext.Session["IsLogin"] == null)
                {
                    if ((string)(filterContext.RouteData.Values["Controller"]) == loginController)
                    {

                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Login/Index");
                    }
                }
            }
        }
    }
}