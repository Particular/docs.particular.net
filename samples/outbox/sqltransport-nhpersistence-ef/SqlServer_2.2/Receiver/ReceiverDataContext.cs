using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;

#region EntityFramework

public class ReceiverDataContext :
    DbContext
{
    public ReceiverDataContext(DbConnection connection)
    {
        this.connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connection);
        optionsBuilder.ConfigureWarnings(
            warningsConfigurationBuilderAction: builder =>
            {
                builder.Ignore(RelationalEventId.AmbientTransactionWarning);
            });
    }

    DbConnection connection;

    public DbSet<Order> Orders { get; set; }
}

#endregion
