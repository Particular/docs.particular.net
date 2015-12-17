namespace Snippets6.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region CreatingMessageHandler

    public class MyAsyncHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            //do something with the message data
            await SomeLibrary.SomeAsyncMethod(message.Data);
        }
    }

    #endregion
}