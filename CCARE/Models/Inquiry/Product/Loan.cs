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
        public static LoanInfo Loan(Params request)
        {
            LoanInfo loan = new LoanInfo();
            request.WSDL = "LoanInfoDetail";

            ESBData data = EAI.RetrieveESBData(request);
            if (data != null && data.Result.Count != 0)
            {
                loan.AccountNumber = data.Result[0][LoanInquiryStatusResultKeyName.AccountNumber];
                loan.LoanNumber = data.Result[0][LoanInquiryStatusResultKeyName.LoanNo];
                loan.LoanTerm = data.Result[0][LoanInquiryStatusResultKeyName.LoanTerm];
                loan.Currency = data.Result[0][LoanInquiryStatusResultKeyName.Currency];
                loan.RemainingTerm = data.Result[0][LoanInquiryStatusResultKeyName.RemainingTerm];
                loan.LoanType = Utility.GetLoanType(data.Result[0][LoanInquiryStatusResultKeyName.LoanType]);
                loan.InterestRate = Utility.GetParsedDouble(data.Result[0][LoanInquiryStatusResultKeyName.InterestRate], false);
                loan.OriginalPrincipal = Utility.GetParsedDouble(data.Result[0][LoanInquiryStatusResultKeyName.OriginalPrincipal], false);
                loan.OutstandingPrincipal = Utility.GetParsedDouble(data.Result[0][LoanInquiryStatusResultKeyName.OutstandingPrincipal], false);
                loan.TotalUnpaidInstallment = Utility.GetParsedDouble(data.Result[0][LoanInquiryStatusResultKeyName.TotalUnpaidInstallment], false);
                loan.OpenDate = data.Result[0][LoanInquiryStatusResultKeyName.OpenDate];
                loan.DueDate = data.Result[0][LoanInquiryStatusResultKeyName.DueDate];
                loan.NextInterestRateChangeDate = Utility.ParseExact(data.Result[0][LoanInquiryStatusResultKeyName.NextInterestRateChangeDate], "MMddyyyy");
                
                /* The collection will only have the following keys, for Installment related records:
                   * Installment Date
                   * Unpaid Installment Amount
                */

                for (int i = 1; i < data.Result.Count; i++)
                {
                    string str = data.Result[i][LoanInquiryStatusResultKeyName.InstallmentDate];
                    LoanInstall installment = new LoanInstall();
                    installment.UnpaidInstallmentAmount = Utility.GetParsedDouble(data.Result[i][LoanInquiryStatusResultKeyName.UnpaidInstallmentAmount], false);
                    installment.InstallmentDate = Utility.ParseExact(data.Result[i][LoanInquiryStatusResultKeyName.InstallmentDate], "ddMMyyyy");
                    //installment.InstallmentDate = esbData.Result[i][LoanInquiryStatusResultKeyName.InstallmentDate];
                    loan.Installments.Add(installment);
                }

            }
            return loan;
        }
    }
}