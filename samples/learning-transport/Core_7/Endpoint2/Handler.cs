using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler :
    IHandleMessages<TheMessage>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public Task Handle(TheMessage message, IMessageHandlerContext context)
    {
        if (message.ThrowException)
        {
            log.Info($"Received. MessageId:{context.MessageId}. Going to throw an exception.");
            throw new Exception("The exception message.");
        }
        log.Info($"Handling MessageId:{context.MessageId}");
        log.Info("Delay 10 seconds");
        return Task.Delay(TimeSpan.FromSeconds(10));
    }
}
#endregion