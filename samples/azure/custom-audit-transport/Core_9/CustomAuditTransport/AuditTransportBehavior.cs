using System;
using System.Threading;
using System.Threading.Tasks;
using CustomAuditTransport;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

public class AuditTransportBehavior :
    Behavior<IAuditContext>
{
    public override async Task Invoke(IAuditContext context, Func<Task> next)
    {
        var sendOptions = new SendOptions();
        sendOptions.SetDestination(context.AuditAddress);

        var transportOperations = CreateTransportOperations(context.Message);

        // TODO: Can we get this from anywhere?
        TransportTransaction transportTransaction = null;

        await AuditViaASQFeatureStartup.AsqDispatcher.Dispatch(transportOperations, transportTransaction, context.CancellationToken);
    }

    private static TransportOperations CreateTransportOperations(OutgoingMessage message)
    {
        return new TransportOperations(new TransportOperation(message, null));
    }
}