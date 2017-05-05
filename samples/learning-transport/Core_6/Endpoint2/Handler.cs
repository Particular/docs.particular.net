using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler :
    IHandleMessages<TheMessage>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public async Task Handle(TheMessage message, IMessageHandlerContext context)
    {
        if (message.ThrowException)
        {
        log.Info($"Received. MessageId:{context.MessageId}. Going to throw an exception.");
            throw new Exception("The exception message.");
        }
        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
        log.Info($"Finished handling. MessageId:{context.MessageId}");
    }
}
#endregion