using BookTrackersApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookTrackersApi.DatabaseContext
{
    public class SqlServerDataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public SqlServerDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }

    }
}
