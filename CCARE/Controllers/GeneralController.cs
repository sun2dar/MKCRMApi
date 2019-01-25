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
    public class GeneralController : Controller
    {
        private CRMDb db = new CRMDb();

        public JsonResult GetLabel()
        {
            ChangeStatusInput input = new ChangeStatusInput
            {
                EntityType = String.IsNullOrEmpty(Request["EntityType"]) ? 0 : Convert.ToInt32(Request["EntityType"])
            };

            var data = new SelectList(StatusLabel.GetLabel(input), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewStatus()
        {
            ChangeStatusInput input = new ChangeStatusInput
            {
                EntityType = String.IsNullOrEmpty(Request["EntityType"]) ? 0 : Convert.ToInt32(Request["EntityType"]),
                StatusType = String.IsNullOrEmpty(Request["StatusType"]) ? 0 : Convert.ToInt32(Request["StatusType"]),
                Status = Request["Status"],
                SecurityRoleId = new Guid(System.Web.HttpContext.Current.Session["RoleID"].ToString())
            };

            var data = new SelectList(StatusLabel.GetNextStatus(input), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
