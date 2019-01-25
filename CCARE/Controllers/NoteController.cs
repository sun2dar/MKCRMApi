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
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        //Note Submit process
        public ActionResult notesSubmit(Annotation model, string actionType, Guid ObjectID, Guid OwningBusinessUnit)
        {
            List<string> errorMessage = new List<string>();
            if (ModelState.IsValid)
            {
                model.ModifiedByID = new Guid(Session["CurrentUserID"].ToString());

                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                if (actionType == "Create")
                {
                    model.AnnotationID = Guid.NewGuid();
                    model.NoteText = "";
                    model.CreatedByID = new Guid(Session["CurrentUserID"].ToString());
                    model.BusinessUnitID = OwningBusinessUnit;
                    results = db.SpInsertNote(model, ObjectID);
                    ViewBag.tempAnnotationID = model.AnnotationID;
                    return Json(new { AnnotationID = model.AnnotationID });
                }
                else if (actionType == "Edit")
                {
                    model.ObjectID = ObjectID;
                    results = db.SpUpdateNote(model);
                    var tableNote = db.SpGetNotes(ObjectID).ToList();
                    return Json(new { Data = tableNote.First().NoteText });
                }
                return Json(new { tes = "test" });
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                return View();
            }
        }

        public ActionResult CreateNote(Guid ObjectID, Guid BusinessUnitID)
        {
            Annotation model = new Annotation();
            model.CreatedOn = DateTime.Now;
            return notesSubmit(model, "Create", ObjectID, BusinessUnitID);
        }

        public ActionResult EditNote(Annotation model)
        {
            string CreatedBy = Session["Fullname"].ToString();
            model.ModifiedOn = DateTime.Now;
            string CreatedOn = model.ModifiedOn.ToString();
            string tempNoteText = model.NoteText;
            string formatNote = "\nModified By  " + CreatedBy + "    on   " + CreatedOn + "\n";

            Annotation request = db.annotation.Find(model.AnnotationID);

            Annotation prevNote = new Annotation();
            prevNote = db.annotation.Where(x => x.ObjectID == request.ObjectID && x.IsDocument == false).OrderByDescending(x => x.ModifiedOn).FirstOrDefault();

            model.NoteText = "\n\n" + formatNote + tempNoteText + "\n\n-------------------------------------------------------------------------------\n" + prevNote.NoteText;
            
            return notesSubmit(model, "Edit", prevNote.ObjectID, Guid.Empty);
        }

        //Remove pending
        public ActionResult AppendToNote(Guid ObjectID, Guid BusinessUnitID, string reason)
        {
            Request request = db.request.Find(ObjectID);
            Annotation model = new Annotation();
            
            string CreatedBy = Session["Fullname"].ToString();
            model.ModifiedByID = new Guid(Session["CurrentUserID"].ToString());
            model.ModifiedOn = DateTime.Now;
            string CreatedOn = model.ModifiedOn.ToString();
            
            if (request.Notes.Count() > 0)
            {
                model = db.annotation.Find(request.Notes.FirstOrDefault().AnnotationID);
                model.NoteText = reason;
            }
            else
            {
                model.CreatedOn = DateTime.Now;
                model.AnnotationID = Guid.NewGuid();
                model.NoteText = "";
                model.CreatedByID = new Guid(Session["CurrentUserID"].ToString());
                model.BusinessUnitID = BusinessUnitID;
                db.SpInsertNote(model, ObjectID);
            }

            Annotation prevNote = new Annotation();
            string tempNoteText = reason;
            string formatNote = "\nModified By  " + CreatedBy + "    on   " + CreatedOn + "\n";
            prevNote = db.annotation.Where(x => x.ObjectID == request.RequestID && x.IsDocument == false).FirstOrDefault();
            model.NoteText = "\n\n" + formatNote + tempNoteText + "\n\n-------------------------------------------------------------------------------\n" + prevNote.NoteText;

            return notesSubmit(model, "Edit", ObjectID, Guid.Empty);
        }

        public ActionResult GetRequestAnnotation(Guid requestId) 
        {
            Request request = new Request();
            request = db.request.Find(requestId);

            if (request.Notes.Where(x => x.IsDocument == false).Count() > 0)
            {
                return Json(new { AnnotationID = request.Notes.Where(x => x.IsDocument == false).FirstOrDefault().AnnotationID });
            }
            else
            {
                return CreateNote(new Guid(request.RequestID.ToString()), new Guid(request.BusinessUnitID.ToString()));
            }
        }

        public JSONResponse SubmitFile(Annotation model, ActionType actionType, string entityName)
        {
            JSONResponse result = new JSONResponse();
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.LetterTemplate.Alert_Success;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, string.Empty);
                switch (actionType)
                {
                    case ActionType.Create:
                        results = db.Annotation_Insert(model);
                        break;
                    case ActionType.Edit:
                        results = db.Annotation_Update(model);
                        break;
                    case ActionType.Delete:
                        results = db.Annotation_Delete(model);
                        break;
                    default:
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = "";
                    if (entityName == "request")
                    {
                        url = u.Action("Edit", "Request", new { id = model.ObjectID, success = successMessage, position = "documents" });
                    }
                    else if (entityName == "task")
                    {
                        url = u.Action("Edit", "Task", new { id = model.ObjectID, success = successMessage, position = "documents" });
                    }

                    result.Value = true;
                    result.Response = url;
                    return result;
                }
                else
                {
                    result.Value = false;
                    result.Response = results.Value.ToString();
                    return result;
                }
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var error = ModelState[key].Errors.FirstOrDefault();
                    if (error != null)
                    {
                        errorMessage.Add(error.ErrorMessage);
                    }
                }

                result.Value = false;
                result.Response = errorMessage.First();
                return result;
            }
        }

        [HttpPost]
        public ActionResult DeleteDocument()
        {
            JSONResponse result = new JSONResponse();
            string entityName = Request["entityname"].ToString();

            try
            {
                var annotationID = HttpContext.Request.Form["AnnotationID"];
                Annotation annotation = db.annotation.Where(x => x.AnnotationID == new Guid(annotationID)).FirstOrDefault();
                result = SubmitFile(annotation, ActionType.Delete, entityName);
            }
            catch (Exception e)
            {
                result.Value = false;
                result.Response = e.Message;
            }

            ContentResult response = new ContentResult();
            response.ContentType = "text/html";
            response.Content = new JavaScriptSerializer().Serialize(result);
            return response;
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            JSONResponse result = new JSONResponse();

            try
            {
                if (HttpContext.Request.Files.AllKeys.Any() && HttpContext.Request.Form.AllKeys.Any())
                {
                    string entityName = Request["parentEntityName"].ToString();
                    var file = HttpContext.Request.Files["import_filepath"];
                    var objectID = HttpContext.Request.Form["ObjectID"];
                    var modifiedBy = Session["CurrentUserID"].ToString(); 
                    var bussinessUnitID = Session["BusinessUnitID"].ToString(); 
                    var noteText = HttpContext.Request.Form["NoteText"];
                    var objectTypeCode = ConfigurationManager.AppSettings["typeCodeLetterTemplate"];
                    var subject = ConfigurationManager.AppSettings["subjectLetterTemplate"];
                    var letterTemplateID = new Guid(objectID);
                    if (file != null)
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);

                        Annotation annotation = db.annotation.Where(x => x.ObjectID == letterTemplateID).FirstOrDefault();
                        bool isCreate = annotation == null ? true : false;
                        var fileSize = file.ContentLength;

                        annotation = new Annotation();
                        annotation.AnnotationID = Guid.NewGuid();

                        // Open a file and read the contents into a byte array.
                        System.IO.Stream stream = file.InputStream;
                        byte[] byteData = new byte[stream.Length];
                        stream.Read(byteData, 0, byteData.Length);
                        stream.Close();

                        // Encode the data using base64.
                        string encodedData = System.Convert.ToBase64String(byteData);

                        annotation.ObjectID = letterTemplateID;
                        annotation.ObjectTypeCode = int.Parse(objectTypeCode);
                        annotation.MimeType = file.ContentType;
                        annotation.IsDocument = true;
                        annotation.Subject = subject;
                        annotation.DocumentBody = encodedData;
                        annotation.FileSize = fileSize;
                        annotation.FileName = System.IO.Path.GetFileName(file.FileName);
                        annotation.BusinessUnitID = new Guid(bussinessUnitID);
                        annotation.ModifiedByID = new Guid(modifiedBy);
                        annotation.NoteText = noteText;

                        result = SubmitFile(annotation, ActionType.Create, entityName);
                    }
                    else
                    {
                        throw new Exception(Resources.LetterTemplate.Alert_NoFileAttached);
                    }
                }
                else
                {
                    throw new Exception(Resources.LetterTemplate.Alert_NoFileAttached);
                }
            }
            catch (Exception e)
            {
                result.Value = false;
                result.Response = e.Message;
            }

            ContentResult response = new ContentResult();
            response.ContentType = "text/html";
            response.Content = new JavaScriptSerializer().Serialize(result);
            return response;
        }

        [HttpPost]
        public void DownloadFile(Guid annotationID)
        {
            Annotation annotation = db.annotation.Where(p => p.AnnotationID == annotationID).FirstOrDefault();
            if (annotation != null)
            {
                byte[] byteContent = Convert.FromBase64String(annotation.DocumentBody);
                string fileName = annotation.FileName.Replace(" ", "-");

                Response.Clear();
                Response.Charset = string.Empty;
                Response.ContentType = annotation.MimeType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.OutputStream.Write(byteContent, 0, byteContent.Length);
            }
        }
    }
}
