using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CCARE.Models.SalesMarketing
{
    public class Lead
    {
        [Key]
        public System.Guid LeadID { get; set; }
        public Nullable<System.Guid> ContactId { get; set; }
        public Nullable<System.Guid> AccountId { get; set; }
        public Nullable<System.Guid> CampaignId { get; set; }
        public Nullable<System.Guid> ProductId { get; set; }
        public string ProductName { get; set; }
        public string Salutation { get; set; }
        public string Topic { get; set; }
        //public string Company { get; set; }
        //public string JobTitle { get; set; }
        //public string Firstname { get; set; }
        //public string Lastname { get; set; }
        public string Fullname { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string MobilePhone { get; set; }
        public Nullable<int> AddressTypeCode { get; set; }
        public string AddressTypeLabel { get; set; }
        public Nullable<int> GenderCode { get; set; }
        public string GenderLabel { get; set; }
        //public Nullable<System.Guid> BusinessUnitID { get; set; }
        //public Nullable<System.Guid> OwnerID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public int StateCode { get; set; }
        public string StateLabel { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public int DeletionStateCode { get; set; }
    }
}