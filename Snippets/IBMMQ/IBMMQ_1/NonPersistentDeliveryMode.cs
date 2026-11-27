using System.Threading.Tasks;
using NServiceBus;

class NonPersistentDeliveryMode
{
    public async Task RequestNonPersistent(IMessageHandlerContext context)
    {
        #region ibmmq-non-persistent-delivery-mode

        var options = new SendOptions();

        options.RouteToThisEndpoint();
        options.SetHeader(Headers.NonDurableMessage, bool.TrueString);

        await context.Send(new MyMessage(), options);

        #endregion
    }

    class MyMessage : IMessage;
}
