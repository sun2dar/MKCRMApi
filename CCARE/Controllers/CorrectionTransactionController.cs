using CCARE.App_Function;
using CCARE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models.General;
using System.Windows;
using System.ComponentModel;
using System.Data;
using CCARE.Models.Transaction;

namespace CCARE.Controllers
{
    public class CorrectionTransactionController : Controller
    {
        private HistDb db = new HistDb();

        
        public ActionResult Index()
        {
            return View();
        }
       
        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            string getAccountNo = Request["AccountNo"].Trim();
            string startDate = string.Empty;
            string endDate = string.Empty;

            if (Request["TransDate1"] != "")
            {
                startDate = Formatter.FormatDateHist(DateTime.ParseExact(Request["TransDate1"], "dd/MM/yyyy", null));
                if (Request["TransDate2"] != "")
                {
                    if (Request["TransDate2"] == Request["TransDate1"])
                    {
                        endDate = Formatter.FormatDateHist(DateTime.ParseExact(Request["TransDate1"], "dd/MM/yyyy", null).AddDays(1));
                    }
                    else {
                        endDate = Formatter.FormatDateHist(DateTime.ParseExact(Request["TransDate2"], "dd/MM/yyyy", null).AddDays(1));
                    }
                }
                else if(Request["TransDate2"] == "")
                {
                    endDate = Formatter.FormatDateHist(DateTime.ParseExact(Request["TransDate1"], "dd/MM/yyyy", null).AddDays(1));
                }
            }

            var data = db.GetHistoryCorrectionTransactions(getAccountNo, startDate, endDate).ToList()
                                .Select(x => new
                                {
                                    Name = x.Name,
                                    Branch  = x.Branch,
                                    Currency = x.Currency,
                                    AccountNo = x.AccountNo,
                                    TransactionDate = x.TransactionDate,
                                    InputDate = x.InputDate,
                                    DebitCredit = x.DebitCredit,
                                    ProsesDate  = x.ProsesDate,
                                    WSID = x.Wsid,
                                    Slid = x.Slid,
                                    Kt = x.Kt,
                                    Rate = x.Rate,
                                    NilaiIDR = x.NilaiIDR,
                                    NilaiVAL = x.NilaiVAL,
                                    HasilIDR  = x.HasilIDR,
                                    HasilVAL = x.HasilVAL,
                                    GagalIDR = x.GagalIDR,
                                    GagalVAL = x.GagalVAL,
                                    Description = x.Description,
                                    RowNo = x.RowNo,
                                    RequestID = x.RequestId,
                                    DateOnly = x.DateOnly,
                                    Time = x.Time,
                                    Date2 = x.TransactionDate.ToString(),
                                    InputDate2 = x.InputDate,
                                    ProsesDate2 = x.ProsesDate,
                                    TransactionDate2 = x.TransactionDate
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

        [HttpPost]
        public ActionResult Update(CorrectionTransaction model)
        {
            return formSubmit(model, "Update");
        }


        public ActionResult formSubmit(CorrectionTransaction model, string actionType)
        {
            List<string> errorMessage = new List<string>();
            string successMessage = Resources.NotifResource.RequestSuccess;

                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, "");
                string AccountNo = Request["AccountNo"].Trim();
                DateTime TransactionDate = DateTime.Parse(Request["TransactionDate"].Trim());
                string RowNo = Request["RowNo"].Trim();
                string RequestID = Request["RequestID"].Trim();

                db.UpdateHistoryCorrectionTransaction(AccountNo, TransactionDate, RowNo, RequestID);

                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("Index", "CorrectionController");
            
                var jsonData = new { flag = true, Message = url };
                return Json(jsonData);
        }
    }
}
