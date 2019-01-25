using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using CCARE.App_Function;
using BCA.CRM.OSB.Model;

namespace CCARE.Models.Transaction
{
    public partial class CCTransaction
    {
        public static CreditCardOutstandingTransaction OutstandingTransactions(Params request)
        {
            CreditCardOutstandingTransaction transaction = new CreditCardOutstandingTransaction();
            try
            {
                if(request.Parameter.ContainsKey("cardNo"))
                {
                    string cardNo = request.Parameter["cardNo"];
                    CreditCardInformation information = CreditCardService.RetrieveCreditCardDetail(cardNo, CreditCardService.RetrieveCreditCardType(cardNo));
                    transaction.CustomerNo = information.CustomerNo;
                    transaction.CreditCards.Add(information);
                    RetrieveCreditCardOutstandingTransactions(cardNo, transaction);
                }
                else if (request.Parameter.ContainsKey("custNo"))
                {
                    Params param = new Params() { Parameter = new Dictionary<string, string>() };
                    param.RequestTransType = "GetAllRelatedCreditCardInOneCustNo";
                    param.Parameter.Add("custNo", request.Parameter["custNo"]);
                    param.WSDL = "AllRelatedDetail";
                    ESBData cardData = EAI.RetrieveESBData(param);
                    transaction.CustomerNo = request.Parameter["custNo"];

                    if (cardData.Result != null && cardData.Result.Count != 0)
                    {
                        foreach (StringDictionary entry in cardData.Result) {
                            string cardno = entry["cardNo"];
                            if (!string.IsNullOrEmpty(cardno)) {
                                CreditCardInformation information = CreditCardService.RetrieveCreditCardDetail(cardno, entry["cardType"]);
                                transaction.CreditCards.Add(information);
                                RetrieveCreditCardOutstandingTransactions(cardno, transaction);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return transaction;
        }

        private static void RetrieveCreditCardOutstandingTransactions(string creditCardNo, CreditCardOutstandingTransaction transaction)
        {
            try
            {
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                param.RequestTransType = "GetUnsettleCreditCardTransactionByCardNo";
                param.Parameter.Add("cardNo", creditCardNo);
                param.WSDL = "CCInquiryUnsettleTransaction";
                ESBData data = EAI.RetrieveESBData(param);

                if (data.Result.Count > 1)
                {
                    data.Result.RemoveAt(0); // Remove the first record as it contains the keys "cardNo" and "appCode"
                    foreach (StringDictionary entry in data.Result)
                    {
                        transaction.Transactions.Add(new CreditCardTransaction()
                        {
                            CreditCardNo = creditCardNo,
                            EffectiveDate = Formatter.ParseExact(entry[OutstandingCreditCardTransactionResultKeyName.EffectiveDate], "JY"),
                            Description = entry[OutstandingCreditCardTransactionResultKeyName.Description],
                            TransactionType = entry[OutstandingCreditCardTransactionResultKeyName.TransactionType],
                            Source = entry[OutstandingCreditCardTransactionResultKeyName.Source].PadLeft(6, '0'),
                            Days = Formatter.GetParsedNumeric(entry[OutstandingCreditCardTransactionResultKeyName.Days], false),
                            Nominal = Formatter.GetParsedDouble(entry[OutstandingCreditCardTransactionResultKeyName.Nominal], false)
                        });
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}