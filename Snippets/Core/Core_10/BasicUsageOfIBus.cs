namespace Core;

using System.Threading.Tasks;
using NServiceBus;

class BasicUsageOfIBus
{
    async Task Send(IMessageSession messageSession)
    {
        #region BasicSend

        var message = new MyMessage();
        await messageSession.Send(message);

        #endregion
    }

    #region SendFromHandler

    public class MyMessageHandler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var otherMessage = new OtherMessage();
            return context.Send(otherMessage);
        }
    }

    #endregion

    async Task SetDestination(IMessageSession messageSession)
    {
        #region BasicSendSetDestination

        var options = new SendOptions();
        options.SetDestination("MyDestination");
        await messageSession.Send(new MyMessage(), options);

        #endregion
    }

    async Task SpecificInstance(IMessageSession messageSession)
    {
        #region BasicSendSpecificInstance

        var options = new SendOptions();
        options.RouteToSpecificInstance("MyInstance");
        var message = new MyMessage();
        await messageSession.Send(message, options);

        #endregion
    }

    async Task ThisEndpoint(IMessageSession messageSession)
    {
        #region BasicSendToAnyInstance

        var options = new SendOptions();
        options.RouteToThisEndpoint();
        await messageSession.Send(new MyMessage(), options);
        // or
        await messageSession.SendLocal(new MyMessage());

        #endregion
    }

    async Task ThisInstance(IMessageSession messageSession)
    {
        #region BasicSendToThisInstance

        var options = new SendOptions();
        options.RouteToThisInstance();
        var message = new MyMessage();
        await messageSession.Send(message, options);

        #endregion
    }

    async Task SendReplyToThisInstance(IMessageSession messageSession)
    {
        #region BasicSendReplyToThisInstance

        var options = new SendOptions();
        options.RouteReplyToThisInstance();
        var message = new MyMessage();
        await messageSession.Send(message, options);

        #endregion
    }

    async Task SendReplyToAnyInstance(IMessageSession messageSession)
    {
        #region BasicSendReplyToAnyInstance

        var options = new SendOptions();
        options.RouteReplyToAnyInstance();
        var message = new MyMessage();
        await messageSession.Send(message, options);

        #endregion
    }

    async Task SendReplyTo(IMessageSession messageSession)
    {
        #region BasicSendReplyToDestination

        var options = new SendOptions();
        options.RouteReplyTo("MyDestination");
        var message = new MyMessage();
        await messageSession.Send(message, options);

        #endregion
    }

    async Task ReplySendReplyToThisInstance(IMessageHandlerContext context)
    {
        #region BasicReplyReplyToThisInstance

        var options = new ReplyOptions();
        options.RouteReplyToThisInstance();
        var myMessage = new MyMessage();
        await context.Reply(myMessage, options);

        #endregion
    }

    async Task ReplySendReplyToAnyInstance(IMessageHandlerContext context)
    {
        #region BasicReplyReplyToAnyInstance

        var options = new ReplyOptions();
        options.RouteReplyToAnyInstance();
        var myMessage = new MyMessage();
        await context.Reply(myMessage, options);

        #endregion
    }

    async Task ReplySendReplyTo(IMessageHandlerContext context)
    {
        #region BasicReplyReplyToDestination

        var options = new ReplyOptions();
        options.RouteReplyTo("MyDestination");
        var myMessage = new MyMessage();
        await context.Reply(myMessage, options);

        #endregion
    }

    public class MyMessage
    {
    }

    public class OtherMessage
    {
    }
}