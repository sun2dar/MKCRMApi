using CCARE.Models.General;
using CCARE.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.App_Function
{
    public class RoleNPrivilege
    {

        // Check Access of Entity
        public static bool HasAccess(Guid RoleID, string entity) { 
            bool hasAccess = false;
            Root role = RoleNPrivilege.getRoleObject(RoleID);
            
            foreach (RootMenu menu in role.Menu.Where(x => x.check == true)) 
            { 
                foreach(RootMenuSubmenu subMenu in menu.Submenu.Where(x=>x.check == true))
                {
                    foreach(RootMenuSubmenuEntity entry in subMenu.Entity.Where(x=>x.check == true))
                    {
                        if (entity.Equals(entry.name)) {
                            hasAccess = true;
                            break;
                        }
                    }
                }
            }
            return hasAccess;
        }



        //Get XML role for writing menu
        public static Root getRoleObject(Guid roleID)
        {
            CRMDb db = new CRMDb();
            Root roleObject = new Root();
            var getCurrentRole = db.securityRole.Find(roleID);

            roleObject = (Root)ParseXMLHelper.DeserializeXml(getCurrentRole.XMLRole, "db"); ;
            return roleObject;
        }

        //Check entity by roleid - used on call wrapup function
        public static bool isEntityChecked(Guid roleID, string entityName)
        {
            CRMDb db = new CRMDb();
            var getCurrentRole = db.securityRole.Find(roleID);
            Root roleObject = (Root)ParseXMLHelper.DeserializeXml(getCurrentRole.XMLRole, "db"); ;

            if (roleObject.Menu.Any(a => a.Submenu.Any(b => b.Entity.Any(c => c.name == entityName && c.check == true))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Check action by roleid to decide when the button should be showed or hidden
        public static bool checkEntityButtonByRoleID(Guid roleID, string entityName, string action)
        {
            CRMDb db = new CRMDb();
            var getCurrentRole = db.securityRole.Find(roleID);
            Root roleObject = (Root)ParseXMLHelper.DeserializeXml(getCurrentRole.XMLRole, "db"); ;

            if (roleObject.Menu.Any(a => a.Submenu.Any(b => b.Entity.Any(c => c.name == entityName && c.Action != null))))
            {
                bool isCheck = roleObject.Menu.Any(a => a.Submenu.Any(b => b.Entity.Any(c => c.name == entityName && c.Action.Any(d => d.name == action && d.check == true))));
                return isCheck;
            }
            else
            {
                return false;
            }
        }

        //Check action by xml role to decide when the button should be showed or hidden
        public static bool checkEntityButtonByXML(Root roleObject, string entityName, string action)
        {
            RootMenuSubmenuEntity entity = new RootMenuSubmenuEntity();
            entity.name = entityName;

            if (roleObject.Menu.Any(a => a.Submenu.Any(b => b.Entity.Any(c => c.name == entityName && c.Action != null))))
            {
                bool isCheck = roleObject.Menu.Any(a => a.Submenu.Any(b => b.Entity.Any(c => c.name == entityName && c.Action.Any(d => d.name == action && d.check == true))));
                return isCheck;
            }
            else
            {
                return false;
            }
        }

        //Get all actions list to from selected role and entityname
        public static List<RootMenuSubmenuEntityAction> getActionButton(Guid roleID, string entityName)
        {
            CRMDb db = new CRMDb();
            var getCurrentRole = db.securityRole.Find(roleID);
            Root roleObject = (Root)ParseXMLHelper.DeserializeXml(getCurrentRole.XMLRole, "db"); ;


            var ent =
                (
                    from menu in roleObject.Menu
                    from submenu in menu.Submenu
                    from entity in submenu.Entity
                    where entity.name == entityName && entity.Action != null

                    //where entity.name == entityName
                    //where action != null && action.check == true
                    select new RootMenuSubmenuEntity { Action = entity.Action }
                ).ToList();

            List<RootMenuSubmenuEntityAction> actionList = ent.SelectMany(x => x.Action).ToList();

            return actionList;
        }
    }
}