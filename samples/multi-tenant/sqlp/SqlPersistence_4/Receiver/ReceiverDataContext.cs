using System.Data;
using System.Data.Common;
using System.Data.Entity;

public class ReceiverDataContext :
    DbContext
{
    public ReceiverDataContext(IDbConnection connection)
        : base((DbConnection)connection, false)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var orders = modelBuilder.Entity<Order>();
        orders.ToTable("Orders");
        orders.HasKey(x => x.OrderId);
        orders.Property(x => x.Value);
    }
}