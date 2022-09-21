using Microsoft.EntityFrameworkCore;

public class MyDataContext : DbContext
{
    public MyDataContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<MyEntity> MyEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var orders = modelBuilder.Entity<MyEntity>();
        orders.ToTable("MyEntities");
        orders.HasKey(x => x.Id);
        orders.Property(x => x.Processed);
    }
}