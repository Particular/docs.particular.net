using Microsoft.EntityFrameworkCore;

public class ReceiverDataContext(DbContextOptions options) :
    DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var orders = modelBuilder.Entity<Order>();
        orders.ToTable("Orders");
        orders.HasKey(x => x.OrderId);
        orders.Property(x => x.Value);

        var shipments = modelBuilder.Entity<Shipment>();
        shipments.ToTable("Shipments");
        shipments.HasKey(x => x.Id);
    }
}
