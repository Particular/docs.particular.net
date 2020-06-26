using System.Data.Entity;
using System.Data;
using System.Data.Common;

public class ReceiverDataContext :
    DbContext
{
    public ReceiverDataContext(IDbConnection connection)
        : base((DbConnection) connection, false)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var orders = modelBuilder.Entity<Order>();
        orders.ToTable("Orders");
        orders.HasKey(x => x.OrderId);
        orders.Property(x => x.Value);

        var shipments = modelBuilder.Entity<Shipment>();
        shipments.ToTable("Shipments");
        shipments.HasKey(x => x.Id);
        //shipments.HasKey(x => x.OrderId);
        shipments.HasRequired(x => x.Order);
    }
}