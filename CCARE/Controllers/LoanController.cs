//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using System.Globalization;
using System.Collections.Specialized;
using BCA.CRM.OSB.Model;

namespace CCARE.Controllers
{
    public class LoanController : Controller
    {
        public ActionResult Index()
        {
            Loan model = new Loan();

            if (!string.IsNullOrEmpty(Request["Id"])) model.Id = new Guid(Request["Id"]);
            model.AccountNo = string.IsNullOrEmpty(Request["AccountNo"]) ? string.Empty : Request["AccountNo"];
            model.LoanNumber = string.IsNullOrEmpty(Request["LoanNumber"]) ? string.Empty : Request["LoanNumber"];
            if (!string.IsNullOrEmpty(Request["CustomerId"])) model.CustomerId = new Guid(Request["CustomerId"]);
            model.CustomerIdName = string.IsNullOrEmpty(Request["CustomerIdName"]) ? string.Empty : Request["CustomerIdName"];
            model.LoanTypeIdCode = string.IsNullOrEmpty(Request["LoanTypeIdCode"]) ? string.Empty : Request["LoanTypeIdCode"];
            model.LoanTypeIdName = string.IsNullOrEmpty(Request["LoanTypeIdName"]) ? string.Empty : Request["LoanTypeIdName"];

            return View(model);
        }

        public ActionResult LoanPartial()
        {
            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            string accountno = Request["accountNo"];
            string loannumber = Request["loanNumber"];
            char pad = '0';

            param.RequestTransType = "GetLoanInfoDetail";
            param.Parameter.Add("acctNo", accountno);
            param.Parameter.Add("noteNo", loannumber.PadLeft(5, pad));

            LoanInfo model = ProductInquiry.Loan(param);

            if (model != null &&  model.LoanNumber != null)
            {
                ViewBag.searchResults = model;
                ViewBag.searchResultsCount = 1;
                ViewBag.searchResultsList = model.Installments;
                ViewBag.searchResultsListCount = model.Installments.Count;
            }

            return PartialView("_Loan", model);
        }

    }
}
