using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace CCARE.jqGrid
{
    public class HandleJsonExceptionAttribute : ActionFilterAttribute
    {
        // next class example are from the http://www.dotnetcurry.com/ShowArticle.aspx?ID=496
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Exception != null)
            {
                filterContext.HttpContext.Response.StatusCode =
                    (int)System.Net.HttpStatusCode.InternalServerError;

                var exInfo = new List<ExceptionInformation>();
                for (Exception ex = filterContext.Exception; ex != null; ex = ex.InnerException)
                {
                    PropertyInfo propertyInfo = ex.GetType().GetProperty("ErrorCode");
                    exInfo.Add(new ExceptionInformation()
                    {
                        Message = ex.Message,
                        Source = ex.Source,
                        StackTrace = ex.StackTrace
                    });
                }
                filterContext.Result = new JsonResult() { Data = exInfo };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}