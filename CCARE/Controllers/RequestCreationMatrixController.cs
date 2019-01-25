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
    public class RequestCreationMatrixController : Controller
    {
        private CRMDb db = new CRMDb();

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];
            string getEbanking = Request["_eBanking"];
            string getStatusType = Request["_StatusType"];

            var model = db.requestCreationMatrix.Take(0);
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(getEbanking))
            {
                int Ebanking = int.Parse(getEbanking);
                model = db.requestCreationMatrix.Where(x => x.RequestCreationMatrixID != Guid.Empty && x.DeletionStateCode == 0 && x.EntityType == Ebanking);
                if (!string.IsNullOrEmpty(getStatusType))
                {
                    int StatusType = int.Parse(getStatusType);
                    model = model.Where(x => x.RequestCreationMatrixID != Guid.Empty && x.DeletionStateCode == 0 && x.StatusType == StatusType);
                }
                totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "eBankingKey":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.EntityType) : model.OrderBy(x => x.EntityType);
                    break;
                case "eBanking":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.EntityName) : model.OrderBy(x => x.EntityName);
                    break;
                case "StatusType":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StatusType) : model.OrderBy(x => x.StatusType);
                    break;
                case "PrevStatus":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.PrevStatusLabel) : model.OrderBy(x => x.PrevStatusLabel);
                    break;
                case "NewStatus":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.NewStatusLabel) : model.OrderBy(x => x.NewStatusLabel);
                    break;
                case "CategoryName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CategoryName) : model.OrderBy(x => x.CategoryName);
                    break;
                case "ProductName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ProductName) : model.OrderBy(x => x.ProductName);
                    break;
                case "RequestStatus":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.RequestStatusLabel) : model.OrderBy(x => x.RequestStatusLabel);
                    break;
                case "Summary":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Summary) : model.OrderBy(x => x.Summary);
                    break;
                case "ModifiedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedOn) : model.OrderBy(x => x.ModifiedOn);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.EntityName) : model.OrderBy(x => x.EntityName);
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
                    Id = x.RequestCreationMatrixID,
                    eBankingKey = x.EntityType,
                    eBanking = x.EntityName,
                    StatusType = x.StatusType,
                    PrevStatus = x.PrevStatusLabel,
                    NewStatus = x.NewStatusLabel,
                    CategoryName = x.CategoryName,
                    ProductName = x.ProductName,
                    RequestStatus = x.RequestStatusLabel,
                    Summary = x.Summary,
                    ModifiedOn = x.ModifiedOn,
                    StateCode = x.StateCode
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

        public IEnumerable<BaseAttribute> GetTypesName()
        {
            var result = db.statusmapper.OrderBy(x => x.EntityName)
                            .GroupBy(x => x.EntityName)
                            .ToList()
                            .Select(x => new BaseAttribute
                            {
                                Text = x.First().EntityName,
                                Value = x.First().EntityType.ToString()
                            });

            return result;
        }

        public IEnumerable<BaseAttribute> GetStatusType(int parentVal)
        {
            var result = db.statusmapper.OrderBy(x => x.EntityName)
                            .Where(x => x.EntityType == parentVal)
                            .GroupBy(x => x.StatusName)
                            .ToList()
                            .Select(x => new BaseAttribute
                            {
                                Text = x.First().StatusName,
                                Value = x.First().StatusType.ToString()
                            });

            return result;
        }

        public JsonResult setStatusType(int eBanking)
        {
            var data = new SelectList(GetStatusType(eBanking), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetEntityTypes()
        {

            var result = db.requestCreationMatrix
                .Select(x => new
                {
                    Text = x.EntityName,
                    Value = x.EntityType
                })
                .Distinct()
                .OrderBy(x => x.Value)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Value.ToString()
                })
                .ToList();

            return result;
        }

        public List<SelectListItem> GetStatusTypes(int EntityType)
        {
            var result = db.requestCreationMatrix
                .Where(x => x.EntityType == EntityType)
                .Select(x => new
                {
                    EntityType = x.EntityType,
                    EntityName = x.EntityName,
                    StatusType = x.StatusType,
                    StatusName = x.StatusName
                })
                .Distinct()
                .OrderBy(x => x.StatusType)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.StatusType.ToString()
                })
                .ToList();

            return result;
        }

        public JsonResult GetJsonStatusTypes(int EntityType)
        {
            return Json(this.GetStatusTypes(EntityType), JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetPreviousStatus(int EntityType, int StatusType)
        {
            var result = db.requestCreationMatrix
                .Where(x => x.EntityType == EntityType & x.StatusType == StatusType)
                .Select(x => new
                {
                    Text = x.PrevStatusLabel,
                    Value = x.PrevStatus
                })
                .Distinct()
                .OrderBy(x => x.Value)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Value.ToString()
                })
                .ToList();

            return result;
        }

        public JsonResult GetJsonPrevStatus(int EntityType, int StatusType)
        {
            return Json(this.GetPreviousStatus(EntityType, StatusType), JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetNewStatus(int EntityType, int StatusType, string PrevStatus)
        {
            var result = db.requestCreationMatrix
                .Where(x => x.EntityType == EntityType && x.StatusType == StatusType && x.PrevStatus == PrevStatus)
                .Select(x => new
                {
                    PrevStatus = x.PrevStatus,
                    PrevStatusLabel = x.PrevStatusLabel,
                    NewStatus = x.NewStatus,
                    NewStatusLabel = x.NewStatusLabel,
                    StatusChangeID = x.StatusChangeID
                })
                .Distinct()
                .OrderBy(x => x.PrevStatus)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.NewStatusLabel,
                    Value = x.StatusChangeID.ToString()
                })
                .ToList();

            return result;
        }

        public JsonResult GetJsonNewStatus(int EntityType, int StatusType, string PrevStatus)
        {
            return Json(this.GetNewStatus(EntityType, StatusType, PrevStatus), JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetAccountCode()
        {
            var result = db.statusmapper
                .Where(x => x.EntityName == "BCA By Phone" & x.StatusName == "Customer Category")
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key
                })
                .ToList();

            return result;
        }

        public List<SelectListItem> GetRequestStatus()
        {
            var result = db.pickList
                .Where(x => x.EntityName == "Incident" & x.AttributeName == "StatusCode")
                .ToList()
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new SelectListItem
                {
                    Text = x.label,
                    Value = x.AttributeValue.ToString()
                })
                .ToList();

            return result;
        }

        public ActionResult Index()
        {
            ViewBag.DDL_eBanking = new SelectList(GetTypesName(), "Value", "Text");
            ViewBag.DDL_StatusType = new SelectList(GetStatusType(0), "Value", "Text");

            return View();
        }

        public ActionResult Create()
        {
            RequestCreationMatrix model = new RequestCreationMatrix();

            ViewBag.DDL_eBanking = this.GetEntityTypes();
            ViewBag.DDL_accountCode = this.GetAccountCode();
            ViewBag.DDL_RequestStatus = this.GetRequestStatus();

            model.RequestStatus = 5; /*Always Closed, Confirm by RCH (20151126)*/

            return View(model);
        }

        public ActionResult Edit(Guid Id)
        {
            RequestCreationMatrix model = db.requestCreationMatrix.Find(Id);

            ViewBag.DDL_eBanking = this.GetEntityTypes();
            ViewBag.DDL_StatusType = this.GetStatusTypes(model.EntityType);
            ViewBag.DDL_PrevStatus = this.GetPreviousStatus(model.EntityType, model.StatusType);
            ViewBag.DDL_NewStatus = this.GetNewStatus(model.EntityType, model.StatusType, model.PrevStatus);
            ViewBag.DDL_accountCode = this.GetAccountCode();
            ViewBag.DDL_RequestStatus = this.GetRequestStatus();
            ViewBag.Title = string.Format("Request Creation Matrix: {0}", model.Summary);

            model.RequestStatus = 5; /*Request Status always "Closed", confirmed by RCH*/
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RequestCreationMatrix model)
        {
            model.RequestStatus = 5;
            return FormSubmit(model, ActionType.Create);
        }

        [HttpPost]
        public ActionResult Edit(RequestCreationMatrix model)
        {
            return FormSubmit(model, ActionType.Edit);
        }

        [HttpPost]
        public ActionResult ChangeStatus(RequestCreationMatrix model)
        {
            return FormSubmit(model, ActionType.ChangeStatus);
        }

        [HttpPost]
        public ActionResult Delete(RequestCreationMatrix model)
        {
            return FormSubmit(model, ActionType.Delete);
        }

        public ActionResult FormSubmit(RequestCreationMatrix model, ActionType actionType)
        {
            try
            {
                if (Session["CurrentUserID"] == null)
                    throw new Exception(Resources.RequestCreationMatrix.Alert_SessionIsNull);

                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                model.RequestStatus = 5; /*Always "Closed", Confrimed By RCH*/

                if (ModelState.IsValid)
                {
                    KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                    if (actionType.Equals(ActionType.Create))
                    {
                        model.RequestCreationMatrixID = Guid.NewGuid();
                        results = db.RequestCreationMatrix_Insert(model);
                    }
                    else if (actionType.Equals(ActionType.Edit))
                    {
                        results = db.RequestCreationMatrix_Update(model);
                    }
                    else if (actionType.Equals(ActionType.ChangeStatus))
                    {
                        results = db.RequestCreationMatrix_ChangeStatus(model);
                    }
                    else if (actionType.Equals(ActionType.Delete))
                    {
                        results = db.RequestCreationMatrix_Delete(model);
                    }

                    if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                    {
                        UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                        string controllerName = this.ControllerContext.RouteData.Values["Controller"].ToString();
                        string url = u.Action("Edit", controllerName, new { id = model.RequestCreationMatrixID, success = Resources.RequestCreationMatrix.Alert_SubmissionSuccess });
                        string urlNew = u.Action("Create", controllerName);

                        var jsonData = new { flag = true, Message = url, urlNew = urlNew };
                        return Json(jsonData);
                    }
                    else
                        throw new Exception(results.Value.ToString());
                }
                else
                {
                    string errorMessage = string.Empty;
                    foreach (var key in ModelState.Keys)
                    {
                        var error = ModelState[key].Errors.FirstOrDefault();
                        if (error != null)
                        {
                            errorMessage = error.ErrorMessage;
                            break;
                        }
                    }

                    throw new Exception(errorMessage);
                }
            }
            catch (Exception e)
            {
                var jsonData = new { flag = false, Message = e.Message };
                return Json(jsonData);
            }
        }

    }
}
