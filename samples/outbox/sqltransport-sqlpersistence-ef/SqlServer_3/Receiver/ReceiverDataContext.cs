using System.Data.Entity;
using System.Data;
using System.Data.Common;

public class ReceiverDataContext :
    DbContext
{
    #region EntityFramework
    public ReceiverDataContext(string connection)
        : base(connection)
    {
    }
    public ReceiverDataContext(IDbConnection connection)
        : base((DbConnection) connection, false)
    {
    }
    #endregion

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
