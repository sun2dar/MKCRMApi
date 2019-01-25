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

namespace CCARE.Controllers
{
    public class WSIDController : Controller
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

            var model = db.wsid.Take(1);
            int stateCode = "1".Equals(getFilter) ? 0 : 1;
            int totalRecords = 0;

            string getValToLower = getVal.ToLower();

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchName":
                        model = db.wsid.Where(x => x.DeletionStateCode == 0 && x.StateCode == stateCode && x.Name != null).Where("it.Name.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchLocation":
                        model = db.wsid.Where(x => x.DeletionStateCode == 0 && x.StateCode == stateCode && x.Location != null).Where("it.Location.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchLok":
                        model = db.wsid.Where(x => x.DeletionStateCode == 0 && x.StateCode == stateCode && x.Lok != null).Where("it.Lok.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchCity":
                        model = db.wsid.Where(x => x.DeletionStateCode == 0 && x.StateCode == stateCode && x.City != null).Where("it.City.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchAddress":
                        model = db.wsid.Where(x => x.DeletionStateCode == 0 && x.StateCode == stateCode && x.Address != null).Where("it.Address.ToLower().Contains(@0)", getValToLower);
                        break;
                }
                totalRecords = model.Count();
            }
            else
            {
                model = db.wsid.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0);
                totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Name":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
                    break;
                case "Address":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Address) : model.OrderBy(x => x.Address);
                    break;
                case "Camera":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Camera) : model.OrderBy(x => x.Camera);
                    break;
                case "City":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.City) : model.OrderBy(x => x.City);
                    break;
                case "Coordinator":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Coordinator) : model.OrderBy(x => x.Coordinator);
                    break;
                case "DescKU":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.DescKU) : model.OrderBy(x => x.DescKU);
                    break;
                case "DescKW":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.DescKW) : model.OrderBy(x => x.DescKW);
                    break;
                case "InstallDate":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.InstallDate) : model.OrderBy(x => x.InstallDate);
                    break;
                case "Island":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Island) : model.OrderBy(x => x.Island);
                    break;
                case "Location":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Location) : model.OrderBy(x => x.Location);
                    break;
                case "Lok":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Lok) : model.OrderBy(x => x.Lok);
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
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Name) : model.OrderBy(x => x.Name);
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
                    Id = x.WSIDID,
                    Name = x.Name,
                    Address = x.Address,
                    Camera = x.Camera,
                    City = x.City,
                    Coordinator = x.Coordinator,
                    DescKU = x.DescKU,
                    DescKW = x.DescKW,
                    InstallDate = x.InstallDate,
                    Island = x.Island,
                    Location = x.Location,
                    Lok = x.Lok,
                    CreatedByDSC = x.CreatedByDSC,
                    ModifiedByDSC = x.ModifiedByDSC,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByName,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedByName = x.ModifiedByName,
                    ModifiedOn = x.ModifiedOn,
                    StateLabel = x.StateLabel,
                    DeletionStateCode = x.DeletionStateCode,
                    StateCode = x.StateCode,
                    StatusCode = x.StatusCode
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
            WSID model = new WSID();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(WSID model)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            WSID model = db.wsid.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(WSID model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(WSID model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(WSID model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(WSID model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                if (actionType == "Create")
                {
                    model.WSIDID = Guid.NewGuid();
                    results = db.WSID_Insert(model, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    results = db.WSID_Update(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.WSID_ChangeStatus(model);
                }
                else if (actionType == "Delete")
                {
                    results = db.WSID_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "WSID", new { id = model.WSIDID, success = successMessage });
                    string urlNew = u.Action("Create", "WSID");

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
    }
}
