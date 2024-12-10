using System.Data.Common;
using Microsoft.EntityFrameworkCore;

public class ReceiverDataContext :
    DbContext
{
    DbConnection connection;

    public ReceiverDataContext(DbConnection connection)
    {
        this.connection = connection;
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var orders = modelBuilder.Entity<Order>();
        orders.ToTable("Orders");
        orders.HasKey(x => x.OrderId);
        orders.Property(x => x.Value);
    }
}