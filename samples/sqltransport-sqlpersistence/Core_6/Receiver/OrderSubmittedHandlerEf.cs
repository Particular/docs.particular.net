using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NServiceBus;
using NServiceBus.Logging;

public class OrderEfContext : DbContext
{
    DbConnection connection;

    public DbSet<Order> OrdersEf { get; set; }

    public OrderEfContext(DbConnection connection)
    {
        this.connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
        options.UseSqlServer(connection);
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.HasDefaultSchema("receiver");
    }
}

public class OrderSubmittedHandlerEf :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} being persisted by EF");

        #region StoreUserDataEf

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var connection = (SqlConnection)session.Connection;
        var transaction = (SqlTransaction)session.Transaction;
        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };

        using (var orderContext = new OrderEfContext(session.Connection))
        {
            orderContext.Database.UseTransaction(transaction);
            await orderContext.OrdersEf.AddAsync(order)
                .ConfigureAwait(false);
            await orderContext.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        #endregion
    }

}