using System.Threading.Tasks;
using NServiceBus;

class NonPersistentDeliveryMode
{
    public async Task RequestNonPersistent(IMessageHandlerContext context)
    {
        #region ibmmq-non-persistent-delivery-mode
        var options = new SendOptions();

        options.UseNonPersistentDeliveryMode();

        await context.Send(new MyMessage(), options);
        #endregion
    }

    class MyMessage { }
}
