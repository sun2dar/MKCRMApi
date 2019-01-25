using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class ChangeStatus
    {
        [StringLength(20)]
        public string eBankingType { get; set; }

        public int? eBankingValue { get; set; }

        [StringLength(60)]
        public string eBankingName { get; set; }

        [StringLength(20)]
        public string StatusName { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(100)]
        public string StatusLabel { get; set; }

        [StringLength(30)]
        public string NewStatus { get; set; }
    }

    public class ChangeStatusResult
    {
        public ChangeStatusResultType Status { get; set; }
        public string Result { get; set; }
        public string UpdateTime { get; set; }
        public ChangeStatusUpdatedBy UpdatedBy { get; set; }
    }

    public class ChangeStatusUpdatedBy
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public enum ChangeStatusResultType
    {
        Failure = 0,
        Success = 1,
        FailedToWriteToLog = 2
    }
}