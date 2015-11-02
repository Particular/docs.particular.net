namespace Snippets6.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region 5to6-messagehandler
    public class UpgradeMyAsynchronousHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await SomeLibrary.SomeMethodAsync(message);
        }
    }

    public class UpgradeMySynchronousHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // when no asynchronous code is executed in a handler Task.FromResult(0) can be returned
            SomeLibrary.SomeMethod(message.Data);
            return Task.FromResult(0);
        }
    }
    #endregion

    #region 5to6-bus-send-publish
    public class SendAndPublishHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await context.SendAsync(new MyOtherMessage());
            await context.PublishAsync(new MyEvent());
        }
    }
    #endregion
}