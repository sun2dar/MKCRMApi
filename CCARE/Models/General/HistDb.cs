using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data;
using System.Configuration;
using CCARE.Models.Role;

namespace CCARE.Models.General
{
    public partial class HistDb : DbContext
    {

        public HistDb()
        {
            Database.SetInitializer<HistDb>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 30;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public static object CheckForDbNull(object value)
        {
            if (value == null || value == "")
            {
                return DBNull.Value;
            }

            return value;
        }
    }
}