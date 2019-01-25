using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using System.Globalization;
using CCARE.Models;
using CCARE.Models.General;

namespace CCARE.Controllers
{
    public class CustomerAddressController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }
      
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            var data = db.customeradress
                .Where(x => x.AddressNumber > 2)
                .OrderBy(x => x.CustomerAddressId)
                .Select(x => new
                {
                    Id = x.CustomerAddressId,
                    ParentId = x.ParentId,
                    AddressTypeLabel = x.AddressTypeLabel,
                    Street1 = x.Line1,
                    Street2 = x.Line2,
                    Street3 = x.Line3,
                    City = x.City,
                    PostalCode = x.PostalCode,
                    TelephoneNo = x.Telephone1,
                    CreditCardCustomerNo = x.CreditCardCustomerNo,
                    ModifiedOn = x.ModifiedOn
                });


            string parentid = Request["_parentid"];

            data = data.Where(x => x.ParentId == new Guid(parentid));

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

        public ActionResult Edit(Guid id)
        {
            CustomerAddress model = db.customeradress.Find(id);

            StringMapController dropDown = new StringMapController();

            ViewBag.displayStatus = "disabled";

            //buat hide save and new di form
            ViewBag.hideSaveAndOpen = true;

            //buat dropdown di form
            ViewBag.SL_AddressType = dropDown.getDropDown("Contact", "address1_addresstypecode", model.AddressTypeCode == null ? 0 : (int)model.AddressTypeCode);

            return View(model);
        }

    }
}
