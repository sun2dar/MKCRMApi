using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
    public class SessionForSP
    {
        public Guid OrganizationID { get; set; }

        public Guid BusinessUnitID { get; set; }

        public Guid CurrentUserID { get; set; }
    }
}