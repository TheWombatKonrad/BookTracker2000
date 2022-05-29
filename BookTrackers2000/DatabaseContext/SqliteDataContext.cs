namespace BookTrackersApi.DatabaseContext
{
    using Microsoft.EntityFrameworkCore;

    public class SqliteDataContext : SqlServerDataContext
    {
        public SqliteDataContext(IConfiguration configuration) : base(configuration)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
        }
    }
}
