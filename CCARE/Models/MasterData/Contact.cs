﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.MasterData
{
    public class Contact
    {
        [Key]
        public System.Guid ContactID { get; set; }
        public Nullable<int> CustomerTypeCode { get; set; }
        public string CustomerTypeLabel { get; set; }
        public string FullName { get; set; }
        public string Salutation { get; set; }
        public Nullable<int> GenderCode { get; set; }
        public string GenderLabel { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string MobilePhone { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Fax { get; set; }
        public string EMailAddress1 { get; set; }
        public Nullable<int> AddressTypeCode { get; set; }
        public string AddressTypeLabel { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string PostalCode { get; set; }
        public string StateOrProvince { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.Guid> BusinessUnit { get; set; }
        public int StateCode { get; set; }
        public string StateLabel { get; set; }
        public int StatusCode { get; set; }
        public int DeletionStateCode { get; set; }
    }
}