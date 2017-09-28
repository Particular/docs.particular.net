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
        log.Info($"Serialized Message Body:\r\n{bodyAsString}");
        return Task.CompletedTask;
    }
}

public static class OutgoingMessageBodyWriterHelper
{
    public static void RegisterOutgoingMessageLogger(this EndpointConfiguration endpointConfiguration)
    {
        // register the mutator so the the message on the wire is written
        endpointConfiguration.RegisterMessageMutator(new OutgoingMessageBodyWriter());
    }
}

#endregion