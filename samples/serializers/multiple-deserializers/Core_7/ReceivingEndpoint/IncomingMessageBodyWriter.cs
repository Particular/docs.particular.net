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
        var contentType = context.Headers[Headers.ContentType];
        log.Info($"ContentType '{contentType}'. Serialized Message Body:\r\n{bodyAsString}");
        return Task.CompletedTask;
    }
}
#endregion