﻿using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AnspiritConsoleUI.Data
{
    public class AnspiritContext : DbContext
    {
        public DbSet<WarOfficersModel> WarOfficers { get; set; }
        public DbSet<AnspiritAdminModel> AnspiritAdmins { get; set; }
        public DbSet<IngamePlayerDiscordLinkModel> IngamePlayerDiscordLinks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(DiscordConstants.DatabaseConnectionStringPath))
            {
                throw new FileNotFoundException("Expected a database connection string file");
            }

            var connectionString = File.ReadAllText(DiscordConstants.DatabaseConnectionStringPath);

            optionsBuilder.UseMySQL(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WarOfficersModel>().ToTable("warOfficers");
            modelBuilder.Entity<AnspiritAdminModel>().ToTable("anspiritAdmins");
            modelBuilder.Entity<IngamePlayerDiscordLinkModel>().ToTable("ingameDiscordLink");
        }
    }
}
