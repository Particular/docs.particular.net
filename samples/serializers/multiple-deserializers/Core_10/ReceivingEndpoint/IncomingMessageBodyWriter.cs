using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.MessageMutator;

#region incomingmutator
public class IncomingMessageBodyWriter(ILogger<IncomingMessageBodyWriter> logger) :
    IMutateIncomingTransportMessages, IIncomingMessageBodyWriter
{
    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var bodyAsString = Encoding.UTF8
            .GetString(context.Body.ToArray());
        var contentType = context.Headers[Headers.ContentType];

        logger.LogInformation("ContentType '{ContentType}'. Serialized Message Body:\r\n{Body}", contentType, bodyAsString);
        return Task.CompletedTask;
    }
}
#endregion