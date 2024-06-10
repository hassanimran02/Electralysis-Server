using Microsoft.EntityFrameworkCore;
using Pleromi.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.DatabaseContext
{
    public class ElectralysisDbContext : DbContext
    {
        public ElectralysisDbContext(DbContextOptions<ElectralysisDbContext> options) : base(options)
        {
        }

        // DbSet for each entity
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<ConsumerType> ConsumerTypes { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceStatus> DeviceStatuses { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<ElectricityUsage> ElectricityUsages { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SubscriptionFeature> SubscriptionFeatures { get; set; }
        public DbSet<SubscriptionPackage> SubscriptionPackages { get; set; }
        public DbSet<SubscriptionRequest> SubscriptionRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary keys for each entity
            modelBuilder.Entity<AppConfig>().HasKey(a => a.AppConfigID);
            modelBuilder.Entity<ConsumerType>().HasKey(c => c.ConsumerTypeID);
            modelBuilder.Entity<CustomerFeedback>().HasKey(c => c.CustomerFeedbackID);
            modelBuilder.Entity<Device>().HasKey(d => d.DeviceID);
            modelBuilder.Entity<DeviceStatus>().HasKey(d => d.DeviceStatusID);
            modelBuilder.Entity<DeviceType>().HasKey(d => d.DeviceTypeID);
            modelBuilder.Entity<ElectricityUsage>().HasKey(e => e.ElectricityUsageID);
            modelBuilder.Entity<Faq>().HasKey(f => f.FaqID);
            modelBuilder.Entity<Language>().HasKey(l => l.LanguageID);
            modelBuilder.Entity<Status>().HasKey(s => s.StatusID);
            modelBuilder.Entity<SubscriptionFeature>().HasKey(s => s.SubscriptionPackageFeatureID);
            modelBuilder.Entity<SubscriptionPackage>().HasKey(s => s.SubscriptionPackageID);
            modelBuilder.Entity<SubscriptionRequest>().HasKey(s => s.SubscriptionRequestID);
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<UserSubscription>().HasKey(u => u.UserSubscriptionID);

            // Configure other entity configurations (relationships, etc.) here.
        }
    }
}
