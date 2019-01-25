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
    public class BusinessUnitController : Controller
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
            bool isDisabled = "1".Equals(getFilter) ? false : true;
            int totalRecords = 0;
            var model = db.businessunit.Take(1);

            if (string.IsNullOrEmpty(getVal))
            {
                totalRecords = db.businessunit.OrderBy(x => x.Name).Where(x => x.IsDisabled == isDisabled).Count();
                model = db.businessunit.OrderBy(x => x.Name).Where(x => x.IsDisabled == isDisabled).Take(int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"]));
            }
            else 
            {
                switch (getParam)
                {
                    case "searchBUName":
                        totalRecords = db.businessunit.OrderBy(x => x.Name).Where(x => x.IsDisabled == isDisabled).Where(x => x.Name.ToLower().Contains(getVal.ToLower())).Count();
                        model = db.businessunit.OrderBy(x => x.Name).Where(x => x.IsDisabled == isDisabled).Where(x => x.Name.ToLower().Contains(getVal.ToLower())).Take(int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"]));
                        break;
                    case "searchBUParent":
                        totalRecords = db.businessunit.OrderBy(x => x.Name).Where(x => x.IsDisabled == isDisabled).Where(x => (string.IsNullOrEmpty(x.ParentBusinessUnit) ? string.Empty : x.ParentBusinessUnit).ToLower().Contains(getVal.ToLower())).Count();
                        model = db.businessunit.OrderBy(x => x.Name).Where(x => x.IsDisabled == isDisabled).Where(x => (string.IsNullOrEmpty(x.ParentBusinessUnit) ? string.Empty : x.ParentBusinessUnit).ToLower().Contains(getVal.ToLower())).Take(int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"]));
                        break;
                }
            }
            

            var data = model.ToList().Select(x => new
            {
                Id = x.BusinessUnitId,
                name = x.Name,
                mainPhone = x.MainPhone,
                webSite = x.WebSite,
                parentBusiness = x.ParentBusinessUnit,
                isDisabled = x.IsDisabled
            });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
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

        public ActionResult Edit(Guid id)
        {
            BusinessUnit model = db.businessunit.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(BusinessUnit model)
        {
            return formSubmit(model, "Edit");
        }

        public ActionResult Create()
        {
            BusinessUnit model = new BusinessUnit();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(BusinessUnit model)
        {
            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult ChangeStatus(BusinessUnit model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(BusinessUnit model)
        {
            return formSubmit(model, "Delete");
        }


        public ActionResult formSubmit(BusinessUnit model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                KeyValuePair<int, string> results2 = new KeyValuePair<int, string>(1, "");

                if (actionType == "Create")
                {
                    model.ModifiedByID = new Guid(Session["CurrentUserID"].ToString());
                    model.BusinessUnitId = Guid.NewGuid();
                    results = db.spBusinessUnit_Insert(model, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    model.ModifiedByID = new Guid(Session["CurrentUserID"].ToString());
                    //--
                    results2 = db.spBusinessUnit_DetectLoop(model);
                    if (results2.Key == 0)
                    {
                        results = db.spBusinessUnit_Update(model, sessionParam);
                        results = db.spBusinessUnit_ChParent(model);

                        UrlHelper ua = new UrlHelper(this.ControllerContext.RequestContext);
                        string urlLoop = ua.Action("Edit", "BusinessUnit", new { id = model.BusinessUnitId,success = successMessage});
                        string urlNew = ua.Action("Create", "BusinessUnit");
                        var jsonData2 = new { flag = true, Message = urlLoop, urlNew = urlNew };
                        return Json(jsonData2);

                    }
                    else
                    {
                        var jsonData2 = new { flag = false, Message = results2.Value };
                        return Json(jsonData2);
                    }
                    //--

                }
                else if (actionType == "Delete")
                {
                    results = db.spBusinessUnit_DetectLoop(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    model.ModifiedByID = new Guid(Session["CurrentUserID"].ToString());
                    results = db.spBusinessUnit_ChangeStatus(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6 )
                {

                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "BusinessUnit", new { id = model.BusinessUnitId, success = successMessage});
                    string urlNew = u.Action("Create", "BusinessUnit");

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
