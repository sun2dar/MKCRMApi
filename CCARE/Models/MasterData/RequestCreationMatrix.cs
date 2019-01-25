using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class RequestCreationMatrix
    {
        public Guid? MappingID { get; set; }

        public Guid? RequestCreationMatrixID { get; set; }

        public int EntityType { get; set; }

        public string EntityName { get; set; }

        public int StatusType { get; set; }

        public string StatusName { get; set; }

        public string PrevStatus { get; set; }

        public string PrevStatusLabel { get; set; }

        public string NewStatus { get; set; }

        public string NewStatusLabel { get; set; }

        public int RequestStatus { get; set; }

        public string RequestStatusLabel { get; set; }

        public string Summary { get; set; }

        public Guid? CategoryID { get; set; }

        public string CategoryName { get; set; }

        public Guid? ProductID { get; set; }

        public string ProductName { get; set; }

        public int StateCode { get; set; }

        public string StateLabel { get; set; }

        public string AccountCode { get; set; }

        public string AccountCodeLabel { get; set; }

        public Guid? StatusChangeID { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public string ModifiedByName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public string CreatedByName { get; set; }


        //[Key]
        //public Guid RCM_ID { get; set; }

        //public int? eBanking { get; set; }

        //[StringLength(255)]
        //public string eBankingLabel { get; set; }

        //public int? Channel { get; set; }

        //[StringLength(255)]
        //public string ChannelLabel { get; set; }

        //[StringLength(100)]
        //public string PrevStatus { get; set; }

        //[StringLength(100)]
        //public string NewStatus { get; set; }
        
        //[StringLength(255)]
        //public string RequestStatusLabel { get; set; }

        //public int? RequestStatus { get; set; }

        //public Guid? CategoryID { get; set; }

        //[StringLength(100)]
        //public string CategoryName { get; set; }

        //public Guid? ProductID { get; set; }

        //[StringLength(100)]
        //public string ProductName { get; set; }

        //[StringLength(200)]
        //public string Summary { get; set; }

        //public DateTime? ModifiedOn { get; set; }

        //[StringLength(160)]
        //public string ModifiedBy { get; set; }

        //public Guid? ModifiedBy_ID { get; set; }

        //public DateTime? CreatedOn { get; set; }

        //[StringLength(160)]
        //public string CreatedBy { get; set; }

        //public Guid? CreatedBy_ID { get; set; }

        //public int StateCode { get; set; }

        //[StringLength(255)]
        //public string StateLabel { get; set; }

        //public int? StatusCode { get; set; }

        public int DeletionStateCode { get; set; }
    }
}