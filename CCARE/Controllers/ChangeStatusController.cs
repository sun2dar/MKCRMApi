using System;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using CCARE.Models;

namespace CCARE.Controllers
{
    public class ChangeStatusController : Controller
    {
        public ActionResult WriteMessageToLog()
        {
            string entityType = string.IsNullOrEmpty(Request["EntityType"]) ? string.Empty : Request["EntityType"];
            string statusType = string.IsNullOrEmpty(Request["StatusType"]) ? string.Empty : Request["StatusType"];
            string oldval = string.IsNullOrEmpty(Request["OldValue"]) ? string.Empty : Request["OldValue"];
            string newval = string.IsNullOrEmpty(Request["NewValue"]) ? string.Empty : Request["NewValue"];
            string message = string.IsNullOrEmpty(Request["Message"]) ? string.Empty : Request["Message"];
            ChangeStatusLog.Message(entityType, statusType, oldval, newval, message);

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
