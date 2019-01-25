//Created by Giovanni
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCARE.Models
{
    public class Request
    {
        [ForeignKey("ObjectID")]
        public virtual ICollection<Annotation> Notes { get; set; }

        [ForeignKey("RequestID")]
        public virtual ICollection<RequestAuditLog> RequestAuditLog { get; set; }

        [Key]
        public Guid? RequestID { get; set; }

        [StringLength(100)]
        public string CCQRequestID { get; set; }

        [StringLength(100)]
        public string TicketNumber { get; set; }

        public int? CustomerIdType { get; set; }

        public Boolean? ForCustomer { get; set; }

        public Guid? CustomerID { get; set; }

        [StringLength(160)]
        public string CustomerName { get; set; }

        public Guid? NonCustomerID { get; set; }

        [StringLength(200)]
        public string NonCustomerName { get; set; }

        [StringLength(100)]
        public string AccountNo { get; set; }

        [StringLength(100)]
        public string CardNo { get; set; }

        [StringLength(100)]
        public string CustomerNo { get; set; }

        [StringLength(100)]
        public string LoanNo { get; set; }

        //public Guid? EBankingId { get; set; }

        public int? ContactMethod { get; set; }

        [StringLength(255)]
        public string ContactMethodLabel { get; set; }

        public int? CustomerAttitude { get; set; }

        [StringLength(255)]
        public string CustomerAttitudeLabel { get; set; }

        [StringLength(100)]
        public string InteractionCount { get; set; }

        public int? Location { get; set; }

        [StringLength(255)]
        public string LocationLabel { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        public string PostalCode { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string CommunicationPhone { get; set; }

        public Guid? CategoryID { get; set; }

        [StringLength(100)]
        public string CategoryName { get; set; }

        public Guid? ProductID { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; }

        public Guid? WorkgroupId { get; set; }

        [StringLength(200)]
        public string WorkgroupName { get; set; }

        public int? ServiceLevel { get; set; }

        public int? PriorityCode { get; set; }

        [StringLength(200)]
        public string PriorityLabel { get; set; } /*Added By Ardi, For Print Ticket (20151102)*/

        [StringLength(200)]
        public string CaseOriginLabel { get; set; } /*Added By Ardi, For Print Ticket (20151102)*/

        [StringLength(200)]
        public string CurrencyLabel { get; set; } /*Added By Ardi, For Print Ticket (20151102)*/

        public int? StatusCode { get; set; }

        public string StatusLabel { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(50)]
        public string TransactionTime { get; set; }

        public int? TransactionTimeZone { get; set; }

        public int? Currency { get; set; }

        public decimal? TransactionAmount { get; set; }

        [StringLength(8000)]
        public string TransactionAmount_txt { get; set; }

        public Guid? WsIdID { get; set; }

        [StringLength(100)]
        public string WsIdName { get; set; }

        public int? CaseOriginCode { get; set; }

        [StringLength(100)]
        public string Lokasi { get; set; }

        public Guid? BranchID { get; set; }

        [StringLength(100)]
        public string BranchName { get; set; }

        public Guid? UserBranchID { get; set; }

        [StringLength(100)]
        public string UserBranchName { get; set; }

        public Guid? CauseID { get; set; }

        [StringLength(100)]
        public string CauseName { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public Guid? OwnerID { get; set; }

        [StringLength(160)]
        public string OwnerName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        public DateTime? DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ClosedOn { get; set; }

        public DateTime? ReopenedOn { get; set; }

        public Guid BusinessUnitID { get; set; }

        public int? Source { get; set; }

        public int? DeletionStateCode { get; set; }

        public int? StateCode { get; set; }

        /* new requested attributes */
        public string RefTicketNumber { get; set; }

        public string IncomingPhoneNumber { get; set; }

        public int? ExtraCardType { get; set; }

        public string ExtraCardLabel { get; set; }

        public string ExtraCardNumber { get; set; }
        /* new requested attributes */

        public string Profession { get; set; }

        /*Workflow enhance field*/
        public Guid? KotaID { get; set; }
        [StringLength(100)]
        public string KotaName { get; set; }

        public Guid? SegmentationID { get; set; }
        [StringLength(200)]
        public string SegmentationName { get; set; }
        [StringLength(500)]
        public string SegmentationDesc { get; set; }

        public Guid? ServiceLevelID { get; set; }
    }
}
