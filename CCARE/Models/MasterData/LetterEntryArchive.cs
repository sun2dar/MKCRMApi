/* Model created by Ardi */
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
    public class LetterEntryArchive
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ArchivedOn { get; set; }

        [Key]
        public Guid LetterEntryID { get; set; }

        //[StringLength(200)]
        //public string RequestName { get; set; }

        public Guid? RequestID { get; set; }

        [StringLength(100)]
        public string TicketNumber { get; set; }

        [StringLength(100)]
        public string TemplateName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? LetterDate { get; set; }

        [StringLength(100)]
        public string LetterNo { get; set; }

     
        [StringLength(100)]
        public string CC_Name { get; set; }

        //[StringLength(160)]
        //public string OwnerName { get; set; }

        //public Guid? OwnerID { get; set; }

        public bool? AttchATM { get; set; }

        public bool? AttchDebit { get; set; }

        public bool? AttchCirrus { get; set; }

        public bool? AttchATMSwitching { get; set; }

        public bool? AttchMaestro { get; set; }

        public int? AttchDuration { get; set; }

        [StringLength(100)]
        public string Description1 { get; set; }

        [StringLength(100)]
        public string Description2 { get; set; }

        [StringLength(100)]
        public string Description3 { get; set; }

        [StringLength(100)]
        public string Description4 { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string AccountNo { get; set; }

        [StringLength(100)]
        public string CardNo { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(100)]
        public string Address3 { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string ZIPPostalCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? ModifiedOn { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        //public Guid? BusinessUnitID { get; set; }

        //public int? Source { get; set; }

        //public int? DeletionStateCode { get; set; }

        //public int StateCode { get; set; }

        //public int? StatusCode { get; set; }
    }
}