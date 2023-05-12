using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region OrderSaga

public class OrderSaga : Saga<OrderSagaData>,
  IAmStartedByMessages<PlaceOrder>,
  IHandleMessages<CustomerBilled>,
  IHandleMessages<InventoryStaged>
{
  static ILog log = LogManager.GetLogger(typeof(OrderSagaData));

  protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
  {
    mapper.MapSaga(sagaData => sagaData.OrderId)
     .ToMessage<PlaceOrder>(s => s.OrderId)
     .ToMessage<CustomerBilled>(s => s.OrderId)
     .ToMessage<InventoryStaged>(s => s.OrderId);
  }

  public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
  {
    log.Info($"Placing order: {Data.OrderId}");

    await context.Publish(new OrderReceived()
    {
      OrderId = message.OrderId
    });
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

  async Task ShipIt(IMessageHandlerContext context)
  {
    log.Info($"Order {Data.OrderId} has been shipped.");

    // Send duplicate message for outbox test.
    await context.Publish(new OrderShipped { OrderId = Data.OrderId });
    MarkAsComplete();
  }
}


#endregion