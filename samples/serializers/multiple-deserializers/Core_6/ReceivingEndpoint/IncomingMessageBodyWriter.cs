using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region incomingmutator
public class IncomingMessageBodyWriter :
    IMutateIncomingTransportMessages
{
    static ILog log = LogManager.GetLogger<IncomingMessageBodyWriter>();

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var bodyAsString = Encoding.UTF8
            .GetString(context.Body);
        log.Info($"ContentType '{context.Headers[Headers.ContentType]}'. Serialized Message Body:");
        log.Info(bodyAsString);
        return Task.FromResult(0);
    }
}
#endregion