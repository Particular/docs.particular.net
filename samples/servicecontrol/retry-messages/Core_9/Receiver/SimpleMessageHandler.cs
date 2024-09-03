using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class SimpleMessageHandler :
    IHandleMessages<SimpleMessage>
{
    static ILog log = LogManager.GetLogger<SimpleMessageHandler>();
    public static bool FaultMode { get; set; } = true;

    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        #region ReceiverHandler

        log.Info("Received message.");
        if (FaultMode)
        {
            throw new Exception("Simulated error.");
        }
        log.Info("Successfully processed message.");
        return Task.CompletedTask;

        #endregion
    }
}