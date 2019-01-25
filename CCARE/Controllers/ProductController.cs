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
    public class ProductController : Controller
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
            var model = db.product.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0);
            
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchProductName":
                        model = model.Where("it.Name.Contains(@0)", getVal);
                        break;
                    case "searchParentName":
                        model = model.Where("(string.IsNullOrEmpty(it.ParentName) ? string.Empty : it.ParentName).Contains(@0)", getVal);
                        break;
                    case "searchDesc":
                        model = model.Where("it.Description.Contains(@0)", getVal);
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
                case "Description":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Description) : model.OrderBy(x => x.Description);
                    break;
                case "ParentName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.ParentName) : model.OrderBy(x => x.ParentName);
                    break;
                case "CreatedByName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StateCode) : model.OrderBy(x => x.StateCode);
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
                    Id = x.ProductID,
                    Name = x.Name,
                    Description = x.Description,
                    CCQId = x.CCQId,
                    ParentName = x.ParentName,
                    ParentProductId = x.ParentProductID,
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
            Product model = new Product();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Product model)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            Product model = db.product.Find(id);
            var parent = db.product
                        .Where(x => x.DeletionStateCode == 0)
                        .Where(x => x.ParentProductID == id).ToList();
            ViewBag.child = parent.Count;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Product model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(Product model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(Product model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(Product model, string actionType)
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
                    model.ProductID = Guid.NewGuid();
                    results = db.Product_Insert(model, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    results = db.Product_Update(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.Product_ChangeStatus(model);
                }
                else if (actionType == "Delete")
                {
                    results = db.Product_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Product", new { id = model.ProductID, success = successMessage });
                    string urlNew = u.Action("Create", "Product");

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
