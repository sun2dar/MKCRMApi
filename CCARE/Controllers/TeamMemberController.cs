using CCARE.App_Function;
using CCARE.Models;
using CCARE.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;

namespace CCARE.Controllers
{
    public class TeamMemberController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index(Guid id)
        {
            Team model = db.team.Find(id);
            ViewBag.teamId = id;
            ViewBag.teamName = model.Name;
            return View();
        }

        public ActionResult _addMember(Guid? teamId, string type = null)
        {
            string currentUserID = Session["CurrentUserID"].ToString(); 

            List<Team> listTeam = db.team
                            .Where(x => x.DeletionStateCode == 0)
                            .ToList();
            var Team = (from r in listTeam
                        select new
                        {
                            CreatedOn = r.CreatedOn.Value.ToString()
                        }).ToList();

            List<SystemUser> listUser = db.systemUser
                                .Where(x => x.DeletionStateCode == 0)
                                .Take(int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString()))
                                .ToList();

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var teamS = serializer.Serialize(listTeam);
            var userS = serializer.Serialize(listUser);
            ViewBag.Team = teamS;
            ViewBag.User = userS;
            ViewBag.teamId = teamId;

            return PartialView();
        }

        //updated by Gio 17 may 2016
        public ActionResult searchUser(string _searchText)
        {
            List<SystemUser> model = db.systemUser
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.FullName.Contains(_searchText))
                                .Take(int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString()))
                                .ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult formSubmitAddMembers(string data)
        {
            string currentUserID = Session["CurrentUserID"].ToString();
            string businessUnitID = Session["BusinessUnitID"].ToString();
            var listData = new JavaScriptSerializer().Deserialize<List<TeamMember>>(data);

            string successMessage = Resources.NotifResource.RequestSuccess;
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            var jsonData = new JSONResponse();

            if (listData.Count == 0)
            {
                jsonData.Value = false;
                jsonData.Response = "";
            }
            else
            {
                foreach (TeamMember item in listData)
                {
                    item.TeamMembershipID = Guid.NewGuid();                    
                    results = db.TeamMember_Insert(item);
                    if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                    {
                        jsonData.Value = true;
                        jsonData.Response = "success";
                    }
                    else
                    {
                        jsonData.Value = false;
                        jsonData.Response = results.Value.ToString();
                    }
                }
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult formDeleteTeamMembers(string data) 
        {           
            string currentUserID = Session["CurrentUserID"].ToString();
            string businessUnitID = Session["BusinessUnitID"].ToString();
            SessionForSP sessionParam = new SessionForSP();
            sessionParam.CurrentUserID = new Guid(currentUserID);
            var listData = new JavaScriptSerializer().Deserialize<List<TeamMember>>(data);

            string successMessage = Resources.NotifResource.RequestSuccess;
            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");

            var jsonData = new JSONResponse();

            if (listData.Count == 0)
            {
                jsonData.Value = false;
                jsonData.Response = "";
            }
            else
            {
                foreach (TeamMember item in listData)
                {
                    results = db.TeamMember_Delete(item, sessionParam);
                    if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                    {
                        jsonData.Value = true;
                        jsonData.Response = "success";
                    }
                    else
                    {
                        jsonData.Value = false;
                        jsonData.Response = results.Value.ToString();
                    }
                }
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page)
        {

            string getTeamId = Request["_teamId"];
            string getVal = Request["_val"];
            string getFilter = Request["_filter"];

            Object[] data = new Object[1];
            int pageSize = 0;
            int totalRecords = 0;
            int totalPages = 0;
            Guid teamId = new Guid(getTeamId);

            var temp = db.teamMember
                        .Where(x => x.TeamId == teamId)
                        .Select(x => new
                        {
                            Id = x.TeamMembershipID,
                            FullName = x.SystemUserName,
                            BusinessUnit = x.BusinessUnitName,
                            Title = x.Title,
                            UserStatus = x.UserStatus
                        });
            pageSize = rows;
            totalRecords = temp.Count();
            totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            temp = temp.OrderBy(sidx + " " + sord).Skip((page - 1) * pageSize).Take(pageSize);
            data = temp.ToArray();
            
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
