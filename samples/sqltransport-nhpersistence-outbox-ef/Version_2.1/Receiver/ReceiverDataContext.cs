using System.Data.Entity;

namespace Receiver
{
    using System.Data.Entity.ModelConfiguration;

    public class ReceiverDataContext : DbContext
    {
        #region EntityFramework
        public ReceiverDataContext()
            : base("NServiceBus/Persistence")
        {
        }
        #endregion

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            EntityTypeConfiguration<Order> orders = modelBuilder.Entity<Order>();
            orders.ToTable("Orders");
            orders.HasKey(x => x.OrderId);
            orders.Property(x => x.Value);
        }
    }
}