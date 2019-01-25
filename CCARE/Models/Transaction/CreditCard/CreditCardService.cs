using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using CCARE.App_Function;

namespace CCARE.Models
{
    public class CreditCardService
    {
        public static CreditCardRetrieveMultipleStatementDate RetrieveCreditCardStatementDates(Params request)
        {
            CreditCardRetrieveMultipleStatementDate response = new CreditCardRetrieveMultipleStatementDate();
            try
            {
                if (request.Parameter.ContainsKey("cardNo"))
                {
                    string cardNo = request.Parameter["cardNo"];
                    CreditCardInformation information = RetrieveCreditCardDetail(cardNo, RetrieveCreditCardType(cardNo));
                    if (!string.IsNullOrEmpty(information.CustomerNo)) {
                        request.Parameter.Add("custNo", information.CustomerNo);
                    }
                }

                if (request.Parameter.ContainsKey("custNo"))
                {
                    Params param = new Params() { Parameter = new Dictionary<string, string>() };
                    param.RequestTransType = "GetHistoryCreditCardTransaction";
                    param.Parameter.Add("custNo", request.Parameter["custNo"]);
                    param.WSDL = "CCStatementDateHistory";
                    ESBData data = EAI.RetrieveESBData(param);

                    if (data.Result != null && data.Result.Count != 0)
                    {
                        response.MinimumPayment = Formatter.GetParsedDouble(data.Result[0]["minPayment"], false);
                        response.Name = data.Result[0]["name1"];
                        response.Address1 = data.Result[0]["addr1"];
                        response.Address2 = data.Result[0]["addr2"];
                        response.City = data.Result[0]["city"];
                    }

                    foreach (StringDictionary eachRecord in data.Result)
                    {
                        if (eachRecord.ContainsKey("stateDate"))
                        {
                            response.Statements.Add(new CreditCardStatementInformation()
                            {
                                StatementDateInJulianFormat = eachRecord["stateDate"],
                                StatementDate = Formatter.ParseExact(eachRecord["stateDate"], "JY")
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return response;
        }

        public static AllRelatedCreditCardInformation RetrieveAllRelatedCreditCards(Params request)
        {
            AllRelatedCreditCardInformation information = new AllRelatedCreditCardInformation();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.RequestTransType = "GetAllRelatedCreditCardInOneCustNo";
            if (request.Parameter.ContainsKey("cardNo")) 
            {
                param.Parameter.Add("cardNo", request.Parameter["cardNo"]);
            }
            if (request.Parameter.ContainsKey("custNo"))
            {
                param.Parameter.Add("custNo", request.Parameter["custNo"]);
            }
            param.WSDL = "AllRelatedDetail";
            ESBData data = EAI.RetrieveESBData(param);
            if (data.Result != null && data.Result.Count != 0)
            {
                foreach (StringDictionary entry in data.Result) {
                    if (entry.ContainsKey("cardType") && entry.ContainsKey("cardNo")) {
                        information.RelatedCreditCards.Add(new CreditCardInformation()
                        {
                            CardType = entry["cardType"],
                            CreditCardNo = entry["cardNo"]
                        });
                    }
                }

            }
            return information;
        }

        public static CreditCardInformation RetrieveCreditCardDetail(string CreditCardNo, string CreditCardType)
        {
            CreditCardInformation CCInformation = new CreditCardInformation();

            if (!string.IsNullOrEmpty(CreditCardType) && !string.IsNullOrEmpty(CreditCardNo))
            {
                try
                {
                    Params param = new Params() { Parameter = new Dictionary<string, string>() };
                    param.RequestTransType = "GetCreditCardDetailByCardNoAndCardType";
                    param.Parameter.Add("cardNo", CreditCardNo);
                    param.Parameter.Add("cardType", CreditCardType);
                    param.WSDL = "CCInformationDetail";
                    ESBData data = EAI.RetrieveESBData(param);
                    if (data.Result != null && data.Result.Count == 1)
                    {
                        CCInformation.CreditCardNo = CreditCardNo;
                        CCInformation.CustomerNo = data.Result[0]["custNo"];
                        CCInformation.CardType = CreditCardType;
                        CCInformation.CardholderName = data.Result[0]["cardHolderName"];
                        CCInformation.AvailableCredit = Formatter.GetParsedDouble(data.Result[0]["availCredit"], false);
                    }
                }
                catch (Exception e)
                {
                }
            }
            return CCInformation;
        }

        public static string RetrieveCreditCardType(string CreditCardNo)
        {
            string CardType = string.Empty;
            try
            {
                Params param = new Params() { Parameter = new Dictionary<string, string>() };
                param.RequestTransType = "GetAllRelatedCreditCardInOneCustNo";
                param.Parameter.Add("cardNo", CreditCardNo);
                param.WSDL = "AllRelatedDetail";
                ESBData data = EAI.RetrieveESBData(param);
                if (data.Result != null && data.Result.Count != 0)
                {
                    foreach (StringDictionary entry in data.Result)
                    {
                        if (string.Compare(entry["cardNo"], CreditCardNo, StringComparison.InvariantCulture) == 0)
                        {
                            CardType = entry["cardType"];
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return CardType;
        }
    }
}