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
        public string connectionStr { get; set; }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            connectionStr = "Data Source = 34.107.116.105;Initial Catalog=supple-lock-352509:europe-west3:bgu-se-workshop-20a; Integrated Security = False; User Id = sqlserver; Password = WorkshopSadna20a; Encrypt = True; TrustServerCertificate = True; MultipleActiveResultSets = True";
            options.UseSqlServer(connectionStr);
        }
    }
}
