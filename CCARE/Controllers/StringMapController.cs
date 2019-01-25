using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.Models.General;

namespace CCARE.Controllers
{
    public class StringMapController : Controller
    {
        public CCARE.Models.General.CRMDb db = new CCARE.Models.General.CRMDb();

        public SelectList getDropDown(string entityName, String attrName, int? selectedVal)
        {
            if (attrName == "caseorigincode")
            {
                SelectList list2 = new SelectList(db.pickList
                    .OrderBy(x => x.label)
                    .Where(x => x.EntityName.Equals(entityName))
                    .Where(x => x.AttributeName.Contains(attrName))
                    , "AttributeValue", "label", selectedVal);

                return list2;
            }
            else
            {
                SelectList list = new SelectList(db.pickList
                        .OrderBy(x => x.AttributeValue)
                        .Where(x => x.EntityName.Equals(entityName))
                        .Where(x => x.AttributeName.Equals(attrName))
                        , "AttributeValue", "label", selectedVal);

                return list;
            }
        }
    }
}
