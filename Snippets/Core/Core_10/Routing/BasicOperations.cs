namespace Core.Routing;

using System.Threading.Tasks;
using NServiceBus;

class BasicOperations
{
    async Task InterfaceSend(IMessageSession messageSession)
    {
        #region InterfaceSend

        await messageSession.Send<IMyMessage>(message =>
        {
            message.SomeProperty = "Hello world";
        });

        #endregion
    }

    Task InterfaceReply(IMessageHandlerContext context)
    {
        #region InterfaceReply

        return context.Reply<IMyReply>(message =>
        {
            message.SomeProperty = "Hello world";
        });

        #endregion
    }

    Task InterfacePublish(IMessageHandlerContext context)
    {
        #region InterfacePublish

        return context.Publish<IMyEvent>(message =>
        {
            message.SomeProperty = "Hello world";
        });

        #endregion
    }

    class InterfaceMessageCreatedUpFront
    {
        IMessageHandlerContext context = null;
        IMessageSession messageSession = null;

        #region IMessageCreatorUsage

        //IMessageCreator is available via dependency injection
        async Task PublishEvent(IMessageCreator messageCreator)
        {
            var eventMessage = messageCreator.CreateInstance<IMyEvent>(message =>
            {
                message.SomeProperty = "Hello world";
            });


            await messageSession.Publish(eventMessage);

            //or if on a message handler

            await context.Publish(eventMessage);
        }

        #endregion
    }

    async Task Subscribe(IMessageSession messageSession)
    {
        #region ExplicitSubscribe

        await messageSession.Subscribe<MyEvent>();

        await messageSession.Unsubscribe<MyEvent>();

        #endregion
    }

    class MyEvent
    {
    }

    interface IMyMessage
    {
        string SomeProperty { get; set; }
    }

    interface IMyReply
    {
        string SomeProperty { get; set; }
    }

    interface IMyEvent
    {
        string SomeProperty { get; set; }
    }
}