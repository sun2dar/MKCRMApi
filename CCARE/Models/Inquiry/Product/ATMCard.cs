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
        public static DepositInfo ATMCard(Params request)
        {
            DepositInfo ATMCard = new DepositInfo();
            request.WSDL = "DepositAccountAndCardDetail";
            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                ATMCard.AccountType = Utility.GetAccountType(data.Result[0][DepositATMCardInquiryStatusResultKeyName.AccountType]);
                ATMCard.ATMCardType = Utility.GetCardTypeByCardNo(data.Result[0][DepositATMCardInquiryStatusResultKeyName.ATMCardNo]);
                ATMCard.Name = data.Result[0][DepositATMCardInquiryStatusResultKeyName.CustomerName];
                ATMCard.AccountNumber = data.Result[0][DepositATMCardInquiryStatusResultKeyName.AccountNo];
                ATMCard.CardNumber = data.Result[0][DepositATMCardInquiryStatusResultKeyName.ATMCardNo];
                ATMCard.RelationType = data.Result[0][DepositATMCardInquiryStatusResultKeyName.RelationType];
                ATMCard.OwnerType = data.Result[0][DepositATMCardInquiryStatusResultKeyName.OwnerType];
                ATMCard.WrongPinCounter = data.Result[0][DepositATMCardInquiryStatusResultKeyName.WongPinCounter];
                ATMCard.UpdateByUserNumber = data.Result[0][DepositATMCardInquiryStatusResultKeyName.UpdateByUserNumber];
                ATMCard.UpdateByUserGroup = data.Result[0][DepositATMCardInquiryStatusResultKeyName.UpdateByUserGroup];
                ATMCard.LastStatusKey = data.Result[0][DepositATMCardInquiryStatusResultKeyName.Status];
                //ATMCard.LastStatus = Utility.GetStringMap(14, 9, ATMCard.LastStatusKey);
                ATMCard.LastStatus = Utility.GetStringMap(14, 1, ATMCard.LastStatusKey);
                ATMCard.OpenDate = data.Result[0][DepositATMCardInquiryStatusResultKeyName.OpenDate];
                ATMCard.UpdateDate = data.Result[0][DepositATMCardInquiryStatusResultKeyName.UpdateDateTime];
                ATMCard.LastTransactionDate = data.Result[0][DepositATMCardInquiryStatusResultKeyName.LastTransactionDate];
            }
            return ATMCard;
        }
    }
}