using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

#region thesaga
public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>,
    IHandleTimeouts<CancelOrder>
{
    private readonly ILogger<OrderSaga> logger;

    public OrderSaga(ILogger<OrderSaga> logger)
    {
        this.logger = logger;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<StartOrder>(message => message.OrderId)
            .ToMessage<CompleteOrder>(message => message.OrderId);
    }

    public async Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        // Correlation property Data.OrderId is automatically assigned with the value from message.OrderId;
        logger.LogInformation("StartOrder received with OrderId {OrderId}", message.OrderId);

        logger.LogInformation("Sending a CompleteOrder that will be delayed by 10 seconds. Stop the endpoint now to see the saga data in: {SagaLocation}", LearningLocationHelper.GetSagaLocation<OrderSaga>(Data.Id));
        var completeOrder = new CompleteOrder
        {
            OrderId = Data.OrderId
        };
        var sendOptions = new SendOptions();
        sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(10));
        sendOptions.RouteToThisEndpoint();
        await context.Send(completeOrder, sendOptions);

        var timeout = DateTimeOffset.UtcNow.AddSeconds(30);
        logger.LogInformation($@"Requesting a CancelOrder that will be executed in 30 seconds.
Stop the endpoint now to see the timeout data in the delayed directory
{LearningLocationHelper.TransportDelayedDirectory(timeout)}");
        await RequestTimeout<CancelOrder>(context, timeout);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("CompleteOrder received with OrderId {OrderId}", message.OrderId);
        MarkAsComplete();
        return Task.CompletedTask;
    }

    public Task Timeout(CancelOrder state, IMessageHandlerContext context)
    {
        logger.LogInformation("CompleteOrder not received soon enough OrderId {OrderId}. Calling MarkAsComplete", Data.OrderId);
        MarkAsComplete();
        return Task.CompletedTask;
    }
}
#endregion
