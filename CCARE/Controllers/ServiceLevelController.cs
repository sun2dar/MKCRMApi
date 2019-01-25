using CCARE.App_Function;
using CCARE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using CCARE.jqGrid;
using System.Configuration;
using CCARE.Models.MasterData;

namespace CCARE.Controllers
{
    public class ServiceLevelController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            int stateCode = "1".Equals(getFilter) ? 0 : 1;
            var model = db.serviceLevel.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0);
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchSLAName":
                        model = model.Where("(string.IsNullOrEmpty(it.Name) ? string.Empty : it.Name).Contains(@0)", getVal);
                        break;
                    case "searchCategoryName":
                        model = model.Where("(string.IsNullOrEmpty(it.CategoryIDName) ? string.Empty : it.CategoryIDName).Contains(@0)", getVal);
                        break;
                    case "searchProductName":
                        model = model.Where("(string.IsNullOrEmpty(it.ProductIDName) ? string.Empty : it.ProductIDName).Contains(@0)", getVal);
                        break;
                    case "searchSLADays":
                        model = model.Where("(string.IsNullOrEmpty(it.SLADays) ? string.Empty : it.SLADays).Contains(@0)", getVal);
                        break;
                    case "searchWorkGroupName":
                        model = model.Where("(string.IsNullOrEmpty(it.WorkgroupIDName) ? string.Empty : it.WorkgroupIDName).Contains(@0)", getVal);
                        break;
                    case "searchSegmentation":
                        model = model.Where("(string.IsNullOrEmpty(it.SegmentationName) ? string.Empty : it.SegmentationName).Contains(@0)", getVal);
                        break;
                    case "searchKota":
                        model = model.Where("(string.IsNullOrEmpty(it.KotaName) ? string.Empty : it.KotaName).Contains(@0)", getVal);
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
                case "CategoryIDName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CategoryIDName) : model.OrderBy(x => x.CategoryIDName);
                    break;
                case "ProductIDName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ProductIDName) : model.OrderBy(x => x.ProductIDName);
                    break;
                case "SLADays":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.SLADays) : model.OrderBy(x => x.SLADays);
                    break;
                case "WorkgroupIDName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.WorkgroupIDName) : model.OrderBy(x => x.WorkgroupIDName);
                    break;
                case "CreatedByName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedByName) : model.OrderBy(x => x.CreatedByName);
                    break;
                case "CreatedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
                    break;
                case "ModifiedByName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedByName) : model.OrderBy(x => x.ModifiedByName);
                    break;
                case "ModifiedOn":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ModifiedOn) : model.OrderBy(x => x.ModifiedOn);
                    break;
                case "StateLabel":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StateLabel) : model.OrderBy(x => x.StateLabel);
                    break;
                case "SegmentationName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.SegmentationName) : model.OrderBy(x => x.SegmentationName);
                    break;
                case "KotaName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.KotaName) : model.OrderBy(x => x.KotaName);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CreatedOn) : model.OrderBy(x => x.CreatedOn);
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
                    Id = x.ServiceLevelID,
                    Name = x.Name,
                    CategoryId = x.CategoryID,
                    CategoryName = x.CategoryIDName,
                    ProductId = x.ProductID,
                    ProductName = x.ProductIDName,
                    SLADays = x.SLADays,
                    WorkgroupId = x.WorkgroupID,
                    WorkgroupName = x.WorkgroupIDName,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByName,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedByName = x.ModifiedByName,
                    ModifiedOn = x.ModifiedOn,
                    StateLabel = x.StateLabel,
                    DeletionStateCode = x.DeletionStateCode,
                    StateCode = x.StateCode,
                    StatusCode = x.StatusCode,
                    ParentID = x.ParentID,
                    SegmentationID = x.SegmentationID,
                    SegmentationName = x.SegmentationName,
                    KotaID = x.KotaID,
                    KotaName = x.KotaName
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

        public ActionResult Create()
        {
            ServiceLevel model = new ServiceLevel();
            model.SLADays = 1;

            ViewBag.Kota = new SelectList(db.Kota.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");
            ViewBag.Segmentation = new SelectList(db.Segmentation.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ServiceLevel model)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            ServiceLevel model = db.serviceLevel.Find(id);
            ViewBag.Kota = new SelectList(db.Kota.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");
            ViewBag.Segmentation = new SelectList(db.Segmentation.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ServiceLevel model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(ServiceLevel model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(ServiceLevel model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(ServiceLevel model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.ServiceLevel.SLASuccess;
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());

                if (actionType == "Create")
                {
                    int count = db.serviceLevel
                        .Where(x => x.CategoryID == model.CategoryID)
                        .Where(x => x.ProductID == model.ProductID)
                        .Where(x => x.KotaID == model.KotaID)
                        .Where(x => x.SegmentationID == model.SegmentationID)
                        //.Where(x => x.WorkgroupID == model.WorkgroupID)
                        .Count();
                    if (count > 0)
                    {
                        var jsonData = new { flag = false, Message = Resources.ServiceLevel.SLASame };
                        return Json(jsonData);
                    }
                    else {
                        model.ServiceLevelID = Guid.NewGuid();
                        results = db.ServiceLevel_Insert(model, sessionParam);
                    }
                }
                else if (actionType == "Edit")
                {
                    results = db.ServiceLevel_Update(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.ServiceLevel_ChangeStatus(model);
                }
                else if (actionType == "Delete")
                {
                    results = db.ServiceLevel_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "ServiceLevel", new { id = model.ServiceLevelID, success = successMessage });
                    string urlNew = u.Action("Create", "ServiceLevel");

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

        //Load WorkflowGrid
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadGridWorkflow(string sidx, string sord, int rows, int page = 1)
        {
            if (sidx == "Id")
            {
                sidx = "SeqNo";
                sord = "asc";
            }

            CRMDb db = new CRMDb();
            string getVal = Request["_val"];

            if (getVal == "" || getVal == null)
            {
                getVal = Guid.Empty.ToString();
            }

            Guid serviceLevelID = new Guid(getVal);

            //Query to workflow by service Level ID
            var wf = db.Workflow.Where(x => x.SLID == serviceLevelID);

            var data = wf.ToList()
                .Select(x => new
                {
                    ID = x.ID,
                    ServiceLevelID = x.SLID,
                    WgID = x.WgID,
                    WgName = x.WgName,
                    WorkflowSLADays = x.WorkflowSLADays,
                    SeqNo = x.SeqNo,
                    Keterangan = x.Keterangan
                });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.AsQueryable().OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);

            var recordCount = data.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult executeWorkflow(Workflow model, string actionString)
        {
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = Resources.Workflow.WorkflowCreateSuccess;
            model.CreatedBy = model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());

            if (actionString == "Create")
            {
                if (db.Workflow.
                    Where(x => x.WgID == model.WgID).
                    Where(x=>x.SLID == model.SLID).Count() > 0)
                {
                    var jsonData = new { flag = false, Message = Resources.Workflow.WorkflowSame };
                    return Json(jsonData);
                }
            }
            //Validate if there is only 1 record then you cannot delete workflow
            else if (actionString == "Delete")
            {
                if (db.Workflow.Where(x => x.SLID == model.SLID).Count() == 1)
                {
                    var jsonData = new { flag = false, Message = Resources.Workflow.WorkflowLeft1 };
                    return Json(jsonData);
                }

            }

            switch (actionString)
            {
                case "Delete":
                    message = Resources.Workflow.WorkflowDelSuccess;
                    results = db.Workflow_Delete(model);
                    break;

                case "Edit":
                    message = Resources.Workflow.WorkflowUpdateSuccess;
                    results = db.Workflow_Update(model);
                    break;

                case "Create":
                    model.ID = Guid.NewGuid();
                    
                    results = db.Workflow_Insert(model);
                    break;
            }

            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                ServiceLevel newsl = new ServiceLevel();
                newsl = db.serviceLevel.Find(model.SLID);

                var jsonData = new { flag = true, Message = message, newWGID = newsl.WorkgroupID, newWGName = newsl.WorkgroupIDName, newsldays = newsl.SLADays };
                return Json(jsonData);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData);
            }
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Swap(Guid ID, String Direction)
        {
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = "Workgroup Swapped";

            Workflow currentWf = db.Workflow.Find(ID);

            results = db.Workflow_Swap(currentWf, Direction);

            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                ServiceLevel newsl = new ServiceLevel();
                newsl = db.serviceLevel.Find(currentWf.SLID);

                var jsonData = new { flag = true, Message = message, newWGID = newsl.WorkgroupID, newWGName = newsl.WorkgroupIDName, newsldays = newsl.SLADays };
                return Json(jsonData);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData);
            }
        }


        public JsonResult LoadSegmentation(string sidx, string sord, int rows, int page = 1)
        {
            string getServiceLevelID = Request["_serviceLevelID"];

            var data = db.serviceLevel.Where(x => x.ParentID == new Guid(getServiceLevelID) && x.DeletionStateCode == 0).ToList()
                .Select(x => new
                {
                    Id = x.ServiceLevelID,
                    Name = x.Name,
                    KotaName = x.KotaName,
                    SegmentationName = x.SegmentationName,
                    CategoryId = x.CategoryID,
                    CategoryName = x.CategoryIDName,
                    ProductId = x.ProductID,
                    ProductName = x.ProductIDName,
                    SLADays = x.SLADays,
                    WorkgroupId = x.WorkgroupID,
                    WorkgroupName = x.WorkgroupIDName
                });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.Skip((page - 1) * pageSize).Take(pageSize);

            var recordCount = data.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SegmentationList(Guid id)
        {
            ServiceLevel model = db.serviceLevel.Find(id);
            return View(model);
        }

        
        public ActionResult NewSegmentation(Guid id)
        {
            ServiceLevel model = db.serviceLevel.Find(id);
            ServiceLevel segment = new ServiceLevel();
            segment.ParentID = model.ServiceLevelID;
            segment.ProductID = model.ProductID;
            segment.ProductIDName = model.ProductIDName;
            segment.CategoryID = model.CategoryID;
            segment.CategoryIDName = model.CategoryIDName;
            segment.SLADays = 1;

            ViewBag.Kota = new SelectList(db.Kota.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");
            ViewBag.Segmentation = new SelectList(db.Segmentation.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");

            return View("Create", segment);
        }

        public ActionResult ServiceLevelChild(Guid id)
        {
            ServiceLevel model = db.serviceLevel.Find(id);

            ServiceLevel segment = new ServiceLevel();
            segment.ParentID = model.ServiceLevelID;
            segment.ParentName = model.ParentName;
            segment.ProductID = model.ProductID;
            segment.ProductIDName = model.ProductIDName;
            segment.CategoryID = model.CategoryID;
            segment.CategoryIDName = model.CategoryIDName;
            segment.SLADays = 1;

            ViewBag.Kota = new SelectList(db.Kota.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");
            ViewBag.Segmentation = new SelectList(db.Segmentation.Select(x => new { x.ID, x.Name }).Distinct(), "ID", "Name");

            return View(segment);
        }

        public PartialViewResult _Workflow(Guid? id, string oprType)
        {
            //Load this partial only when data exist
            ServiceLevel model = db.serviceLevel.Find(id);
            return PartialView(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult SearchChild()
        {
            Guid? slid = new Guid(Request.QueryString["slid"].ToString());

            ServiceLevel slchild = new ServiceLevel();
            if (Request.QueryString["kota"] == "" )
            {
                Guid segment = new Guid(Request.QueryString["segment"].ToString());
                slchild = db.serviceLevel.Where(x => x.ParentID == slid && x.SegmentationID == segment && x.DeletionStateCode == 0).FirstOrDefault();
            }
            else if (Request.QueryString["segment"] == "")
            {
                Guid kota = new Guid(Request.QueryString["kota"].ToString());
                slchild = db.serviceLevel.Where(x => x.ParentID == slid && x.KotaID == kota && x.DeletionStateCode == 0).FirstOrDefault();
            }
            else
            {
                Guid segment = new Guid(Request.QueryString["segment"].ToString());
                Guid kota = new Guid(Request.QueryString["kota"].ToString());
                slchild = db.serviceLevel.Where(x => x.ParentID == slid && x.KotaID == kota && x.SegmentationID == segment && x.DeletionStateCode == 0).FirstOrDefault();
            }
            
            //Check if record is found
            if (slchild != null)
            {
                var jsonData = new { flag = true, slChildId = slchild.ServiceLevelID};
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { flag = false, slParent = slid };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult saveNewChild(ServiceLevel model, string actionString)
        {
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = Resources.ServiceLevel.SLASegmentSuccess;
            model.CreatedBy = model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());

            switch (actionString)
            {
                case "Create":
                    model.ServiceLevelID = Guid.NewGuid();

                    bool checkExistingRecord = db.serviceLevel
                        .Where(x => x.ProductID == model.ProductID && x.CategoryID == model.CategoryID
                            && x.KotaID == model.KotaID && x.SegmentationID == model.SegmentationID).Any();

                    if (checkExistingRecord == true)
                    {
                        var jsonData = new { flag = false, Message = Resources.ServiceLevel.SLASame, slChildId = model.ServiceLevelID };
                        return Json(jsonData);
                    }

                    results = db.ServiceLevel_Insert(model, sessionParam);
                    break;
            }

            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = message, slChildId = model.ServiceLevelID };
                return Json(jsonData);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData);
            }
        }
    }
}
