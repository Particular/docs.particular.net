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

    public ReceiverDataContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (IsConnectionProvided())
        {
            optionsBuilder.UseSqlServer(connection);
        }
        else
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        optionsBuilder.ConfigureWarnings(
            warningsConfigurationBuilderAction: builder =>
            {
                builder.Ignore(RelationalEventId.AmbientTransactionWarning);
            });
    }

    bool IsConnectionProvided()
    {
        return connection != null;
    }

    DbConnection connection;
    string connectionString;

    public DbSet<Order> Orders { get; set; }
}

#endregion
