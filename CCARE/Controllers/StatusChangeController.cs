using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using System.Configuration;

namespace CCARE.Controllers
{
    public class StatusChangeController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult formSubmit(string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

            string getStatusChangeId = Request["StatusChangeID"];
            string getEntity = Request["eBanking"];
            string getStatusType = Request["StatusType"];
            string getCurrentStatus = Request["CurrentStatus"];
            string getNewStatus = Request["NewStatus"];

            StatusChange model = new StatusChange();
            
            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                if (actionType == "Insert")
                {
                    model.StatusChangeID = Guid.NewGuid();
                    model.MappingID = new Guid(getCurrentStatus);
                    model.NewKey = getNewStatus;
                    results = db.StatusChange_Insert(model);
                }
                else if (actionType == "Delete")
                {
                    model.StatusChangeID = new Guid(getStatusChangeId);
                    results = db.StatusChange_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);

                    var jsonData = new { flag = true };
                    return Json(jsonData);
                }
                else
                {
                    var jsonData = new { flag = false };
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

        //
        // GET: /ChangeStatus/

        public IEnumerable<BaseAttribute> GetTypesName()
        {
            var result = db.statusmapper.OrderBy(x => x.EntityName)
                            .GroupBy(x => x.EntityName)
                            .ToList()
                            .Select(x => new BaseAttribute
                            {
                                Text = x.First().EntityName,
                                Value = x.First().EntityType.ToString()
                            });

            return result;
        }

        public IEnumerable<BaseAttribute> GetStatusType(int parentVal)
        {
            var result = db.statusmapper.OrderBy(x => x.EntityName)
                            .Where(x => x.EntityType == parentVal)
                            .GroupBy(x => x.StatusName)
                            .ToList()
                            .Select(x => new BaseAttribute
                            {
                                Text = x.First().StatusName,
                                Value = x.First().StatusType.ToString()
                            });

            return result;
        }

        public IEnumerable<BaseAttribute> GetCurrentStatus(int eBanking, int statusType)
        {
            var result = db.statusmapper.OrderBy(x => x.Value)
                            .Where(x => x.EntityType == eBanking && x.StatusType == statusType)
                            //.GroupBy(x => x.MappingID)
                            .ToList()
                            .Select(x => new BaseAttribute
                            {
                                Text = x.Value,
                                Value = x.MappingID.ToString()
                            });
            
            return result;
        }

        public IEnumerable<BaseAttribute> GetNewStatus(int eBanking, int statusType, string currentStatus)
        {
            var temp = db.statuschange.Where(x => x.MappingID == new Guid(currentStatus)).ToList();
            string currentKey = db.statusmapper.Where(x => x.MappingID == new Guid(currentStatus)).FirstOrDefault().Key;

            var result = db.statusmapper.OrderBy(x => x.Value).OrderBy(x => x.Key)
                            .Where(x => x.EntityType == eBanking && x.StatusType == statusType && x.Key != currentKey)
                            .GroupBy(x => x.MappingID)
                            .ToList()
                            .Select(x => new BaseAttribute
                            {
                                Text = x.First().Value,
                                Value = x.First().Key
                            });

            if (temp[0] != null)
                result = result.Where(x => !temp.Select(a => a.NewKey).Contains(x.Value));

            return result;
        }

        public JsonResult setStatusType(int eBanking)
        {
            var data = new SelectList(GetStatusType(eBanking), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult setCurrentStatus(int eBanking, int statusType)
        {
            var data = new SelectList(GetCurrentStatus(eBanking, statusType), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult setNewStatus(int eBanking, int statusType, string currentStatus)
        {
            var data = new SelectList(GetNewStatus(eBanking, statusType, currentStatus), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            ViewBag.DDL_eBanking = new SelectList(GetTypesName(), "Value", "Text");
            ViewBag.DDL_StatusType = new SelectList(GetStatusType(0), "Value", "Text");
            ViewBag.DDL_CurrentStatus = new SelectList(GetCurrentStatus(0, 0), "Value", "Text");
            
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            int totalRecords = 0;
            var model = db.statuschange.Take(0);
            if (!string.IsNullOrEmpty(Request["_mappingID"]))
            {
                Guid getMappingID = new Guid(Request["_mappingID"]);
                model = db.statuschange.Where(x => x.MappingID == getMappingID && x.StatusChangeID != null && x.StateCode == 0);
            }
            totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            switch (sidx)
            {
                case "Key":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.NewKey) : model.OrderBy(x => x.NewKey);
                    break;
                case "Value":
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.NewValue) : model.OrderBy(x => x.NewValue);
                    break;
                default:
                    model = (sord.ToLower() == "desc") ? model.OrderByDescending(x => x.Key) : model.OrderBy(x => x.Key);
                    break;
            }
            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            var data = model
                .Select(x => new
                {
                    Id = x.StatusChangeID,
                    Key = x.NewKey,
                    Value = x.NewValue
                });

            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }
    }
}

