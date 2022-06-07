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
        //spublic string connectionStr { get; set; } = "Data Source=34.107.116.105;Initial Catalog=bgu-se-workshop-20a;Integrated Security=False;User Id=sqlserver;Password=WorkshopSadna20a;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        //public string connectionStr { get; set; } = "Server=tcp:127.0.0.1,1443;Database=bgu-se-workshop-20a;Integrated Security=False;User Id=sqlserver;Password=WorkshopSadna20a;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        public string connectionStr { get; set; } = @"Server=(localdb)\MSSQLLocalDB;Database=TestDB;Trusted_Connection=True;";

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(connectionStr);
        }
    }
}
