using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using System.Web.Script.Serialization;
using System.Configuration;

namespace CCARE.Controllers
{
    public partial class NoteController : Controller
    {
        // private ArchiveDb ArDb = new ArchiveDb();
        
        /*
         * Archieve Section
         */

        //[HttpPost]
        //public void DownloadFileArchive(Guid annotationID)
        //{

        //    AnnotationArchive annotation = ArDb.annotationarchive.Where(p => p.AnnotationID == annotationID).FirstOrDefault();
        //    if (annotation != null)
        //    {
        //        byte[] byteContent = Convert.FromBase64String(annotation.DocumentBody);
        //        string fileName = annotation.FileName.Replace(" ", "-");

        //        Response.Clear();
        //        Response.Charset = string.Empty;
        //        Response.ContentType = annotation.MimeType;
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        //        Response.OutputStream.Write(byteContent, 0, byteContent.Length);
        //    }
        //}

    }
}
