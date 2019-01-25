using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.App_Function;
using System.Globalization;
using System.Configuration;
using System.Collections.Specialized;
using CCARE.Models;
using CCARE.Models.General;
using BCA.CRM.OSB.Model;
using System.Web.Script.Serialization;
using System.Web.UI;
using DevTrends.MvcDonutCaching;

using System.Resources;
using System.Collections;

namespace CCARE.Controllers
{
    public partial class CustomerController : Controller
    {
        private CRMDb db = new CRMDb();

        public ActionResult Index()
        {
            return View();
        }

        private IQueryable<Customer> GetCustomerList(Params paramList)
        {
            
            DBServices dbservices = new DBServices();
            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            string ccCardNumber = string.Empty;
            if (paramList.RequestTransType == "SearchCustByCC")
            {
                ccCardNumber = paramList.Parameter["cardNo"];
            }
            /*
            string customersegmentation = string.Empty;

            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            data = EAI.RetrieveESBData(paramList);

            string ccCardNumber = string.Empty;
            if (paramList.RequestTransType == "SearchCustByCC")
            {
                ccCardNumber = paramList.Parameter["cardNo"];
            }

            return (data.Result == null || data.Result.Count() == 0) ? GetCustomerinCRM(paramList) : dbservices.InsertUpdateCustomer(data.Result, ccCardNumber).AsQueryable();
            */
            return GetCustomerinCRM(paramList);
            //dbservices.InsertUpdateCustomer(data.Result, ccCardNumber).AsQueryable();
        }

        public JsonResult LoadGrid(string sidx, string sord, int rows, int page = 1)
        {
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            paramList = BuildParams(Request["_criteria"], Request["_fullname"], Request["_birthdate"], Request["_gender"], Request["_cisnumber"],
                                    Request["_product"], Request["_productparam"], Request["_productparamval"], Request["_ebanking"], Request["_ebankingparam"], Request["_ebankingparamval"]);

            var data = GetCustomerList(paramList);
            
            int pageSize = rows;
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.OrderBy(x => x.FullName).Skip((page - 1) * pageSize).Take(pageSize);
            
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.Select(x => new {
                                        Id = x.CustomerID,
                                        FullName = x.FullName,
                                        BirthPlace = x.BirthPlace,
                                        BirthDate = x.BirthDate,
                                        TelephoneNo = x.Telephone2,
                                        HandphoneNo = x.MobilePhone,
                                        MothersMaidenName = x.MothersMaidenName,
                                        Gender = x.GenderLabel
                                        }).ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        




        private Params BuildParams(string criteria, string fullname = null, string birthdate = null, string gender = null, string cisnumber = null,
								   string product = null, string productparam = null, string productparamval = null, string ebanking = null, string ebankingparam = null, string ebankingparamval = null)
        {
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            paramList.WSDL = "CustomerInformationInquiry";

			if (criteria == "ebanking")
			{
				ebankingparamval = String.IsNullOrEmpty(ebankingparamval) ? null : ebankingparamval;
				ebankingparamval = Server.HtmlEncode(ebankingparamval);

				if (ebanking == "KlikBCA Individu")
				{
					if (ebankingparam == "Card No")
					{
                        paramList.RequestTransType = "SearchCustByATM";
						paramList.Parameter.Add("atmNo", ebankingparamval);
					}
					else if (ebankingparam == "User ID")
					{
                        paramList.RequestTransType = "SearchCustByIB";
						paramList.Parameter.Add("userId", ebankingparamval);
					}
					else if (ebankingparam == "E-Mail")
					{
                        paramList.RequestTransType = "SearchCustInfoByEmailIB";
						paramList.Parameter.Add("emailIB", ebankingparamval);
					}
                    else if (ebankingparam == "S/N Key BCA")
					{
                        paramList.RequestTransType = "SearchCustByToken";
						paramList.Parameter.Add("snToken", ebankingparamval);
					}
				}
				else if (ebanking == "KlikBCA Bisnis")
				{
					if (ebankingparam == "Corporate ID")
					{
                        paramList.RequestTransType = "SearchCustByCorpId";
						paramList.Parameter.Add("corpId", ebankingparamval);
					}
					else if (ebankingparam == "E-Mail")
					{
                        paramList.RequestTransType = "SearchCustByCorporateEmail";
						paramList.Parameter.Add("corporateEmail", ebankingparamval);
					}
                    else if (ebankingparam == "S/N Key BCA")
					{
                        paramList.RequestTransType = "SearchCustByTokenSME";
						paramList.Parameter.Add("snToken", ebankingparamval);
					}
				}
				else if (ebanking == "m-BCA")
				{
					if (ebankingparam == "Card No")
					{
                        paramList.RequestTransType = "SearchCustByATM";
						paramList.Parameter.Add("atmNo", ebankingparamval);
					}
                    else if (ebankingparam == "Handphone No")
					{
                        paramList.RequestTransType = "SearchCustByMB";
						paramList.Parameter.Add("mobileNo", ebankingparamval);
					}
				}
				else if (ebanking == "BCA by Phone")
				{
					if (ebankingparam == "Customer ID")
					{
                        paramList.RequestTransType = "SearchCustByPBATM";
						paramList.Parameter.Add("atmNo", ebankingparamval);
					}
					else if (ebankingparam == "Account No")
					{
                        paramList.RequestTransType = "SearchCustByPBAcct";
						paramList.Parameter.Add("acctNo", ebankingparamval);
					}
                    else if (ebankingparam == "S/N Key BCA")
					{
                        paramList.RequestTransType = "SearchCustByToken";
						paramList.Parameter.Add("snToken", ebankingparamval);
					}
				}
				else if (ebanking == "SMS BCA")
				{
					if (ebankingparam == "Card No")
					{
                        paramList.RequestTransType = "SearchCustByATM";
						paramList.Parameter.Add("atmNo", ebankingparamval);
					}
                    else if (ebankingparam == "Handphone No")
					{
                        paramList.RequestTransType = "SearchCustBySMS";
						paramList.Parameter.Add("mobileNo", ebankingparamval);
					}
				}
                else if (ebanking == "SMS Top Up")
				{
					if (ebankingparam == "Card No")
					{
                        paramList.RequestTransType = "SearchCustByATM";
						paramList.Parameter.Add("atmNo", ebankingparamval);
					}
                    else if (ebankingparam == "Handphone No")
					{
                        paramList.RequestTransType = "SearchCustByTOPUP";
						paramList.Parameter.Add("mobileNo", ebankingparamval);
					}
				}
			}
			else if (criteria == "product")
			{
				productparamval = String.IsNullOrEmpty(productparamval) ? null : productparamval;
				productparamval = Server.HtmlEncode(productparamval);

				if (product == "Credit Card")
				{
					if (productparam == "Card No")
					{
                        paramList.RequestTransType = "SearchCustByCC";
						paramList.Parameter.Add("cardNo", productparamval);
					}
                    else if (productparam == "Customer No")
					{
                        paramList.RequestTransType = "SearchCustInfoByCustCC";
						paramList.Parameter.Add("custCC", productparamval);
					}
				}
				else if (product == "Deposit")
				{
					if (productparam == "Account No")
					{
                        paramList.RequestTransType = "SearchCustByAcctNo";
						paramList.Parameter.Add("acctNo", productparamval);
					}
                    else if (productparam == "Card No")
					{
                        paramList.RequestTransType = "SearchCustByATM";
						paramList.Parameter.Add("atmNo", productparamval);
					}
				}
                else if (product == "Loan")
				{
                    if (productparam == "Account No")
                    {
                        paramList.RequestTransType = "SearchCustInfoByLoanAcct";
                        paramList.Parameter.Add("loanAcctNo", productparamval);
                    }
                }
			}
			else if (criteria == "fullname")
			{
				CultureInfo provider = CultureInfo.InvariantCulture;
				fullname = String.IsNullOrEmpty(fullname) ? null : fullname;
				fullname = Server.HtmlEncode(fullname);
				birthdate = String.IsNullOrEmpty(fullname) ? null : birthdate;
				birthdate = Server.HtmlEncode(birthdate);
				DateTime? birthDate = DateTime.ParseExact(birthdate, "dd/MM/yyyy", provider);

                paramList.RequestTransType = "SearchCustByInfoCust";
				paramList.Parameter.Add("custName", fullname);
				paramList.Parameter.Add("dayOfBirth", birthDate.HasValue ? birthDate.Value.ToString("yyyyMMdd") : string.Empty);
				paramList.Parameter.Add("gender", gender);
			}
            else if (criteria == "cisnumber")
			{
				cisnumber = String.IsNullOrEmpty(cisnumber) ? null : cisnumber;
				cisnumber = Server.HtmlEncode(cisnumber);

                paramList.RequestTransType = "SearchCustInfoByCIS";
				paramList.Parameter.Add("cisNo", cisnumber);
			}

            return (paramList);
        }

        private Params BuildParams(string AccountNo = null, string AtmCardNo = null, string CreditCardNo = null, string CreditCardCustomerNo = null)
        {
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            paramList.WSDL = "CustomerInformationInquiry";

            if (!String.IsNullOrEmpty(AccountNo))
            {
                paramList.RequestTransType = "SearchCustByAcctNo";
                paramList.Parameter.Add("acctNo", AccountNo);
            }
            else if (!String.IsNullOrEmpty(AtmCardNo))
            {
                paramList.RequestTransType = "SearchCustByATM";
                paramList.Parameter.Add("atmNo", AtmCardNo);
            }
            else if (!String.IsNullOrEmpty(CreditCardNo))
            {
                paramList.RequestTransType = "SearchCustByCC";
                paramList.Parameter.Add("cardNo", CreditCardNo);
            }
            else if (!String.IsNullOrEmpty(CreditCardCustomerNo))
            {
                paramList.RequestTransType = "SearchCustInfoByCustCC";
                paramList.Parameter.Add("custCC", CreditCardCustomerNo);
            }

            return (paramList);
        }

        private IQueryable<Customer> GetCustomerinCRM(Params paramList)
        {
            CRMDb db = new CRMDb();

            var query = db.customer.Select(x => x);
            query = query.Where(x => x.DeletionStateCode == 0);
            query = query.Where(x => x.StateCode == 0);

            string request = paramList.RequestTransType;

            if (request == "SearchCustByInfoCust")
            {
                string fullname = paramList.Parameter["custName"];
                query = query.Where(x => x.FullName.Contains(fullname));
                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime birthDate = DateTime.ParseExact(paramList.Parameter["dayOfBirth"], "yyyyMMdd", provider);
                query = query.Where(x => x.BirthDate == birthDate);
                int gendercode = 0;
                switch (paramList.Parameter["gender"])
                {
                    case "M": { gendercode = 1; break; }
                    case "F": { gendercode = 2; break; }
                    case "O": { gendercode = 200000; break; }
                }
                query = query.Where(x => x.GenderCode == gendercode);
            }
            else if (request == "SearchCustInfoByCIS")
            {
                string cisnumber = paramList.Parameter["cisNo"];
                query = query.Where(x => x.CISNumber == cisnumber);
            }
            else
            {
                query = query.Where(x => x.CISNumber == String.Empty);
            }

            return (query);
        }

        /* For Customer Segementation
         * For Temporary, Next Store Customer Segmentation & Membership in Database 
         */
        private string Membership = string.Empty;
        private string Segmentation = string.Empty;
        private void GetSegmentation(string cisno)
        {
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "SearchCustInfoByCIS";
            param.WSDL = "CustomerInformationInquiry";
            param.Parameter.Add("cisNo", cisno);
            ESBData data = EAI.RetrieveESBData(param);

            string customersegmentation = string.Empty;
            if (data.Result != null)
            {
                foreach (StringDictionary entry in data.Result)
                {
                    customersegmentation = string.IsNullOrEmpty(entry["customersegmentation"]) ? customersegmentation : entry["customersegmentation"];
                }
            }

            if (!string.IsNullOrEmpty(customersegmentation))
            {
                this.Membership = Utility.GetCustomerSegmentation(customersegmentation, "Membership");
                this.Segmentation = Utility.GetCustomerSegmentation(customersegmentation, "Segmentation");
            }

        }

        [DonutOutputCache(Duration = 60, VaryByParam = "id", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Edit(Guid id)
        {
            Customer model = db.customer.Find(id);

            StringMapController dropDown = new StringMapController();

            ViewBag.displayStatus = "disabled";

            // For hide save and new In Form
            ViewBag.hideSaveAndOpen = true;

            // For Dropdown In form
            // ViewBag.SL_Category = dropDown.getDropDown("Contact", "customertypecode", model.CustomerTypeCode == null ? 1 : (int)model.CustomerTypeCode);
            ViewBag.SL_Gender = dropDown.getDropDown("Contact", "gendercode", model.GenderCode == null ? 0 : (int)model.GenderCode);
            ViewBag.SL_AddressType = dropDown.getDropDown("Contact", "address1_addresstypecode", model.AddressTypeCode == null ? 0 : (int)model.AddressTypeCode);
            
            // For Customer Segementation
            //GetSegmentation(model.CISNumber);
            //ViewBag.Membership = this.Membership;
            //ViewBag.Segmentation = this.Segmentation;
            return View(model);
        }

        [DonutOutputCache(Duration = 60, VaryByParam = "id", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult MoreAddress(Guid id)
        {
            Customer model = db.customer.Find(id);
            return View(model);
        }

        public ActionResult CallWrapUpCustomer(Guid id)
        {
            Customer model = db.customer.Find(id);
            return View(model);
        }

        [OutputCache(Duration = 60, VaryByParam = "id", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult SingleView(Guid id)
        {
            bool hasAccess = RoleNPrivilege.HasAccess(new Guid(Session["RoleID"].ToString()), "Customer");
            int gridMaxRows = Convert.ToInt32(ConfigurationManager.AppSettings["GridMaxRows"].ToString());
            ViewBag.GridMaxRows = gridMaxRows;

            if (hasAccess)
            {
                Customer model = db.customer.Find(id);
                List<CreditCard> creditcardList = new List<CreditCard>();
                List<Deposit> depositList = new List<Deposit>();
                List<Loan> loanList = new List<Loan>();
                List<Channel> channelList = new List<Channel>();

                List<KeyValuePair<int, string>> results = new List<KeyValuePair<int, string>>();
                Params paramProductList = new Params() { Parameter = new Dictionary<string, string>() };
                Params paramChannelList = new Params() { Parameter = new Dictionary<string, string>() };

                DBServices dbservices = new DBServices();

                paramProductList.WSDL = "CustomerProductListInquiry";
                if (string.IsNullOrEmpty(model.CISNumber)
                    || model.CISNumber.StartsWith("C", true, CultureInfo.InvariantCulture)
                    //|| customer.GenderCode == 200000 // commented for a while
                    )
                {
                    paramProductList.RequestTransType = "SearchAllProductsByCustCC";
                    paramProductList.Parameter.Add("custNo", model.CreditCardCustomerNo);
                }
                else
                {
                    paramProductList.RequestTransType = "SearchAllProductsByCIS";
                    paramProductList.Parameter.Add("cisNo", model.CISNumber);
                }

                ESBData esbProduct = new ESBData() { Result = new List<StringDictionary>() };
                esbProduct = EAI.RetrieveESBData(paramProductList);
                if (esbProduct.Result != null && esbProduct.Result.Count() > 0)
                {
                    dbservices.GetProducts(model, esbProduct.Result, null, out creditcardList, out depositList, out loanList);
                }
                else
                {
                    ViewBag.Message = esbProduct.Message;
                }

                paramChannelList.WSDL = "CustomerChannelListInquiry";
                if (string.IsNullOrEmpty(model.CISNumber) ||
                    model.CISNumber.StartsWith("C", true, CultureInfo.InvariantCulture))
                {
                    paramChannelList.RequestTransType = "SearchAllChannelsByCustCC";
                    paramChannelList.Parameter.Add("cisNo", "");
                    paramChannelList.Parameter.Add("custNo", model.CreditCardCustomerNo);
                }
                else
                {
                    paramChannelList.RequestTransType = "SearchAllChannelsByCIS";
                    paramChannelList.Parameter.Add("cisNo", model.CISNumber);
                    paramChannelList.Parameter.Add("custNo", "");
                }
                ESBData esbChannel = EAI.RetrieveESBData(paramChannelList);
                if (esbChannel.Result != null && esbChannel.Result.Count() > 0)
                {
                    dbservices.GetChannel(model, esbChannel.Result, out channelList);
                }
                else
                {
                    ViewBag.Message = esbChannel.Message;
                }

                // DEPOSIT
                var modelDeposit = (from x in depositList
                                    select new
                                    {
                                        Id = x.Id,
                                        CustomerId = x.CustomerId,
                                        CustomerIdName = x.CustomerIdName,
                                        AccountNo = x.AccountNo,
                                        AccountTypeIdCode = x.AccountTypeIdCode,
                                        AccountTypeIdName = x.AccountTypeIdName,
                                        CardNo = x.CardNo,
                                        CardTypeIdName = x.CardTypeIdName,
                                        OwnerTypeLabel = x.OwnerTypeLabel,
                                        RelationType = x.RelationType,
                                        RelationshipWith = x.RelationshipWith
                                    });

                // CREDIT CARD
                var modelCreditCard = (from x in creditcardList
                                       group x by x.Id into y
                                       select new
                                       {
                                           Id = y.Key,
                                           CreditCardNumber = y.Select(z => z.CreditCardNumber).FirstOrDefault(),
                                           CardholderName = y.Select(z => z.CardholderName).FirstOrDefault(),
                                           CardType = y.Select(z => z.CardType).FirstOrDefault(),
                                           CCType = y.Select(z => z.CCType).FirstOrDefault(),
                                           CreditCardCustomerNo = y.Select(z => z.CreditCardCustomerNo).FirstOrDefault(),
                                           CustomerId = y.Select(z => z.CustomerId).FirstOrDefault(),
                                           CustomerIdName = y.Select(z => z.CustomerIdName).FirstOrDefault(),
                                           Status = y.Select(z => z.Status).FirstOrDefault()
                                       });

                // LOAN
                var modelLoan = loanList
                                .OrderBy(x => x.AccountNo)
                                .ThenBy(x => x.LoanTypeIdCode)
                                .ThenBy(x => x.LoanNumber);

                // CHANNEL
                var modelChannel = channelList
                                .OrderBy(x => x.ChannelType)
                                .ThenBy(x => x.Id);



                ViewBag.DepositModel = modelDeposit;
                ViewBag.DepositModelCount = modelDeposit.Count();
                if (ViewBag.DepositModelCount > gridMaxRows)
                {
                    ViewBag.DepositModel = modelDeposit.Take(gridMaxRows);
                }

                ViewBag.CreditCardModel = modelCreditCard;
                ViewBag.CreditCardModelCount = modelCreditCard.Count();
                if (ViewBag.CreditCardModelCount > gridMaxRows)
                {
                    ViewBag.CreditCardModel = modelCreditCard.Take(gridMaxRows);
                }

                ViewBag.LoanModel = modelLoan;
                ViewBag.LoanModelCount = modelLoan.Count();
                if (ViewBag.LoanModelCount > gridMaxRows)
                {
                    ViewBag.LoanModel = modelLoan.Take(gridMaxRows);
                }

                ViewBag.ChannelModel = modelChannel;
                ViewBag.ChannelModelCount = modelChannel.Count();

                ViewBag.CustomerId = model.CustomerID;
                ViewBag.CustomerIdName = model.FullName;
                ViewBag.CISNumber = model.CISNumber;
                ViewBag.hasAccess = 1;
            }
            else {
                ViewBag.hasAccess = 0;
            }
            return View();
        }

        public JsonResult GetAllCreditCardStatus(string _strCreditCards = null)
        {
            List<CreditCard> _creditCards = string.IsNullOrEmpty(_strCreditCards) ? new List<CreditCard>() : new JavaScriptSerializer().Deserialize<List<CreditCard>>(_strCreditCards);

            Params paramListCCbyCardNoCardType = new Params() { Parameter = new Dictionary<string, string>() };
            paramListCCbyCardNoCardType.WSDL = "CCInformationDetail";
            paramListCCbyCardNoCardType.RequestTransType = "GetCreditCardDetailByCardNoAndCardType";

            List<CreditCard> modelCreditCard = new List<CreditCard>();
            foreach (var record in _creditCards)
            {
                paramListCCbyCardNoCardType.Parameter.Remove("cardNo");
                paramListCCbyCardNoCardType.Parameter.Remove("cardType");
                paramListCCbyCardNoCardType.Parameter.Add("cardNo", record.CreditCardNumber);
                paramListCCbyCardNoCardType.Parameter.Add("cardType", record.CCType);
                ESBData searchResultCCbyCardNoCardType = EAI.RetrieveESBData(paramListCCbyCardNoCardType);

                if (searchResultCCbyCardNoCardType.Result != null && searchResultCCbyCardNoCardType.Result.Count != 0)
                {
                    record.Status = Utility.GetStringMap(13, 1, searchResultCCbyCardNoCardType.Result[0][CreditCardInquiryStatusResultKeyName.BlockCode]);
                }
                else
                {
                    record.Status = "";
                }
                modelCreditCard.Add(record);
            }
            return Json(modelCreditCard, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CustomerSearch()
        {
            Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
            paramList = BuildParams(Request["AccountNo"], Request["AtmCardNo"], Request["CreditCardNo"], Request["CreditCardCustomerNo"]);

            if (paramList.IsValid())
            {
                var data = GetCustomerList(paramList);
                if (data.Count() > 0)
                {
                    Guid id = data.FirstOrDefault().CustomerID;
                    if (Request["isPopup"] == "on")
                    {
                        Session.Add("customerSearchGuid", id);
                    }
                    return RedirectToAction("Edit", "Customer", new { id = id });
                }
                else
                {
                    return RedirectToAction("CustomerNotFound", "Error");
                }
            }
            return null;
        }

        public ActionResult RequestList()
        {
            ViewBag.CustomerId = new Guid(Request["CustomerId"]);
            ViewBag.CustomerIdName = Request["CustomerIdName"];

            return View();
        }

        public JsonResult GetCreditCardData(string _cisno = null, string _cardno = null)
        {
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.Parameter.Add("cisNo", _cisno);
            param.Parameter.Add("acctNo", _cardno);
            param.Parameter.Add("applicationType", "CreditCard");
            CreditCard creditcard = ProductInquiry.SpecificCreditCard(param);
            return Json(creditcard, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepositData(string _cisno = null, string _accountno = null)
        {
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.Parameter.Add("cisNo", _cisno);
            param.Parameter.Add("acctNo", _accountno);
            param.Parameter.Add("applicationType", "Deposit");
            Deposit deposit = ProductInquiry.SpecificDeposit(param);
            return Json(deposit, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoanData(string _accountno = null, string _loannumber = null)
        {
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetLoanInfoDetail";
            param.Parameter.Add("acctNo", _accountno);
            param.Parameter.Add("noteNo", _loannumber);
            LoanInfo loan = ProductInquiry.Loan(param);
            return Json(loan, JsonRequestBehavior.AllowGet);
        }
    }
}
