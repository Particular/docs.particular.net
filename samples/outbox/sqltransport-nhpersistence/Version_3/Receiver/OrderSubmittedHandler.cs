using System;
using System.Threading.Tasks;
using NServiceBus;
using NHibernate;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static readonly Random ChaosGenerator = new Random();
    
    public ISession Session { get; set; }

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

        #region StoreUserData

        Session.Save(new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        });

        #endregion

        #region Reply

        await context.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });

        #endregion

        if (ChaosGenerator.Next(2) == 0)
        {
            throw new Exception("Boom!");
        }
    }
}