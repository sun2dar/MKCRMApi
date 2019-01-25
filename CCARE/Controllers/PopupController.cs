using CCARE.App_Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.jqGrid;
using CCARE.Models;
using CCARE.Models.General;
using CCARE.Models.MasterData;
using System.Web.Script.Serialization;
using System.Configuration;

namespace CCARE.Controllers
{

    public class PopupAttrModel
    {
        public string id { get; set; }
        public string val { get; set; }
        public string depend { get; set; }
        public string dependId { get; set; }
        public string dependVal { get; set; }
    }

    public class PopupSearchModel
    {
        public string id { get; set; }
        public string value { get; set; }
        public int level { get; set; }
        public int isBold { get; set; }
    }

    public class TreeRecusiveObject
    {
        public Guid? id { get; set; }
        public string text { get; set; }
        public int isBold { get; set; }
        public Dictionary<string, bool> state { get; set; }
        public List<TreeRecusiveObject> nodes { get; set; }
        public TreeRecusiveObject()
        {
            nodes = new List<TreeRecusiveObject>();
        }
    }

    public class PopupController : Controller
    {
        private CRMDb db = new CRMDb();

        [HttpPost]
        public JsonResult _broadcastPolling(string data)
        {
            string currentUserID = Session["CurrentUserID"].ToString();
            var BroadcastMessageFindByUser = db.broadcastMessageDetail
                .Where(x => x.IsRead == false)
                .Where(x => x.ToUserID == new Guid(currentUserID))
                .OrderByDescending(x => x.CreatedOn)
                .Take(1)
                .FirstOrDefault();

            var lastDate = BroadcastMessageFindByUser;
            string repsol = string.Empty;
            try
            {
                repsol = (lastDate != null) ? lastDate.CreatedOn.Value.ToString("MM/dd/yyyy HH:mm:ss") : repsol;
            }
            catch (Exception e)
            {
            }

            return Json(repsol);
        }

        public ActionResult _broadcastViewContent(string type = null)
        {
            string currentUserID = Session["CurrentUserID"].ToString();
            var BroadcastMessageFindByUser = db.broadcastMessageDetail
                                            .Where(x => x.IsRead == false)
                                            .Where(x => x.ToUserID == new Guid(currentUserID))
                                            .OrderByDescending(x => x.CreatedOn)
                                            .ToList();

            List<BroadcastMessageDetail> BroadcastMessageDetail = BroadcastMessageFindByUser;

            return PartialView("_broadcastViewContent", BroadcastMessageDetail);
        }

        public ActionResult _broadcastViewContentWindow(string type = null)
        {
            string currentUserID = Session["CurrentUserID"].ToString();
            var BroadcastMessageFindByUser = db.broadcastMessageDetail
                                            .Where(x => x.IsRead == false)
                                            .Where(x => x.ToUserID == new Guid(currentUserID))
                                            .OrderByDescending(x => x.CreatedOn)
                                            .ToList();

            List<BroadcastMessageDetail> BroadcastMessageDetail = BroadcastMessageFindByUser;

            return PartialView("_broadcastViewContentWindow", BroadcastMessageDetail);
        }



        public ActionResult _broadcastCreateContent(string type = null)
        {
            string CurrentUserID = Session["CurrentUserID"].ToString();
            Guid BU = new Guid(Session["BusinessUnitID"].ToString());
            List<SystemUser> ListUser = new List<SystemUser>();
            bool IsSPV = false;
            
            var BroadcastMessageFindByUser = db.broadcastMessageDetail.Where(x => x.IsRead == false).Where(x => x.ToUserID == new Guid(CurrentUserID)).ToList();

            List<BroadcastMessageDetail> BroadcastMessageDetail = (from r in BroadcastMessageFindByUser
                                                                   where r.ExpireDate == null || r.ExpireDate.Value.Date > DateTime.Now.Date
                                                                   where r.StartDate == null || r.StartDate.Value.Date >= DateTime.Now.Date
                                                                   select r).ToList();

            var AllTeam = db.team.Where(x => x.DeletionStateCode == 0);
            var DistinctTeam = AllTeam.ToList().Distinct();

            var CheckSPVByTitle = db.systemUser
                                    .Where(x => x.DeletionStateCode == 0)
                                    .Where(x => x.IsDisabled == false)
                                    .Where(x => x.SystemUserId == new Guid(CurrentUserID))
                                    .Where(x => x.Title.ToUpper() == "SPV")
                                    .SingleOrDefault();

            if (CheckSPVByTitle != null)
            {
                IsSPV = true;
            }
            
            switch (IsSPV)
            {
                case true:
                    AllTeam = AllTeam.Where(x => x.BusinessUnitID == BU);
                    var TeamMembers = db.SpGetListBroadcastUserForSPV(new Guid(CurrentUserID));
                    foreach (var entry in TeamMembers)
                    {
                        ListUser.Add(new SystemUser() { SystemUserId = entry.SystemUserId, FullName = entry.SystemUserName });
                    }
                    break;

                case false:
                    AllTeam = AllTeam.Where(x => x.BusinessUnitID == BU);
                    break;
            }

            var ListTeamDistinct = AllTeam.ToList().Distinct();
            var ListUserDistinct = ListUser.Distinct();

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var teamS = serializer.Serialize(ListTeamDistinct);
            var userS = serializer.Serialize(ListUserDistinct);
            var teamAllS = serializer.Serialize(DistinctTeam);
            ViewBag.Team = teamS;
            ViewBag.User = userS;
            ViewBag.TeamAll = teamAllS;

            return PartialView(BroadcastMessageDetail);
        }


        public ActionResult formSubmitBroadcastMessage(string data)
        {
            string currentUserID = Session["CurrentUserID"].ToString();
            var listData = new JavaScriptSerializer().Deserialize<List<BroadcastMessageDetail>>(data);
            
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
                foreach (BroadcastMessageDetail item in listData)
                {
                    item.BroadcastMessageID = Guid.NewGuid();
                    item.CreatedBy = new Guid(currentUserID);
                    results = db.BroadcastMessage_Read(item);
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

        public ActionResult formSubmitBroadcastMessageInsert(string data)
        {
            string currentUserID = Session["CurrentUserID"].ToString();
            string businessUnitID = Session["BusinessUnitID"].ToString();
            var listData = new JavaScriptSerializer().Deserialize<List<BroadcastMessage>>(data);

            var listUser = listData.Where(x => x.UserID != null).Select(x => x.UserID);

            var listTeam = listData.Where(x => x.TeamID != null).Select(x => x.TeamID).ToList();

            List<string> listUserS = listUser.Select(x => x.ToString()).Distinct().ToList();
            List<string> listTeamS = listTeam.Select(x => x.ToString()).Distinct().ToList();

            var userTeamList = (
                        from a in db.teamMember.Where(x => x.DeletionStateCode == 0)
                            .AsEnumerable()
                        where listTeamS.Any(c => c.Contains(a.TeamId.ToString()))
                        select new { systemUserId = a.SystemUserId.ToString() }).Distinct().ToList();

            foreach (var item in userTeamList)
            {
                listUserS.Add(item.systemUserId);
            }


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
                foreach (var item in listUserS.Distinct())
                {
                    BroadcastMessage bm = listData.FirstOrDefault();
                    bm.TeamID = null;
                    bm.UserID = new Guid(item);
                    bm.BroadcastMessageID = Guid.NewGuid();
                    bm.ModifiedBy = new Guid(currentUserID);
                    bm.BusinessUnit = new Guid(businessUnitID);
                    results = db.BroadcastMessage_Insert(bm);
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

        [HttpPost]
        public JsonResult SystemUser(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.systemUser
                            .Where(x => x.DeletionStateCode == 0)
                            .Where(x => x.IsDisabled == false)
                            .Where(x => x.FullName.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.SystemUserId,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Customer(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.customer
                            .Where(x => x.FullName.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.CustomerID,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult NonCustomer(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.noncustomer
                            .Select(x => new
                            {
                                id = x.NonCustomerID,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.noncustomer
                            .Where(x => x.FullName.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.NonCustomerID,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Template(string searchText, PopupAttrModel attr)
        {
            var data = db.letterTemplate
                .Where(x => x.DeletionStateCode == 0)
                .Select(x => new
                {
                    id = x.LetterTemplateId,
                    value = x.Name
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                data = data.Where(x => x.value.Contains(searchText));
            }

            data = data.Take(10);

            return Json(data.ToList());
        }

        [HttpPost]
        public JsonResult Branch(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.branch
                             .Where(x => x.DeletionStateCode == 0)
                             .Where(x => x.Name.Contains(searchText) || x.OfficeCode.Contains(searchText))
                             .Select(x => new
                             {
                                 id = x.BranchID,
                                 value = x.Name
                             }).Take(10);

                data = temp.ToList();
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult WSID(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.wsid
                            .Where(x => x.Name.Contains(searchText) || x.Location.Contains(searchText) || x.Lok.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.WSIDID,
                                value = x.Name
                            }).Take(10);

                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Cause(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.cause
                            .Where(x => x.Name.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.CauseID,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Category(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.category
                            .Where(x => x.Name.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.CategoryID,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult CategoryTree(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (!string.IsNullOrEmpty(searchText))
            {
                if (attr.dependVal == null)
                {
                    data = db.categorytree.Where(x => x.IsBold == 1 && x.CategoryName.Contains(searchText.ToUpper()))
                                          .GroupBy(x => x.CategoryID)
                                          .Select(x => new
                                          {
                                              id = x.FirstOrDefault().CategoryID,
                                              value = x.FirstOrDefault().Name,
                                              level = x.FirstOrDefault().Level,
                                              isBold = x.FirstOrDefault().IsBold,
                                              treeId = x.FirstOrDefault().TreeID
                                          }).Take(10).AsEnumerable().OrderBy(x => x.treeId).Select(x => new
                                          {
                                              id = x.id.ToString(),
                                              value = x.value,
                                              level = x.level,
                                              isBold = x.isBold
                                          });
                }
                else
                {
                    Guid dependValue = new Guid(attr.dependVal.ToString());

                    data = db.categorytree.Where(x => x.IsBold == 1 && x.ProductID == dependValue && x.CategoryName.Contains(searchText.ToUpper()))
                                          .OrderBy(x => x.TreeID)
                                          .GroupBy(x => x.CategoryID)
                                          .Select(x => new
                                          {
                                              id = x.FirstOrDefault().CategoryID,
                                              value = x.FirstOrDefault().Name,
                                              level = x.FirstOrDefault().Level,
                                              isBold = x.FirstOrDefault().IsBold,
                                              treeId = x.FirstOrDefault().TreeID
                                          }).Take(10).AsQueryable().OrderBy(x => x.treeId).Select(x => new
                                          {
                                              id = x.id,
                                              value = x.value,
                                              level = x.level,
                                              isBold = x.isBold
                                          });
                }
            }
            else
            {
                //Tricky way to return no selected data
                List<PopupSearchModel> temp = new List<PopupSearchModel>();
                PopupSearchModel emptyModel = new PopupSearchModel()
                {
                    id = "",
                    isBold = 1,
                    level = 1,
                    value = ""
                };
                temp.Add(emptyModel);
                temp.Add(emptyModel);

                data = temp;
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult Product(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.product
                            .Where(x => x.Name.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.ProductID,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult ProductTree(string searchText, PopupAttrModel attr)
        {

            var data = new object();

            if (!string.IsNullOrEmpty(searchText))
            {
                if (attr.dependVal == null)
                {
                    data = db.producttree.Where(x => x.IsBold == 1 && x.ProductName.Contains(searchText.ToUpper()))
                                          .GroupBy(x => x.ProductID)
                                          .Select(x => new
                                          {
                                              id = x.FirstOrDefault().ProductID,
                                              value = x.FirstOrDefault().Name,
                                              level = x.FirstOrDefault().Level,
                                              isBold = x.FirstOrDefault().IsBold,
                                              treeId = x.FirstOrDefault().TreeID
                                          }).Take(10).AsEnumerable().OrderBy(x => x.treeId).Select(x => new
                                          {
                                              id = x.id.ToString(),
                                              value = x.value,
                                              level = x.level,
                                              isBold = x.isBold
                                          });
                }
                else
                {
                    string dependValue = attr.dependVal.ToString();

                    data = db.producttree.Where(x => x.IsBold == 1 && x.CategoryID == new Guid(dependValue) && x.ProductName.Contains(searchText.ToUpper()))
                                          .OrderBy(x => x.TreeID)
                                          .GroupBy(x => x.ProductID)
                                          .Select(x => new
                                          {
                                              id = x.FirstOrDefault().ProductID,
                                              value = x.FirstOrDefault().Name,
                                              level = x.FirstOrDefault().Level,
                                              isBold = x.FirstOrDefault().IsBold,
                                              treeId = x.FirstOrDefault().TreeID
                                          }).Take(10).AsQueryable().OrderBy(x => x.treeId).Select(x => new
                                          {
                                              id = x.id,
                                              value = x.value,
                                              level = x.level,
                                              isBold = x.isBold
                                          });
                }
            }
            else
            {
                //Tricky way to return no selected data
                List<PopupSearchModel> temp = new List<PopupSearchModel>();
                PopupSearchModel emptyModel = new PopupSearchModel()
                {
                    id = "",
                    isBold = 1,
                    level = 1,
                    value = ""
                };
                temp.Add(emptyModel);
                temp.Add(emptyModel);

                data = temp;
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult RequestLookUp(string searchText, PopupAttrModel attr)
        {
            var data = new object();
            string getCurrentCustomerID = attr.dependId;


            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                if (!string.IsNullOrEmpty(getCurrentCustomerID))
                {
                    var temp = db.request
                                .Where(x => x.CustomerID == new Guid(getCurrentCustomerID))
                                .Where(x => x.StatusCode == 5)
                                .Where(x => x.TicketNumber.Contains(searchText))
                                .Select(x => new
                                {
                                    id = x.RequestID,
                                    value = x.TicketNumber
                                }).Take(10);
                    data = temp.ToList();
                }
                else
                {
                    var temp = db.request
                                .Where(x => x.StatusCode == 5)
                                .Where(x => x.TicketNumber.Contains(searchText))
                                .Select(x => new
                                {
                                    id = x.RequestID,
                                    value = x.TicketNumber
                                }).Take(10);
                    data = temp.ToList();
                }
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult ParentBusinessUnit(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.businessunit
                            .Select(x => new
                            {
                                id = x.BusinessUnitId,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.businessunit
                            .Where(x => x.Name.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.BusinessUnitId,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult CallWrapUpCategory(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.callWrapUpCategory
                            .Where(x => x.StateCode == 0 && x.StatusCode == 1)
                            .Select(x => new
                            {
                                id = x.CallWrapUpCategoryID,
                                value = x.Description
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.callWrapUpCategory
                            .Where(x => x.Description.Contains(searchText))
                            .Where(x => x.StateCode == 0 && x.StatusCode == 1)
                            .Select(x => new
                            {
                                id = x.CallWrapUpCategoryID,
                                value = x.Description
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }


        [HttpPost]
        public JsonResult WorkGroup(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.queue.Where(x => x.QueueTypeCode == 1)
                            .Select(x => new
                            {
                                id = x.QueueId,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.queue.Where(x => x.QueueTypeCode == 1)
                            .Where(x => x.Name.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.QueueId,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult BusinessUnit(string searchText, PopupAttrModel attr)
        {
            List<Guid?> buList = new List<Guid?>();
            buList.Add(new Guid(Session["BusinessUnitID"].ToString()));
            //getBU(buList, new Guid(Session["BusinessUnitID"].ToString()));
            getBUChild(buList, new Guid(Session["BusinessUnitID"].ToString()));

            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.businessunit
                    .Where(x => buList.Contains(x.BusinessUnitId))
                    .Where(x => x.Name.Contains(searchText))
                    .Select(x => new
                    {
                        id = x.BusinessUnitId,
                        value = x.Name
                    }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }


        [HttpPost]
        public JsonResult BusinessUnitQueue(string searchText, PopupAttrModel attr)
        {
            List<Guid?> buList = new List<Guid?>();
            buList.Add(new Guid(Session["BusinessUnitID"].ToString()));
            getBU(buList, new Guid(Session["BusinessUnitID"].ToString()));
            
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.businessunit
                    .Where(x => buList.Contains(x.BusinessUnitId))
                    .Where(x => x.Name.Contains(searchText))
                    .Select(x => new
                    {
                        id = x.BusinessUnitId,
                        value = x.Name
                    }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult BusinessUnit_ORG(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.organizationStructure
                            .Select(x => new
                            {
                                id = x.BusinessUnitID,
                                value = x.BusinessUnit
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.organizationStructure
                            .Where(x => x.BusinessUnit.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.BusinessUnitID,
                                value = x.BusinessUnit
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult SecurityRole(string searchText, PopupAttrModel attr)
        {
            List<Guid?> buList = new List<Guid?>();
            if (attr.dependVal != null && attr.dependVal != "")
            {
                buList.Add(new Guid(attr.dependVal));
                getBUChild(buList, new Guid(attr.dependVal));
            }

            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp; 
            }
            else
            {
                var temp = db.securityRole
                            .Where(x => x.XMLRole != null && x.XMLRole != "")
                            .Where(x => x.Name.Contains(searchText))
                            .Where(x => buList.Contains(x.BusinessUnitId))
                            .Select(x => new
                            {
                                id = x.SecurityRoleId,
                                value = x.Name
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Owner(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.systemUser
                            .Where(x => x.DeletionStateCode == 0)
                            .Where(x => x.IsDisabled == false)
                            .Select(x => new
                            {
                                id = x.SystemUserId,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.systemUser
                            .Where(x => x.DeletionStateCode == 0)
                            .Where(x => x.IsDisabled == false)
                            .Where(x => x.FullName.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.SystemUserId,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Regarding(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.request.Where(x => x.DeletionStateCode == 0)
                            .Select(x => new
                            {
                                id = x.RequestID,
                                value = x.Title
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.request.Where(x => x.DeletionStateCode == 0)
                            .Where(x => x.Title.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.RequestID,
                                value = x.Title
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }


        [HttpPost]
        public JsonResult User(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = db.systemUser
                            .Where(x => x.DeletionStateCode == 0)
                            .Where(x => x.IsDisabled == false)
                            .Select(x => new
                            {
                                id = x.SystemUserId,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            else
            {
                var temp = db.systemUser
                            .Where(x => x.DeletionStateCode == 0)
                            .Where(x => x.IsDisabled == false)
                            .Where(x => x.Title.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.SystemUserId,
                                value = x.FullName
                            }).Take(10);
                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult MasterReport(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.masterReport
                            .Where(x => x.ReportName.Contains(searchText) || x.Description.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.ReportID,
                                value = x.ReportName
                            }).Take(10);

                data = temp.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Report(string searchText, PopupAttrModel attr)
        {
            var data = new object();

            if (searchText == "")
            {
                var temp = new { id = Guid.Empty, value = "" };
                data = temp;
            }
            else
            {
                var temp = db.report
                            .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                            .Select(x => new
                            {
                                id = x.ID,
                                value = x.Name
                            }).Take(10);

                data = temp.ToList();
            }
            return Json(data);
        }

        public JsonResult Index()
        {

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult _popupContent(string type = null, string dependBu = null)
        {
            ViewBag.type = type;
            ViewBag.dependVal = dependBu;

            ViewBag.colModels = Request.QueryString["colModels"].ToString();
            ViewBag.searchText = Request.QueryString["searchText"].ToString();
            ViewBag.hiddenInputId = Request.QueryString["hiddenInputId"].ToString();
            ViewBag.hiddenParentId = Request.QueryString["hiddenParentId"].ToString();

            return PartialView();
        }

        public static TreeRecusiveObject walk(TreeRecusiveObject node)
        {
            if (node.nodes.Count < 1)
            {
                return node;
            }

            foreach (var e in node.nodes)
            {
                node = walk(e);
            }
            return node;
        }

        public JsonResult TreeView(string type, string searchText, string hiddenInputId, string dependVal)
        {
            List<PopupSearchModel> data = new List<PopupSearchModel>();
            int takeData = int.Parse(ConfigurationManager.AppSettings["TreeviewStaticCount"].ToString());
            int countData = 0;
            switch (type)
            {
                case "category":
                    if (searchText == "")
                    {
                        if (dependVal == "")
                        {
                            var temp = db.categorytree.GroupBy(x => x.CategoryID)
                                                  .Select(x => new
                                                  {
                                                      id = x.FirstOrDefault().CategoryID,
                                                      value = x.FirstOrDefault().Name,
                                                      level = x.FirstOrDefault().Level,
                                                      isBold = x.FirstOrDefault().IsBold,
                                                      treeId = x.FirstOrDefault().TreeID,
                                                      childCount = x.FirstOrDefault().ChildCount,
                                                  }).AsEnumerable();
                            
                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData) 
                                        .Select(x => new PopupSearchModel()
                                        {
                                            id = x.id.ToString(),
                                            value = x.value,
                                            level = x.level,
                                            isBold = x.isBold
                                        }).ToList();                            
                        }
                        else
                        {
                            string dependValue = dependVal.ToString();

                            var temp = db.categorytree.Where(x => x.ProductID == new Guid(dependValue))
                                                      .GroupBy(x => x.CategoryID)
                                                      
                                                      .Select(x => new
                                                      {
                                                          id = x.FirstOrDefault().CategoryID,
                                                          value = x.FirstOrDefault().Name,
                                                          level = x.FirstOrDefault().Level,
                                                          isBold = x.FirstOrDefault().IsBold,
                                                          treeId = x.FirstOrDefault().TreeID
                                                      }).AsEnumerable();

                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData)
                                       .Select(x => new PopupSearchModel()
                                       {
                                           id = x.id.ToString(),
                                           value = x.value,
                                           level = x.level,
                                           isBold = x.isBold
                                       }).ToList();
                        }
                    }
                    else
                    {
                        if (dependVal == "")
                        {
                            var temp = db.categorytree.Where(x => x.CategoryName.Contains(searchText.ToUpper()))
                                                      .GroupBy(x => x.CategoryID)
                                                      
                                                      .Select(x => new
                                                      {
                                                          id = x.FirstOrDefault().CategoryID,
                                                          value = x.FirstOrDefault().Name,
                                                          level = x.FirstOrDefault().Level,
                                                          isBold = x.FirstOrDefault().IsBold,
                                                          treeId = x.FirstOrDefault().TreeID
                                                      }).AsEnumerable();

                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData)
                                       .Select(x => new PopupSearchModel()
                                       {
                                           id = x.id.ToString(),
                                           value = x.value,
                                           level = x.level,
                                           isBold = x.isBold
                                       }).ToList();
                        }
                        else
                        {
                            string dependValue = dependVal.ToString();

                            var temp = db.categorytree.Where(x => x.ProductID == new Guid(dependValue) && x.CategoryName.Contains(searchText.ToUpper()))
                                                      .GroupBy(x => x.CategoryID)
                                                      
                                                      .Select(x => new
                                                      {
                                                          id = x.FirstOrDefault().CategoryID,
                                                          value = x.FirstOrDefault().Name,
                                                          level = x.FirstOrDefault().Level,
                                                          isBold = x.FirstOrDefault().IsBold,
                                                          treeId = x.FirstOrDefault().TreeID
                                                      }).AsEnumerable();

                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData)
                                       .Select(x => new PopupSearchModel()
                                       {
                                           id = x.id.ToString(),
                                           value = x.value,
                                           level = x.level,
                                           isBold = x.isBold
                                       }).ToList();
                        }
                    }
                    break;
                case "product":
                    if (searchText == "")
                    {
                        if (dependVal == "")
                        {
                            var temp = db.producttree.GroupBy(x => x.ProductID)
                                                  
                                                  .Select(x => new
                                                  {
                                                      id = x.FirstOrDefault().ProductID,
                                                      value = x.FirstOrDefault().Name,
                                                      level = x.FirstOrDefault().Level,
                                                      isBold = x.FirstOrDefault().IsBold,
                                                      treeId = x.FirstOrDefault().TreeID
                                                  }).AsEnumerable();

                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData)
                                       .Select(x => new PopupSearchModel()
                                       {
                                           id = x.id.ToString(),
                                           value = x.value,
                                           level = x.level,
                                           isBold = x.isBold
                                       }).ToList();
                        }
                        else
                        {
                            string dependValue = dependVal.ToString();

                            var temp = db.producttree.Where(x => x.CategoryID == new Guid(dependValue))
                                                      .GroupBy(x => x.ProductID)
                                                      
                                                      .Select(x => new
                                                      {
                                                          id = x.FirstOrDefault().ProductID,
                                                          value = x.FirstOrDefault().Name,
                                                          level = x.FirstOrDefault().Level,
                                                          isBold = x.FirstOrDefault().IsBold,
                                                          treeId = x.FirstOrDefault().TreeID
                                                      }).AsEnumerable();

                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData)
                                       .Select(x => new PopupSearchModel()
                                       {
                                           id = x.id.ToString(),
                                           value = x.value,
                                           level = x.level,
                                           isBold = x.isBold
                                       }).ToList();
                        }
                    }
                    else
                    {
                        if (dependVal == "")
                        {
                            var temp = db.producttree.Where(x => x.ProductName.Contains(searchText.ToUpper()))
                                                      .GroupBy(x => x.ProductID)
                                                      
                                                      .Select(x => new
                                                      {
                                                          id = x.FirstOrDefault().ProductID,
                                                          value = x.FirstOrDefault().Name,
                                                          level = x.FirstOrDefault().Level,
                                                          isBold = x.FirstOrDefault().IsBold,
                                                          treeId = x.FirstOrDefault().TreeID
                                                      }).AsEnumerable();

                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData)
                                       .Select(x => new PopupSearchModel()
                                       {
                                           id = x.id.ToString(),
                                           value = x.value,
                                           level = x.level,
                                           isBold = x.isBold
                                       }).ToList();
                        }
                        else
                        {
                            string dependValue = dependVal.ToString();

                            var temp = db.producttree.Where(x => x.CategoryID == new Guid(dependValue) && x.ProductName.Contains(searchText.ToUpper()))
                                                      .GroupBy(x => x.ProductID)
                                                      
                                                      .Select(x => new
                                                      {
                                                          id = x.FirstOrDefault().ProductID,
                                                          value = x.FirstOrDefault().Name,
                                                          level = x.FirstOrDefault().Level,
                                                          isBold = x.FirstOrDefault().IsBold,
                                                          treeId = x.FirstOrDefault().TreeID
                                                      }).AsEnumerable();

                            data = temp.OrderBy(x => x.treeId).TakeWhile(x => (countData = x.level == 1 ? countData + 1 : countData) <= takeData)
                                       .Select(x => new PopupSearchModel()
                                       {
                                           id = x.id.ToString(),
                                           value = x.value,
                                           level = x.level,
                                           isBold = x.isBold
                                       }).ToList();
                        }
                    }
                    break;
            }

            var serializer = new JavaScriptSerializer();
            var listIsBoldS = serializer.Serialize(data);
            ViewBag.listIsBold = listIsBoldS;

            List<TreeRecusiveObject> list = new List<TreeRecusiveObject>();
            int tempLevel = 1;

            var obj = new TreeRecusiveObject();
            foreach (var item in data)
            {
                if (item.level == 1)
                {
                    string guid = item.id.ToString();
                    obj = new TreeRecusiveObject();
                    obj.id = new Guid(guid);
                    obj.text = item.value;
                    obj.isBold = item.isBold;
                    var listState = new Dictionary<string, bool>();
                    if (item.isBold == 0)
                    {
                        listState.Add("disabled", true);
                    }
                    if (item.id == hiddenInputId)
                        listState.Add("selected", true);
                    obj.state = listState;

                    list.Add(obj);
                    tempLevel = int.Parse(item.level.ToString());
                    continue;
                }
                if (item.level == tempLevel)
                {
                    string guid = item.id.ToString();
                    obj = new TreeRecusiveObject();
                    obj.id = new Guid(guid);
                    obj.text = item.value;
                    obj.isBold = item.isBold;
                    var listState = new Dictionary<string, bool>();
                    if (item.isBold == 0)
                    {
                        listState.Add("disabled", true);
                    }
                    if (item.id == hiddenInputId)
                        listState.Add("selected", true);
                    obj.state = listState;

                    int c = list.Count;
                    var node = list[c - 1];
                    for (int i = 1; i <= item.level - 2; i++)
                    {
                        if (node.nodes.Count < 1)
                        {
                            continue;
                        }
                        foreach (var e in node.nodes)
                        {
                            node = e;
                        }
                    }
                    node.nodes.Add(obj);
                }
                if (item.level < tempLevel)
                {
                    string guid = item.id.ToString();
                    obj = new TreeRecusiveObject();
                    obj.id = new Guid(guid);
                    obj.text = item.value;
                    obj.isBold = item.isBold;
                    var listState = new Dictionary<string, bool>();
                    if (item.isBold == 0)
                    {
                        listState.Add("disabled", true);
                    }
                    if (item.id == hiddenInputId)
                        listState.Add("selected", true);
                    obj.state = listState;

                    int c = list.Count;
                    var node = list[c - 1];
                    for (int i = 1; i <= item.level - 2; i++)
                    {
                        if (node.nodes.Count < 1)
                        {
                            continue;
                        }
                        foreach (var e in node.nodes)
                        {
                            node = e;
                        }
                    }
                    node.nodes.Add(obj);
                }
                if (item.level > tempLevel)
                {
                    int c = list.Count;
                    if (c < 1)
                    {
                        continue;
                    }
                    var lastIndex = walk(list[c - 1]);
                    string guid = item.id.ToString();
                    obj = new TreeRecusiveObject();
                    obj.id = new Guid(guid);
                    obj.text = item.value;
                    obj.isBold = item.isBold;
                    var listState = new Dictionary<string, bool>();
                    if (item.isBold == 0)
                    {
                        listState.Add("disabled", true);
                    }
                    if (item.id == hiddenInputId)
                        listState.Add("selected", true);
                    obj.state = listState;

                    lastIndex.nodes.Add(obj);
                }
                tempLevel = int.Parse(item.level.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadGrid(
            string searchField,
            string searchOper,
            string searchString,
            string sidx,
            string sord,
            int rows,
            int page = 1,
            string _searchText = "",
            string _hiddenInputId = "",
            string _entity = "",
            bool _search = false)
        {
            var searchText = Server.HtmlEncode(_searchText);
            var hiddenInputId = Server.HtmlEncode(_hiddenInputId);

            Object[] data = new Object[1];
            int pageSize = 0;
            int totalRecords = int.Parse(ConfigurationManager.AppSettings["RecordStaticCount"].ToString());
            int totalPages = 0;
            List<Guid?> buList = new List<Guid?>();

            switch (_entity)
            {
                case "customer":
                    ViewBag.LookFor = "Customer";
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.customer.Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.CustomerID.Equals(new Guid(hiddenInputId)));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "CISNumber":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CISNumber) : tempModel.OrderBy(x => x.CISNumber);
                                break;
                            case "IDNumber":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IdentityNo) : tempModel.OrderBy(x => x.IdentityNo);
                                break;
                            case "BusinessNo":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone1) : tempModel.OrderBy(x => x.Telephone1);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.EMailAddress1) : tempModel.OrderBy(x => x.EMailAddress1);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CustomerID,
                                    Name = x.FullName,
                                    CISNumber = x.CISNumber,
                                    IDNumber = x.IdentityNo,
                                    BusinessNo = x.Telephone1,
                                    EMail = x.EMailAddress1
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.customer.Where(x => x.DeletionStateCode == 0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "CISNumber":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CISNumber) : tempModel.OrderBy(x => x.CISNumber);
                                break;
                            case "IDNumber":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IdentityNo) : tempModel.OrderBy(x => x.IdentityNo);
                                break;
                            case "BusinessNo":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone1) : tempModel.OrderBy(x => x.Telephone1);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.EMailAddress1) : tempModel.OrderBy(x => x.EMailAddress1);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CustomerID,
                                    Name = x.FullName,
                                    CISNumber = x.CISNumber,
                                    IDNumber = x.IdentityNo,
                                    BusinessNo = x.Telephone1,
                                    EMail = x.EMailAddress1
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.customer.Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.FullName.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "CISNumber":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CISNumber) : tempModel.OrderBy(x => x.CISNumber);
                                break;
                            case "IDNumber":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IdentityNo) : tempModel.OrderBy(x => x.IdentityNo);
                                break;
                            case "BusinessNo":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone1) : tempModel.OrderBy(x => x.Telephone1);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.EMailAddress1) : tempModel.OrderBy(x => x.EMailAddress1);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CustomerID,
                                    Name = x.FullName,
                                    CISNumber = x.CISNumber,
                                    IDNumber = x.IdentityNo,
                                    BusinessNo = x.Telephone1,
                                    EMail = x.EMailAddress1
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
                        data = temp.ToArray();
                    }
                    break;


                case "noncustomer":
                    ViewBag.LookFor = "NonCustomer";
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.noncustomer.Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.NonCustomerID.Equals(new Guid(hiddenInputId)));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.PhoneNo) : tempModel.OrderBy(x => x.PhoneNo);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.NonCustomerID,
                                    Name = x.FullName,
                                    Phone = x.PhoneNo
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.noncustomer.Where(x => x.DeletionStateCode == 0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.PhoneNo) : tempModel.OrderBy(x => x.PhoneNo);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.NonCustomerID,
                                    Name = x.FullName,
                                    Phone = x.PhoneNo
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.noncustomer.Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.FullName.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.PhoneNo) : tempModel.OrderBy(x => x.PhoneNo);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.NonCustomerID,
                                    Name = x.FullName,
                                    Phone = x.PhoneNo
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
                        data = temp.ToArray();
                    }
                    break;


                case "categories":
                    var model = db.category.Where(x => x.DeletionStateCode == 0);
                    if (hiddenInputId != "")
                    {
                        var tempModel = model
                                    .Where(x => x.CategoryID == new Guid(hiddenInputId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "ParentName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ParentName) : tempModel.OrderBy(x => x.ParentName);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StateLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StateLabel) : tempModel.OrderBy(x => x.StateLabel);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CategoryID,
                                    Name = x.Name,
                                    Description = x.Description,
                                    ParentName = x.ParentName,
                                    ParentCategoryId = x.ParentCategoryID,
                                    CCQId = x.CCQId,
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
                        data = temp.ToArray();
                        
                    }
                    else if (searchText == "")
                    {
                        var tempModel = model;
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "ParentName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ParentName) : tempModel.OrderBy(x => x.ParentName);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StateLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StateLabel) : tempModel.OrderBy(x => x.StateLabel);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CategoryID,
                                    Name = x.Name,
                                    Description = x.Description,
                                    ParentName = x.ParentName,
                                    ParentCategoryId = x.ParentCategoryID,
                                    CCQId = x.CCQId,
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = model
                                    .Where(x => x.Name.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "ParentName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ParentName) : tempModel.OrderBy(x => x.ParentName);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StateLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StateLabel) : tempModel.OrderBy(x => x.StateLabel);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CategoryID,
                                    Name = x.Name,
                                    Description = x.Description,
                                    ParentName = x.ParentName,
                                    ParentCategoryId = x.ParentCategoryID,
                                    CCQId = x.CCQId,
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
                        data = temp.ToArray();
                    }
                    break;

                case "products":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.product.Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.ProductID == new Guid(hiddenInputId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "ParentName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ParentName) : tempModel.OrderBy(x => x.ParentName);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StateLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StateLabel) : tempModel.OrderBy(x => x.StateLabel);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.ProductID,
                                    Name = x.Name,
                                    Description = x.Description,
                                    CCQId = x.CCQId,
                                    ParentName = x.ParentName,
                                    ParentProductId = x.ParentProductID,
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.product.Where(x => x.DeletionStateCode == 0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "ParentName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ParentName) : tempModel.OrderBy(x => x.ParentName);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StateLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StateLabel) : tempModel.OrderBy(x => x.StateLabel);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.ProductID,
                                    Name = x.Name,
                                    Description = x.Description,
                                    CCQId = x.CCQId,
                                    ParentName = x.ParentName,
                                    ParentProductId = x.ParentProductID,
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.product.Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.Name.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "ParentName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ParentName) : tempModel.OrderBy(x => x.ParentName);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StateLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StateLabel) : tempModel.OrderBy(x => x.StateLabel);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.ProductID,
                                    Name = x.Name,
                                    Description = x.Description,
                                    CCQId = x.CCQId,
                                    ParentName = x.ParentName,
                                    ParentProductId = x.ParentProductID,
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
                        data = temp.ToArray();
                    }
                    break;

                case "branch":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.branch
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.BranchID == new Guid(hiddenInputId));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "OfficeCode":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.OfficeCode) : tempModel.OrderBy(x => x.OfficeCode);
                                break;
                            case "Address":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Address) : tempModel.OrderBy(x => x.Address);
                                break;
                            case "Fax":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Fax) : tempModel.OrderBy(x => x.Fax);
                                break;
                            case "Initials":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Initials) : tempModel.OrderBy(x => x.Initials);
                                break;
                            case "Zipcode":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Zipcode) : tempModel.OrderBy(x => x.Zipcode);
                                break;
                            case "City":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.City) : tempModel.OrderBy(x => x.City);
                                break;
                            case "Telephone1":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone1) : tempModel.OrderBy(x => x.Telephone1);
                                break;
                            case "Telephone2":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone2) : tempModel.OrderBy(x => x.Telephone2);
                                break;
                            case "RegionName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.RegionName) : tempModel.OrderBy(x => x.RegionName);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BranchID,
                                    Name = x.Name,
                                    OfficeCode = x.OfficeCode,
                                    LongDistanceCode = x.LongDistanceCode,
                                    Address = x.Address,
                                    Fax = x.Fax,
                                    Initials = x.Initials,
                                    Zipcode = x.Zipcode,
                                    City = x.City,
                                    Telephone1 = x.Telephone1,
                                    Telephone2 = x.Telephone1,
                                    RegionName = x.RegionName
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.branch.Where(x => x.DeletionStateCode == 0);

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "OfficeCode":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.OfficeCode) : tempModel.OrderBy(x => x.OfficeCode);
                                break;
                            case "Address":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Address) : tempModel.OrderBy(x => x.Address);
                                break;
                            case "Fax":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Fax) : tempModel.OrderBy(x => x.Fax);
                                break;
                            case "Initials":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Initials) : tempModel.OrderBy(x => x.Initials);
                                break;
                            case "Zipcode":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Zipcode) : tempModel.OrderBy(x => x.Zipcode);
                                break;
                            case "City":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.City) : tempModel.OrderBy(x => x.City);
                                break;
                            case "Telephone1":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone1) : tempModel.OrderBy(x => x.Telephone1);
                                break;
                            case "Telephone2":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone2) : tempModel.OrderBy(x => x.Telephone2);
                                break;
                            case "RegionName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.RegionName) : tempModel.OrderBy(x => x.RegionName);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BranchID,
                                    Name = x.Name,
                                    OfficeCode = x.OfficeCode,
                                    Address = x.Address,
                                    LongDistanceCode = x.LongDistanceCode,
                                    Fax = x.Fax,
                                    Initials = x.Initials,
                                    Zipcode = x.Zipcode,
                                    City = x.City,
                                    Telephone1 = x.Telephone1,
                                    Telephone2 = x.Telephone1,
                                    RegionName = x.RegionName
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.branch
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.Name.Contains(_searchText) || x.OfficeCode.Contains(_searchText));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "OfficeCode":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.OfficeCode) : tempModel.OrderBy(x => x.OfficeCode);
                                break;
                            case "Address":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Address) : tempModel.OrderBy(x => x.Address);
                                break;
                            case "Fax":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Fax) : tempModel.OrderBy(x => x.Fax);
                                break;
                            case "Initials":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Initials) : tempModel.OrderBy(x => x.Initials);
                                break;
                            case "Zipcode":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Zipcode) : tempModel.OrderBy(x => x.Zipcode);
                                break;
                            case "City":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.City) : tempModel.OrderBy(x => x.City);
                                break;
                            case "Telephone1":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone1) : tempModel.OrderBy(x => x.Telephone1);
                                break;
                            case "Telephone2":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Telephone2) : tempModel.OrderBy(x => x.Telephone2);
                                break;
                            case "RegionName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.RegionName) : tempModel.OrderBy(x => x.RegionName);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BranchID,
                                    Name = x.Name,
                                    OfficeCode = x.OfficeCode,
                                    Address = x.Address,
                                    LongDistanceCode = x.LongDistanceCode,
                                    Fax = x.Fax,
                                    Initials = x.Initials,
                                    Zipcode = x.Zipcode,
                                    City = x.City,
                                    Telephone1 = x.Telephone1,
                                    Telephone2 = x.Telephone1,
                                    RegionName = x.RegionName
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
                        data = temp.ToArray();
                    }
                    break;

                case "wsid":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.wsid
                                .Where(x => x.WSIDID == new Guid(hiddenInputId));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Address":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Address) : tempModel.OrderBy(x => x.Address);
                                break;
                            case "City":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.City) : tempModel.OrderBy(x => x.City);
                                break;
                            case "Lok":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Lok) : tempModel.OrderBy(x => x.Lok);
                                break;
                            case "Location":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Location) : tempModel.OrderBy(x => x.Location);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.WSIDID,
                                    Name = x.Name,
                                    Address = x.Address,
                                    City = x.City,
                                    Lok = x.Lok,
                                    Location = x.Location
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.wsid.Where(x => x.DeletionStateCode == 0);

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Address":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Address) : tempModel.OrderBy(x => x.Address);
                                break;
                            case "City":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.City) : tempModel.OrderBy(x => x.City);
                                break;
                            case "Lok":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Lok) : tempModel.OrderBy(x => x.Lok);
                                break;
                            case "Location":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Location) : tempModel.OrderBy(x => x.Location);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.WSIDID,
                                    Name = x.Name,
                                    Address = x.Address,
                                    City = x.City,
                                    Lok = x.Lok,
                                    Location = x.Location
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.wsid
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.Name.Contains(_searchText) || x.Location.Contains(_searchText) || x.Lok.Contains(_searchText));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Address":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Address) : tempModel.OrderBy(x => x.Address);
                                break;
                            case "City":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.City) : tempModel.OrderBy(x => x.City);
                                break;
                            case "Lok":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Lok) : tempModel.OrderBy(x => x.Lok);
                                break;
                            case "Location":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Location) : tempModel.OrderBy(x => x.Location);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.WSIDID,
                                    Name = x.Name,
                                    Address = x.Address,
                                    City = x.City,
                                    Lok = x.Lok,
                                    Location = x.Location
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
                        data = temp.ToArray();
                    }
                    break;

                case "cause":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.cause
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.CauseID == new Guid(hiddenInputId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CauseID,
                                    Name = x.Name,
                                    CreatedOn = x.CreatedOn
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.cause.Where(x => x.DeletionStateCode == 0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CauseID,
                                    Name = x.Name,
                                    CreatedOn = x.CreatedOn
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.cause
                                .Where(x => x.Name.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CauseID,
                                    Name = x.Name,
                                    CreatedOn = x.CreatedOn
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
                        data = temp.ToArray();
                    }
                    break;

                case "parentbusinessunit":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.businessunit
                           .Where(x => x.BusinessUnitId == new Guid(hiddenInputId)).Where(x => x.IsDisabled == false).Where(x => x.DeletionStateCode == 0);

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.businessunit.Where(x => x.IsDisabled == false).Where(x => x.DeletionStateCode == 0);

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.businessunit
                                .Where(x => x.Name.Contains(_searchText)).Where(x => x.IsDisabled == false).Where(x => x.DeletionStateCode == 0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    break;

                case "workgroup":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.queue.Where(x => x.DeletionStateCode == 0).Where(x => x.QueueTypeCode == 1)
                                .Where(x => x.QueueId == new Guid(hiddenInputId));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "EMailAddress":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.EMailAddress) : tempModel.OrderBy(x => x.EMailAddress);
                                break;
                            case "OwnerName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.OwnerName) : tempModel.OrderBy(x => x.OwnerName);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "QueueTypeLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.QueueTypeLabel) : tempModel.OrderBy(x => x.QueueTypeLabel);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.QueueId,
                                    Name = x.Name,
                                    BusinessUnitId = x.BusinessUnitID,
                                    BusinessUnitName = x.BusinessUnitName,
                                    EMailAddress = x.EMailAddress,
                                    OwnerId = x.OwnerId,
                                    OwnerName = x.OwnerName,
                                    Description = x.Description,
                                    QueueTypeCode = x.QueueTypeCode,
                                    QueueTypeLabel = x.QueueTypeLabel,
                                    ModifiedOn = x.ModifiedOn,
                                    DeletionStateCode = x.DeletionStateCode
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.queue.Where(x => x.DeletionStateCode == 0).Where(x => x.QueueTypeCode == 1);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "EMailAddress":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.EMailAddress) : tempModel.OrderBy(x => x.EMailAddress);
                                break;
                            case "OwnerName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.OwnerName) : tempModel.OrderBy(x => x.OwnerName);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "QueueTypeLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.QueueTypeLabel) : tempModel.OrderBy(x => x.QueueTypeLabel);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.QueueId,
                                    Name = x.Name,
                                    BusinessUnitId = x.BusinessUnitID,
                                    BusinessUnitName = x.BusinessUnitName,
                                    EMailAddress = x.EMailAddress,
                                    OwnerId = x.OwnerId,
                                    OwnerName = x.OwnerName,
                                    Description = x.Description,
                                    QueueTypeCode = x.QueueTypeCode,
                                    QueueTypeLabel = x.QueueTypeLabel,
                                    ModifiedOn = x.ModifiedOn,
                                    DeletionStateCode = x.DeletionStateCode
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.queue.Where(x => x.DeletionStateCode == 0).Where(x => x.QueueTypeCode == 1)
                                .Where(x => x.Name.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "EMailAddress":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.EMailAddress) : tempModel.OrderBy(x => x.EMailAddress);
                                break;
                            case "OwnerName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.OwnerName) : tempModel.OrderBy(x => x.OwnerName);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "QueueTypeLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.QueueTypeLabel) : tempModel.OrderBy(x => x.QueueTypeLabel);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.QueueId,
                                    Name = x.Name,
                                    BusinessUnitId = x.BusinessUnitID,
                                    BusinessUnitName = x.BusinessUnitName,
                                    EMailAddress = x.EMailAddress,
                                    OwnerId = x.OwnerId,
                                    OwnerName = x.OwnerName,
                                    Description = x.Description,
                                    QueueTypeCode = x.QueueTypeCode,
                                    QueueTypeLabel = x.QueueTypeLabel,
                                    ModifiedOn = x.ModifiedOn,
                                    DeletionStateCode = x.DeletionStateCode
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
                        data = temp.ToArray();
                    }
                    break;

                case "user":
                    if (searchText == "")
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Phone) : tempModel.OrderBy(x => x.Phone);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "Site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "Title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    Phone = x.Phone,
                                    BusinessUnitName = x.BusinessUnitName,
                                    Site = x.Site,
                                    Title = x.Title,
                                    EMail = x.InternalEMailAddress
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false)
                                        .Where(x => x.FullName.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Phone) : tempModel.OrderBy(x => x.Phone);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "Site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "Title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    Phone = x.Phone,
                                    BusinessUnitName = x.BusinessUnitName,
                                    Site = x.Site,
                                    Title = x.Title,
                                    EMail = x.InternalEMailAddress
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
                        data = temp.ToArray();
                    }
                    break;

                case "user_request":
                    Guid getBusinessUnitID = new Guid(Session["BusinessUnitID"].ToString());
                    
                    if (searchText == "")
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false)
                                        .Where(x => x.BusinessUnitID == getBusinessUnitID);
                        
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Phone) : tempModel.OrderBy(x => x.Phone);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "Site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "Title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    Phone = x.Phone,
                                    BusinessUnitName = x.BusinessUnitName,
                                    Site = x.Site,
                                    Title = x.Title,
                                    EMail = x.InternalEMailAddress
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false)
                                        .Where(x => x.FullName.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Phone) : tempModel.OrderBy(x => x.Phone);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "Site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "Title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    Phone = x.Phone,
                                    BusinessUnitName = x.BusinessUnitName,
                                    Site = x.Site,
                                    Title = x.Title,
                                    EMail = x.InternalEMailAddress
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
                        data = temp.ToArray();
                    }
                    break;

                case "queue":
                    if (searchText == "")
                    {
                        var tempModel = db.queue
                                .Where(x => x.QueueTypeCode == 1);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.QueueId,
                                    Name = x.Name
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.queue
                                .Where(x => x.QueueTypeCode == 1)
                                .Where(x => x.Name.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.QueueId,
                                    Name = x.Name
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
                        data = temp.ToArray();
                    }
                    break;
                case "businessunit":
                    buList.Add(new Guid(Session["BusinessUnitID"].ToString()));
                    getBUChild(buList, new Guid(Session["BusinessUnitID"].ToString()));

                    if (hiddenInputId != "")
                    {
                        var tempModel = db.businessunit.Where(x => x.IsDisabled == false)
                           .Where(x => x.BusinessUnitId == new Guid(hiddenInputId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.businessunit.Where(x => x.IsDisabled == false)
                            .Where(x => buList.Contains(x.BusinessUnitId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.businessunit.Where(x => x.IsDisabled == false)
                                .Where(x => x.Name.Contains(_searchText))
                                .Where(x => buList.Contains(x.BusinessUnitId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    break;

                case "businessunitqueue":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.businessunit.Where(x => x.IsDisabled == false)
                           .Where(x => x.BusinessUnitId == new Guid(hiddenInputId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.businessunit.Where(x => x.IsDisabled == false)
                            .Where(x => buList.Contains(x.BusinessUnitId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.businessunit.Where(x => x.IsDisabled == false)
                                .Where(x => x.Name.Contains(_searchText))
                                .Where(x => buList.Contains(x.BusinessUnitId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "IsDisabled":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.IsDisabled) : tempModel.OrderBy(x => x.IsDisabled);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitId,
                                    Name = x.Name,
                                    isDisabled = x.IsDisabled

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
                        data = temp.ToArray();
                    }
                    break;

                case "businessunit_org":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.organizationStructure.Where(x => x.DeletionStateCode == 0)
                           .Where(x => x.BusinessUnitID == new Guid(hiddenInputId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnit) : tempModel.OrderBy(x => x.BusinessUnit);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnit) : tempModel.OrderBy(x => x.BusinessUnit);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitID,
                                    Name = x.BusinessUnit

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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.organizationStructure.Where(x => x.DeletionStateCode == 0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnit) : tempModel.OrderBy(x => x.BusinessUnit);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnit) : tempModel.OrderBy(x => x.BusinessUnit);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitID,
                                    Name = x.BusinessUnit

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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.organizationStructure.Where(x => x.DeletionStateCode == 0)
                                    .Where(x => x.BusinessUnit.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnit) : tempModel.OrderBy(x => x.BusinessUnit);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnit) : tempModel.OrderBy(x => x.BusinessUnit);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.BusinessUnitID,
                                    Name = x.BusinessUnit

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
                        data = temp.ToArray();
                    }
                    break;

                case "owner":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false)
                                        .Where(x => x.SystemUserId == new Guid(hiddenInputId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.HomePhone) : tempModel.OrderBy(x => x.HomePhone);
                                break;
                            case "businessunit":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "email":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    phone = x.HomePhone,
                                    businessunit = x.BusinessUnitName,
                                    site = x.Site,
                                    title = x.Title,
                                    email = x.InternalEMailAddress

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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.HomePhone) : tempModel.OrderBy(x => x.HomePhone);
                                break;
                            case "businessunit":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "email":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    phone = x.HomePhone,
                                    businessunit = x.BusinessUnitName,
                                    site = x.Site,
                                    title = x.Title,
                                    email = x.InternalEMailAddress

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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false)
                                        .Where(x => x.FullName.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.HomePhone) : tempModel.OrderBy(x => x.HomePhone);
                                break;
                            case "businessunit":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "email":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    phone = x.HomePhone,
                                    businessunit = x.BusinessUnitName,
                                    site = x.Site,
                                    title = x.Title,
                                    email = x.InternalEMailAddress

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
                        data = temp.ToArray();
                    }
                    break;

                case "reports":
                    if (searchText == "")
                    {
                        var tempModel = db.masterReport.Where(x => x.Status == 1);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ReportName) : tempModel.OrderBy(x => x.ReportName);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ReportName) : tempModel.OrderBy(x => x.ReportName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.ReportID,
                                    Name = x.ReportName,
                                    Description = x.Description
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.masterReport.Where(x => x.Status == 1).Where(x => x.ReportName.Contains(_searchText) || x.Description.Contains(_searchText));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ReportName) : tempModel.OrderBy(x => x.ReportName);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ReportName) : tempModel.OrderBy(x => x.ReportName);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.ReportID,
                                    Name = x.ReportName,
                                    Description = x.Description
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
                        data = temp.ToArray();
                    }
                    break;

                case "reportsnew":
                    if (searchText == "")
                    {
                        var tempModel = db.report.Take(0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? db.report.OrderByDescending(x => x.Name) : db.report.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? db.report.OrderByDescending(x => x.Description) : db.report.OrderBy(x => x.Description);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? db.report.OrderByDescending(x => x.Name) : db.report.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.ID,
                                    Name = x.Name,
                                    Description = x.Description
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.report.Where(x => x.Name.Contains(_searchText) || x.Description.Contains(_searchText));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "Description":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.ID,
                                    Name = x.Name,
                                    Description = x.Description
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
                        data = temp.ToArray();
                    }
                    break;

                case "templates":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.letterTemplate
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.LetterTemplateId == new Guid(hiddenInputId));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "TypeLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.TypeLabel) : tempModel.OrderBy(x => x.TypeLabel);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "Desc1":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description1) : tempModel.OrderBy(x => x.Description1);
                                break;
                            case "Desc2":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description2) : tempModel.OrderBy(x => x.Description2);
                                break;
                            case "Desc3":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description3) : tempModel.OrderBy(x => x.Description3);
                                break;
                            case "Desc4":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description4) : tempModel.OrderBy(x => x.Description4);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.LetterTemplateId,
                                    Name = x.Name,
                                    AutoID = x.AutoID,
                                    TypeLabel = x.TypeLabel,
                                    CreatedOn = x.CreatedOn,
                                    Desc1 = x.Description1,
                                    Desc2 = x.Description2,
                                    Desc3 = x.Description3,
                                    Desc4 = x.Description4
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.letterTemplate
                                .Where(x => x.DeletionStateCode == 0);

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "TypeLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.TypeLabel) : tempModel.OrderBy(x => x.TypeLabel);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "Desc1":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description1) : tempModel.OrderBy(x => x.Description1);
                                break;
                            case "Desc2":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description2) : tempModel.OrderBy(x => x.Description2);
                                break;
                            case "Desc3":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description3) : tempModel.OrderBy(x => x.Description3);
                                break;
                            case "Desc4":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description4) : tempModel.OrderBy(x => x.Description4);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.LetterTemplateId,
                                    Name = x.Name,
                                    AutoID = x.AutoID,
                                    TypeLabel = x.TypeLabel,
                                    CreatedOn = x.CreatedOn,
                                    Desc1 = x.Description1,
                                    Desc2 = x.Description2,
                                    Desc3 = x.Description3,
                                    Desc4 = x.Description4
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.letterTemplate
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.Name.Contains(_searchText));

                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            case "TypeLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.TypeLabel) : tempModel.OrderBy(x => x.TypeLabel);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "Desc1":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description1) : tempModel.OrderBy(x => x.Description1);
                                break;
                            case "Desc2":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description2) : tempModel.OrderBy(x => x.Description2);
                                break;
                            case "Desc3":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description3) : tempModel.OrderBy(x => x.Description3);
                                break;
                            case "Desc4":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description4) : tempModel.OrderBy(x => x.Description4);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.LetterTemplateId,
                                    Name = x.Name,
                                    AutoID = x.AutoID,
                                    TypeLabel = x.TypeLabel,
                                    CreatedOn = x.CreatedOn,
                                    Desc1 = x.Description1,
                                    Desc2 = x.Description2,
                                    Desc3 = x.Description3,
                                    Desc4 = x.Description4
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
                        data = temp.ToArray();
                    }
                    break;

                case "securityrole":
                    string businesUnitId = Request["_dependBu"];

                    buList.Add(new Guid(businesUnitId));
                    getBUChild(buList, new Guid(businesUnitId));

                    if (hiddenInputId != "")
                    {
                        var tempModel = db.securityRole
                           .Where(x => x.XMLRole != null && x.XMLRole != "")
                           .Where(x => x.SecurityRoleId == new Guid(hiddenInputId))
                           .Where(x => buList.Contains(x.BusinessUnitId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SecurityRoleId,
                                    Name = x.Name
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.securityRole
                                .Where(x => x.XMLRole != null && x.XMLRole != "")
                                .Where(x => buList.Contains(x.BusinessUnitId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SecurityRoleId,
                                    Name = x.Name
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.securityRole
                                .Where(x => x.XMLRole != null && x.XMLRole != "")
                                .Where(x => x.Name.Contains(_searchText))
                                .Where(x => buList.Contains(x.BusinessUnitId));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Name) : tempModel.OrderBy(x => x.Name);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SecurityRoleId,
                                    Name = x.Name
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
                        data = temp.ToArray();
                    }
                    break;
                
                case "request":
                    if (searchText == "")
                    {
                        var tempModel = db.request
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.StatusCode != 5 && x.StatusCode != 6);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "category":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CategoryName) : tempModel.OrderBy(x => x.CategoryName);
                                break;
                            case "product":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ProductName) : tempModel.OrderBy(x => x.ProductName);
                                break;
                            case "status":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StatusLabel) : tempModel.OrderBy(x => x.StatusLabel);
                                break;
                            case "createdon":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "duedate":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.DueDate) : tempModel.OrderBy(x => x.DueDate);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.RequestID,
                                    Name = x.Title,
                                    ccqreqid = x.CCQRequestID,
                                    category = x.CategoryName,
                                    product = x.ProductName,
                                    status = x.StatusLabel,
                                    createdon = x.CreatedOn,
                                    duedate = x.DueDate

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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.request
                                .Where(x => x.DeletionStateCode == 0)
                                .Where(x => x.Title.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "category":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CategoryName) : tempModel.OrderBy(x => x.CategoryName);
                                break;
                            case "product":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ProductName) : tempModel.OrderBy(x => x.ProductName);
                                break;
                            case "status":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StatusLabel) : tempModel.OrderBy(x => x.StatusLabel);
                                break;
                            case "createdon":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "duedate":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.DueDate) : tempModel.OrderBy(x => x.DueDate);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.RequestID,
                                    Name = x.Title,
                                    ccqreqid = x.CCQRequestID,
                                    category = x.CategoryName,
                                    product = x.ProductName,
                                    status = x.StatusLabel,
                                    createdon = x.CreatedOn,
                                    duedate = x.DueDate

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
                        data = temp.ToArray();
                    }
                    break;

                case "requestTransaction":
                    string currentCustomerId = Request["_dependId"];
                    if (currentCustomerId == "undefined") currentCustomerId = "";

                    pageSize = rows;
                    totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                    if (searchText != "")
                    {
                        var tempModel = db.request.Where(x => x.DeletionStateCode == 0);

                        if (!string.IsNullOrEmpty(currentCustomerId))
                        {
                            tempModel = tempModel
                                    .Where(x => x.StatusCode == 5)
                                    .Where(x => x.CustomerID == new Guid(currentCustomerId))
                                    .Where(x => x.TicketNumber.Contains(searchText));
                        }
                        else
                        {
                            tempModel = tempModel
                                    .Where(x => x.StatusCode == 5)
                                    .Where(x => x.TicketNumber.Contains(searchText));
                        }

                        switch (sidx)
                        {
                            case "Summary":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.TicketNumber) : tempModel.OrderBy(x => x.TicketNumber);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StatusLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StatusLabel) : tempModel.OrderBy(x => x.StatusLabel);
                                break;
                            case "CategoryName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CategoryName) : tempModel.OrderBy(x => x.CategoryName);
                                break;
                            case "ProductName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ProductName) : tempModel.OrderBy(x => x.ProductName);
                                break;
                            case "DueDate":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.DueDate) : tempModel.OrderBy(x => x.DueDate);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.RequestID,
                                    Summary = x.Title,
                                    Name = x.TicketNumber,
                                    CustomerID = x.CustomerID,
                                    CCQRequestID = x.CCQRequestID,
                                    CreatedBy = x.CreatedBy,
                                    CreatedByName = x.CreatedByName,
                                    CreatedOn = x.CreatedOn,
                                    ModifiedBy = x.ModifiedBy,
                                    ModifiedByName = x.ModifiedByName,
                                    ModifiedOn = x.ModifiedOn,
                                    StatusLabel = x.StatusLabel,
                                    DeletionStateCode = x.DeletionStateCode,
                                    StateCode = x.StateCode,
                                    StatusCode = x.StatusCode,
                                    CategoryName = x.CategoryName,
                                    ProductName = x.ProductName,
                                    DueDate = x.DueDate
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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.request.Where(x => x.DeletionStateCode == 0);

                        if (!string.IsNullOrEmpty(currentCustomerId))
                        {
                            tempModel = tempModel
                                    .Where(x => x.StatusCode == 5)
                                    .Where(x => x.CustomerID == new Guid(currentCustomerId));
                        }
                        else
                        {
                            tempModel = tempModel
                                    .Where(x => x.StatusCode == 5);
                        }

                        switch (sidx)
                        {
                            case "Summary":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.TicketNumber) : tempModel.OrderBy(x => x.TicketNumber);
                                break;
                            case "CreatedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedByName) : tempModel.OrderBy(x => x.CreatedByName);
                                break;
                            case "CreatedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            case "ModifiedByName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedByName) : tempModel.OrderBy(x => x.ModifiedByName);
                                break;
                            case "ModifiedOn":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ModifiedOn) : tempModel.OrderBy(x => x.ModifiedOn);
                                break;
                            case "StatusLabel":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.StatusLabel) : tempModel.OrderBy(x => x.StatusLabel);
                                break;
                            case "CategoryName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CategoryName) : tempModel.OrderBy(x => x.CategoryName);
                                break;
                            case "ProductName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.ProductName) : tempModel.OrderBy(x => x.ProductName);
                                break;
                            case "DueDate":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.DueDate) : tempModel.OrderBy(x => x.DueDate);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.RequestID,
                                    Summary = x.Title,
                                    Name = x.TicketNumber,
                                    CustomerID = x.CustomerID,
                                    CCQRequestID = x.CCQRequestID,
                                    CreatedBy = x.CreatedBy,
                                    CreatedByName = x.CreatedByName,
                                    CreatedOn = x.CreatedOn,
                                    ModifiedBy = x.ModifiedBy,
                                    ModifiedByName = x.ModifiedByName,
                                    ModifiedOn = x.ModifiedOn,
                                    StatusLabel = x.StatusLabel,
                                    DeletionStateCode = x.DeletionStateCode,
                                    StateCode = x.StateCode,
                                    StatusCode = x.StatusCode,
                                    CategoryName = x.CategoryName,
                                    ProductName = x.ProductName,
                                    DueDate = x.DueDate
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
                        data = temp.ToArray();
                    }
                    break;

                case "callwrapupcategory":
                    if (hiddenInputId != "")
                    {
                        var tempModel = db.callWrapUpCategory.Where(x => x.CallWrapUpCategoryID == new Guid(hiddenInputId))
                                        .Where(x => x.StateCode == 0 && x.StatusCode == 1)
                                        .Where(x => x.DeletionStateCode == 0);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "Createdon":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CallWrapUpCategoryID,
                                    Name = x.Description,
                                    Createdon = x.CreatedOn

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
                        data = temp.ToArray();
                    }
                    else if (searchText == "")
                    {
                        var tempModel = db.callWrapUpCategory.Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.StateCode == 0 && x.StatusCode == 1);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "Createdon":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CallWrapUpCategoryID,
                                    Name = x.Description,
                                    Createdon = x.CreatedOn

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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.callWrapUpCategory.Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.StateCode == 0 && x.StatusCode == 1)
                                        .Where(x => x.Description.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Description) : tempModel.OrderBy(x => x.Description);
                                break;
                            case "Createdon":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.CallWrapUpCategoryID,
                                    Name = x.Description,
                                    Createdon = x.CreatedOn

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
                        data = temp.ToArray();
                    }
                    break;


                case "User":
                    if (searchText == "")
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false);
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Phone) : tempModel.OrderBy(x => x.Phone);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "Site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "Title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    Phone = x.Phone,
                                    BusinessUnitName = x.BusinessUnitName,
                                    Site = x.Site,
                                    Title = x.Title,
                                    EMail = x.InternalEMailAddress
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
                        data = temp.ToArray();
                    }
                    else
                    {
                        var tempModel = db.systemUser
                                        .Where(x => x.DeletionStateCode == 0)
                                        .Where(x => x.IsDisabled == false)
                                        .Where(x => x.FullName.Contains(_searchText));
                        pageSize = rows;
                        totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                        switch (sidx)
                        {
                            case "Name":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.FullName) : tempModel.OrderBy(x => x.FullName);
                                break;
                            case "Phone":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Phone) : tempModel.OrderBy(x => x.Phone);
                                break;
                            case "BusinessUnitName":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.BusinessUnitName) : tempModel.OrderBy(x => x.BusinessUnitName);
                                break;
                            case "Site":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Site) : tempModel.OrderBy(x => x.Site);
                                break;
                            case "Title":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.Title) : tempModel.OrderBy(x => x.Title);
                                break;
                            case "EMail":
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.InternalEMailAddress) : tempModel.OrderBy(x => x.InternalEMailAddress);
                                break;
                            default:
                                tempModel = (sord.ToLower() == "desc") ? tempModel.OrderByDescending(x => x.CreatedOn) : tempModel.OrderBy(x => x.CreatedOn);
                                break;
                        }

                        if (tempModel.Skip((page - 1) * pageSize).Take(pageSize).Count() == 0)
                        {
                            page--;
                            if (page < 1)
                            {
                                page = 1;
                            }
                        }

                        tempModel = tempModel.Skip((page - 1) * pageSize).Take(pageSize);

                        var temp = tempModel
                                .Select(x => new
                                {
                                    Id = x.SystemUserId,
                                    Name = x.FullName,
                                    Phone = x.Phone,
                                    BusinessUnitName = x.BusinessUnitName,
                                    Site = x.Site,
                                    Title = x.Title,
                                    EMail = x.InternalEMailAddress
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
                        data = temp.ToArray();
                    }
                    break;
            }

            var recordCount = totalRecords;
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = recordCount;
            jTable.rows = data;

            var t = data.Count();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        public List<Guid?> getBUChild(List<Guid?> BUUpdate, Guid? parentBU)
        {
            CRMDb db = new CRMDb();
            List<Guid?> buQuery = db.businessunit.Where(x => x.ParentBusinessUnitId == parentBU).Select(x => x.BusinessUnitId).ToList();

            foreach (Guid? currentBU in buQuery)
            {
                BUUpdate.Add(currentBU);
                getBUChild(BUUpdate, currentBU);
            }

            return BUUpdate;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        // added by william, so popup search can show all Business Unit for all user
        public List<Guid?> getBU(List<Guid?> BUUpdate, Guid? parentBU)
        {
            CRMDb db = new CRMDb();
            List<Guid?> buQuery = db.businessunit.Select(x => x.BusinessUnitId).ToList();

            foreach (Guid? currentBU in buQuery)
            {
                BUUpdate.Add(currentBU);
                getBUChild(BUUpdate, currentBU);
            }

            return BUUpdate;
        }

    }
}
