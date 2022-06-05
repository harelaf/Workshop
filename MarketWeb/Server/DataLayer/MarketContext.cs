using Microsoft.EntityFrameworkCore;
using System;

namespace MarketWeb.Server.DataLayer
{
    public class MarketContext: DbContext
    {
        public DbSet<StoreDAL> StoreDALs { get; set; }
        public DbSet<RegisteredDAL> RegisteredDALs { get; set; }
        public DbSet<ComplaintDAL> ComplaintDALs { get; set; }
        public DbSet<StorePurchasedBasketDAL> StorePurchaseHistory { get; set; }
        public DbSet<RegisteredPurchasedCartDAL> RegisteredPurchaseHistory { get; set; }
        public string connectionStr { get; }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(connectionStr);
        }
    }
}
