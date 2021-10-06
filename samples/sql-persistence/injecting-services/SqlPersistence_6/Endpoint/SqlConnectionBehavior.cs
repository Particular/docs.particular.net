using Microsoft.Data.SqlClient;
using NServiceBus.Pipeline;
using NServiceBus.Transport;
using System;
using System.Threading.Tasks;

public class SqlConnectionBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        // Get the SQL connection/transaction used by SQL Transport
        var transportTransaction = context.Extensions.Get<TransportTransaction>();

        // Get the connection holder object for this message from the DI container
        var connectionHolder = context.Builder.Build<ConnectionHolder>();

        // Assign the connection/transaction for the data service to use later
        connectionHolder.Connection = transportTransaction.Get<SqlConnection>("System.Data.SqlClient.SqlConnection");
        connectionHolder.Transaction = transportTransaction.Get<SqlTransaction>("System.Data.SqlClient.SqlTransaction");

        // Invoke the next stage of the pipeline, which includes the message handler
        return next();
    }
}