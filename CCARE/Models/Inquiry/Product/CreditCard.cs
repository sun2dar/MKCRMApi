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
        public static CreditCardInfo CreditCard(Params request)
        {
            CreditCardInfo creditCard = new CreditCardInfo();

            string cardNo = request.Parameter["cardNo"];
            string cardType = null;

            if (string.IsNullOrEmpty(cardNo)) 
            {
                return creditCard;
            }

            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            request.RequestTransType = "GetAllRelatedCreditCardInOneCustNo";
            request.WSDL = "AllRelatedDetail";

            data = EAI.RetrieveESBData(request);
            
            if (data != null && data.Result.Count != 0)
            {
                string creditLimit = string.Empty;

                // All related credit cards are returned as a set of "cardType", "cardNo", "creditLinePerCard"
                // for (int i = 1; i < data.Result.Count; i++)
                for (int i = 0; i < data.Result.Count; i++)
                {
                    if (string.Compare(data.Result[i]["cardNo"], cardNo, StringComparison.InvariantCulture) == 0)
                    {
                        cardType = data.Result[i][CreditCardInquiryStatusResultKeyName.CardType];
                        creditLimit = data.Result[i][CreditCardInquiryStatusResultKeyName.CreditLimit];
                        break;
                    }
                }

                creditCard.CustomerName = data.Result[0][CreditCardInquiryStatusResultKeyName.Name];
                creditCard.CycleDate = data.Result[0][CreditCardInquiryStatusResultKeyName.CycleDate];
                creditCard.AutoDebet = data.Result[0][CreditCardInquiryStatusResultKeyName.Autodebet];

                creditCard.CreditLimit = Utility.GetParsedDouble(creditLimit, false);
                creditCard.CreditLine = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.CreditLine], false);
                creditCard.AvailableCreditLimit = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.AvailableCreditLimit], false);
                creditCard.CashAdvancedLimit = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.CashAdvancedLimit], false);
                creditCard.PermanentCreditLimit = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.PermanentCreditLimit], false);
                creditCard.TemporaryCreditLimit = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.TemporaryCreditLimit], false);
                creditCard.LimitCredit = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.LimitCredit], false);
                creditCard.EffectiveDate = Utility.ParseExact(data.Result[0][CreditCardInquiryStatusResultKeyName.EffectiveDate], "ddMMyyyy");

                double? cashLine = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.CashLine], false);
                double? cashAdvancedLimit = Utility.GetParsedDouble(data.Result[0][CreditCardInquiryStatusResultKeyName.CashAdvancedLimit], false);

                if (cashLine != null && cashAdvancedLimit != null)
                {
                    creditCard.CashAdvanced = cashLine.Value - cashAdvancedLimit.Value;
                }

                //creditCard.BillingStatementSendStatus = Utility.GetStringMap(13, 16, data.Result[0][CreditCardInquiryStatusResultKeyName.StatementStatus]);
                creditCard.BillingStatementSendStatus = Utility.GetStringMap(13, 7, data.Result[0][CreditCardInquiryStatusResultKeyName.StatementStatus]);
                creditCard.TemporaryCreditLimitExpiredDate = Utility.ParseExact(data.Result[0][CreditCardInquiryStatusResultKeyName.TemporaryCreditLimitExpiredDate], "ddMMyyyy");
                creditCard.MaintenanceDate = Utility.ParseExact(data.Result[0][CreditCardInquiryStatusResultKeyName.MaintenanceDate], "ddMMyyyy");
            }


            if (cardType != null)
            {
                Params paramInformation = new Params() { Parameter = new Dictionary<string, string>() };
                paramInformation.RequestTransType = "GetCreditCardDetailByCardNoAndCardType";
                paramInformation.Parameter.Add("cardNo", cardNo);
                paramInformation.Parameter.Add("cardType", cardType);
                paramInformation.WSDL = "CCInformationDetail";

                ESBData information = EAI.RetrieveESBData(paramInformation);

                if (information != null && information.Result.Count != 0)
                {
                    creditCard.CardHolderName = information.Result[0][CreditCardInquiryStatusResultKeyName.CardholderName];
                    creditCard.CardType = information.Result[0]["cardTypeDesc"];
                    creditCard.AutoDebetAccountNo = information.Result[0][CreditCardInquiryStatusResultKeyName.AutodebetAccountNo];
                    creditCard.SecuredAccountNo = information.Result[0][CreditCardInquiryStatusResultKeyName.SecuredAccountNo];
                    creditCard.AreaCard = information.Result[0][CreditCardInquiryStatusResultKeyName.AreaCard];
                    creditCard.CreditCardNumber = information.Result[0][CreditCardInquiryStatusResultKeyName.CreditCardNumber];
                    creditCard.CustomerNo = information.Result[0][CreditCardInquiryStatusResultKeyName.CustomerNo];
                    creditCard.OwnershipFlag = information.Result[0][CreditCardInquiryStatusResultKeyName.OwnershipFlag];
                    creditCard.StatusKey = information.Result[0][CreditCardInquiryStatusResultKeyName.BlockCode];
                    //creditCard.Status = Utility.GetStringMap(13, 4, information.Result[0][CreditCardInquiryStatusResultKeyName.BlockCode]);
                    creditCard.Status = Utility.GetStringMap(13, 1, creditCard.StatusKey);
                    //creditCard.BlockCode = Utility.GetStringMap(13, 12, information.Result[0][CreditCardInquiryStatusResultKeyName.BlockCode]);
                    creditCard.BlockCode = Utility.GetStringMap(13, 6, information.Result[0][CreditCardInquiryStatusResultKeyName.BlockCode]);
                    //creditCard.UserCode = Utility.GetStringMap(13, 14, information.Result[0][CreditCardInquiryStatusResultKeyName.UserCode]);
                    creditCard.UserCode = Utility.GetStringMap(13, 16, information.Result[0][CreditCardInquiryStatusResultKeyName.UserCode]);
                    creditCard.CurrentBalance = Utility.GetParsedDouble(information.Result[0][CreditCardInquiryStatusResultKeyName.CurrentBalance], false);
                    creditCard.PastDue = Utility.GetParsedDouble(information.Result[0][CreditCardInquiryStatusResultKeyName.PastDue], false);
                    creditCard.LastCreditLimit = Utility.GetParsedDouble(information.Result[0][CreditCardInquiryStatusResultKeyName.LastCreditLimit], false);

                    string cardExpiryDate = information.Result[0][CreditCardInquiryStatusResultKeyName.ExpiredDate];
                    if (!string.IsNullOrEmpty(cardExpiryDate))
                    {
                        cardExpiryDate = cardExpiryDate.PadLeft(4, '0');
                        creditCard.ExpiredDate = string.Format("{0}/{1}", cardExpiryDate.Substring(0, 2), cardExpiryDate.Substring(2));
                    }

                    creditCard.AnnualFeeDate = Utility.ParseExact(information.Result[0][CreditCardInquiryStatusResultKeyName.AnnualFeeDate], "ddMMyyyy");
                    creditCard.CreditLimitDate = Utility.ParseExact(information.Result[0][CreditCardInquiryStatusResultKeyName.CreditLimitDate], "ddMMyyyy");
                    creditCard.LastCreditLimitDate = Utility.ParseExact(information.Result[0][CreditCardInquiryStatusResultKeyName.LastCreditLimitDate], "ddMMyyyy");
                    creditCard.PinMaintainDate = Utility.ParseExact(information.Result[0][CreditCardInquiryStatusResultKeyName.PinDateMaintain], "ddMMyyyy");
                    creditCard.OpenDate = Utility.ParseExact(information.Result[0][CreditCardInquiryStatusResultKeyName.OpenDate], "ddMMyyyy");
                    creditCard.CloseDate = Utility.ParseExact(information.Result[0][CreditCardInquiryStatusResultKeyName.CloseDate], "ddMMyyyy");
                }
            }

            Params paramDetails = new Params() { Parameter = new Dictionary<string, string>() };
            paramDetails.RequestTransType = "GetCardDetailByCardNo";
            paramDetails.Parameter.Add("cardNo", cardNo);
            paramDetails.WSDL = "CardManagement";

            ESBData details = EAI.RetrieveESBData(paramDetails);

            if (details != null && details.Result.Count != 0)
            {
                //creditCard.LastTandemStatus = Utility.GetStringMap(13, 13, details.Result[0][CreditCardInquiryStatusResultKeyName.LastTandemStatus]);
                creditCard.LastTandemStatus = Utility.GetStringMap(13, 4, details.Result[0][CreditCardInquiryStatusResultKeyName.LastTandemStatus]);
                creditCard.TandemWrongPinCounter = details.Result[0][CreditCardInquiryStatusResultKeyName.TandemWrongPinCounter];
                creditCard.UpdateByUserGroup = details.Result[0][CreditCardInquiryStatusResultKeyName.UpdateByUserGroup];
                creditCard.UpdateByUserNumber = details.Result[0][CreditCardInquiryStatusResultKeyName.UpdateByUserNumber];
                creditCard.TandemUpdateDate = Utility.ParseExact(details.Result[0][CreditCardInquiryStatusResultKeyName.TandemUpdateDateTime], "yy/MM/dd H:mm").Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
            
            return creditCard;
        }

        public static List<CreditCardAutoPay> AutoPay(Params request)
        {
            List<CreditCardAutoPay> autopay = new List<CreditCardAutoPay>();

            request.WSDL = "CCListAutoPayment";

            ESBData data = EAI.RetrieveESBData(request);

            int count = 0;
            foreach (StringDictionary record in data.Result)
            {
                // Ignoring the Blank Records
                if (string.IsNullOrEmpty(record[CreditCardAutoPayInquiryStatusResultKeyName.CustomerId])
                    && string.IsNullOrEmpty(record[CreditCardAutoPayInquiryStatusResultKeyName.MerchantName]))
                {
                    continue;
                }

                autopay.Add(new CreditCardAutoPay()
                {
                    Number = (++count).ToString(),
                    MerchantName = record[CreditCardAutoPayInquiryStatusResultKeyName.MerchantName],
                    CustomerNumber = record[CreditCardAutoPayInquiryStatusResultKeyName.CustomerId]
                });
            }
            return autopay;
        }

        public static CreditCard SpecificCreditCard(Params request)
        {
            CreditCard creditcard = new CreditCard();

            string cisNo = request.Parameter["cisNo"];
            string cardNo = request.Parameter["acctNo"];

            if (string.IsNullOrEmpty(cisNo) || string.IsNullOrEmpty(cardNo)) 
            {
                return creditcard;
            }

            ESBData data = new ESBData() { Result = new List<StringDictionary>() };
            request.RequestTransType = "SearchSpecificCreditCardByCIS";
            request.WSDL = "CustomerSpecificProductInquiry";

            data = EAI.RetrieveESBData(request);

            if (data != null && data.Result.Count != 0)
            {
                for (int i = 0; i < data.Result.Count; i++)
                {
                    if ((string.Compare(data.Result[i]["cardno"], cardNo, StringComparison.InvariantCulture) == 0)
                     && (Convert.ToInt32(data.Result[i]["cctype"]) <= Convert.ToInt32(ConfigurationManager.AppSettings["MaxValidCCType"].ToString())))
                    {
                        creditcard.CreditCardCustomerNo = data.Result[0]["custno"];
                        creditcard.CreditCardNumber = data.Result[i]["cardno"];
                        creditcard.CardholderName = data.Result[i]["shortname"];
                        creditcard.CardType = data.Result[i]["ccdesc"];
                        creditcard.CCType = data.Result[i]["cctype"];

                        break;
                    }
                }
            }
            return creditcard;
        }
    }
}