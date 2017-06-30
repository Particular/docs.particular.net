using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFramework
{
    #region SubmittedOrderDbContext

    public class SubmittedOrderDbContext : DbContext
    {
        DbConnection connection;

        public DbSet<SubmittedOrder> SubmittedOrder { get; set; }

        public SubmittedOrderDbContext(DbConnection connection)
        {
            this.connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(connection);
        }
    }

    #endregion
}
