using System.Data;
using System.Data.Common;
using System.Data.Entity;

public class FrontendDataContext :
    DbContext
{
    public FrontendDataContext(IDbConnection connection)
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
    }
}