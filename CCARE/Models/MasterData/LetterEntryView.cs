using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class LetterEntryView
    {
        public string RequestIds { get; set; }

        public bool IsMultipleLetter { get; set; }

        public Annotation Annotation { get; set; }

        public LetterEntry LetterEntry { get; set; }
    }
}