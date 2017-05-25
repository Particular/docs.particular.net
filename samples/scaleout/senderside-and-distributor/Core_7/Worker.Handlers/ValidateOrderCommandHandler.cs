using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class ValidateOrderCommandHandler :
    IHandleMessages<ValidateOrder>
{
    static ILog log = LogManager.GetLogger<ValidateOrderCommandHandler>();

    #region Defer

    public Task Handle(ValidateOrder message, IMessageHandlerContext context)
    {
        var validated = new OrderValidated
        {
            OrderId = message.OrderId,
            Sender = message.Sender
        };

        log.Info($"Validating order {message.OrderId}. It will take 3 seconds.");

        var options = new SendOptions();
        options.RouteToThisEndpoint();
        options.DelayDeliveryWith(TimeSpan.FromSeconds(3));
        return context.Send(validated, options);
    }

    #endregion
}