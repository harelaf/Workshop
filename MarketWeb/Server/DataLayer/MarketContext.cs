﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MarketWeb.Server.DataLayer
{
    public class MarketContext: DbContext
    {
        public DbSet<StoreDAL> StoreDALs { get; set; }
        public DbSet<RegisteredDAL> RegisteredDALs { get; set; }
        public DbSet<ComplaintDAL> ComplaintDALs { get; set; }
        public DbSet<StorePurchasedBasketDAL> StorePurchaseHistory { get; set; }
        public DbSet<RegisteredPurchasedCartDAL> RegisteredPurchaseHistory { get; set; }
        public DbSet<MessageToStoreDAL> MessageToStoreDALs { get; set; }
        public DbSet<SystemRoleDAL> SystemRoleDALs { get; set; }
        public DbSet<SystemAdminDAL> SystemAdminDALs { get; set; }
        public DbSet<StoreFounderDAL> StoreFounderDALs { get; set; }
        public DbSet<StoreManagerDAL> storeManagerDALs { get; set; }
        public DbSet<StoreOwnerDAL> storeOwnerDALs { get; set; }
        public string connectionStr { get; set; } = "Data Source=34.159.230.231;Initial Catalog=marketdb;User Id=sqlserver;Password=WorkshopSadna20a;"; //Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(connectionStr);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RegisteredDAL>();//.HasOne(e => e._cart).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<StoreDAL>().HasOne(e => e._stock).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            //builder.Entity<StoreDAL>().HasOne(e => e._discountPolicy).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            //builder.Entity<StoreDAL>().HasOne(e => e._purchasePolicy).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            //builder.Entity<StoreDAL>().HasOne(e => e._rating).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<StockDAL>().HasMany(e => e._itemAndAmount).WithOne().OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<StockItemDAL>().HasOne(x => x.item).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ItemDAL>().HasOne(x => x._rating).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<RatingDAL>().HasMany(x => x._ratings).WithOne().OnDelete(DeleteBehavior.Cascade);


            builder.Entity<ComplaintDAL>();
            builder.Entity<MessageToStoreDAL>();
            builder.Entity<StorePurchasedBasketDAL>();
            builder.Entity<RegisteredPurchasedCartDAL>();

            builder.Entity<SystemRoleDAL>().HasMany("_operationsWrappers").WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<SystemAdminDAL>().HasMany("_operationsWrappers").WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<StoreFounderDAL>().HasMany("_operationsWrappers").WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<StoreOwnerDAL>().HasMany("_operationsWrappers").WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<StoreManagerDAL>().HasMany("_operationsWrappers").WithOne().OnDelete(DeleteBehavior.ClientCascade);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}