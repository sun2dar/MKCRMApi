using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Script.Serialization;
using CCARE.jqGrid;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;

namespace CCARE.Controllers
{
    public class LetterTemplateController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var businessunitid = new Guid(Convert.ToString(Session["BusinessUnitID"]));
            var systemuserid = new Guid(Convert.ToString(Session["CurrentUserID"]));

            var types = db.pickList.ToList()
                .Where(a => a.EntityName.Equals("lettertemplate") && a.AttributeName.Equals("type"))
                .OrderBy(a => a.DisplayOrder)
                .GroupBy(p => new
                {
                    p.EntityName,
                    p.AttributeName,
                    p.DisplayOrder,
                    p.AttributeValue,
                    p.label
                })
                .Select(p => new SelectListItem { Text = p.Key.label, Value = p.Key.AttributeValue.ToString() })
                .ToList();

            var languages = db.pickList.ToList()
                .Where(a => a.EntityName.Equals("lettertemplate") && a.AttributeName.Equals("language"))
                .OrderBy(a => a.DisplayOrder)
                .GroupBy(p => new
                {
                    p.EntityName,
                    p.AttributeName,
                    p.DisplayOrder,
                    p.AttributeValue,
                    p.label
                })
                .Select(p => new SelectListItem { Text = p.Key.label, Value = p.Key.AttributeValue.ToString() })
                .ToList();

            var userid = db.systemUser.Single(p => p.SystemUserId == systemuserid);

            ViewData["ddl_types"] = types;
            ViewData["ddl_languages"] = languages;

            LetterTemplate model = new LetterTemplate();
            model.OwnerID = systemuserid;
            model.OwnerName = userid.FullName;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(LetterTemplate model)
        {
            model.LetterTemplateId = Guid.NewGuid();
            model.ModifiedBy = model.OwnerID;
            model.OwningBusinessUnit = new Guid(Convert.ToString(Session["BusinessUnitID"]));
            return SubmitForm(model, ActionType.Create);
        }

        public ActionResult Edit(Guid Id)
        {
            LetterTemplate model = db.letterTemplate.Find(Id);
            Annotation annotation = db.annotation.Where(x => x.ObjectID == model.LetterTemplateId).FirstOrDefault();

            LetterTemplateView ltv = new LetterTemplateView();
            ltv.LetterTemplate = model;
            ltv.Annotation = annotation;

            var languages = db.pickList.ToList()
                .Where(a => a.EntityName.Equals("lettertemplate") && a.AttributeName.Equals("language"))
                .OrderBy(a => a.DisplayOrder)
                .GroupBy(p => new
                {
                    p.EntityName,
                    p.AttributeName,
                    p.DisplayOrder,
                    p.AttributeValue,
                    p.label
                })
                .Select(p => new SelectListItem { Text = p.Key.label, Value = p.Key.AttributeValue.ToString() })
                .ToList();

            ViewData["ddl_types"] = new List<SelectListItem>() { new SelectListItem { Text = model.TypeLabel, Value = model.Type.Value.ToString(), Selected = true } };
            ViewData["ddl_languages"] = languages;

            return View(ltv);
        }

        [HttpPost]
        public ActionResult Edit(LetterTemplateView model)
        {
            LetterTemplate lt = model.LetterTemplate;
            return SubmitForm(lt, ActionType.Edit);
        }

        [HttpPost]
        public ActionResult ChangeStatus(LetterTemplateView model)
        {
            return SubmitForm(model.LetterTemplate, ActionType.ChangeStatus);
        }

        [HttpPost]
        public ActionResult Delete(LetterTemplateView model)
        {
            return SubmitForm(model.LetterTemplate, ActionType.Delete);
        }

        public ActionResult SubmitForm(LetterTemplate model, ActionType actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.LetterTemplate.Alert_Success;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, string.Empty);
                model.ModifiedBy = new Guid(Convert.ToString(Session["CurrentUserID"]));
                switch (actionType)
                {
                    case ActionType.Create:
                        results = db.LetterTemplate_Insert(model);
                        break;
                    case ActionType.Edit:
                        results = db.LetterTemplate_Update(model);
                        break;
                    case ActionType.Delete:
                        results = db.LetterTemplate_Delete(model);
                        break;
                    case ActionType.ChangeStatus:
                        results = db.LetterTemplate_ChangeStatus(model);
                        break;
                    default:
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "LetterTemplate", new { id = model.LetterTemplateId, success = successMessage });
                    string urlNew = u.Action("Create", "LetterTemplate");

                    var jsonData = new { flag = true, Message = url, urlNew = urlNew };
                    return Json(jsonData);
                }
                else
                {
                    var jsonData = new { flag = false, Message = results.Value.ToString() };
                    return Json(jsonData);
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
                var jsonData = new { flag = false, Message = errorMessage.First() };
                return Json(jsonData);
            }
        }

        public JSONResponse SubmitFile(Annotation model, ActionType actionType)
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
                    default:
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "LetterTemplate", new { id = model.ObjectID, success = successMessage });

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

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            int stateCode = "1".Equals(getFilter) ? 0 : 1;
            var model = db.letterTemplate.Where(x => x.DeletionStateCode == 0 && x.StateCode == stateCode);
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchID":
                        model = model.Where("it.AutoID.Contains(@0)", getVal);
                        break;
                    case "searchName":
                        model = model.Where("(string.IsNullOrEmpty(it.Name) ? string.Empty : it.Name).Contains(@0)", getVal);
                        break;
                    case "searchType":
                        model = model.Where("(string.IsNullOrEmpty(it.TypeLabel) ? string.Empty : it.TypeLabel).Contains(@0)", getVal);
                        break;
                }
            }
            totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
                    break;
                case "TypeLabel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TypeLabel) : model.OrderBy(x => x.TypeLabel);
                    break;
                case "LanguageLabel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LanguageLabel) : model.OrderBy(x => x.LanguageLabel);
                    break;
                case "OwnerName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.OwnerName) : model.OrderBy(x => x.OwnerName);
                    break;
                case "Description1":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Description1) : model.OrderBy(x => x.Description1);
                    break;
                case "Description2":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Description2) : model.OrderBy(x => x.Description2);
                    break;
                case "Description3":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Description3) : model.OrderBy(x => x.Description3);
                    break;
                case "Description4":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Description4) : model.OrderBy(x => x.Description4);
                    break;
                case "CreatedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
                case "CreatedByName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedByName) : model.OrderBy(x => x.CreatedByName);
                    break;
                case "ModifiedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedOn) : model.OrderBy(x => x.ModifiedOn);
                    break;
                case "ModifiedByName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedByName) : model.OrderBy(x => x.ModifiedByName);
                    break;
                case "OwningBusinessUnit":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.OwningBusinessUnit) : model.OrderBy(x => x.OwningBusinessUnit);
                    break;
                case "StateLabel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StateLabel) : model.OrderBy(x => x.StateLabel);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
            }
            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            var data = model
                .Select(x => new
                {
                    Id = x.LetterTemplateId,
                    Name = x.Name,
                    Type = x.Type,
                    TypeLabel = x.TypeLabel,
                    AutoID = x.AutoID,
                    Language = x.Language,
                    LanguageLabel = x.LanguageLabel,
                    OwnerName = x.OwnerName,
                    OwnerID = x.OwnerID,
                    Description1 = x.Description1,
                    Description2 = x.Description2,
                    Description3 = x.Description3,
                    Description4 = x.Description4,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByName,
                    ModifiedOn = x.ModifiedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedByName = x.ModifiedByName,
                    OwningBusinessUnit = x.OwningBusinessUnit,
                    StateCode = x.StateCode,
                    StateLabel = x.StateLabel,
                    StatusCode = x.StatusCode,
                    DeletionStateCode = x.DeletionStateCode
                });

            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            JSONResponse result = new JSONResponse();

            try
            {
                if (HttpContext.Request.Files.AllKeys.Any() && HttpContext.Request.Form.AllKeys.Any())
                {
                    var file = HttpContext.Request.Files["import_filepath"];
                    var objectID = HttpContext.Request.Form["ObjectID"];
                    var modifiedBy = HttpContext.Request.Form["ModifiedBy"];
                    var bussinessUnitID = HttpContext.Request.Form["BusinessUnitID"];
                    var objectTypeCode = ConfigurationManager.AppSettings["typeCodeLetterTemplate"];
                    var subject = ConfigurationManager.AppSettings["subjectLetterTemplate"];
                    var letterTemplateID = new Guid(objectID);
                    if (file != null)
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);

                        if (!ext.ToLower().Equals(".docx")) throw new Exception(Resources.LetterTemplate.Alert_OnlyDocument);

                        Annotation annotation = db.annotation.Where(x => x.ObjectID == letterTemplateID).FirstOrDefault();
                        bool isCreate = annotation == null ? true : false;
                        var fileSize = file.ContentLength;

                        if (isCreate)
                        {
                            annotation = new Annotation();
                            annotation.AnnotationID = Guid.NewGuid();
                        }

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
                        annotation.NoteText = string.Empty;

                        if (isCreate)
                        {
                            result = SubmitFile(annotation, ActionType.Create);
                        }
                        else
                        {
                            result = SubmitFile(annotation, ActionType.Edit);
                        }
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
