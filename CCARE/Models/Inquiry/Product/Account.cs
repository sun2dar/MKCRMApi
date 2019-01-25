using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Globalization;
using CCARE.App_Function;
using BCA.CRM.OSB.Model;   

namespace CCARE.Models
{
    public partial class ProductInquiry
    {
        public static DepositInfo Account(Params request)
        {
            DepositInfo account = new DepositInfo();
            request.WSDL = "DepositAccountAndCardDetail";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                account.AccountType = Utility.GetAccountType(data.Result[0][DepositAccountInquiryStatusResultKeyName.AccountType]);
                account.ATMCardType = Utility.GetCardTypeByCardNo(data.Result[0][DepositAccountInquiryStatusResultKeyName.ATMCardNo]);
                account.Name = data.Result[0][DepositAccountInquiryStatusResultKeyName.CustomerName];
                account.AccountNumber = data.Result[0][DepositAccountInquiryStatusResultKeyName.AccountNo];
                account.CardNumber = data.Result[0][DepositAccountInquiryStatusResultKeyName.ATMCardNo];
                account.RelationType = data.Result[0][DepositAccountInquiryStatusResultKeyName.RelationType];
                account.OwnerType = data.Result[0][DepositAccountInquiryStatusResultKeyName.OwnerType];
                account.BranchHome = data.Result[0][DepositAccountInquiryStatusResultKeyName.Branch];
                account.LastStatusKey = data.Result[0][DepositAccountInquiryStatusResultKeyName.Status];
                //account.LastStatus = Utility.GetStringMap(14, 9, account.LastStatusKey);
                account.LastStatus = Utility.GetStringMap(14, 1, account.LastStatusKey);
                account.OpenDate = data.Result[0][DepositAccountInquiryStatusResultKeyName.OpenDate];
                account.CloseDate = data.Result[0][DepositAccountInquiryStatusResultKeyName.CloseDate];
            }
            return account;
        }

        public static Deposit SpecificDeposit(Params request)
        {
            Deposit deposit = new Deposit();

            string cisNo = request.Parameter["cisNo"];
            string acctNo = request.Parameter["acctNo"];

            if (string.IsNullOrEmpty(cisNo) || string.IsNullOrEmpty(acctNo))
            {
                return deposit;
            }

            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            request.RequestTransType = "SearchSpecificDepositByCIS";
            request.WSDL = "CustomerSpecificProductInquiry";

            data = EAI.RetrieveESBData(request);

            if (data != null && data.Result.Count != 0)
            {
                deposit.AccountNo = data.Result[0]["acctno"];
                deposit.CardNo = data.Result[0]["cardnumber"];
                deposit.OwnerTypeLabel = data.Result[0]["ownerdesc"];
                deposit.RelationType = data.Result[0]["reldesc"];
            }
            return deposit;
        }
    }
}