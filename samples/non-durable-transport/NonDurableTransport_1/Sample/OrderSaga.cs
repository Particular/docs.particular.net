using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region thesaga
public class OrderSaga(ILogger<OrderSaga> logger) :
    Saga<OrderSagaData>,
    IAmStartedByMessages<PlaceOrder>,
    IHandleMessages<PaymentCompleted>,
    IHandleTimeouts<CancelOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId)
            .ToMessage<PaymentCompleted>(message => message.OrderId);
    }

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("PlaceOrder received with OrderId {MessageOrderId}", message.OrderId);

        var processPayment = new ProcessPayment
        {
            OrderId = Data.OrderId
        };

        logger.LogInformation("Sending ProcessPayment to PaymentEndpoint");
        await context.Send(processPayment);

        var timeout = DateTimeOffset.UtcNow.AddSeconds(30);
        logger.LogInformation("Requesting CancelOrder timeout in 30 seconds");
        await RequestTimeout<CancelOrder>(context, timeout);
    }

    public Task Handle(PaymentCompleted message, IMessageHandlerContext context)
    {
        logger.LogInformation("PaymentCompleted received with OrderId {MessageOrderId}. Completing saga.", message.OrderId);
        MarkAsComplete();
        return Task.CompletedTask;
    }

    public Task Timeout(CancelOrder state, IMessageHandlerContext context)
    {
        logger.LogInformation("CancelOrder timeout fired for OrderId {DataOrderId}. Completing saga.", Data.OrderId);
        MarkAsComplete();
        return Task.CompletedTask;
    }
}
#endregion
