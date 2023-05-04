using NServiceBus;
using NServiceBus.Logging;

using Messages;

namespace SagaSample;

public class OrderSaga : Saga<OrderSagaData>,
  IAmStartedByMessages<PlaceOrder>,
  IHandleMessages<CancelOrder>,
  IHandleTimeouts<OrderReceived>,
  IHandleMessages<CustomerBilled>,
  IHandleMessages<InventoryStaged>,
  IHandleMessages<OrderShipped>
{
  static ILog log = LogManager.GetLogger(typeof(OrderSagaData));

  protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
  {
    mapper.MapSaga(sagaData => sagaData.OrderId)
     .ToMessage<PlaceOrder>(s => s.OrderId)
     .ToMessage<CancelOrder>(s => s.OrderId)
     .ToMessage<InventoryStaged>(s => s.OrderId)
     .ToMessage<CustomerBilled>(s => s.OrderId)
     .ToMessage<OrderShipped>(s => s.OrderId);
  }

  public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
  {
    log.Info($"Placing order: {Data.OrderId}");

    // Delay 20 seconds to allow time for order cancellation
    await RequestTimeout<OrderReceived>(context, DateTimeOffset.UtcNow.AddSeconds(20));
  }

  public async Task Timeout(OrderReceived message, IMessageHandlerContext context)
  {
    log.Info($"Order {Data.OrderId} has been received.");

    Data.OrderProcessing = true;

    await context.Publish(new OrderReceived { OrderId = Data.OrderId });
  }

  public Task Handle(CancelOrder message, IMessageHandlerContext context)
  {
    return CancelOrder();
  }

  public async Task Handle(CustomerBilled message, IMessageHandlerContext context)
  {
    log.Info($"The customer for order {Data.OrderId} has been billed.");
    Data.CustomerBilled = true;

    if (Data.CustomerBilled && Data.InventoryStaged)
    {
      await ShipIt(context);
    }
  }

  public async Task Handle(InventoryStaged message, IMessageHandlerContext context)
  {
    log.Info($"The inventory for order {Data.OrderId} has been staged.");
    Data.InventoryStaged = true;

    if (Data.CustomerBilled && Data.InventoryStaged)
    {
      await ShipIt(context);
    }
  }

  public Task Handle(OrderShipped message, IMessageHandlerContext context)
  {
    log.Info($"{Data.OrderId} has been shipped.");

    MarkAsComplete();

    return Task.CompletedTask;
  }

  Task CancelOrder()
  {
    log.Info($"Order {Data.OrderId} has been cancelled");
    MarkAsComplete();
    return Task.CompletedTask;
  }

  async Task ShipIt(IMessageHandlerContext context)
  {
    log.Info($"Order {Data.OrderId} is ready to be shipped.");

    var messageId = Guid.NewGuid().ToString("N");

    var sendOptions = new SendOptions();
    sendOptions.SetMessageId(messageId);

    // Send duplicate message for outbox test.
    await context.Send(new ShipOrder { OrderId = Data.OrderId }, sendOptions);
    await context.Send(new ShipOrder { OrderId = Data.OrderId, IsDuplicate = true }, sendOptions);
  }
}
