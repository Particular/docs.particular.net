#pragma warning disable 1998
namespace Core8.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region CreatingMessageHandler

    public class MyAsyncHandler :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // do something with the message data
        }
    }

    #endregion

}
