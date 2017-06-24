using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#region OrderEfContext

public class OrderEfContext : DbContext
{
    DbConnection connection;

    public DbSet<Order> OrdersEf { get; set; }

    public OrderEfContext(DbConnection connection)
    {
        this.connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.ConfigureWarnings(
            warningsBuilder =>
            {
                warningsBuilder.Ignore(RelationalEventId.AmbientTransactionWarning);
            });
        options.UseSqlServer(connection);
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.HasDefaultSchema("receiver");
    }
}

#endregion