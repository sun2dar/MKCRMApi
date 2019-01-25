using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using CCARE.Models;
using CCARE.Models.General;
using System.Collections.Specialized;
using System.Globalization;
using System.Diagnostics;

using BCA.CRM.OSB;
using BCA.CRM.OSB.Messaging;
using BCA.CRM.OSB.Model;
using BCA.CRM.OSB.ServiceAgent;
using BCA.CRM.OSB.Trace;

namespace CCARE.App_Function
{
    public class DBServices
    {
        public void GetProducts(Customer customer, List<StringDictionary> esbProductList, string product, out List<CreditCard> creditcardList, out List<Deposit> depositList, out List<Loan> loanList)
        {
            CRMDb db = new CRMDb();
            creditcardList = new List<CreditCard>();
            depositList = new List<Deposit>();
            loanList = new List<Loan>();

            if (esbProductList.Count() != 0)
            {
                foreach (StringDictionary esbProduct in esbProductList)
                {
                    if (esbProduct["type"] == "Credit" || product == "Credit")
                    {
                        string ccType = esbProduct[String.IsNullOrEmpty(product) ? "cctype" : "cardtype"];
                        ccType = string.IsNullOrEmpty(ccType) ? null : ccType;
                        int maxValidCCType = Convert.ToInt32(ConfigurationManager.AppSettings["MaxValidCCType"].ToString());
                        if (!(string.IsNullOrEmpty(esbProduct["cardNo"]) && string.IsNullOrEmpty(esbProduct["custNo"]))
                            && (Convert.ToInt32(ccType) <= maxValidCCType))
                        {
                            CreditCard creditcard = new CreditCard();

                            creditcard.Id = Guid.NewGuid();
                            creditcard.CustomerId = customer.CustomerID;
                            creditcard.CustomerIdName = customer.FullName;
                            creditcard.CreditCardNumber = esbProduct["cardNo"];
                            creditcard.CardholderName = String.IsNullOrEmpty(product) ? esbProduct["shortName"] : esbProduct["name"];
                            creditcard.CardType = esbProduct[String.IsNullOrEmpty(product) ? "ccDesc" : "cardTypeDesc"];
                            creditcard.CCType = ccType;
                            creditcard.CreditCardCustomerNo = String.IsNullOrEmpty(product) ? esbProduct["custNo"] : String.Empty;

                            creditcardList.Add(creditcard);
                        }
                    }
                    else if (esbProduct["type"] == "Deposit" || product == "Deposit")
                    {
                        if (!(string.IsNullOrEmpty(esbProduct["acctNo"])))
                        {
                            Deposit deposit = new Deposit();

                            deposit.Id = Guid.NewGuid();
                            deposit.CustomerId = customer.CustomerID;
                            deposit.CustomerIdName = customer.FullName;
                            deposit.AccountNo = esbProduct["acctNo"];
                            deposit.AccountTypeIdCode = esbProduct["acctType"];
                            deposit.AccountTypeIdName = App_Function.Utility.GetAccountType(deposit.AccountTypeIdCode);
                            deposit.CardNo = esbProduct[String.IsNullOrEmpty(product) ? "cardNumber" : "atmNo"];
                            deposit.CardTypeIdName = deposit.CardNo;
                            deposit.CardTypeIdName = App_Function.Utility.GetCardTypeByCardNo(deposit.CardNo);
                            deposit.OwnerTypeLabel = esbProduct[String.IsNullOrEmpty(product) ? "ownerDesc" : "ownerType"];
                            deposit.RelationType = esbProduct[String.IsNullOrEmpty(product) ? "relDesc" : "relationType"];
                            deposit.RelationshipWith = esbProduct.ContainsKey("custName") ? esbProduct["custName"] : string.Empty;

                            depositList.Add(deposit);
                        }
                    }
                    else if (esbProduct["type"] == "Loan" || product == "Loan")
                    {
                        if (!(string.IsNullOrEmpty(esbProduct["noteNo"]) && string.IsNullOrEmpty(esbProduct["acctNo"])))
                        {
                            Loan loan = new Loan();

                            loan.Id = Guid.NewGuid();
                            loan.CustomerId = customer.CustomerID;
                            loan.CustomerIdName = customer.FullName;
                            loan.AccountNo = esbProduct["acctNo"];
                            loan.LoanNumber = esbProduct["noteNo"];
                            loan.LoanTypeIdCode = esbProduct["noteType"];
                            loan.LoanTypeIdName = Utility.GetLoanType(esbProduct["noteType"]);

                            loanList.Add(loan);
                        }
                    }
                }
            }
        }

        public void GetChannel(Customer customer, List<StringDictionary> esbChannelList, out List<Channel> channelList)
        {
            CRMDb db = new CRMDb();
            channelList = new List<Channel>();

            if (esbChannelList.Count() != 0)
            {
                foreach (StringDictionary esbChannel in esbChannelList)
                {
                    Channel channel = new Channel();

                    channel.Id = Guid.NewGuid();
                    channel.CustomerId = customer.CustomerID;
                    channel.CustomerIdName = customer.FullName;

                    if (string.Compare("IB", esbChannel["type"], true, CultureInfo.CurrentCulture) == 0 ||
                    string.Compare("KBI", esbChannel["type"], true, CultureInfo.CurrentCulture) == 0)
                    {
                        channel.ChannelTypeId = 901;
                        channel.ChannelType = Utility.GetChannelType(901);
                        channel.CardNo = esbChannel["cardNo"];
                        channel.UserId = esbChannel["UserId"];
                        channel.EmailAddress = esbChannel["email"];
                        channel.HpNo = "";
                        channel.CorpId = "";
                        channel.AccountNo = "";
                        channel.CustomerNo = "";
                        channel.SNKeyBCA = esbChannel["snNumber"];
                        channel.Category = null;
                    }
                    else if (string.Compare("MB", esbChannel["type"], true, CultureInfo.CurrentCulture) == 0)
                    {
                        channel.ChannelTypeId = 902;
                        channel.ChannelType = Utility.GetChannelType(902);
                        channel.CardNo = esbChannel["cardNo"];
                        channel.UserId = "";
                        channel.EmailAddress = "";
                        channel.HpNo = esbChannel["hpNo"];
                        channel.CorpId = "";
                        channel.AccountNo = "";
                        channel.CustomerNo = "";
                        channel.SNKeyBCA = "";
                        channel.Category = null;
                    }
                    else if (string.Compare("TopUp", esbChannel["type"], true, CultureInfo.CurrentCulture) == 0)
                    {
                        channel.ChannelTypeId = 903;
                        channel.ChannelType = Utility.GetChannelType(903);
                        channel.CardNo = esbChannel["cardNo"];
                        channel.UserId = "";
                        channel.EmailAddress = "";
                        channel.HpNo = esbChannel["hpNo"];
                        channel.CorpId = "";
                        channel.AccountNo = "";
                        channel.CustomerNo = "";
                        channel.SNKeyBCA = "";
                        channel.Category = null;
                    }
                    else if (string.Compare("PB", esbChannel["type"], true, CultureInfo.CurrentCulture) == 0)
                    {
                        channel.ChannelTypeId = 904;
                        channel.ChannelType = Utility.GetChannelType(904);
                        channel.CardNo = esbChannel["cardNumber"];
                        channel.UserId = "";
                        channel.EmailAddress = "";
                        channel.HpNo = "";
                        channel.CorpId = "";
                        channel.AccountNo = esbChannel["accountNumber"];
                        channel.CustomerNo = esbChannel["customerId"];
                        channel.SNKeyBCA = esbChannel["snToken"];
                        if (string.Compare(esbChannel["category"], "B", true, CultureInfo.CurrentCulture) == 0)
                        {
                            channel.Category = Utility.GetCategory(1); // Business
                        }
                        else if (string.Compare(esbChannel["category"], "P", true, CultureInfo.CurrentCulture) == 0)
                        {
                            channel.Category = Utility.GetCategory(2); // Prioritas
                        }
                        else if (string.Compare(esbChannel["category"], "C", true, CultureInfo.CurrentCulture) == 0)
                        {
                            channel.Category = Utility.GetCategory(3); // Consumer
                        }
                        else
                        {
                            channel.Category = String.Empty;
                        }
                    }
                    else if (string.Compare("KBB", esbChannel["type"], true, CultureInfo.CurrentCulture) == 0)
                    {
                        channel.ChannelTypeId = 905;
                        channel.ChannelType = Utility.GetChannelType(905);
                        channel.CardNo = "";
                        channel.UserId = "";
                        channel.EmailAddress = esbChannel["email"];
                        channel.HpNo = "";
                        channel.CorpId = esbChannel["corpCd"];
                        channel.AccountNo = "";
                        channel.CustomerNo = "";
                        channel.SNKeyBCA = "";
                        channel.Category = null;
                    }
                    else if (string.Compare("SMS", esbChannel["type"], true, CultureInfo.CurrentCulture) == 0)
                    {
                        channel.ChannelTypeId = 906;
                        channel.ChannelType = Utility.GetChannelType(906);
                        channel.CardNo = esbChannel["cardNo"];
                        channel.UserId = "";
                        channel.EmailAddress = "";
                        channel.HpNo = esbChannel["hpNo"];
                        channel.CorpId = "";
                        channel.AccountNo = "";
                        channel.CustomerNo = "";
                        channel.SNKeyBCA = "";
                        channel.Category = null;
                    }

                    channelList.Add(channel);
                }
            }
        }

        public List<Customer> InsertUpdateCustomer(List<StringDictionary> esbCustList, string ccCardNo)
        {
            Guid user = new Guid(System.Web.HttpContext.Current.Session["CurrentUserID"].ToString());
            Guid businessunit = new Guid(System.Web.HttpContext.Current.Session["BusinessUnitID"].ToString());

            KeyValuePair<int, string> result = new KeyValuePair<int, string>();
            List<KeyValuePair<int, string>> results = new List<KeyValuePair<int, string>>();

            CRMDb db = new CRMDb();

            CultureInfo provider = CultureInfo.InvariantCulture;

            CustomerInfo customerInfo = new CustomerInfo();
            List<Customer> customerList = new List<Customer>();

            if (esbCustList != null)
            {
                foreach (StringDictionary esbCust in esbCustList)
                {
                    customerInfo = GetCustomerDatainCRM(esbCust["custno"], esbCust["custNoCC"], ccCardNo);
                    var custsegment = esbCust["customersegmentation"];

                    if (customerInfo.StatusUpdate == -1)
                    {
                        // do nothing
                    }
                    else
                    {
                        if (customerInfo.StatusUpdate == 0)
                        {
                            customerInfo.Customer = new Customer();
                        }

                        customerInfo.Customer.CustomerTypeCode =
                            custsegment.Contains("PR") ? 2 :
                            custsegment.Contains("SL") ? 3 :
                            custsegment.Contains("BB") ? 4 : 1;

                        if (string.IsNullOrEmpty(esbCust["custno"]) || string.Compare(esbCust["custno"], "00000000000", StringComparison.Ordinal) == 0)
                        {
                            if (customerInfo.Customer != null && (customerInfo.Customer.CISNumber == null || string.Compare(customerInfo.Customer.CISNumber, "00000000000", StringComparison.Ordinal) == 0))
                            {
                                string CISNumber = (db.Customer_GetCISNumber().ToString());
                                customerInfo.Customer.CISNumber = string.Format(CultureInfo.InvariantCulture, "C_{0}", CISNumber.PadLeft(11, '0'));
                            }
                            customerInfo.Customer.CreditCardCustomerNo = esbCust["custNoCC"];
                        }
                        else
                        {
                            customerInfo.Customer.CISNumber = esbCust["custno"];
                            customerInfo.Customer.CreditCardCustomerNo = esbCust["custNoCC"];
                        }

                        DateTime? birthDate = new DateTime();
                        if (esbCust["birthDt"] == "" || esbCust["birthDt"] == null)
                        {
                            birthDate = null;
                        }
                        else
                        {
                            birthDate = DateTime.ParseExact(esbCust["birthDt"], "yyyyMMdd", provider);
                        }

                        customerInfo.Customer.BirthDate = birthDate;
                        customerInfo.Customer.ModifiedBy = user;
                        customerInfo.Customer.BusinessUnit = businessunit;

                        customerInfo.Customer.EMailAddress1 = esbCust["email"];
                        customerInfo.Customer.Fax = esbCust["faxNumber"];
                        customerInfo.Customer.FirstName = esbCust["firstname"];
                        customerInfo.Customer.FullName = esbCust["fullname"];

                        string[] corporateCode = { "O" };
                        string[] maleCode = { "M", "L", "0" };
                        string[] femaleCode = { "P", "F", "1" };
                        customerInfo.Customer.GenderCode = 0;
                        if (corporateCode.Contains(esbCust["gender"]))
                        {
                            customerInfo.Customer.GenderCode = 200000;
                        }
                        else if (maleCode.Contains(esbCust["gender"]))
                        {
                            customerInfo.Customer.GenderCode = 1;
                        }
                        else if (femaleCode.Contains(esbCust["gender"]))
                        {
                            customerInfo.Customer.GenderCode = 2;
                        }

                        customerInfo.Customer.LastName = esbCust["fullname"]; 
                        customerInfo.Customer.MobilePhone = esbCust["cellPhone"];
                        customerInfo.Customer.Salutation = esbCust["salutation"];
                        customerInfo.Customer.Telephone1 = esbCust["businessPhone"];
                        customerInfo.Customer.Telephone2 = esbCust["homePhone"];

                        string[] priorityCode = { "9", "09", "009", "10", "010", "11", "011", "14", "014" };
                        string[] solitaireCode = { "88", "088", "89", "089" };
                        if (priorityCode.Contains(esbCust["acctCd"]))
                        {
                            customerInfo.Customer.AccountCode = 1;
                        }
                        else if (solitaireCode.Contains(esbCust["acctCd"]))
                        {
                            customerInfo.Customer.AccountCode = 2;
                        }

                        customerInfo.Customer.AliasName = esbCust["aliasName"];
                        customerInfo.Customer.BirthPlace = esbCust["birthPlace"];
                        customerInfo.Customer.FirstName = esbCust["firstname"];
                        customerInfo.Customer.IdentityNo = esbCust["idNo"];
                        customerInfo.Customer.LastName = esbCust["lastname"];
                        customerInfo.Customer.MothersMaidenName = esbCust["motherMaidenName"];

                        if (customerInfo.StatusUpdate == 0)
                        {
                            customerInfo.Customer.CustomerID = Guid.NewGuid();
                            result = db.Customer_Insert(customerInfo.Customer);
                        }
                        else 
                        {
                            result = db.Customer_Update(customerInfo.Customer);
                        }

                        if (result.Key == 0)
                        {
                            InsertUpdateCustAddress(customerInfo.Customer, esbCust);
                        }

                        customerList.Add(customerInfo.Customer);
                    }
                }
            }
            return (customerList);
        }

        private CustomerInfo GetCustomerDatainCRM(string cisNumber, string ccCustNo, string ccCardNo)
        {
            CustomerInfo customerInfo = new CustomerInfo();

            if (string.IsNullOrEmpty(cisNumber) || string.Compare(cisNumber, "00000000000", StringComparison.Ordinal) == 0)
            {
                customerInfo = RetrieveCustomerByCustomerNumber(ccCustNo);
                if (customerInfo.StatusUpdate == 0)
                {
                    if (!string.IsNullOrEmpty(ccCardNo) && !HasCreditCard(ccCardNo)) {
                        customerInfo.StatusUpdate = -1;
                    }
                    
                };

                if (customerInfo.Customer != null && !customerInfo.Customer.CISNumber.StartsWith("C_"))
                {
                    customerInfo.Customer.CreditCardCustomerNo = string.Empty;
                    customerInfo.Customer = null;
                }
            }
            else
            {
                customerInfo = RetrieveCustomerByCisNumber(cisNumber);
            }
            return (customerInfo);
        }

        private CustomerInfo RetrieveCustomerByCisNumber(string cisNumber)
        {
            CRMDb db = new CRMDb();

            CustomerInfo customerInfo = new CustomerInfo();
            var query = db.customer.Where(x => x.CISNumber == cisNumber);
            if (query.Count() == 1)
            {
                customerInfo.Customer = query.First();
                customerInfo.StatusUpdate = 1;
            }
            else if (query.Count() > 1) 
            {
                customerInfo.Customer = null;
                customerInfo.StatusUpdate = -1;
            }
            else
            {
                customerInfo.Customer = null;
                customerInfo.StatusUpdate = 0;
            }

            return (customerInfo);
        }

        private CustomerInfo RetrieveCustomerByCustomerNumber(string customerNumber)
        {
            CRMDb db = new CRMDb();

            CustomerInfo customerInfo = new CustomerInfo();
            var query = db.customer.Where(x => x.CreditCardCustomerNo == customerNumber);
            if (query.Count() == 1)
            {
                customerInfo.Customer = query.First();
                customerInfo.StatusUpdate = 1;
            }
            else if (query.Count() > 1) 
            {
                customerInfo.Customer = null;
                customerInfo.StatusUpdate = -1;
            }
            else
            {
                customerInfo.Customer = null;
                customerInfo.StatusUpdate = 0;
            }

            return (customerInfo);
        }

        private bool HasCreditCard(string cardNo)
        {
            Params param = new Params() { Parameter = new Dictionary<string, string>() };

            param.Parameter.Add("cardNo", cardNo);

            CreditCardInfo creditCard = ProductInquiry.CreditCard(param);

            if (string.IsNullOrEmpty(creditCard.CreditCardNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private CustomerAddress RetrieveAddress(int addressType, StringDictionary esbCust)
        {
            CustomerAddress address = null;


            switch (addressType)
            {
                case 1: // ContactAddressType = Home
                    address = new CustomerAddress()
                    {
                        AddressTypeCode = addressType,
                        Line1 = esbCust["homeAddrLine1"],
                        Line2 = esbCust["homeAddrLine2"],
                        Line3 = esbCust["homeAddrLine3"],
                        City = esbCust["homeCity"],
                        StateOrProvince = esbCust["homeState"],
                        Country = esbCust["homeCountry"],
                        PostalCode = esbCust["homeZipCode"],
                    };

                    break;

                case 2: // ContactAddressType = Office
                    address = new CustomerAddress()
                    {
                        AddressTypeCode = addressType,
                        Line1 = esbCust["officeAddrLine1"],
                        Line2 = esbCust["officeAddrLine2"],
                        Line3 = esbCust["officeAddrLine3"],
                        City = esbCust["officeCity"],
                        StateOrProvince = esbCust["officeState"],
                        Country = esbCust["officeCountry"],
                        PostalCode = esbCust["officeZipCode"],
                    };

                    break;

                case 200002: // ContactAddressType = Mailing
                    address = new CustomerAddress()
                    {
                        AddressTypeCode = addressType,
                        Line1 = esbCust["mailingAddrLine1"],
                        Line2 = esbCust["mailingAddrLine2"],
                        Line3 = esbCust["mailingAddrLine3"],
                        City = esbCust["mailingCity"],
                        StateOrProvince = esbCust["mailingState"],
                        Country = esbCust["mailingCountry"],
                        PostalCode = esbCust["mailingZipCode"],
                    };

                    break;

                case 200000: // ContactAddressType.Billing
                    address = new CustomerAddress()
                    {
                        AddressTypeCode = addressType,
                        Line1 = esbCust["billingAddrLine1"],
                        Line2 = esbCust["billingAddrLine2"],
                        Line3 = esbCust["billingAddrLine3"],
                        City = esbCust["billingCity"],
                        StateOrProvince = esbCust["billingState"],
                        Country = esbCust["billingCountry"],
                        PostalCode = esbCust["billingZipCode"],
                    };

                    if (esbCust["gender"] == "O") // Corporate
                    {
                        if (esbCust.ContainsKey("custNoCC"))
                        {
                            address.CreditCardCustomerNo = Convert.ToInt64((string)esbCust["custNoCC"]).ToString();
                        }
                    }
                    else
                    {
                        if (esbCust.ContainsKey("custNo"))
                        {
                            address.CreditCardCustomerNo = Convert.ToInt64((string)esbCust["custNo"]).ToString();
                        }
                    }

                    break;

                case 200001: // ContactAddressType = Other
                    /* For Address of type "Other", the "State" and "Country" are never provided by the cardpac credit card management system.
                     * Although the service has "OtherState" and "OtherCountry" properties, we can ignore them in code, as they never have any value.
                     */
                    address = new CustomerAddress()
                    {
                        AddressTypeCode = addressType,
                        Line1 = esbCust["otherAddrLine1"],
                        Line2 = esbCust["otherAddrLine2"],
                        Line3 = esbCust["otherAddrLine3"],
                        City = esbCust["otherCity"],
                        PostalCode = esbCust["otherZipCode"],
                    };

                    if (esbCust["gender"] == "O") // Corporate
                    {
                        if (esbCust.ContainsKey("custNoCC"))
                        {
                            address.CreditCardCustomerNo = Convert.ToInt64((string)esbCust["custNoCC"]).ToString();
                        }
                    }
                    else
                    {
                        if (esbCust.ContainsKey("custNo"))
                        {
                            address.CreditCardCustomerNo = Convert.ToInt64((string)esbCust["custNo"]).ToString();
                        }
                    }

                    break;
            }

            return address;
        }

        private void InsertUpdateCustAddress(Customer customer, StringDictionary esbCust)
        {
            CRMDb db = new CRMDb();

            List<CustomerAddress> addressList = new List<CustomerAddress>();
            List<KeyValuePair<int, string>> results = new List<KeyValuePair<int, string>>();

            /*  [CustomerIndicator] can either be 0, 1 or 2.                    
                0: The customer has Credit Card & Deposit
                          ALL address types will be returned 
                1: The customer has Deposit Only
                          Home and Mailing Address will be returned
                2: The customer has Credit Card Only
                          Billing, Office and Other Address will be returned
            */

            bool isCorporate = (customer.GenderCode == 200000);

            switch (esbCust["customerIndicator"])
            {
                case "0": // The customer has Credit Card and Deposit
                    addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeHome), esbCust)); // ContactAddressType = Home
                    addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeOffice), esbCust)); // ContactAddressType = Office
                    addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeMailing), esbCust)); // ContactAddressType = Mailing
                    if (!isCorporate)
                    {
                        Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
                        paramList.RequestTransType = "GetCreditCardAddressesByCisNo";
                        paramList.Parameter.Add("cisNo", customer.CISNumber);
                        paramList.WSDL = "CCAddressDetail";

                        ESBData addresses = new ESBData() { Result = new List<StringDictionary>() };
                        addresses = EAI.RetrieveESBData(paramList);
                        foreach (StringDictionary address in addresses.Result)
                        {
                            addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeBilling), address)); // ContactAddressType = Billing
                            addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeOther), address)); // ContactAddressType = Other
                        }
                    }
                    else
                    {
                        addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeBilling), esbCust)); // ContactAddressType = Billing
                        addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeOther), esbCust)); // ContactAddressType = Other
                    }
                    break;

                case "1": // The customer has Deposit Card only
                    addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeOffice), esbCust)); // ContactAddressType = Office
                    addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeHome), esbCust)); // ContactAddressType = Home
                    addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeMailing), esbCust)); // ContactAddressType = Mailing
                    break;

                case "2": // The customer has Credit Card only
                    // Change for fixing the customer search defect which has a cis number "00000000000". 
                    if (esbCust["custno"] == "00000000000" || isCorporate)
                    {
                        addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeHome), esbCust)); // ContactAddressType = Home
                        addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeOffice), esbCust)); // ContactAddressType = Office
                        addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeMailing), esbCust)); // ContactAddressType = Mailing
                        addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeBilling), esbCust)); // ContactAddressType = Billing
                        addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeOther), esbCust)); // ContactAddressType = Other
                    }
                    else
                    {
                        Params paramList = new Params() { Parameter = new Dictionary<string, string>() };
                        paramList.RequestTransType = "GetCreditCardAddressesByCisNo";
                        paramList.Parameter.Add("cisNo", customer.CISNumber);
                        paramList.WSDL = "CCAddressDetail";

                        ESBData addresses = EAI.RetrieveESBData(paramList);
                        foreach (StringDictionary address in addresses.Result)
                        {
                            addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeBilling), address)); // ContactAddressType = Billing
                            addressList.Add(RetrieveAddress(int.Parse(Resources.Customer.ContactAddressTypeOther), address)); // ContactAddressType = Other
                        }
                    }

                    break;
            }

            // Push CustAddress to DB
            // ----------------------

            KeyValuePair<int, string> result = new KeyValuePair<int, string>();
            result = db.CustomerAddress_Delete(customer);
            if (result.Key == 0) // deleteCustAddress successful, then insertCustAddresses
            {
                KeyValuePair<int, string> res = new KeyValuePair<int, string>();

                int i = 3;
                int addrType = (customer.CISNumber.StartsWith("C_") ? 200000 : 1);
                bool insert = false;

                foreach (CustomerAddress address in addressList)
                {
                    if (!(address.Line1 == "" && address.Line2 == "" && address.Line3 == "" && address.City == "" && address.Country == "" && address.StateOrProvince == "" && address.PostalCode == ""))
                    {
                        address.CustomerAddressId = Guid.NewGuid();
                        address.ParentId = customer.CustomerID;
                        address.AddressNumber = i;
                        address.CreatedBy = customer.ModifiedBy;
                        address.ModifiedBy = customer.ModifiedBy;
                        address.CreditCardCustomerNo = customer.CreditCardCustomerNo;
                        res = db.CustomerAddress_Insert(address);
                        i += 1;

                        if ((address.AddressTypeCode == addrType) && !insert)
                        {
                            address.CustomerAddressId = Guid.NewGuid();
                            address.AddressNumber = 1;
                            res = db.CustomerAddress_Insert(address);
                            insert = true;
                        }
                    }
                }
                results.Add(res);
            }
        }
    }
}