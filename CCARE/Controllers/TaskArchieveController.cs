using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using System.Windows.Forms;
using System.Configuration;

namespace CCARE.Controllers
{
    public partial class TaskController : Controller
    {
        /*
         * Archieve Section
         */

        //public ActionResult EditArchive(Guid id)
        //{

        //    ArchiveDb ArDb = new ArchiveDb();
        //    TaskArchive model = ArDb.taskarchive.Find(id);

        //    List<TaskArchive> notelist = ArDb.taskarchive.Where(x => x.RequestID == id).ToList();
        //    ViewBag.notel = notelist;

        //    List<AnnotationArchive> doclist = ArDb.annotationarchive.Where(x => x.ObjectID == model.TaskID.Value).ToList();
        //    ViewBag.docl = doclist;


        //    return View(model);
        //}

    }
}
