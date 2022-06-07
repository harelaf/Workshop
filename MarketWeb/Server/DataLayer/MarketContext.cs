using Microsoft.Data.SqlClient;
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


        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = new SqlConnectionStringBuilder()
            {
                // Remember - storing secrets in plain text is potentially unsafe. Consider using
                // something like https://cloud.google.com/secret-manager/docs/overview to help keep
                // secrets secret.
                DataSource = "34.107.116.105,1433",
                // Set Host to 'cloudsql' when deploying to App Engine Flexible environment
                UserID = "sqlserver",         // e.g. 'my-db-user'
                Password = "WorkshopSadna20a",       // e.g. 'my-db-password'
                InitialCatalog = "supple-lock-352509:europe-west3:bgu-se-workshop-20a", // e.g. 'my-database'

                // The Cloud SQL proxy provides encryption between the proxy and instance
                Encrypt = true,
            };
            connectionString.Pooling = true;
            options.UseSqlServer(connectionString.ConnectionString);
        }
    }
}
