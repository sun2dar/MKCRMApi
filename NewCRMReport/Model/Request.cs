//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewCRMReport.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        public System.Guid RequestID { get; set; }
        public string CCQRequestID { get; set; }
        public string TicketNumber { get; set; }
        public int CustomerIdType { get; set; }
        public Nullable<bool> ForCustomer { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.Guid> NonCustomerID { get; set; }
        public string NonCustomerName { get; set; }
        public string AccountNo { get; set; }
        public string CardNo { get; set; }
        public string CustomerNo { get; set; }
        public string LoanNo { get; set; }
        public Nullable<int> ContactMethod { get; set; }
        public string ContactMethodLabel { get; set; }
        public Nullable<int> CustomerAttitude { get; set; }
        public string CustomerAttitudeLabel { get; set; }
        public string InteractionCount { get; set; }
        public Nullable<int> Location { get; set; }
        public string LocationLabel { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CompanyName { get; set; }
        public string CommunicationPhone { get; set; }
        public Nullable<System.Guid> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.Guid> WorkgroupID { get; set; }
        public string WorkgroupName { get; set; }
        public Nullable<int> ServiceLevel { get; set; }
        public Nullable<int> PriorityCode { get; set; }
        public string PriorityLabel { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public string StatusLabel { get; set; }
        public string Title { get; set; }
        public string TransactionTime { get; set; }
        public Nullable<int> TransactionTimeZone { get; set; }
        public string TransactionTimeZoneLabel { get; set; }
        public Nullable<int> Currency { get; set; }
        public string CurrencyLabel { get; set; }
        public Nullable<decimal> TransactionAmount { get; set; }
        public string TransactionAmount_txt { get; set; }
        public Nullable<System.Guid> WsIdID { get; set; }
        public string WsIdName { get; set; }
        public Nullable<int> CaseOriginCode { get; set; }
        public string CaseOriginLabel { get; set; }
        public string Lokasi { get; set; }
        public Nullable<System.Guid> BranchID { get; set; }
        public string BranchName { get; set; }
        public Nullable<System.Guid> UserBranchID { get; set; }
        public string UserBranchName { get; set; }
        public Nullable<System.Guid> CauseID { get; set; }
        public string CauseName { get; set; }
        public string Reason { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public Nullable<System.Guid> OwnerID { get; set; }
        public string OwnerName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> ClosedOn { get; set; }
        public Nullable<System.DateTime> ReopenedOn { get; set; }
        public Nullable<System.Guid> BusinessUnitID { get; set; }
        public Nullable<int> Source { get; set; }
        public Nullable<int> DeletionStateCode { get; set; }
        public int StateCode { get; set; }
        public string ReferenceNumber { get; set; }
        public string RefTicketNumber { get; set; }
        public string IncomingPhoneNumber { get; set; }
        public Nullable<int> ExtraCardType { get; set; }
        public string ExtraCardLabel { get; set; }
        public string ExtraCardNumber { get; set; }
        public string Profession { get; set; }
    }
}
