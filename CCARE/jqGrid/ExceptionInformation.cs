﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.jqGrid
{
    // to send exceptions as json we define [HandleJsonException] attribute
    public class ExceptionInformation
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}