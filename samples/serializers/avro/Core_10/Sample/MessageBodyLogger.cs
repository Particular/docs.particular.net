using System.Text;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;

// The behavior is only needed to print the serialized avro message content to the console
public class MessageBodyLogger(ILogger<MessageBodyLogger> logger) : Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var bodyAsString = Encoding.UTF8
            .GetString(context.Message.Body.ToArray());

        logger.LogInformation("Serialized Message Body:");
        logger.LogInformation(bodyAsString);

        return next();
    }
}