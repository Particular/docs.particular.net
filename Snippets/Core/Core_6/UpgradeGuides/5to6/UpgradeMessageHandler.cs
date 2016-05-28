namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using Handlers;

    #region 5to6-messagehandler
    public class UpgradeMyAsynchronousHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return SomeLibrary.SomeAsyncMethod(message);
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
            await context.Send(new MyOtherMessage())
                .ConfigureAwait(false);
            await context.Publish(new MyEvent())
                .ConfigureAwait(false);
        }
    }
    #endregion
}
