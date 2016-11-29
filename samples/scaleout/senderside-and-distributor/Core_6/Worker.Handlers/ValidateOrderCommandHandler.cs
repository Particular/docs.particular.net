using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class ValidateOrderCommandHandler : 
    IHandleMessages<ValidateOrder>
{
    static ILog log = LogManager.GetLogger<ValidateOrderCommandHandler>();

    public async Task Handle(ValidateOrder message, IMessageHandlerContext context)
    {
        
        var validated = new OrderValidated
        {
            OrderId = message.OrderId,
            Sender = message.Sender
        };

        log.Info($"Validating order {message.OrderId}. It will take 3 seconds.");

        #region Defer

        var options = new SendOptions();
        options.RouteToThisEndpoint();
        options.DelayDeliveryWith(TimeSpan.FromSeconds(3));
        await context.Send(validated, options).ConfigureAwait(false);

        #endregion

    }
}