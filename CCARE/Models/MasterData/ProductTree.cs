using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models.MasterData
{
    public class ProductTree
    {
        //[Key]
        //public Guid? ProductID { get; set; }
        //public Guid? CategoryID { get; set; }
        //public string Name { get; set; }
        //public int Level { get; set; }
        //public int IsBold { get; set; }
        //public string TreeId { get; set; }
        //public int ChildCount { get; set; }

        public Guid? CategoryID { get; set; }
        public string ProductName { get; set; }
        public int IsBold { get; set; }
        public int Level { get; set; }
        public int ChildCount { get; set; }
        public string TreeID { get; set; }
        public string Name { get; set; }
        public Guid ProductID { get; set; }
        public Guid? ParentProductID { get; set; }
        [Key]
        public Guid GUID { get; set; }
    }
}