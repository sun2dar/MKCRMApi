using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.jqGrid;
using System.Configuration;

namespace CCARE.Controllers
{
    public class QueueItemController : Controller
    {
        private CRMDb db = new CRMDb();


        public ActionResult formSubmit(QueueItem model, string actionType, Guid hiddenId, int a)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                if (actionType == "Assign")
                {
                    model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "QueueItem", new { id = model.QueueItemId, success = results.Value });


                    var jsonData = new { flag = true, Message = url };
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

        public ActionResult Index()
        {
            List<QueueUser> model = new List<QueueUser>();
            Guid currUser = new Guid(Session["BusinessUnitID"].ToString());
            model = db.queueuser.Where(x => x.BusinessUnitId == currUser).ToList();
            
            return View(model);
        }

        public JsonResult MyCreatedRequest(string sidx, string sord, int rows, int page)
        {
            Guid currUser = new Guid(Session["CurrentUserID"].ToString());
            string getSearch = Request["_lookUp"];
            string getVal = Request["_val"];
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString()) * 10;        // multiply with 10, different with usual

            var model = db.request.Where(x => x.CreatedBy == currUser).Where(x => x.StatusCode == 2 || x.StatusCode == 3);
            switch (getSearch)
            {
                case "searchCategory": model = model.Where(x => x.CategoryName.ToLower().Contains(getVal.ToLower())); break;
                case "searchProduct": model = model.Where(x => x.ProductName.ToLower().Contains(getVal.ToLower())); break;
            }
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Title) : model.OrderBy(x => x.Title);
                    break;
                case "Product":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ProductName) : model.OrderBy(x => x.ProductName);
                    break;
                case "Category":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CategoryName) : model.OrderBy(x => x.CategoryName);
                    break;
                case "CreatedDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
                case "Duedate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.DueDate) : model.OrderBy(x => x.DueDate);
                    break;
                case "Servicelevel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ServiceLevel) : model.OrderBy(x => x.ServiceLevel);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
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

            var data = from x in model
                       select new
                       {
                           Id = x.RequestID,
                           Objectid = x.RequestID,
                           Name = x.Title,
                           DeletionStateCode = x.DeletionStateCode,
                           OwnerId = x.OwnerID,
                           Product = x.ProductName,
                           Category = x.CategoryName,
                           CreatedDate = x.CreatedOn,
                           Duedate = x.DueDate,
                           Servicelevel = x.ServiceLevel,
                           Ticketnum = x.TicketNumber
                       };

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

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page)
        {
            Guid currUser = new Guid(Session["CurrentUserID"].ToString());
            string getParam = Request["_param"];            // Queue
            string getFilter = Request["_filter"];          // In Progress = 1 ; My Created Request = 2
            string getSearch = Request["_lookUp"];          // searchCategory
            string getVal = Request["_val"];
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString()) * 10;        // multiply with 10, different with usual

            var model = db.queueitem
                              .Join(db.request, queue => queue.ObjectId, request => request.RequestID, (queue, request) => new { Queue = queue, Request = request })
                              .Where(qr => qr.Queue.ObjectId == qr.Request.RequestID)
                              .Where(qr => qr.Queue.DeletionStateCode == 0)
                              .Where(qr => qr.Queue.ObjectTypeCode == 112);


            if (string.IsNullOrEmpty(getVal))
            {
                if (!string.IsNullOrEmpty(getParam))
                {
                    Guid param = new Guid(getParam);
                    model = model
                              .Where(qr => qr.Queue.QueueId == param);
                }
                else
                {
                    model = model.Where(qr => qr.Queue.QueueOwnerID == currUser).Where(qr => qr.Queue.QueueTypeCode == 2 || qr.Queue.QueueTypeCode == 3);
                        //"2".Equals(getFilter) ?
                        // db.request.Where(x => x.CreatedBy == currUser).Where(x => x.StatusCode == 2 || x.StatusCode == 3) : 
                        //model.Where(qr => qr.Request.CreatedBy == currUser).Where(qr => qr.Queue.QueueTypeCode == 2 || qr.Queue.QueueTypeCode == 3) : 
                }
            }
            else
            {
                switch (getSearch)
                {
                    case "searchCategory":
                        if (!string.IsNullOrEmpty(getParam))
                        {
                            model = model
                                      .Where(qr => qr.Queue.QueueId == new Guid(getParam))
                                      .Where(qr => qr.Queue.CategoryName != null 
                                          && qr.Queue.CategoryName.ToLower().Contains(getVal.ToLower()));
                        }
                        else
                        {
                            model = model
                                      .Where(qr => qr.Queue.QueueOwnerID == currUser)
                                      .Where(qr => qr.Queue.QueueTypeCode == 2 || qr.Queue.QueueTypeCode == 3)
                                      .Where(qr => qr.Queue.CategoryName != null 
                                          && qr.Queue.CategoryName.ToLower().Contains(getVal.ToLower()));
                        }
                        break;
                    case "searchProduct":
                        if (!string.IsNullOrEmpty(getParam))
                        {
                            model = model
                                      .Where(qr => qr.Queue.QueueId == new Guid(getParam))
                                      .Where(qr => qr.Queue.ProductName != null
                                          && qr.Queue.ProductName.ToLower().Contains(getVal.ToLower()));
                        }
                        else
                        {
                            model = model
                                      .Where(qr => qr.Queue.QueueOwnerID == currUser)
                                      .Where(qr => qr.Queue.QueueTypeCode == 2 || qr.Queue.QueueTypeCode == 3)
                                      .Where(qr => qr.Queue.ProductName != null
                                          && qr.Queue.ProductName.ToLower().Contains(getVal.ToLower()));
                        }
                        break;
                }
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Queue.Title) : model.OrderBy(x => x.Queue.Title);
                    break;
                case "Product":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Queue.ProductName) : model.OrderBy(x => x.Queue.ProductName);
                    break;
                case "Category":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Queue.CategoryName) : model.OrderBy(x => x.Queue.CategoryName);
                    break;
                case "CreatedDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Request.CreatedOn) : model.OrderBy(x => x.Request.CreatedOn);
                    break;
                case "Duedate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Request.DueDate) : model.OrderBy(x => x.Request.DueDate);
                    break;
                case "Servicelevel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Request.ServiceLevel) : model.OrderBy(x => x.Request.ServiceLevel);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.Request.CreatedOn) : model.OrderBy(x => x.Request.CreatedOn);
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

            var data = from x in model
                       select new
                       {
                           Id = x.Queue.QueueItemId,
                           Objectid = x.Queue.ObjectId,
                           Name = x.Queue.Title,
                           DeletionStateCode = x.Queue.DeletionStateCode,
                           OwnerId = x.Queue.QueueOwnerID,
                           Qtypecode = x.Queue.QueueTypeCode,
                           Qid = x.Queue.QueueId,
                           Product = x.Queue.ProductName,
                           Category = x.Queue.CategoryName,
                           CreatedDate = x.Request.CreatedOn,
                           Duedate = x.Request.DueDate,
                           Servicelevel = x.Request.ServiceLevel,
                           Ticketnum = x.Request.TicketNumber,
                           Type = x.Queue.ObjectType
                       };

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

        public ActionResult Edit(Guid id)
        {
            QueueItem model = db.queueitem.Single(x => x.QueueItemId == id);
            StringMapController dropDown = new StringMapController();
            return View(model);
        }

        public ActionResult AssignToWG(QueueItem model, Guid OldQueueOwnerID, string type)
        {
            int a;
            if (type == "user")
                a = 2;
            else
                a = 1;

            return formSubmit(model, "Assign", OldQueueOwnerID, a);
        }

        public ActionResult AssignToMe(QueueItem model, Guid OldQueueOwnerID)
        {
            int a = 2;
            model.QueueOwnerID = new Guid(Session["CurrentUserID"].ToString());
            //session login user
            return formSubmit(model, "Assign", OldQueueOwnerID, a);
        }
    }
}
