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
            // when no asynchronous code is executed in a handler Task.FromResult(0) or Task.CompletedTask can be returned
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


    #region 5to6-handler-migration-beginning
    public class MigrationBeginning : IHandleMessagesFromPreviousVersions<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            Bus.Send(new MyOtherMessage());
            Bus.Publish(new MyEvent());
        }
    }
    #endregion

    #region 5to6-handler-migration-step1
    public class MigrationStep1 : IHandleMessages<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            Bus.Send(new MyOtherMessage());
            Bus.Publish(new MyEvent());
        }

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
        }
    }
    #endregion

    #region 5to6-handler-migration-step2
    public class MigrationStep2 : IHandleMessages<MyMessage>
    {
        public IBus context { get; set; }

        public void Handle(MyMessage message)
        {
            context.Send(new MyOtherMessage());
            context.Publish(new MyEvent());
        }

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
        }
    }
    #endregion

#pragma warning disable 4014
    #region 5to6-handler-migration-step3
    public class MigrationStep3 : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // CS4014: Consider applying the 'await' operator to the result of the call.
            context.Send(new MyOtherMessage());
            context.Publish(new MyEvent());
        }
    }
    #endregion
#pragma warning restore 4014

    #region 5to6-handler-migration-step4
    public class MigrationStep4 : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            context.Send(new MyOtherMessage());
            await context.Publish(new MyEvent()).ConfigureAwait(false);
        }
    }
    #endregion

    public interface IHandleMessagesFromPreviousVersions<in TMessage>
    {
        void Handle(TMessage message);
    }

    public interface IBus
    {
        void Send(object message);
        void Publish(object message);
    }
}
