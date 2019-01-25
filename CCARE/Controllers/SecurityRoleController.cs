using CCARE.App_Function;
using CCARE.Models;
using CCARE.Models.General;
using CCARE.Models.Role;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using CCARE.jqGrid;

namespace CCARE.Controllers
{
    public class SecurityRoleController : Controller
    {
        CRMDb db = new CRMDb();
        string xmlPath = @"\App_Data\SecurityRole.xml";

        public ActionResult formSubmit(RetrieveRole model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.SecurityRoleSuccess;

            SecurityRole securityRole = new SecurityRole();
            securityRole.Name = model.role.Name;
            securityRole.SecurityRoleId = model.role.SecurityRoleId;

            KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
            if (actionType == "Create")
            {
                //Check if there is already role with the same name
                int compareRoleName = db.securityRole.Where(x => x.Name == model.role.Name).Count();
                if (compareRoleName > 0)
                {
                    var jsonData = new { flag = false, Message = "Security role tidak boleh sama" };
                    return Json(jsonData);
                }

                securityRole.BusinessUnitId = model.role.BusinessUnitId;
                securityRole.CreatedOn = DateTime.Now;
                securityRole.CreatedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());
                securityRole.DeletionStateCode = 0;
                securityRole.Status = 0;
                securityRole.FileModifiedDate = DateTime.ParseExact(System.IO.File.GetLastWriteTime(Server.MapPath(xmlPath)).ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", null);
                securityRole.XMLRole = ParseXMLHelper.SerializeXml(model.xmlRole);

                db.securityRole.Add(securityRole);
                db.SaveChanges();

                results = new KeyValuePair<int, string>(0, "Role berhasil ditambahkan");
            }
            else if (actionType == "Edit")
            {
                securityRole.ModifiedOn = DateTime.Now;
                securityRole.ModifiedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());
                securityRole.BusinessUnitId = model.role.BusinessUnitId;
                securityRole.XMLRole = ParseXMLHelper.SerializeXml(model.xmlRole);
                securityRole.FileModifiedDate = DateTime.ParseExact(System.IO.File.GetLastWriteTime(Server.MapPath(xmlPath)).ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", null);

                securityRole.DeletionStateCode = model.role.DeletionStateCode;
                securityRole.Status = model.role.Status;
                securityRole.CreatedBy = model.role.CreatedBy;
                securityRole.CreatedOn = model.role.CreatedOn;

                db.Entry(securityRole).State = EntityState.Modified;
                db.SaveChanges();

                results = new KeyValuePair<int, string>(0, "Role berhasil diupdate");
            }
            //else if (actionType == "Delete")
            //{
            //    //rcm.ModifiedBy_ID = new Guid(Session["CurrentUserID"].ToString());
            //    //results = sp.RequestCreationMatrix_Delete(rcm);
            //}

            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("Edit", "SecurityRole", new { id = securityRole.SecurityRoleId, success = successMessage });
                string urlNew = u.Action("Create", "SecurityRole");

                var jsonData = new { flag = true, Message = url, urlNew = urlNew };
                return Json(jsonData);
            }
            else
            {
                var jsonData = new { flag = false, Message = results.Value.ToString() };
                return Json(jsonData);
            }
        }

        public ActionResult Index()
        {
            //ViewBag.BusinessUnit = new SelectList(db.businessunit, "BusinessUnitId", "Name");
            return View();
        }

        public ActionResult Create()
        {
            //ViewBag.BusinessUnit = new SelectList(db.businessunit, "BusinessUnitId", "Name");

            RetrieveRole model = new RetrieveRole();
            SecurityRole role = new SecurityRole();
            model.role = role;
            Root xmlRoot = (Root)ParseXMLHelper.DeserializeXml(xmlPath, "file");
            model.xmlRole = xmlRoot;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RetrieveRole model)
        {
            model.role.SecurityRoleId = Guid.NewGuid();

            return formSubmit(model, "Create");
        }

        //Check submenu on all xml
        public RootMenuSubmenu getSubmenu(Root rootDB, string subMenuName)
        {
            RootMenuSubmenu data =
                (
                    from menu in rootDB.Menu
                    from submenu in menu.Submenu
                    where submenu.name == subMenuName
                    select new RootMenuSubmenu { Entity = submenu.Entity, check = submenu.check, name = submenu.name, text = submenu.text }
                ).SingleOrDefault();

            return data;
        }

        //Check submenu on all xml
        public RootMenuSubmenuEntity getEntity(Root rootDB, string entityName)
        {
            RootMenuSubmenuEntity data =
                (
                    from menu in rootDB.Menu
                    from submenu in menu.Submenu
                    from entity in submenu.Entity
                    where entity.name == entityName
                    select new RootMenuSubmenuEntity { Action = entity.Action, check = entity.check, name = entity.name, text = entity.text, path = entity.path }
                ).SingleOrDefault();

            return data;
        }


        public Root functionUpdateXML(Root xmlFile, Root xmlDb)
        {
            for (int m = 0; m < xmlFile.Menu.Length; m++)
            {
                //Modified Menu XML
                var menuDB = xmlDb.Menu.Where(x => x.name == xmlFile.Menu[m].name).SingleOrDefault();
                if (menuDB != null)
                {
                    //rootResult.Menu[m].text = menuDB.text;
                    xmlFile.Menu[m].check = menuDB.check;
                }

                for (int sub = 0; sub < xmlFile.Menu[m].Submenu.Length; sub++)
                {
                    //Modified Submenu XML
                    //var submenuDB = menuDB.Submenu.Where(x => x.name == rootResult.Menu[m].Submenu[sub].name).SingleOrDefault();
                    var submenuDB = getSubmenu(xmlDb, xmlFile.Menu[m].Submenu[sub].name);
                    if (submenuDB != null)
                    {
                        //rootResult.Menu[m].Submenu[sub].text = submenuDB.text;
                        xmlFile.Menu[m].Submenu[sub].check = submenuDB.check;
                    }

                    if (xmlFile.Menu[m].Submenu[sub].Entity != null)
                    {
                        for (int e = 0; e < xmlFile.Menu[m].Submenu[sub].Entity.Length; e++)
                        {
                            //Modified Entity XML
                            //var entityDB = submenuDB.Entity.Where(x => x.name == rootResult.Menu[m].Submenu[sub].Entity[e].name).SingleOrDefault();
                            var entityDB = getEntity(xmlDb, xmlFile.Menu[m].Submenu[sub].Entity[e].name);
                            if (entityDB != null)
                            {
                                //rootResult.Menu[m].Submenu[sub].Entity[e].text = entityDB.text;
                                xmlFile.Menu[m].Submenu[sub].Entity[e].check = entityDB.check;
                                //rootResult.Menu[m].Submenu[sub].Entity[e].path = entityDB.path;
                            }

                            if (xmlFile.Menu[m].Submenu[sub].Entity[e].Action != null)
                            {
                                for (int a = 0; a < xmlFile.Menu[m].Submenu[sub].Entity[e].Action.Length; a++)
                                {
                                    //Modified Action XML
                                    if (entityDB != null)
                                    {
                                        if (entityDB.Action == null) continue;

                                        var actionDB = entityDB.Action.Where(x => x.name == xmlFile.Menu[m].Submenu[sub].Entity[e].Action[a].name).SingleOrDefault();
                                        if (actionDB != null)
                                        {
                                            //rootResult.Menu[m].Submenu[sub].Entity[e].Action[a].text = actionDB.text;
                                            xmlFile.Menu[m].Submenu[sub].Entity[e].Action[a].check = actionDB.check;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return xmlFile;
        }

        //Edit load xml from
        public ActionResult Edit(Guid Id)
        {
            ViewBag.BusinessUnit = new SelectList(db.businessunit, "BusinessUnitId", "Name");
            ViewBag.actionType = "Edit";

            var getRoleDB = db.securityRole
                .Where(x => x.SecurityRoleId == Id)
                .FirstOrDefault();

            //Get xml from file
            Root rootFile = (Root)ParseXMLHelper.DeserializeXml(xmlPath, "file");
            //Get xml from DB
            Root rootDB = new Root();
            if (getRoleDB.XMLRole != null && getRoleDB.XMLRole != "")
            {
                rootDB = (Root)ParseXMLHelper.DeserializeXml(getRoleDB.XMLRole, "db");
            }

            Root rootResult = rootFile;

            RetrieveRole model = new RetrieveRole();
            model.role = getRoleDB;

            DateTime fileModified = DateTime.ParseExact(System.IO.File.GetLastWriteTime(Server.MapPath(xmlPath)).ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", null);
            bool isFileDBSame = fileModified == getRoleDB.FileModifiedDate;

            //If new table role empty get xml from file
            if (getRoleDB.FileModifiedDate == null && getRoleDB.XMLRole == null)
            {

            }
            //else if (isFileDBSame == true)
            //{
            //    rootResult = rootDB;
            //}
            //IF there are any new entities added to xml file then compare and add new entity and action to xml db string
            else //if (isFileDBSame == false)
            {
                rootResult = functionUpdateXML(rootFile, rootDB);
            }

            model.xmlRole = rootResult;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RetrieveRole model)
        {
            return formSubmit(model, "Edit");
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            CRMDb db = new CRMDb();
            //string getParam = Request["_param"];
            string getVal = Request["_val"];
            //string getFilter = Request["_filter"];

            var role = db.securityRole.Where(x => x.DeletionStateCode == 0);

            if (getVal != "" && getVal != null)
            {
                role = role.Where(x => x.Name.Contains(getVal));
            }

            var data = role.ToList()
                .Select(x => new
                {
                    Id = x.SecurityRoleId,
                    Name = x.Name,
                    BusinessUnit = (x.BusinessUnitId == null ) ? string.Empty : x.businessUnit.Name,
                    Status = x.Status
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

        public ActionResult JobUpdateText()
        {
            var model = db.securityRole.
                            Where(x => x.XMLRole != null).
                            Where(x => x.BusinessUnitId != null).
                            OrderBy(x => x.Name).ToList();

            List<string> results = new List<string>();

            foreach (SecurityRole entry in model)
            {
                string message = "Role " + entry.Name;
                bool flag = UpdateXMLRole(entry);
                if (flag)
                {
                    message += " berhasil diupdate!";
                }
                else
                {
                    message += " gagal diupdate!";
                }
                results.Add(message);
            }

            var jsonData = new { Results = results };
            return Json(jsonData);
        }

        private bool UpdateXMLRole(SecurityRole entryRole)
        {
            bool flag = false;
            CRMDb conn = new CRMDb();
            
            RetrieveRole retrieveRole = new RetrieveRole();

            //var role = db.securityRole.Find(SecurityRoleId);

            retrieveRole.role = entryRole;

            Root xmlFile = (Root)ParseXMLHelper.DeserializeXml(xmlPath, "file");

            Root dbFile = new Root();

            if (entryRole.XMLRole != null && entryRole.XMLRole != "")
            {
                dbFile = (Root)ParseXMLHelper.DeserializeXml(entryRole.XMLRole, "db");

                dbFile = functionUpdateXML(xmlFile, dbFile);
            }

            retrieveRole.xmlRole = dbFile;

            SecurityRole securityRole = conn.securityRole.Find(retrieveRole.role.SecurityRoleId);

            securityRole.ModifiedOn = DateTime.Now;
            securityRole.ModifiedBy = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());
            //securityRole.BusinessUnitId = retrieveRole.role.BusinessUnitId;
            securityRole.XMLRole = ParseXMLHelper.SerializeXml(retrieveRole.xmlRole);
            securityRole.FileModifiedDate = DateTime.ParseExact(System.IO.File.GetLastWriteTime(Server.MapPath(xmlPath)).ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", null);
            
            conn.Entry(securityRole).State = EntityState.Modified;

            conn.SaveChanges();
            flag = true;

            return flag;
        }
    }
}