using Inzynierka.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Inzynierka.DAL
{
    public class ExpoesContext:DbContext
    {
        public ExpoesContext() : base("DefaultConnection")
        {

        }
        public DbSet<Expo> Expos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<HistoryW> HistoryW { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<promoter> Promoters { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

        public System.Data.Entity.DbSet<Inzynierka.Models.Invite> Invites { get; set; }

        public System.Data.Entity.DbSet<Inzynierka.Models.Event> Events { get; set; }
    }
}