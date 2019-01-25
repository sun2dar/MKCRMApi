using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCARE.Models.MasterData
{
    public class CategoryTree
    {
        //[Key]
        //public Guid? CategoryID { get; set; }
        //public Guid? ProductID { get; set; }
        //public string Name { get; set; }
        //public int Level { get; set; }
        //public int IsBold { get; set; }
        //public string TreeId { get; set; }
        //public int ChildCount { get; set; }
        
        public Guid? ProductID { get; set; }
        public string CategoryName { get; set; }
        public int IsBold { get; set; }
        public int Level { get; set; }
        public int ChildCount { get; set; }
        public string TreeID { get; set; }
        public string Name { get; set; }
        public Guid? CategoryID { get; set; }
        public Guid? ParentCategoryID { get; set; }
        [Key]
        public Guid GUID { get; set; }
    }
}