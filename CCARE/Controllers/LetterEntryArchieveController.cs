using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using CCARE.jqGrid;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.Models.MasterData;
using CCARE.Models.Transaction;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;

namespace CCARE.Controllers
{
    public partial class LetterEntryController : Controller
    {
        //public ActionResult EditArchive(Guid id)
        //{
        //    ArchiveDb ArDb = new ArchiveDb();
        //    LetterEntryArchive model = ArDb.letterentriesarchive.Find(id);
        //    List<LetterEntryArchive> notelist = ArDb.letterentriesarchive.Where(x => x.RequestID == id).ToList();
        //    ViewBag.notel = notelist;
        //    List<AnnotationArchive> doclist = ArDb.annotationarchive.Where(x => x.ObjectID == model.LetterEntryID).ToList();
        //    ViewBag.docl = doclist;
        //    return View(model);
        //}
    }
}
