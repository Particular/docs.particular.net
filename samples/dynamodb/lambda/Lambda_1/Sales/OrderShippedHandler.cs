using Messages;

using NServiceBus;

namespace ClientUI;

public class OrderShippedHandler : IHandleMessages<OrderShipped>
{


  public Task Handle(OrderShipped message, IMessageHandlerContext context)
  {
    throw new NotImplementedException();
  }
}
