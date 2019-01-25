using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models.Btn
{
    public class BtnWorkflow
    {
        [Key]
        public Guid ID { get; set; }

        public int Code { get; set; }

        public string Label { get; set; }

        public int Seq { get; set; }

        public int ProductType { get; set; }

        public string ProductTypeName { get; set; }
    }
}