using CCARE.Filters;
using CCARE.Models;
using CCARE.Models.General;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CCARE.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            if (Session["IsLogin"] == null)
            {
                int clock = Convert.ToInt32(DateTime.Now.ToString("HH:mm").Split(':')[0]);
                string bg = string.Empty;
                if (clock < 6 || clock > 17)
                {
                    bg = "bg2";
                }
                else
                {
                    bg = "bg";
                }
                ViewBag.Background = bg;
                return View();
            }
            else {
                return RedirectToAction("Index", "Homepage");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        //Check session is active
        public JsonResult isActiveSession()
        {
            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            string loginUrl = u.Action("doLogout", "Login");

            if (Session["IsLogin"] != null)
            {
                var jsonDataFalse = new { flag = true };
                return Json(jsonDataFalse);
            }
            else
            {
                var jsonDataFalse = new { flag = false, loginUrl = loginUrl };
                return Json(jsonDataFalse);
            }
        }

        //Validate user through Active Directory
        public bool validateUserDomain(string varDomain, string varUserName, string varPwd)
        {
            Boolean isValidUser;

            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, varDomain))
                {
                    isValidUser = pc.ValidateCredentials(varUserName, varPwd);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return isValidUser;
        }

        public ActionResult validateUserDomainByJson(string varUserName, string varPwd)
        {
            var isValidUser = false;
            if ("yes".Equals(ConfigurationManager.AppSettings["IsByPassLogin"]))
            {
                return Json(new { _isValidUser = true });
            }
            else 
            {
                try
                {
                    string domain = "Yes".Equals(ConfigurationManager.AppSettings["IsProduction"].ToString()) ? ConfigurationManager.AppSettings["DomainName"].ToString() : ConfigurationManager.AppSettings["DomainName_dev"].ToString();
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
                    {
                        isValidUser = pc.ValidateCredentials(varUserName, varPwd);
                    }
                }
                catch (Exception)
                {
                    return Json(new { _isValidUser = false });
                }
                return Json(new { _isValidUser = isValidUser });
            }
        }


        // Validate user through Database
        public SystemUser validateUserDb(string domainName)
        {
            CRMDb db = new CRMDb();
            var model = db.systemUser.Where(x => x.DomainName == domainName)
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.IsDisabled == false)
                                .Select(x => x);
            //User not found
            if (model.ToList().Count <= 0)
            {
                return null;
            }
            else
            {
                return model.Single();
            }
        }

        [TrackExecutingTime]
        [HttpPost]
        public JsonResult doLogin(FormCollection form)
        {
            string domain = "Yes".Equals(ConfigurationManager.AppSettings["IsProduction"].ToString()) ? ConfigurationManager.AppSettings["DomainName"].ToString() : ConfigurationManager.AppSettings["DomainName_dev"].ToString();
            string userInput = form["txtUser"].ToString();
            string pass = form["txtPass"].ToString();

            string domainLoginName = domain + "\\" + userInput; ;
            string userName = userInput;

            string message = Resources.NotifResource.InvalidCredential;

            if (userInput.Contains("\\"))
            {
                //Split domain and Username
                domain = userInput.Substring(0, userName.IndexOf("\\"));
                userName = userInput.Substring(userName.IndexOf("\\") + 1, userName.Length - (userName.IndexOf("\\") + 1));
                domainLoginName = userInput;
            }

            if (domain != "")
            {
                //Authenticaiton through Active directory
                bool adValidate = "yes".Equals(ConfigurationManager.AppSettings["IsByPassLogin"]) ? true : validateUserDomain(domain, userName, pass);
                
                if (adValidate == true)
                {
                    //Check User to Crm database   
                    SystemUser logonUser = validateUserDb(domainLoginName);
                    if (logonUser != null)
                    {
                        if (logonUser.SecurityRoleID != null)
                        {
                            Session.Clear();
                            Session["IsLogin"] = true;
                            Session["DomainName"] = logonUser.DomainName;
                            Session["DomainUserName"] = userName;
                            Session["CurrentUserID"] = logonUser.SystemUserId;
                            Session["Fullname"] = logonUser.FullName;
                            Session["RoleID"] = logonUser.SecurityRoleID;
                            Session["RoleName"] = logonUser.RoleName;
                            Session["BusinessUnitID"] = logonUser.BusinessUnitID;
                            Session["BusinessUnitName"] = logonUser.BusinessUnitName;
                            Session["OrganizationID"] = logonUser.OrganizationID;
                            Session["isSuperAdmin"] = (userName == "aidcrm" || userName == "admincrm")? "Y" : "N";

                            //FormsAuthentication.SetAuthCookie(logonUser.SystemUserId.ToString(), false);

                            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                            string newUrl = null;

                            newUrl = u.Action("Index", Resources.StaticData.HomeController);

                            var jsonDataTrue = new { flag = true, newUrl = newUrl };
                            return Json(jsonDataTrue);
                        }
                        else
                        {
                            message = Resources.NotifResource.InvalidUserAccess;
                        }
                    }
                }
            }
            var jsonDataFalse = new { flag = false, message = message };
            return Json(jsonDataFalse);
        }

        public ActionResult doLogout()
        {
            Session.Abandon();
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}
