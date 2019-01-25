using CCARE.App_Function;
using CCARE.Models;
using CCARE.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCARE.Controllers
{
    public class TeamController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            Guid bu = new Guid(Session["BusinessUnitID"].ToString());
            BusinessUnit businessUnit = db.businessunit.Where(x => x.BusinessUnitId == bu).SingleOrDefault();

            Team model = new Team();
            model.BusinessUnitID = new Guid(businessUnit.BusinessUnitId.ToString());
            model.BusinessUnitName = businessUnit.Name;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Team model)
        {            
            return formSubmit(model, "Create");
        }

        public ActionResult Edit(Guid id)
        {
            Team model = db.team.Where(x => x.TeamID == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Team model)
        {
            return formSubmit(model, "Edit");
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            Team model = db.team.Where(x => x.TeamID == id).FirstOrDefault();
            return formSubmit(model, "Delete");
        }

        public ActionResult formSubmit(Team model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = "Team Insert Success";
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.OrganizationID = new Guid(Session["OrganizationID"].ToString());
            model.OrganizationID = sessionParam.OrganizationID;

            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                model.ModifiedBy = new Guid(Session["CurrentUserID"].ToString());
                if (actionType == "Create")
                {
                    model.TeamID = Guid.NewGuid();
                    results = db.Team_Insert(model, sessionParam);
                }
                else if (actionType == "Edit")
                {
                    results = db.Team_Update(model, sessionParam);
                }
                else if (actionType == "Delete")
                {
                    successMessage = "Team Delete Success";
                    results = db.Team_Delete(model);
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                    {
                        UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                        string url = u.Action("Edit", "Team", new { id = model.TeamID, success = successMessage });
                        string urlNew = u.Action("Create", "Team");

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

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page)
        {
            string currentUserID = Session["CurrentUserID"].ToString();
            string getParam = Request["_param"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            Object[] data = new Object[1];
            int pageSize = 0;
            int totalRecords = 0;
            int totalPages = 0;

            switch (getFilter)
            {
                case "0":
                    var temp = db.team.Where(x => x.DeletionStateCode == 0)
                        .Select(x => new
                        {
                            Id = x.TeamID,
                            TeamName = x.Name,
                            BusinessUnit = x.BusinessUnitName
                        }).ToList().Distinct();
                        var tempT = temp.AsQueryable();
                        pageSize = rows;
                        totalRecords = tempT.Count();
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        tempT = tempT.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
                        data = tempT.ToArray();
                    break;
                case "1":
                    Guid bu = new Guid(Session["BusinessUnitID"].ToString());
                    var temp1 = db.team
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.BusinessUnitID == bu)
                                .Select(x => new
                                {
                                    Id = x.TeamID,
                                    TeamName = x.Name,
                                    BusinessUnit = x.BusinessUnitName
                                }).ToList().Distinct();

                        var temp1T = temp1.AsQueryable();
                        pageSize = rows;
                        totalRecords = temp1T.Count();
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        temp1T = temp1T.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
                        data = temp1T.ToArray();
                        break;
                case "2":
                        Guid bu2 = new Guid(Session["BusinessUnitID"].ToString());
                        Guid su = new Guid(currentUserID);
                        var temp2 = db.teamMember
                            .Join(db.team, t => t.TeamId, tm => tm.TeamID, (tm, t) => new { tm, t })
                                        .Where(x => x.tm.DeletionStateCode == 0)
                                        .Where(x => x.tm.SystemUserId == su)
                                        .Select(x => new
                                        {
                                            Id = x.tm.TeamId,
                                            TeamName = x.tm.TeamName,
                                            BusinessUnit = x.t.BusinessUnitName
                                        }).Distinct();

                        var temp2T = temp2.AsQueryable();
                        pageSize = rows;
                        totalRecords = temp2T.Count();
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        temp2T = temp2T.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
                        data = temp2T.ToArray();
                        break;
                case "3":
                        Guid bu3 = new Guid(Session["BusinessUnitID"].ToString());
                        var temp3 = db.team
                                    .Where(x => x.DeletionStateCode == 0)
                                    .Where(x => x.Parent_BusinessUnitID == bu3)
                                    .Select(x => new
                                    {
                                        Id = x.TeamID,
                                        TeamName = x.Name,
                                        BusinessUnit = x.BusinessUnitName
                                    }).Distinct();

                        var temp3T = temp3.AsQueryable();
                        pageSize = rows;
                        totalRecords = temp3T.Count();
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        temp3T = temp3T.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
                        data = temp3T.ToArray();
                        break;
            }
            var recordCount = totalRecords;
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = recordCount;
            jTable.rows = data;

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

    }
}
