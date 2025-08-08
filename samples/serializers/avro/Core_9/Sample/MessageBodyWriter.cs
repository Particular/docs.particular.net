using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;

#region write-body-behavior

public class MessageBodyWriter(ILogger<MessageBodyWriter> logger) : Behavior<IIncomingPhysicalMessageContext>
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

#endregion