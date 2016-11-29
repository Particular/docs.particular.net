using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class ConfirmOrderCommandHandler : 
    IHandleMessages<ConfirmOrder>
{
    static ILog log = LogManager.GetLogger<ConfirmOrderCommandHandler>();

    public Task Handle(ConfirmOrder message, IMessageHandlerContext context)
    {
        log.Info($"Confirming order {message.OrderId}");
        return Task.CompletedTask;
    }
}