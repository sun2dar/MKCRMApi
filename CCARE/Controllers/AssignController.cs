using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
namespace CCARE.Controllers
{
    public class AssignController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult formSubmit(Guid OwnerID, Guid ObjectId, string ObjectTypeCode, string BusinessUnitID, string actionType, int a,string entityname)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                if (actionType == "Assign")
                {

                    Guid ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                    results = db.QueueItem_AssignTo(OwnerID,ModifiedBy,ObjectId, ObjectTypeCode, BusinessUnitID, a);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = "";

                    Guid? temp = new Guid();
                    if (entityname == "queueitem")
                    {
                        temp = db.queueitem.Single(x => x.ObjectId == ObjectId).QueueItemId;

                        url = u.Action("Edit", "QueueItem", new { id = temp, success = results.Value });
                    }
                    else if (entityname == "task")
                    {
                        temp = ObjectId;
                        url = u.Action("Edit", "Task", new { id = temp, success = results.Value });
                    }
                    else if (entityname == "request")
                    {
                        temp = ObjectId;
                        url = u.Action("Edit", "Request", new { id = temp, success = results.Value });
                    }
                    var jsonData = new { flag = true, Message = url };
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

        public ActionResult AssignToWG( Guid OwnerID, Guid ObjectId, string ObjectTypeCode, string BusinessUnitID, string type, string entityname)
        {
            int NewQueueTypeCode = 0;
            if (type == "user")
            {
                NewQueueTypeCode = 2;
            }
            else if (type == "currentuser")
            {
                NewQueueTypeCode = 2;
                OwnerID = new Guid(Session["CurrentUserID"].ToString());
            }
            else if (type == "queue")
            {
                NewQueueTypeCode = 1;
            }


            return formSubmit(OwnerID,ObjectId,ObjectTypeCode, BusinessUnitID, "Assign", NewQueueTypeCode, entityname);
        }

    }
}
