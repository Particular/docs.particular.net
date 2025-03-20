using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.MessageMutator;

#region mutator
public class MessageBodyWriter(ILogger<MessageBodyWriter> logger) :
    IMutateIncomingTransportMessages
{
    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var bodyAsString = Encoding.UTF8
            .GetString(context.Body.ToArray());

        logger.LogInformation   ("Serialized Message Body:");
        logger.LogInformation(bodyAsString);

        return Task.CompletedTask;
    }
}
#endregion