using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CTCServer.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserContext, Migrations.Configuration>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Zone> Zones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}