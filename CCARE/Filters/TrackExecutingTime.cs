using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CCARE.Filters
{
    public class TrackExecutingTime : ActionFilterAttribute, IExceptionFilter
    {
        private static Logger log = NLog.LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ////Redirect if no role provided -- underdevelopment
            //if (CCARE.App_Function.RoleNPrivilege.isEntityChecked(new Guid(filterContext.HttpContext.Session["RoleID"].ToString()), filterContext.RouteData.Values["controller"].ToString()) == false)
            //{
            //    filterContext.Result = new RedirectResult("~/Homepage/Index");
            //}
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //Modified by Giovanni
            //if (filterContext.HttpContext.Session["Fullname"] != null)
            //{
                string message =
                    "\r\n" +
                    filterContext.HttpContext.Session["Fullname"] + "#" + filterContext.HttpContext.Session["BranchName"] + " -> " +
                    filterContext.RouteData.Values["controller"].ToString() +
                    " -> " + filterContext.RouteData.Values["action"].ToString() +
                    //" -> OnResultExecuted \t- " +
                    " " + DateTime.Now.ToString() +
                    "\r\n";

                LogExecutingTime(message);
                LogExecutingTime("\r\n" + "-------------------------------------------------------------------------" + "\r\n");
            //}
        }

        public void OnException(ExceptionContext filterContext)
        {
            string message2 =
                "\r\n" + filterContext.RouteData.Values["controller"].ToString() +
                " -> " + filterContext.RouteData.Values["action"].ToString() +
                " -> " + filterContext.Exception.Message + " \t- " +
                DateTime.Now.ToString() +
                "\r\n";
            string message3 =
                "\r\n" + filterContext.RouteData.Values["controller"].ToString() +
                " -> " + filterContext.RouteData.Values["action"].ToString() +
                " -> OnException \t- " +
                DateTime.Now.ToString() +
                "\r\n" +
                filterContext.Exception.Message +
                "\r\n";
            string message =
                "\r\n" + filterContext.Exception;
            LogError(filterContext);
        }

        private void LogExecutingTime(string data)
        {
            log.Info(data);
        }

        private void LogError(ExceptionContext filterContext)
        {
            string message =
                filterContext.RouteData.Values["controller"].ToString() +
                " -> " + filterContext.RouteData.Values["action"].ToString() +
                " -> OnException \t- " +
                DateTime.Now.ToString();

            StringBuilder builder = new StringBuilder();
            builder
                .AppendLine()
                .AppendFormat(message)
                .AppendLine()
                .AppendFormat("Source:\t{0}", filterContext.Exception.Source)
                .AppendLine()
                .AppendFormat("Target:\t{0}", filterContext.Exception.TargetSite)
                .AppendLine()
                .AppendFormat("Type:\t{0}", filterContext.Exception.GetType().Name)
                .AppendLine()
                .AppendFormat("Message:\t{0}", filterContext.Exception.Message)
                .AppendLine()
                .AppendFormat("Stack:\t{0}", filterContext.Exception.StackTrace)
                .AppendLine()
                .AppendLine("------------------------------------------------------" + "\r\n");

            log.Error(builder);
        }

        private string cekTanggal()
        {
            var datetime = System.DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = @ConfigurationManager.AppSettings["logPath"] + datetime + ".log";

            if (!System.IO.File.Exists(filePath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = System.IO.File.CreateText(filePath))
                {

                }
            }

            return filePath;
        }

    }
}