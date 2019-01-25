﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class Cause
    {

        [Key]
        public Guid? CauseID { get; set; }

        [StringLength(200)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int DeletionStateCode { get; set; }

        public int StateCode { get; set; }

        [StringLength(100)]
        public string StateLabel { get; set; }

        public int StatusCode { get; set; }
    }
}