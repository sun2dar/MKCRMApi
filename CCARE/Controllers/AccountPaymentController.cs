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
    public class AccountPaymentController : Controller
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

            var model = db.accountPayment.Take(1);
            int stateCode = "1".Equals(getFilter) ? 0 : 1;
            int totalRecords = 0;

            string getValToLower = getVal.ToLower();

            if (!string.IsNullOrEmpty(getVal))
            {
                switch (getParam)
                {
                    case "searchName":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.NamaPerusahaan.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchAlur":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.AlurTransaksiATM.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchCabang":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.CabangKoordinator.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchDenominasi":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.DenominasiVoucher.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchEBanking":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.EBanking.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchKerjasama":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.JenisKerjasama.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchPembayaran":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.JenisPembayaran.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchKode":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.KodePerusahaan.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchLimit":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.Limit.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchNoRek":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.NoRekPerusahaan.ToLower().Contains(@0)", getValToLower);
                        break;
                    case "searchSandi":
                        model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0).Where("it.SandiPerusahaanMBCA.ToLower().Contains(@0)", getValToLower);
                        break;
                }
                totalRecords = model.Count();
            }
            else
            {
                model = db.accountPayment.Where(x => x.StateCode == stateCode && x.DeletionStateCode == 0);
                totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "NamaPerusahaan":
                    model = model.OrderBy(x => x.NamaPerusahaan + " " + sord);
                    break;
                case "JenisPembayaran":
                    model = model.OrderBy(x => x.JenisPembayaran + " " + sord);
                    break;
                case "KodePerusahaan":
                    model = model.OrderBy(x => x.KodePerusahaan + " " + sord);
                    break;
                case "NoRekPerusahaan":
                    model = model.OrderBy(x => x.NoRekPerusahaan + " " + sord);
                    break;
                case "CabangKoordinator":
                    model = model.OrderBy(x => x.CabangKoordinator + " " + sord);
                    break;
                case "JenisKerjasama":
                    model = model.OrderBy(x => x.JenisKerjasama + " " + sord);
                    break;
                case "AlurTransaksiATM":
                    model = model.OrderBy(x => x.AlurTransaksiATM + " " + sord);
                    break;
                case "EBanking":
                    model = model.OrderBy(x => x.EBanking + " " + sord);
                    break;
                case "SandiPerusahaanMBCA":
                    model = model.OrderBy(x => x.SandiPerusahaanMBCA + " " + sord);
                    break;
                case "Limit":
                    model = model.OrderBy(x => x.Limit + " " + sord);
                    break;
                case "DenominasiVoucher":
                    model = model.OrderBy(x => x.DenominasiVoucher + " " + sord);
                    break;
                case "CreatedByName":
                    model = model.OrderBy(x => x.CreatedByName + " " + sord);
                    break;
                case "CreatedOn":
                    model = model.OrderBy(x => x.CreatedOn + " " + sord);
                    break;
                case "ModifiedByName":
                    model = model.OrderBy(x => x.ModifiedByName + " " + sord);
                    break;
                case "ModifiedOn":
                    model = model.OrderBy(x => x.ModifiedOn + " " + sord);
                    break;
                case "StateLabel":
                    model = model.OrderBy(x => x.StateLabel + " " + sord);
                    break;
                default:
                    model = model.OrderByDescending(x => x.ModifiedOn);
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
                    Id = x.AccountPaymentID,
                    NamaPerusahaan = x.NamaPerusahaan,
                    JenisPembayaran = x.JenisPembayaran,
                    KodePerusahaan = x.KodePerusahaan,
                    NoRekPerusahaan = x.NoRekPerusahaan,
                    CabangKoordinator = x.CabangKoordinator,
                    JenisKerjasama = x.JenisKerjasama,
                    AlurTransaksiATM = x.AlurTransaksiATM,
                    EBanking = x.EBanking,
                    SandiPerusahaanMBCA = x.SandiPerusahaanMBCA,
                    Limit = x.Limit,
                    DenominasiVoucher = x.DenominasiVoucher,
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
            AccountPayment model = new AccountPayment();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AccountPayment model)
        {
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            AccountPayment model = db.accountPayment.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AccountPayment model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult ChangeStatus(AccountPayment model)
        {
            return formSubmit(model, "ChangeStatus");
        }

        [HttpPost]
        public ActionResult Delete(AccountPayment model)
        {
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(AccountPayment model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            SessionForSP sessionParam = new SessionForSP();
            string successMessage = Resources.NotifResource.RequestSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());

                if (actionType == "Create")
                {
                    model.AccountPaymentID = Guid.NewGuid();
                    sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
                    results = db.AccountPayment_Insert(model, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    results = db.AccountPayment_Update(model);
                }
                else if (actionType == "ChangeStatus")
                {
                    results = db.AccountPayment_ChangeStatus(model);
                }
                else if (actionType == "Delete")
                {
                    results = db.AccountPayment_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "AccountPayment", new { id = model.AccountPaymentID, success = successMessage });
                    string urlNew = u.Action("Create", "AccountPayment");

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
