using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;

namespace CCARE.Controllers
{
    public class NotificationController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        /*
         * Task - Activity Status
         * 0 -> Open
         * 1 -> Complete
         * 2 -> Canceled
         */
        public JsonResult RetreiveNotification(string type = null)
        {
            Guid CurrentUserID = new Guid(Session["CurrentUserID"].ToString());

            /*
             * Section Task
             */
            int taskCount = 0;
            var modelTask = db.task.Where(x => x.OwnerID == CurrentUserID).Where(x => x.ActivityStatus == 0);
            taskCount = modelTask.Count();
            modelTask = modelTask.Take(10);
            var dataTask = modelTask.Select(x => new { 
                            ID = x.TaskID,
                            TaskID = x.TaskNumber,
                            From = x.CreatedByName
                        });

            var jsonData = new { 
                                    flag = true, 
                                    Task = dataTask.ToArray(), 
                                    TaskCount = taskCount 
                                };

            return Json(jsonData);
        }

    }
}
