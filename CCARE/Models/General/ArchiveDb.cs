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
    public partial class ArchiveDb : DbContext
    {

        public ArchiveDb()
        {
            Database.SetInitializer<ArchiveDb>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 30;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestArchive>().ToTable("ARC.Request");
            modelBuilder.Entity<LetterEntryArchive>().ToTable("ARC.LetterEntry");
            modelBuilder.Entity<TaskArchive>().ToTable("ARC.Task");
            modelBuilder.Entity<AnnotationArchive>().ToTable("ARC.Annotation");
            modelBuilder.Entity<RequestResolutionArchive>().ToTable("ARC.RequestResolution");

            
        }
        public DbSet<RequestArchive> requestarchive { get; set; }
        public DbSet<LetterEntryArchive> letterentriesarchive { get; set; }
        public DbSet<TaskArchive> taskarchive { get; set; }
        public DbSet<AnnotationArchive> annotationarchive { get; set; }
        public DbSet<RequestResolutionArchive> requestresoluitonarchive { get; set; }

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