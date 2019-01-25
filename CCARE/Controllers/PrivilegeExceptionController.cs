using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.Models.Role;

namespace CCARE.Controllers
{
    public class PrivilegeExceptionController : Controller
    {
        CRMDb db = new CRMDb();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(Guid RoleId)
        {
            ViewBag.Entity = new SelectList(db.privilegeException.Select(x => new { x.EntityType, x.EntityName }).Distinct(), "EntityName", "EntityName");
            ViewBag.RoleID = RoleId;

            var model = db.privilegeException.Where(x => x.SecurityRoleID == RoleId);

            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            CRMDb db = new CRMDb();
            string getVal = Request["_val"];

            if (getVal == "" || getVal == null)
            {
                getVal = Guid.Empty.ToString();
            }

            Guid roleId = new Guid(getVal);

            //Query to privilege by role id
            var priv = db.privilegeException.Where(x => x.SecurityRoleID == roleId);

            var data = priv.ToList()
                .Select(x => new
                {
                    Id = x.PrivilegeExceptionID,
                    EntityType = x.EntityType,
                    EntityName = x.EntityName,
                    StatusType = x.StatusType,
                    StatusName = x.StatusName,
                    ExKey = x.Key,
                    ExValue = x.Value,
                    NewKey = x.NewKey,
                    NewValue = x.NewValue,
                    Inclusive = x.Inclusive,
                    InclusiveLabel = x.InclusiveLabel,
                    StatusChangeID = x.StatusChangeID
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
        public ActionResult executePE(PrivilegeException model, string actionString)
        {
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            string message = Resources.NotifResource.PrivilegeExceptionSuccess;

            model.Inclusive = true;

            if (actionString == "Create")
            {
                if (db.privilegeException.
                    Where(x => x.EntityName == model.EntityName &&
                        x.StatusName == model.StatusName &&
                        x.Key == model.Key &&
                        x.NewKey == model.NewKey &&
                        x.SecurityRoleID == model.SecurityRoleID).Count() > 0)
                {
                    var jsonData = new { flag = false, Message = Resources.NotifResource.PrivilegeExceptionSame };
                    return Json(jsonData);
                }
            }
            if (actionString == "Delete") message = Resources.NotifResource.PrivilegeExceptionDelSuccess;

            switch (actionString)
            {
                case "Delete":
                    results = db.PrivilegeException_Delete(model);
                    break;

                case "Edit":
                    model.StatusChangeID = db.privilegeException
                        .Where(x => x.EntityName == model.EntityName &&
                        x.StatusName == model.StatusName &&
                        x.Key == model.Key &&
                        x.NewKey == model.NewKey).FirstOrDefault().StatusChangeID; 
                    results = db.PrivilegeException_Update(model);
                    break;

                case "Create":
                    model.PrivilegeExceptionID = Guid.NewGuid();
                    var stChangeId = db.privilegeException
                        .Where(x => x.EntityName == model.EntityName &&
                        x.StatusName == model.StatusName &&
                        x.Key == model.Key &&
                        x.NewKey == model.NewKey);

                    model.StatusChangeID = stChangeId.FirstOrDefault().StatusChangeID;
                    results = db.PrivilegeException_Insert(model);
                    break;
            }

            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                var jsonData = new { flag = true, Message = message };
                return Json(jsonData);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData);
            }
        }


        //Get value for all dropdownlist
        public ActionResult getActionList(string entityName)
        {
            var model = db.privilegeException.Where(x => x.EntityName == entityName).Select(x => new { key = x.StatusName, value = x.StatusName }).Distinct().ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getValueList(string entityName, string actionName)
        {
            var model = db.privilegeException.Where(x => x.EntityName == entityName && x.StatusName == actionName).Select(x => new { key = x.Key, value = x.Value }).Distinct();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNewValueList(string entityName, string actionName, string key)
        {
            var model = db.privilegeException.Where(x => x.EntityName == entityName && x.StatusName == actionName && x.Key.Equals(key)).Select(x => new { key = x.NewKey, value = x.NewValue }).Distinct();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
