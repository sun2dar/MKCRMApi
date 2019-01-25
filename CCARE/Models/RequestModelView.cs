//Created by Giovanni
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class RequestModelView
    {

        public Guid StringMapID { get; set; }

        public string EntityName { get; set; }

        public string AttributeName { get; set; }

        public int AttributeValue { get; set; }

        public string label { get; set; }

        public int DisplayOrder { get; set; }

        [Key]
        public Guid? IncidentId { get; set; }

        public string TicketNumber { get; set; }

        public string Bca_accountno { get; set; }

        public string Bca_CardNo { get; set; }

        public string BCA_LoanIdName { get; set; }

        public string BCA_LoanAccountNo { get; set; }

        public string Bca_CustomerNo { get; set; }

        public string Title { get; set; }

        public string Bca_CCQRequestID { get; set; }

        public string BCA_CategoryIdName { get; set; }

        public string BCA_ProductIdName { get; set; }

        public int? StatusCode { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? BCA_UserBranchId { get; set; }

        public string BCA_UserBranchIdName { get; set; }

        public Guid? bca_creditcardid { get; set; }

        public Guid? bca_depositid { get; set; }

        public Guid? bca_loanid { get; set; }
    }
}