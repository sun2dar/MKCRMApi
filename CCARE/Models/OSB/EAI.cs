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
using BCA.CRM.OSB;
using BCA.CRM.OSB.Messaging;
using BCA.CRM.OSB.Model;
using BCA.CRM.OSB.ServiceAgent;
using BCA.CRM.OSB.Trace;
using BCA.CRM.EAI.REST;
using Newtonsoft.Json;

namespace CCARE.Models
{
    public class EAI
    {
        delegate List<StringDictionary> OptTimeout(string RequestTransType, Dictionary<string, string> Parameters, ServiceAgentResponse response);
        delegate List<StringDictionary> RestTimeout(string RequestTransType, Dictionary<string, string> Parameters);

        public static ESBData RetrieveESBData(Params param)
        {
            ESBData data = new ESBData();
            CustomerSearchResponse response = new CustomerSearchResponse();
			OptTimeout timeout = null;
            RestTimeout restTimeout = null;
            bool isRest = false;

            switch (param.WSDL)
			{
                /*
                 * Integration with EAI
                 * WSDL
                 */
                case "AccountDetail": timeout = new OptTimeout(IntegrationManager.ExecuteAccountDetail); break;
                case "AccountStatementByDate": timeout = new OptTimeout(IntegrationManager.ExecuteAccountStatementByDate); break;
                case "AllRelatedDetail": timeout = new OptTimeout(IntegrationManager.ExecuteAllRelatedDetail); break;
                case "CallStoreProcedure": timeout = new OptTimeout(IntegrationManager.ExecuteCallStoreProcedure); break;
                case "CallStoreProcedureDelimiter": timeout = new OptTimeout(IntegrationManager.ExecuteCallStoreProcedureDelimiter); break;
                case "CardManagement": timeout = new OptTimeout(IntegrationManager.ExecuteCardManagement); break;
                case "CardStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteCardStatusUpdate); break;
                case "CCAddressDetail": timeout = new OptTimeout(IntegrationManager.ExecuteCCAddressDetail); break;
                case "CCBillingStatementByStatementDate": timeout = new OptTimeout(IntegrationManager.ExecuteCCBillingStatementByStatementDate); break;
                case "CCInformationDetail": timeout = new OptTimeout(IntegrationManager.ExecuteCCInformationDetail); break;
                case "CCInquiryTransactionDetail": timeout = new OptTimeout(IntegrationManager.ExecuteCCInquiryTransactionDetail); break;
                case "CCInquiryUnsettleTransaction": timeout = new OptTimeout(IntegrationManager.ExecuteCCInquiryUnsettleTransaction); break;
                case "CCListAutoPayment": timeout = new OptTimeout(IntegrationManager.ExecuteCCListAutoPayment); break;
                case "CCMerchantInformation": timeout = new OptTimeout(IntegrationManager.ExecuteCCMerchantInformation); break;
                case "CCStatementDateHistory": timeout = new OptTimeout(IntegrationManager.ExecuteCCStatementDateHistory); break;
                case "CorporateCardBlock": timeout = new OptTimeout(IntegrationManager.ExecuteCorporateCardBlock); break;
                case "CorporateCardInquiry": timeout = new OptTimeout(IntegrationManager.ExecuteCorporateCardInquiry); break;
                case "CustomerChannelListInquiry": timeout = new OptTimeout(IntegrationManager.ExecuteCustomerChannelListInquiry_V2); break;
                case "CustomerInformationInquiry": timeout = new OptTimeout(IntegrationManager.ExecuteCustomerInformationInquiry); break;
                case "CustomerProductListInquiry": timeout = new OptTimeout(IntegrationManager.ExecuteCustomerProductListInquiry_V2); break;
                case "CustomerSpecificChannelInquiry": timeout = new OptTimeout(IntegrationManager.ExecuteCustomerSpecificChannelListInquiry); break;
                case "CustomerSpecificProductInquiry": timeout = new OptTimeout(IntegrationManager.ExecuteCustomerSpecificProductListInquiry); break;
                case "DepositAccountAndCardDetail": timeout = new OptTimeout(IntegrationManager.ExecuteDepositAccountAndCardDetail); break;
                case "ESBDBDelimiter": timeout = new OptTimeout(IntegrationManager.ExecuteESBDBDelimiter); break;
                case "LoanInfoDetail": timeout = new OptTimeout(IntegrationManager.ExecuteLoanInfoDetail_V2); break;
                case "LoginIBIStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteLoginIBIStatusUpdate); break;
                case "LoginSMEStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteLoginSMEStatusUpdate); break;
                case "MBManagement": timeout = new OptTimeout(IntegrationManager.ExecuteUserMobileBankingManagement); break;
                case "Merchant": timeout = new OptTimeout(IntegrationManager.ExecuteCCMerchantInformation); break;
                case "PhoneBankingTransaction": timeout = new OptTimeout(IntegrationManager.ExecuteRemoteBankingInquiryTransaction); break;
                case "POSManagement": timeout = new OptTimeout(IntegrationManager.ExecutePOSRegistration); break;
                case "TokenAuthentication": timeout = new OptTimeout(IntegrationManager.ExecuteTokenAuthentication); break;
                case "TokenResynch": timeout = new OptTimeout(IntegrationManager.ExecuteTokenResynch); break;
                case "TokenSMEUpdateStatus": timeout = new OptTimeout(IntegrationManager.ExecuteTokenSMEUpdateStatus); break;
                case "TokenUnlock": timeout = new OptTimeout(IntegrationManager.ExecuteTokenUnlock); break;
                case "TokenUpdateStatus": timeout = new OptTimeout(IntegrationManager.ExecuteTokenUpdateStatus); break;
                case "UserIBIStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteUserIBIStatusUpdate); break;
                case "UserInternetBankingManagement": timeout = new OptTimeout(IntegrationManager.ExecuteUserInternetBankingManagement); break;
                case "UserMBStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteUserMBStatusUpdate); break;
                case "UserMobileBankingManagement": timeout = new OptTimeout(IntegrationManager.ExecuteUserMobileBankingManagement); break;
                case "UserPhoneBankingInquiry": timeout = new OptTimeout(IntegrationManager.ExecuteUserPhoneBankingInquiry); break;
                case "UserPhoneBankingManagement": timeout = new OptTimeout(IntegrationManager.ExecuteUserPhoneBankingManagement); break;
                case "UserPhoneBankingStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteUserPhoneBankingStatusUpdate); break;
                case "UserSMSBankingStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteUserSMSBankingStatusUpdate); break;
                case "UserSMSTopUpStatusUpdate": timeout = new OptTimeout(IntegrationManager.ExecuteUserSMSTopUpStatusUpdate); break;
                /*
                 * Integration with EAI
                 * REST SERVICE
                 */
                case "OTPCashOut": restTimeout = new RestTimeout(RestManager.ExecuteGetOTPCashOut); isRest = true; break;

                default: timeout = new OptTimeout(IntegrationManager.ExecuteCRMGeneralFunction); break;
			}
            Stopwatch exectime = Stopwatch.StartNew();
            IAsyncResult results = isRest ? restTimeout.BeginInvoke(param.RequestTransType, param.Parameter, null, null) : timeout.BeginInvoke(param.RequestTransType, param.Parameter, response, null, null);
            try
            {
                if (!results.IsCompleted)
                {
                    results.AsyncWaitHandle.WaitOne(30000, false);
                    if (!results.IsCompleted)
                    {
                        exectime.Stop();
                        data.Message = "Request doesnt complete.";
                    }
                    else
                    {
                        data.Result = isRest ? restTimeout.EndInvoke(results) : timeout.EndInvoke(results);
                        exectime.Stop();
                    }
                }
            }
            catch (Exception e)
            {
                data.Message = e.ToString();
            }
            return data;
        }
    }
}