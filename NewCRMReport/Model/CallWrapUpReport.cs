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
    
    public partial class CallWrapUpReport
    {
        public System.Guid CallWrapUpID { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.Guid> NonCustomerID { get; set; }
        public string NonCustomerName { get; set; }
        public Nullable<System.DateTime> CallEndTime { get; set; }
        public Nullable<System.DateTime> CallStartTime { get; set; }
        public Nullable<int> Source { get; set; }
        public string SourceDesc { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedByName { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<int> DeletionStateCode { get; set; }
        public int StateCode { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public string CategoryName { get; set; }
        public string HandphoneNo { get; set; }
        public string PhoneNo { get; set; }
    }
}
