using ADExtended;
using CCARE.App_Function;
using CCARE.Models;
using CCARE.Models.General;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.jqGrid;

namespace CCARE.Controllers
{
    public class SystemUserController : Controller
    {
        private CRMDb db = new CRMDb();
        string entityName = "systemuser";

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            bool isAll = "2".Equals(getFilter) ? true : false;
            bool isDisabled = "0".Equals(getFilter) ? false : true;

            var model = db.systemUser.Where(x => x.DeletionStateCode == 0);
            model = isAll? model : model.Where(x => x.IsDisabled == isDisabled);

            if (!string.IsNullOrEmpty(getVal)) {
                model = model.Where(x => x.FullName.Contains(getVal) || x.DomainName.Contains(getVal));
            }

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "fullName":
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.FullName) : model.OrderBy(x => x.FullName);
                    break;
                case "domainName":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.DomainName) : model.OrderBy(x => x.DomainName);
                    break;
                case "businessUnit":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.BusinessUnitName) : model.OrderBy(x => x.BusinessUnitName);
                    break;
                case "role":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.RoleName) : model.OrderBy(x => x.RoleName);
                    break;
                case "status":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.IsDisabled) : model.OrderBy(x => x.IsDisabled);
                    break;
                default:
                    model = (sord.ToLower() == "asc") ? model.OrderByDescending(x => x.FullName) : model.OrderBy(x => x.FullName);
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
                .OrderBy(x => x.FullName)
                .Select(x => new
                {
                    Id = x.SystemUserId,
                    fullName = x.FullName,
                    domainName = x.DomainName,
                    businessUnit = x.BusinessUnitName,
                    role = x.RoleName,
                    status = x.IsDisabled == true ? "Disable" : "Enable"
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
            Guid logonUser = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            SystemUser user = new SystemUser();
            user.CreatedBy = logonUser;

            StringMapController dropDown = new StringMapController();
            ViewBag.PreferredPhoneCode = dropDown.getDropDown(entityName, "preferredphonecode", user.PreferredPhoneCode == null ? 0 : (int)user.PreferredPhoneCode);
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Edit(Guid id)
        {
            SystemUser model = db.systemUser.Find(id);
            StringMapController dropDown = new StringMapController();

            //ViewBag.PreferredPhoneCode = new SelectList(db.pickList.Where(x => x.EntityName == "Systemuser" && x.AttributeName == "preferredphonecode").OrderBy(x => x.DisplayOrder), "AttributeValue", "Label" , user.PreferredPhoneCode);
            ViewBag.PreferredPhoneCodeList = dropDown.getDropDown(entityName, "preferredphonecode", model.PreferredPhoneCode == null ? 0 : (int)model.PreferredPhoneCode);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(SystemUser model)
        {
            bool checkExistingUser = db.systemUser.Where(x => x.DomainName == model.DomainName).Count() > 0;

            if (checkExistingUser == true)
            {
                var jsonData = new { flag = false, Message = Resources.NotifResource.DomainNameExistinCRM };
                return Json(jsonData);
            }
            else
            {
                return formSubmit(model, "Create");
            }
        }

        [HttpPost]
        public ActionResult Edit(SystemUser model)
        {
            return formSubmit(model, "Edit");
        }

        public ActionResult formSubmit(SystemUser model, string actionType)
        {
            Guid logonUser = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                switch (actionType)
                {
                    case "Create":
                        model.SystemUserId = Guid.NewGuid();
                        model.CreatedBy = logonUser;
                        model.CreatedOn = DateTime.Now;
                        results = db.SystemUser_Insert(model);
                        break;

                    case "Edit":
                        model.ModifiedBy = logonUser;
                        model.ModifiedOn = DateTime.Now;
                        results = db.SystemUser_Update(model);
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Edit", "SystemUser", new { id = model.SystemUserId, success = successMessage });
                    string urlNew = u.Action("Create", "SystemUser");

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



        //Set Enable or disable system user based on oprTpe "Disable" or "Enable"
        public ActionResult setEnableDisable(Guid id, string oprType)
        {
            Guid userID = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = "";
            bool isDisable = false;
            bool flag = false;

            if (oprType == "Disable")
            {
                isDisable = true;
            }

            results = db.SystemUser_SetEnableDisable(id, userID, isDisable);
            flag = true;
            message = Resources.NotifResource.SaveSuccess;

            return Json(new { flag = flag, message = message }, JsonRequestBehavior.AllowGet);
        }


        //Check User input in active directory domain
        public ActionResult checkUserAD(string userName)
        {
            string domain = userName.Substring(0, userName.IndexOf("\\"));
            string userId = userName.Substring(userName.IndexOf("\\") + 1, userName.Length - (userName.IndexOf("\\") + 1));

            string UserDomain = "Domain : " + domain + " UserId : " + userId;

            string FullDomainName = "Yes".Equals(ConfigurationManager.AppSettings["IsProduction"].ToString()) ? ConfigurationManager.AppSettings["FullDomainName"].ToString() : ConfigurationManager.AppSettings["FullDomainName_dev"].ToString();
            string ContainerDomainName = "Yes".Equals(ConfigurationManager.AppSettings["IsProduction"].ToString()) ? ConfigurationManager.AppSettings["ContainerDomainName"].ToString() : ConfigurationManager.AppSettings["ContainerDomainName_dev"].ToString();

            try
            {
                using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, FullDomainName, ContainerDomainName, ContextOptions.Negotiate))
                {
                    string message = "";

                    using (UserPrincipalEx foundUser = UserPrincipalEx.FindByIdentity(domainContext, IdentityType.SamAccountName, userId))
                    {
                        if (foundUser == null)
                        {
                            message = Resources.NotifResource.DomainNameNotFoundAD;
                            return Json(new { isExist = 0, message = message }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var adId = foundUser.Guid;
                            string objSid = foundUser.Sid.ToString();
                            if (foundUser.GivenName == null) foundUser.GivenName = "";
                            if (foundUser.Surname == null) foundUser.Surname = "";
                            return Json(new { isExist = 1, fname = foundUser.GivenName, lname = foundUser.Surname, objSid = objSid, message = message, adId = adId, email = foundUser.EmailAddress, title = foundUser.Title, phone = foundUser.homePhone }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger log = NLog.LogManager.GetCurrentClassLogger();
                string message = "Incorrect Domain Server";
                log.Error(ex.StackTrace + "====Inner==== :" + ex.InnerException.ToString() + " ====Message :" + ex.Message);
                return Json(new { isExist = 0, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
