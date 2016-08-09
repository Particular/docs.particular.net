using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region outgoingmutator
class OutgoingMessageBodyWriter :
    IMutateOutgoingTransportMessages
{
    static ILog log = LogManager.GetLogger<OutgoingMessageBodyWriter>();

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        var bodyAsString = Encoding.UTF8
            .GetString(context.OutgoingBody);
        log.Info("Serialized Message Body:");
        log.Info(bodyAsString);
        return Task.FromResult(0);
    }
}

public static class OutgoingMessageBodyWriterHelper
{
    public static void RegisterOutgoingMessageLogger(this EndpointConfiguration endpointConfiguration)
    {
        // register the mutator so the the message on the wire is written
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<OutgoingMessageBodyWriter>(DependencyLifecycle.InstancePerCall);
            });
    }
}

#endregion