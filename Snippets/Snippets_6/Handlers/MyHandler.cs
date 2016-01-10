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

    #region EmptyHandler

    public class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // do something in the client process
            return Task.FromResult(0);
        }
    }

    #endregion
}