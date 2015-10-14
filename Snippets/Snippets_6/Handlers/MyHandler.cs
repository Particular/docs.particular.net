namespace Snippets6.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region CreatingMessageHandler

    public class MyAsyncHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message)
        {
            //do something with the message data
            await SomeLibrary.SomeMethodAsync(message.Data);
        }
    }

    #endregion
}