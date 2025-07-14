using Microsoft.Data.SqlClient;
using NServiceBus.Pipeline;
using NServiceBus.Transport;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint;

#region Behavior
public class SqlConnectionBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        // Get the SQL connection/transaction used by SQL Transport
        var transportTx = context.Extensions.Get<TransportTransaction>();

        // Get this message's ConnectionHolder from the DI container
        var connectionHolder = context.Builder.GetRequiredService<ConnectionHolder>();

        // Assign the connection/transaction for the data service to use later
        connectionHolder.Connection = transportTx.Get<SqlConnection>(
            "System.Data.SqlClient.SqlConnection");

        connectionHolder.Transaction = transportTx.Get<SqlTransaction>(
            "System.Data.SqlClient.SqlTransaction");

        // Invoke the next stage of the pipeline,
        // which includes the message handler
        return next();
    }
}
#endregion