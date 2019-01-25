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
        public static CreditCardCurrentTransaction CurrentTransactions(Params request)
        {
            CreditCardCurrentTransaction transaction = new CreditCardCurrentTransaction();

            try
            {
                if (request != null)
                {
                    if (request.Parameter.ContainsKey("cardNo"))
                    {
                        string cardNo = request.Parameter["cardNo"];
                        CreditCardInformation information = CreditCardService.RetrieveCreditCardDetail(cardNo, CreditCardService.RetrieveCreditCardType(cardNo));
                        transaction.CustomerNo = information.CustomerNo;
                        transaction.CreditCards.Add(information);
                        CreditCardCurrentTransaction currentTrx = RetrieveCreditCardCurrentTransactions(cardNo);
                        foreach (CreditCardTransaction trx in currentTrx.Transactions)
                        {
                            transaction.Transactions.Add(trx);
                        }
                    }
                    else if (request.Parameter.ContainsKey("custNo"))
                    {
                        // If a customer has multiple Credit Cards, the "Credit Card Customer No" is still the same for all the credit cards.                    
                        Params param = new Params() { Parameter = new Dictionary<string, string>() };
                        param.RequestTransType = "GetAllRelatedCreditCardInOneCustNo";
                        param.Parameter.Add("custNo", request.Parameter["custNo"]);
                        param.WSDL = "AllRelatedDetail";
                        ESBData data = EAI.RetrieveESBData(param);
                        transaction.CustomerNo = request.Parameter["custNo"];

                        if (data.Result != null && data.Result.Count != 0)
                        {
                            // All related credit cards (from the service 'GetAllRelatedCreditCardInOneCustNo') are returned as a set of "cardType", "cardNo", "creditLinePerCard"
                            foreach (StringDictionary entry in data.Result) {
                                CreditCardInformation information = CreditCardService.RetrieveCreditCardDetail(entry["cardNo"], entry["cardType"]);
                                transaction.CreditCards.Add(information);
                                CreditCardCurrentTransaction currentTrx = RetrieveCreditCardCurrentTransactions(entry["cardNo"]);
                                foreach (CreditCardTransaction trx in currentTrx.Transactions)
                                {
                                    transaction.Transactions.Add(trx);
                                }
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

        private static CreditCardCurrentTransaction RetrieveCreditCardCurrentTransactions(string cardNo)
        {
            CreditCardCurrentTransaction transaction = new CreditCardCurrentTransaction();
            try
            {
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                param.RequestTransType = "GetCurrentCreditCardTransactionByCardNo";
                param.Parameter.Add("cardNo", cardNo);
                param.WSDL = "CCInquiryTransactionDetail";
                ESBData data = EAI.RetrieveESBData(param);

                if (data.Result.Count > 1)
                {
                    data.Result.RemoveAt(0); // Remove the first record as it contains the keys "card", "number" and "appCode"

                    foreach (StringDictionary entry in data.Result)
                    {
                        transaction.Transactions.Add(new CreditCardTransaction()
                        {
                            CreditCardNo = cardNo,
                            PostingDate = Formatter.ParseExact(entry[CurrentCreditCardTransactionResultKeyName.PostingDate], "JY"),
                            TransactionDate = Formatter.ParseExact(entry[CurrentCreditCardTransactionResultKeyName.TransactionDate], "JY"),
                            Description = entry[CurrentCreditCardTransactionResultKeyName.Description],
                            TransactionCode = entry[CurrentCreditCardTransactionResultKeyName.TransactionCode],
                            MerchantCode = entry[CurrentCreditCardTransactionResultKeyName.MerchantCode],
                            ReferenceNumber = entry[CurrentCreditCardTransactionResultKeyName.ReferenceNumber],
                            Amount = Formatter.GetParsedDouble(entry[CurrentCreditCardTransactionResultKeyName.Amount], false)
                        });
                    }
                }
            }
            catch (Exception e)
            {
            }
            return transaction;
        }
    }
}