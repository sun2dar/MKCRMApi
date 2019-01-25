using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.App_Function;
using CCARE.Models.Transaction;

namespace CCARE.Models
{
    public partial class CreditCardTransaction
    {
        public DateTime? EffectiveDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public double? Amount { get; set; }
        public double? Nominal { get; set; }
        public double? PreviousBalance { get; set; }
        public int? Days { get; set; }
        public string CreditCardNo { get; set; }
        public string Description { get; set; }
        public string MerchantCode { get; set; }
        public string ReferenceNumber { get; set; }
        public string Source { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionType { get; set; }

        public CreditCardTransaction()
        {
            EffectiveDate = DateTime.Now;
            PostingDate = DateTime.Now;
            TransactionDate = DateTime.Now;
            Amount = 0.0;
            Nominal = 0.0;
            PreviousBalance = 0.0;
            Days = 0;
            CreditCardNo = string.Empty;
            Description = string.Empty;
            MerchantCode = string.Empty;
            ReferenceNumber = string.Empty;
            Source = string.Empty;
            TransactionCode = string.Empty;
            TransactionType = string.Empty;
        }
    }

    public class GridCreditCardTransaction
    {
        public string CreditCardNo { get; set; }
        public string CardholderName { get; set; }
        public double? SubTotal { get; set; }
        public double? AvailableCredit { get; set; }
        public double? PreviousBalance { get; set; }
        public List<CreditCardTransaction> Trx { get; set; }

        // for history transactions

        public GridCreditCardTransaction()
        {
            this.Trx = new List<CreditCardTransaction>();
            this.SubTotal = 0.0;
            this.AvailableCredit = 0.0;
        }
    }

    public class CreditCardOutstandingTransaction
    {
        public string CustomerNo { get; set; }
        public List<CreditCardInformation> CreditCards { get; set; }
        public List<CreditCardTransaction> Transactions { get; set; }
        public string TotalAmount { get; set; }
        public List<GridCreditCardTransaction> TransactionData { get; set; }

        public CreditCardOutstandingTransaction()
        {
            this.Transactions = new List<CreditCardTransaction>();
            this.CreditCards = new List<CreditCardInformation>();
            this.TransactionData = new List<GridCreditCardTransaction>();
        }

        public void getTotalNominal()
        {
            double? total = 0.0;
            foreach (CreditCardTransaction trx in Transactions)
            {
                total += trx.Nominal;
            }
            this.TotalAmount = Formatter.FormatNumeric(total);
        }

        public void Sampling() {
            foreach (CreditCardInformation card in CreditCards)
            {
                if (!string.IsNullOrEmpty(card.CreditCardNo)) {
                    GridCreditCardTransaction data = new GridCreditCardTransaction();
                    data.CreditCardNo = card.CreditCardNo;
                    data.CardholderName = card.CardholderName;
                    data.AvailableCredit = card.AvailableCredit;

                    foreach (CreditCardTransaction trx in Transactions)
                    {
                        if (trx.CreditCardNo == card.CreditCardNo)
                        {
                            card.SubTotal += trx.Nominal;
                            data.Trx.Add(trx);
                        }
                    }
                    TransactionData.Add(data);
                }
            }
        }
    }

    public class CreditCardCurrentTransaction
    {
        public string CustomerNo { get; set; }
        public List<CreditCardInformation> CreditCards { get; set; }
        public List<CreditCardTransaction> Transactions { get; set; }
        public string TotalAmount { get; set; }
        public List<GridCreditCardTransaction> TransactionData { get; set; }

        public CreditCardCurrentTransaction()
        {
            this.Transactions = new List<CreditCardTransaction>();
            this.CreditCards = new List<CreditCardInformation>();
            this.TransactionData = new List<GridCreditCardTransaction>();
        }

        public void getTotalAmount()
        {
            double? total = 0.0;
            foreach (CreditCardTransaction trx in Transactions)
            {
                double? amt = trx.Amount;
                foreach (string x in Utility.HistoricalTransactionCodesForNegativeAmount)
                {
                    if (x.Contains(trx.TransactionCode))
                    {
                        amt = 0 - amt;
                        break;
                    }
                }
                total += amt;
            }
            this.TotalAmount = Formatter.FormatNumeric(total);
        }

        public void Sampling()
        {
            foreach (CreditCardInformation card in CreditCards)
            {
                if (!string.IsNullOrEmpty(card.CreditCardNo)) { 
                    GridCreditCardTransaction data = new GridCreditCardTransaction();
                    data.CreditCardNo = card.CreditCardNo;
                    data.CardholderName = card.CardholderName;

                    double? NegativeAmount = 0.0;
                    double? PositiveAmount = 0.0;
                
                    foreach (CreditCardTransaction trx in Transactions)
                    {
                        if (trx.CreditCardNo == card.CreditCardNo)
                        {
                            if (CCTransaction.HistoricalTransactionCodesForNegativeAmount.Contains(trx.TransactionCode))
                            {
                                NegativeAmount += trx.Amount;
                            }
                            else
                            {
                                PositiveAmount += trx.Amount;
                            }
                            data.Trx.Add(trx);
                        }
                    }

                    data.SubTotal = PositiveAmount - NegativeAmount;
                    TransactionData.Add(data);
                }
            }
        }
    }

    public class HistoricalCreditCardInformationDetail {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CustomerNo { get; set; }
        public DateTime? AccountDate { get; set; }
        public double? NewBalance { get; set; }
        public double? MinimumPayment { get; set; }
        public DateTime? MaturityDate { get; set; }
        public double? OldBalance { get; set; }
        public double? Credit { get; set; }
        public double? NewBill { get; set; }
    }

    public class CreditCardHistoricalTransaction
    {
        public HistoricalCreditCardInformationDetail Information { get; set; }
        public List<CreditCardInformation> CreditCards { get; set; }
        public List<CreditCardTransaction> Transactions { get; set; }
        public List<GridCreditCardTransaction> TransactionData { get; set; }

        public CreditCardHistoricalTransaction()
        {
            this.Information = new HistoricalCreditCardInformationDetail();
            this.Transactions = new List<CreditCardTransaction>();
            this.CreditCards = new List<CreditCardInformation>();
            this.TransactionData = new List<GridCreditCardTransaction>();
        }

        public void Sampling() {
            foreach (CreditCardInformation card in CreditCards)
            {
                if (!string.IsNullOrEmpty(card.CreditCardNo))
                {
                    GridCreditCardTransaction data = new GridCreditCardTransaction();
                    data.CreditCardNo = card.CreditCardNo;
                    data.CardholderName = card.CardholderName;

                    double? NegativeAmount = 0.0;
                    double? PositiveAmount = 0.0;
                    double? PreviousBalance = 0.0;

                    foreach (CreditCardTransaction trx in Transactions)
                    {
                        if (trx.CreditCardNo == card.CreditCardNo)
                        {
                            if (CCTransaction.HistoricalTransactionCodesForNegativeAmount.Contains(trx.TransactionCode))
                            {
                                NegativeAmount += trx.Amount;
                            }
                            else
                            {
                                PositiveAmount += trx.Amount;
                            }
                            PreviousBalance += trx.PreviousBalance;
                            data.Trx.Add(trx);
                        }
                    }
                    data.PreviousBalance = PreviousBalance;
                    data.SubTotal = PositiveAmount - NegativeAmount + PreviousBalance;
                    TransactionData.Add(data);
                }
            }
        }

        public void CompletingInformation(CreditCardRetrieveMultipleStatementDate statement)
        {
            Information.Name = statement.Name;
            Information.Address = statement.Address1 + " " + statement.Address2;
            Information.MinimumPayment = statement.MinimumPayment;
            Information.NewBalance = 0.0;
            
            foreach (CreditCardInformation card in CreditCards)
            {
                if (!string.IsNullOrEmpty(card.CreditCardNo))
                {
                    GridCreditCardTransaction data = new GridCreditCardTransaction();
                    data.CreditCardNo = card.CreditCardNo;
                    data.CardholderName = card.CardholderName;

                    double? NegativeAmount = 0.0;
                    double? PositiveAmount = 0.0;
                    double? PreviousBalance = 0.0;

                    foreach (CreditCardTransaction trx in Transactions)
                    {
                        if (trx.CreditCardNo == card.CreditCardNo)
                        {
                            if (CCTransaction.HistoricalTransactionCodesForNegativeAmount.Contains(trx.TransactionCode))
                            {
                                NegativeAmount += trx.Amount;
                            }
                            else
                            {
                                PositiveAmount += trx.Amount;
                            }
                            PreviousBalance += trx.PreviousBalance;
                            if (!"INTEREST".Equals(trx.Description)) {
                                data.Trx.Add(trx);                                   
                            }
                        }
                    }
                    data.PreviousBalance = PreviousBalance;
                    data.SubTotal = PositiveAmount - NegativeAmount + PreviousBalance;
                    TransactionData.Add(data);
                    Information.NewBalance += data.SubTotal;
                }
            }
            Information.NewBill = Information.NewBalance - (Information.OldBalance + Information.Credit);
        }
    }
}