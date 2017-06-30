using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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
    }

    DbConnection connection;

    public DbSet<Order> Orders { get; set; }
}

#endregion
