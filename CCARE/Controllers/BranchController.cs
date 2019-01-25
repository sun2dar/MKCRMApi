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
    public class BranchController : Controller
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
            int totalRecords = 0;
            var model = db.branch.Where(x => x.DeletionStateCode == 0).Where(x => x.StateCode == stateCode);

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchBranchCode":
                        model = model.Where(x => x.OfficeCode != null).Where("it.OfficeCode.Contains(@0)", getVal);
                        break;
                    case "searchInitial":
                        model = model.Where(x => x.Initials != null).Where("it.Initials.Contains(@0)", getVal);
                        break;
                    case "searchName":
                        model = model.Where(x => x.Name != null).Where("it.Name.Contains(@0)", getVal);
                        break;
                    case "searchRegionName":
                        model = model.Where(x => x.RegionName != null).Where("it.RegionName.Contains(@0)", getVal);
                        break;
                    case "searchAddress":
                        model = model.Where(x => x.Address != null).Where("it.Address.Contains(@0)", getVal);
                        break;
                    case "searchCity":
                        model = model.Where(x => x.City != null).Where("it.City.Contains(@0)", getVal);
                        break;
                    case "searchTelephone":
                        model = model.Where(x => (x.Telephone1 != null && x.Telephone1.ToLower().Contains(getVal.ToLower()) || (x.Telephone2.ToLower() != null && x.Telephone2.ToLower().Contains(getVal.ToLower()))));
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
                case "Address":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Address) : model.OrderBy(x => x.Address);
                    break;
                case "AddressPriority":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.AddressPriority) : model.OrderBy(x => x.AddressPriority);
                    break;
                case "BankRepHead":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.BankRepHead) : model.OrderBy(x => x.BankRepHead);
                    break;
                case "BcaBizz":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.BCABizz) : model.OrderBy(x => x.BCABizz);
                    break;
                case "BcaPrioritas":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.BCAPrioritas) : model.OrderBy(x => x.BCAPrioritas);
                    break;
                case "BranchClass":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.BranchClass) : model.OrderBy(x => x.BranchClass);
                    break;
                case "CashOffice":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.CashOffice) : model.OrderBy(x => x.CashOffice);
                    break;
                case "City":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.City) : model.OrderBy(x => x.City);
                    break;
                case "Consumer":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Consumer) : model.OrderBy(x => x.Consumer);
                    break;
                case "Fax":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Fax) : model.OrderBy(x => x.Fax);
                    break;
                case "FaxPrioritas":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.FaxPrioritas) : model.OrderBy(x => x.FaxPrioritas);
                    break;
                case "Initials":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Initials) : model.OrderBy(x => x.Initials);
                    break;
                case "LeaderName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LeaderName) : model.OrderBy(x => x.LeaderName);
                    break;
                case "LeaderNamePrioritas":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LeaderNamePrioritas) : model.OrderBy(x => x.LeaderNamePrioritas);
                    break;
                case "LongDistanceCode":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.LongDistanceCode) : model.OrderBy(x => x.LongDistanceCode);
                    break;
                case "OfficeCode":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.OfficeCode) : model.OrderBy(x => x.OfficeCode);
                    break;
                case "PostalCodePriority":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.PostalCodePriority) : model.OrderBy(x => x.PostalCodePriority);
                    break;
                case "Province":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Province) : model.OrderBy(x => x.Province);
                    break;
                case "Region":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Region) : model.OrderBy(x => x.Region);
                    break;
                case "RegionalAddressOffice":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.RegionalAddressOffice) : model.OrderBy(x => x.RegionalAddressOffice);
                    break;
                case "StatusForeignPerceptions":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StatusForeignPerceptions) : model.OrderBy(x => x.StatusForeignPerceptions);
                    break;
                case "StatusPerceptions":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.StatusPerceptions) : model.OrderBy(x => x.StatusPerceptions);
                    break;
                case "Telephone1":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Telephone1) : model.OrderBy(x => x.Telephone1);
                    break;
                case "Telephone1Prioritas":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Telephone1Prioritas) : model.OrderBy(x => x.Telephone1Prioritas);
                    break;
                case "Telephone2":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Telephone2) : model.OrderBy(x => x.Telephone2);
                    break;
                case "Telephone2Prioritas":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Telephone2Prioritas) : model.OrderBy(x => x.Telephone2Prioritas);
                    break;
                case "Zipcode":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Zipcode) : model.OrderBy(x => x.Zipcode);
                    break;
                case "VSAT":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.VSAT) : model.OrderBy(x => x.VSAT);
                    break;
                case "SDB":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.SDB) : model.OrderBy(x => x.SDB);
                    break;
                case "Notes":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Notes) : model.OrderBy(x => x.Notes);
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
                    Id = x.BranchID,
                    Name = x.Name,
                    Address = x.Address,
                    AddressPriority = x.AddressPriority,
                    BankRepHead = x.BankRepHead,
                    BcaBizz = x.BCABizz,
                    BcaPrioritas = x.BCAPrioritas,
                    BranchClass = x.BranchClass,
                    CashOffice = x.CashOffice,
                    City = x.City,
                    Consumer = x.Consumer,
                    Fax = x.Fax,
                    FaxPrioritas = x.FaxPrioritas,
                    //HeadOfMarketing = x.HeadOfMarketing,
                    Initials = x.Initials,
                    LeaderName = x.LeaderName,
                    LeaderNamePrioritas = x.LeaderNamePrioritas,
                    LongDistanceCode = x.LongDistanceCode,
                    OfficeCode = x.OfficeCode,
                    PostalCodePriority = x.PostalCodePriority,
                    Province = x.Province,
                    Region = x.Region,
                    RegionalAddressOffice = x.RegionalAddressOffice,
                    RegionName = x.RegionName,
                    StatusForeignPerceptions = x.StatusForeignPerceptions,
                    StatusPerceptions = x.StatusPerceptions,
                    Telephone1 = x.Telephone1,
                    Telephone1Prioritas = x.Telephone1Prioritas,
                    Telephone2 = x.Telephone2,
                    Telephone2Prioritas = x.Telephone2Prioritas,
                    Zipcode = x.Zipcode,
                    VSAT = x.VSAT,
                    SDB = x.SDB,
                    Notes = x.Notes,
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
            Branch model = new Branch();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Branch model)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            Branch Branch = db.branch.Find(id);
            return View(Branch);
        }

        [HttpPost]
        public ActionResult Edit(Branch model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(Branch model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(Branch model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(Branch model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;
            SessionForSP sessionParam = new SessionForSP();

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                if (actionType == "Create")
                {
                    model.BranchID = Guid.NewGuid();
                    sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
                    results = db.Branch_Insert(model, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
                    results = db.Branch_Update(model, sessionParam);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.Branch_ChangeStatus(model);
                }
                else if (actionType == "Delete")
                {
                    results = db.Branch_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "Branch", new { id = model.BranchID, success = successMessage });
                    string urlNew = u.Action("Create", "Branch");

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
