using System.Threading.Tasks;
using NServiceBus;

class RabbitMQNonPersistentDeliveryMode
{
    public async Task RequestNonPersistent(IMessageHandlerContext context)
    {
        #region rabbitmq-non-persistent-delivery-mode
        var options = new SendOptions();

        options.RouteToThisEndpoint();
        options.UseNonPersistentDeliveryMode();

        await context.Send(new MyMessage(), options);
        #endregion
    }

    class MyMessage { }
}
