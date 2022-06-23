using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketWeb.Server.DataLayer
{
    public class MarketContext: DbContext
    {
        public DbSet<StoreDAL> StoreDALs { get; set; }
        public DbSet<BidDAL> BidDAL { get; set; }
        public DbSet<BidsOfVisitor> BidOfVisitor { get; set; }
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
        public DbSet<ItemDAL> itemDALs { get; set; }
        public static string datasource { get; set; } = "";
        public static string initialcatalog { get; set; } = "";
        public static string userid { get; set; } = "";
        public static string password { get; set; } = "";
        public string connectionStr { get; set; } = $"Data Source=34.159.230.231;Initial Catalog=marketdb;User Id=sqlserver;Password=WorkshopSadna20a;"; //Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        public static bool testMode { get; set; } = false;
        public MarketContext()
        {
        }
        public override int SaveChanges()
        {
            int result = base.SaveChanges();
            if (testMode)
            {
                List<string> tableNames = new List<string>();
                this.ChangeTracker.DetectChanges(); // Not sure we need to call this, but should be cheap enough in test and will be more reliable.
                foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
                {
                    tableNames.Add(entry.Metadata.GetTableName());
                }
                foreach (string tableName in tableNames)
                    this.Database.ExecuteSqlRaw($"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL;" +
                        $"DELETE FROM {tableName};" +
                        $"ALTER TABLE {tableName} CHECK CONSTRAINT ALL;");
            }
            return result;
        }
        private int DisposeAndGetZero()
        {
            base.Dispose();
            return 0;
        }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //string connectionStr = $"Data Source={datasource};Initial Catalog={initialcatalog};User Id={userid};Password={password}";
            //connectionStr = "Data Source=34.159.230.231;Initial Catalog=marketdb;User Id=sqlserver;Password=WorkshopSadna20a;";
            options.UseSqlServer(connectionStr);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RegisteredDAL>().HasOne(e => e._cart).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<StoreDAL>().HasMany(e => e._stock).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            //builder.Entity<StoreDAL>().HasOne(e => e._discountPolicy).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            //builder.Entity<StoreDAL>().HasOne(e => e._purchasePolicy).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            //builder.Entity<StoreDAL>().HasOne(e => e._rating).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<ItemDAL>().HasMany(x => x._rating).WithOne().OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<PurchaseDetailsDAL>().HasMany(x => x.discountListJSON).WithOne().OnDelete(DeleteBehavior.ClientCascade);


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
