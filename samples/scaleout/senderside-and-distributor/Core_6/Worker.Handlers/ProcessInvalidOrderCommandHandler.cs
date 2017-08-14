using System;
using System.Reflection;
using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class ProcessInvalidOrderCommandHandler :
    IHandleMessages<PlaceInvalidOrder>
{
    static ILog log = LogManager.GetLogger<ProcessInvalidOrderCommandHandler>();

    public Task Handle(PlaceInvalidOrder placeOrder, IMessageHandlerContext context)
    {

        #region DelayedRetry

        var attempt = GetProcessingAttempt(context);

        if (attempt < 2)
        {
            log.Info($"Processing order {placeOrder.OrderId} failed. It will be retried using delayed retries.");
            //First attempt
            throw new Exception("Unexpected failure.");
        }

        #endregion

        log.Info($"Successfully processed order {placeOrder.OrderId}.");

        var placed = new PlaceOrderResponse
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };

        return context.Reply(placed);
    }

    static int GetProcessingAttempt(IMessageHandlerContext context)
    {
        if (context.MessageHeaders.TryGetValue("NServiceBus.Retries", out var attemptString))
        {
            return int.Parse(attemptString);
        }
        return 0;
    }
}
