using CCARE.App_Function;
using CCARE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using CCARE.jqGrid;

namespace CCARE.Controllers
{
    public class MultiDictList<K, T> : Dictionary<K, List<T>>
    {
        public void Add(K key, T addObject)
        {
            if (!ContainsKey(key)) Add(key, new List<T>());
            if (!base[key].Contains(addObject)) base[key].Add(addObject);
        }
    }

    public class MappingController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            ViewBag.categoryModel = getFieldLabel("");
            return View();
        }

        public List<Mapping> getFieldLabel (string category) // List for shown title label and field for each category in edit and create page
        {
            List<Mapping> model = new List<Mapping>();
            Mapping selectedModel;

            // The model used as a vessel that contain field name. For example: CategoryName used as Title in edit and create Page
            if (category == "StatusMapper" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Status Mapper";
                selectedModel.ObjectName = "Entity Type";
                selectedModel.AttributeName = "Status Type";
                selectedModel.Code = "Key";
                selectedModel.Label = "Value";
                model.Add(selectedModel);
            }
            if (category == "TransactionAttributeMapping" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Transaction Attribute Mapping";
                selectedModel.ObjectName = "Channel";
                selectedModel.AttributeName = "Attribute";
                selectedModel.Code = "Key";
                selectedModel.Label = "Value";
                model.Add(selectedModel);
            }
            if (category == "LoanType" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Loan Types";
                selectedModel.ObjectName = "";
                selectedModel.AttributeName = "Category";
                selectedModel.Code = "Code";
                selectedModel.Label = "Name";
                model.Add(selectedModel);
            }
            if (category == "AccountCardType" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "ATM Card Type";
                selectedModel.ObjectName = "";
                selectedModel.AttributeName = "";
                selectedModel.Code = "Card No";
                selectedModel.Label = "Card Type";
                model.Add(selectedModel);
            }
            if (category == "AccountType" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Deposit Types";
                selectedModel.ObjectName = "";
                selectedModel.AttributeName = "";
                selectedModel.Code = "Account Code";
                selectedModel.Label = "Account Type";
                model.Add(selectedModel);
            }
            if (category == "CurrencyCode" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Currency Code";
                selectedModel.ObjectName = "";
                selectedModel.AttributeName = "";
                selectedModel.Code = "Currency Code";
                selectedModel.Label = "Currency Name";
                model.Add(selectedModel);
            }
            if (category == "ResponseCode" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Response Code";
                selectedModel.ObjectName = "";
                selectedModel.AttributeName = "Type";
                selectedModel.Code = "Code";
                selectedModel.Label = "Name";
                model.Add(selectedModel);
            }
            if (category == "TransactionType" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Jenis Transaksi";
                selectedModel.ObjectName = "";
                selectedModel.AttributeName = "Type";
                selectedModel.Code = "Code";
                selectedModel.Label = "Name";
                model.Add(selectedModel);
            }
            if (category == "StringMap" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "String Map";
                selectedModel.ObjectName = "Entity Name";
                selectedModel.AttributeName = "Attribute Name";
                selectedModel.Code = "Key";
                selectedModel.Label = "Value";
                model.Add(selectedModel);
            }
            if (category == "ChangeStatusRCM" || category == "")
            {
                selectedModel = new Mapping();
                selectedModel.CategoryName = "Change Status RCM";
                selectedModel.ObjectName = "Entity";
                selectedModel.AttributeName = "Attribute";
                selectedModel.Code = "Code";
                selectedModel.Label = "Description";
                selectedModel.NewCodeList = "New Code";
                model.Add(selectedModel);
            }
            return model;
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];
            string getCategory = Request["_category"];


            var Mapping = db.mapping.OrderBy(x => x.ModifiedOn).Where(x => x.StateCode >= 0 );

            if (getCategory.Equals("StatusChangeRCM"))
                Mapping = Mapping.Where(x => x.CategoryName.ToLower().Contains("statusmapper") && x.NewCodeList != null && x.NewCodeList != "");
            else
                Mapping = Mapping.Where(x => (getCategory).Equals(x.CategoryName));

            var data = Mapping.ToList()
                .Select(x => new
                {
                    Id = x.MappingID,
                    CategoryCode = x.CategoryCode,
                    CategoryName = x.CategoryName,
                    ObjectCode = x.ObjectCode,
                    ObjectName = x.ObjectName,
                    AttributeCode = x.AttributeCode,
                    AttributeName = x.AttributeName,
                    Code = x.Code,
                    Label = x.Label,
                    NewCodeList = x.NewCodeList,
                    DisplayOrder= x.DisplayOrder,
                    StateCode = x.StateCode,
                    StateLabel = x.StateLabel,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByName,
                    ModifiedOn = x.ModifiedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedByName = x.ModifiedByName
                });

            switch (getFilter)
            {
                case "0":
                    data = data.Where(x => x.StateCode == 0);
                    break;
                case "1":
                    data = data.Where(x => x.StateCode == 1);
                    break;
            }

            if (getVal.Length > 0)
            {
                switch (getParam)
                {
                    case "SearchObjectName":
                        data = data.Where(x => x.ObjectName != null && x.ObjectName.ToLower().Contains(getVal.ToLower()));
                        break;
                    case "SearchAttributeName":
                        data = data.Where(x => x.AttributeName != null && x.AttributeName.ToLower().Contains(getVal.ToLower()));
                        break;
                    case "SearchCode":
                        data = data.Where(x => x.Code != null && x.Code.ToLower().Contains(getVal.ToLower()));
                        break;
                    case "SearchLabel":
                        data = data.Where(x => x.Label != null && x.Label.ToLower().Contains(getVal.ToLower()));
                        break;
                }
            }
            else
                data = data.Select(x => x);

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

        public ActionResult Create()
        {
            Mapping model = new Mapping();
            ViewBag.DropDownObjectName = getDropDown("ObjectNameDropDown", model.CategoryName, model.ObjectCode);
            ViewBag.DropDownAttributeName = getDropDown("AttributeNameDropDown", model.CategoryName, model.ObjectCode);
            ViewBag.categoryModel = getFieldLabel("");
            ViewBag.categoryName = Request["_category"];
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Mapping model)
        {
            ViewBag.categoryName = Request["_category"];
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            Mapping model = db.mapping.Find(id);
            try
            {
                ViewBag.DropDownObjectName = getDropDown("ObjectNameDropDown", model.CategoryName, model.ObjectCode);
                ViewBag.DropDownAttributeName = getDropDown("AttributeNameDropDown", model.CategoryName, model.ObjectCode);
            }
            catch(Exception e)
            {
            }
            try
            {
                ViewBag.categoryModel = getFieldLabel(model.CategoryName).ElementAt(0);
            }
            catch (Exception e)
            {
                ViewBag.categoryModel = getFieldLabel("StatusMapper").ElementAt(0);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Mapping model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(Mapping model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(Mapping model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(Mapping model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());

                String category = model.CategoryName;
                if (actionType == "Create")
                {
                    if (category == "LoanType")
                        model.ObjectName = "Loan";
                    else if (category == "ATM Card Type")
                    {
                        model.ObjectName = "Account";
                        model.AttributeName = "CardType";
                    }
                    else if (category == "AccountType")
                    {
                        model.ObjectName = "Account";
                        model.AttributeName = "AccountType";
                    }
                    else if (category == "AccountCardType")
                    {
                        model.ObjectName = "Account";
                        model.AttributeName = "CardType";
                    }
                    else if (category == "CurrencyCode")
                    {
                        model.ObjectName = "Currency";
                        model.AttributeName = "CurrencyCode";
                    }
                    else if (category == "ResponseCode")
                        model.ObjectName = "Response";
                    else if (category == "TransactionType")
                        model.ObjectName = "TransactionType";

                    model.MappingID = Guid.NewGuid();
                    results = db.Mapping_Insert(model);
                }
                else if (actionType == "Edit")
                {
                    results = db.Mapping_Update(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.Mapping_ChangeStatus(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Mapping", new { id = model.MappingID, success = successMessage });
                    string urlNew = u.Action("Create", "Mapping");

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

        public SelectList getDropDown(String type, String category , int? selectedVal)
        {
            if (type == "ObjectNameDropDown")
            {
                var model = (from x in db.mapping
                             where x.CategoryName.Equals(category)
                             select x.ObjectName).Distinct();

                SelectList list2 = new SelectList(
                    model, selectedVal);

                return list2;
            }
            else // -> if type = "AttributeNameDropDown"
            {
                var model = (from x in db.mapping
                             where x.CategoryName.Equals(category)
                             select x.AttributeName).Distinct();

                SelectList list = new SelectList(
                    model, selectedVal);
                return list;
            }
        }

        public JsonResult getDropDownCreate(String type, String category)
        {
            List<BaseAttribute> dropDownList = new List<BaseAttribute>();

            if (type == "ObjectNameDropDown")
            {
                dropDownList = (from x in db.mapping
                                where x.CategoryName.Equals(category)
                                select new BaseAttribute {Value = x.ObjectName , Text = x.ObjectName}).Distinct().ToList();
            }
            else if (type == "AttributeNameDropDown")
            {
                dropDownList = (from x in db.mapping
                                where x.CategoryName.Equals(category)
                                select new BaseAttribute { Value = x.AttributeName, Text = x.AttributeName }).Distinct().ToList();
            }
            var data = new SelectList(dropDownList, "Value", "Text");
            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }
}
