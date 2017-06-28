using Microsoft.EntityFrameworkCore;
using System.Data.Common;

public class ReceiverDataContext :
    DbContext
{
    #region EntityFramework
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
        if (IsConnectionProvided(connection))
        {
            optionsBuilder.UseSqlServer(connection);
        }
        else
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    static bool IsConnectionProvided(DbConnection connection)
    {
        return connection != null;
    }

    DbConnection connection;
    string connectionString;
    #endregion

    public DbSet<Order> Orders { get; set; }
}
