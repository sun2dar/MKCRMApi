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
        public static CreditCardHistoricalTransaction HistoryTransactions(Params request)
        {
            CreditCardHistoricalTransaction transaction = new CreditCardHistoricalTransaction();
            string StatementDate = request.Parameter["statementDate"];
            try
            {
                if (request != null)
                {
                    AllRelatedCreditCardInformation allRelatedCreditCardInformation = null;

                    if (request.Parameter.ContainsKey("custNo"))
                    {
                        transaction.Information.CustomerNo = request.Parameter["custNo"];
                    }
                    allRelatedCreditCardInformation = CreditCardService.RetrieveAllRelatedCreditCards(request);

                    foreach (CreditCardInformation entry in allRelatedCreditCardInformation.RelatedCreditCards)
                    {
                        CreditCardInformation information = CreditCardService.RetrieveCreditCardDetail(entry.CreditCardNo, entry.CardType);
                        transaction.Information.CustomerNo = information.CustomerNo;
                        transaction.CreditCards.Add(information);
                        CreditCardHistoricalTransaction historyTrx = RetrieveCreditCardHistoricalTransactions(entry.CreditCardNo, StatementDate);
                        foreach (CreditCardTransaction trx in historyTrx.Transactions)
                        {
                            transaction.Transactions.Add(trx);
                        }
                    }

                    transaction.Information.OldBalance = (
                                            from creditCardTransaction in transaction.Transactions
                                            select creditCardTransaction.PreviousBalance).Sum();

                    transaction.Information.Credit = (
                                        from creditCardTransaction in transaction.Transactions
                                        where HistoricalTransactionCodesForNegativeAmount.Contains(creditCardTransaction.TransactionCode)
                                        select creditCardTransaction.Amount).Sum();

                    transaction.Information.AccountDate = Formatter.ParseExact(StatementDate, "JY");
                    if (transaction.Information.AccountDate.HasValue)
                    {
                        transaction.Information.MaturityDate = transaction.Information.AccountDate.Value.AddDays(14);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return transaction;
        }

        private static CreditCardHistoricalTransaction RetrieveCreditCardHistoricalTransactions(string creditCardNo, string statementDate)
        {
            CreditCardHistoricalTransaction transaction = new CreditCardHistoricalTransaction();
            try
            {
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                param.RequestTransType = "GetHistoryCreditCardTransactionOnStateDate";
                param.Parameter.Add("cardNo", creditCardNo);
                param.Parameter.Add("stateDate", statementDate);
                param.WSDL = "CCBillingStatementByStatementDate";
                ESBData data = EAI.RetrieveESBData(param);

                if (data.Result.Count > 1)
                {
                    int counter = 0;
                    if (data.Result.Count > 0)
                    {
                        // The first record contains the keys "nbrDetail","length","appCode","card"
                        int.TryParse(data.Result[0]["nbrDetail"], out counter);
                    }

                    for (int i = 1; i <= counter; i++)
                    {
                        StringDictionary eachTransaction = data.Result[i];

                        //if ("INTEREST".Equals(eachTransaction["desc"]) && Formatter.GetParsedDouble(eachTransaction["amount"], false) == 0.00)
                        //    continue;

                        transaction.Transactions.Add(new CreditCardTransaction()
                        {
                            CreditCardNo = creditCardNo,
                            TransactionDate = Formatter.ParseExact(eachTransaction["postDate"], "JY"),
                            Description = eachTransaction["desc"],
                            TransactionCode = eachTransaction["txnCode"],
                            ReferenceNumber = string.IsNullOrEmpty(eachTransaction["refNmbr"]) ? string.Empty : eachTransaction["refNmbr"].PadLeft(11, '0'),
                            Amount = Formatter.GetParsedDouble(eachTransaction["amount"], false),
                            PreviousBalance = Formatter.GetParsedDouble(eachTransaction["prevBal"], false),
                        });
                    }
                }
            }
            catch (Exception e)
            {
            }
            return transaction;
        }

        public static string[] HistoricalTransactionCodesForNegativeAmount
        {
            get
            {
                return new string[] { "9", "11", "13", "15", "17", "19", "20", "21", "22", "23", "31", "33", "39", "41", "43", "49", "50", "61", "66", "67", "69", "91" };
            }
        }
    }
}