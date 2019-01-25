using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using CCARE.Models.General;
using CCARE.Models.Role;
using CCARE.Models;
using System.Configuration;

namespace CCARE.Controllers
{
    public class ReportFilterController : Controller
    {
        CRMDb db = new CRMDb();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(Guid ReportID)
        {
            Report rp = db.report.Find(ReportID);

            List<EntityColumn> entlist = db.entityColumn
                                           .Where(x => x.EntityID == rp.EntityID)
                                           .Where(x => x.IsFilter == true)
                                           .OrderBy(x => x.Label).ToList();
            ViewBag.entity = entlist;
            var entname = db.entity.Where(x => x.ID == rp.EntityID).FirstOrDefault();
            ViewBag.entityname = entname.Name;
            return View(rp);
        }

        public IEnumerable<BaseAttribute> getDropdownItem(string entity, string name)
        {
            if (entity == "Request")
            {
                entity = "Incident";
            }

            var result = db.pickList
                           .Where(x => x.EntityName == entity && x.AttributeName == name)
                           .OrderBy(x => x.label)
                           .ToList()
                           .Select(x => new BaseAttribute
                           {
                               Text = x.label,
                               Value = x.AttributeValue.ToString()
                           });

            return result;
        }

        public JsonResult setDropdownItem(string entity, string name)
        {
            var data = new SelectList(getDropdownItem(entity, name), "Value", "Text");

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult formSubmit(ReportFilter model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.DataSuccess;

            SessionForSP sessionParam = new SessionForSP();
            sessionParam.CurrentUserID = new Guid(Session["CurrentUserID"].ToString());

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                var jsonData = new { flag = false, Message = "" };

                if (actionType == "Create")
                {
                    model.ID = Guid.NewGuid();

                    jsonData = new { flag = true, Message = "Filter berhasil ditambahkan" };
                    results = db.ReportFilter_Insert(model);
                }

                else if (actionType == "Delete")
                {
                    jsonData = new { flag = true, Message = "Filter berhasil dihapus" };
                    results = db.ReportFilter_Delete(model);
                }
                else if (actionType == "SwapSeqNo")
                {
                    try
                    {
                        Guid ID1 = new Guid(TempData["ID1"].ToString());
                        int SeqNo1 = int.Parse(TempData["SeqNo1"].ToString());
                        Guid ID2 = new Guid(TempData["ID2"].ToString());
                        int SeqNo2 = int.Parse(TempData["SeqNo2"].ToString());

                        jsonData = new { flag = true, Message = "Swap filter berhasil" };
                        results = db.ReportFilter_SwapSeqNo(ID1, SeqNo1, ID2, SeqNo2);
                    }
                    catch (Exception e)
                    {
                    }
                }

                //Update report
                ReportView reportModel = new ReportView();
                reportModel = db.reportView.Where(x => x.ID == model.ReportID).FirstOrDefault();
                reportModel.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                results = db.Report_Update(reportModel);

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string urlNew = u.Action("Edit", "MasterReport", new { id = model.ReportID, success = results.Value });

                    return Json(jsonData);
                }
                else
                {
                    jsonData = new { flag = false, Message = "Action gagal" };
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

        [HttpPost]
        public ActionResult Delete()
        {
            ReportFilter model = new ReportFilter();
            string ReportID = Request["reportid"];
            model.ReportID = new Guid(ReportID);
            model.ID = new Guid(Request["ID"]);

            string type = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                         .Where(x => x.ID == model.ID).FirstOrDefault().DataType;

            int checkExistingFilter = 0;
            string errorMessage = "";
            if (type == "datetime")
            {
                checkExistingFilter = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                     .Where(x => x.DataType == type).Count();
                errorMessage = "Delete gagal. Report harus memiliki minimal 1 filter type date.";
            }
            else
            {
                checkExistingFilter = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                     .Where(x => x.DataType != "datetime").Count();
                errorMessage = "Delete gagal. Report harus memiliki minimal 1 filter type non date.";
            }

            if (checkExistingFilter == 1)
            {
                var jsonData = new { flag = false, Message = errorMessage };
                return Json(jsonData);
            }

            return formSubmit(model, "Delete");
        }

        [HttpPost]
        public ActionResult Create()
        {
            string type = Request["hiddentype"];
            string name = Request["hiddenname"];
            string value = "";
            DateTime startdate;
            DateTime enddate;
            ReportFilter model = new ReportFilter();
            string ReportID = Request["reportid"];
            string entitycolumnid = Request["entitycolumnid"];
            string hiddenlabel = Request["hiddenlabel"];

            model.ReportID = new Guid(ReportID);
            model.EntityColumnID = new Guid(entitycolumnid);

            switch (type)
            {
                case "text":
                    value = Request[name];

                    var checkExistingFilter = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                             .Where(x => x.Name == name);
                    if (checkExistingFilter.Count() == 0)
                    {
                        model.ValueText = value;
                    }
                    else
                    {
                        var jsonData = new { flag = false, Message = "Filter yang dipilih sudah ada" };
                        return Json(jsonData);
                    }

                    break;
                case "datetime":
                    var checkExistingFilterDatetime = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                                     .Where(x => x.EntityColumnID == model.EntityColumnID);
                    if (checkExistingFilterDatetime.Count() > 0)
                    {
                        var jsonData = new { flag = false, Message = "Filter yang dipilih sudah ada" };
                        return Json(jsonData);
                    }
                    break;
                case "dropdown":
                    value = Request[name];

                    int tempValueCode = Convert.ToInt32(value);
                    var checkExistingFilterDropdown = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                             .Where(x => x.EntityColumnID == model.EntityColumnID)
                                                             .Where(x => x.ValueCode == tempValueCode);
                    if (checkExistingFilterDropdown.Count() == 0)
                    {
                        model.ValueCode = Convert.ToInt32(value);
                        model.ValueText = hiddenlabel;
                    }
                    else
                    {
                        var jsonData = new { flag = false, Message = "Filter yang dipilih sudah ada" };
                        return Json(jsonData);
                    }
                    break;
                case "popup":
                    value = Request[name];
                    
                    if (string.IsNullOrEmpty(value))
                    {
                        var countFilter = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                         .Where(x => x.EntityColumnID == model.EntityColumnID);
                        if (countFilter.Count() == 0)
                        {
                            var checkEmptyFilter = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                                  .Where(x => x.EntityColumnID == model.EntityColumnID)
                                                                  .Where(x => x.ValueID == null);
                            if (checkEmptyFilter.Count() > 0)
                            {
                                var jsonData = new { flag = false, Message = "Filter yang dipilih sudah ada" };
                                return Json(jsonData);
                            }
                        }
                        else
                        {
                            var jsonData = new { flag = false, Message = "Silahkan isi value terlebih dahulu" };
                            return Json(jsonData);
                        }
                    }
                    else
                    {
                        Guid tempValueID = new Guid(value);
                        var checkExistingFilterPopup = db.reportFilter.Where(x => x.ReportID == model.ReportID)
                                                                 .Where(x => x.EntityColumnID == model.EntityColumnID)
                                                                 .Where(x => x.ValueID == tempValueID);
                        if (checkExistingFilterPopup.Count() == 0)
                        {
                            model.ValueID = new Guid(value);
                            model.ValueText = Request[name + "Name"];
                        }
                        else
                        {
                            var jsonData = new { flag = false, Message = "Filter yang dipilih sudah ada" };
                            return Json(jsonData);
                        }
                    }
                    break;
            }
            
            var max = db.reportFilter.Where(x => x.ReportID == model.ReportID).Max(x => x.SeqNo);
            if (max == null)
            {
                max = 0;
            }
            model.SeqNo = max + 1;

            return formSubmit(model, "Create");
        }

        [HttpPost]
        public ActionResult SwapUp(Guid ID, Guid ReportID)
        {
            ReportFilter model = new ReportFilter();
            var currentFilter = db.reportFilter.Find(ID);
            var tempFilterList = db.reportFilter.Where(x => x.ReportID == ReportID).OrderBy(x => x.SeqNo);
            List<ReportFilter> filterList = new List<ReportFilter>();
            foreach (var x in tempFilterList)
            {
                if (x.ID != ID)
                    filterList.Add(x);
                else
                    break;
            }

            if (filterList.Count == 0)
            {
                return formSubmit(model, "SwapSeqNo");
            }

            var previousFilter = filterList[filterList.Count - 1];

            TempData["ID1"] = ID.ToString();
            TempData["SeqNo1"] = currentFilter.SeqNo;
            TempData["ID2"] = previousFilter.ID.ToString();
            TempData["SeqNo2"] = previousFilter.SeqNo;

            return formSubmit(model, "SwapSeqNo");
        }

        [HttpPost]
        public ActionResult SwapDown(Guid ID, Guid ReportID)
        {
            ReportFilter model = new ReportFilter();
            var currentFilter = db.reportFilter.Find(ID);
            var tempFilterList = db.reportFilter.Where(x => x.ReportID == ReportID).OrderByDescending(x => x.SeqNo);
            List<ReportFilter> filterList = new List<ReportFilter>();
            foreach (var x in tempFilterList)
            {
                if (x.ID != ID)
                    filterList.Add(x);
                else
                    break;
            }

            if (filterList.Count == 0)
            {
                return formSubmit(model, "SwapSeqNo");
            }

            var previousFilter = filterList[filterList.Count - 1];

            TempData["ID1"] = ID.ToString();
            TempData["SeqNo1"] = currentFilter.SeqNo;
            TempData["ID2"] = previousFilter.ID.ToString();
            TempData["SeqNo2"] = previousFilter.SeqNo;

            return formSubmit(model, "SwapSeqNo");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getVal = Request["_val"];
            var data = db.reportFilter.Where(x => x.ReportID == new Guid(getVal)).OrderBy(x => x.Name).ToList()

              .Select(x => new
              {
                  Id = x.ID,
                  Type = x.Label,
                  // Value = MappingSC(x.FilterValue, x.FilterType),
                  Value = x.ValueText,
                  startdate = x.ValueStartDate,
                  enddate = x.ValueEndDate
                  //Editable = x.IsEditable == 0 ? "No" : "Yes"
              });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.Skip((page - 1) * pageSize).Take(pageSize);

            var recordCount = data.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }
    }
}
