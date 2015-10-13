namespace Snippets6.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region CreatingMessageHandler

    public class MySynchronousHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message)
        {
            //do something relevant with the message
            SomeLibrary.SomeMethod(message);
            return Task.FromResult(0);
        }
    }

    public class MyAsynchronousHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message)
        {
            //do something relevant with the message
            await SomeLibrary.SomeMethodAsync(message);
        }
    }

    #endregion


}


