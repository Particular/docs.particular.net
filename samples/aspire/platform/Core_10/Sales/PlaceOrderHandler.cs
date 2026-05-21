using Messages;
using Microsoft.Extensions.Logging;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> log) : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        // This is normally where some business logic would occur

        // Force a transient exception to demonstrate failures in telemetry data
        if (Random.Shared.Next(0, 5) == 0)
        {
            throw new Exception("Oops");
        }

        var publishOptions = new PublishOptions();
        // https://docs.particular.net/nservicebus/operations/opentelemetry#traces-span-relationships-publish-operations
        publishOptions.ContinueExistingTraceOnReceive();

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };

        return context.Publish(orderPlaced, publishOptions);
    }
}