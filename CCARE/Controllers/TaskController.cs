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
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {

            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];
            Guid currUser = new Guid(Session["CurrentUserID"].ToString());

            var model = db.task.Where(x => x.DeletionStateCode == 0);
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());

            if (string.IsNullOrEmpty(getVal))
            {
                switch (getFilter)
                {
                    case "myTask":
                        model = model.Where(x => x.OwnerID == currUser);
                        break;
                    case "myCreatedTask":
                        model = model.Where(x => x.CreatedBy == currUser);
                        break;
                    case "FilterAllTask":
                        model = model.Where(x => x.OwnerID == currUser || x.CreatedBy == currUser);
                        break;
                }
            }
            else {
                switch (getFilter)
                {
                    case "myTask":
                        switch (getParam)
                        {
                            case "searchTaskID" :
                                model = model.Where(x => x.OwnerID == currUser && x.TaskNumber != null).Where(x => x.TaskNumber == getVal);
                                break;
                            case "searchSubject":
                                model = model.Where(x => x.OwnerID == currUser && x.Subject != null).Where("it.Subject.StartsWith(@0)", getVal);
                                break;
                            case "searchRegarding":
                                model = model.Where(x => x.OwnerID == currUser && x.Regarding != null).Where("it.Regarding.StartsWith(@0)", getVal);
                                break;
                            case "searchRequestID":
                                model = model.Where(x => x.OwnerID == currUser && x.RequestTicketNumber != null).Where("it.RequestTicketNumber.StartsWith(@0)", getVal);
                                break;
                        }
                        break;
                    case "myCreatedTask":
                        switch (getParam)
                        {
                            case "searchTaskID":
                                model = model.Where(x => x.CreatedBy == currUser && x.TaskNumber != null).Where(x => x.TaskNumber == getVal);
                                break;
                            case "searchSubject":
                                model = model.Where(x => x.CreatedBy == currUser && x.Subject != null).Where("it.Subject.StartsWith(@0)", getVal);
                                break;
                            case "searchRegarding":
                                model = model.Where(x => x.CreatedBy == currUser && x.Regarding != null).Where("it.Regarding.StartsWith(@0)", getVal);
                                break;
                            case "searchRequestID":
                                model = model.Where(x => x.CreatedBy == currUser && x.RequestTicketNumber != null).Where("it.RequestTicketNumber.StartsWith(@0)", getVal);
                                break;
                        }
                        break;
                    case "FilterAllTask":
                        switch (getParam)
                        {
                            case "searchTaskID":
                                model = model.Where(x => x.TaskNumber != null).Where(x => x.TaskNumber == getVal);
                                break;
                            case "searchSubject":
                                model = model.Where(x => x.Subject != null).Where("it.Subject.StartsWith(@0)", getVal);
                                break;
                            case "searchRegarding":
                                model = model.Where(x => x.Regarding != null).Where("it.Regarding.StartsWith(@0)", getVal);
                                break;
                            case "searchRequestID":
                                model = model.Where(x => x.RequestTicketNumber != null).Where("it.RequestTicketNumber.StartsWith(@0)", getVal);
                                break;
                        }
                        break;
                }
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "TaskID":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TaskNumber) : model.OrderBy(x => x.TaskNumber);
                    break;
                case "Subject":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Subject) : model.OrderBy(x => x.Subject);
                    break;
                case "Regarding":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Regarding) : model.OrderBy(x => x.Regarding);
                    break;
                case "RequestTicketNumber":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.RequestTicketNumber) : model.OrderBy(x => x.RequestTicketNumber);
                    break;
                case "TaskStatusLabel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.TaskStatusLabel) : model.OrderBy(x => x.TaskStatusLabel);
                    break;
                case "ActivityStatusLabel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ActivityStatusLabel) : model.OrderBy(x => x.ActivityStatusLabel);
                    break;
                case "StartDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StartDate) : model.OrderBy(x => x.StartDate);
                    break;
                case "DueDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.DueDate) : model.OrderBy(x => x.DueDate);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.DueDate) : model.OrderBy(x => x.DueDate);
                    break;
            }

            if (model.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
            {
                page--;
                if (page < 1)
                {
                    page = 1;
                }
            }

            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            var data = model
                        .Select(x => new
                        {
                            Id = x.TaskID,
                            TaskID = x.TaskNumber,
                            Subject = x.Subject,
                            Regarding = x.Regarding,
                            TicketNumber = x.RequestTicketNumber,
                            TaskStatusLabel = x.TaskStatusLabel,
                            ActivityStatusLabel = x.ActivityStatusLabel,
                            StartDate = x.StartDate,
                            DueDate = x.DueDate
                        });

            if (data.Count() == 0)
            {
                page--;
                if (page < 1)
                {
                    page = 1;
                }
                totalPages = page;
                totalRecords = page * 10 - data.Count();
            }

            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult formSubmit(Task model, string actionType )
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            SessionForSP param = new SessionForSP();

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                param.BusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());
                param.OrganizationID = new Guid(Session["OrganizationID"].ToString());

                if (actionType == "Edit")
                {
                    model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                    results = db.spTask_Update(model, param);
                }
                else if (actionType == "Create")
                {
                    model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                    model.TaskID = Guid.NewGuid();
                    results = db.spTask_Insert(model, param);
                }
                else if (actionType == "ChangeStatus")
                {
                    model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                    results = db.spTask_SetStatus(model);
                }
                else if (actionType == "Delete")
                {
                    model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                    results = db.spTask_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Task", new { id = model.TaskID, success = successMessage });
                    string urlNew = u.Action("Create", "Task");

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
        
        public ActionResult Edit(Guid id)
        {
            Task model = db.task.Find(id);
            StringMapController dropDown = new StringMapController();
            
            // Notes
            ViewBag.ObjectID = id;
            ViewBag.BusinessUnitID = model.BusinessUnitID;

            // Documents
            ViewBag.TabPosition = Request["position"];
            ViewBag.SL_Priority = dropDown.getDropDown("Task", "priority", model.Priority == null ? 0 : (int)model.Priority);

            Annotation note = db.SpGetNotes(id).Select(x => new Annotation
            {
                AnnotationID = x.AnnotationID,
                CreatedByID = x.CreatedBy,
                CreatedBy = x.CreatedByName,
                CreatedOn = x.CreatedOn,
                DeletionStateCode = x.DeletionStateCode,
                ModifiedByID = x.ModifiedBy,
                ModifiedBy = x.ModifiedByName,
                ModifiedOn = x.ModifiedOn,
                NoteText = x.NoteText,
                ObjectID = x.ObjectID,
                ObjectTypeCode = x.ObjectTypeCode,
                BusinessUnitID = x.OwningBusinessUnit,
                Subject = x.Subject
            }).FirstOrDefault();

            for (int i = 0; i < model.Notes.Count(); i++)
                if (!model.Notes.ElementAt(i).IsDocument)
                    model.Notes.Remove(model.Notes.ElementAt(i));

            model.Notes.Add(note);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Task task)
        {
            return formSubmit(task, "Edit");
        }

        public ActionResult Create()
        {
            Task model = new Task();
            string getRequestID = Request["RequestID"];
            string getRegarding = Request["Regarding"];
            string getTicketNumber = Request["TicketNumber"];
            Guid RequestID = new Guid();
            string Regarding = "";
            if (getRequestID != "" && getRequestID != null)
            {
                RequestID = new Guid(getRequestID);
                model.RequestID = RequestID;
                model.RequestTicketNumber = getTicketNumber;
                Regarding = getRegarding;
                model.Regarding = Regarding;
            }
            StringMapController dropDown = new StringMapController();

            ViewBag.SL_StatusCode = dropDown.getDropDown("Task", "taskstatus", model.TaskStatus == null ? 0 : (int)model.TaskStatus);
            ViewBag.SL_Priority = dropDown.getDropDown("Task", "priority", model.Priority == null ? 0 : (int)model.Priority);
            model.Priority = 2;
            model.TaskStatus = 1;

            Guid currUser = new Guid(Session["CurrentUserID"].ToString());
            var usname = new List<SystemUser>();
            var su = db.systemUser.Single(p => p.SystemUserId == currUser);

            model.OwnerName = su.FullName;
            model.OwnerID = currUser;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Task model)
        {
            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult ChangeStatus(Task model, int newstatus)
        {
            model.ActivityStatus = newstatus;
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(Task model)
        {
            return formSubmit(model, "Delete");
        }

    }
}
