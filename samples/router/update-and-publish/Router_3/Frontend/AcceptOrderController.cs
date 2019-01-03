using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Logging;
using NServiceBus.Transport;

public class AcceptOrderController :
    Controller
{
    static ILog log = LogManager.GetLogger<AcceptOrderController>();

    IMessageSession messageSession;
    Func<SqlConnection> connectionFactory;

    public AcceptOrderController(IMessageSession messageSession, Func<SqlConnection> connectionFactory)
    {
        this.messageSession = messageSession;
        this.connectionFactory = connectionFactory;
    }


    #region MessageSessionUsage
    [HttpPost]
    public async Task<string> Post([FromBody] string orderId)
    {
        using (var connection = connectionFactory())
        {
            await connection.OpenAsync().ConfigureAwait(false);
            using (var transaction = connection.BeginTransaction())
            {
                var dataContext = new FrontendDataContext(connection);
                dataContext.Database.UseTransaction(transaction);

                dataContext.Orders.Add(new Order
                {
                    OrderId = orderId
                });

                var message = new OrderAccepted
                {
                    OrderId = orderId
                };
                var options = new PublishOptions();
                var transportTransaction = new TransportTransaction();
                transportTransaction.Set(connection);
                transportTransaction.Set(transaction);
                options.GetExtensions().Set(transportTransaction);
                await messageSession.Publish(message).ConfigureAwait(false);

                await dataContext.SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();
            }
        }

        log.Info($"Order {orderId} accepted.");
        return $"Order {orderId} accepted.";

    }
    #endregion
}
