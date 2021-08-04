using FingerprintAttendance.Models;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace FingerprintAttendance.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\FingerprintAttendance.mdf;Initial Catalog=FingerprintAttendance;Integrated Security=True"providerName="System.Data.SqlClient

        public ApplicationDbContext() : base("Server=(localdb)\\mssqllocaldb;Database=FingerprintAttendance;Trusted_Connection=True;MultipleActiveResultSets=true")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<FingerPrint> FingerPrints { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
